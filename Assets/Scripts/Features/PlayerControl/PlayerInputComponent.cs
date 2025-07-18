using Unity.Entities;
using Unity.Mathematics;

namespace Features.PlayerControl
{
    public struct PlayerInputComponent : IComponentData
    {
        public float2 move;
    }
}