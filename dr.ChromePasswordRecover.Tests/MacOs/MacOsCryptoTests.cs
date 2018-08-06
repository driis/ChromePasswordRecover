using dr.ChromePasswordRecover.MacOS;
using Xunit;

namespace dr.ChromePasswordRecover.Tests.MacOs
{
    public class MacOsCryptoTests
    {
        const string Password = "8ZZ/Bxfx/1EYuhZEgufKvg==";
        private readonly MacOsCrypto _sut = new MacOsCrypto(Password);
        
        [Fact]
        public void CanDecryptKnownSecret()
        {
            byte[] cipherText = {118, 49, 48, 255, 66, 49, 234, 108, 92, 73, 51, 164, 227, 189, 217, 118, 104, 109, 53};

            string result = _sut.DecryptString(cipherText);
            
            Assert.Equal("abekat22", result);

        }
 
        [Theory]
        [InlineData("x")]
        [InlineData("")]
        [InlineData("abekat22")]
        [InlineData("6BPtMzjWY2HtKPcAWerT")]
        [InlineData("CnB6^.Wb8G'$FpN$i:(-")]
        [InlineData("u]_2rk9|bX!(gnuk8AgX4bm;9")]
        public void CanRoundTripPlainText(string secret)
        {
            var cipherText = _sut.EncryptString(secret);
            var plainTextBack = _sut.DecryptString(cipherText);
            
            Assert.Equal(secret, plainTextBack);

        }
        
    }
}