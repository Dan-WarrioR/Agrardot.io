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

    public struct BotTag : IComponentData
    {
        
    }
    
    public struct MovementComponent : IComponentData
    {
        public float3 velocity;
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
                else
                {
                    AddComponent<BotTag>(entity);
                }
                
                AddComponent(entity, new MovementComponent
                {
                    baseSpeed = authoring.moveSpeed,
                    currentSpeed = authoring.moveSpeed,
                });
            }
        }
    }
}