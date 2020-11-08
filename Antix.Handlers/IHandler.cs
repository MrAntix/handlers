using System.Threading.Tasks;

namespace Antix.Handlers
{
    /// <summary>
    /// A Handler for the given message
    /// </summary>
    /// <typeparam name="TMessage">Message Type</typeparam>
    public interface IHandler<TMessage>
        where TMessage : class
    {
        /// <summary>
        /// Handle the message, called by an executor
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task HandleAsync(TMessage message);
    }

    /// <summary>
    /// A handler for the given message and scope object
    /// </summary>
    /// <typeparam name="TMessage">Message Type</typeparam>
    /// <typeparam name="TScope">Scope Type</typeparam>
    public interface IHandler<TMessage, TScope>
       where TMessage : class
       where TScope : class
    {
        /// <summary>
        /// Handle the message, called by an executor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="scope">Scope object</param>
        /// <returns>Task</returns>
        Task HandleAsync(TMessage message, TScope scope);
    }
}
