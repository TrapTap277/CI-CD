using Core.EventBus;

namespace Application.Services
{
    public interface IObjectDistanceUpdater : IGlobalEventSubscriber
    {
        void UpdateDistance(float distance);
    }
}