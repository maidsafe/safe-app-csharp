using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class FilesTest
    {
        [OneTimeSetUp]
        public void Setup() => TestUtils.PrepareTestData();

        [OneTimeTearDown]
        public void TearDown() => TestUtils.RemoveTestData();

        [Test]
        public async Task FilesContainerCreateAndGetTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap1) = await CreateTestFilesContainerAsync(session);
            await Validate.XorUrlAsync(xorUrl, DataType.PublishedSeqAppendOnlyData, ContentType.FilesContainer, 1100);
            Assert.NotNull(processedFiles.Files.Find(q => q.FileName.Equals("index.html")));
            Assert.NotNull(processedFiles);
            Assert.NotNull(filesMap1);

            var (version, filesMap2) = await session.Files.FilesContainerGetAsync(xorUrl);
            Assert.AreEqual(0, version);
        }

        [Test]
        public Task FilesContainerSyncTest()
            => RunSyncTest(dryRun: false);

        [Test]
        public Task FilesContainerSyncDryRunTest()
            => RunSyncTest(dryRun: true);

        [Test]
        public Task FilesContainerAddTest()
            => RunAddTest(dryRun: false);

        public Task FilesContainerAddDryRunTest()
            => RunAddTest(dryRun: true);

        [Test]
        public Task FilesContainerRemoveTest()
            => RunRemoveTest(dryRun: false);

        public Task FilesContainerRemoveDryRunTest()
            => RunRemoveTest(dryRun: true);

        [Test]
        public async Task FilesContainerAddFromRawTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await CreateTestFilesContainerAsync(session);
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newFilesMap) =
                await session.Files.FilesContainerAddFromRawAsync(
                    TestUtils.GetRandomString(50).ToUtfBytes(),
                    newFileName,
                    false,
                    false,
                    false);
            ValidateFiles(1, version, processedFiles, newProcessedFiles, filesMap, newFilesMap);
        }

        [Test]
        public async Task FilesPutAndGetPublishedImmutableTest()
        {
            var session = await TestUtils.CreateTestApp();
            var data = TestUtils.GetRandomString(20).ToUtfBytes();
            var xorUrl = await session.Files.FilesPutPublicImmutableAsync(data, "text/plain", false);
            var newData = await session.Files.FilesGetPublicImmutableAsync(xorUrl);
            Assert.AreEqual(data, newData);
        }

        [Test]
        public async Task FilesPutAndGetPublishedImmutableRangeTest()
        {
            var session = await TestUtils.CreateTestApp();
            var data = TestUtils.GetRandomString(20).ToUtfBytes();
            var xorUrl =
                await session.Files.FilesPutPublicImmutableAsync(data, "text/plain", false);
            var firstHalf =
                await session.Files.FilesGetPublicImmutableAsync(xorUrl, 0, (ulong)(data.Length / 2));
            var secondHalf =
                await session.Files.FilesGetPublicImmutableAsync(xorUrl, (ulong)(data.Length / 2), (ulong)data.Length);
            Assert.AreEqual(data.Take(data.Length / 2), firstHalf);
            Assert.AreEqual(data.Skip(data.Length / 2), secondHalf);
        }

        async Task RunSyncTest(bool dryRun)
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await CreateTestFilesContainerAsync(session);
            Directory.CreateDirectory($"{TestUtils.TestDataDir}/newDir");
            var testFilePath = Path.Combine($"{TestUtils.TestDataDir}/newDir", "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var (version, newProcessedFiles, newFilesMap) = await session.Files.FilesContainerSyncAsync(
                $"{TestUtils.TestDataDir}/newDir",
                xorUrl,
                recursive: true,
                delete: false,
                updateNrs: false,
                followLinks: false,
                dryRun: dryRun);

            ValidateFiles(1, version, processedFiles, newProcessedFiles, filesMap, newFilesMap);
        }

        async Task RunAddTest(bool dryRun)
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await CreateTestFilesContainerAsync(session);
            var testFilePath = Path.Combine(TestUtils.TestDataDir, "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newFilesMap) = await session.Files.FilesContainerAddAsync(
                $"{TestUtils.TestDataDir}/hello.html",
                newFileName,
                false,
                false,
                false,
                dryRun);

            ValidateFiles(1, version, processedFiles, newProcessedFiles, filesMap, newFilesMap);
        }

        async Task RunRemoveTest(bool dryRun)
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await CreateTestFilesContainerAsync(session);
            var testFilePath = Path.Combine(TestUtils.TestDataDir, "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newFilesMap) = await session.Files.FilesContainerAddAsync(
                $"{TestUtils.TestDataDir}/hello.html",
                newFileName,
                false,
                false,
                false,
                dryRun);

            ValidateFiles(1, version, processedFiles, newProcessedFiles, filesMap, newFilesMap);

            (version, newProcessedFiles, _) = await session.Files.FilesContainerRemovePathAsync(
                newFileName,
                false,
                false,
                dryRun);
            Assert.AreEqual(2, version);
            Assert.AreEqual(1, newProcessedFiles.Files.Count);
            Assert.AreEqual("-", newProcessedFiles.Files[0].FileMetaData);
        }

        void ValidateFiles(
            ulong expectedVersion,
            ulong actualVersion,
            ProcessedFiles originalProcessedFiles,
            ProcessedFiles newProcessedFiles,
            FilesMap originalFilesMap,
            FilesMap newFilesMap)
        {
            Assert.AreEqual(expectedVersion, actualVersion);
            Assert.NotNull(newProcessedFiles.Files.Find(q => q.FileName.Equals("hello.html")));

            Assert.IsNotNull(originalProcessedFiles.Files);
            Assert.IsNotNull(newProcessedFiles.Files);
            Assert.AreNotEqual(originalProcessedFiles.Files, newProcessedFiles.Files);

            Assert.IsNotNull(originalFilesMap.Files);
            Assert.IsNotNull(newFilesMap.Files);
            Assert.AreNotEqual(originalFilesMap.Files, newFilesMap.Files);
        }

        public static Task<(string, ProcessedFiles, FilesMap)> CreateTestFilesContainerAsync(Session session)
        {
            return session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false,
                false);
        }
    }
}
