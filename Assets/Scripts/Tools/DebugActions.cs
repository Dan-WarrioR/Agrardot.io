using Core.HSM;
using Core.HSM.States;
using UnityEngine;

namespace Tools
{
    public class DebugActions : MonoBehaviour
    {
        public void MoveToMenuState()
        {
            var machine = Dependency.Get<GameStateMachine>();
            machine.SetState<MainMenuState>();
        }
        
        public void MoveToGameState()
        {
            var machine = Dependency.Get<GameStateMachine>();
            machine.SetState<GameState>();
        }
    }
}