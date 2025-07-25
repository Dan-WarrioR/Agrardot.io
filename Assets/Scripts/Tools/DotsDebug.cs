using Unity.Burst;
using UnityEngine;

namespace Tools
{
    public static class DotsDebug
    {
        [BurstDiscard]
        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }
        
        [BurstDiscard]
        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }
        
        [BurstDiscard]
        public static void LogError(object message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }
    }
}