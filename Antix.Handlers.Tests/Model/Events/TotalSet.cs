namespace Antix.Handlers.Tests.Model.Events
{
    public sealed class TotalSet
    {
        public TotalSet(
            int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
