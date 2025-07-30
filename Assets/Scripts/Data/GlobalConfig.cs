using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Data
{
    public struct GlobalConfigComponent : IComponentData
    {
        public Random random;
        
        public int foodCount;
        public int playerCount;

        public float2 mapMin;
        public float2 mapMax;
        
        public float foodRespawnDelay;
        public float playerRespawnDelay;
    }
    
    [CreateAssetMenu(menuName = "Configs/GlobalConfig", order = 0)]
    public class GlobalConfig : ScriptableObject
    {
        public uint randomSeed = 1;
        public Vector2 mapMin = new(-50, -50);
        public Vector2 mapMax = new(50, 50);
        
        
        public int foodCount;
        public int playerCount;

        public float foodRespawnDelay = 10f;
        public float playerRespawnDelay = 5f;
    }
}