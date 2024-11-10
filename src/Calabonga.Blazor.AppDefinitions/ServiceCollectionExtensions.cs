using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Extension for <see cref="WebApplicationBuilder"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// FindModules modules and register application dependencies. 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="modulesPath"></param>
    /// <param name="additionalTypes">Some additional type that can be defined in shell or in the other projects outside the shell and modules</param>
    public static void AddBlazorModulesDefinitions(this WebApplicationBuilder source, string modulesPath, params Type[]? additionalTypes)
    {
        if (string.IsNullOrEmpty(modulesPath))
        {
            throw new ModulesConfigurationException("Folder path for modules not provided");
        }

        var types = new List<Type>();

        if (additionalTypes is not null)
        {
            types.AddRange(additionalTypes);
        }

        var moduleTypes = ModulesFinder.FindModules(modulesPath);
        if (moduleTypes.Ok)
        {
            types.AddRange(moduleTypes.Result);
        }

        if (!types.Any())
        {
            return;
        }

        source.AddDefinitions(types.ToArray());
    }

    /// <summary>
    /// Finding all definitions in your project and include their into pipeline.<br/>
    /// Using <see cref="WebApplication"/> for registration.
    /// </summary>
    /// <remarks>
    /// When executing on development environment there are more diagnostic information available on console.
    /// </remarks>
    /// <param name="source"></param>
    public static void UseDefinitions(this WebApplication source)
    {
        var logger = source.Services.GetRequiredService<ILogger<AppDefinition>>();
        var definitionCollection = source.Services.GetService<AppDefinitionCollection>();

        if (definitionCollection is null)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("Some dependencies was not found. Make sure .AddDefinition(...) or .AddDefinitionWithModules(...) invoked");
            }
            return;
        }

        var items = definitionCollection.GetDistinct().OrderBy(x => x.Definition.ApplicationOrderIndex).ToList();

        foreach (var item in items)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("[AppDefinitions ConfigureApplication with order index {@ApplicationOrderIndex}]: {@AssemblyName}:{@AppDefinitionName} is {EnabledOrDisabled}",
                    item.Definition.ApplicationOrderIndex,
                    item.AssemblyName,
                    item.Definition.GetType().Name,
                    item.Enabled
                        ? "enabled"
                        : "disabled");
            }

            item.Definition.ConfigureApplication(source);
        }

        if (!logger.IsEnabled(LogLevel.Debug))
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("[AppDefinitions applied: {Count} of {Total}", items.Count, definitionCollection.GetEnabled().Count());
            }
            return;
        }

        var skipped = definitionCollection.GetEnabled().Except(items).ToList();
        if (!skipped.Any())
        {
            logger.LogInformation("[AppDefinitions applied: {Count} of {Total}", items.Count, definitionCollection.GetEnabled().Count());
            return;
        }

        logger.LogWarning("[AppDefinitions skipped ConfigureApplication: {Count}", skipped.Count);
        foreach (var item in skipped)
        {
            logger.LogWarning("[AppDefinitions skipped ConfigureApplication]: {@AssemblyName}:{@AppDefinitionName} is {EnabledOrDisabled} {ExportEnabled}",
                item.AssemblyName,
                item.Definition.GetType().Name,
                item.Enabled ? "enabled" : "disabled",
                item.Exported ? "(exportable)" : string.Empty);
        }

        logger.LogInformation("[AppDefinitions applied: {Count} of {Total}", items.Count, definitionCollection.GetEnabled().Count());
    }

    /// <summary>
    /// Finding all definitions in your project and include their into pipeline.<br/>
    /// Using <see cref="IServiceCollection"/> for registration.
    /// </summary>
    /// <remarks>
    /// When executing on development environment there are more diagnostic information available on console.
    /// </remarks>
    /// <param name="builder"></param>
    /// <param name="entryPointsAssembly"></param>
    private static void AddDefinitions(this WebApplicationBuilder builder, params Type[] entryPointsAssembly)
    {
        var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<IAppDefinition>>();
        try
        {
            var appDefinitionInfo = builder.Services.BuildServiceProvider().GetService<AppDefinitionCollection>();
            var definitionCollection = appDefinitionInfo ?? new AppDefinitionCollection();

            foreach (var entryPoint in entryPointsAssembly)
            {
                definitionCollection.AddEntryPoint(entryPoint.Name);

                var types = entryPoint.Assembly.ExportedTypes.Where(Predicate);
                var instances = types.Select(Activator.CreateInstance).Cast<IAppDefinition>().Where(x => x.Enabled).OrderBy(x => x.ServiceOrderIndex).ToList();

                foreach (var definition in instances)
                {
                    definitionCollection.AddInfo(new AppDefinitionItem(definition, entryPoint.Name, definition.Enabled, definition.Exported));
                }
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("[AppDefinitions entry points found]: {@items}", string.Join(", ", definitionCollection.EntryPoints));
            }

            var items = definitionCollection.GetDistinct().ToList();

            foreach (var item in items)
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("[AppDefinitions ConfigureServices with order index {@ServiceOrderIndex}]: {@AssemblyName}:{@AppDefinitionName} is {EnabledOrDisabled} {ExportEnabled}",
                        item.Definition.ServiceOrderIndex,
                        item.AssemblyName,
                        item.Definition.GetType().Name,
                        item.Enabled ? "enabled" : "disabled",
                        item.Exported ? "(exportable)" : string.Empty);
                }

                item.Definition.ConfigureServices(builder);
            }

            builder.Services.AddSingleton(definitionCollection);

            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            var skipped = definitionCollection.GetEnabled().Except(items).ToList();
            if (!skipped.Any())
            {
                return;
            }

            logger.LogWarning("[AppDefinitions skipped ConfigureServices: {Count}", skipped.Count);
            foreach (var item in skipped)
            {
                logger.LogWarning("[AppDefinitions skipped ConfigureServices]: {@AssemblyName}:{@AppDefinitionName} is {EnabledOrDisabled}",
                    item.AssemblyName,
                    item.Definition.GetType().Name,
                    item.Enabled ? "enabled" : "disabled");
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            throw;
        }
    }

    /// <summary>
    /// Finds an AppDefinition in the list of types
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool Predicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(AppDefinition).IsAssignableFrom(type);
}
