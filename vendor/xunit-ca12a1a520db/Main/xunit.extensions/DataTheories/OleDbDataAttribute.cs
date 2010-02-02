using System.Data;
using System.Data.OleDb;

namespace Xunit.Extensions
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from an OLEDB connection.
    /// </summary>
    public class OleDbDataAttribute : DataAdapterDataAttribute
    {
        readonly string connectionString;
        readonly string selectStatement;

        /// <summary>
        /// Creates a new instance of <see cref="OleDbDataAttribute"/>.
        /// </summary>
        /// <param name="connectionString">The OLEDB connection string to the data</param>
        /// <param name="selectStatement">The SELECT statement used to return the data for the theory</param>
        public OleDbDataAttribute(string connectionString, string selectStatement)
        {
            this.connectionString = connectionString;
            this.selectStatement = selectStatement;
        }

        /// <inheritdoc/>
        protected override IDataAdapter DataAdapter
        {
            get { return new OleDbDataAdapter(selectStatement, connectionString); }
        }
    }
}