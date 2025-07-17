namespace Core.HSM.States
{
    public abstract class BaseState
    {
        protected GameStateMachine StateMachine { get; private set; }

        public void Initialize(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            OnInitialize();
        }

        public virtual void OnInitialize() { }
        public virtual void OnExecute(float deltaTime) { }
        public virtual void OnDispose() { }
    }
}