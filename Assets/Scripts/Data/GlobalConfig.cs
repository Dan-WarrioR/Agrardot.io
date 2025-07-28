using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Data
{
    public struct GlobalConfigComponent : IComponentData
    {
        public int foodCount;
        public int playerCount;

        public float2 mapMin;
        public float2 mapMax;
    }
    
    [CreateAssetMenu(menuName = "Configs/GlobalConfig", order = 0)]
    public class GlobalConfig : ScriptableObject
    {
        public int foodCount;
        public int playerCount;
        
        public Vector2 mapMin = new(-50, -50);
        public Vector2 mapMax = new(50, 50);
    }
}