using System;
using System.Collections.Generic;
using Xunit;

public class EqualExample
{
    [Fact]
    public void EqualStringIgnoreCase()
    {
        string expected = "TestString";
        string actual = "teststring";

        Assert.False(actual == expected);
        Assert.NotEqual(expected, actual);
        Assert.Equal(expected, actual, StringComparer.CurrentCultureIgnoreCase);
    }

    public class DateComparer : IComparer<DateTime>
    {
        public int Compare(DateTime lhs,
                           DateTime rhs)
        {
            if (lhs.Year != rhs.Year)
                return lhs.Year - rhs.Year;

            if (lhs.Month != rhs.Month)
                return lhs.Month - rhs.Month;

            if (lhs.Day != rhs.Day)
                return lhs.Day - rhs.Day;

            return 0;
        }
    }

    [Fact]
    public void DateShouldBeEqualEvenThoughTimesAreDifferent()
    {
        DateTime firstTime = DateTime.Now.Date;
        DateTime later = firstTime.AddMinutes(90);

        Assert.NotEqual(firstTime, later);
        Assert.Equal(firstTime, later, new DateComparer());
    }
}