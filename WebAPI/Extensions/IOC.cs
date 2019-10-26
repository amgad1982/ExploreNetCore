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
        private static Array LoadAssemblyTypes (Assembly assembly,Type type) {
            return assembly.ExportedTypes.Where (x =>
                type.IsAssignableFrom (x) && !x.IsInterface && !x.IsAbstract).Select (Activator.CreateInstance).ToArray();
        }

        public static void RegisterAsemblyTypeHavingRegistrar<T> (this IServiceCollection services, Action<T> action, params Assembly[] assemblies) {
            var types = new List<T> ();
            assemblies.ToList ().ForEach (assembly => types.AddRange (LoadAssemblyTypes<T> (assembly)));
            types.ForEach (t => action (t));

        }
        public static void RegisterAsemblyTypes(this IServiceCollection services,IEnumerable<Tuple<Type,Assembly,Action<dynamic>>> types){
            foreach(var t in types)
            {
                var scandtypes = LoadAssemblyTypes(t.Item2, t.Item1);
                foreach(var obj in scandtypes){
                    t.Item3(obj);
                }
            }
        }
    }
}