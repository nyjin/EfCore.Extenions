using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EfCore.Extensions.WebApi.Services
{
    public static class RuntimeHelper
    {
        public static IDictionary<string, object> ToDictionary(object subject)
        {
            return subject.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(subject, null));
        }

        public static IEnumerable<string> GetPropertyNames(Type type) => type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => x.Name);
    }
}