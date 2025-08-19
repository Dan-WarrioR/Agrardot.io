using Features.Input;
using Features.Units.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Control
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    [BurstCompile]
    public partial struct PlayerControllerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<InputComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var input = SystemAPI.GetSingleton<InputComponent>();
            float3 direction = new float3(input.movement, 0f);

            foreach (var movement in SystemAPI
                         .Query<RefRW<MovementComponent>>()
                         .WithAll<UserTag>())
            {
                movement.ValueRW.velocity = direction;
            }
        }
    }
}