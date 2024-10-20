using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yart.Test;
public class ResultOfTTests
{
    [Fact]
    public void OkResultsAreSuccessful()
    {
        var success = Ok<object>(new object());

        Assert.True(success.IsSuccessful);
        Assert.False(success.IsFailure);
    }

    [Fact]
    public void CanCastUntypedFailures()
    {
        Result<object> failure = Failure();

        Assert.True(failure.IsFailure);
        Assert.False(failure.IsSuccessful);
    }

    [Fact]
    public void CastingUntypedSuccessesThrows()
    {
        var untypedSuccess = Ok();

        Result<object> Act() => untypedSuccess;

        Assert.Throws<InvalidCastException>(() =>
        {
            Act();
        });
    }

    [Fact]
    public void AccessingErrorOnSuccessThrows()
    {
        var success = Ok(new object());

        Error? Act() => success.Error;

        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void CanImplicitlyCastErrorToResult()
    {
        var error = new Error("Error message");

        Result result = error;

        Assert.True(result.IsFailure);
        Assert.Same(error, result.Error);
    }

    [Fact]
    public void CanMatchResults()
    {
        var result = Ok(new object());

        var resultCode = result.Match(
            (_) => 1,
            (error) => 2);

        Assert.Equal(1, resultCode);
    }

    [Fact]
    public async Task CanConvertResultToTask()
    {
        var result = Ok(new object());

        Task<Result<object>> Act() => result;

        var resultTask = Act();
        Assert.True(resultTask.IsCompleted);

        var resultValue = await resultTask;
        Assert.True(resultValue.IsSuccessful);
    }

    [Fact]
    public async Task CanConvertResultToValueTask()
    {
        var result = Ok(new object());

        ValueTask<Result<object>> Act() => result;


        var resultTask = Act();
        Assert.True(resultTask.IsCompleted);

        var resultValue = await resultTask;
        Assert.True(resultValue.IsSuccessful);
    }
}
