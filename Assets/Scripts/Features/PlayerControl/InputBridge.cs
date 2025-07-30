using Tools;

namespace Features.PlayerControl
{
    public class InputBridge : DependencyMonoBehaviour
    {
        private InputMap _inputMap;
        
        public InputMap InputMap => _inputMap;
        
        protected override void Awake()
        {
            base.Awake();
            _inputMap = new();
            _inputMap.Enable();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _inputMap?.Disable();
            _inputMap?.Dispose();
        }
    }
}