﻿namespace Calabonga.Blazor.AppDefinitions;

/// <summary>
/// Assemblies configuration exception
/// </summary>
public class ModulesConfigurationException : InvalidOperationException
{
    public ModulesConfigurationException(string? message) : base(message) { }

    public ModulesConfigurationException(string? message, Exception innerException) : base(message, innerException) { }
}
