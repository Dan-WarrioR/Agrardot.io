using Unity.Entities;
using UnityEngine;

namespace Features.Units
{
    public struct InitializeCharacterFlag : IComponentData, IEnableableComponent
    {
        
    }
    
    public class CharacterAuthoring : MonoBehaviour
    {
        private class CharacterAuthoringBaker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<InitializeCharacterFlag>(entity);
            }
        }
    }
}