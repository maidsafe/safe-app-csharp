using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class NrsTest
    {
        private const bool SetDefault = true;
        private const bool DirectLink = true;
        private const bool DryRun = false;

        [OneTimeSetUp]
        public void Setup() => TestUtils.PrepareTestData();

        [OneTimeTearDown]
        public void TearDown() => TestUtils.RemoveTestData();

        [Test]
        public async Task ParseUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var safeUrl = await Nrs.ParseUrlAsync(xorUrl);

            // todo: verify that these are actually the expected values
            Assert.AreEqual(ContentType.Raw, safeUrl.ContentType);
            Assert.AreEqual(0, safeUrl.ContentVersion);
            Assert.AreEqual(DataType.SafeKey, safeUrl.DataType);
            Assert.AreEqual(1, safeUrl.EncodingVersion);
            Assert.AreEqual(string.Empty, safeUrl.Path);
            Assert.IsEmpty(safeUrl.SubNames);
            Assert.AreEqual(0, safeUrl.TypeTag);
            Validate.XorName(safeUrl.XorName);
        }

        [Test]
        public async Task ParseAndResolveUrlTest()
        {
            var session = await TestUtils.CreateTestApp();

            var xorUrl = await CreateFilesContainerAsync(session);

            var api = session.Nrs;
            var (_, _, nrsMapXorUrl) = await api.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{xorUrl}?v=0",
                false,
                DryRun,
                SetDefault);
            var (xorUrlEncoder, resolvedFrom) = await api.ParseAndResolveUrlAsync(nrsMapXorUrl);

            Validate.Encoder(xorUrlEncoder, DataType.PublishedSeqAppendOnlyData, ContentType.FilesContainer, 1100);
            Validate.Encoder(resolvedFrom, DataType.PublishedSeqAppendOnlyData, ContentType.NrsMapContainer, 1500);
        }

        [Test]
        public async Task CreateNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);

            var link = await CreateFilesContainerAsync(session);

            var api = session.Nrs;
            var (nrsMapRaw, processedEntries, xorUrl) = await api.CreateNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                false,
                DryRun,
                SetDefault);

            Assert.IsNotNull(processedEntries);
            Validate.RawNrsMap(nrsMapRaw);
            await Validate.XorUrlAsync(xorUrl, DataType.PublishedSeqAppendOnlyData, ContentType.NrsMapContainer, 1500);
        }

        [Test]
        public async Task AddToNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrlResult = await CreateNrsMapContainerXorUrlAsync(session, name);

            var link = await CreateFilesContainerAsync(session);

            var (nrsMapRaw, xorUrl, version) = await session.Nrs.AddToNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                SetDefault,
                DirectLink,
                DryRun);

            Assert.AreEqual(1, version);
            Validate.RawNrsMap(nrsMapRaw);
            await Validate.XorUrlAsync(xorUrl, DataType.PublishedSeqAppendOnlyData, ContentType.NrsMapContainer, 1500);
        }

        [Test]
        public async Task RemoveFromNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrlResult = await CreateNrsMapContainerXorUrlAsync(session, name);

            var (nrsMapRaw, xorUrl, version) = await session.Nrs.RemoveFromNrsMapContainerAsync(name, DryRun);

            Assert.AreEqual(1, version);
            Assert.AreEqual("{\"sub_names_map\":{},\"default\":\"NotSet\"}", nrsMapRaw);
            await Validate.XorUrlAsync(xorUrl, DataType.PublishedSeqAppendOnlyData, ContentType.NrsMapContainer, 1500);
        }

        [Test]
        public async Task GetNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrl = await CreateNrsMapContainerXorUrlAsync(session, name);

            var api = session.Nrs;
            var (nrsMapRaw, version) = await api.GetNrsMapContainerAsync(xorUrl);

            Assert.AreEqual(0, version);
            Validate.RawNrsMap(nrsMapRaw);
            await Validate.XorUrlAsync(xorUrl, DataType.PublishedSeqAppendOnlyData, ContentType.NrsMapContainer, 1500);
        }

        async Task<string> CreateFilesContainerAsync(Session session)
        {
            var (xorUrl, _, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);

            return xorUrl;
        }

        async Task<string> CreateNrsMapContainerXorUrlAsync(Session session, string name)
        {
            var link = await CreateFilesContainerAsync(session);

            var api = session.Nrs;
            var (_, _, xorUrl) = await api.CreateNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                DirectLink,
                DryRun,
                SetDefault);

            return xorUrl;
        }
    }
}
