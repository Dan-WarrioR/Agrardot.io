using Data;
using EditorAttributes;
using Features;
using Features.Spawn;
using Core.SceneManagement;
using Tools;
using Unity.Entities;

namespace Core.HSM.States
{
    public class GameState : BaseState
    {
        private const string GameSceneName = "Game";
        
        protected override void OnDispose()
        {
            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var gameQuery = manager.CreateEntityQuery(typeof(GameEntityTag));
            manager.DestroyEntity(gameQuery);
            
            SceneLoader.UnloadScene(GameSceneName);
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(false);
        }
        
        protected override void OnInitialize()
        {
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(true);                 

            SceneLoader.LoadScene(GameSceneName);
            var globalConfig = Dependency.Get<GlobalConfig>();
            SpawnEntities(globalConfig);
        }

        private void SpawnEntities(GlobalConfig globalConfig)
        {
            SpawnEntity(new SpawnRequest
            {
                type = SpawnRequestType.Food,
                count = globalConfig.foodCount,
            });
            
            if (globalConfig.playerCount <= 0)
            {
                return;
            }
            
            SpawnEntity(new SpawnRequest
            {
                type = SpawnRequestType.Player,
                count = globalConfig.playerCount - 1,
            });
            SpawnEntity(new SpawnRequest
            {
                type = SpawnRequestType.User,
                count = 1,
            });
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