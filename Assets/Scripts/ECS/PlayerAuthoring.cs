using ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Systems
{
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField] 
        private float moveSpeed = 5f;
    
        private class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<PlayerTagComponent>(entity);
                AddComponent<PlayerInputComponent>(entity);
                AddComponent(entity, new MovementComponent
                {
                    direction = float2.zero,
                    speed = authoring.moveSpeed
                });
            }
        }
    }
}