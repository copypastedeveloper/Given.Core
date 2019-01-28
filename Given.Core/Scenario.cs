using System.Linq;
using System.Reflection;
using Xunit;

namespace Given.Core
{
    public abstract class Scenario<T> where T : class, new()
    {
        protected delegate void given(T context);
        protected delegate void when(T context);
        protected delegate void then(T context);

        T _context;

        [Fact(DisplayName = "Run")]
        public void RunScenario()
        {
            _context = new T();

            var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            var givens = fields.Where(x => x.FieldType == typeof(given)).ToList();

            var whens = fields.Where(x => x.FieldType == typeof(when)).ToList();

            var thens = fields.Where(x => x.FieldType == typeof(then)).ToList();

            givens.ForEach(x => ((given)x.GetValue(this)).Invoke(_context));
            whens.ForEach(x => ((when)x.GetValue(this)).Invoke(_context));
            thens.ForEach(x => ((then)x.GetValue(this)).Invoke(_context));
        }
    }
}