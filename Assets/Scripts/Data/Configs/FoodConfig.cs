using System;
using Unity.Entities;

namespace Data.Configs
{
    [Serializable]
    public struct FoodConfig : IComponentData
    {
        public float minMass;
        public float massToScale;
    }
}