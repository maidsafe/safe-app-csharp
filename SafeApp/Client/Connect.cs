using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SafeApp.AppBindings;

namespace SafeApp.Client
{
    [SuppressMessage("ReSharper", "All", Justification = "Pending")]
    public class Connect
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;

        public Task<IntPtr> ConnectAsync(string appId, string authCredentials)
            => throw new NotImplementedException();
    }
}
