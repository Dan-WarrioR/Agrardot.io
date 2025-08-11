using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Input
{
    [UpdateInGroup(typeof(GameplaySystemGroup), OrderFirst = true)]
    public partial class InputSystem : SystemBase, InputMap.IPlayerActions
    {
        private InputMap _inputMap;
        private ChangedData<InputComponent> _cachedInput;

        protected override void OnCreate()
        {
            _cachedInput = new();
            _inputMap = new();
            _inputMap.Enable();
            
            EntityManager.CreateSingleton(_cachedInput.data);
            _inputMap.Player.SetCallbacks(this);
        }
        
        protected override void OnDestroy()
        {
            _inputMap.Player.SetCallbacks(null);
            _inputMap.Dispose();
            _inputMap = null;
        }
        
        protected override void OnStartRunning()
        {
            _inputMap.Enable();
        }
        
        protected override void OnStopRunning()
        {
            _inputMap.Disable();
            var entity = SystemAPI.GetSingletonEntity<InputComponent>();
            _cachedInput = default;
            EntityManager.SetComponentData(entity, _cachedInput.data);
        }

        protected override void OnUpdate()
        {
            var entity = SystemAPI.GetSingletonEntity<InputComponent>();

            if (_cachedInput.PopIsChanged)
            {
                EntityManager.SetComponentData(entity, _cachedInput.data);
            }
        }

        void InputMap.IPlayerActions.OnMove(InputAction.CallbackContext context)
        {
            _cachedInput.data.movement = context.ReadValue<Vector2>();
            _cachedInput.MarkAsChanged();
        }

        void InputMap.IPlayerActions.OnLook(InputAction.CallbackContext context)
        {
            _cachedInput.data.look = context.ReadValue<Vector2>();
            _cachedInput.MarkAsChanged();
        }

        public void OnJump(InputAction.CallbackContext context) //for future abilities
        {
            // _cachedInput.data
            // _cachedInput.MarkAsChanged(); 
        }
    }
}