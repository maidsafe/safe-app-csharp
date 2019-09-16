using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;

namespace SafeApp
{
    public class Connect
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;

        public Task<IntPtr> ConnectAsync(string appId, string authCredentials)
            => throw new NotImplementedException();
    }
}
