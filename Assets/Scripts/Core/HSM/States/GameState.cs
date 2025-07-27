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
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var foodRequest = entityManager.CreateEntity();
            entityManager.AddComponentData(foodRequest, new SpawnRequest
            {
                type = SpawnRequestType.Food,
                count = globalConfig.foodCount,
            });

            var playerRequest = entityManager.CreateEntity();
            entityManager.AddComponentData(playerRequest, new SpawnRequest
            {
                type = SpawnRequestType.Player,
                count = globalConfig.playerCount,
            });
        }
    }
}