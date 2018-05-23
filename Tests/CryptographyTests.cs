using Extensions;
using System.Security.Cryptography;
using Xunit;

namespace Tests
{
    public class CryptographyTest
    {
        [Fact]
        public void HashPassword_Are_Equal()
        {
            string CreatedPassword = "Password1";

            var hashed = CreatedPassword.HashPassword(128, SHA256.Create());

            string EnteredPassword = "Password1";

            var hashWithSalt = EnteredPassword.HashPassword(hashed.Salt, SHA256.Create());

            Assert.Equal(hashed.Digest, hashWithSalt.Digest);
        }

        [Fact]
        public void HashPassword_AreNot_Equal()
        {
            string CreatedPassword = "Password1";

            var hashed = CreatedPassword.HashPassword(128, SHA256.Create());

            string EnteredPassword = "password1";

            var hashWithSalt = EnteredPassword.HashPassword(hashed.Salt, SHA256.Create());

            Assert.NotEqual(hashed.Digest, hashWithSalt.Digest);
        }

        [Fact]
        public void Encrypt_String()
        {
            string CreatedPassword = "Hello World";

            var encryptedString = CreatedPassword.Encrypt();
            var decryptedString = encryptedString.Descrypt();

            Assert.Equal(decryptedString, CreatedPassword);

        }
    }
}

