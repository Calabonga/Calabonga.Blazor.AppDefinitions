namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Information about <see cref="IAppDefinition"/>
/// </summary>
/// <param name="Definition"></param>
/// <param name="AssemblyName"></param>
/// <param name="Enabled"></param>
/// <param name="Exported"></param>
internal sealed record AppDefinitionItem(IAppDefinition Definition, string AssemblyName, bool Enabled, bool Exported);
