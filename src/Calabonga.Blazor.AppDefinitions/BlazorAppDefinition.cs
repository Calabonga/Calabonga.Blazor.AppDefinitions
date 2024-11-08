using System.Reflection;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Represents metadata collected from AppDefinition
/// </summary>
public class BlazorAppDefinition
{
    public BlazorAppDefinition(Assembly assembly, IEnumerable<IBlazorModule> modules)
    {
        Assembly = assembly;
        Modules = modules;
    }

    /// <summary>
    /// Assembly information
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// Modules in provided assembly
    /// </summary>
    public IEnumerable<IBlazorModule> Modules { get; }
}
