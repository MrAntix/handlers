using System.Collections.Generic;
using System.Collections.Immutable;

namespace Antix.Handlers.Tests.Model
{
    public sealed class EventLog
    {
        readonly IList<string> _events = new List<string>();

        public void Add(string @event)
        {
            _events.Add(@event);
        }

        public IImmutableList<string> GetAll()
        {
            return _events.ToImmutableList();
        }
    }
}
