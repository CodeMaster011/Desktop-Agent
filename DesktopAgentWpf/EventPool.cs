using System;
using System.Collections.Generic;

namespace DesktopAgentWpf
{
    public class EventPool
    {
        private readonly Dictionary<string, List<IEventHandler>> _eventPool = new Dictionary<string, List<IEventHandler>>();


        public void RegisterForEvent<TEventHandler>(string eventName, TEventHandler toHandler)
            where TEventHandler : IEventHandler, new()
        {
            if (_eventPool.TryGetValue(eventName, out var handlers))
            {
                handlers.Add(toHandler);
                return;
            }

            _eventPool.Add(eventName, new List<IEventHandler> { toHandler });
        }

        public void RaiseEvent(string eventName, object eventData)
        {
            if (!_eventPool.TryGetValue(eventName, out var handlers))
                return;

            foreach (var handler in handlers)
            {
                handler.Handle(eventName, eventData);
            }
        }
    }

    public interface IEventHandler
    {
        void Handle(string eventName, object eventData);
    }
}
