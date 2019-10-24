
using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtension
    {
        public static T GetConfig<T>(this IConfiguration configuration,string key=default(string))
        {
            T obj = Activator.CreateInstance<T>();
            configuration.GetSection(string.IsNullOrEmpty(key) ? typeof(T).Name : key).Bind(obj);
            return obj;
        }
    }
}
