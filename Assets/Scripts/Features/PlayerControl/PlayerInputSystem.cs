using Features.Units.Player;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.PlayerControl
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class PlayerInputSystem : SystemBase
    {
        private InputBridge InputBridge => _inputBridge ??= Tools.Dependency.Get<InputBridge>();
        private InputBridge _inputBridge;
        
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerTag>();
            RequireForUpdate<MovementComponent>();
        }

        protected override void OnUpdate()
        {
            var input = (float2)InputBridge.InputMap.Player.Move.ReadValue<Vector2>();

            foreach (var movement in SystemAPI.Query<RefRW<MovementComponent>>().WithAll<UserTag>())
            {
                movement.ValueRW.velocity = input;
            }
        }
    }
}