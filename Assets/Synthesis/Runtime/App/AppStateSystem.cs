using Chef.EntitiesEvents;
using gs.ChefDI;
using Synthesis.SystemGroups;
using MessagePipe;
using Unity.Entities;

namespace Synthesis.App
{
    [UpdateInGroup(typeof(SynthesisInitializationSystemGroup), OrderFirst = true)]
    public partial class AppStateSystem : SystemBase
    {
        [Inject] private readonly IPublisher<CurrentAppStateEvent> _appStateComponentPublisher;
        private EventReader<OnChangeAppStateEvent> _onChangeAppStateEventReader;

        protected override void OnCreate()
        {
            RequireForUpdate<AppStateSystemAuthoring.SystemIsEnabledTag>();
            _onChangeAppStateEventReader = this.GetEventReader<OnChangeAppStateEvent>();
        }

        protected override void OnUpdate()
        {
            foreach (var eventData in _onChangeAppStateEventReader.Read())
            {
                AppState = eventData.AppState;
                _appStateComponentPublisher.Publish(new CurrentAppStateEvent
                {
                    AppState = AppState,
                });
            }
        }

        public AppState AppState
        {
            get
            {
                if (!SystemAPI.HasSingleton<AppStateComponent>())
                {
                    return AppState.None;
                }

                return SystemAPI.GetSingleton<AppStateComponent>().AppState;
            }
            set
            {
                if (SystemAPI.HasSingleton<AppStateComponent>())
                {
                    var appStateComponent = SystemAPI.GetSingleton<AppStateComponent>();

                    SystemAPI.SetSingleton<AppStateComponent>(new AppStateComponent
                    {
                        AppState = value,
                        IsLevelPaused = appStateComponent.IsLevelPaused,
                        IsLevelOver = appStateComponent.IsLevelOver
                    });
                }
            }
        }
    }
}