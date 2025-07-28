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
        public float baseSpeed;
        public float currentSpeed;
    }
    
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private bool isUser = false;
        
        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(entity);

                if (authoring.isUser)
                {
                    AddComponent<UserTag>(entity);
                }
                
                AddComponent(entity, new MovementComponent
                {
                    velocity = float2.zero,
                    baseSpeed = authoring.moveSpeed,
                    currentSpeed = authoring.moveSpeed,
                });
            }
        }
    }
}