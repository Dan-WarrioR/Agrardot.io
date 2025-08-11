using Unity.Entities;
using UnityEngine;

namespace Features.Absorption
{
    public struct EatableComponent : IComponentData
    {
        public float mass;
    }
    
    public struct FoodTag : IComponentData
    {
        
    }

    public struct ScalableTag : IComponentData
    {
        
    }
    
    public class EatableAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float initialMass = 1f;
        [SerializeField]
        private bool isScalable = true;
        
        private class EatableAuthoringBaker : Baker<EatableAuthoring>
        {
            public override void Bake(EatableAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<FoodTag>(entity);
                AddComponent(entity, new EatableComponent()
                {
                    mass = authoring.initialMass,
                });

                if (authoring.isScalable)
                {
                    AddComponent<ScalableTag>(entity);
                }
            }
        }
    }
}