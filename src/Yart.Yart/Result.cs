using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yart.Yart;

/// <summary>
/// Represents the outcome of an operation
/// </summary>
/// <remarks>
/// <para><see cref="Result"/> is a type that wraps the outcome of an operation. A result can be either a success or a failure.</para>
/// <para>The <see cref="Result"/> type does not expose its constructor. Instead, use the <see cref="Ok"/> and <see cref="Failure"/> methods to create instances.</para>
/// </remarks>
public readonly struct Result
{
    /// <summary>
    /// Creates a successful result
    /// </summary>
    /// <returns>A successful <see cref="Result"/></returns>
    public static Result Ok() => new(isSuccessful: true);

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    /// <typeparam name="T">The result's value</typeparam>
    /// <param name="value">The value of the result</param>
    /// <returns>A <see cref="Result{T}"/> containing the provided result</returns>
    public static Result<T> Ok<T>(T value) => new(isSuccessful: true, value: value);

    /// <summary>
    /// Creates a failure result
    /// </summary>
    /// <param name="error">An optional error for the result</param>
    /// <returns>A failure <see cref="Result"/></returns>
    public static Result Failure(Error? error = default) => new(isSuccessful: false, error: error);

    private readonly bool _isSuccessful;
    private readonly Error? _error;

    private Result(bool isSuccessful, Error? error = default)
    {
        _isSuccessful = isSuccessful;
        _error = error;
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
    /// The error for a failure result. May be <see langword="null"/>
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is not a failure.</exception>
    public Error? Error => !_isSuccessful
        ? _error
        : throw new InvalidOperationException();

    /// <summary>
    /// Calls a delegate depending on the result's type
    /// </summary>
    /// <param name="successAction">A delegate called when the result is a success</param>
    /// <param name="failureAction">A delegate called when the result is a failure</param>
    public void Match(Action successAction, Action<Error?> failureAction)
    {
        if (_isSuccessful)
        {
            successAction();
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
    public TReturn Match<TReturn>(Func<TReturn> successFunc, Func<Error?, TReturn> failureFunc) =>
        _isSuccessful
            ? successFunc()
            : failureFunc(_error);

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a <see cref="Result"/>
    /// </summary>
    /// <param name="error">The error to convert</param>
    public static implicit operator Result(Error error) => new(isSuccessful: false, error);

    /// <summary>
    /// Converts a <see cref="Result"/> to a <see cref="Task{Result}"/>
    /// </summary>
    /// <param name="result">The result to wrap in a <see cref="Task{Result}"/></param>
    public static implicit operator Task<Result>(Result result) => Task.FromResult(result);

    /// <summary>
    /// Wraps a <see cref="Result" /> in a <see cref="ValueTask{Result}" />
    /// </summary>
    /// <param name="result">A <see cref="ValueTask{Result}" /> containing a result</param>
    public static implicit operator ValueTask<Result>(Result result) => new(result);
}
