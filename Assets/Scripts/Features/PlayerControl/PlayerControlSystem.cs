using Features.Units.Food;
using Unity.Burst;
using Unity.Entities;

namespace Features.PlayerControl
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    [UpdateAfter(typeof(PlayerInputSystem))]
    [BurstCompile]
    public partial struct PlayerControlSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (input, movement) 
                     in SystemAPI.Query<
                             RefRO<PlayerInputComponent>, 
                             RefRW<MovementComponent>>()
                         .WithAll<PlayerTag>())
            {
                movement.ValueRW.direction = input.ValueRO.move;
            }
        }
    }
}