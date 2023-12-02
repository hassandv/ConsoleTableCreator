using System;

namespace ConsoleTableCreator.Extensions
{
    public static class ExtensionEnumerable
    {
        public static int CountAll(this IEnumerable<string> values)
        {
            int num = 0;
            foreach (string value in values)
            {
                if (value != null && value.Count() >= num)
                {
                    num = value.Count();
                }
            }

            return num;
        }

        public static int CountValues(this List<KeyValue> keyVals, string key)
        {
            string key2 = key;
            int num = (from x in keyVals
                       where x.Key == key2
                       select x into c
                       select c.Value?.ToString()).CountAll();
            return (num < key2.Length) ? key2.Length : num;
        }
    }
}
