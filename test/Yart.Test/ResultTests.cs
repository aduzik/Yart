using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yart.Yart;

namespace Yart.Test;

public class ResultTests
{
    [Fact]
    public void OkResultsAreSuccessful()
    {
        var success = Ok();

        Assert.True(success.IsSuccessful);
        Assert.False(success.IsFailure);
    }

    [Fact]
    public void FailResultsAreNotSuccessful()
    {
        var failure = Failure();

        Assert.True(failure.IsFailure);
        Assert.False(failure.IsSuccessful);
    }

    [Fact]
    public void FailuresCanHaveErrors()
    {
        var error = new Error("error message");
        
        var failure = Failure(error);

        Assert.True(failure.IsFailure);
        Assert.Same(error, failure.Error);
    }

    [Fact]
    public void AccessingErrorOnSuccessThrows()
    {
        var success = Ok();

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
        var result = Ok();

        var resultCode = result.Match(
            () => 1,
            (error) => 2);

        Assert.Equal(1, resultCode);
    }

    [Fact]
    public async Task CanConvertResultToTask()
    {
        var result = Ok();

        Task<Result> Act() => result;

        var resultTask = Act();
        Assert.True(resultTask.IsCompletedSuccessfully);

        var resultValue = await resultTask;
        Assert.True(resultValue.IsSuccessful);
    }

    [Fact]
    public async Task CanConvertResultToValueTask()
    {
        var result = Ok();

        ValueTask<Result> Act() => result;

        var resultTask = Act();
        Assert.True(resultTask.IsCompletedSuccessfully);

        var resultValue = await resultTask;
        Assert.True(resultValue.IsSuccessful);
    }

}
