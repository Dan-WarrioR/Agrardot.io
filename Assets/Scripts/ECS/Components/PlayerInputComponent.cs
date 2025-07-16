using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct PlayerInputComponent : IComponentData
    {
        public float2 move;
    }
}