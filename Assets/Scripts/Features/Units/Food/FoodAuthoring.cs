using Unity.Entities;
using UnityEngine;

namespace Features.Units.Food
{
    public struct FoodComponent : IComponentData
    {
        public float mass;
    }
    
    public struct RadiusComponent : IComponentData
    {
        public float radius;
    }
    
    public struct FoodTag : IComponentData
    {
        
    }
    
    public class FoodAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float initialMass = 1f;
        [SerializeField]
        private float initialRadius = 1f;
        
        private class FoodAuthoringBaker : Baker<FoodAuthoring>
        {
            public override void Bake(FoodAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<FoodTag>(entity);
                AddComponent(entity, new FoodComponent
                {
                    mass = authoring.initialMass,
                });
                AddComponent(entity, new RadiusComponent
                {
                    radius = authoring.initialRadius,
                });
            }
        }
    }
}