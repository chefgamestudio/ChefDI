using Unity.Entities;
using Unity.Scenes;

namespace Synthesis.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(SceneSystemGroup))]
    public partial class SynthesisLastSimulationGroup : ComponentSystemGroup
    {
        
    }
}