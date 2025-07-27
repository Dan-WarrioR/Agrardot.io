using Unity.Entities;
using UnityEngine;

namespace Features.Units.Food
{
    public struct FoodComponent : IComponentData
    {
        public float radius;
        public float baseFoodRadius;
    }
    
    public struct FoodTag : IComponentData
    {
        
    }
    
    public class FoodAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float initialRadius = 1f;
        
        private class FoodAuthoringBaker : Baker<FoodAuthoring>
        {
            public override void Bake(FoodAuthoring authoring)
            {
                var renderer = authoring.GetComponent<SpriteRenderer>();
                float spriteWorldRadius = renderer.bounds.extents.magnitude;
                float scaleFactor = authoring.initialRadius / spriteWorldRadius;
                authoring.transform.localScale = scaleFactor * Vector3.one;
                
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<FoodTag>(entity);
                AddComponent(entity, new FoodComponent
                {
                    radius = authoring.initialRadius,
                    baseFoodRadius = spriteWorldRadius,
                });
            }
        }
    }
}