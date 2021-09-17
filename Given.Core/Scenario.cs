using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Given.Core
{
    public abstract class Scenario<T> where T : class, new()
    {
        public delegate void given(T context);
        public delegate void when(T context);
        public delegate void then(T context);

        [Theory, ThenData]
        public void Run(string then)
        {
            var context = new T();

            var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToLookup(x => x.FieldType);
            
            fields[typeof(given)].ToList().ForEach(x => ((given)x.GetValue(this)).Invoke(context));
            fields[typeof(when)].ToList().ForEach(x => ((when)x.GetValue(this)).Invoke(context));
            var currentThen = (then)fields[typeof(then)].First(x => x.Name == then).GetValue(this);
            currentThen.Invoke(context);
        }
    }

    public abstract class Scenario
    {
        public delegate void given();
        public delegate void when();
        public delegate void then();

        [Theory, ThenData]
        public void Run(string then)
        {
            IEnumerable<Type> getAllTypes(Type current)
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
            
            var fields = getAllTypes(GetType())
                .Reverse()
                .SelectMany(t => t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                .ToLookup(x => x.FieldType);
            
            fields[typeof(given)].ToList().ForEach(x => ((given)x.GetValue(this)).Invoke());
            fields[typeof(when)].ToList().ForEach(x => ((when)x.GetValue(this)).Invoke());
            var currentThen = (then)fields[typeof(then)].First(x => x.Name == then).GetValue(this);
            currentThen.Invoke();
        }
    }

    public class ThenDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod.ReflectedType == null) return new List<object[]>();

            var fields = testMethod.ReflectedType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            return fields.Where(x => x.FieldType.Name.Contains("then")).Select(f => new[] { f.Name });

        }
    }

    public class ScenarioDiscoverer : TheoryDiscoverer
    {
        public ScenarioDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }
    }
}