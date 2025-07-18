using UnityEngine;

namespace Tools
{
    public class DependencyMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Dependency.Register(GetType(), this);
        }

        protected virtual void OnDestroy()
        {
            Dependency.Unregister(GetType(), this);
        }
    }
}