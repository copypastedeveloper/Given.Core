using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Given.Core;

namespace TestExample.Core
{
    // ReSharper disable once UnusedMember.Global
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class building_a_toyota_corrola : Scenario<FactoryContext>
    {
        given a_toyota_factory = _ => _.Factory = new CarFactory("Toyota");

        when building_a_corolla = _ => _.Car = _.Factory.Build("Corrola");

        then the_car_should_be_made_by_the_correct_manufacturer = _ => _.Car.CarType.Should().StartWith("Toyota");

        then the_car_should_be_the_correct_make = _ => _.Car.CarType.Should().EndWith("Corrola");
    }

    public class antoher : Scenario
    {
        given first_given = () => { };
        
        then first_then = () => { };
    }
    
    public class chaibase : antoher
    {
        internal static CarFactory _factory;
        internal static Car _car;

        given a_toyota_factory = () => _factory = new CarFactory("Toyota");

        when building_a_corolla = () => _car = _factory.Build("Corrola");

        then second_then = () => { };
    }
    
    public class building_a_toyota_camry : chaibase
    {
        given final_given = () => { };

        then the_car_should_be_made_by_the_correct_manufacturer = () => _car.CarType.Should().StartWith("Toyota");

        then the_car_should_be_the_correct_make = () => _car.CarType.Should().EndWith("Corrola");
    }
}