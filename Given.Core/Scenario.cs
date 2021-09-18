using System.Linq;
using Xunit;
using Xunit.Abstractions;

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

            var fields = FieldHelper.GetFields(GetType());

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
            var fields = FieldHelper.GetFields(GetType());

            fields[typeof(given)].ToList().ForEach(x => ((given)x.GetValue(this)).Invoke());
            fields[typeof(when)].ToList().ForEach(x => ((when)x.GetValue(this)).Invoke());
            var currentThen = (then)fields[typeof(then)].First(x => x.Name == then).GetValue(this);
            currentThen.Invoke();
        }
    }
}