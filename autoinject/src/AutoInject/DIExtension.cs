using AutoInject.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoInject
{
  public static class DIExtension
  {
    private static IServiceCollection _services;
    private static ServiceLifetime _serviceLifetime;
    public static IServiceCollection AddAutoDI(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
      _services = services;
      _serviceLifetime = serviceLifetime;
      var assemblyNames = assembly.GetReferencedAssemblies();
      GetAllClass(services, DiscardMicrosoftAndSystemAssemblies(assemblyNames));
      return services;
    }
    private static IEnumerable<AssemblyName> DiscardMicrosoftAndSystemAssemblies(AssemblyName[] assemblyNames)
    {
      return assemblyNames.Where(x => !x.Name.StartsWith("Microsoft.") && !x.Name.StartsWith("System."));
    }
    private static void GetAllClass(IServiceCollection services, IEnumerable<AssemblyName> assemblyNames)
    {
      foreach (var assemblyName in assemblyNames)
      {
        Assembly assembly = Assembly.Load(assemblyName);
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
          var interfaces = type.GetInterfaces().ToList();
          if (interfaces?.Count > 0)
          {
            bool isAutoDI = interfaces.Any(x => x.Name == typeof(IAutoDI).Name);
            if (isAutoDI)
            {
              var @interface = type.GetInterfaces().ToList().FirstOrDefault(x => x.Name.EndsWith(type.Name));
              AddInjection(@interface.Assembly.GetType(@interface.FullName), type);
            }
          }
        }
      }
    }
    private static IServiceCollection AddInjection(Type serviceType, Type implementationType)
    {
      if (_serviceLifetime == ServiceLifetime.Singleton)
      {
        return _services.AddSingleton(serviceType, implementationType);
      }
      if (_serviceLifetime == ServiceLifetime.Transient)
      {
        return _services.AddTransient(serviceType, implementationType);
      }
      return _services.AddScoped(serviceType, implementationType);
    }
  }
}
