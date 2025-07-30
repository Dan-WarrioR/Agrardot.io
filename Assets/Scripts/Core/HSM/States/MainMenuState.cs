using EditorAttributes;
using Tools;

namespace Core.HSM.States
{
    public class MainMenuState : BaseState
    {
        private const string MainMenuSceneName = "MainMenu";

        protected override void OnInitialize()
        {
            SceneLoader.LoadScene(MainMenuSceneName);
        }

        protected override void OnDispose()
        {
            SceneLoader.UnloadScene(MainMenuSceneName);
        }

        [Button]
        private void ToGame()
        {
            StateMachine.SetState<GameState>();
        }
    }
}