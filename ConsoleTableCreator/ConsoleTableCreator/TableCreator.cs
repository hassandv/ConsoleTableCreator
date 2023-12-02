using ConsoleTableCreator.Extensions;
using System.Reflection;
using System;

namespace ConsoleTableCreator
{
    public class TableCreator<T> where T : new()
    {
        private readonly T _obj;

        private readonly List<string> _keys;

        private readonly List<KeyValue> _KeyValues;

        private int CountAll;

        public string LineShape { private get; set; } = "-";


        public string SpaceShape { private get; set; } = "+";


        public string SideShape { private get; set; } = "|";


        public int LimitedChar { private get; set; } = 0;


        public TableCreator(T t)
        {
            _obj = t;
            _KeyValues = GetKeysAndValues();
            _keys = (from x in _KeyValues.DistinctBy((KeyValue c) => c.Key)
                     select x.Key.ToString()).ToList();
        }

        public string GetTable()
        {
            return bollockKey() + "\n" + bollockValue() + EndLine();
        }

        public string bollockKey()
        {
            string text = "";
            string text2 = "";
            bool flag = true;
            foreach (string key in _keys)
            {
                int countLimited = GetCountLimited(key);
                if (flag)
                {
                    text += NoText(countLimited, LineShape, countLimited - 1, SpaceShape, hasStart: true);
                    text2 += WithText(countLimited, SideShape, key, hasStart: true);
                    flag = false;
                }
                else
                {
                    text += NoText(countLimited, LineShape, countLimited - 1, SpaceShape);
                    text2 += WithText(countLimited, SideShape, key);
                }
            }

            CountAll = text.Length;
            return text + "\n" + text2 + "\n" + text;
        }

        public string bollockValue()
        {
            string text = string.Empty;
            int num = 0;
            foreach (KeyValue keyValue in _KeyValues)
            {
                int countLimited = GetCountLimited(keyValue.Key);
                text = ((num != 0) ? (text + WithText(countLimited, SideShape, keyValue?.Value?.ToString())) : (text + WithText(countLimited, SideShape, keyValue?.Value?.ToString(), hasStart: true)));
                num++;
                if (num == _keys.Count())
                {
                    text += "\n";
                    num = 0;
                }
            }

            return text;
        }

        public string EndLine()
        {
            string text = string.Empty;
            for (int i = 0; i < CountAll; i++)
            {
                text = ((i != 0) ? ((i != CountAll - 1) ? (text + LineShape) : (text + SpaceShape)) : (text + SpaceShape));
            }

            return text;
        }

        private string NoText(int spaceNumber, string patern, int spaceBreak, string paternSpace, bool hasStart = false)
        {
            string text = string.Empty;
            int num = 0;
            for (int i = 0; i < spaceNumber; i++)
            {
                if (hasStart && i == 0)
                {
                    text += paternSpace;
                }

                text += patern;
                if (num == spaceBreak)
                {
                    text += paternSpace;
                    num = 0;
                }

                num++;
            }

            return text;
        }

        private string WithText(int spaceNumber, string patern, string text, bool hasStart = false)
        {
            string text2 = string.Empty;
            for (int i = 0; i < spaceNumber; i++)
            {
                if (hasStart && i == 0)
                {
                    text2 += patern;
                }

                text2 = ((!(i < text?.Length)) ? (text2 + " ") : (text2 + text[i]));
                if (i + 1 == spaceNumber)
                {
                    return text2 += patern;
                }
            }

            return text2;
        }

        private List<KeyValue> GetKeysAndValues()
        {
            List<KeyValue> list = new List<KeyValue>();
            if (_obj is IEnumerable<object>)
            {
                IEnumerable<object> enumerable = _obj as IEnumerable<object>;
                foreach (object item in enumerable)
                {
                    PropertyInfo[] properties = item.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        list.Add(new KeyValue
                        {
                            Key = propertyInfo.Name,
                            Value = propertyInfo.GetValue(item)
                        });
                    }
                }

                return list;
            }

            if (_obj != null)
            {
                PropertyInfo[] properties2 = _obj.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo2 in properties2)
                {
                    list.Add(new KeyValue
                    {
                        Key = propertyInfo2.Name,
                        Value = propertyInfo2.GetValue(_obj)
                    });
                }
            }

            return list;
        }

        private int GetCountLimited(string key)
        {
            bool flag = ((LimitedChar != 0) ? true : false);
            int num = _KeyValues.CountValues(key);
            if (flag)
            {
                return (num > LimitedChar) ? LimitedChar : num;
            }

            return num;
        }
    }
}