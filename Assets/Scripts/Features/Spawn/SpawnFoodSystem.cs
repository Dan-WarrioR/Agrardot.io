using Data;
using Features.Units.Food;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Features.Spawn
{
    public enum SpawnRequestType : byte
    {
        Food,
        Player,
    }
    
    public struct SpawnRequest : IComponentData
    {
        public SpawnRequestType type;
        public int count;
    }
    
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct SpawnFoodSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoodPrefabComponent>();
            state.RequireForUpdate<FoodSpawnConfigComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var prefabConfig = SystemAPI.GetSingleton<FoodPrefabComponent>();
            var spawnConfig = SystemAPI.GetSingleton<FoodSpawnConfigComponent>();
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var random = new Random((uint)SystemAPI.Time.DeltaTime + 1);

            foreach (var (request, requestEntity) in SystemAPI
                         .Query<RefRO<SpawnRequest>>()
                         .WithEntityAccess())
            {
                for (int i = 0; i < request.ValueRO.count; i++)
                {
                    var prefab = GetEntityPrefab(request.ValueRO.type, prefabConfig);
                    var instance = ecb.Instantiate(prefab);

                    float3 position = new float3(
                        random.NextFloat(-5f, 5f),
                        random.NextFloat(-5f, 5f),
                        0f);

                    ecb.SetComponent(instance, LocalTransform.FromPosition(position));
                    ecb.SetComponent(instance, new FoodComponent()
                    {
                        radius = spawnConfig.initialRadius,
                    });
                }
                
                ecb.DestroyEntity(requestEntity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        private Entity GetEntityPrefab(SpawnRequestType type, FoodPrefabComponent config)
        {
            return type switch 
            {
                SpawnRequestType.Food => config.foodPrefab,
                SpawnRequestType.Player => config.playerPrefab,
                _ => Entity.Null,
            };
        }
    }
}