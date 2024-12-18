﻿using Calabonga.OperationResults;
using System.Reflection;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Command Finder helper
/// </summary>
internal static class ModulesFinder
{
    /// <summary>
    /// Finds all items in all assemblies
    /// </summary>
    /// <param name="modulesFolderPath"></param>
    /// <exception cref="FileNotFoundException"></exception>
    internal static Operation<Type[], ModuleFileNotFoundException> FindModules(string modulesFolderPath)
    {
        try
        {
            if (!Directory.Exists(modulesFolderPath))
            {
                Directory.CreateDirectory(modulesFolderPath);
                return Operation.Result(Array.Empty<Type>());
            }

            var types = new List<Type>();

            var modulesDirectory = new DirectoryInfo(modulesFolderPath);
            var files = modulesDirectory.GetFiles("*.dll");

            if (!files.Any())
            {
                return Operation.Result(Array.Empty<Type>());
            }

            foreach (var fileInfo in files)
            {
                var assembly = Assembly.LoadFrom(fileInfo.FullName) ?? throw new ArgumentNullException(nameof(modulesFolderPath));

                var exportedTypes = assembly.GetExportedTypes();
                var appDefinitions = exportedTypes.Where(Predicate).ToList();

                var definitionTypes = appDefinitions.Select(Activator.CreateInstance)
                    .Cast<IAppDefinition>()
                    .Where(x => x.Enabled && x.Exported)
                    .Select(x => x.GetType())
                    .ToList();

                if (!definitionTypes.Any())
                {
                    var error = new ModuleFileNotFoundException($"There are no any AppDefinition found in {fileInfo.FullName}");
                    return Operation.Error(error);
                }

                var modules = exportedTypes.Where(BlazorModulePredicate).ToList();

                var blazorModules = modules.Select(Activator.CreateInstance).Cast<IBlazorModule>().ToList();
                var modulesTypes = blazorModules.Select(x => x.GetType()).ToList();

                if (!modulesTypes.Any())
                {
                    var error = new ModuleFileNotFoundException($"AppDefinition found in {fileInfo.FullName}, but there are no IBlazorModule implementation were found. May be you forget enable an Export property set up.");

                    return Operation.Error(error);
                }

                ModuleDefinitions.Instance.AddAssembly(new BlazorAppDefinition(assembly, blazorModules));

                types.AddRange(exportedTypes);
            }

            return types.ToArray();
        }
        catch (Exception exception)
        {
            return Operation.Error(new ModuleFileNotFoundException(exception.Message, exception));
        }
    }

    /// <summary>
    /// Finds an AppDefinition in the list of types
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool Predicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(AppDefinition).IsAssignableFrom(type);

    /// <summary>
    /// Finds Modules in AppDefinitions
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool BlazorModulePredicate(Type type) => type is { IsAbstract: false, IsInterface: false } && typeof(IBlazorModule).IsAssignableFrom(type);
}
