using System;
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

    [Test]
    public async void NewAssymmetric() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      using (var publicEncryptKey = encKeyPairTuple.Item1)
      using (var _ = encKeyPairTuple.Item2)
      using (var handle = await CipherOpt.NewAsymmetricAsync(publicEncryptKey)) {
        Assert.NotNull(handle);
      }
    }

    [Test]
    public void NewAssymmetricNeg() {
      Utils.InitialiseSessionForRandomTestApp();
      Assert.Throws<ArgumentNullException>(async () => await CipherOpt.NewAsymmetricAsync(null));
    }

    [Test]
    public async void NewSymmetric() {
      Utils.InitialiseSessionForRandomTestApp();
      using (var handle = await CipherOpt.NewSymmetricAsync()) {
        Assert.NotNull(handle);
      }
    }
  }
}
