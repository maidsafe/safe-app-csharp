using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SafeApp.AppBindings;

namespace SafeApp.Client
{
    /// <summary>
    /// Class comment stub.
    /// </summary>
    [SuppressMessage("ReSharper", "All", Justification = "Pending")]
    public class Wallet
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Wallet(IntPtr app)
            => _app = app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<string> CreateNewWalletAsync(
            string payWith,
            bool noBalance,
            string name,
            string keyUrl,
            string secretKey,
            bool testCoins,
            string preload)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<string> InsertBalanceToWalletAsync(
            string target,
            string payWith,
            string secretKey,
            string name,
            string keyUrl,
            bool @default)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<string> QueryWalletBalanceAsync(string target)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<ulong> TransferWalletBalanceAsync(string from, string to, string amount, ulong id)
            => throw new NotImplementedException();
    }
}
