using Features.Units.Food;
using Features.Units.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Gameplay
{
    public struct AbsorptionEvent : IComponentData
    {
        public Entity  eater;
        public Entity  target;
    }
    
    [UpdateBefore(typeof(FoodAbsorptionApplySystem))]
    [BurstCompile]
    public partial struct FoodAbsorptionDetectionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<FoodTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var eaterAspect in SystemAPI
                         .Query<MassAspect>()
                         .WithAll<PlayerTag, FoodTag>())
            {
                float eaterRadius = eaterAspect.Radius;
                float2 eaterPosition = new float2(eaterAspect.Transform.Position.x, eaterAspect.Transform.Position.y);

                foreach (var foodAspect in SystemAPI
                             .Query<MassAspect>()
                             .WithAll<FoodTag>()
                             .WithNone<PlayerTag>()) //eat players?
                {
                    if (eaterAspect.entity == foodAspect.entity)
                    {
                        continue;
                    }

                    float foodRadius = foodAspect.Radius;
                    float2 foodPosition = new float2(foodAspect.Transform.Position.x, foodAspect.Transform.Position.y);

                    float2 offset = foodPosition - eaterPosition;
                    float distanceSq = math.lengthsq(offset);

                    float requiredDistance = eaterRadius - foodRadius;

                    if (requiredDistance <= 0f || distanceSq > requiredDistance * requiredDistance)
                    {
                        continue;
                    }

                    var absorptionEvent = ecb.CreateEntity();
                    ecb.AddComponent(absorptionEvent, new AbsorptionEvent()
                    {
                        eater = eaterAspect.entity,
                        target = foodAspect.entity,
                    });
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}