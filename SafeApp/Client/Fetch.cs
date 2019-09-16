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
    public class Fetch
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Fetch(IntPtr app)
            => _app = app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<byte[]> FetchAsync(string url)
            => throw new NotImplementedException();
    }
}
