namespace Tests
{
    using Extensions;
    using Xunit;

    public class Person
    {
        public string Name;
        public int Age;
        public Address Address; //Reference Type
    }

    public class Address
    {
        public string StreetName;
    }

    public class SerilizationTests
    {
        //[the name of the tested feature]_[expected input / tested state]_[expected behavior].
        //registerNewUserAccount_ExistingEmailAddressGiven_ShouldShowErrorMessage(). 

        [Fact]
        public void Copy_Model_PointToSameMemoryAddress()
        {
            var model = new Person()
            {
                Name = "Jon Doe",
                Age = 21,
                Address = new Address()
                {
                    StreetName = "Coronation Street"
                }
            };

            var copy = model;

            Assert.True(model.Equals(copy));
        }

        [Fact]
        public void DeepCopy_Model_PointToDifferentMemoryAddress()
        {
            var model = new Person()
            {
                Name = "Jon Doe",
                Age = 21,
                Address = new Address()
                {
                    StreetName = "Coronation Street"
                }
            };

            var copy = model.DeepCopy<Person>();

            model.Address.StreetName = "Penny Lane";

            Assert.NotEqual(model.Address.StreetName, copy.Address.StreetName);
        }

        [Fact]
        public void ShallowCopy_Model_PointToDifferentMemoryAddress()
        {
            var model = new Person()
            {
                Name = "Jon Doe",
                Age = 21,
                Address = new Address()
                {
                    StreetName = "Coronation Street"
                }
            };

            var copy = model.ShallowCopy<Person>();

            model.Address.StreetName = "Penny Lane";

            Assert.Equal(model.Address.StreetName, copy.Address.StreetName);

        }
    }
}
