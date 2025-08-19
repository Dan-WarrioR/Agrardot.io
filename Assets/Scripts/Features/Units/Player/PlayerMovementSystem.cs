using Data.Configs;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Units.Player
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    [BurstCompile]
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MovementComponent>();
            state.RequireForUpdate<SpawnConfig>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spawnConfig = SystemAPI.GetSingleton<SpawnConfig>();
            
            float2 min = spawnConfig.mapMin;
            float2 max = spawnConfig.mapMax;

            foreach (var (movement, localTransform) in SystemAPI
                         .Query<RefRO<MovementComponent>, RefRW<LocalTransform>>())
            {
                float3 desired = movement.ValueRO.velocity * movement.ValueRO.currentSpeed;
                float3 newPosition = localTransform.ValueRW.Position + desired * SystemAPI.Time.DeltaTime;
                newPosition.x = math.clamp(newPosition.x, min.x, max.x);
                newPosition.y = math.clamp(newPosition.y, min.y, max.y);
                localTransform.ValueRW.Position = newPosition;
            }
        }
    }
}