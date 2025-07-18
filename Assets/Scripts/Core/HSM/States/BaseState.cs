using UnityEngine;

namespace Core.HSM.States
{
    public abstract class BaseState : MonoBehaviour
    {
        protected GameStateMachine StateMachine { get; private set; }

        public void Initialize(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            OnInitialize();
            enabled = true;
        }
        
        public void Dispose()
        {
            enabled = false;
            OnDispose();
            Destroy(gameObject);
        }

        public void Execute()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            OnExecute();
        }
        
        public void ExecuteLate()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            OnLateExecute();
        }
        
        private void OnEnable()
        {
            OnActivate();
        }
        
        private void OnDisable()
        {
            OnDeactivate();
        }
        
        protected virtual void OnInitialize() { }
        protected virtual void OnDispose() { }
        protected virtual void OnActivate() { }
        protected virtual void OnDeactivate() { }
        protected virtual void OnExecute() { }
        protected virtual void OnLateExecute() { }
    }
}