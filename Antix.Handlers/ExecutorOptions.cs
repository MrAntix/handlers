namespace Antix.Handlers
{
    public sealed class ExecutorOptions
    {
        public ExecutorOptions(
            bool requireHandlers)
        {
            RequireHandlers = requireHandlers;
        }

        public bool RequireHandlers { get; }

        public ExecutorOptions SetRequireHandlers(bool value)
        {
            return new ExecutorOptions(
                requireHandlers:value
                );
        }

        public static readonly ExecutorOptions Default
            = new ExecutorOptions(
                requireHandlers: false
                );
    }
}
