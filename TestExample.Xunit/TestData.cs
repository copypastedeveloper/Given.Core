namespace TestExample.Core
{
    public class FactoryContext
    {
        public CarFactory Factory { get; set; }
        public Car Car { get; set; }
    }

    public class CarFactory
    {
        readonly string _manufacturer;

        public CarFactory(string manufacturer)
        {
            _manufacturer = manufacturer;
        }
        public Car Build(string carType)
        {
            return new Car {CarType = $"{_manufacturer} {carType}"};
        }
    }

    public class Car
    {
        public string CarType { get; set; }
    }
}