using ECS.Components;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(PlayerInputSystem))]
    public partial struct PlayerControlSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (input, movement) 
                     in SystemAPI.Query<
                             RefRO<PlayerInputComponent>, 
                             RefRW<PlayerMovementComponent>>()
                         .WithAll<PlayerTagComponent>())
            {
                movement.ValueRW.direction = input.ValueRO.move;
            }
        }
    }
}