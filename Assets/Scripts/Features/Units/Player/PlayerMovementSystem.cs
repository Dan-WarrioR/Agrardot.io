using Data;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Units.Player
{
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GlobalConfigComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<GlobalConfigComponent>();
            
            foreach (var (transform, movement) 
                     in SystemAPI.Query<
                         RefRW<LocalTransform>, 
                         RefRO<MovementComponent>>())
            {
                float2 direction = movement.ValueRO.velocity;
                float3 delta = new float3(direction.x, direction.y, 0) * movement.ValueRO.baseSpeed * SystemAPI.Time.DeltaTime;
                float3 newPosition = transform.ValueRO.Position + delta;
                
                newPosition.x = math.clamp(newPosition.x, config.mapMin.x, config.mapMax.x);
                newPosition.y = math.clamp(newPosition.y, config.mapMin.y, config.mapMax.y);

                transform.ValueRW.Position = newPosition;
            }
        }
    }
}