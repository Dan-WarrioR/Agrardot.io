using Features.Spawn;
using Features.Units.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Units.Food
{
    public readonly partial struct MassAspect  : IAspect
    {
        public readonly RefRW<FoodComponent> radius;
        public readonly RefRW<LocalTransform> transform;
        
        public float Radius => radius.ValueRO.radius;
        public float Mass => Radius * Radius * math.PI;
        public LocalTransform Transform => transform.ValueRW;
        
        public void AddMass(float additionalMass)
        {
            float totalMass = Mass + additionalMass;
            radius.ValueRW.radius = math.sqrt(totalMass / math.PI);
            float scaleFactor = Radius / radius.ValueRO.baseFoodRadius;
            transform.ValueRW.Scale = scaleFactor;
        }
    }
    
    [BurstCompile]
    public partial struct FoodAbsorptionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoodTag>();
            state.RequireForUpdate<FoodComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            int foodToRespawn = 0;
            int playersToRespawn = 0;
            
            foreach (var (eaterAspect, eater) in SystemAPI
                         .Query<MassAspect>()
                         .WithAll<PlayerTag>()
                         .WithEntityAccess())
            {
                float eaterRadius = eaterAspect.Radius;
                float2 eaterPosition = new float2(eaterAspect.Transform.Position.x, eaterAspect.Transform.Position.y);
                float addedMass = 0f;

                foreach (var (foodAspect, food) in SystemAPI
                             .Query<MassAspect>()
                             .WithAll<FoodTag>()
                             .WithNone<PlayerTag>()
                             .WithEntityAccess())
                {
                    if (eater == food)
                    {
                        continue;
                    }

                    float foodRadius = foodAspect.Radius;
                    float2 foodPosition = new float2(foodAspect.Transform.Position.x, foodAspect.Transform.Position.y);

                    float2 offset = foodPosition - eaterPosition;
                    float distanceSq = math.lengthsq(offset);

                    float requiredDistance = eaterRadius - foodRadius;

                    if (requiredDistance <= 0f || distanceSq > requiredDistance * requiredDistance)
                    {
                        continue;
                    }

                    addedMass += foodAspect.Mass;
                    ecb.DestroyEntity(food);
                    
                    if (SystemAPI.HasComponent<PlayerTag>(food))
                    {
                        playersToRespawn++;
                    }
                    else
                    {
                        foodToRespawn++;
                    }
                }
                
                if (addedMass > 0f)
                {
                    eaterAspect.AddMass(addedMass);
                }
            }
            
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
                    type = SpawnRequestType.Player,
                    count = playersToRespawn,
                });
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}