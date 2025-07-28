using Data;
using Features;
using Features.Spawn;
using Tools;
using Unity.Entities;

namespace Core.HSM.States
{
    public class GameState : BaseState
    {
        protected override void OnInitialize()
        {
            var globalConfig = Dependency.Get<GlobalConfig>();
            SpawnEntities(globalConfig);
        }

        protected override void OnActivate()
        {
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(true);                 
        }

        protected override void OnDeactivate()
        {
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(false);
        }

        private void SpawnEntities(GlobalConfig globalConfig)
        {
            SpawnEntity(new SpawnRequest
            {
                type = SpawnRequestType.Food,
                count = globalConfig.foodCount,
            });
            SpawnEntity(new SpawnRequest
            {
                type = SpawnRequestType.PlayerUser,
                count = 1,
            });
            SpawnEntity(new SpawnRequest
            {
                type = SpawnRequestType.PlayerBot,
                count = globalConfig.playerCount - 1,
            });
        }

        private void SpawnEntity<T>(T request) where T : unmanaged, IComponentData
        {
            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entity = manager.CreateEntity();
            manager.AddComponentData(entity, request);
        }
    }
}