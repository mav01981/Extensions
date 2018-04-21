namespace Tests
{
    using Extensions;
    using System.Collections.Generic;
    using Xunit;

    public class CarsLots
    {
        public List<Car> Lots { get; set; }
    }
    public class CarsArray
    {
        public Car[] Lots { get; set; }
    }

    public class Car
    {
        public string Name { get; set; }
        public string Colour { get; set; }
        public Engine Engine { get; set; }
        public List<InsuredDriver> InsuredDriver { get; set; }
        public int[] Ids { get; set; }
        public InsuredDriver[] PreviousDriverrs { get; set; }
    }
    public class Engine
    {
        public string EngineSize { get; set; }
        public Type Measure { get; set; }

    }
    public class Type
    {
        public string Name { get; set; }
    }
    public class InsuredDriver
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class ObjectExtensionTests
    {
        [Fact]
        public void Flatten_Model_ToString()
        {
            var vehicle = new Car()
            {
                Ids = new int[] { 1, 2, 3 },

                Name = "Nissan Primera 1.8 Flare 5dr",
                Colour = "Red",
                Engine = new Engine()
                {
                    EngineSize = "1.8L",
                    Measure = new Type()
                    {
                        Name = "New measure Type"
                    }
                }
            };

            var output = vehicle.ObjectToString<Car>();

            Assert.Equal("Name:Jon Doe Age:34 Address:Tests.Address ", output);
        }

        [Fact]
        public void Flatten_ModelWithList_ToString()
        {
            var lots = new CarsLots()
            {
                Lots = new List<Car>()
                {
                    new Car()
                    {
                        Ids = new int[] { 1, 2, 3 },

                        Name = "Nissan Primera 1.8 Flare 5dr",
                        Colour = "Red",
                        Engine = new Engine()
                        {
                            EngineSize = "1.8L",
                            Measure = new Type()
                            {
                                Name = "New measure Type"
                            }
                        },
                        InsuredDriver = new List<InsuredDriver>()
                        {
                            new InsuredDriver()
                            {
                                FullName = "Jonathan Smart",
                                EmailAddress = "mail@mailaddrees.com"
                            }
                        },
                        PreviousDriverrs = new InsuredDriver[]
                        {
                              new InsuredDriver()
                            {
                                FullName = "Jonathan Smart",
                                EmailAddress = "mail@mailaddrees.com"
                            },
                                   new InsuredDriver()
                            {
                                FullName = "Jonathan Smart",
                                EmailAddress = "mail@mailaddrees.com"
                            }

                        }
                    },
                    new Car()
                    {
                        Ids = new int[] { 1, 2, 3 },
                        Name = "Nissan Primera 1.8 Flare 5dr",
                        Colour = "Red",
                        Engine = new Engine()
                        {
                            EngineSize = "1.8L",
                            Measure = new Type()
                            {
                                Name = "New measure Type"
                            }
                        }
                    }

                }


            };

            var output = lots.ObjectToString<CarsLots>();

            Assert.Equal("Name:Jon Doe Age:34 Address:Tests.Address ", output);
        }

        [Fact]
        public void Flatten_ModelWithArray_ToString()
        {
            var array = new CarsArray()
            {
                Lots = new Car[]
                {
                  new Car()
                  {

                  }
              }
            };

            //var output = lots.ObjectToString<CarsLots>();

            //Assert.Equal("Name:Jon Doe Age:34 Address:Tests.Address ", output);
        }
    }
}
