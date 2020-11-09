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
        /// Add all handlers for the given data type in the same assembly as the data type
        /// </summary>
        /// <typeparam name="TDataImplements">Type implemented by the data types</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlers<TDataImplements>(
            this IServiceCollection services)
        {
            return services
                .AddHandlersInAssembly<TDataImplements, TDataImplements>();
        }

        /// <summary>
        /// Add all handlers for the given data type in the same assembly as the TAssemblyOf type
        /// </summary>
        /// <typeparam name="TDataImplements">Type implemented by the data types</typeparam>
        /// <typeparam name="TAssemblyOf">A Type in the target assembly</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlersInAssembly<TDataImplements, TAssemblyOf>(
            this IServiceCollection services
            )
        {
            services.TryAddTransient<Executor<TDataImplements>>();

            var dataType = typeof(TDataImplements);
            var handlerType = typeof(IHandler<>);
            var handleMethodName = nameof(IHandler<object>.HandleAsync);

            foreach (var info in (from implementation in typeof(TAssemblyOf).Assembly.GetTypes()
                                  from service in implementation.GetInterfaces()
                                  where service.IsGenericType
                                          && service.GetGenericTypeDefinition() == handlerType
                                  let arguments = service.GetGenericArguments()
                                  where arguments[0].IsAssignableTo(dataType)
                                  select new
                                  {
                                      data = arguments[0],
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

                        return new Handler<TDataImplements>(
                            info.data,
                            (data) =>
                            {
                                try
                                {
                                    return (Task)handle.Invoke(
                                          sp.GetRequiredService(info.service),
                                          new object[] { data }
                                          );
                                }
                                catch (TargetInvocationException tiex)
                                {

                                    throw new HandlerException<TDataImplements>(data, tiex);
                                }
                            });
                    });
            }

            return services;
        }

        /// <summary>
        /// Add all handlers for the given data type and scope type in the same assembly as the data type
        /// </summary>
        /// <typeparam name="TDataImplements">Type implemented by the data types</typeparam>
        /// <typeparam name="TScope">Scope Type</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlers<TDataImplements, TScope>(
            this IServiceCollection services)
            where TScope : class
        {
            return services
                .AddHandlersInAssembly<TDataImplements, TScope, TDataImplements>();
        }

        /// <summary>
        /// Add all handlers for the given data type and scope type in the same assembly as the TAssemblyOf type
        /// </summary>
        /// <typeparam name="TDataImplements">Type implemented by the data types</typeparam>
        /// <typeparam name="TScope">Scope Type</typeparam>
        /// <typeparam name="TAssemblyOf">A Type in the target assembly</typeparam>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddHandlersInAssembly<TDataImplements, TScope, TAssemblyOf>(
            this IServiceCollection services
            )
            where TScope : class
        {
            services.TryAddSingleton<TScope>();
            services.TryAddTransient<Executor<TDataImplements, TScope>>();

            var dataType = typeof(TDataImplements);
            var scopeType = typeof(TScope);
            var handlerType = typeof(IHandler<,>);
            var handleMethodName = nameof(IHandler<TScope, object>.HandleAsync);

            foreach (var info in (from implementation in typeof(TAssemblyOf).Assembly.GetTypes()
                                  from service in implementation.GetInterfaces()
                                  where service.IsGenericType
                                          && service.GetGenericTypeDefinition() == handlerType
                                  let arguments = service.GetGenericArguments()
                                  where arguments[0].IsAssignableTo(dataType)
                                        && arguments[1] == scopeType
                                  select new
                                  {
                                      data = arguments[0],
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

                        return new Handler<TDataImplements, TScope>(
                            info.data,
                            info.scope,
                            (data, scope) =>
                            {
                                try
                                {
                                    return (Task)handle.Invoke(
                                        sp.GetRequiredService(info.service),
                                        new object[] { data, scope }
                                        );
                                }
                                catch (TargetInvocationException tiex)
                                {

                                    throw new HandlerException<TDataImplements, TScope>(data, scope, tiex);
                                }
                            });
                    });
            }

            return services;
        }
    }
}
