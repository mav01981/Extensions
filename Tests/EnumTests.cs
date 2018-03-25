namespace Tests
{
    using Xunit;
    using Extensions;

    enum Job
    {
        PoliceMan,
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
    }
}
