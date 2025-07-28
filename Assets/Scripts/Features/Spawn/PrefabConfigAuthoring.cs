using Unity.Entities;
using UnityEngine;

namespace Features.Spawn
{
    public struct FoodPrefabComponent : IComponentData
    {
        public Entity foodPrefab;
        public Entity botPrefab;
        public Entity userPrefab;
    }
    
    public class PrefabConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject foodPrefab;
        [SerializeField]
        private GameObject botPrefab;
        [SerializeField]
        private GameObject userPrefab;
        
        private class PrefabConfigAuthoringBaker : Baker<PrefabConfigAuthoring>
        {
            public override void Bake(PrefabConfigAuthoring authoring)
            {
                var foodEntity = GetEntity(TransformUsageFlags.None);
                AddComponent(foodEntity, new FoodPrefabComponent()
                {
                    foodPrefab = GetEntity(authoring.foodPrefab, TransformUsageFlags.Dynamic),
                    botPrefab = GetEntity(authoring.botPrefab, TransformUsageFlags.Dynamic),
                    userPrefab = GetEntity(authoring.userPrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}