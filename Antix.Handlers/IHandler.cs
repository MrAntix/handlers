using System.Threading.Tasks;

namespace Antix.Handlers
{
    /// <summary>
    /// A Handler for the given data
    /// </summary>
    /// <typeparam name="TData">Data Type</typeparam>
    public interface IHandler<TData>
        where TData : class
    {
        /// <summary>
        /// Handle the data, called by an executor
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Task</returns>
        Task HandleAsync(TData data);
    }

    /// <summary>
    /// A handler for the given data and scope object
    /// </summary>
    /// <typeparam name="TData">Data Type</typeparam>
    /// <typeparam name="TScope">Scope Type</typeparam>
    public interface IHandler<TData, TScope>
       where TData : class
       where TScope : class
    {
        /// <summary>
        /// Handle the data, called by an executor
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="scope">Scope object</param>
        /// <returns>Task</returns>
        Task HandleAsync(TData data, TScope scope);
    }
}
