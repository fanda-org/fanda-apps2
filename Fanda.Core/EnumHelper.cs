using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Fanda.Core
{
    public static class EnumHelper<T> where T : struct
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (var fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, true));
            }

            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static int ParseInt(string value)
        {
            return (int)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType()
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(fi => fi.Name)
                .ToList();
        }

        public static IList<EnumListItem<T>> GetEnumList()
        {
            var enumList = new List<EnumListItem<T>>();
            foreach (var fi in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                #region Read Description and Display attributes

                var descriptionAttribute = fi.GetCustomAttribute(
                    typeof(DescriptionAttribute), false) as DescriptionAttribute;

                var displayAttribute = fi.GetCustomAttribute(
                    typeof(DisplayAttribute), false) as DisplayAttribute;

                string displayText;
                if (descriptionAttribute != null && string.IsNullOrEmpty(descriptionAttribute.Description))
                {
                    displayText = descriptionAttribute.Description;
                }
                else if (displayAttribute != null)
                {
                    if (string.IsNullOrEmpty(displayAttribute.Description))
                    {
                        displayText = displayAttribute.Description;
                    }
                    else if (string.IsNullOrEmpty(displayAttribute.Name))
                    {
                        displayText = displayAttribute.Name;
                    }
                    else
                    {
                        displayText = fi.Name;
                    }
                }
                else
                {
                    displayText = fi.Name;
                }

                #endregion Read Description and Display attributes

                enumList.Add(new EnumListItem<T>
                {
                    Id = ParseInt(fi.Name),
                    //Value = Parse(fi.Name),
                    Text = fi.Name,
                    DisplayText = displayText
                });
            }

            return enumList;
        }

        public static string GetDisplayName(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
            {
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
            }

            if (descriptionAttributes == null)
            {
                return string.Empty;
            }

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Name : value.ToString();
        }

        private static string LookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (var staticProperty in resourceManagerProvider
                .GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(ResourceManager))
                {
                    var resourceManager = (ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey);
                }
            }

            return resourceKey; // Fallback with the key name
        }
    }

    public static class EnumParserExtensions
    {
        public static T Parse<T>(this Enum _, string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this string value, T defaultValue)
            where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }
    }

    public class EnumListItem<T>
        where T : struct
    {
        public int Id { get; set; }

        //public T Value { get; set; }
        public string Text { get; set; }

        public string DisplayText { get; set; }
    }
}