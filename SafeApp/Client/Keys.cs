using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

namespace SafeApp.Client
{
    /// <summary>
    /// Class comment stub.
    /// </summary>
    [SuppressMessage("ReSharper", "All", Justification = "Pending")]
    public class Keys
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Keys(IntPtr app)
            => _app = app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<(string, BlsKeyPair, string)> GenerateNewSafeKeyPairAsync(
            bool testCoins,
            string payWith,
            string preload,
            string pk)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<string> QueryKeyBalanceAsync(string key, string secret)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<ulong> TransferKeyBalanceAsync(string from, string to, string amount, ulong id)
            => throw new NotImplementedException();
    }
}
