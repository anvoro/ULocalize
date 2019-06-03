using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace UnityLocalize.Editor
{
    internal static class ReflectionHelper
    {
        public static string[] GetAllSubtypesNames(Type baseType)
        {
            return GetAllSubtypes(baseType)
                .Select(e => e.Name)
                .ToArray();
        }

        public static Type[] GetAllSubtypes(Type baseType)
        {
            var types = Assembly.GetAssembly(baseType).DefinedTypes;

            Type[] result;
            if (baseType.IsInterface)
            {
                result = types
                    .Where(type => type.ImplementedInterfaces.Any(inter => inter == baseType)
                                   && !type.IsInterface
                                   && !type.IsAbstract)
                    .Cast<Type>()
                    .ToArray();
            }
            else
            {
                result =
                    types.Where(type => type.IsSubclassOf(baseType) && !type.IsAbstract)
                        .Cast<Type>()
                        .ToArray();
            }

            return result;
        }

        public static List<Type> GetMarkedTypesFromAssembly(Type attributeType, Type subType)
        {
            var result = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<Type> markedTypes = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(subType) && t.GetCustomAttribute(attributeType) != null);

                result.AddRange(markedTypes);
            }

            return result;
        }

        public static List<Delegate> ExtractGetMethods<TTarget, TProperty>()
            where TTarget : Object
        {
            Type unityObjectType = typeof(TTarget);

            Type propType = typeof(TProperty);

            var props = unityObjectType.GetProperties()
                .Where(f => f.PropertyType == propType);

            return props.Select(p => ExtractGetMethod<TTarget, TProperty>(p.Name)).Cast<Delegate>().ToList();
        }

        public static Func<TTarget, TProperty> ExtractGetMethod<TTarget, TProperty>(string methodName)
        {
            Type objectType = typeof(TTarget);

            PropertyInfo prop = objectType.GetProperties()
                .Single(f => f.Name == methodName);

            MethodInfo m = prop?.GetGetMethod();

            return (Func<TTarget, TProperty>)Delegate.CreateDelegate(typeof(Func<TTarget, TProperty>), null, m ?? throw new InvalidOperationException());
        }

        public static MethodInfo GetGenericMethod(Type typeForExtraction, string methodName,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static,
            params Type[] genericTypes)
        {
            if (genericTypes == null)
            {
                throw new ArgumentNullException(nameof(genericTypes));
            }

            MethodInfo method = typeForExtraction.GetMethod(methodName, bindingFlags);

            if (method == null)
                throw new NullReferenceException($"No method named: '{methodName}' in class '{typeof(Type).Name}'");

            return method.MakeGenericMethod(genericTypes);
        }
    }
}