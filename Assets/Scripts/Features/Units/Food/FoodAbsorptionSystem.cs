using Features.Units.Player;
using Tools;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Units.Food
{
    public readonly partial struct FoodAspect : IAspect
    {
        public readonly Entity entity;
        public readonly RefRO<LocalTransform> transform;
        public readonly RefRW<RadiusComponent> radius;
        public readonly RefRW<FoodComponent> food;
    }
    
    [BurstCompile]
    public partial struct FoodAbsorptionSystem : ISystem
    {
        private const float GrowthBase = 2f;
        private const float GrowthDecayRate = 0.5f;
        private const float MinGrowthFactor = 1f;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoodComponent>();
            state.RequireForUpdate<RadiusComponent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            DotsDebug.Log("Update");
            
            foreach (var eater in SystemAPI.Query<FoodAspect>().WithAll<FoodComponent>().WithAll<PlayerTag>())
            {
                float2 eaterPos = new(eater.transform.ValueRO.Position.x, eater.transform.ValueRO.Position.z);
                float eaterRadius = eater.radius.ValueRO.radius;
                float totalMassGained = 0f;

                foreach (var target in SystemAPI.Query<FoodAspect>())
                {
                    if (eater.entity == target.entity)
                        continue;

                    float2 targetPos = new float2(target.transform.ValueRO.Position.x, target.transform.ValueRO.Position.z);
                    float targetRadius = target.radius.ValueRO.radius;

                    float distance = math.distance(eaterPos, targetPos);
                        
                    bool canEat = eaterRadius > targetRadius;
                    bool closeEnough = distance <= eaterRadius - targetRadius;

                    DotsDebug.Log($"clsoeEnoguh: {closeEnough}: distance: {distance} and can eat: {canEat}");
                    
                    if (canEat && closeEnough)
                    {
                        totalMassGained += target.food.ValueRO.mass;
                        ecb.DestroyEntity(target.entity);
                    }
                }
                
                if (totalMassGained > 0)
                {
                    float currentMass = eater.food.ValueRO.mass;
                
                    float growthFactor = GrowthBase / (1f + GrowthDecayRate * math.log(currentMass + 1));
                    growthFactor = math.max(growthFactor, MinGrowthFactor);
                
                    float newMass = currentMass + totalMassGained * growthFactor;
                    float newRadius = math.sqrt(newMass / math.PI);
                
                    eater.food.ValueRW.mass = newMass;
                    eater.radius.ValueRW.radius = newRadius;
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}