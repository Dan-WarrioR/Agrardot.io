using Data;
using Features.Gameplay;
using Features.Units.Food;
using Features.Units.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Control
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct BotTargetingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GlobalConfigComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (botMass, botMovement) in SystemAPI
                         .Query<MassAspect, RefRW<MovementComponent>>()
                         .WithAll<PlayerTag>()
                         .WithNone<UserTag>())
            {
                float3 botPosition = botMass.Transform.Position;
                float botMassValue = botMass.Mass;

                float minDistanceSq = float.MaxValue;
                float2 bestDirection = float2.zero;

                foreach (var foodMass in SystemAPI
                             .Query<MassAspect>()
                             .WithAll<FoodTag>())
                {
                    float foodMassValue = foodMass.Mass;
                    if (foodMassValue >= botMassValue)
                        continue;

                    float3 foodPosition = foodMass.Transform.Position;
                    float distanceSq = math.distancesq(botPosition, foodPosition);

                    if (distanceSq < minDistanceSq)
                    {
                        minDistanceSq = distanceSq;
                        float2 dir = foodPosition.xy - botPosition.xy;
                        bestDirection = math.normalizesafe(dir);
                    }
                }

                botMovement.ValueRW.velocity = bestDirection;
            }
        }
    }
}