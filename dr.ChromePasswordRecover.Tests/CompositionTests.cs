using Xunit;

namespace dr.ChromePasswordRecover.Tests
{    
    public class CompositionTests
    {
        [Fact]
        public void CanGetCompositionRoot()
        {
            Assert.NotNull(CompositionRoot.Current);
        }

        [Fact]
        public void CanGetLoginReader()
        {
            var dataFile = CompositionRoot.Current.DataFile;
            Assert.NotNull(dataFile);
        }
    }
}