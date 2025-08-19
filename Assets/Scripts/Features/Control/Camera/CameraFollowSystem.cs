using Features.Absorption;
using Features.Units.Food;
using Features.Units.Player;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Control.Camera
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
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
            var camera = UnityEngine.Camera.main;
            var cameraTransform = camera.transform;
            var config = SystemAPI.GetSingleton<CameraSettingsComponent>();
            var playerEntity = SystemAPI.GetSingletonEntity<UserTag>();

            var playerTransform = SystemAPI.GetComponentRO<LocalToWorld>(playerEntity).ValueRO;
            var playerMass = SystemAPI.GetComponentRO<EatableComponent>(playerEntity);

            float3 targetPosition = playerTransform.Position + config.offset;
            targetPosition.z = cameraTransform.position.z;

            float3 newPosition = math.lerp(cameraTransform.position, targetPosition, config.lerpSpeed * SystemAPI.Time.DeltaTime);
            cameraTransform.position = newPosition;
        }
    }
}