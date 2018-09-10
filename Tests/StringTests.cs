using Extensions;
using Xunit;

namespace Tests
{
    public class StringTests
    {
        [Fact]
        public void Save_Windows_Safe_FileName()
        {
            string fileName = @":>?</\Test.png";

            Assert.Equal("Test.png", fileName.RemoveInvalidSaveCharacters());
        }

    }
}
