using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Data.Configs
{
    [Serializable]
    public struct SpawnConfig : IComponentData
    {
        public float2 mapMin;
        public float2 mapMax;
        
        public float foodRespawnDelay;
        public float playerRespawnDelay;
    }
}