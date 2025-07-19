using Unity.Entities;
using UnityEngine;

namespace Data
{
    public struct GlobalConfigComponent : IComponentData
    {
        public int foodCount;
        public Entity foodPrefab;

        public int playerCount;
        public Entity playerPrefab;
        public Entity mainPlayerPrefab;
    }
    
    [CreateAssetMenu(menuName = "Configs/GlobalConfig", order = 0)]
    public class GlobalConfig : ScriptableObject
    {
        public int foodCount;
        public GameObject foodPrefab;
        
        public int playerCount;
        public GameObject playerPrefab;
        public GameObject mainPlayerPrefab;
    }
}