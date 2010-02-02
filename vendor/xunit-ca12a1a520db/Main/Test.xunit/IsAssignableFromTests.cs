using System;
using Xunit;
using Xunit.Sdk;

public class IsAssignableFromTests
{
    [Fact]
    public void IsAssignableFrom_SameType()
    {
        InvalidCastException expected = new InvalidCastException();
        Assert.IsAssignableFrom(typeof(InvalidCastException), expected);
        Assert.IsAssignableFrom<InvalidCastException>(expected);
    }

    [Fact]
    public void IsAssignableFrom_BaseType()
    {
        InvalidCastException expected = new InvalidCastException();
        Assert.IsAssignableFrom(typeof(Exception), expected);
        Assert.IsAssignableFrom<Exception>(expected);
    }

    [Fact]
    public void IsAssignableFromReturnsCastObject()
    {
        InvalidCastException expected = new InvalidCastException();
        InvalidCastException actual = Assert.IsAssignableFrom<InvalidCastException>(expected);
        Assert.Same(expected, actual);
    }

    [Fact]
    public void IsAssignableFromThrowsExceptionWhenWrongType()
    {
        AssertException exception =
            Assert.Throws<IsAssignableFromException>(
                () => Assert.IsAssignableFrom<InvalidCastException>(new InvalidOperationException()));

        Assert.Equal("Assert.IsAssignableFrom() Failure", exception.UserMessage);
    }

    [Fact]
    public void IsAssignableFromThrowsExceptionWhenPassedNull()
    {
        Assert.Throws<IsAssignableFromException>(() => Assert.IsAssignableFrom<object>(null));
    }
}