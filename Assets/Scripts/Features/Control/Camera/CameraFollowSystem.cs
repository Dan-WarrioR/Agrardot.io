using Features.Units.Player;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Control.Camera
{
    public partial class CameraFollowSystem : SystemBase
    {
        private float3 _targetPosition;
        
        protected override void OnCreate()
        {
            RequireForUpdate<UserTag>();
            RequireForUpdate<CameraSettingsComponent>();
        }
        
        protected override void OnUpdate()
        {
            if (!SystemAPI.HasSingleton<UserTag>() 
                || !SystemAPI.HasSingleton<CameraSettingsComponent>())
            {
                return;
            }

            var camera = UnityEngine.Camera.main.transform;
            var config = SystemAPI.GetSingleton<CameraSettingsComponent>();

            var playerEntity = SystemAPI.GetSingletonEntity<UserTag>();
            var playerTransform = SystemAPI.GetComponentRO<LocalToWorld>(playerEntity).ValueRO;
            
            float3 desiredPosition = playerTransform.Position + config.offset;
            float3 cameraPosition = camera.position;

            float distanceToPlayer = math.distance(cameraPosition, desiredPosition);

            if (distanceToPlayer > config.followThreshold)
            {
                _targetPosition = desiredPosition;
            }

            float maxStep = config.lerpSpeed * SystemAPI.Time.DeltaTime;
            float3 direction = _targetPosition - cameraPosition;
            float distance = math.length(direction);
            
            float3 newPosition = math.lerp(cameraPosition, _targetPosition, config.lerpSpeed * SystemAPI.Time.DeltaTime);
            camera.position = newPosition;
            
            //float3 move = direction / distance * maxStep;
            //camera.position = cameraPosition + move;
        }
    }
}