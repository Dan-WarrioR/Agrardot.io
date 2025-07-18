using System;
using System.Collections.Generic;
using Core.HSM.States;
using Tools;
using UnityEngine;

namespace Core.HSM
{
    public class GameStateMachine : DependencyMonoBehaviour
    {
        #region Data
        
        public BaseState CurrentState => _states.Count > 0 ? _states.Peek() : null;
        
        private readonly Stack<BaseState> _states = new();
        private readonly Queue<Action> _commands = new();
        
        #endregion
        
        //////////////////////////////////////////////////
        
        #region  Unity Life Cycle

        private void OnEnable()
        {
            if (_states.Count <= 0)
            {
                return;
            }

            _states.Peek().enabled = true;
        }
        
        private void OnDisable()
        {
            if (_states.Count <= 0)
            {
                return;
            }

            _states.Peek().enabled = false;
        }

        private void Update()
        {
            CurrentState?.Execute();
        }

        private void LateUpdate()
        {
            CurrentState?.ExecuteLate();
            
            while (_commands.Count > 0)
            {
                var command = _commands.Dequeue();
                command?.Invoke();
            }
        }
        
        #endregion
        
        //////////////////////////////////////////////////
        
        #region Interface

        public void SetState<T>(T state) where T : BaseState
        {
            if (CurrentState is T)
            {
                return;
            }
            
            _commands.Enqueue(() => SetStateInternal(typeof(T)));
        }

        public void SetState(Type stateType)
        {
            if (CurrentState != null && CurrentState.GetType() == stateType)
            {
                return;
            }
            
            _commands.Enqueue(() => SetStateInternal(stateType));
        }

        public void AddState<T>() where T : BaseState
        {
            _commands.Enqueue(() => AddStateInternal(typeof(T)));
        }
        
        public void AddState(Type stateType)
        {
            if (CurrentState != null && CurrentState.GetType() == stateType)
            {
                return;
            }
            _commands.Enqueue(() => AddStateInternal(stateType));
        }

        public void PopCurrent()
        {
            _commands.Enqueue(PopCurrentState);
        }
        
        #endregion

        //////////////////////////////////////////////////
        
        #region Private Implementation

        private void SetStateInternal(Type stateType)
        {
            if (HasClone(stateType))
            {
                return;
            }

            ClearAll();
            InitializeState(stateType);
        }

        private void AddStateInternal(Type stateType)
        {
            if (HasClone(stateType))
            {
                return;
            }
            
            if (_states.Count > 0)
            {
                _states.Peek().enabled = false;
            }

            InitializeState(stateType);
        }

        private void PopCurrentState()
        {
            var state = _states.Pop();
            state.Dispose();
        }

        private bool HasClone(Type stateType)
        {
            foreach (var state in _states)
            {
                if (state.GetType() == stateType)
                {
                    return true;
                }
            }
            
            return false;
        }

        private void InitializeState(Type stateType)
        {
            var stateObject = new GameObject(stateType.Name);
            stateObject.SetActive(false);
            stateObject.transform.SetParent(transform);
            var state = stateObject.AddComponent(stateType) as BaseState;
            state.Initialize(this);
            _states.Push(state);
            stateObject.SetActive(true);
        }

        private void ClearAll()
        {
            while (_states.Count > 0)
            {
                var state = _states.Pop();
                state.Dispose();
            }
            
            _states.Clear();
        }
        
        #endregion
    }
}