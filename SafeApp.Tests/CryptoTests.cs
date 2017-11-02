using System;
using System.Linq;
using NUnit.Framework;
using SafeApp.Misc;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CryptoTests {
    private const int EncKeySize = 32;

    [Test]
    public async void GenerateEncKeyPair() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      await Crypto.EncPubKeyFreeAsync(encKeyPairTuple.Item1);
      await Crypto.EncSecretKeyFreeAsync(encKeyPairTuple.Item2);
    }

    [Test]
    public async void GetAppPubSignKey() {
      Utils.InitialiseSessionForRandomTestApp();
      var handle = await Crypto.AppPubSignKeyAsync();
      Assert.NotNull(handle);
      await Crypto.SignKeyFreeAsync(handle);
    }

    [Test]
    public async void SealedBoxEncryption() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      var plainBytes = new byte[1024];
      new Random().NextBytes(plainBytes);
      var cipherBytes = await Crypto.EncryptSealedBoxAsync(plainBytes.ToList(), encKeyPairTuple.Item1);
      var decryptedBytes = await Crypto.DecryptSealedBoxAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
      Assert.AreEqual(plainBytes, decryptedBytes);
      await Crypto.EncPubKeyFreeAsync(encKeyPairTuple.Item1);
      await Crypto.EncSecretKeyFreeAsync(encKeyPairTuple.Item2);
    }

    [Test]
    public async void GetPublicEncryptKey() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      var rawKey = await Crypto.EncPubKeyGetAsync(encKeyPairTuple.Item1);
      Assert.AreEqual(rawKey.Count, EncKeySize);
      var handle = await Crypto.EncPubKeyNewAsync(rawKey);
      Assert.NotNull(handle);
      rawKey = await Crypto.EncSecretKeyGetAsync(encKeyPairTuple.Item2);
      Assert.AreEqual(rawKey.Count, EncKeySize);
      handle = await Crypto.EncSecretKeyNewAsync(rawKey);
      Assert.NotNull(handle);
      await Crypto.EncPubKeyFreeAsync(encKeyPairTuple.Item1);
      await Crypto.EncSecretKeyFreeAsync(encKeyPairTuple.Item2);
    }
  }
}
