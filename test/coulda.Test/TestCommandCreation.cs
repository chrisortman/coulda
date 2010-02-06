namespace Coulda.Test
{
    using System.Linq;

    using Xunit;

    public class TestCommandCreation
    {
        [Fact]
        public void Should_be_able_to_list_the_tests_by_description()
        {
            var context = new CouldaTestContext("My context");
            context.Should("have a method",null);
            context.Should("have a second method",null);

            var testNames = context.GetTestDescriptions();
            Assert.Equal(2, testNames.Count());
            Assert.Contains<string>("My context should have a method", testNames);
            Assert.Contains<string>("My context should have a second method", testNames);

        }

        [Fact]
        public void Should_be_able_to_get_a_test_by_name()
        {
            bool ranShould = false;
            var context = new CouldaTestContext("My context");
            context.Should("have a method",null);
            context.Should("have a second method",() => ranShould = true);
            context.Should("have this one other method too",null);

            ShouldContext should = context.GetTestByDescription("My context should have a second method");
            Assert.False(ranShould);
            should.Should();
            Assert.True(ranShould);
        }

        [Fact]
        public void Should_be_able_to_nest_contexts()
        {
            var context = new CouldaTestContext("My context");
            context.Should("have a method",null);
            context.Should("have a second method",null);
            context.Context("that is nested", ctx =>
            {
                ctx.Should("have a should",null);
            });

            var testNames = context.GetTestDescriptions();
            Assert.Equal(3, testNames.Count());
            Assert.Contains("My context that is nested should have a should",testNames);
        }
    }

}