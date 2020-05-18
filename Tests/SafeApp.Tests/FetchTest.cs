using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    public class FetchTest
    {
        [OneTimeSetUp]
        public void Setup() => TestUtils.PrepareTestData();

        [OneTimeTearDown]
        public void TearDown() => TestUtils.RemoveTestData();

        [Test]
        public async Task FetchDataTypesTest()
        {
            var session = await TestUtils.CreateTestApp();

            var (keyUrl, keys) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10");
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(keyUrl), isFetch: true);

            var walletUrl = await session.Wallet.WalletCreateAsync();
            await session.Wallet.WalletInsertAsync(walletUrl, TestUtils.GetRandomString(5), true, keys.SK);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(walletUrl), isFetch: true);

            var (filesXorUrl, processedFiles, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(filesXorUrl), isFetch: true);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(processedFiles.Files[1].FileXorUrl), isFetch: true);

            var (_, _, nrsXorUrl) = await session.Nrs.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{filesXorUrl}?v=0",
                false,
                false,
                true);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(nrsXorUrl), isFetch: true, expectNrs: true);
        }

        [Test]
        public async Task InspectDataTypesTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (keyUrl, keys) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10");
            var keyInspectResult = await session.Fetch.InspectAsync(keyUrl);

            // Todo: Re-enable proper validation once the insepect API is updated.
            // ValidateFetchOrInspectDataTypes(keyInspectResult);
            Assert.IsNotNull(keyInspectResult);

            var walletUrl = await session.Wallet.WalletCreateAsync();
            await session.Wallet.WalletInsertAsync(walletUrl, TestUtils.GetRandomString(5), true, keys.SK);
            var walletInspectResult = await session.Fetch.InspectAsync(walletUrl);

            // ValidateFetchOrInspectDataTypes(walletInspectResult);
            Assert.IsNotNull(walletInspectResult);

            var (filesXorUrl, processedFiles, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
            var filesInspectResult = await session.Fetch.InspectAsync(filesXorUrl);

            // ValidateFetchOrInspectDataTypes(filesInspectResult);
            Assert.IsNotNull(filesInspectResult);

            var fileInspectResult = await session.Fetch.InspectAsync(processedFiles.Files[1].FileXorUrl);

            // ValidateFetchOrInspectDataTypes(fileInspectResult;
            Assert.IsNotNull(fileInspectResult);

            var (_, _, nrsXorUrl) = await session.Nrs.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{filesXorUrl}?v=0",
                false,
                false,
                true);
            var nrsInspectResult = await session.Fetch.InspectAsync(nrsXorUrl);

            // ValidateFetchOrInspectDataTypes(nrsInspectResult, expectNrs: true);
            Assert.IsNotNull(nrsInspectResult);
        }

        public void ValidateFetchOrInspectDataTypes(ISafeData data, bool isFetch = false, bool expectNrs = false)
        {
            if (data != null)
            {
                switch (data)
                {
                    case SafeKey key:
                        Validate.XorName(key.XorName);
                        Assert.IsNotNull(key.ResolvedFrom);
                        break;
                    case Wallet wallet:
                        Validate.XorName(wallet.XorName);
                        Assert.IsNotNull(wallet.ResolvedFrom);
                        if (isFetch)
                            Assert.NotZero(wallet.Balances.WalletBalances.Count);
                        else
                            Assert.Zero(wallet.Balances.WalletBalances.Count);
                        break;
                    case FilesContainer filesContainer:
                        Validate.XorName(filesContainer.XorName);
                        Assert.NotNull(filesContainer.FilesMap);
                        Assert.NotZero(filesContainer.FilesMap.Files.Count);
                        Assert.NotNull(filesContainer.FilesMap.Files[0].FileMetaData);
                        Assert.NotZero(filesContainer.FilesMap.Files[0].FileMetaData.Count);
                        Assert.IsNotNull(filesContainer.FilesMap.Files[0].FileName);
                        Assert.IsNotEmpty(filesContainer.FilesMap.Files[0].FileName);
                        Assert.IsNotNull(filesContainer.ResolvedFrom);
                        break;
                    case PublishedImmutableData immutableData:
                        Assert.IsNotNull(immutableData.Data);
                        Validate.XorName(immutableData.XorName);
                        if (isFetch)
                            Assert.NotZero(immutableData.Data.Length);
                        else
                            Assert.Zero(immutableData.Data.Length);
                        Assert.IsNull(immutableData.ResolvedFrom);
                        break;
                    case SafeDataFetchFailed dataFetchOrInspectFailed:
                        Assert.IsNotNull(dataFetchOrInspectFailed.Description);
                        Assert.AreNotEqual(0, dataFetchOrInspectFailed.Code);
                        Assert.Fail(dataFetchOrInspectFailed.Description);
                        break;
                }
            }
            else
            {
                if (isFetch)
                    Assert.Fail("Fetch data type not available");
                else
                    Assert.Fail("Inspect data type not available");
            }
        }
    }
}
