using Unity.Entities;
using UnityEngine;

namespace Features.Units.Food
{
    public struct MassChangedTag : IComponentData, IEnableableComponent
    {
        
    }
    
    public struct FoodComponent : IComponentData
    {
        public float radius;
    }
    
    public struct FoodTag : IComponentData
    {
        
    }
    
    public class FoodAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float initialRadius = 1f;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                transform.localScale = initialRadius * 2f * Vector3.one;
            }
        }
#endif
        
        private class FoodAuthoringBaker : Baker<FoodAuthoring>
        {
            public override void Bake(FoodAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<FoodTag>(entity);
                AddComponent<MassChangedTag>(entity);
                SetComponentEnabled<MassChangedTag>(entity, false);
                AddComponent(entity, new FoodComponent
                {
                    radius = authoring.initialRadius,
                });
            }
        }
    }
}