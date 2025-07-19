using Unity.Entities;
using UnityEngine;

namespace Tools
{
    public static class DotsExtensions
    {
        public static void SwitchSystem<T>(bool isEnabled) where T : ComponentSystemBase
        {
            var world = World.DefaultGameObjectInjectionWorld;
            
            if (world is not {IsCreated: true})
            {
                return;
            }

            var system = world.GetExistingSystemManaged<T>();
            if (system == null)
            {
                Debug.LogWarning($"[DOTS] SystemBase {typeof(T).Name} not found in world {world.Name}");
                return;
            }

            system.Enabled = isEnabled;
        }
    }
}