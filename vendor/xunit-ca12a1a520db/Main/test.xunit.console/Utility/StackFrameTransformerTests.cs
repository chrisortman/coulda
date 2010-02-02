﻿using Xunit;
using Xunit.ConsoleClient;

public class StackFrameTransformerTests
{
    [Fact]
    public void NullStackFrame()
    {
        Assert.Null(StackFrameTransformer.TransformFrame(null));
    }

    [Fact]
    public void StackFrameWithFile_Windows()
    {
        string input = @"   at SampleTests.SampleTest() in C:\Users\Brad\sample\SampleTests.cs:line 15";
        string expected = @"   C:\Users\Brad\sample\SampleTests.cs(15,0): at SampleTests.SampleTest()";

        string result = StackFrameTransformer.TransformFrame(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StackFrameWithFile_Mono()
    {
        string input = @"  at SampleTests.SampleTest () [0x00000] in /Users/brad/sample/SampleTests.cs:15";
        string expected = @"  /Users/brad/sample/SampleTests.cs(15,0): at SampleTests.SampleTest () [0x00000]";

        string result = StackFrameTransformer.TransformFrame(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StackFrameWithoutFile_Windows()
    {
        string input = @"   at SampleTests.SampleTest()";

        string result = StackFrameTransformer.TransformFrame(input);

        Assert.Equal("" + input, result);
    }

    [Fact]
    public void StackFrameWithoutFile_Mono()
    {
        string input = @"  at SampleTests.SampleTest () [0x00000]";

        string result = StackFrameTransformer.TransformFrame(input);

        Assert.Equal("" + input, result);
    }

    [Fact]
    public void NullStackTrace()
    {
        Assert.Null(StackFrameTransformer.TransformStack(null));
    }

    [Fact]
    public void StackTrace_Windows()
    {
        string input = @"
   at SampleTests.ThrowException() in C:\Users\Brad\sample\SampleTests.cs:line 42
   at SampleTests.SampleTest() in C:\Users\Brad\sample\SampleTests.cs:line 15
   at Sample.Main()
";
        string expected = @"
   C:\Users\Brad\sample\SampleTests.cs(42,0): at SampleTests.ThrowException()
   C:\Users\Brad\sample\SampleTests.cs(15,0): at SampleTests.SampleTest()
   at Sample.Main()
";

        string result = StackFrameTransformer.TransformStack(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void StackTrace_Mono()
    {
        string input = @"
  at SampleTests.ThrowException () [0x00000] in /Users/brad/sample/SampleTests.cs:42
  at SampleTests.SampleTest () [0x00000] in /Users/brad/sample/SampleTests.cs:15
  at Sample.Main () [0x00000]
";
        string expected = @"
  /Users/brad/sample/SampleTests.cs(42,0): at SampleTests.ThrowException () [0x00000]
  /Users/brad/sample/SampleTests.cs(15,0): at SampleTests.SampleTest () [0x00000]
  at Sample.Main () [0x00000]
";

        string result = StackFrameTransformer.TransformStack(input);

        Assert.Equal(expected, result);
    }
}