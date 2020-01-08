using System.IO;
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
            var (xorUrl, processedFiles, filesMap1) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
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
        public async Task FilesContainerAddFromRawTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, false, false);
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newFilesMap) = await session.Files.FilesContainerAddFromRawAsync(TestUtils.GetRandomString(50).ToUtfBytes(), newFileName, false, false, false);

            ValidateFiles(1, version, processedFiles, newProcessedFiles, filesMap, newFilesMap);
        }

        [Test]
        public async Task FilesPutAndGetPublishedImmutableTest()
        {
            var session = await TestUtils.CreateTestApp();
            var data = TestUtils.GetRandomString(20).ToUtfBytes();
            var xorUrl = await session.Files.FilesPutPublishedImmutableAsync(data, "text/plain", false);
            var newData = await session.Files.FilesGetPublishedImmutableAsync(xorUrl);
            Assert.AreEqual(data, newData);
        }

        async Task RunSyncTest(bool dryRun)
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, true, false);
            Directory.CreateDirectory($"{TestUtils.TestDataDir}/newDir");
            var testFilePath = Path.Combine($"{TestUtils.TestDataDir}/newDir", "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var (version, newProcessedFiles, newFilesMap) = await session.Files.FilesContainerSyncAsync(
                $"{TestUtils.TestDataDir}/newDir",
                xorUrl,
                recursive: true,
                delete: false,
                updateNrs: false,
                dryRun: dryRun);

            ValidateFiles(1, version, processedFiles, newProcessedFiles, filesMap, newFilesMap);
        }

        async Task RunAddTest(bool dryRun)
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, false, false);
            var testFilePath = Path.Combine(TestUtils.TestDataDir, "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newFilesMap) = await session.Files.FilesContainerAddAsync(
                $"{TestUtils.TestDataDir}/hello.html",
                newFileName,
                false,
                false,
                dryRun);

            ValidateFiles(1, version, processedFiles, newProcessedFiles, filesMap, newFilesMap);
        }

        void ValidateFiles(
            ulong expectedVersion,
            ulong actualVersion,
            ProcessedFiles originalProcessedFiles,
            ProcessedFiles newProcessedFiles,
            string originalFilesMap,
            string newFilesMap)
        {
            Assert.AreEqual(expectedVersion, actualVersion);
            Assert.NotNull(newProcessedFiles.Files.Find(q => q.FileName.Equals("hello.html")));

            // TODO: fix this; incorrect way of testing equality of this struct
            Assert.AreNotEqual(originalProcessedFiles, newProcessedFiles);

            Assert.AreNotEqual(originalFilesMap, newFilesMap);
        }
    }
}
