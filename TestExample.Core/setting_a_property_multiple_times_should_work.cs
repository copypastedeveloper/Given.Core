using Given.Core;
using Xunit;

namespace TestExample.Core
{
    public class setting_a_property_multiple_times_should_work : Scenario<TestData>
    {
        given a_data_thing = context => context.Thing = 1234;

        when something_happens = context => context.Thing = 4567;

        then it_should_work = context => Assert.Equal(4567, context.Thing);

        then it_should_throw = context => Assert.Equal(4566, context.Thing);
    }
}