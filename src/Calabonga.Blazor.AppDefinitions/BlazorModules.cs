using System.Reflection;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Blazor modules registered during application start.
/// </summary>
public sealed class BlazorModules
{
    private readonly List<Assembly> _assemblies = [];

    public void AddAssembly(Assembly assembly) => _assemblies.Add(assembly);

    public IEnumerable<Assembly> Modules => _assemblies;

    #region Singleton

    private BlazorModules() { }

    public static BlazorModules Instance => Lazy.Value;

    private static readonly Lazy<BlazorModules> Lazy = new(() => new BlazorModules());


    #endregion
}
