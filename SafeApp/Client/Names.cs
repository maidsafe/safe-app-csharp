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
    public class Names
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Names(IntPtr app)
            => _app = app;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<(NrsMap, string)> CreateNewPublicNameAsync(
            string name,
            string link,
            bool directLink,
            bool dryRun,
            bool @default)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<(NrsMap, string, ulong)> AddUpdateSubNameAsync(
            string name,
            string link,
            bool @default,
            bool directLink,
            bool dryRun)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<(NrsMap, string, ulong)> RemoveSubNameAsync(string name, bool dryRun)
            => throw new NotImplementedException();
    }
}
