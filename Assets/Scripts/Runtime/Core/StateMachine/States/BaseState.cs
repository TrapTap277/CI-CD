using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public abstract class BaseState : IEnterState
    {
        private readonly StateMachine _stateMachine;

        protected BaseState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual UniTask Enter(CancellationToken cancellationToken) =>
            UniTask.CompletedTask;

        protected UniTask GoTo<T>(CancellationToken cancellationToken) where T : BaseState =>
            _stateMachine.GoTo<T>(cancellationToken);
    }
}