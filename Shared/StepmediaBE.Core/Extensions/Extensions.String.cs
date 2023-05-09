using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Metatrade.Core.Extensions
{
    public static partial class MiscExtensions
    {
        #region Fields

        private static readonly string[] VietnameseSigns =
        {
            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        #endregion

        #region Methods

        public static string CamelCase(this string value)
        {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static bool EqualsIgnoreCase(this string source, string value)
        {
            if (source == null || value == null)
                return source == value;
            return source.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimUpper(this string source)
        {
            return string.IsNullOrEmpty(source) ? null : source.Trim().ToUpper();
        }

        public static string TrimNull(this string source)
        {
            return string.IsNullOrEmpty(source) ? null : source.Trim();
        }

        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }

        public static string GetDescription<T>(this T enumValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs.Length > 0) description = ((DescriptionAttribute) attrs[0]).Description;
            }

            return description;
        }

        public static string JoinString(this IEnumerable<string> strings, string defaultSeperator = ", ")
        {
            return strings.Aggregate(new StringBuilder(),
                (sb, s) => (sb.Length == 0 ? sb : sb.Append(defaultSeperator))
                    .Append(s), sb => sb.ToString());
        }

        public static string RemoveSignVietnamese(this string str)
        {
            for (var i = 1; i < VietnameseSigns.Length; i++)
            for (var j = 0; j < VietnameseSigns[i].Length; j++)
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            return str;
        }

        public static string RemoveWhitespace(this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
                input = input.Replace(" ", string.Empty);

            return input;
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            var dictionary = new Dictionary<char, char[]>
            {
                {'a', new[] {'à', 'á', 'ä', 'â', 'ã'}},
                {'A', new[] {'À', 'Á', 'Ä', 'Â', 'Ã'}},
                {'c', new[] {'ç'}},
                {'C', new[] {'Ç'}},
                {'e', new[] {'è', 'é', 'ë', 'ê'}},
                {'E', new[] {'È', 'É', 'Ë', 'Ê'}},
                {'i', new[] {'ì', 'í', 'ï', 'î'}},
                {'I', new[] {'Ì', 'Í', 'Ï', 'Î'}},
                {'o', new[] {'ò', 'ó', 'ö', 'ô', 'õ'}},
                {'O', new[] {'Ò', 'Ó', 'Ö', 'Ô', 'Õ'}},
                {'u', new[] {'ù', 'ú', 'ü', 'û'}},
                {'U', new[] {'Ù', 'Ú', 'Ü', 'Û'}}
            };

            value = dictionary.Keys.Aggregate(value, (x, y) => dictionary[y].Aggregate(x, (z, c) => z.Replace(c, y)));

            return new Regex("[^0-9a-zA-Z._ ]+?").Replace(value, string.Empty);
        }

        public static IEnumerable<T> SplitTo<T>(this string str, params string[] separator) where T : IConvertible
        {
            foreach (var s in str.Split(separator, StringSplitOptions.None))
                yield return (T) Convert.ChangeType(s, typeof(T));
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }

        public static TEnum ToEnum<TEnum>(this object value)
        {
            var type = typeof(TEnum);

            if (!type.IsEnum) throw new ArgumentException($"{type} is not an enum.");

            if (type.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0)
            {
                var values = Enum.GetValues(type);
                switch (Enum.GetUnderlyingType(type).FullName)
                {
                    case "System.Byte":
                    {
                        var myVal = (byte) value;
                        foreach (byte val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    case "System.SByte":
                    {
                        var myVal = (sbyte) value;
                        foreach (sbyte val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    case "System.UInt16":
                    {
                        var myVal = (ushort) value;
                        foreach (ushort val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    case "System.Int16":
                    {
                        var myVal = (short) value;
                        foreach (short val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    case "System.UInt32":
                    {
                        var myVal = (uint) value;
                        foreach (uint val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    case "System.Int32":
                    {
                        var myVal = (int) value;
                        foreach (int val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    case "System.UInt64":
                    {
                        var myVal = (ulong) value;
                        foreach (ulong val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    case "System.Int64":
                    {
                        var myVal = (long) value;
                        foreach (long val in values)
                            if ((myVal & val) > 0)
                                myVal ^= val;
                        if (myVal > 0) throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
                    }
                        break;
                    default:
                        var underlyingType = Enum.GetUnderlyingType(type);
                        throw new ArgumentException($"{type} does not have a valid backing type ({underlyingType}).");
                }
            }
            else
            {
                if (!type.IsEnumDefined(value))
                    throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
            }

            return (TEnum) Enum.ToObject(type, value);
        }

        public static int? ToNullableInt(this string s) => int.TryParse(s, out var i) ? i : (int?) null;

        #endregion

        // public static string GetActionNameAttr(this Enum value)
        // {
        //     string str = (string)null;
        //     ActionNameAttribute[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(ActionNameAttribute), false) as ActionNameAttribute[];
        //     if (customAttributes != null && (uint)customAttributes.Length > 0U)
        //         str = customAttributes[0].Value;
        //     return str;
        // }


        public static MemoryStream GetFileStream(this string fileName)
        {
            var returnedStream = new MemoryStream();

            using (var stream =
                new FileStream(fileName,
                    FileMode.Open, FileAccess.Read))
            {
                stream.CopyTo(returnedStream);
            }

            returnedStream.Seek(0, SeekOrigin.Begin);

            return returnedStream;
        }
    }
}