using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;

// ReSharper disable ConvertToLocalFunction

// ReSharper disable InconsistentNaming

namespace SafeApp.IData
{
    /// <summary>
    /// Immutable Data APIs.
    /// </summary>
    [PublicAPI]
    public class IData
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an IData object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal IData(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Close the writer.
        /// When invoked, the DataMap is generated and saved in the network.
        /// The address of the DataMap is returned on success.
        /// </summary>
        /// <param name="seH"></param>
        /// <param name="cipherOptH"></param>
        /// <returns>Address as XOR Name in byte array format.</returns>
        public Task<byte[]> CloseSelfEncryptorAsync(ulong seH, NativeHandle cipherOptH)
        {
            return AppBindings.IDataCloseSelfEncryptorAsync(_appPtr, seH, cipherOptH);
        }

        /// <summary>
        /// Fetch an existing Immutable Data for the given address
        /// to prepare a reader to read the Immutable Data chunks from the network.
        /// </summary>
        /// <param name="xorName">The XOR Name on the network.</param>
        /// <returns>SE handle</returns>
        public async Task<NativeHandle> FetchSelfEncryptorAsync(byte[] xorName)
        {
            var sEReaderHandle = await AppBindings.IDataFetchSelfEncryptorAsync(_appPtr, xorName);
            return new NativeHandle(_appPtr, sEReaderHandle, SelfEncryptorReaderFreeAsync);
        }

        /// <summary>
        /// Create a new writer instance for storing Immutable Data in the network.
        /// </summary>
        /// <returns>New writer NativeHandle.</returns>
        public async Task<NativeHandle> NewSelfEncryptorAsync()
        {
            var sEWriterHandle = await AppBindings.IDataNewSelfEncryptorAsync(_appPtr);
            return new NativeHandle(_appPtr, sEWriterHandle, null);
        }

        /// <summary>
        /// Read the data for a specific range.
        /// </summary>
        /// <param name="seHandle">Reader handle.</param>
        /// <param name="fromPos">Start position.</param>
        /// <param name="len">End position or end of data.</param>
        /// <returns></returns>
        public async Task<List<byte>> ReadFromSelfEncryptorAsync(NativeHandle seHandle, ulong fromPos, ulong len)
        {
            var dataArray = await AppBindings.IDataReadFromSelfEncryptorAsync(_appPtr, seHandle, fromPos, len);
            return new List<byte>(dataArray);
        }

        /// <summary>
        /// Invoked to free the reader reference from memory.
        /// </summary>
        /// <param name="sEReaderHandle">Reader handle.</param>
        /// <returns></returns>
        private Task SelfEncryptorReaderFreeAsync(ulong sEReaderHandle)
        {
            return AppBindings.IDataSelfEncryptorReaderFreeAsync(_appPtr, sEReaderHandle);
        }

        /// <summary>
        /// Invoked to free the writer reference from memory.
        /// </summary>
        /// <param name="sEWriterHandle">Writer handle of Immutable Data.</param>
        /// <returns></returns>
        private Task SelfEncryptorWriterFreeAsync(ulong sEWriterHandle)
        {
            return AppBindings.IDataSelfEncryptorWriterFreeAsync(_appPtr, sEWriterHandle);
        }

        /// <summary>
        /// The size of the serialised Immutable Data.
        /// </summary>
        /// <param name="xorName">XOR address of Immutable Data.</param>
        /// <returns>Length in bytes.</returns>
        public Task<ulong> SerialisedSizeAsync(byte[] xorName)
        {
            return AppBindings.IDataSerialisedSizeAsync(_appPtr, xorName);
        }

        /// <summary>
        /// Get the size of the Immutable Data.
        /// </summary>
        /// <param name="seHandle">SE handle.</param>
        /// <returns>Length in bytes.</returns>
        public Task<ulong> SizeAsync(NativeHandle seHandle)
        {
            return AppBindings.IDataSizeAsync(_appPtr, seHandle);
        }

        /// <summary>
        /// Write Immutable Data.
        /// Only when CloseSelfEncryptorAsync is invoked, the DataMap is generated and saved in the network.
        /// </summary>
        /// <param name="seHandle">SE handle</param>
        /// <param name="data">Data to append in existing Immutable Data.</param>
        /// <returns></returns>
        public Task WriteToSelfEncryptorAsync(NativeHandle seHandle, List<byte> data)
        {
            return AppBindings.IDataWriteToSelfEncryptorAsync(_appPtr, seHandle, data);
        }
    }
}
