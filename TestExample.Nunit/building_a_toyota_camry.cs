using Given.Core.Nunit;
using NUnit.Framework;
using System;

namespace TestExample.Given.Core
{
    
    public abstract class another : Scenario
    {
        given first_given = () => { };
        
        then first_then = () => { };
    }
    
    public abstract class chaibase : another
    {
        internal static CarFactory _factory;
        internal static Car _car;

        given a_toyota_factory = () => _factory = new CarFactory("Toyota");

        when building_a_corolla = () => _car = _factory.Build("Camry");

        then second_then = () => { };
    }

    public class building_a_toyota_camry : chaibase
    {
        given final_given = () => { };

        then the_car_should_be_made_by_the_correct_manufacturer = () => Assert.True(_car.CarType.StartsWith("Toyota"));

        then the_car_should_be_the_correct_make = () => Assert.True(_car.CarType.EndsWith("Camry"));
    }
}
