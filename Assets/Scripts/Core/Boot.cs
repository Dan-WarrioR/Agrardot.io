using System;
using Core.HSM;
using Core.HSM.States;
using Data;
using EditorAttributes;
using Tools;
using UnityEngine;

namespace Core
{
    public class Boot : MonoBehaviour
    {
        private enum StartState
        {
            Game,
            Menu,
            Boot,
        }
        
        private static bool IsPreloaded = false;

        [SerializeField, SceneDropdown]
        private string coreScene = "Core";
        [SerializeField]
        private StartState startState;
        [SerializeField]
        private GlobalConfig globalConfig;
        
        private void Awake()
        {
            Dependency.Register(globalConfig);
            
            if (gameObject.scene.name.Equals(coreScene))
            {
                Debug.LogError("Trying to load core twice!");
                return;
            }

            if (IsPreloaded)
            {
                return;
            }
                
            IsPreloaded = true;
            SceneLoader.LoadScene(coreScene, () =>
            {
                var stateMachine = Dependency.Get<GameStateMachine>();
                var state = GetStateType(startState);
                stateMachine.SetState(state);
            });
        }

        private void Start()
        {
            SceneLoader.ForceActiveScene = gameObject.scene;
        }

        private Type GetStateType(StartState state)
        {
            return state switch 
            {
                StartState.Game => typeof(GameState),
                StartState.Menu => typeof(MainMenuState),
                StartState.Boot => typeof(BootState),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}