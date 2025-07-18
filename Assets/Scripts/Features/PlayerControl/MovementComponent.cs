using Unity.Entities;
using Unity.Mathematics;

namespace Features.PlayerControl
{
    public struct MovementComponent : IComponentData
    {
        public float2 direction;
        public float speed;
    }
}