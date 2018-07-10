using System.IO;
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
            var reader = CompositionRoot.Current.LoginReader(Path.GetTempFileName());
            Assert.NotNull(reader);
        }

        [Fact]
        public void CanGetDataFile()
        {
            var dataFile = CompositionRoot.Current.DataFile;
            Assert.NotNull(dataFile);
        }
    }
}