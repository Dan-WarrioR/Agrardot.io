using Data;
using Features.Units.Food;
using Tools;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Features.Spawn
{
    public enum SpawnRequestType : byte
    {
        Food,
        Player,
        User,
    }
    
    public struct SpawnRequest : IComponentData
    {
        public SpawnRequestType type;
        public int count;
        public double time;
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
            var config = SystemAPI.GetSingletonRW<GlobalConfigComponent>();
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var now = SystemAPI.Time.ElapsedTime;

            foreach (var (request, requestEntity) in SystemAPI
                         .Query<RefRO<SpawnRequest>>()
                         .WithEntityAccess())
            {
                if (request.ValueRO.time > now)
                {
                    continue;
                }
                
                for (int i = 0; i < request.ValueRO.count; i++)
                {
                    var prefab = GetEntityPrefab(request.ValueRO.type, prefabConfig);
                    var instance = ecb.Instantiate(prefab);
                    var foodComponent = state.EntityManager.GetComponentData<FoodComponent>(prefab);
                    var transform = new LocalTransform
                    {
                        Position = GetRandomPosition(config),
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
                SpawnRequestType.Player => config.botPrefab,
                SpawnRequestType.User => config.userPrefab,
                _ => Entity.Null,
            };
        }
//
        private float3 GetRandomPosition(RefRW<GlobalConfigComponent> config)
        {
            float x = config.ValueRW.random.NextFloat(config.ValueRO.mapMin.x, config.ValueRO.mapMax.x);
            float y = config.ValueRW.random.NextFloat(config.ValueRO.mapMin.y, config.ValueRO.mapMax.y);
            return new float3(x, y, 0f);
        }
    }
}