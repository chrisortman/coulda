namespace Coulda.Examples
{
    using Test;

    using Xunit;

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