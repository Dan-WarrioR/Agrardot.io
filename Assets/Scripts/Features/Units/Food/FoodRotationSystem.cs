using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Features.Units.Food
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    [BurstCompile]
    public partial struct FoodRotationSystem : ISystem
    {
        private const float RotationSpeed = 1f;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FoodTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new RotateJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                speed = RotationSpeed
            };

            job.Schedule();
        }
    }
    
    [BurstCompile]
    [WithAll(typeof(FoodTag))]
    [WithNone(typeof(PlayerTag), typeof(BotTag))]
    public partial struct RotateJob : IJobEntity
    {
        public float deltaTime;
        public float speed;

        public void Execute(ref LocalTransform transform)
        {
            float angle = speed * deltaTime;
            transform = transform.RotateZ(angle);
        }
    }
}