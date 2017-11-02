using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Misc;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CipherOptTests {
    [Test]
    public async void CreatePlainCipherOpt() {
      Utils.InitialiseSessionForRandomTestApp();
      using (var handle = await CipherOpt.NewPlaintextAsync()) {
        Assert.NotNull(handle);
      }
    }
  }
}
