using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Input
{
    public struct InputComponent : IComponentData, IEquatable<InputComponent>
    {
        public float2 movement;
        public float2 look;
        
        public bool Equals(InputComponent other)
        {
            return movement.Equals(other.movement) 
                   && look.Equals(other.look);
        }

        public override bool Equals(object obj)
        {
            return obj is InputComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(movement, look);
        }
    }
}