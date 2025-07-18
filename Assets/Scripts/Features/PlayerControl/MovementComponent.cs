using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct MovementComponent : IComponentData
    {
        public float2 direction;
        public float speed;
    }
}