using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Control.Camera
{
    public struct CameraSettingsComponent : IComponentData
    {
        public float3 offset;
        public float lerpSpeed;
        public float followThreshold;
    }
    
    public class CameraSettingsAuthoring : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset = new Vector3(0, 0, -10);
        [SerializeField]
        private float lerpSpeed = 5f;
        [SerializeField]
        private float followThreshold = 1.5f;
        
        private class CameraSettingsAuthoringBaker : Baker<CameraSettingsAuthoring>
        {
            public override void Bake(CameraSettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CameraSettingsComponent
                {
                    offset = authoring.offset,
                    lerpSpeed = authoring.lerpSpeed,
                    followThreshold = authoring.followThreshold
                });
            }
        }
    }
}