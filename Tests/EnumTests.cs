namespace Tests
{
    using Extensions;
    using System.Collections.Generic;
    using Xunit;

    public enum Job
    {
        PoliceMan = 1,
        Salesman,
        Teacher,
        Gardener
    }

    public class EnumTests
    {
        [Fact]
        public void Enum_Has_TypeAndValue()
        {
            Assert.True(Job.Gardener.Has(0));
        }
        [Fact]
        public void Enum_HasNot_TypeAndValue()
        {
            Assert.False(Job.Gardener.Has("Test"));
        }
        [Fact]
        public void Enum_Is_Value()
        {
            Assert.True(Job.Gardener.Is(3));
        }
        [Fact]
        public void Enum_IsNot_Value()
        {
            Assert.False(Job.Gardener.Is("Test"));
        }

        [Fact]
        public void Enum_To_IEnumerable()
        {
            var ienumerable = HelpersV1.Enumerations.EnumToEnumerable<Job>(typeof(Job));

            Assert.True(ienumerable is IEnumerable<Job>);
        }
        [Fact]
        public void Enum_To_List()
        {
            var list = HelpersV1.Enumerations.EnumToList<Job>(typeof(Job));
       
            Assert.True(list is List<Job>);
            Assert.Equal(4, list.Count);
        }
    }
}
