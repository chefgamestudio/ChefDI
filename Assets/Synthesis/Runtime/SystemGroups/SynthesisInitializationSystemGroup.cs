using Unity.Entities;

namespace Synthesis.SystemGroups
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class SynthesisInitializationSystemGroup : ComponentSystemGroup
    {
        
    }
}