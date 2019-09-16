using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

namespace SafeApp
{
    public class Names
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        public Names(IntPtr app)
            => _app = app;

        public Task<(NrsMap, string)> CreateNewPublicNameAsync(
            string name,
            string link,
            bool directLink,
            bool dryRun,
            bool @default)
            => throw new NotImplementedException();

        public Task<(NrsMap, string, ulong)> AddUpdateSubNameAsync(
            string name,
            string link,
            bool @default,
            bool directLink,
            bool dryRun)
            => throw new NotImplementedException();

        public Task<(NrsMap, string, ulong)> RemoveSubNameAsync(string name, bool dryRun)
            => throw new NotImplementedException();
    }
}
