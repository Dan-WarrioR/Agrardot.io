using Unity.Entities;
using UnityEngine;

namespace Data
{
    public abstract class BaseConfig : ScriptableObject
    {
        public abstract void Register(World world);
    }
}