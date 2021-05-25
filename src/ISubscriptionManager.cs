using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVID19.Termin.Bot
{
    public interface ISubscriptionManager
    {
        Task Add(string id, Action action);
        Task Remove(string id);

        Task Run();
    }

    class SubscriptionManager : ISubscriptionManager
    {
        private Dictionary<string, Action> _dict = new Dictionary<string, Action>();
        public Task Add(string id, Action action)
        {
            _dict.Add(id, action);
            return Task.CompletedTask;
        }

        public Task Remove(string id)
        {
            if (_dict.ContainsKey(id))
            {
                _dict.Remove(id);
            }

            return Task.CompletedTask;
        }

        public Task Run()
        {
            foreach (var action in _dict.Values)
            {
                action.Invoke();
            }
            return Task.CompletedTask;
        }
    }
}