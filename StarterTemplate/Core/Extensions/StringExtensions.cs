using System;
using System.Text.RegularExpressions;

namespace StarterTemplate.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatWith(this string s, params object[] args)
        {
            if (s.HasNoValue())
                return s;

            return String.Format(s, args);
        }

        public static bool HasValue(this string s)
        {
            return !String.IsNullOrWhiteSpace(s);
        }

        public static bool HasNoValue(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        public static string ValueOr(this string s, string defaultValue)
        {
            return (s.HasValue()) ? s : defaultValue;
        }

        public static string ToWords(this string s)
        {
            if (!s.HasValue()) return s;

            var regex = new Regex("(?<=[a-z])(?<x>[A-Z0-9])|(?<=.)(?<x>[A-Z0-9])(?=[a-z])");
            return regex.Replace(s, " ${x}");
        }
    }
}
