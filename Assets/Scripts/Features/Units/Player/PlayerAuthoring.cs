using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Units.Player
{
    public struct UserTag : IComponentData
    {
	    
    }
    
    public struct PlayerTag : IComponentData
    {
        
    }
    
    public struct MovementComponent : IComponentData
    {
        public float2 velocity;
        public float speed;
    }
    
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;
        
        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(entity);
                AddComponent<UserTag>(entity);
                
                AddComponent(entity, new MovementComponent
                {
                    velocity = float2.zero,
                    speed = authoring.moveSpeed,
                });
            }
        }
    }
}