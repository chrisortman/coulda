namespace Coulda.Examples
{
    using Test;

    using Xunit;
   
    public class SimpleExample : CouldaBase
    {

        [CouldaTestAttribute]
        public CouldaTestContext A_simple_example()
        {
            return Context("My example test", ctx =>
            {
                ctx.NoSetup();
                ctx.Should("be able to assert", () =>
                {
                    Assert.True(true);
                });

            });
        }

       
    }

    public class FailingExample : CouldaBase
    {
        [CouldaTestAttribute]
        public CouldaTestContext A_failing_example()
        {
            return Context("A test with an assertion", ctx =>
            {
                ctx.Should("be able to fail", () =>
                {
                    Assert.True(false);
                });
            });
        }
    }
}