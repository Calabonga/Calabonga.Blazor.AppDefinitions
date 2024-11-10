using Microsoft.AspNetCore.Components.Routing;

namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Base Blazor modules class for metadata. It should be used for modules creation.
/// </summary>
public abstract class BlazorModule : IBlazorModule
{
    /// <summary>
    /// Visibility for UI. For example, NavMenu
    /// </summary>
    public virtual bool IsHidden => false;

    /// <summary>
    /// Order index sorting operations
    /// </summary>
    public virtual int OrderIndex => 0;

    /// <summary>
    /// Module title
    /// </summary>
    public abstract string Title { get; }

    /// <summary>
    /// Brief module description
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Navigation Route 
    /// </summary>
    public abstract string Route { get; }

    /// <summary>
    /// NavLink type match
    /// </summary>
    public virtual NavLinkMatch Match => NavLinkMatch.Prefix;
}
