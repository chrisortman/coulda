namespace Coulda.Examples
{
    using System;

    using Test;

    public class ShouldChangeExample : CouldaBase
    {
        [CouldaTestAttribute]
        public CouldaTestContext Should_change_pass_test()
        {
            return Context("The should change macro", ctx =>
            {
                int number = 1;

                ctx.Before(() =>
                {
                    number = 5;
                });

                ctx.ShouldChange("the number", x => number).From(1).To(5);
            });
        }

        [CouldaTestAttribute]
        public CouldaTestContext Should_change_wrong_from_test()
        {
            return Context("The should change macro", ctx =>
            {
                int number = 1;

                ctx.Before(() =>
                {
                    number = 5;
                });

                ctx.ShouldChange("the number", x => number).From(1).To(5);
            });
        }

        [CouldaTestAttribute]
        public CouldaTestContext Should_change_wrong_to_test()
        {
            return Context("The should change macro", ctx =>
            {
                int number = 1;
                ctx.Before(() =>
                {
                    number = 5;
                });
                ctx.ShouldChange("the number", x => number).From(1).To(2);
            });
        }
    }
}