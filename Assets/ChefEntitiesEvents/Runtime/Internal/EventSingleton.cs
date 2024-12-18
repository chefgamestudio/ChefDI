using Unity.Entities;

namespace Chef.EntitiesEvents.Internal
{
    public unsafe struct EventSingleton<T> : IComponentData
        where T : unmanaged
    {
        public Events<T> events;
    }
}