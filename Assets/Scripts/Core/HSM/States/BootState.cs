using Core.SceneManagement;

namespace Core.HSM.States
{
    public class BootState : BaseState
    {
        private const string BootScene = "Boot";
        
        protected override void OnInitialize()
        {
            StateMachine.SetState<MainMenuState>();
        }

        protected override void OnDispose()
        {
            SceneLoader.UnloadScene(BootScene);
        }
    }
}