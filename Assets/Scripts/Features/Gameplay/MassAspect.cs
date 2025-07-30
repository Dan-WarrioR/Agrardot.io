using Features.Units.Food;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Gameplay
{
    public readonly partial struct MassAspect : IAspect
    {
        private const float GrowthBase = 1f;
        private const float GrowthDecayRate = 0.5f;
        private const float MinGrowthFactor = 0.05f;
        
        public readonly Entity entity;
        public readonly RefRW<FoodComponent> radius;
        public readonly RefRW<LocalTransform> transform;
        [Optional] public readonly EnabledRefRW<MassChangedTag> massChangedTag;
        
        public float Radius => radius.ValueRO.radius;
        public float Mass => Radius * Radius * math.PI;
        public LocalTransform Transform => transform.ValueRW;
        
        public void AddMass(float additionalMass)
        {
            //float totalMass = Mass + additionalMass;
            
            float growthFactor = GrowthBase / (1f + GrowthDecayRate * math.log(Mass + 1f));
            growthFactor = math.max(growthFactor, MinGrowthFactor);
            
            float totalMass = Mass + additionalMass * growthFactor;
            
            if (massChangedTag.IsValid)
            {
                massChangedTag.ValueRW = true;
            }
            
            float newRadius = math.sqrt(totalMass / math.PI);
            radius.ValueRW.radius = newRadius;
            
            Resize();
        }

        public void Resize()
        {
            transform.ValueRW.Scale = Radius * 2f;
        }
    }
}