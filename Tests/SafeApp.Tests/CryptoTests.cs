using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Utilities;

// ReSharper disable AccessToDisposedClosure

namespace SafeApp.Tests
{
    [TestFixture]
    internal class CryptoTests
    {
        [Test]
        public async Task EncryptAndDecryptTest()
        {
            var session = await Utils.CreateTestApp();
            var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
            using (encKeyPairTuple.Item1)
            using (encKeyPairTuple.Item2)
            {
                var plainBytes = Utils.GetRandomData(200).ToList();
                var cipherBytes = await session.Crypto.EncryptAsync(plainBytes.ToList(), encKeyPairTuple.Item1, encKeyPairTuple.Item2);
                var decryptedBytes = await session.Crypto.DecryptAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
                Assert.That(decryptedBytes, Is.EqualTo(plainBytes));
                Assert.That(
                    async () => await session.Crypto.DecryptAsync(
                        Utils.GetRandomData(20).ToList(),
                        encKeyPairTuple.Item1,
                        encKeyPairTuple.Item2),
                    Throws.TypeOf<FfiException>());
                Assert.That(
                    async () => await session.Crypto.DecryptAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item1),
                    Throws.TypeOf<FfiException>());
            }

            session.Dispose();
        }

        [Test]
        public async Task EncryptKeyPair()
        {
            var session = await Utils.CreateTestApp();
            var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
            using (encKeyPairTuple.Item1)
            using (encKeyPairTuple.Item2)
            {
                var rawKey = await session.Crypto.EncPubKeyGetAsync(encKeyPairTuple.Item1);
                Assert.That(rawKey.Length, Is.EqualTo(AppConstants.AsymPublicKeyLen));
                using (var handle = await session.Crypto.EncPubKeyNewAsync(rawKey))
                {
                    Assert.That(handle, Is.Not.Null);
                }

                rawKey = await session.Crypto.EncSecretKeyGetAsync(encKeyPairTuple.Item2);
                Assert.That(rawKey.Length, Is.EqualTo(AppConstants.AsymSecretKeyLen));
                using (var handle = await session.Crypto.EncSecretKeyNewAsync(rawKey))
                {
                    Assert.That(handle, Is.Not.Null);
                }
            }

            session.Dispose();
        }

        [Test]
        public async Task SealedBoxEncryption()
        {
            var session = await Utils.CreateTestApp();
            var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
            using (encKeyPairTuple.Item1)
            using (encKeyPairTuple.Item2)
            {
                var plainBytes = Utils.GetRandomData(100).ToList();
                var cipherBytes = await session.Crypto.EncryptSealedBoxAsync(plainBytes, encKeyPairTuple.Item1);
                var decryptedBytes = await session.Crypto.DecryptSealedBoxAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
                Assert.That(decryptedBytes, Is.EqualTo(plainBytes));
                Assert.That(
                    async () => await session.Crypto.DecryptSealedBoxAsync(
                        Utils.GetRandomData(10).ToList(),
                        encKeyPairTuple.Item1,
                        encKeyPairTuple.Item2),
                    Throws.TypeOf<FfiException>());
                cipherBytes = await session.Crypto.EncryptSealedBoxAsync(new List<byte>(), encKeyPairTuple.Item1);
                await session.Crypto.DecryptSealedBoxAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
            }

            session.Dispose();
        }

        [Test]
        public async Task SignAndVerify()
        {
            var session = await Utils.CreateTestApp();
            var signKeyPairTuple = await session.Crypto.SignGenerateKeyPairAsync();
            using (signKeyPairTuple.Item1)
            using (signKeyPairTuple.Item2)
            {
                var plainBytes = Utils.GetRandomData(200).ToList();
                var signedData = await session.Crypto.SignAsync(plainBytes, signKeyPairTuple.Item2);
                var verifiedData = await session.Crypto.VerifyAsync(signedData, signKeyPairTuple.Item1);
                Assert.That(verifiedData, Is.EqualTo(plainBytes));
                Assert.That(
                    async () => await session.Crypto.VerifyAsync(Utils.GetRandomData(20).ToList(), signKeyPairTuple.Item1),
                    Throws.TypeOf<FfiException>());
            }

            session.Dispose();
        }

        [Test]
        public async Task SignKeyPair()
        {
            var session = await Utils.CreateTestApp();
            var signKeyPair = await session.Crypto.SignGenerateKeyPairAsync();
            using (signKeyPair.Item1)
            using (signKeyPair.Item2)
            {
                var rawKey = await session.Crypto.SignPubKeyGetAsync(signKeyPair.Item1);
                Assert.That(rawKey.Length, Is.EqualTo(AppConstants.SignPublicKeyLen));
                using (var handle = await session.Crypto.SignPubKeyNewAsync(rawKey))
                {
                    Assert.That(handle, Is.Not.Null);
                }

                rawKey = await session.Crypto.SignSecKeyGetAsync(signKeyPair.Item2);
                Assert.That(rawKey.Length, Is.EqualTo(AppConstants.SignSecretKeyLen));
                using (var handle = await session.Crypto.SignSecKeyNewAsync(rawKey))
                {
                    Assert.That(handle, Is.Not.Null);
                }
            }

            session.Dispose();
        }
    }
}
