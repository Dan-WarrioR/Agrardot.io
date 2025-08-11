using Unity.Entities;
using UnityEngine;

namespace Features.Absorption
{
    public struct EaterTag : IComponentData
    {
        
    }
    
    public class EaterAuthoring : MonoBehaviour
    {
        private class EaterAuthoringBaker : Baker<EaterAuthoring>
        {
            public override void Bake(EaterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<EaterTag>(entity);
            }
        }
    }
}