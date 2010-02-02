using System;

namespace Xunit
{
    /// <summary>
    /// Attribute used to decorate a test method with arbitrary name/value pairs ("traits").
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TraitAttribute : Attribute
    {
        readonly string name;
        readonly string value;

        /// <summary>
        /// Creates a new instance of the <see cref="TraitAttribute"/> class.
        /// </summary>
        /// <param name="name">The trait name</param>
        /// <param name="value">The trait value</param>
        public TraitAttribute(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Gets the trait name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <inheritdoc/>
        public override object TypeId
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the trait value.
        /// </summary>
        public string Value
        {
            get { return value; }
        }
    }
}