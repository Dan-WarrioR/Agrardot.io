using Data;
using EditorAttributes;
using Features;
using Features.Spawn;
using Core.SceneManagement;
using Data.Configs;
using Tools;
using Unity.Entities;

namespace Core.HSM.States
{
    public class GameState : BaseState
    {
        private const string GameSceneName = "Game";
        
        protected override void OnDispose()
        {
            SceneLoader.UnloadScene(GameSceneName);
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(false);
        }
        
        protected override void OnInitialize()
        {
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(true);                 

            SceneLoader.LoadScene(GameSceneName);
        }

        private void SpawnEntities(GlobalConfig globalConfig)
        {
            return;
            DotsExtensions.CreateBufferRequest(new SpawnRequestComponent
            {
                type = SpawnRequestType.Food,
                count = 10,
            });
            DotsExtensions.CreateBufferRequest(new SpawnRequestComponent
            {
                type = SpawnRequestType.User,
                count = 1,
            });
            
            
            //
            // if (globalConfigs.playerCount <= 0)
            // {
            //     return;
            // }
            //
            // SpawnEntity(new SpawnRequestComponent
            // {
            //     type = SpawnRequestType.Player,
            //     count = globalConfigs.playerCount - 1,
            // });
            // SpawnEntity(new SpawnRequestComponent
            // {
            //     type = SpawnRequestType.User,
            //     count = 1,
            // });
        }

        private void SpawnEntity<T>(T request) where T : unmanaged, IComponentData
        {
            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entity = manager.CreateEntity();
            manager.AddComponentData(entity, request);
        }

        [Button]
        private void ToMenu()
        {
            StateMachine.SetState<MainMenuState>();
        }
    }
}