using System;
using Unity.Entities;
using UnityEngine;

namespace Data.Configs
{
    [Serializable]
    public struct GlobalConfig : IComponentData
    {
        public int foodCount;
        [Min(2)]
        public int playerCount;
    }
}