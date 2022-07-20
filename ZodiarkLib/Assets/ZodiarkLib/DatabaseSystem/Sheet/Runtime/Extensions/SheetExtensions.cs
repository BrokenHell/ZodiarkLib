namespace ZodiarkLib.Database.Sheet.Extensions
{
    public static class SheetExtensions
    {
        public static int ParseToInt(this string s)
        {
            int.TryParse(s, out var value);
            return value;
        }
        
        public static float ParseToFloat(this string s)
        {
            float.TryParse(s, out var value);
            return value;
        }
        
        public static bool ParseToBool(this string s)
        {
            bool.TryParse(s, out var value);
            return value;
        }
    }
}