using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Given.Core
{
    static class FieldHelper
    {
        internal static Dictionary<Type,List<FieldInfo>> GetFields(Type current)
        {
            IEnumerable<Type> getAllTypes()
            {
                var types = new List<Type> { current };

                while (true)
                {
                    var baseType = current.BaseType;

                    if (baseType == typeof(Scenario) || baseType == typeof(object))
                        break;

                    current = baseType;
                    types.Add(current);
                }

                return types;
            };
            
            var fields = getAllTypes()
                .Reverse()
                .SelectMany(t => t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                .GroupBy(x => x.FieldType)
                .ToDictionary(x => x.Key, x => x.ToList());

            return fields;
        }
    }
}