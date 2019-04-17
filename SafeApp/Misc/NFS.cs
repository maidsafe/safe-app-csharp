using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable InconsistentNaming
namespace SafeApp.Misc
{
    /// <summary>
    /// NFS (Network File System) Emulation APIs on top of an MutableData.
    /// </summary>
    [PublicAPI]

    // ReSharper disable once InconsistentNaming
    public class NFS
    {
        /// <summary>
        /// Open Modes are used while opening a file for reading or writing.
        /// </summary>
        [PublicAPI]
        public enum OpenMode
        {
            /// <summary>
            /// Appends to existing data in the file.
            /// </summary>
            Append = 2,

            /// <summary>
            /// Replaces the entire content of the file.
            /// </summary>
            Overwrite = 1,

            /// <summary>
            /// Open file to read.
            /// </summary>
            Read = 4
        }

        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initialises an NFS object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr"></param>
        internal NFS(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Delete a file from path.
        /// Directly commit to the network.
        /// </summary>
        /// <param name="mDataInfo">mdataInfo to access Mutable Data.</param>
        /// <param name="fileName">The path/file name.</param>
        /// <param name="version">The version successor, to handle the concurrency issue.</param>
        /// <returns>The version successor for the deleted file.</returns>
        public Task<ulong> DirDeleteFileAsync(MDataInfo mDataInfo, string fileName, ulong version)
        {
            return AppBindings.DirDeleteFileAsync(_appPtr, ref mDataInfo, fileName, version);
        }

        /// <summary>
        /// Get the file from the directory.
        /// </summary>
        /// <param name="mDataInfo">mdataInfo to access Mutable Data.</param>
        /// <param name="fileName">The path/file name.</param>
        /// <returns>The file found for the given path.</returns>
        public Task<(File, ulong)> DirFetchFileAsync(MDataInfo mDataInfo, string fileName)
        {
            return AppBindings.DirFetchFileAsync(_appPtr, ref mDataInfo, fileName);
        }

        /// <summary>
        /// Insert the given file in the directory.
        /// Directly commit to the network.
        /// </summary>
        /// <param name="mDataInfo">mdataInfo to access Mutable Data.</param>
        /// <param name="fileName">The path to store the file under.</param>
        /// <param name="file">The file to Serialise and store.</param>
        /// <returns></returns>
        public Task DirInsertFileAsync(MDataInfo mDataInfo, string fileName, File file)
        {
            return AppBindings.DirInsertFileAsync(_appPtr, ref mDataInfo, fileName, ref file);
        }

        /// <summary>
        /// Replace the existing file with a new file.
        /// Directly commit to the network.
        /// </summary>
        /// <param name="mDataInfo">mdataInfo to access Mutable Data.</param>
        /// <param name="fileName">The path to store the file under.</param>
        /// <param name="file">The file to Serialise and store.</param>
        /// <param name="version">Version successor, to handle the concurrency issue.</param>
        /// <returns>The version successor for the updated file.</returns>
        public Task<ulong> DirUpdateFileAsync(MDataInfo mDataInfo, string fileName, File file, ulong version)
        {
            return AppBindings.DirUpdateFileAsync(_appPtr, ref mDataInfo, fileName, ref file, version);
        }

        /// <summary>
        /// Close a file after reading or writing operation.
        /// </summary>
        /// <param name="fileContextHandle"></param>
        /// <returns></returns>
        public Task<File> FileCloseAsync(NativeHandle fileContextHandle)
        {
            return AppBindings.FileCloseAsync(_appPtr, fileContextHandle);
        }

        /// <summary>
        /// Open a file for reading or writing.
        /// </summary>
        /// <param name="mDataInfo">mdataInfo to access Mutable Data.</param>
        /// <param name="file">The file to be opened.</param>
        /// <param name="openMode">File opening Mode.</param>
        /// <returns></returns>
        public async Task<NativeHandle> FileOpenAsync(MDataInfo mDataInfo, File file, OpenMode openMode)
        {
            var fileContextHandle = await AppBindings.FileOpenAsync(_appPtr, ref mDataInfo, ref file, (ulong)openMode);
            return new NativeHandle(
              _appPtr,
              fileContextHandle,
              handle => { return openMode == OpenMode.Read ? AppBindings.FileCloseAsync(_appPtr, handle) : Task.Run(() => { }); });
        }

        /// <summary>
        /// Read the file content for a specified range.
        /// </summary>
        /// <param name="fileContextHandle">File handle to access file.</param>
        /// <param name="start">Start position.</param>
        /// <param name="end">End position.</param>
        /// <returns>Content of file in provided range.</returns>
        public Task<List<byte>> FileReadAsync(NativeHandle fileContextHandle, ulong start, ulong end)
        {
            return AppBindings.FileReadAsync(_appPtr, fileContextHandle, start, end);
        }

        /// <summary>
        /// Get the file size.
        /// </summary>
        /// <param name="fileContextHandle">File handle.</param>
        /// <returns>File size.</returns>
        public Task<ulong> FileSizeAsync(NativeHandle fileContextHandle)
        {
            return AppBindings.FileSizeAsync(_appPtr, fileContextHandle);
        }

        /// <summary>
        /// Write file.
        /// File will be created on network only when FileCloseAsync is invoked.
        /// </summary>
        /// <param name="fileContextHandle">File handle.</param>
        /// <param name="data">Data to write in file.</param>
        /// <returns></returns>
        public Task FileWriteAsync(NativeHandle fileContextHandle, List<byte> data)
        {
            return AppBindings.FileWriteAsync(_appPtr, fileContextHandle, data);
        }
    }
}
