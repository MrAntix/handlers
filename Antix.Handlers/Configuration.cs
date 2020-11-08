using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Antix.Handlers
{
    public static class Configuration
    {
        /// <summary>
        /// Add all handlers for the given message type in the same assembly as the message type
        /// </summary>
        /// <typeparam name="TMessageImplements">Type implemented by the message types</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlers<TMessageImplements>(
            this IServiceCollection services)
        {
            return services
                .AddHandlersInAssembly<TMessageImplements, TMessageImplements>();
        }

        /// <summary>
        /// Add all handlers for the given message type in the same assembly as the TAssemblyOf type
        /// </summary>
        /// <typeparam name="TMessageImplements">Type implemented by the message types</typeparam>
        /// <typeparam name="TAssemblyOf">A Type in the target assembly</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlersInAssembly<TMessageImplements, TAssemblyOf>(
            this IServiceCollection services
            )
        {
            services.TryAddTransient<Executor<TMessageImplements>>();

            var messageType = typeof(TMessageImplements);
            var handlerType = typeof(IHandler<>);
            var handleMethodName = nameof(IHandler<object>.HandleAsync);

            foreach (var info in (from implementation in typeof(TAssemblyOf).Assembly.GetTypes()
                                  from service in implementation.GetInterfaces()
                                  where service.IsGenericType
                                          && service.GetGenericTypeDefinition() == handlerType
                                  let arguments = service.GetGenericArguments()
                                  where arguments[0].IsAssignableTo(messageType)
                                  select new
                                  {
                                      message = arguments[0],
                                      service,
                                      implementation
                                  }).ToArray())
            {
                services.AddTransient(info.service, info.implementation);
                services.AddSingleton(
                    sp =>
                    {
                        var handle = info.service
                            .GetMethod(handleMethodName);

                        return new Handler<TMessageImplements>(
                            info.message,
                            (message) =>
                            {
                                try
                                {
                                    return (Task)handle.Invoke(
                                          sp.GetRequiredService(info.service),
                                          new object[] { message }
                                          );
                                }
                                catch (TargetInvocationException tiex)
                                {

                                    throw tiex.InnerException;
                                }
                            });
                    });
            }

            return services;
        }

        /// <summary>
        /// Add all handlers for the given message type and scope type in the same assembly as the message type
        /// </summary>
        /// <typeparam name="TMessageImplements">Type implemented by the message types</typeparam>
        /// <typeparam name="TScope">Scope Type</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlers<TMessageImplements, TScope>(
            this IServiceCollection services)
            where TScope : class
        {
            return services
                .AddHandlersInAssembly<TMessageImplements, TScope, TMessageImplements>();
        }

        /// <summary>
        /// Add all handlers for the given message type and scope type in the same assembly as the TAssemblyOf type
        /// </summary>
        /// <typeparam name="TMessageImplements">Type implemented by the message types</typeparam>
        /// <typeparam name="TScope">Scope Type</typeparam>
        /// <typeparam name="TAssemblyOf">A Type in the target assembly</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlersInAssembly<TMessageImplements, TScope, TAssemblyOf>(
            this IServiceCollection services
            )
            where TScope : class
        {
            services.TryAddSingleton<TScope>();
            services.TryAddTransient<Executor<TMessageImplements, TScope>>();

            var messageType = typeof(TMessageImplements);
            var scopeType = typeof(TScope);
            var handlerType = typeof(IHandler<,>);
            var handleMethodName = nameof(IHandler<TScope, object>.HandleAsync);

            foreach (var info in (from implementation in typeof(TAssemblyOf).Assembly.GetTypes()
                                  from service in implementation.GetInterfaces()
                                  where service.IsGenericType
                                          && service.GetGenericTypeDefinition() == handlerType
                                  let arguments = service.GetGenericArguments()
                                  where arguments[0].IsAssignableTo(messageType)
                                        && arguments[1] == scopeType
                                  select new
                                  {
                                      message = arguments[0],
                                      scope = arguments[1],
                                      service,
                                      implementation
                                  }).ToArray())
            {
                services.AddTransient(info.service, info.implementation);
                services.AddSingleton(
                    sp =>
                    {
                        var handle = info.service
                            .GetMethod(handleMethodName);

                        return new Handler<TMessageImplements, TScope>(
                            info.message,
                            info.scope,
                            (message, scope) =>
                            {
                                try
                                {
                                    return (Task)handle.Invoke(
                                        sp.GetRequiredService(info.service),
                                        new object[] { message, scope }
                                        );
                                }
                                catch (TargetInvocationException tiex)
                                {

                                    throw tiex.InnerException;
                                }
                            });
                    });
            }

            return services;
        }
    }
}
