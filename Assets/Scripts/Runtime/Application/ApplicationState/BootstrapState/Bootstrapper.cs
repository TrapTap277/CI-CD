using Core.StateMachine;
using VContainer.Unity;

namespace Application.GameStateMachine
{
    public class Bootstrapper : IStartable
    {
        private readonly StateMachine _stateMachine;
        private readonly BootstrapState _bootstrapState;
        private readonly GameState _gameState;

        public Bootstrapper(StateMachine stateMachine,
            BootstrapState bootstrapState,
            GameState gameState)
        {
            _stateMachine = stateMachine;
            _bootstrapState = bootstrapState;
            _gameState = gameState;
        }

        void IStartable.Start()
        {
            _stateMachine.Initialize(initialState: _bootstrapState, _bootstrapState, _gameState);
        }
    }
}