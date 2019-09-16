using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;

namespace SafeApp
{
    public class Fetch
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        public Fetch(IntPtr app)
            => _app = app;

        public Task<byte[]> FetchAsync(string url)
            => throw new NotImplementedException();
    }
}
