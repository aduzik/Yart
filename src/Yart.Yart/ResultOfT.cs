using System;
using System.Collections.Generic;
using System.Text;

namespace Yart.Yart;

/// <summary>
/// Represents the outcome of an operation
/// </summary>
/// <remarks>
/// <para><see cref="Result{T}"/> is a type that wraps the outcome of an operation. A result can be either a success or a failure.</para>
/// <para>The <see cref="Result{T}"/> type does not expose its constructor. Instead, use the <see cref="Result.Ok{T}"/> and <see cref="Result.Failure"/> methods to create instances.</para>
/// </remarks>
public readonly struct Result<T>
{
    private readonly bool _isSuccessful;
    private readonly Error? _error;
    private readonly T? _value;
    internal Result(bool isSuccessful, Error? error = default, T? value = default)
    {
        _isSuccessful = isSuccessful;
        _error = error;
        _value = value;
    }

    /// <summary>
    /// <see langword="true"/> when the result is a success, <see langword="false" /> otherwise.
    /// </summary>
    public bool IsSuccessful => _isSuccessful;

    /// <summary>
    /// <see langword="true"/> when the result is a failure, <see langword="false" /> otherwise.
    /// </summary>
    public bool IsFailure => !_isSuccessful;

    /// <summary>
    /// The error for a failure result. May be <see langword="null" />
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is not a failure.</exception>
    public Error? Error =>
        !_isSuccessful
            ? _error
            : throw new InvalidOperationException();

    /// <summary>
    /// The value for a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is not a success.</exception>
    public T Value => 
        _isSuccessful
            ? _value!
            : throw new InvalidOperationException();

    /// <summary>
    /// Calls a delegate depending on the result's type
    /// </summary>
    /// <param name="successAction">A delegate called when the result is a success</param>
    /// <param name="failureAction">A delegate called when the result is a failure</param>
    public void Match(Action<T> successAction, Action<Error?> failureAction)
    {
        if (_isSuccessful)
        {
            successAction(_value!);
        }
        else
        {
            failureAction(_error);
        }
    }

    /// <summary>
    /// Calls a delegate depending on the result's type
    /// </summary>
    /// <typeparam name="TReturn">The delegate's return type</typeparam>
    /// <param name="successFunc">A delegate called when the result is a success</param>
    /// <param name="failureFunc">A delegate called when the result is a failure</param>
    /// <returns>The return value of the called delegate</returns>
    public TReturn Match<TReturn>(Func<T, TReturn> successFunc, Func<Error?, TReturn> failureFunc) =>
        _isSuccessful
            ? successFunc(_value!)
            : failureFunc(_error);

    /// <summary>
    /// Converts an untyped error result to a typed error result
    /// </summary>
    /// <param name="result">An untyped error result</param>
    /// <exception cref="InvalidCastException">Thrown if the result is not a failure</exception>
    public static implicit operator Result<T>(Result result) =>
        result.IsFailure
            ? new Result<T>(isSuccessful: false, result.Error)
            : throw new InvalidCastException();

    /// <summary>
    /// Converts a typed result into an untyped result
    /// </summary>
    /// <param name="result">An untyped result</param>
    public static implicit operator Result(Result<T> result) => 
        result._isSuccessful
            ? Result.Ok()
            : Result.Failure(result._error);
}
