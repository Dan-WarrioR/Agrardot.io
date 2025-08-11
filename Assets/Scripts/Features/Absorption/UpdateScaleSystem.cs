using Data.Configs;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Absorption
{
    public partial struct UpdateScaleSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoodConfig>();
            state.RequireForUpdate<ScalableTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<FoodConfig>();

            foreach (var (eatable, transform) in SystemAPI
                         .Query<RefRO<EatableComponent>, RefRW<LocalTransform>>()
                         .WithAll<ScalableTag>()
                         .WithChangeFilter<EatableComponent>())
            {
                transform.ValueRW.Scale = MassToRadius(eatable.ValueRO.mass, config.massToScale) * 2f;
            }
        }
        
        public static float MassToRadius(float mass, float baseRadius)
        {
            return baseRadius * math.sqrt(mass / math.PI);
        }
    }
}