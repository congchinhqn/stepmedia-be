using System;
using System.Linq;

namespace Metatrade.Core.Extensions
{
    public static partial class MiscExtensions
    {
        public static EnumDescription<T>[] GetEnumDescriptions<T>() where T: struct, Enum, IConvertible
        {
            return Enum.GetValues<T>().Select(e => new EnumDescription<T>(e)).ToArray();
        }
    }

    public class EnumDescription<TEnum> where TEnum : struct, IConvertible
    {
        public EnumDescription(TEnum value)
        {
            Value = value;
            Name = value.ToString();
            Description = value.GetDescription();
        }

        public TEnum Value { get; }
        public string Name { get; }
        public string Description { get; }
    }
}
