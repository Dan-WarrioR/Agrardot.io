using Features.Absorption;
using Features.Units.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Control
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct BotTargetingSystem : ISystem
    {
        private EntityQuery _eatableQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EatableComponent>();
            state.RequireForUpdate<EaterTag>();
            state.RequireForUpdate<BotTag>();
            
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<EatableComponent, LocalTransform>();
            _eatableQuery = state.GetEntityQuery(builder);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var eatableEntities = _eatableQuery.ToEntityArray(Allocator.TempJob);
            var eatableTransforms = _eatableQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);
            var eatableComponents = _eatableQuery.ToComponentDataArray<EatableComponent>(Allocator.TempJob);

            var job = new BotSeekJob
            {
                eatableEntities = eatableEntities,
                eatableTransforms = eatableTransforms,
                eatableComponents = eatableComponents,
            };

            var jobHandle = job.ScheduleParallel(state.Dependency);
            
            jobHandle = eatableEntities.Dispose(jobHandle);
            jobHandle = eatableTransforms.Dispose(jobHandle);
            jobHandle = eatableComponents.Dispose(jobHandle);
            
            state.Dependency = jobHandle;
        }
        
        [BurstCompile]
        [WithAll(typeof(BotTag))]
        private partial struct BotSeekJob : IJobEntity
        {
            [ReadOnly] 
            public NativeArray<Entity> eatableEntities;
            [ReadOnly] 
            public NativeArray<LocalTransform> eatableTransforms;
            [ReadOnly] 
            public NativeArray<EatableComponent> eatableComponents;
            
            private void Execute(
                Entity botEntity,
                ref MovementComponent movement,
                in LocalTransform botTransform,
                in EatableComponent botEatable)
            {
                float3 botPos = botTransform.Position;
                float2 bestDirection = float2.zero;
                float bestDistanceSq = float.MaxValue;
                
                for (int i = 0; i < eatableEntities.Length; i++)
                {
                    if (eatableEntities[i] == botEntity)
                    {
                        continue;
                    }

                    if (eatableComponents[i].mass >= botEatable.mass)
                    {
                        continue;
                    }

                    float3 targetPos = eatableTransforms[i].Position;
                    float2 direction = targetPos.xy - botPos.xy;
                    float distanceSq = math.lengthsq(direction);

                    if (!(distanceSq < bestDistanceSq))
                    {
                        continue;
                    }

                    bestDistanceSq = distanceSq;
                    bestDirection = direction;
                }

                movement.velocity = new float3(math.normalizesafe(bestDirection), 0f);
            }
        }
    }
}