namespace Antix.Handlers.Tests.Model.Commands
{
    public sealed class CauseError
    {
        public CauseError(
            string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
