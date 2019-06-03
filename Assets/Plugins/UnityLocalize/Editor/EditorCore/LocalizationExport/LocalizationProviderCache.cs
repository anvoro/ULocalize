using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityLocalize.Editor
{
    internal class LocalizationProviderCache
    {
        private readonly Dictionary<Type, List<Delegate>> _getMethodsCache = new Dictionary<Type, List<Delegate>>();

        public List<Delegate> GetCachedMethods(Type type)
        {
            if (this._getMethodsCache.ContainsKey(type))
            {
                return this._getMethodsCache[type];
            }

            MethodInfo extractGetMethods = ReflectionHelper.GetGenericMethod(typeof(ReflectionHelper), "ExtractGetMethods",
                genericTypes: new[] { type, typeof(LocalizableString) });

            List<Delegate> getMethodsDelegates = (List<Delegate>)extractGetMethods.Invoke(null, null);
            this._getMethodsCache[type] = getMethodsDelegates;

            return getMethodsDelegates;
        }
    }
}
