using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class WalletTest
    {
        private string _testWalletOne = TestUtils.GetRandomString(10);
        private string _testWalletTwo = TestUtils.GetRandomString(10);

        [Test]
        public async Task CreateWalletTest()
        {
            var (api, _) = await GetKeysAndWalletAPIs();

            var wallet = await api.WalletCreateAsync();
            var balance = await api.WalletBalanceAsync(wallet);
            Validate.IsEqualAmount("0.0", balance);
        }

        [Test]
        public async Task InsertAndBalanceTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            var walletXorUrl = await api.WalletCreateAsync();
            var keyPair_1_Balance = "123";
            var keyPair_2_Balance = "321";
            var expectedEndBalance = "444";
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync(keyPair_1_Balance);
            var (_, keyPair2) = await keysApi.KeysCreatePreloadTestCoinsAsync(keyPair_2_Balance);
            await api.WalletInsertAsync(walletXorUrl, _testWalletOne, setDefault: true, keyPair1.SK);

            var currentBalance = await api.WalletBalanceAsync(walletXorUrl);
            Validate.IsEqualAmount(keyPair_1_Balance, currentBalance);

            await api.WalletInsertAsync(walletXorUrl, _testWalletTwo, setDefault: false, keyPair2.SK);
            currentBalance = await api.WalletBalanceAsync(walletXorUrl);
            Validate.IsEqualAmount(expectedEndBalance, currentBalance);
        }

        [Test]
        public async Task InsertAndGetTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            var walletXorUrl = await api.WalletCreateAsync();
            var (xorUrl1, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123");
            var (xorUrl2, keyPair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("321");

            await api.WalletInsertAsync(walletXorUrl, _testWalletOne, setDefault: true, keyPair1.SK);
            await api.WalletInsertAsync(walletXorUrl, _testWalletTwo, setDefault: false, keyPair2.SK);

            var walletBalances = await api.WalletGetAsync(walletXorUrl);
            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(_testWalletOne)).IsDefault);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletOne).XorUrl, xorUrl1);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletOne).Sk, keyPair1.SK);

            Assert.IsFalse(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(_testWalletTwo)).IsDefault);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletTwo).XorUrl, xorUrl2);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletTwo).Sk, keyPair2.SK);
        }

        [Test]
        public async Task WalletInsertAndSetDefaultTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            var walletXorUrl = await api.WalletCreateAsync();
            var (xorUrl1, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123");
            var (xorUrl2, keyPair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("321");

            await api.WalletInsertAsync(walletXorUrl, _testWalletOne, setDefault: true, keyPair1.SK);
            await api.WalletInsertAsync(walletXorUrl, _testWalletTwo, setDefault: true, keyPair2.SK);
            var walletBalances = await api.WalletGetAsync(walletXorUrl);

            Assert.IsFalse(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(_testWalletOne)).IsDefault);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletOne).XorUrl, xorUrl1);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletOne).Sk, keyPair1.SK);

            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(_testWalletTwo)).IsDefault);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletTwo).XorUrl, xorUrl2);
            Assert.AreEqual(FindWalletBalance(walletBalances, _testWalletTwo).Sk, keyPair2.SK);
        }

        WalletSpendableBalance FindWalletBalance(WalletSpendableBalances list, string name)
            => list.WalletBalances.Find(q => q.WalletName.Equals(name)).Balance;

        [Test]
        public async Task TransferWithoutDefaultBalanceTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            // without default balance
            var fromWalletXorUrl = await api.WalletCreateAsync();

            // with default balance
            var toWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair) = await keysApi.KeysCreatePreloadTestCoinsAsync("123");
            await api.WalletInsertAsync(toWalletXorUrl, "TestBalance", setDefault: true, keyPair.SK);

            AssertThrows(-203, () => api.WalletTransferAsync(fromWalletXorUrl, toWalletXorUrl, "123", 0));
            AssertThrows(-203, () => api.WalletTransferAsync(toWalletXorUrl, fromWalletXorUrl, "123", 0));
        }

        [Test]
        public async Task TransferFromZeroBalanceTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            var fromWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.0");
            await api.WalletInsertAsync(fromWalletXorUrl, "TestBalance", setDefault: true, keyPair1.SK);

            var (toXorUrl, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.5");

            AssertThrows(-301, () => api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "0.5", 0));

            var toWalletXorUrl = await api.WalletCreateAsync();
            var (xorurl2, keypair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.5");
            await api.WalletInsertAsync(toWalletXorUrl, "NewTestBalance", setDefault: true, keypair2.SK);

            AssertThrows(-300, () => api.WalletTransferAsync(fromWalletXorUrl, toWalletXorUrl, "0", 0));
        }

        [Test]
        public async Task TransferDifferentAmountsTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            var fromWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123.321");
            await api.WalletInsertAsync(fromWalletXorUrl, _testWalletOne, setDefault: true, keyPair1.SK);

            var (toXorUrl, keypair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.5");
            await api.WalletInsertAsync(fromWalletXorUrl, "TestBalance2", setDefault: false, keypair2.SK);

            // transfering amount more than current balance
            AssertThrows(-301, () => api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "321.123", 0));

            // transfering invalid amount
            AssertThrows(-300, () => api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "0.dgnda", 0));

            // valid transfer
            await api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "10", 0);
        }

        [Test]
        public async Task TransferToSafeKeyTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            var fromWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123.321");
            await api.WalletInsertAsync(fromWalletXorUrl, "TestBalance", setDefault: true, keyPair1.SK);

            var (xorUrl2, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("10.0");

            // transfer from wallet to key
            await api.WalletTransferAsync(fromWalletXorUrl, xorUrl2, "100.001", 0);
        }

        [Test]
        public async Task TransferFromSafeKeyTest()
        {
            var (api, keysApi) = await GetKeysAndWalletAPIs();

            var (safekeyXorUrl1, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("10");
            var (safekeyXorUrl2, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("0");

            AssertThrows(-207, () => api.WalletTransferAsync(safekeyXorUrl1, safekeyXorUrl2, "5", 0));
        }

        [Ignore("ToDo: FFI Callback issue in these API")]
        public async Task TransferFromUnownedWalletTest()
        {
            var (api1, keysApi1) = await GetKeysAndWalletAPIs();

            var account1WalletXORURL = await api1.WalletCreateAsync();
            var (keyXorUrl1, keyPair1) = await keysApi1.KeysCreatePreloadTestCoinsAsync("123.321");
            await api1.WalletInsertAsync(account1WalletXORURL, "TestBalance", setDefault: true, keyPair1.SK);

            var (api2, keysApi2) = await GetKeysAndWalletAPIs();
            var (keyXorUrl, keyPair2) = await keysApi2.KeysCreatePreloadTestCoinsAsync("123.321");

            AssertThrows(-203, () => api2.WalletTransferAsync(account1WalletXORURL, keyXorUrl, "5", 0));
        }

        async Task<(API.Wallet, API.Keys)> GetKeysAndWalletAPIs()
        {
            var session = await TestUtils.CreateTestApp();
            return (session.Wallet, session.Keys);
        }

        void AssertThrows(int errorCode, AsyncTestDelegate func)
        {
            var ex = Assert.ThrowsAsync<FfiException>(func);
            Assert.AreEqual(errorCode, ex.ErrorCode);
        }
    }
}
