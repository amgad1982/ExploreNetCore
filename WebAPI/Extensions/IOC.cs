using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace  Microsoft.Extensions.DependencyInjection {
    public static class IOCExtensions {
        private static IEnumerable<T> LoadAssemblyTypes<T> (Assembly assembly) {
            return assembly.ExportedTypes.Where (x =>
                typeof (T).IsAssignableFrom (x) && !x.IsInterface && !x.IsAbstract).Select (Activator.CreateInstance).Cast<T> ().ToList ();
        }

        public static void RegisterAsemblyTypeHavingRegistrar<T> (this IServiceCollection services, Action<T> action, params Assembly[] assemblies) {
            var types = new List<T> ();
            assemblies.ToList ().ForEach (assembly => types.AddRange (LoadAssemblyTypes<T> (assembly)));
            types.ForEach (t => action (t));

        }
    }
}