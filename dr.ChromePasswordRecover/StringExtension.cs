namespace dr.ChromePasswordRecover
{
    public static class StringExtension
    {
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
