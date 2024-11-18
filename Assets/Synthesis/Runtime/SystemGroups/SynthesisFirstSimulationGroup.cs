using Unity.Entities;

namespace Synthesis.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class SynthesisFirstSimulationGroup : ComponentSystemGroup
    {
        
    }
}