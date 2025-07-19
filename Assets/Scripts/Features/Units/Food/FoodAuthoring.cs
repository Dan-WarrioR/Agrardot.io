using Unity.Entities;
using UnityEngine;

namespace Features.Units.Food
{
    public struct FoodTag : IComponentData
    {
        
    }
    
    public class FoodAuthoring : MonoBehaviour
    {
        private class FoodAuthoringBaker : Baker<FoodAuthoring>
        {
            public override void Bake(FoodAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<FoodTag>(entity);
            }
        }
    }
}