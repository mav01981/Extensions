using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
    public static class Cryptography
    {
        public class HashWithSaltResult
        {
            public string Salt { get; }
            public string Digest { get; set; }

            public HashWithSaltResult(string salt, string digest)
            {
                Salt = salt;
                Digest = digest;
            }
        }
        public static HashWithSaltResult HashPassword(this string password, int saltLength, HashAlgorithm hashAlgorithym)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(salt);
            byte[] digestBytes = hashAlgorithym.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(salt), Convert.ToBase64String(digestBytes));
        }

        public static HashWithSaltResult HashPassword(this string password, string saltValue, HashAlgorithm hashAlgorithym)
        {
            byte[] salt = Convert.FromBase64String(saltValue);

            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(salt);
            byte[] digestBytes = hashAlgorithym.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(salt), Convert.ToBase64String(digestBytes));
        }

        public static string Encrypt(this string input)
        {
            string output = string.Empty;

            //Create a network stream from the TCP connection.   
            //FileStream NetStream = new FileStream("C:\\development\\test.txt", FileMode.Open, FileAccess.ReadWrite);

            MemoryStream stream = new MemoryStream();

            //Create a new instance of the RijndaelManaged class  
            // and encrypt the stream.  
            RijndaelManaged RMCrypto = new RijndaelManaged();

            byte[] Key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            //Create a CryptoStream, pass it the NetworkStream, and encrypt   
            //it with the Rijndael class.  
            CryptoStream CryptStream = new CryptoStream(stream,
            RMCrypto.CreateEncryptor(Key, IV),
            CryptoStreamMode.Write);

            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, RMCrypto.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(input);
                    }
                    encrypted = msEncrypt.ToArray();

                    output = Convert.ToBase64String(encrypted);
                }
            }

            return output;
        }

        public static string Descrypt(this string input)
        {

            string output = string.Empty;
            //The key and IV must be the same values that were used  
            //to encrypt the stream.    
            byte[] Key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            // convert string to stream
            byte[] byteArray = Convert.FromBase64String(input);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            //Create a new instance of the RijndaelManaged class  
            // and decrypt the stream.  
            RijndaelManaged RMCrypto = new RijndaelManaged();

            //Create a CryptoStream, pass it the NetworkStream, and decrypt   
            //it with the Rijndael class using the key and IV.  
            CryptoStream CryptStream = new CryptoStream(stream,
               RMCrypto.CreateDecryptor(Key, IV),
               CryptoStreamMode.Read);

            //Read the stream.  
            StreamReader SReader = new StreamReader(CryptStream);

            //Display the message.  
            output = SReader.ReadToEnd();

            //Close the streams.  
            SReader.Close();

            return output;
        }
    }

}

