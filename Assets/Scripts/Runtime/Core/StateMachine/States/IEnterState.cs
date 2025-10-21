using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public interface IEnterState
    {
        UniTask Enter(CancellationToken cancellationToken);
    }
}