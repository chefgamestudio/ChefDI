using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using gs.ChefDI.Unity;

namespace gs.ChefDI.Tests.Unity
{
    [TestFixture]
    public class UnityObjectResolverTest
    {
        [Test]
        public void InjectGameObject()
        {
            var builder = new ContainerBuilder();
            builder.Register<ServiceA>(Lifetime.Singleton);
            var container = builder.Build();

            var gameObject = new GameObject();
            var component = gameObject.AddComponent<SampleMonoBehaviour>();

            Assert.That(component.ServiceA, Is.Null);

            container.InjectGameObject(gameObject);
            Assert.That(component.ServiceA, Is.InstanceOf<ServiceA>());
        }

        [Test]
        public void InjectGameObjectAllMonoBehaviour()
        {
            var builder = new ContainerBuilder();
            builder.Register<ServiceA>(Lifetime.Singleton);
            builder.Register<ServiceB>(Lifetime.Singleton);
            var container = builder.Build();

            var gameObject = new GameObject();
            var component1 = gameObject.AddComponent<SampleMonoBehaviour>();
            var component2 = gameObject.AddComponent<SampleMonoBehaviour2>();

            Assert.That(component1.ServiceA, Is.Null);
            Assert.That(component2.ServiceB, Is.Null);

            container.InjectGameObject(gameObject);
            Assert.That(component1.ServiceA, Is.InstanceOf<ServiceA>());
            Assert.That(component2.ServiceB, Is.InstanceOf<ServiceB>());
        }

        [Test]
        public void InjectGameObjectWithChildren()
        {
            var builder = new ContainerBuilder();
            builder.Register<ServiceA>(Lifetime.Singleton);
            builder.Register<ServiceB>(Lifetime.Singleton);
            var container = builder.Build();

            var gameObject = new GameObject("Parent");
            var component1 = gameObject.AddComponent<SampleMonoBehaviour>();
            var component2 = gameObject.AddComponent<SampleMonoBehaviour2>();

            var child1 = new GameObject("Child 1");
            child1.transform.SetParent(gameObject.transform);

            var child2 = new GameObject("Child 2");
            child2.transform.SetParent(child1.transform);
            var child2Component = child2.AddComponent<SampleMonoBehaviour>();

            container.InjectGameObject(gameObject);
            Assert.That(component1.ServiceA, Is.InstanceOf<ServiceA>());
            Assert.That(component2.ServiceB, Is.InstanceOf<ServiceB>());
            Assert.That(child2Component.ServiceA, Is.InstanceOf<ServiceA>());
        }

        [Test]
        public void InstantiateMonoBehaviour()
        {
            var builder = new ContainerBuilder();
            builder.Register<ServiceA>(Lifetime.Singleton);
            builder.Register<ServiceB>(Lifetime.Singleton);
            var container = builder.Build();

            var parent = new GameObject("Parent");
            var original = new GameObject("Original").AddComponent<SampleMonoBehaviour>();
            original.gameObject.AddComponent<SampleMonoBehaviour2>();

            var instance1 = container.Instantiate(original);
            Assert.That(instance1, Is.Not.EqualTo(original));
            Assert.That(instance1.ServiceA, Is.InstanceOf<ServiceA>());
            Assert.That(instance1.GetComponent<SampleMonoBehaviour2>().ServiceB, Is.InstanceOf<ServiceB>());


            var instance2 = container.Instantiate(original, parent.transform);
            Assert.That(parent.GetComponentInChildren<SampleMonoBehaviour>(), Is.EqualTo(instance2));
            Assert.That(instance2, Is.Not.EqualTo(original));
            Assert.That(instance2.ServiceA, Is.InstanceOf<ServiceA>());
            Assert.That(instance2.GetComponent<SampleMonoBehaviour2>().ServiceB, Is.InstanceOf<ServiceB>());


            var instance3 = container.Instantiate(
                original,
                new Vector3(1f, 2f, 3f),
                Quaternion.Euler(1f, 2f, 3f));

            Assert.That(instance3, Is.Not.EqualTo(original));
            Assert.That(instance3.ServiceA, Is.InstanceOf<ServiceA>());
            Assert.That(instance3.GetComponent<SampleMonoBehaviour2>().ServiceB, Is.InstanceOf<ServiceB>());
            Assert.That(instance3.transform.position, Is.EqualTo(new Vector3(1f, 2f, 3f)));
            Assert.That(instance3.transform.rotation, Is.EqualTo(Quaternion.Euler(1f, 2f, 3f)));
        }


        [Test]
        public void InstantiateGameObject()
        {
            var builder = new ContainerBuilder();
            builder.Register<ServiceA>(Lifetime.Singleton);
            var container = builder.Build();

            var parent = new GameObject("Parent");
            var original = new GameObject("Original").AddComponent<SampleMonoBehaviour>();

            var instance1 = container.Instantiate(original);
            AssertInstantiatedInstance(instance1);

            var instance2 = container.Instantiate(original, parent.transform);
            Assert.That(parent.transform.GetChild(0), Is.EqualTo(instance2.transform));
            AssertInstantiatedInstance(instance2);

            var instance3 = container.Instantiate(
                original,
                new Vector3(1f, 2f, 3f),
                Quaternion.Euler(1f, 2f, 3f));

            AssertInstantiatedInstance(instance3);

            Assert.That(instance3.transform.position, Is.EqualTo(new Vector3(1f, 2f, 3f)));
            Assert.That(instance3.transform.rotation, Is.EqualTo(Quaternion.Euler(1f, 2f, 3f)));

            void AssertInstantiatedInstance(SampleMonoBehaviour instance)
            {
                Assert.That(instance, Is.Not.EqualTo(original));
                Assert.That(instance.ServiceAInAwake, Is.InstanceOf<ServiceA>());
                Assert.That(instance.ServiceA, Is.InstanceOf<ServiceA>());
            }
        }
        
        [Test]
        public void WhenFailingDuringInstantiateGameObject_TheGameObjectIsStillEnabled()
        {
            var builder = new ContainerBuilder();
            builder.Register<ServiceA>(Lifetime.Singleton);
            var container = builder.Build();

            var parent = new GameObject("Parent");
            GameObject original = new GameObject("Original");
            original.AddComponent<CrashingSampleMonoBehaviour>();

            Assert.Catch(() => container.Instantiate(original, parent.transform));
            
            Assert.That(original.gameObject.activeSelf, Is.True);
        }

        [Test]
        public void Instantiate_ExceptionThrownDuringInjection_PrefabIsStillEnabled()
        {
            var builder = new ContainerBuilder();

            var original = new GameObject("Original").AddComponent<SampleMonoBehaviour>();

            //Throws in resolving
            builder.Register<ServiceA>(_ => throw new Exception(), Lifetime.Singleton);

            var container = builder.Build();

            Assert.Throws<Exception>(() => container.Instantiate(original));
            Assert.That(original.gameObject.activeSelf, Is.True);
        }

        [Test]
        public void ReenterInjectGameObject()
        {
            var builder = new ContainerBuilder();
            builder.Register<ServiceA>(Lifetime.Singleton);
            var container = builder.Build();

            var parent = new GameObject("Parent");
            var original = new GameObject("Original");

            var behaviour = original.AddComponent<SampleMonoBehaviour3>();
            behaviour.Prefab = new GameObject("Nested");

            var instance1 = container.Instantiate(original);
            Assert.That(instance1, Is.Not.EqualTo(original));
            Assert.That(instance1.GetComponent<SampleMonoBehaviour3>(), Is.InstanceOf<SampleMonoBehaviour3>());
        }
    }
}