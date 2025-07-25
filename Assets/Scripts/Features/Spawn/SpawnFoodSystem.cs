using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Spawn
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct SpawnFoodSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoodPrefabComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var config = SystemAPI.GetSingleton<FoodPrefabComponent>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var random = new Random((uint)SystemAPI.Time.DeltaTime + 1);
            
            for (int i = 0; i < config.count; i++)
            {
                var food = ecb.Instantiate(config.foodPrefab);

                float3 position = new float3(
                    random.NextFloat(-5f, 5f),
                    random.NextFloat(-5f, 5f),
                    0f);

                ecb.SetComponent(food, LocalTransform.FromPosition(position));
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}