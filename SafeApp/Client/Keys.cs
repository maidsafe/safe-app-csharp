using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

namespace SafeApp
{
    public class Keys
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        public Keys(IntPtr app)
            => _app = app;

        public Task<(string, BlsKeyPair, string)> GenerateNewSafeKeyPairAsync(
            bool testCoins,
            string payWith,
            string preload,
            string pk)
            => throw new NotImplementedException();

        public Task<string> QueryKeyBalanceAsync(string key, string secret)
            => throw new NotImplementedException();

        public Task<ulong> TransferKeyBalanceAsync(string from, string to, string amount, ulong id)
            => throw new NotImplementedException();
    }
}
