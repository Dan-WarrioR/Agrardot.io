using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Features.Units.Player
{
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, movement) 
                     in SystemAPI.Query<
                         RefRW<LocalTransform>, 
                         RefRO<MovementComponent>>())
            {
                float2 direction = movement.ValueRO.velocity;
                float3 delta = new float3(direction.x, direction.y, 0) * movement.ValueRO.speed * SystemAPI.Time.DeltaTime;
                
                transform.ValueRW.Position += delta;
            }
        }
    }
}