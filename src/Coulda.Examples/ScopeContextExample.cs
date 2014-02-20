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

                ctx.Context("and a nested context",nc =>
                {
                    nc.Before(() =>
                    {
                        i = i + 1;
                    });
                    
                    nc.Should("still be predictable",() =>
                    {
                        Assert.Equal(6,i);
                    });
                });

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

        [CouldaTestAttribute]
        public CouldaTestContext Nested_contexts_with_setups()
        {
            int i = 5;
            return Context("A parent context with a before block", ctx =>
            {
                ctx.Before(() => i = i + 1 /* 6 */);

                ctx.Context("A nested context within the parent", nc2 =>
                {
                    nc2.Before(() => i = i + 1 /*7*/);

                    nc2.Context("A nested context with in the nested context", nc3 =>
                    {
                        nc3.Before(() => i = i + 1 /* 8 */);

                        nc3.Should("have invoked all the before hooks", () => Assert.Equal(8, i));
                    });
                });
            });
        }
    }
}