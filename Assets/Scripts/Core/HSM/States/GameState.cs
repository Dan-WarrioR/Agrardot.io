using Data;
using EditorAttributes;
using Features;
using Features.Spawn;
using Core.SceneManagement;
using Data.Configs;
using Tools;
using Unity.Entities;

namespace Core.HSM.States
{
    public class GameState : BaseState
    {
        private const string GameSceneName = "Game";
        
        protected override void OnDispose()
        {
            SceneLoader.UnloadScene(GameSceneName);
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(false);
        }
        
        protected override void OnInitialize()
        {
            DotsExtensions.SwitchSystem<GameplaySystemGroup>(true);                 

            SceneLoader.LoadScene(GameSceneName);
        }

        [Button]
        private void ToMenu()
        {
            StateMachine.SetState<MainMenuState>();
        }
    }
}