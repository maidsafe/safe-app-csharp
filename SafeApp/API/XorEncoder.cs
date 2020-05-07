using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// SafeUrl API.
    /// </summary>
    public static class XorEncoder
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;

        /// <summary>
        /// Returns an encoded SafeUrl string based on the parameters.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="nrsName">Content's nrs name on the Network.</param>
        /// <param name="typeTag">TypeTag (if content is Mutable Data).</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="path">Content path.</param>
        /// <param name="subNames">Url sub name.</param>
        /// <param name="queryString">query.</param>
        /// <param name="fragment">fragment.</param>
        /// <param name="contentVersion">Current content version.</param>
        /// <param name="baseEncoding">Base encoding (base32z, base32, base64).</param>
        /// <returns>Encoded SafeUrl string.</returns>
        public static Task<string> EncodeAsync(
            byte[] xorName,
            string nrsName,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            List<string> subNames,
            string queryString,
            string fragment,
            ulong contentVersion,
            string baseEncoding)
            => AppBindings.SafeUrlEncodeAsync(xorName, nrsName, typeTag, dataType, contentType, path, subNames, queryString, fragment, contentVersion, baseEncoding);

        /// <summary>
        /// Returns an SafeUrl instance based on the parameters.
        /// Default base encoding (base32x) is used.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="typeTag">TypeTag (if content is Mutable Data).</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="path">Content path.</param>
        /// <param name="subNames">Url sub name.s</param>
        /// <param name="contentVersion">Current content version.</param>
        /// <param name="queryParams">query.</param>
        /// <param name="fragment">fragment.</param>
        /// <returns>New SafeUrl instance.</returns>
        public static Task<SafeUrl> NewSafeUrlAsync(
            byte[] xorName,
            string nrsName,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            List<string> subNames,
            string queryString,
            string fragment,
            ulong contentVersion)
            => AppBindings.NewSafeUrlAsync(xorName, nrsName, typeTag, dataType, contentType, path, subNames, queryString, fragment, contentVersion);

        /// <summary>
        /// Returns an SafeUrl instance from a XorUrl string.
        /// Default base encoding (base32x) is used.
        /// </summary>
        /// <param name="xorUrl">XorUrl string for which encoder is required.</param>
        /// <returns>New SafeUrl instance.</returns>
        public static Task<SafeUrl> SafeUrlFromUrl(string safeUrl)
            => AppBindings.SafeUrlFromUrlAsync(safeUrl);

        /// <summary>
        /// Returns an encoded SafeUrl string based on the parameters.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="baseEncoding">Base encoding (base32z, base32, base64).</param>
        /// <returns>Encoded SafeUrl string.</returns>
        public static Task<string> EncodeSafeKeyAsync(
            byte[] xorName,
            string baseEncoding)
            => AppBindings.EncodeSafekeyAsync(xorName, baseEncoding);

        /// <summary>
        /// Returns an encoded SafeUrl string based on the parameters.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="baseEncoding">Base encoding (base32z, base32, base64).</param>
        /// <returns>Encoded SafeUrl string.</returns>
        public static Task<string> EncodeImmutableDataAsync(
            byte[] xorName,
            ContentType contentType,
            string baseEncoding)
            => AppBindings.EncodeImmutableDataAsync(xorName, contentType, baseEncoding);

        /// <summary>
        /// Returns an encoded SafeUrl string based on the parameters.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="typeTag">TypeTag (if content is Mutable Data).</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="baseEncoding">Base encoding (base32z, base32, base64).</param>
        /// <returns>Encoded SafeUrl string.</returns>
        public static Task<string> EncodeMutableDataAsync(
            byte[] xorName,
            ulong typeTag,
            ContentType contentType,
            string baseEncoding)
            => AppBindings.EncodeMutableDataAsync(xorName, typeTag, contentType, baseEncoding);

        /// <summary>
        /// Returns an encoded SafeUrl string based on the parameters.
        /// </summary>
        /// <param name="xorName">Content XorName on the Network.</param>
        /// <param name="typeTag">TypeTag (if content is Mutable Data).</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="baseEncoding">Base encoding (base32z, base32, base64).</param>
        /// <returns>Encoded SafeUrl string.</returns>
        public static Task<string> EncodeAppendOnlyDataAsync(
            byte[] xorName,
            ulong typeTag,
            ContentType contentType,
            string baseEncoding)
            => AppBindings.EncodeAppendOnlyDataAsync(xorName, typeTag, contentType, baseEncoding);
    }
}
