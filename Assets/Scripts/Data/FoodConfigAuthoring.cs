using Unity.Entities;
using UnityEngine;

namespace Data
{
    public struct FoodSpawnConfigComponent : IComponentData
    {
        public float foodRadius;
        public float playerRadius;
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
                    foodRadius = authoring.config.initialRadius,
                    playerRadius = authoring.config.playerRadius,
                });
            }
        }
    }
}