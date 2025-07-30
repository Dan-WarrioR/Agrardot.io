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
        
        public float zoomMultiplier;
        public float minZoom;
        public float maxZoom;
        public float zoomLerpSpeed; 
    }
    
    public class CameraSettingsAuthoring : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset = new Vector3(0, 0, -10);
        [SerializeField]
        private float lerpSpeed = 5f;
        [SerializeField]
        private float followThreshold = 1.5f;

        [Header("Zoom")]
        [SerializeField]
        private float zoomMultiplier = 1.5f;
        [SerializeField]
        private float minZoom = 5f;
        [SerializeField]
        private float maxZoom = 50f;
        [SerializeField]
        private float zoomLerpSpeed = 1.5f;
        
        private class CameraSettingsAuthoringBaker : Baker<CameraSettingsAuthoring>
        {
            public override void Bake(CameraSettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CameraSettingsComponent
                {
                    offset = authoring.offset,
                    lerpSpeed = authoring.lerpSpeed,
                    followThreshold = authoring.followThreshold,
                    
                    zoomMultiplier = authoring.zoomMultiplier,
                    minZoom = authoring.minZoom,
                    maxZoom = authoring.maxZoom,
                    zoomLerpSpeed = authoring.zoomLerpSpeed,
                });
            }
        }
    }
}