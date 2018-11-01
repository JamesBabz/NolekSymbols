using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NolekSymbols.Helpers
{
    public static class AbstractClassHelper
    {
        /// <summary>
        ///     Gets all the types in the given namespace
        /// </summary>
        /// <param name="nameSpace">The namespace where the types are</param>
        /// <returns>List of types</returns>
        public static IEnumerable<Type> GetAllTypesInNameSpace(string nameSpace)
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace != null)
                .Where(t => t.Namespace.StartsWith(nameSpace));
        }
    }
}