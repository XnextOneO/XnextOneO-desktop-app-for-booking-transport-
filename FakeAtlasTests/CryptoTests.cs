using FakeAtlas.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FakeAtlasTests
{
    public class CryptoTests
    {
        [Fact]
        public void GetSalt_2Times_ReturnsNotEqual()
        {
            byte[] salt1 = AtlasCrypto.GetSalt();
            byte[] salt2 = AtlasCrypto.GetSalt();
            Assert.NotEqual(salt1, salt2);
        }

        [Fact]
        public void GenerateSaltedHash_CompareAlrdyHashedAnHashedThere_ReturnsNotEqual()
        {
            byte[] salt = AtlasCrypto.GetSalt();
            string alrdyHashedPassword = "G+1SjQSBIuHEOeIpYHvG1G0B5bCIkjcCCpwZq22Xs/A=";
            string password = "123258789";

            byte[] encryptedPassword = AtlasCrypto.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), salt);
            Assert.NotEqual(alrdyHashedPassword, Convert.ToBase64String(encryptedPassword));
        }


    }
}
