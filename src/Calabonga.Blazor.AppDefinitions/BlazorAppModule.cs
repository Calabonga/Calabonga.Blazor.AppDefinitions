using Microsoft.AspNetCore.Components.Routing;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Blazor module from assembly
/// </summary>
public sealed class BlazorAppModule : IBlazorModule
{
    public BlazorAppModule(string title, string description, string route, NavLinkMatch match)
    {
        Title = title;
        Description = description;
        Route = route;
        Match = match;
    }

    /// <summary>
    /// Module title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Brief module description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Navigation Route 
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// NavLink type match
    /// </summary>
    public NavLinkMatch Match { get; }
}
