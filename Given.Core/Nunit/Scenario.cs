using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Given.Core.Nunit.Scenario;

namespace Given.Core.Nunit
{
    [TestFixture]
    public abstract class Scenario
    {
        public delegate void given();
        public delegate void when();
        public delegate void then();

        [Test,ThenTestBuilder]
        public void Run(string then)
        {
            var fields = FieldHelper.GetFields(GetType());

            fields[typeof(given)].ToList().ForEach(x => ((given)x.GetValue(this)).Invoke());
            fields[typeof(when)].ToList().ForEach(x => ((when)x.GetValue(this)).Invoke());
            var currentThen = (then)fields[typeof(then)].First(x => x.Name == then).GetValue(this);
            currentThen.Invoke();
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ThenTestBuilderAttribute : NUnitAttribute, ITestBuilder
    {
        NUnitTestCaseBuilder _builder = new NUnitTestCaseBuilder();
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo testMethod, Test suite)
        {
            if (testMethod.MethodInfo.ReflectedType == null) return new List<TestMethod>();

            var fields = FieldHelper.GetFields(testMethod.MethodInfo.ReflectedType);

            return fields[typeof(then)]
                .Select(f => _builder.BuildTestMethod(testMethod, suite, new TestCaseParameters(new object[] { f.Name })));
        }
    }
}
