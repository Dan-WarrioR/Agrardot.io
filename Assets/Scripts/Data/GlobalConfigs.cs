using Data.Configs;
using EditorAttributes;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Data
{
    public struct RandomComponent : IComponentData
    {
        public Random random;
    }
    
    [CreateAssetMenu(menuName = "Configs/GlobalConfigs", order = 0)]
    public class GlobalConfigs : BaseConfig
    {
        [SerializeField, Line]
        private GlobalConfig globalConfig;
        [SerializeField, Line]
        private FoodConfig foodConfig;
        [SerializeField, Line]
        private SpawnConfig spawnConfig;
        
        public override void Register(World world)
        {
            var manager = world.EntityManager;
            
            uint seed = (uint)UnityEngine.Random.Range(1, int.MaxValue);
            manager.CreateSingleton(new RandomComponent
            {
                random = new(seed),
            });
            
            manager.CreateSingleton(globalConfig);
            manager.CreateSingleton(foodConfig);
            manager.CreateSingleton(spawnConfig);
        }
    }
}