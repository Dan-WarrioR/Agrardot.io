using Data.Configs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Absorption
{
    public partial struct AbsorptionSystem : ISystem
    {
        private EntityQuery _eaterQuery;
        private EntityQuery _eatableQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoodConfig>();
        
            _eaterQuery = SystemAPI.QueryBuilder()
                .WithAll<EaterTag, EatableComponent, LocalTransform>()
                .Build();
            
            _eatableQuery = SystemAPI.QueryBuilder()
                .WithAll<EatableComponent, LocalTransform>()
                .WithNone<EaterTag>()
                .Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float baseRadius = SystemAPI.GetSingleton<FoodConfig>().massToScale;
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
        
            var job = new AbsorptionJob
            {
                baseRadius = baseRadius,
                eatableEntities = _eatableQuery.ToEntityArray(Allocator.TempJob),
                eatableComponents = _eatableQuery.ToComponentDataArray<EatableComponent>(Allocator.TempJob),
                eatableTransforms = _eatableQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob),
                ecb = ecb.AsParallelWriter(),
            };

            var jobHandle = job.ScheduleParallel(_eaterQuery, state.Dependency);
            jobHandle.Complete();
        
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            job.eatableEntities.Dispose();
            job.eatableComponents.Dispose();
            job.eatableTransforms.Dispose();
        }
    }
    
    public partial struct AbsorptionJob : IJobEntity
    {
        [ReadOnly] public float baseRadius;
        [ReadOnly] public NativeArray<Entity> eatableEntities;
        [ReadOnly] public NativeArray<EatableComponent> eatableComponents;
        [ReadOnly] public NativeArray<LocalTransform> eatableTransforms;
        public EntityCommandBuffer.ParallelWriter ecb;

        private void Execute(Entity eaterEntity, [ChunkIndexInQuery] int chunkIndex, 
            in EatableComponent eaterComponent, in LocalTransform eaterTransform)
        {
            float3 eaterPosition = eaterTransform.Position;
            float eaterRadius = UpdateScaleSystem.MassToRadius(eaterComponent.mass, baseRadius);
            float eaterRadiusSq = eaterRadius * eaterRadius;
            float totalMassGained = 0f;
            
            for (int i = 0; i < eatableEntities.Length; i++)
            {
                var targetEntity = eatableEntities[i];
            
                if (targetEntity == eaterEntity)
                {
                    continue;
                }

                var targetComponent = eatableComponents[i];
            
                if (targetComponent.mass >= eaterComponent.mass)
                {
                    continue;
                }

                float3 targetPosition = eatableTransforms[i].Position;
                float deltaX = eaterPosition.x - targetPosition.x;
                float deltaY = eaterPosition.y - targetPosition.y;
                float distanceSq = deltaX * deltaX + deltaY * deltaY;

                if (distanceSq > eaterRadiusSq)
                {
                    continue;
                }

                totalMassGained += targetComponent.mass;
                ecb.DestroyEntity(chunkIndex, targetEntity);
            }

            if (totalMassGained <= 0f)
            {
                return;
            }

            ecb.SetComponent(chunkIndex, eaterEntity, new EatableComponent
            {
                mass = eaterComponent.mass + totalMassGained,
            });
        }
    }
}