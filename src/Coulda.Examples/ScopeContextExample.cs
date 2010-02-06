namespace Coulda.Examples
{
    using Test;

    using Xunit;

    public class ScopeContextExample : CouldaBase
    {
        [CouldaTestAttribute]
        public CouldaTestContext Nested_context_scope()
        {
            int i = 0;
            return Context("A test with nested contexts", ctx =>
            {
                ctx.Should("not retain information from run to run",() =>
                {
                    i = i + 1;
                    Assert.Equal(1,i);
                });

                ctx.Should("still be 1 after incrementing",() =>
                {
                    i = i + 1;
                    Assert.Equal(1,i);
                });
            });
        }

        [CouldaTestAttribute]
        public CouldaTestContext Context_with_setup()
        {
            var i = 0;
            return Context("A test with some setup code", ctx =>
            {
                string s = "hello";
                i = 5;

                ctx.Should("be good to go", () =>
                {
                    i = i*2;
                    Assert.Equal("hello", s);
                    Assert.Equal(10, i);
                });

                ctx.Should("not mess anything up", () =>
                {
                    Assert.Equal(5, i);
                });
            });
        }
    }
}