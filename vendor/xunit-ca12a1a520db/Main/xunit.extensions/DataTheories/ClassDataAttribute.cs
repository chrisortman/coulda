﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Xunit.Extensions
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from a class
    /// which must implement IEnumerable&lt;object[]&gt;.
    /// </summary>
    public class ClassDataAttribute : DataAttribute
    {
        readonly Type @class;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDataAttribute"/> class.
        /// </summary>
        /// <param name="class">The class that provides the data.</param>
        public ClassDataAttribute(Type @class)
        {
            this.@class = @class;
        }

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            return (IEnumerable<object[]>)Activator.CreateInstance(@class);
        }
    }
}