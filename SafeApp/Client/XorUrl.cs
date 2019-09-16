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
    public class XorUrl
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        XorNameArray _name;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public XorUrl(XorNameArray name)
            => _name = name;

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<string> EncodeXorUrlAsync(
            ulong typeTag,
            ulong dataType,
            ulong contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<XorUrlEncoder> NewXorUrlEncoderAsync(
            ulong typeTag,
            ulong dataType,
            ulong contentType,
            string path,
            string subNames,
            ulong contentVersion)
            => throw new NotImplementedException();

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public Task<XorUrlEncoder> NewXorUrlEncoderFromXorUrlAsync(string xorUrl)
            => throw new NotImplementedException();
    }
}
