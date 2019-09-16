using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;

namespace SafeApp
{
    public class Wallet
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        public Wallet(IntPtr app)
            => _app = app;

        public Task<string> CreateNewWalletAsync(
            string payWith,
            bool noBalance,
            string name,
            string keyUrl,
            string secretKey,
            bool testCoins,
            string preload)
            => throw new NotImplementedException();

        public Task<string> InsertBalanceToWalletAsync(
            string target,
            string payWith,
            string secretKey,
            string name,
            string keyUrl,
            bool @default)
            => throw new NotImplementedException();

        public Task<string> QueryWalletBalanceAsync(string target)
            => throw new NotImplementedException();

        public Task<ulong> TransferWalletBalanceAsync(string from, string to, string amount, ulong id)
            => throw new NotImplementedException();
    }
}
