using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

// switch assert to NSubstitute
namespace Core.StateMachine
{
    public class StateMachine
    {
        private readonly Dictionary<Type, BaseState> _states = new();
        
        private BaseState _previousState;
        private BaseState _currentState;
        
        public void Initialize(BaseState initialState, params BaseState[] states)
        {
            foreach (var state in states)
                _states.Add(state.GetType(), state);

            SetInitialStateThroughReflection(initialState);
        }

        public async UniTask GoTo<T>(CancellationToken cancellationTokenSource) where T : BaseState
        {
            if(cancellationTokenSource is { IsCancellationRequested: true })
                return;

            _states.TryGetValue(typeof(T), out var state);
            Assert.IsTrue(state != null, $"State machine doesn't contain {typeof(T)}");

            if(_currentState != null)
                _previousState = _currentState;

            _currentState = state;

            if(_previousState is IExitState exitState)
            {
                Debug.Log($"Previous state {_previousState.GetType()}, exit state type : {exitState.GetType()}");

                await exitState.Exit();
            }

            if(cancellationTokenSource is { IsCancellationRequested: true })
                return;

            await _currentState.Enter(cancellationTokenSource);
        }

        private void SetInitialStateThroughReflection(BaseState initialState)
        {
            var goToMethod = typeof(StateMachine).GetMethod(nameof(GoTo), new[] { typeof(CancellationToken) });
            goToMethod?.MakeGenericMethod(initialState.GetType()).Invoke(this, new object[] { null });
        }
    }
}