using System.Linq;
using System.Reflection;
using Xunit;

namespace TestExample.Core
{
    abstract class ScenarioWithComments<T> where T : class, new()
    {
        protected delegate void given(T context);
        protected delegate void when(T context);
        protected delegate void then(T context);

        //readonly List<ScenarioResult> _results = new List<ScenarioResult>();
        //string GivenPrefix => _results.Any(x => x.StepType == StepType.given) ? "And" : "Given";
        //string WhenPrefix => _results.Any(x => x.StepType == StepType.when) ? "And" : "When";
        //string ThenPrefix => _results.Any(x => x.StepType == StepType.then) ? "And" : "Then";
        T _context;

        //[Fact]
        public void Prove()
        {
            _context = new T();

            var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            var givens = fields.Where(x => x.FieldType == typeof(given)).ToList();

            var whens = fields.Where(x => x.FieldType == typeof(when)).ToList();

            var thens = fields.Where(x => x.FieldType == typeof(then)).ToList();

            givens.ForEach(x => ((given)x.GetValue(this)).Invoke(_context));
            whens.ForEach(x => ((when)x.GetValue(this)).Invoke(_context));
            thens.ForEach(x => ((then)x.GetValue(this)).Invoke(_context));

            //if (_results.Any(r => !r.Success))
            //{
            //    ExceptionDispatchInfo.Capture(_results.First(x => x.Exception != null).Exception).Throw();
            //}
        }

        //void InvokeDelegateField(FieldInfo field)
        //{
        //    ScenarioResult scenarioResult = new ScenarioResult();
        //    try
        //    {
        //        switch (field.GetValue(this))
        //        {
        //            case given given:

        //                scenarioResult = new ScenarioResult
        //                {
        //                    StepType = StepType.given,
        //                    Output = $"{GivenPrefix} {field.Name.Replace("_", " ")}",
        //                    DerivedFromField = field
        //                };
        //                _results.Add(scenarioResult);
        //                given.Invoke((dynamic)this);
        //                scenarioResult.Success = true;
        //                break;
        //            case when @when:
        //                scenarioResult = new ScenarioResult
        //                {
        //                    StepType = StepType.when,
        //                    Output = $"{WhenPrefix} {field.Name.Replace("_", " ")}",
        //                    DerivedFromField = field
        //                };
        //                _results.Add(scenarioResult);
        //                @when.Invoke((dynamic)this);
        //                scenarioResult.Success = true;
        //                break;
        //            case then then:
        //                scenarioResult = new ScenarioResult
        //                {
        //                    StepType = StepType.then,
        //                    Output = $"{ThenPrefix} {field.Name.Replace("_", " ")}",
        //                    DerivedFromField = field
        //                };
        //                _results.Add(scenarioResult);
        //                then.Invoke((dynamic)this);
        //                scenarioResult.Success = true;
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        scenarioResult.Success = false;
        //        scenarioResult.Exception = ex;
        //    }
        //}

        //class ScenarioResult
        //{
        //    public StepType StepType { get; set; }
        //    public FieldInfo DerivedFromField { get; set; }
        //    public string Output { get; set; }
        //    public bool Success { get; set; }
        //    public Exception Exception { get; set; }
        //}

        //enum StepType
        //{
        //    given, when, then
        //}
    }
}