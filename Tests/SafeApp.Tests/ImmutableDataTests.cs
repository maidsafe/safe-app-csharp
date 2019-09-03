using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class ImmutableDataTests
    {
        [Test]
        public async Task WriteAndReadUsingAsymmetricCipherOpt()
        {
            var data = new byte[1024];
            new Random().NextBytes(data);
            var session = await Utils.CreateTestApp();
            using (var pubEncKey = await session.Crypto.AppPubEncKeyAsync())
            {
                using (var cipherOptHandle = await session.CipherOpt.NewAsymmetricAsync(pubEncKey))
                using (var seHandle = await session.IData.NewSelfEncryptorAsync())
                {
                    await session.IData.WriteToSelfEncryptorAsync(seHandle, data.ToList());
                    var dataMapAddress = await session.IData.CloseSelfEncryptorAsync(seHandle, cipherOptHandle);
                    using (var seReaderHandle = await session.IData.FetchSelfEncryptorAsync(dataMapAddress))
                    {
                        var len = await session.IData.SizeAsync(seReaderHandle);
                        var readData = await session.IData.ReadFromSelfEncryptorAsync(seReaderHandle, 0, len);
                        Assert.That(readData, Is.EqualTo(data));
                    }
                }
            }

            session.Dispose();
        }

        [Test]
        public async Task WriteAndReadUsingPlainCipherOpt()
        {
            var data = new byte[1024];
            new Random().NextBytes(data);
            var session = await Utils.CreateTestApp();
            using (var cipherOptHandle = await session.CipherOpt.NewPlaintextAsync())
            {
                using (var seHandle = await session.IData.NewSelfEncryptorAsync())
                {
                    await session.IData.WriteToSelfEncryptorAsync(seHandle, data.ToList());
                    var dataMapAddress = await session.IData.CloseSelfEncryptorAsync(seHandle, cipherOptHandle);
                    using (var seReaderHandle = await session.IData.FetchSelfEncryptorAsync(dataMapAddress))
                    {
                        var len = await session.IData.SizeAsync(seReaderHandle);
                        var readData = await session.IData.ReadFromSelfEncryptorAsync(seReaderHandle, 0, len);
                        Assert.That(readData, Is.EqualTo(data));
                    }
                }
            }

            session.Dispose();
        }

        [Test]
        public async Task WriteAndReadUsingSymmetricCipherOpt()
        {
            var data = new byte[1024];
            new Random().NextBytes(data);
            var session = await Utils.CreateTestApp();
            using (var cipherOptHandle = await session.CipherOpt.NewSymmetricAsync())
            {
                using (var seHandle = await session.IData.NewSelfEncryptorAsync())
                {
                    await session.IData.WriteToSelfEncryptorAsync(seHandle, data.ToList());
                    var dataMapAddress = await session.IData.CloseSelfEncryptorAsync(seHandle, cipherOptHandle);
                    using (var seReaderHandle = await session.IData.FetchSelfEncryptorAsync(dataMapAddress))
                    {
                        var len = await session.IData.SizeAsync(seReaderHandle);
                        var readData = await session.IData.ReadFromSelfEncryptorAsync(seReaderHandle, 0, len);
                        Assert.That(readData, Is.EqualTo(data));
                    }
                }
            }

            session.Dispose();
        }
    }
}
