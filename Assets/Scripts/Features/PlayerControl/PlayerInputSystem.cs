using Features.Units.Player;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.PlayerControl
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class PlayerInputSystem : SystemBase
    {
        private InputMap _inputMap;
        
        protected override void OnCreate()
        {
            _inputMap = new();
            _inputMap.Enable();
        }

        protected override void OnDestroy()
        {
            _inputMap?.Disable();
            _inputMap?.Dispose();
        }
        
        protected override void OnUpdate()
        {
            var input = (float2)_inputMap.Player.Move.ReadValue<Vector2>();

            foreach (var movement in SystemAPI.Query<RefRW<MovementComponent>>().WithAll<UserTag>())
            {
                movement.ValueRW.velocity = input;
            }
        }
    }
}