using System;
using System.IO;
using System.Reflection;

namespace Xunit.Extensions
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming a Microsoft Excel (.xls) spreadsheet.
    /// </summary>
    public class ExcelDataAttribute : OleDbDataAttribute
    {
        const string connectionTemplate =
            "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties=Excel 8.0";

        /// <summary>
        /// Creates a new instance of <see cref="ExcelDataAttribute"/>.
        /// </summary>
        /// <param name="filename">The filename of the XLS spreadsheet file; if the filename provided
        /// is relative, then it is relative to the location of xunit.extensions.dll.</param>
        /// <param name="selectStatement">The SELECT statement that returns the data for the theory</param>
        public ExcelDataAttribute(string filename, string selectStatement)
            : base(string.Format(connectionTemplate, GetFullFilename(filename)), selectStatement) { }

        static string GetFullFilename(string filename)
        {
            string executable = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(executable), filename));
        }

        /// <inheritdoc/>
        protected override object ConvertParameter(object parameter, Type parameterType)
        {
            if ((parameter is double || parameter is float) &&
                (parameterType == typeof(int) || parameterType == typeof(int?)))
            {
                int intValue;
                string floatValueAsString = parameter.ToString();

                if (Int32.TryParse(floatValueAsString, out intValue))
                    return intValue;
            }

            return base.ConvertParameter(parameter, parameterType);
        }
    }
}