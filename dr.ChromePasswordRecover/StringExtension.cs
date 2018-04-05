namespace dr.ChromePasswordRecover
{
    public static class StringExtension
    {
        /// <summary>
        /// Gets the left first <c>count</c> characters from a string. 
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Left(this string value, int count)
        {
            return Left(value, count, null);
        }

        public static string Left(this string value, int count, string suffix)
        {
            if (suffix != null)
                count -= suffix.Length;

            if (string.IsNullOrEmpty(value) || value.Length <= count)
                return value;

            return value.Substring(0, count) + suffix;            
        }
    }
}
