using System.Threading;
using Application.GameState.Controllers;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Application.GameStateMachine
{
    public class GameState : BaseState, IExitState
    {
        private readonly GameController _gameController;

        public GameState(StateMachine stateMachine,
            GameController gameController) : base(stateMachine)
        {
            _gameController = gameController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            base.Enter(cancellationToken).Forget();
            await _gameController.Run(cancellationToken);
        }

        UniTask IExitState.Exit()
        {
            Debug.Log($"Game state exited!");
            
            return UniTask.CompletedTask;
        }
    }
}