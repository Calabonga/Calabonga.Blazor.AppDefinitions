using Microsoft.AspNetCore.Components.Routing;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Blazor module from assembly
/// </summary>
public sealed class BlazorAppModule : IBlazorModule
{
    public BlazorAppModule(string title, string description, string route, NavLinkMatch match, bool isHidden, int orderIndex)
    {
        Title = title;
        Description = description;
        Route = route;
        Match = match;
        IsHidden = isHidden;
        OrderIndex = orderIndex;
    }

    /// <summary>
    /// Visibility for UI. For example, NavMenu
    /// </summary>
    public bool IsHidden { get; }

    /// <summary>
    /// Order index sorting operations
    /// </summary>
    public int OrderIndex { get; }

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
