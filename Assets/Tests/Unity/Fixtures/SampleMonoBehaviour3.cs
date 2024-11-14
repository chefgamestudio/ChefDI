using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gs.ChefDI;
using gs.ChefDI.Unity;

namespace gs.ChefDI.Tests.Unity
{
    public class SampleMonoBehaviour3 : MonoBehaviour
    {
        public GameObject Prefab;
        [Inject]
        public void Construct(IObjectResolver container)
        {
            var go = Object.Instantiate(Prefab);
            container.InjectGameObject(go);
        }
    }
}
