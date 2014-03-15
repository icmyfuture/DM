using System;
using System.Globalization;

namespace DM.Web.SL.Common.Core.DragDrop
{
    /// <summary>
    ///   Provides core Guard utility methods for common parameter validation.
    /// </summary>
    /// <remarks>
    ///   Argument exceptions always return the original argument value to allow chaining.
    ///   <example>
    ///     <code>
    ///       public class BaseObject
    ///       {
    ///       public BaseObject(object value)
    ///       {
    ///       this.value = value;
    ///       }
    ///       }
    ///   
    ///       // --------------------------------------------------------------
    ///       // Define a descendent class that requires value to be a non-null 
    ///       // reference using a guard condition in the call to base. 
    ///       // --------------------------------------------------------------
    ///   
    ///       public class DescendentObject : BaseObject
    ///       {
    ///       public DescendentObject(object value)
    ///       : base(Guard.ArgumentNull("value", value))
    ///       {
    ///       }
    ///       } 
    ///     </code>
    ///   </example>
    /// </remarks>
    public static class Guard
    {
        /// <summary>
        ///   Guards against arguments that are null references.
        /// </summary>
        /// <param name = "argumentName">Name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <returns>The original <paramref name = "argumentValue" /> provided.</returns>
        /// <exception cref = "ArgumentNullException">If <paramref name = "argumentValue" /> is <c>null</c>.</exception>
        public static Object ArgumentNull(string argumentName, object argumentValue)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            return argumentValue;
        }

