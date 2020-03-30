using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class MiscTest
    {
        [Test]
        public void IsMockAuthenticationBuildTest()
            => Assert.That(Authenticator.IsMockBuild(), Is.True);

        [Test]
        public void IsMockSafeAppBuildTest()
            => Assert.That(Session.AppIsMock(), Is.True);

        [Test]
        public async Task SetConfigFileDirPathTest()
            => await Session.SetAppConfigurationDirectoryPathAsync(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

#if WIN64 && NETCOREAPP
        [Ignore("Rust logger not working on .NET Core Windows, needs more testing locally")]
#endif
        [Test]
        public async Task RustLoggerTest()
        {
            var configPath = string.Empty;
            Assert.That(async () => configPath = await TestUtils.InitRustLogging(), Throws.Nothing);
            Assert.That(
                async () =>
                await Session.DecodeIpcMessageAsync("Some Random Invalid String"),
                Throws.TypeOf<IpcMsgException>());
            var fileEmpty = true;
            for (var i = 0; i < 10; ++i)
            {
                await Task.Delay(1000);
                using (var fs = new FileStream(
                    Path.Combine(configPath, "Client.log"),
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    fileEmpty = string.IsNullOrEmpty(sr.ReadToEnd());
                    if (!fileEmpty)
                    {
                        break;
                    }
                }
            }

            Assert.That(fileEmpty, Is.False);
        }
    }
}
