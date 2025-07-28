using Features.Spawn;
using Features.Units.Food;
using Features.Units.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Features.Gameplay
{
    [UpdateAfter(typeof(FoodAbsorptionDetectionSystem))]
    [BurstCompile]
    public partial struct FoodAbsorptionApplySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AbsorptionEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
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
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            return;
            if (foodToRespawn > 0)
            {
                var foodRequest = ecb.CreateEntity();
                ecb.AddComponent(foodRequest, new SpawnRequest
                {
                    type = SpawnRequestType.Food,
                    count = foodToRespawn,
                });
            }

            if (playersToRespawn > 0)
            {
                var playerRequest = ecb.CreateEntity();
                ecb.AddComponent(playerRequest, new SpawnRequest
                {
                    type = SpawnRequestType.PlayerBot,
                    count = playersToRespawn,
                });
            }
            
            //duplicate logic to check user dead and respawn - TODO: rewrite as respawnSystem

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}