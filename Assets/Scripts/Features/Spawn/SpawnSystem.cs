using Data;
using Data.Configs;
using Features.Units.Food;
using Features.Units.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Spawn
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    [BurstCompile]
    public partial struct SpawnSystem : ISystem
    {
        private EntityQuery _foodQuery;
        private EntityQuery _botQuery;
        private EntityQuery _userQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _foodQuery = state.GetEntityQuery(ComponentType.ReadOnly<FoodTag>());
            _botQuery  = state.GetEntityQuery(ComponentType.ReadOnly<BotTag>());
            _userQuery = state.GetEntityQuery(ComponentType.ReadOnly<UserTag>());

            state.RequireForUpdate<GlobalConfig>();
            state.RequireForUpdate<FoodPrefabComponent>();
            state.RequireForUpdate<SpawnConfig>();
            state.RequireForUpdate<RandomComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var globalConfig = SystemAPI.GetSingleton<GlobalConfig>();
            var prefabs = SystemAPI.GetSingleton<FoodPrefabComponent>();
            var spawnConfig = SystemAPI.GetSingleton<SpawnConfig>();
            var randomRW = SystemAPI.GetSingletonRW<RandomComponent>();
            ref var random = ref randomRW.ValueRW.random;
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            TrySpawn(ref commandBuffer, ref _foodQuery, prefabs.foodPrefab, globalConfig.foodCount, in spawnConfig, ref random);
            TrySpawn(ref commandBuffer, ref _userQuery, prefabs.userPrefab, 1, in spawnConfig, ref random);
            TrySpawn(ref commandBuffer, ref _botQuery,  prefabs.botPrefab, globalConfig.playerCount - 1, in spawnConfig, ref random);
            
            commandBuffer.Playback(state.EntityManager);
            commandBuffer.Dispose();
        }
        
        private void TrySpawn(ref EntityCommandBuffer ecb, ref EntityQuery query, Entity prefab, int count, in SpawnConfig config, ref Random random)
        {
            int existing = query.CalculateEntityCount();
            if (existing >= count)
            {
                return;
            }

            int toSpawn = count - existing;
            for (int i = 0; i < toSpawn; i++)
            {
                float3 pos = GetRandomPosition(config, ref random);
                SpawnEntity(ref ecb, prefab, pos);
            }
        }

        private void SpawnEntity(ref EntityCommandBuffer ecb, Entity prefab, in float3 position)
        {
            Entity e = ecb.Instantiate(prefab);
            ecb.SetComponent(e, LocalTransform.FromPosition(position));
        }

        private float3 GetRandomPosition(in SpawnConfig config, ref Random random)
        {
            float x = random.NextFloat(config.mapMin.x, config.mapMax.x);
            float y = random.NextFloat(config.mapMin.y, config.mapMax.y);
            return new float3(x, y, 0f);
        }
    }
}