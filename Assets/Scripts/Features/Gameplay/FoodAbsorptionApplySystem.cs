using Data;
using Features.Spawn;
using Features.Units.Food;
using Features.Units.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Features.Gameplay
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    [UpdateAfter(typeof(FoodAbsorptionDetectionSystem))]
    [BurstCompile]
    public partial struct FoodAbsorptionApplySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AbsorptionEvent>();
            state.RequireForUpdate<GlobalConfigComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var config = SystemAPI.GetSingleton<GlobalConfigComponent>();
            
            int foodToRespawn = 0;
            int playersToRespawn = 0;

            foreach (var (absorptionEvent, entity) in SystemAPI
                .Query<RefRO<AbsorptionEvent>>()
                .WithEntityAccess())
            {
                var eaterEntity = absorptionEvent.ValueRO.eater;
                var eaterAspect = SystemAPI.GetAspect<MassAspect>(eaterEntity);
                
                var targetEntity = absorptionEvent.ValueRO.target;
                var targetAspect = SystemAPI.GetAspect<MassAspect>(targetEntity);
                
                eaterAspect.AddMass(targetAspect.Mass);
                ecb.DestroyEntity(targetEntity);
                ecb.DestroyEntity(entity);
                
                if (SystemAPI.HasComponent<FoodTag>(targetEntity))
                {
                    foodToRespawn++;
                }
                else if (SystemAPI.HasComponent<PlayerTag>(targetEntity))
                {
                    playersToRespawn++;
                }
            }
            
            if (foodToRespawn > 0)
            {
                var foodRequest = ecb.CreateEntity();
                ecb.AddComponent(foodRequest, new SpawnRequest
                {
                    type = SpawnRequestType.Food,
                    count = foodToRespawn,
                    time = config.foodRespawnDelay + SystemAPI.Time.ElapsedTime,
                });
            }

            if (playersToRespawn > 0)
            {
                var playerRequest = ecb.CreateEntity();
                ecb.AddComponent(playerRequest, new SpawnRequest
                {
                    type = SpawnRequestType.Player,
                    count = playersToRespawn,
                    time = config.playerRespawnDelay + SystemAPI.Time.ElapsedTime,
                });
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}