using System;

namespace MessagingService.Extensions
{
    /// <summary>
    /// Defines methods used for parameters validation.
    /// </summary>
    public static class Preconditions
    {
        private static readonly string ArgumentExceptionDefaultMessage = new ArgumentException().Message;

        /// <summary>
        /// Checks that a reference isn't null. Throws ArgumentNullException if null.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="reference">Type instance.</param>
        /// <returns>The reference.</returns>
        /// <exception cref="ArgumentNullException">When reference is null.</exception>
        public static T CheckNotNull<T>([ValidatedNotNull] T reference)
            where T : class
            => CheckNotNull(reference, string.Empty, string.Empty);

        /// <summary>
        /// Checks that a reference isn't null. Throws ArgumentNullException if null.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="reference">Type instance.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <returns>The reference.</returns>
        /// <exception cref="ArgumentNullException">When reference is null.</exception>
        public static T CheckNotNull<T>([ValidatedNotNull] T reference, string paramName)
            where T : class
            => CheckNotNull(reference, paramName, string.Empty);

        /// <summary>
        /// Checks that a reference isn't null. Throws ArgumentNullException if null.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="reference">Type instance.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <param name="message">Exception message.</param>
        /// <returns>The reference.</returns>
        /// <exception cref="ArgumentNullException">When reference is null.</exception>
        public static T CheckNotNull<T>([ValidatedNotNull] T reference, string paramName, string message)
            where T : class
        {
            if (reference != null)
            {
                return reference;
            }

            throw string.IsNullOrEmpty(message)
                ? new ArgumentNullException(paramName)
                : new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws ArgumentException if the bool expression is false.
        /// </summary>
        /// <param name="expression">Expression to be checked.</param>
        /// <exception cref="ArgumentException">When expression is false.</exception>
        public static void CheckArgument(bool expression)
            => CheckArgument(expression, string.Empty, string.Empty);

        /// <summary>
        /// Throws ArgumentException if the bool expression is false.
        /// </summary>
        /// <param name="expression">Expression to be checked.</param>
        /// <param name="message">Exception message.</param>
        /// <exception cref="ArgumentException">When expression is false.</exception>
        public static void CheckArgument(bool expression, string message)
            => CheckArgument(expression, message, string.Empty);

        /// <summary>
        /// Throws ArgumentException if the bool expression is false.
        /// </summary>
        /// <param name="expression">Expression to be checked.</param>
        /// <param name="message">Exception message.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <exception cref="ArgumentException">When expression is false.</exception>
        public static void CheckArgument(bool expression, string message, string paramName)
        {
            if (expression)
            {
                return;
            }

            throw string.IsNullOrEmpty(message)
                ? new ArgumentException(ArgumentExceptionDefaultMessage, paramName)
                : new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Checks that an Enum is defined. Throws ArgumentOutOfRangeException is not.
        /// </summary>
        /// <typeparam name="T">Enum Type.</typeparam>
        /// <param name="value">Enum value.</param>
        /// <returns>Checked enum.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When enum is not defined.</exception>
        public static T CheckIsDefined<T>(T value)
        {
            Type enumType = typeof(T);
            if (!Enum.IsDefined(enumType, value))
            {
                throw new ArgumentOutOfRangeException($"{value:G} is not a valid value for {enumType.FullName}.");
            }

            return value;
        }

        /// <summary>
        /// This checks that the item is greater than or equal to the low value.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="item">Item to check.</param>
        /// <param name="low">Inclusive low value.</param>
        /// <returns>Checked item.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When item is not in range.</exception>
        public static T CheckRange<T>(T item, T low)
            where T : IComparable<T>
            => CheckRange(item, low, nameof(item));

        /// <summary>
        /// This checks that the item is greater than or equal to the low value.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="item">Item to check.</param>
        /// <param name="low">Inclusive low value.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <returns>Checked item.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When item is not in range.</exception>
        public static T CheckRange<T>(T item, T low, string paramName)
            where T : IComparable<T>
            => CheckRange(item, low, paramName, string.Empty);

        /// <summary>
        /// This checks that the item is greater than or equal to the low value.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="item">Item to check.</param>
        /// <param name="low">Inclusive low value.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <param name="message">Exception message.</param>
        /// <returns>Checked item.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When item is not in range.</exception>
        public static T CheckRange<T>(T item, T low, string paramName, string message)
            where T : IComparable<T>
        {
            if (item.CompareTo(low) < 0)
            {
                throw string.IsNullOrEmpty(message)
                    ? new ArgumentOutOfRangeException(paramName)
                    : new ArgumentOutOfRangeException(paramName, message);
            }

            return item;
        }

        /// <summary>
        /// This checks that the item is in the range [low, high).
        /// Throws ArgumentOutOfRangeException if out of range.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="item">Item to check.</param>
        /// <param name="low">Inclusive low value.</param>
        /// <param name="high">Exclusive high value.</param>
        /// <returns>Checked item.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When item is not in range.</exception>
        public static T CheckRange<T>(T item, T low, T high)
            where T : IComparable<T>
            => CheckRange(item, low, high, nameof(item));

        /// <summary>
        /// This checks that the item is in the range [low, high).
        /// Throws ArgumentOutOfRangeException if out of range.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="item">Item to check.</param>
        /// <param name="low">Inclusive low value.</param>
        /// <param name="high">Exclusive high value.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <returns>Checked item.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When item is not in range.</exception>
        public static T CheckRange<T>(T item, T low, T high, string paramName)
            where T : IComparable<T>
            => CheckRange(
                item,
                low,
                high,
                paramName,
                string.Empty);

        /// <summary>
        /// This checks that the item is in the range [low, high).
        /// Throws ArgumentOutOfRangeException if out of range.
        /// </summary>
        /// <typeparam name="T">Type to be checked.</typeparam>
        /// <param name="item">Item to check.</param>
        /// <param name="low">Inclusive low value.</param>
        /// <param name="high">Exclusive high value.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <param name="message">Exception message.</param>
        /// <returns>Checked item.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When item is not in range.</exception>
        public static T CheckRange<T>(
            T item,
            T low,
            T high,
            string paramName,
            string message)
            where T : IComparable<T>
        {
            if (item.CompareTo(low) < 0 || item.CompareTo(high) >= 0)
            {
                throw string.IsNullOrEmpty(message)
                    ? new ArgumentOutOfRangeException(paramName)
                    : new ArgumentOutOfRangeException(paramName, message);
            }

            return item;
        }

        /// <summary>
        /// Checks if the string is null or whitespace, and throws ArgumentException if it is.
        /// </summary>
        /// <param name="value">String.</param>
        /// <returns>Checked string.</returns>
        /// <exception cref="ArgumentException">When string is null or whitespace.</exception>
        public static string CheckNonNullOrWhiteSpace(string value)
            => CheckNonNullOrWhiteSpace(value, string.Empty, string.Empty);

        /// <summary>
        /// Checks if the string is null or whitespace, and throws ArgumentException if it is.
        /// </summary>
        /// <param name="value">String.</param>
        /// <param name="message">Exception message.</param>
        /// <returns>Checked string.</returns>
        /// <exception cref="ArgumentException">When string is null or whitespace.</exception>
        public static string CheckNonNullOrWhiteSpace(string value, string message)
            => CheckNonNullOrWhiteSpace(value, message, string.Empty);

        /// <summary>
        /// Checks if the string is null or whitespace, and throws ArgumentException if it is.
        /// </summary>
        /// <param name="value">String.</param>
        /// <param name="message">Exception message.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <returns>Checked string.</returns>
        /// <exception cref="ArgumentException">When string is null or whitespace.</exception>
        public static string CheckNonNullOrWhiteSpace(string value, string message, string paramName)
        {
            CheckArgument(!string.IsNullOrWhiteSpace(value), message, paramName);

            return value;
        }

        /// <summary>
        /// Checks if the string is null or empty, and throws ArgumentException if it is.
        /// </summary>
        /// <param name="value">String.</param>
        /// <returns>Checked string.</returns>
        /// <exception cref="ArgumentException">When string is null or empty.</exception>
        public static string CheckNonNullOrEmpty(string value)
            => CheckNonNullOrEmpty(value, string.Empty, string.Empty);

        /// <summary>
        /// Checks if the string is null or empty, and throws ArgumentException if it is.
        /// </summary>
        /// <param name="value">String.</param>
        /// <param name="message">Exception message.</param>
        /// <returns>Checked string.</returns>
        /// <exception cref="ArgumentException">When string is null or empty.</exception>
        public static string CheckNonNullOrEmpty(string value, string message)
            => CheckNonNullOrEmpty(value, message, string.Empty);

        /// <summary>
        /// Checks if the string is null or empty, and throws ArgumentException if it is.
        /// </summary>
        /// <param name="value">String.</param>
        /// <param name="message">Exception message.</param>
        /// <param name="paramName">Input parameter name.</param>
        /// <returns>Checked string.</returns>
        /// <exception cref="ArgumentException">When string is null or empty.</exception>
        public static string CheckNonNullOrEmpty(string value, string message, string paramName)
        {
            CheckArgument(!string.IsNullOrEmpty(value), message, paramName);

            return value;
        }
    }
}