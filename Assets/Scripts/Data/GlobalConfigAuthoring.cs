using Unity.Entities;
using UnityEngine;

namespace Data
{
    public class GlobalConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GlobalConfig config;
        
        private class GlobalConfigAuthoringBaker : Baker<GlobalConfigAuthoring>
        {
            public override void Bake(GlobalConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var config = authoring.config;
                // AddComponent(entity, new GlobalConfigComponent
                // {
                //     foodPrefab = GetEntity(config.foodPrefab, TransformUsageFlags.Dynamic),
                //     foodCount = config.foodCount,
                //
                //     playerPrefab = GetEntity(config.playerPrefab, TransformUsageFlags.Dynamic),
                //     mainPlayerPrefab = GetEntity(config.mainPlayerPrefab, TransformUsageFlags.Dynamic),
                //     playerCount = config.playerCount,
                // });
            }
        }
    }
}