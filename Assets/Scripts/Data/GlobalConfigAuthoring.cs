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
                AddComponent(entity, new GlobalConfigComponent
                {
                    foodCount = config.foodCount,
                    playerCount = config.playerCount,
                    mapMin = config.mapMin,
                    mapMax = config.mapMax,
                });
            }
        }
    }
}