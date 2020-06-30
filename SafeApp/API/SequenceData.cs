using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;

namespace SafeApp.API
{
    /// <summary>
    /// Sequence data API.
    /// </summary>
    public class SequenceData
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initialise a Sequence data object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal SequenceData(SafeAppPtr appPtr)
            => _appPtr = appPtr;

        /// <summary>
        /// Create a new Sequence data on the network.
        /// </summary>
        /// <param name="data">Data to be stored (in byte[] format).</param>
        /// <param name="xorName">Specify required XorName for the sequence data,
        /// a random address will be used if not provided.</param>
        /// <param name="typeTag">TypeTag to differential b/w two sequance data at some XorName.</param>
        /// <param name="isPrivate">Specify if the data is private or public.</param>
        /// <returns>
        /// XorUrl of the newly created Sequence data on the network.
        /// </returns>
        public Task<string> CreateSequenceDataAsync(
            byte[] data,
            [Optional]byte[] xorName,
            ulong typeTag,
            bool isPrivate)
            => AppBindings.CreateSequenceAsync(_appPtr, data, xorName, typeTag, isPrivate);

        /// <summary>
        /// Get the latest entry for a sequence data from the network.
        /// </summary>
        /// <param name="xorUrl">XorUrl of the sequence data.</param>
        /// <returns>
        /// Returns the latest version and the data for the sequence data.
        /// </returns>
        public Task<(ulong, byte[])> GetSequenceDataAsync(
            string xorUrl)
            => AppBindings.GetSequenceAsync(_appPtr, xorUrl);

        /// <summary>
        /// Append new entry to the sequence data on the network.
        /// </summary>
        /// <param name="xorUrl">XorUrl of the sequence data.</param>
        /// <param name="data">Data to be stored in new entry (in byte[] format).</param>
        /// <returns>
        /// Append new entry to the sequence data on the network.
        /// Return FFIException in case of any error.
        /// </returns>
        public Task AppendSequenceDataAsync(
            string xorUrl,
            byte[] data)
            => AppBindings.AppendSequenceAsync(_appPtr, xorUrl, data);
    }
}
