using Chnage.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chnage.Helpers
{
    public static class Helper
    {
        public static string GetStructuredJsonInfo<T>(T obj, List<string> propertiesName = null)
        {
            var type = typeof(T);
            var sb = new StringBuilder();
            foreach (var prop in type.GetProperties())
            {
                if (propertiesName != null && propertiesName.Count > 0 &&
                    !propertiesName.Contains(prop.Name)) continue;

                var isExcludeProp = prop.GetCustomAttributes(typeof(ExcludeFromStructuredJsonInfo), false);
                if (isExcludeProp != null && isExcludeProp.Length == 1) continue;

                if (prop.PropertyType == typeof(DateTime) ||
                    prop.PropertyType == typeof(DateTime?))
                {
                    sb.AppendLine($"{prop.Name}: {(prop.GetValue(obj) as DateTime?)?.ToString("MM/dd/yyyy HH:mm:ss")}<br/>");
                }
                else if (prop.PropertyType.IsPrimitive || prop.PropertyType==typeof(string) ||
                    prop.PropertyType.IsValueType)
                {
                    sb.AppendLine($"{prop.Name}: {prop.GetValue(obj)}<br/>");
                }

            }

            return sb.ToString();
        }
    }
}
