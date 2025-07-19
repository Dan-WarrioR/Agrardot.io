using Unity.Entities;
using UnityEngine;

namespace Features.Spawn
{
    public struct FoodPrefabComponent : IComponentData
    {
        public Entity foodPrefab;
        public int count;
    }
    
    public class PrefabConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject foodPrefab;
        [SerializeField]
        private int count = 100;
        
        private class PrefabConfigAuthoringBaker : Baker<PrefabConfigAuthoring>
        {
            public override void Bake(PrefabConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new FoodPrefabComponent()
                {
                    foodPrefab = GetEntity(authoring.foodPrefab, TransformUsageFlags.Dynamic),
                    count = authoring.count,
                });
            }
        }
    }
}