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
        PlayerBot,
        PlayerUser,
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
            state.RequireForUpdate<GlobalConfigComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var prefabConfig = SystemAPI.GetSingleton<FoodPrefabComponent>();
            var config = SystemAPI.GetSingleton<GlobalConfigComponent>();
            
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
                    
                    float x = random.NextFloat(config.mapMin.x, config.mapMax.x);
                    float y = random.NextFloat(config.mapMin.y, config.mapMax.y);
                    float3 position = new float3(x, y, 0f);
                    
                    var foodComponent = state.EntityManager.GetComponentData<FoodComponent>(prefab);

                    var transform = new LocalTransform
                    {
                        Position = position,
                        Rotation = quaternion.identity,
                        Scale = foodComponent.radius,
                    };

                    ecb.SetComponent(instance, transform);
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
                SpawnRequestType.PlayerBot => config.botPrefab,
                SpawnRequestType.PlayerUser => config.userPrefab,
                _ => Entity.Null,
            };
        }
    }
}