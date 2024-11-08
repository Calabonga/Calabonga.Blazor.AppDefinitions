using Microsoft.AspNetCore.Components.Routing;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Module marker for Blazor application 
/// </summary>
public interface IBlazorModule
{
    /// <summary>
    /// Module title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Brief module description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Navigation Route 
    /// </summary>
    string Route { get; }

    /// <summary>
    /// NavLink type match
    /// </summary>
    NavLinkMatch Match { get; }
}
