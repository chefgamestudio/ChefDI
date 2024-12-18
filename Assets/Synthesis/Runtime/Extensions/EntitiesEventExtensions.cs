using System.Runtime.CompilerServices;
using Chef.EntitiesEvents;
using gs.ChefDI;
using Unity.Entities;

namespace Synthesis.Extensions
{
    public static class EntitiesEventExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterEntityEvent<T>(this IContainerBuilder builder, Lifetime scope) where T : unmanaged
        {
            builder.RegisterFactory<EventWriter<T>>(resolver =>
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                return () => entityManager.GetEventWriter<T>();
            }, scope);

            builder.RegisterFactory<EventReader<T>>(resolver =>
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                return () => entityManager.GetEventReader<T>();
            }, scope);
        }
    }
}