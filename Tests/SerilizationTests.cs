namespace Tests
{
    using Extensions;
    using Xunit;


    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
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
                Age = 21
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
                Age = 21
            };

            var copy = model.DeepCopy<Person>();

            Assert.False(model.Equals(copy));
        }

        [Fact]
        public void ShallowCopy_Model_PointToDifferentMemoryAddress()
        {
            var model = new Person()
            {
                Name = "Jon Doe",
                Age = 21
            };

            var copy = model.ShallowCopy<Person>();

            Assert.False(model.Equals(copy));
        }
    }
}
