using Features.Units.Food;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Gameplay
{
    public readonly partial struct MassAspect : IAspect
    {
        public readonly Entity entity;
        public readonly RefRW<FoodComponent> radius;
        public readonly RefRW<LocalTransform> transform;
        
        public float Radius => radius.ValueRO.radius;
        public float Mass => Radius * Radius * math.PI;
        public LocalTransform Transform => transform.ValueRW;
        
        public void AddMass(float additionalMass)
        {
            float totalMass = Mass + additionalMass;
            float newRadius = math.sqrt(totalMass / math.PI);
            radius.ValueRW.radius = newRadius;
                //float scaleFactor = Radius / radius.ValueRO.baseFoodRadius;
            //transform.ValueRW.Scale = scaleFactor;
            transform.ValueRW.Scale = newRadius;
        }
    }
}