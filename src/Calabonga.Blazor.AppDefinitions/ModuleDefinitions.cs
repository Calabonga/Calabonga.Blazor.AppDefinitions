using System.Reflection;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Blazor modules definition registered during application start.
/// </summary>
public sealed class ModuleDefinitions
{
    private readonly List<BlazorAppDefinition> _assemblies = [];

    public void AddAssembly(BlazorAppDefinition appDefinition) => _assemblies.Add(appDefinition);

    /// <summary>
    /// Registered assemblies from <see cref="AppDefinition"/>
    /// </summary>
    public IEnumerable<Assembly> Assemblies => _assemblies.Select(x => x.Assembly);

    /// <summary>
    /// Registered modules from <see cref="AppDefinition"/>
    /// </summary>
    public IEnumerable<BlazorAppDefinition> Modules => _assemblies;

    #region Singleton

    private ModuleDefinitions() { }

    public static ModuleDefinitions Instance => Lazy.Value;

    private static readonly Lazy<ModuleDefinitions> Lazy = new(() => new ModuleDefinitions());


    #endregion
}
