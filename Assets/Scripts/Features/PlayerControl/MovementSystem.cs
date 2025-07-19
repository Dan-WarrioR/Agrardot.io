using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.PlayerControl
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    [BurstCompile]
    public partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, movement)
                     in SystemAPI.Query<
                         RefRW<LocalTransform>,
                         RefRO<MovementComponent>>())
            {
                float2 direction = movement.ValueRO.direction;
                float speed = movement.ValueRO.speed;

                float3 delta = new float3(direction.x, direction.y, 0) * speed * deltaTime;
                transform.ValueRW.Position += delta;
            }
        }
    }
}