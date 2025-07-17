using System;
using System.Collections.Generic;
using Core.HSM.States;
using UnityEngine;

namespace Core.HSM
{
    public class GameStateMachine : MonoBehaviour
    {
        private readonly Dictionary<Type, BaseState> _stateCache = new();
        private readonly Stack<BaseState> _stateStack = new();

        public BaseState CurrentState => _stateStack.Count > 0 ? _stateStack.Peek() : null;

        public void SetState<T>(T instance) where T : BaseState
        {
            InternalSetState(typeof(T), instance);
        }

        public void SetState<T>() where T : BaseState, new()
        {
            var type = typeof(T);

            if (!_stateCache.TryGetValue(type, out var instance))
            {
                instance = new T();
            }

            InternalSetState(type, instance);
        }

        public void Update()
        {
            if (_stateStack.Count == 0)
            {
                return;
            }

            CurrentState.OnExecute(Time.deltaTime);
        }

        public void Clear()
        {
            foreach (var state in _stateCache.Values)
            {
                state.OnDispose();
            }

            _stateCache.Clear();
            _stateStack.Clear();
        }
        
        private void InternalSetState(Type type, BaseState instance)
        {
            if (_stateStack.Count > 0 && CurrentState.GetType() == type)
            {
                return;
            }

            if (_stateStack.Count > 0)
            {
                _stateStack.Pop().OnDispose();
            }

            if (!_stateCache.ContainsKey(type))
            {
                instance.Initialize(this);
                _stateCache.Add(type, instance);
            }

            _stateStack.Push(instance);
        }
    }
}