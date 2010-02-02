using System;
using Xunit;

public class SampleFacts
{
    [Fact]
    public void PassingFact()
    {
        Assert.True(true);
    }
    
    [Fact]
    public void FailingFact()
    {
        Assert.True(false);
    }
    
    [Fact(Skip="This fact is skipped")]
    public void SkippedFact()
    {
        Assert.True(false);
    }
    
    [Fact, Trait("Name", "Value")]
    public void FactWithTraits()
    {
        Assert.True(true);
    }
}

public class FixtureDataFailure : IUseFixture<FailingFixture>
{
    [Fact]
    public void PassingFact() { }

    public void SetFixture(FailingFixture f) { }
}

public class FailingFixture
{
    public FailingFixture()
    {
        throw new Exception();
    }
}

public class FailingConstructor
{
    public FailingConstructor()
    {
        throw new Exception("The class fails before any facts can run");
    }
    
    [Fact]
    public void PassingFact()
    {
        Assert.True(true);
    }
}

public class FailingDispose : IDisposable
{
    public void Dispose()
    {
        throw new Exception("The class fails after the facts run");
    }
    
    [Fact]
    public void PassingFact()
    {
        Assert.True(true);
    }
}