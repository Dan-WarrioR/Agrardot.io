using Unity.Entities;
using UnityEngine;

namespace Data
{
    public struct FoodSpawnConfigComponent : IComponentData
    {
        public float initialRadius;
    }
    
    public class FoodConfigAuthoring : MonoBehaviour
    {
        [SerializeField] 
        private FoodConfig config;
        
        private class FoodConfigAuthoringBaker : Baker<FoodConfigAuthoring>
        {
            public override void Bake(FoodConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new FoodSpawnConfigComponent
                {
                    initialRadius = authoring.config.initialRadius
                });
            }
        }
    }
}