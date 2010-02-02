namespace Coulda.Examples
{
    using Test;

    using Xunit;
   
    public class SimpleExample : CouldaBase
    {

        [CouldaTestAttribute]
        public CouldaTestContext A_simple_example()
        {
            return Context("that does something", ctx =>
            {
                ctx.NoSetup();
                ctx.Should("be able to assert", () =>
                {
                    Assert.True(true);
                });

            });
        }
    }
}