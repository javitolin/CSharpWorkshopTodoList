using System.Text;

namespace ThirdDayToDoConsoleApp.Extensions
{

    public static class CharExtensions
    {
        public static char ToUpper(this char value)
        {
            return $"{value}".ToUpper()[0];
        }

        public static char ToLower(this char value)
        {
            return $"{value}".ToLower()[0];
        }
    }

    public static class StringExtensions
    {

        public static string ToZigZagCase(this string value, bool zeroIndex)
        {
            StringBuilder sb = new StringBuilder();
            int zeroIndexInt = zeroIndex ? 0 : 1;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (i % 2 == zeroIndexInt)
                {
                    sb.Append(zeroIndex ? c.ToLower() : c.ToUpper());
                }

                else
                {
                    sb.Append(zeroIndex ? c.ToUpper() : c.ToLower());
                }
            }

            return sb.ToString();
        }
    }
}