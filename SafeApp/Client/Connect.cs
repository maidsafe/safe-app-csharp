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
    public class Connect
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<IntPtr> ConnectAsync(string appId, string authCredentials)
            => throw new NotImplementedException();
    }
}
