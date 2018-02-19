using System;
using System.ComponentModel;


namespace CP.Utility
{
    /// <summary>
    /// Handle all the enum related operations.
    /// </summary>
    public static class EnumHandler
    {
        /// <summary>
        /// Get enum description of a given enum.
        /// </summary>
        /// <param name="value">Enum to retrieve the description.</param>
        /// <returns>Enum's description.</returns>
        public static string GetEnumDescription(System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Convert string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String representation of enum value</param>
        /// <returns>Enum value</returns>
        public static T ParseToEnum<T>(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);

        }
    }
}
