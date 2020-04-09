using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// XorUrl Encoder API.
    /// </summary>
    public static class XorEncoder
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;

        /// <summary>
        /// Returns an encoded XorUrl string based on the parameters.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="typeTag">TypeTag (if content is Mutable Data).</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="path">Content path.</param>
        /// <param name="subNames">Url sub name.</param>
        /// <param name="contentVersion">Current content version.</param>
        /// <param name="baseEncoding">Base encoding (base32z, base32, base64).</param>
        /// <returns>Encoded XorUrl string.</returns>
        public static Task<string> EncodeAsync(
            byte[] xorName,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            List<string> subNames,
            ulong contentVersion,
            string baseEncoding)
            => AppBindings.XorurlEncodeAsync(xorName, typeTag, dataType, contentType, path, subNames, contentVersion, baseEncoding);

        /// <summary>
        /// Returns an XorUrlEncoder instance based on the parameters.
        /// Default base encoding (base32x) is used.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="typeTag">TypeTag (if content is Mutable Data).</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="path">Content path.</param>
        /// <param name="subNames">Url sub name.s</param>
        /// <param name="contentVersion">Current content version.</param>
        /// <returns>New XorUrlEncoder instance.</returns>
        public static Task<XorUrlEncoder> EncodeAsync(
            byte[] xorName,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            List<string> subNames,
            ulong contentVersion)
            => AppBindings.XorurlEncoderAsync(xorName, typeTag, dataType, contentType, path, subNames, contentVersion);

        /// <summary>
        /// Returns an XorUrlEncoder instance from a XorUrl string.
        /// Default base encoding (base32x) is used.
        /// </summary>
        /// <param name="xorUrl">XorUrl string for which encoder is required.</param>
        /// <returns>New XorUrlEncoder instance.</returns>
        public static Task<XorUrlEncoder> XorUrlEncoderFromUrl(string xorUrl)
            => AppBindings.XorurlEncoderFromUrlAsync(xorUrl);
    }
}
