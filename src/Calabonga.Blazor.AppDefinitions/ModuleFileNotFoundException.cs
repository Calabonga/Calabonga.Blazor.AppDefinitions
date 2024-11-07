namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Module fine not find exception
/// </summary>
public class ModuleFileNotFoundException : FileNotFoundException
{
    public ModuleFileNotFoundException(string? message) : base(message) { }

    public ModuleFileNotFoundException(string? message, string fileName) : base(message, fileName) { }

    public ModuleFileNotFoundException(string? message, Exception innerException) : base(message, innerException) { }
}
