
using Extensions;
using Xunit;

namespace Tests
{
    public class ArrayTests
    {
        [Fact]
        public void Array_String_To_String()
        {
            var array = new string[] { "Jon", "Bob", "Tom", "Simon" };

            var output = array.ToSingleString('|');

            Assert.Equal("Jon|Bob|Tom|Simon", output);

        }
        [Fact]
        public void Array_Int_To_String()
        {
            var array = new int[] { 1, 3, 4, 9 };

            var output = array.ToSingleString('|');

            Assert.Equal("1|2|3|4|9", output);
        }
        [Fact]
        public void Two_Arrays_Are_Not_The_Same()
        {
            var array = new int[] { 1, 3, 4, 9 };

            var ShuffledArray = new int[] { 1, 3, 4, 9 }.Shuffle();

            Assert.False(array[0] == ShuffledArray[0]);
        }
    }
}
