using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

namespace SafeApp
{
    public class XorUrl
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        XorNameArray _name;

        public XorUrl(XorNameArray name)
            => _name = name;

        public Task<string> EncodeXorUrlAsync(
            ulong typeTag,
            ulong dataType,
            ulong contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding)
            => throw new NotImplementedException();

        public Task<XorUrlEncoder> NewXorUrlEncoderAsync(
            ulong typeTag,
            ulong dataType,
            ulong contentType,
            string path,
            string subNames,
            ulong contentVersion)
            => throw new NotImplementedException();

        public Task<XorUrlEncoder> NewXorUrlEncoderFromXorUrlAsync(string xorUrl)
            => throw new NotImplementedException();
    }
}
