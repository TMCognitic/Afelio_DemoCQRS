using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tools.CQRS.Commands;
using Tools.CQRS.Queries;

namespace Tools.CQRS
{
    public static class ServiceCollectionExtentions
    {
        public static void AddHandlers(this IServiceCollection container)
        {
            //Ne peut par retourner null car je n'utilise pas de code non managé
            Assembly? assembly = Assembly.GetEntryAssembly()!;

            List<Type> handlerTypes = assembly.GetTypes()
                .Union(assembly.GetReferencedAssemblies().SelectMany(an => Assembly.Load(an).GetTypes()))
                .Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)) && x.Name.EndsWith("Handler"))                
                .ToList();

            foreach (Type type in handlerTypes)
            {
                Type interfaceType = type.GetInterfaces().Single(y => IsHandlerInterface(y));

                InjectionModeAttribute? attribute = type.GetCustomAttribute<InjectionModeAttribute>();

                if(attribute is not null)
                {
                    switch(attribute.InjectionMode)
                    {
                        case InjectionMode.Singleton: 
                            container.AddSingleton(interfaceType, type);
                            return;
                        case InjectionMode.Transient:
                            container.AddTransient(interfaceType, type);
                            return;
                    }
                }
                
                container.AddScoped(interfaceType, type);
            }
        }

        private static bool IsHandlerInterface(Type type)
        {
            Type[] cqrsTypes = new[] { typeof(ICommandHandler<>), typeof(ICommandHandler<,>), typeof(IQueryHandler<,>) };

            if (!type.IsGenericType)
                return false;

            Type typeDefinition = type.GetGenericTypeDefinition();
            return cqrsTypes.Contains(typeDefinition);
        }
    }
}
