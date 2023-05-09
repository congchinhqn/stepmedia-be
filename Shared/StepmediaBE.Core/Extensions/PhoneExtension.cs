﻿using System;

 namespace Metatrade.Core.Extensions
{
    public static class PhoneExtension
    {
        private const string vnCode = "+84";

        public static string RemoveCountryCode(this string phone)
        {
            return phone.Replace(vnCode, "");
        }

        public static string FormatPhone(string phone) => phone.StartsWith("0") ? phone : $"0{phone}";

        public static string FormatCustomerPhone(string phone, string countryCode = vnCode)
            => countryCode.StartsWith("+")
                ? $"{countryCode}{phone}"
                : $"+{countryCode}{phone}";
    }

    public static class MobileExtension
    {
        private const string currency = "VND";
        public static string DefaultBloodUnit = "ml";
        public static string ToVND(decimal price)
        {
            if (price == decimal.Zero)
                return $"0 {currency}";
            return $"{price:#,#0} {currency}";
        }
        public static string ToDotVnd(string priceString)
        {
            if (string.IsNullOrEmpty(priceString))
            {
                return $"0 {currency}";
            }

            return priceString.Replace(",", ".");
        }

        public static string ToDot(decimal value)
        {
            return ($"{value:#,#0}").Replace(",", ".");
        }
        public static string ToVNDToDotVND(decimal price)
        {
            return ToDotVnd(ToVND(price));
        }

        public static string UserGuideToString(decimal quantity)
        {
            if (quantity == 0) return "0";
            if (quantity == (int)quantity)
            {
                return $"{(int)quantity}";
            }
            else
            {
                return $"{quantity}";
            }
        }

        public static string ToDotVnd(decimal price)
        {
            if (price == decimal.Zero)
                return $"0 {currency}";
            return $"{(object)price:#.#0} {currency}";
        }

        public static string GetTimeSpan12(DateTime date)
        {
            return $"{date:hh\\:mm}";
        }
        public static string GetTimeSpan24(DateTime date)
        {
            return $"{date:HH\\:mm}";
        }
        public static string GetTimeSlot12(DateTime startTime, DateTime endTime)
        {
            return $"{startTime:hh\\:mm} - {endTime:hh\\:mm}";
        }
        public static string GetTimeSlot24(DateTime startTime, DateTime endTime)
        {
            return $"{startTime:HH\\:mm} - {endTime:HH\\:mm}";
        }

        public static string BloodUnit(int amount, string unit = "ml")
        {
            if (amount == 0) return $"0 {unit}";
            return $"{amount:#,#0} {unit}";
        }
    }
}
