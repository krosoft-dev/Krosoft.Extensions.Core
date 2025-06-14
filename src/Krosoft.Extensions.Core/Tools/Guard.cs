﻿using System.Security;
using Krosoft.Extensions.Core.Attributes;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tools;

[SecuritySafeCritical]
public static class Guard
{
    [SecuritySafeCritical]
    public static void IsNotNull(string argumentName, [ValidatedNotNull] object? value)
    {
        if (value == null)
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' n'est pas renseignée.");
        }
    }

    [SecuritySafeCritical]
    public static void IsNotEmpty<T>(string argumentName, [ValidatedNotNull] ReadOnlySpan<T> value)
    {
        if (value.IsEmpty)
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' est vide.");
        }
    }

    [SecuritySafeCritical]
    public static void IsNotNullOrWhiteSpace(string argumentName, [ValidatedNotNull] string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' est vide ou non renseignée.");
        }
    }

    [SecuritySafeCritical]
    public static void IsNotNullOrEmpty(string argumentName, [ValidatedNotNull] string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' est vide ou non renseignée.");
        }
    }

    [SecuritySafeCritical]
    public static void IsNotNullOrEmpty<T>(string argumentName, [ValidatedNotNull] T[]? value)
    {
        if (value == null || value.Length == 0)
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' est vide ou non renseignée.");
        }
    }

    [SecuritySafeCritical]
    public static void IsNotNullOrEmpty<T>(string argumentName, [ValidatedNotNull] List<T>? value)
    {
        if (value == null || value.Count == 0)
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' est vide ou non renseignée.");
        }
    }

    [SecuritySafeCritical]
    public static void IsNotNullOrEmpty(string argumentName, [ValidatedNotNull] long? value)
    {
        if (value == null || value == 0)
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' est vide ou non renseignée.");
        }
    }
}