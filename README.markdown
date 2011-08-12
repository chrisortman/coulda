This is the readme for coulda

I want to write tests like this in C#

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
        
        
Something else to maybe brainstorm...

     public void BetterSyntax()
        {
            Describe("1.00", subject =>
            {
                subject.WhenUsedToCreateAmount().ShouldBe(1M);
                subject.WhenValidated().ShouldBe(true);
                subject.WhenModelBound().ShouldBe(new Amount(1M));

                //or

                WhenUsedToCreateAmount(subject).ShouldBe(1M);
                WhenValidated(subject).ShouldBe(true);
                WhenModelBound(subject).ShouldBe(new Amount(1M));
            })    
        }
        