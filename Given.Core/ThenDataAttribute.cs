using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Given.Core
{
    public class ThenDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod.ReflectedType == null) return new List<object[]>();

            var fields = FieldHelper.GetFields(testMethod.ReflectedType);

            return fields.First(x => x.Key.Name == "then").Value.Select(f => new[] { f.Name });
        }
    }
}