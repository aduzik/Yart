using System;
using System.Collections.Generic;
using System.Text;

namespace Yart.Yart;

#pragma warning disable CA1716 // Allow the name "Error" to be used as a class name

/// <summary>
/// Represents an error for a failure <seealso cref="Result"/>
/// </summary>
public class Error
{
    /// <summary>
    /// An optional error message
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// Creates a new error with the given message
    /// </summary>
    /// <param name="message">An optional message for the error</param>
    public Error(string? message = default)
    {
        Message = message;
    }
}

#pragma warning restore CA1716