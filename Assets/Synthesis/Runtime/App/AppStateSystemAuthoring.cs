using Synthesis.Core;
using Unity.Entities;
using UnityEngine;

namespace Synthesis.App
{
    public class AppStateSystemAuthoring : AbsEnabledSystemAuthoring
    {
        public struct SystemIsEnabledTag : IComponentData {}
        
        private class AppStateSystemAuthoringBaker : Baker<AppStateSystemAuthoring>
        {
            
            
            public override void Bake(AppStateSystemAuthoring authoring)
            {
                if(authoring._isSystemEnabled)
                {
                    Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                    
                    //Turn system on
                    AddComponent<SystemIsEnabledTag>(entity);
                    
                    //Create first component with default values
                    AddComponent(entity,
                        new AppStateComponent
                        {
                            AppState = AppState.None,
                            IsLevelPaused = false,
                            IsLevelOver = false
                        });
                    
                }
            }
        }
    }
    
    public partial struct AppStateComponent : IComponentData
    {
        public AppState AppState;
        public bool IsLevelPaused;
        public bool IsLevelOver;
    }
}