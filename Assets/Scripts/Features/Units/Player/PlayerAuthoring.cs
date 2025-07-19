using Features.PlayerControl;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Units.Food
{
    public struct PlayerTag : IComponentData
    {
	    
    }
    
    public struct BotTag : IComponentData
    {
        
    }
    
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField] 
        private bool isMainPlayer;
        
        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<FoodTag>(entity);

                if (authoring.isMainPlayer)
                {
                    AddComponent<PlayerTag>(entity);
                    AddComponent<PlayerInputComponent>(entity);
                    
                }
                else
                {
                    AddComponent<BotTag>(entity);
                }
                
                AddComponent(entity, new MovementComponent
                {
                    direction = float2.zero,
                    speed = 5
                });
            }
        }
    }
}