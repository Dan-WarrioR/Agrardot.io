using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Features.Units.Player
{
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MovementComponent>();
            state.RequireForUpdate<PhysicsVelocity>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (movement, velocity) in SystemAPI
                         .Query<RefRO<MovementComponent>, RefRW<PhysicsVelocity>>())
            {
                float3 desiredVelocity = movement.ValueRO.velocity * movement.ValueRO.currentSpeed;
                velocity.ValueRW.Linear = desiredVelocity;
                velocity.ValueRW.Angular = float3.zero;
            }
        }
    }
}