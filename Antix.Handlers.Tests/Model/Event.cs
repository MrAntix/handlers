using System;

namespace Antix.Handlers.Tests.Model
{
    public static class Event
    {
        public static IEvent From(
            object data,
            string userId,
            uint sequenceNumber)
        {
            var eventType = typeof(Event<>).MakeGenericType(data.GetType());

            return Activator.CreateInstance(
                eventType,
                data,
                userId, sequenceNumber, null
                ) as IEvent;
        }
    }

    public sealed class Event<TEventData> : IEvent
    {
        public Event(
            TEventData data,
            string userId,
            uint sequenceNumber,
            DateTimeOffset? on = null
        )
        {
            Data = data;
            UserId = userId;
            SequenceNumber = sequenceNumber;
            On = on ?? DateTimeOffset.UtcNow;
        }

        public TEventData Data { get; }
        public string UserId { get; }
        public uint SequenceNumber { get; }
        public DateTimeOffset? On { get; }

        object IEvent.Data => Data;
    }
}
