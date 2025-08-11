using System;
using Unity.Entities;

namespace Data.Configs
{
    [Serializable]
    public struct GlobalConfig : IComponentData
    {
        public int foodCount;
        public int playerCount;
    }
}