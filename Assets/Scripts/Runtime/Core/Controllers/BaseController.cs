using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace Runtime.Core.Controllers
{
    public class BaseController
    {
        public PhaseTypeId CurrentPhaseTypeId { get; private set; }

        public virtual UniTask Run(CancellationToken cancellationToken)
        {
            Assert.IsFalse(CurrentPhaseTypeId == PhaseTypeId.Running, "You're trying to run already running controller!");
            CurrentPhaseTypeId = PhaseTypeId.Running;

            return UniTask.CompletedTask;
        }

        public virtual UniTask Stop(CancellationToken cancellationToken)
        {
            Assert.IsTrue(CurrentPhaseTypeId == PhaseTypeId.Running, "You're trying to stop not active controller!");
            CurrentPhaseTypeId = PhaseTypeId.Stopped;
            
            return UniTask.CompletedTask;
        }
    }
    
    public record BaseController<T>
    {
        public PhaseTypeId CurrentPhaseTypeId { get; private set; }

        public UniTask Run(T data, CancellationToken cancellationToken)
        {
            Assert.IsFalse(CurrentPhaseTypeId == PhaseTypeId.Running, "You're trying to run already running controller!");
            CurrentPhaseTypeId = PhaseTypeId.Running;

            return UniTask.CompletedTask;
        }

        public UniTask Stop(CancellationToken cancellationToken)
        {
            Assert.IsTrue(CurrentPhaseTypeId == PhaseTypeId.Running, "You're trying to stop not active controller!");
            CurrentPhaseTypeId = PhaseTypeId.Stopped;
            
            return UniTask.CompletedTask;
        }
    }
}