        /// <summary>
        ///   Guards against arguments that are null references.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <returns>The original <paramref name = "argumentValue" /> provided.</returns>
        /// <exception cref = "ArgumentNullException">If <paramref name = "argumentValue" /> is <c>null</c>.</exception>
        public static TArg ArgumentNull<TArg>(string argumentName, TArg argumentValue)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            return argumentValue;
        }

        /// <summary>
        ///   Guards against arguments that are not of the expected <typeparamref name = "TArg" />
        /// </summary>
        /// <typeparam name = "TArg">The expected type of the argument, must be a class type.</typeparam>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <returns>
        ///   The original <paramref name = "argumentValue" /> provided.
        /// </returns>
        /// <remarks>
        ///   This method only works with class types as support for additional types would significantly hinder performance.
        /// </remarks>
        /// <exception cref = "ArgumentException">If <paramref name = "argumentValue" /> is not of the desired type.</exception>
        public static TArg ArgumentWrongType<TArg>(string argumentName, object argumentValue) where TArg:class
        {
            // null may be allowed
            if (argumentValue == null)
            {
                return null;
            }

            // cast value
            var arg = argumentValue as TArg;
            if (arg == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Value must be of type {0}.", typeof (TArg).FullName), argumentName);
            }
            return arg;
        }

        /// <summary>
        ///   Guards against arguments that are a null reference or not of the expected <typeparamref name = "TArg" />
        /// </summary>
        /// <typeparam name = "TArg">The expected type of the argument, must be a class type.</typeparam>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <returns>
        ///   The original <paramref name = "argumentValue" /> provided.
        /// </returns>
        /// <remarks>
        ///   This method only works with class types as support for additional types would significantly hinder performance.
        /// </remarks>
        /// <exception cref = "ArgumentException">If <paramref name = "argumentValue" /> is not of the desired type.</exception>
        public static TArg ArgumentNullOrWrongType<TArg>(string argumentName, object argumentValue) where TArg:class
        {
            ArgumentNull(argumentName, argumentValue);
            return ArgumentWrongType<TArg>(argumentName, argumentValue);
        }

        /// <summary>
        ///   Guards against type arguments that cannot be assigned to the expected <paramref name = "targetType" />.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "targetType">The target type the argument must be assignable as.</param>
        /// <param name = "argumentValue">The argument value.</param>
        /// <exception cref = "ArgumentException">If <paramref name = "argumentValue" /> is not assignable as the desired type.</exception>
        public static void ArgumentNotAssignableAs(string argumentName, Type argumentValue, Type targetType)
        {
            if (!targetType.IsAssignableFrom(argumentValue))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Value must be assignable as type {0:s}.", targetType.FullName), argumentName);
            }
        }

        private static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(
            string argumentName, object actualValue, string message)
        {
            return new ArgumentOutOfRangeException(argumentName,
#if !SILVERLIGHT
					actualValue,
#endif
                                                   message
                );
        }

        #region String Guards

        /// <summary>
        ///   Guards against string arguments that are a null reference or empty.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <returns>
        ///   The original <paramref name = "argumentValue" /> provided.
        /// </returns>
        /// <exception cref = "ArgumentNullException">If <paramref name = "argumentValue" /> is <c>null</c>.</exception>
        /// <exception cref = "ArgumentException">If <paramref name = "argumentValue" /> equals <c>string.Empty</c>.</exception>
        public static String ArgumentNullOrEmpty(string argumentName, string argumentValue)
        {
            ArgumentNull(argumentName, argumentValue);
            if (argumentValue.Length == 0)
            {
                throw new ArgumentException("Value cannot be empty.", argumentName);
            }
            return argumentValue;
        }

        /// <summary>
        ///   Guards against string arguments that contain illegal characters.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "illegalCharacters">The illegal characters to guard against.</param>
        /// <exception cref = "ArgumentException">If <paramref name = "argumentValue" /> contains characters in <paramref name = "illegalCharacters" />.</exception>
        public static string ArgumentContains(string argumentName, string argumentValue, char[] illegalCharacters)
        {
            if (argumentValue != null && argumentValue.IndexOfAny(illegalCharacters) != -1)
            {
                throw new ArgumentException("Value contains illegal characters.", argumentName);
            }
            return argumentValue;
        }

        /// <summary>
        ///   Guards against string arguments that contain illegal characters.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "illegalCharacters">The illegal characters to guard against.</param>
        /// <exception cref = "ArgumentException">If <paramref name = "argumentValue" /> contains characters in <paramref name = "illegalCharacters" />.</exception>
        /// <exception cref = "ArgumentNullException">If <paramref name = "argumentValue" /> is null.</exception>
        public static string ArgumentNullOrContains(string argumentName, string argumentValue, char[] illegalCharacters)
        {
            ArgumentNull("argumentValue", argumentValue);
            if (argumentValue.IndexOfAny(illegalCharacters) != -1)
            {
                throw new ArgumentException("Value contains illegal characters.", argumentName);
            }
            return argumentValue;
        }

        #endregion

        #region Range Guards

        /// <summary>
        ///   Guard against arguments that are below the specified <paramref name = "lowerBound" />.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "lowerBound">The value the argument must not be less than.</param>
        /// <exception cref = "ArgumentOutOfRangeException">If <paramref name = "argumentValue" /> is less than <paramref name = "lowerBound" /></exception>
        public static int ArgumentBelowLowerBound(string argumentName, int argumentValue, int lowerBound)
        {
            if (argumentValue < lowerBound)
            {
                throw CreateArgumentOutOfRangeException(argumentName, argumentValue,
                                                        string.Format(CultureInfo.CurrentCulture, "Value must be above {0}.", lowerBound));
            }
            return argumentValue;
        }

        /// <summary>
        ///   Guard against arguments that are above the specified <paramref name = "upperBound" />.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "upperBound">The value the argument must not be greater than.</param>
        /// <exception cref = "ArgumentOutOfRangeException">If <paramref name = "argumentValue" /> is greater than <paramref name = "upperBound" /></exception>
        public static int ArgumentAboveUpperBound(string argumentName, int argumentValue, int upperBound)
        {
            if (argumentValue > upperBound)
            {
                throw CreateArgumentOutOfRangeException(argumentName, argumentValue,
                                                        string.Format(CultureInfo.CurrentCulture, "Value must be below {0}.", upperBound));
            }
            return argumentValue;
        }

        /// <summary>
        ///   Checks if <paramref name = "argumentValue" /> is lower than <paramref name = "lowerBound" /> or
        ///   higher than <paramref name = "upperBound" />, and throws an exception if so.
        /// </summary>
        /// <param name = "argumentName">Name of the argument.</param>
        /// <param name = "argumentValue">The argument value.</param>
        /// <param name = "lowerBound">The lower bound to check against.</param>
        /// <param name = "upperBound">The upper bound to check against.</param>
        public static int ArgumentOutOfRange(string argumentName, int argumentValue, int lowerBound, int upperBound)
        {
            ArgumentBelowLowerBound(argumentName, argumentValue, lowerBound);
            ArgumentAboveUpperBound(argumentName, argumentValue, upperBound);
            return argumentValue;
        }

        /// <summary>
        ///   Guard against arguments that are below the specified <paramref name = "lowerBound" />.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "lowerBound">The value the argument must not be less than.</param>
        /// <exception cref = "ArgumentOutOfRangeException">If <paramref name = "argumentValue" /> is less than <paramref name = "lowerBound" /></exception>
        public static long ArgumentBelowLowerBound(string argumentName, long argumentValue, long lowerBound)
        {
            if (argumentValue < lowerBound)
            {
                throw CreateArgumentOutOfRangeException(argumentName, argumentValue,
                                                        string.Format(CultureInfo.CurrentCulture, "Value must be above {0}.", lowerBound));
            }
            return argumentValue;
        }

        /// <summary>
        ///   Guard against arguments that are above the specified <paramref name = "upperBound" />.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "upperBound">The value the argument must not be greater than.</param>
        /// <exception cref = "ArgumentOutOfRangeException">If <paramref name = "argumentValue" /> is greater than <paramref name = "upperBound" /></exception>
        public static long ArgumentAboveUpperBound(string argumentName, long argumentValue, long upperBound)
        {
            if (argumentValue > upperBound)
            {
                throw CreateArgumentOutOfRangeException(
                    argumentName, argumentValue,
                    string.Format(CultureInfo.CurrentCulture, "Value must be below {0}.", upperBound));
            }
            return argumentValue;
        }

        /// <summary>
        ///   Checks if <paramref name = "argumentValue" /> is lower than <paramref name = "lowerBound" /> or
        ///   higher than <paramref name = "upperBound" />, and throws an exception if so.
        /// </summary>
        /// <param name = "argumentName">Name of the argument.</param>
        /// <param name = "argumentValue">The argument value.</param>
        /// <param name = "lowerBound">The lower bound to check against.</param>
        /// <param name = "upperBound">The upper bound to check against.</param>
        public static long ArgumentOutOfRange(string argumentName, long argumentValue, long lowerBound, long upperBound)
        {
            ArgumentBelowLowerBound(argumentName, argumentValue, lowerBound);
            ArgumentAboveUpperBound(argumentName, argumentValue, upperBound);
            return argumentValue;
        }

        /// <summary>
        ///   Guard against arguments that are below the specified <paramref name = "lowerBound" />.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "lowerBound">The value the argument must not be less than.</param>
        /// <exception cref = "ArgumentOutOfRangeException">If <paramref name = "argumentValue" /> is less than <paramref name = "lowerBound" /></exception>
        public static double ArgumentBelowLowerBound(string argumentName, double argumentValue, double lowerBound)
        {
            if (argumentValue < lowerBound)
            {
                throw CreateArgumentOutOfRangeException(
                    argumentName, argumentValue,
                    string.Format(CultureInfo.CurrentCulture, "Value must be above {0}.", lowerBound));
            }
            return argumentValue;
        }

        /// <summary>
        ///   Guard against arguments that are above the specified <paramref name = "upperBound" />.
        /// </summary>
        /// <param name = "argumentName">The name of the argument being checked, used to format exception messages.</param>
        /// <param name = "argumentValue">The value of the argument being checked.</param>
        /// <param name = "upperBound">The value the argument must not be greater than.</param>
        /// <exception cref = "ArgumentOutOfRangeException">If <paramref name = "argumentValue" /> is greater than <paramref name = "upperBound" /></exception>
        public static double ArgumentAboveUpperBound(string argumentName, double argumentValue, double upperBound)
        {
            if (argumentValue > upperBound)
            {
                throw CreateArgumentOutOfRangeException(
                    argumentName, argumentValue,
                    string.Format(CultureInfo.CurrentCulture, "Value must be below {0}.", upperBound));
            }
            return argumentValue;
        }

        /// <summary>
        ///   Checks if <paramref name = "argumentValue" /> is lower than <paramref name = "lowerBound" /> or
        ///   higher than <paramref name = "upperBound" />, and throws an exception if so.
        /// </summary>
        /// <param name = "argumentName">Name of the argument.</param>
        /// <param name = "argumentValue">The argument value.</param>
        /// <param name = "lowerBound">The lower bound to check against.</param>
        /// <param name = "upperBound">The upper bound to check against.</param>
        public static double ArgumentOutOfRange(
            string argumentName, double argumentValue, double lowerBound, double upperBound)
        {
            ArgumentBelowLowerBound(argumentName, argumentValue, lowerBound);
            ArgumentAboveUpperBound(argumentName, argumentValue, upperBound);
            return argumentValue;
        }

        #endregion
    }
}