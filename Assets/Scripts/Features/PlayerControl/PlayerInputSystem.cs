using Features.Units.Food;
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
            _inputMap = new InputMap();
            _inputMap.Enable();
        }

        protected override void OnDestroy()
        {
            _inputMap?.Disable();
            _inputMap?.Dispose();
        }
        
        protected override void OnUpdate()
        {
            var input = _inputMap.Player.Move.ReadValue<Vector2>();
            float2 moveInput  = new(input.x, input.y);
        
            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref PlayerInputComponent inputComponent) =>
                {
                    inputComponent.move = moveInput;
                }).Run();
        }
    }
}