using Unity.Entities;

namespace Chef.EntitiesEvents
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [CreateBefore(typeof(SimulationSystemGroup))]
    public sealed partial class EventSystemGroup : ComponentSystemGroup { }
}