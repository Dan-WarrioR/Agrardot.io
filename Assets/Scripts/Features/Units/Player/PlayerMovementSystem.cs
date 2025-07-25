using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace Features.Units.Player
{
    public partial struct PlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (velocity, movement) 
                     in SystemAPI.Query<
                         RefRW<PhysicsVelocity>, 
                         RefRO<MovementComponent>>())
            {
                var moveStep = movement.ValueRO.velocity * movement.ValueRO.speed;
                velocity.ValueRW.Linear = new float3(moveStep.x, moveStep.y, 0f);
            }
        }
    }
}