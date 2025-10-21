using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public interface IExitState
    {
        UniTask Exit();
    }
}