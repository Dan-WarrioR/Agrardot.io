using Unity.Entities;
using UnityEngine;

namespace Features.Spawn
{
    public struct FoodPrefabComponent : IComponentData
    {
        public Entity foodPrefab;
        public Entity playerPrefab;
    }
    
    public class PrefabConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject foodPrefab;
        [SerializeField]
        private GameObject playerPrefab;
        
        private class PrefabConfigAuthoringBaker : Baker<PrefabConfigAuthoring>
        {
            public override void Bake(PrefabConfigAuthoring authoring)
            {
                var foodEntity = GetEntity(TransformUsageFlags.None);
                AddComponent(foodEntity, new FoodPrefabComponent()
                {
                    foodPrefab = GetEntity(authoring.foodPrefab, TransformUsageFlags.Dynamic),
                    playerPrefab = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}