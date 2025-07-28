using Features.Units.Food;
using Features.Units.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Gameplay
{
    [UpdateAfter(typeof(FoodAbsorptionApplySystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct SpeedUpdateSystem : ISystem
    {
        private const float SpeedReductionCoefficient = 1f;
        private const float MinSpeed = 0.1f;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MassChangedTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (massAspect, movement, entity) in SystemAPI
                         .Query<MassAspect, RefRW<MovementComponent>>()
                         .WithAll<PlayerTag, MassChangedTag>()
                         .WithEntityAccess())
            {
                float radius = massAspect.Radius;
                float speedReduction = (radius - SpeedReductionCoefficient) / 100f;
                float newSpeed = math.max(MinSpeed, movement.ValueRO.baseSpeed * (1f - speedReduction));
                movement.ValueRW.currentSpeed = newSpeed;
                
                SystemAPI.SetComponentEnabled<MassChangedTag>(entity, false);
            }
        }
    }
}