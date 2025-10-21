using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;

namespace Core.EventBus
{
    public static class EventBus
    {
        private static Dictionary<Type, List<IGlobalEventSubscriber>> _events;

        public static void RaiseEvent<T>(Action<T> @event) where T : IGlobalEventSubscriber
        {
        }
        
        public static void Subscribe(IGlobalEventSubscriber subscriber)
        {
            var subscriberTypes = GetSubscriberTypes(subscriber);

            foreach (var subscriberType in subscriberTypes)
            {
                if(_events.ContainsKey(subscriberType) == false)
                    _events[subscriberType] = new();

                _events[subscriberType].Add(subscriber);
            }
        }

        public static void UnSubscribe(IGlobalEventSubscriber globalEventSubscriber) =>
            _events.Remove(globalEventSubscriber.GetType());

        private static List<Type> GetSubscriberTypes(IGlobalEventSubscriber globalEventSubscriber) =>
            globalEventSubscriber
                .GetType()
                .GetInterfaces()
                .Where(x => x.Implements<IGlobalEventSubscriber>() && x != typeof(IGlobalEventSubscriber))
                .ToList();
    }
}