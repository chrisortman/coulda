using System;
using System.Globalization;
using System.Reflection;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    /// <summary>
    /// Represents a single invocation of a data theory test method.
    /// </summary>
    public class TheoryCommand : TestCommand
    {
        /// <summary>
        /// Creates a new instance of <see cref="TheoryCommand"/>.
        /// </summary>
        /// <param name="testMethod">The method under test</param>
        /// <param name="parameters">The parameters to be passed to the test method</param>
        public TheoryCommand(IMethodInfo testMethod, object[] parameters)
            : base(testMethod, MethodUtility.GetDisplayName(testMethod), MethodUtility.GetTimeoutParameter(testMethod))
        {
            Parameters = parameters ?? new object[0];

            string[] displayValues = new string[Parameters.Length];

            for (int idx = 0; idx < Parameters.Length; idx++)
                displayValues[idx] = ParameterToDisplayValue(Parameters[idx]);

            DisplayName = String.Format("{0}({1})", DisplayName, string.Join(", ", displayValues));
        }

        /// <summary>
        /// Gets the parameter values that are passed to the test method.
        /// </summary>
        public object[] Parameters { get; protected set; }

        /// <inheritdoc/>
        public override MethodResult Execute(object testClass)
        {
            try
            {
                ParameterInfo[] parameterInfos = testMethod.MethodInfo.GetParameters();
                if (parameterInfos.Length != Parameters.Length)
                    throw new InvalidOperationException(string.Format("Expected {0} parameters, got {1} parameters", parameterInfos.Length, Parameters.Length));

                testMethod.Invoke(testClass, Parameters);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionUtility.RethrowWithNoStackTraceLoss(ex.InnerException);
            }

            return new PassedResult(testMethod, DisplayName);
        }

        static string ParameterToDisplayValue(object parameter)
        {
            if (parameter == null)
                return "null";

            if (parameter is String)
            {
                string stringParameter = (string)parameter;

                if (stringParameter.Length > 50)
                    return "\"" + stringParameter.Substring(0, 50) + "\"...";

                return "\"" + stringParameter + "\"";
            }

            return Convert.ToString(parameter, CultureInfo.InvariantCulture);
        }
    }
}