﻿using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// Files API.
    /// </summary>
    public class Files
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initialise a Files object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="safeAppPtr"></param>
        internal Files(SafeAppPtr safeAppPtr)
            => _appPtr = safeAppPtr;

        /// <summary>
        /// Create a FilesContainer.
        /// </summary>
        /// <param name="location">Location of the local data.</param>
        /// <param name="dest">
        /// The XorUrl to put the data on the network.If null, then a random address will be used.
        /// </param>
        /// <param name="recursive">Flag denoting if the sub-folders should be added.</param>
        /// <param name="followLinks">If True, API will resolve the symlink before uploading,
        /// and will be stored as a file or directory on the network.
        /// When false, no path resolution is performed and the file will be stored as a symlink.</param>
        /// <param name="dryRun">Flag denoting whether container will be created locally.</param>
        /// <returns>
        /// FilesContainer's XorUrl,
        /// new instance of ProcessedFiles containing the list of processed files,
        /// FilesMap instance.
        /// </returns>
        public Task<(string, ProcessedFiles, FilesMap)> FilesContainerCreateAsync(
            string location,
            string dest,
            bool recursive,
            bool followLinks,
            bool dryRun)
            => AppBindings.FilesContainerCreateAsync(_appPtr, location, dest, recursive, followLinks, dryRun);

        /// <summary>
        /// Fetch an existing FilesContainer from the network.
        /// </summary>
        /// <param name="url">FilesContainer's XorUrl.</param>
        /// <returns>FilesContainer's version, FilesMap instance.</returns>
        public Task<(ulong, FilesMap)> FilesContainerGetAsync(
            string url)
            => AppBindings.FilesContainerGetAsync(_appPtr, url);

        /// <summary>
        /// Sync up local folder with the content in a FilesContainer.
        /// </summary>
        /// <param name="location">Location of the local data to sync with the network.</param>
        /// <param name="url">The XorUrl to sync the data on the network.</param>
        /// <param name="recursive">Flag denoting if the sub-folders should be added.</param>
        /// <param name="followLinks">If True, API will resolve the symlink before uploading,
        /// and will be stored as a file or directory on the network.
        /// When false, no path resolution is performed and the file will be stored as a symlink.</param>
        /// <param name="delete">Flag denoting if the local files can be deleting during sync operation.</param>
        /// <param name="updateNrs">Flag denoting if the NRS maps should be updated.</param>
        /// <param name="dryRun">Flag denoting whether container will be created locally.</param>
        /// <returns>
        /// FilesContainer's version,
        /// new instance of ProcessedFiles containing the list of processed files,
        /// FilesMap instance.
        /// </returns>
        public Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerSyncAsync(
            string location,
            string url,
            bool recursive,
            bool followLinks,
            bool delete,
            bool updateNrs,
            bool dryRun)
            => AppBindings.FilesContainerSyncAsync(_appPtr, location, url, recursive, followLinks, delete, updateNrs, dryRun);

        /// <summary>
        /// Add a file, either a local path or a published file, to an existing FilesContainer.
        /// </summary>
        /// <param name="sourceFile">Source file location to add to the FilesContainer.</param>
        /// <param name="url">XorUrl to add the FiledContainer.</param>
        /// <param name="force">Flag denoting to force update the FilesContainer.</param>
        /// <param name="updateNrs">Flag denoting if the NRS maps should be updated.</param>
        /// <param name="followLinks">If True, API will resolve the symlink before uploading,
        /// and will be stored as a file or directory on the network.
        /// When false, no path resolution is performed and the file will be stored as a symlink.</param>
        /// <param name="dryRun">Flag denoting whether container will be created locally.</param>
        /// <returns>
        /// FilesContainer's version,
        /// new instance of ProcessedFiles containing the list of processed files,
        /// FilesMap instance.
        /// </returns>
        public Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerAddAsync(
            string sourceFile,
            string url,
            bool force,
            bool updateNrs,
            bool followLinks,
            bool dryRun)
            => AppBindings.FilesContainerAddAsync(_appPtr, sourceFile, url, force, updateNrs, followLinks, dryRun);

        /// <summary>
        /// Remove a file from an existing FilesContainer..
        /// </summary>
        /// <param name="url">XorUrl to remove the FiledContainer.</param>
        /// <param name="recursive">Flag denoting if the sub-folders should be removed.</param>
        /// <param name="updateNrs">Flag denoting if the NRS maps should be updated.</param>
        /// <param name="dryRun">Flag denoting whether container will be created locally.</param>
        /// <returns>
        /// FilesContainer's version,
        /// new instance of ProcessedFiles containing the list of processed files,
        /// FilesMap instance.
        /// </returns>
        public Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerRemovePathAsync(
            string url,
            bool recursive,
            bool updateNrs,
            bool dryRun)
            => AppBindings.FilesContainerRemovePathAsync(_appPtr, url, recursive, updateNrs, dryRun);

        /// <summary>
        /// Add a file, from raw bytes, on an existing FilesContainer.
        /// </summary>
        /// <param name="data">Raw data in byte[] format.</param>
        /// <param name="url">XorUrl to add the FiledContainer.</param>
        /// <param name="force">Flag denoting to force update the FilesContainer.</param>
        /// <param name="updateNrs">Flag denoting if the NRS maps should be updated.</param>
        /// <param name="dryRun">Flag denoting whether container will be created locally.</param>
        /// <returns>
        /// FilesContainer's version,
        /// new instance of ProcessedFiles containing the list of processed files,
        /// FilesMap instance.
        /// </returns>
        public Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerAddFromRawAsync(
            byte[] data,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun)
            => AppBindings.FilesContainerAddFromRawAsync(_appPtr, data, url, force, updateNrs, dryRun);

        /// <summary>
        /// Store public immutable data on the network.
        /// </summary>
        /// <param name="data">Raw data in byte[] format.</param>
        /// <param name="mediaType">Content's MIME type.</param>
        /// <param name="dryRun">Flag denoting whether container will be created locally.</param>
        /// <returns>XorUrl for the published data</returns>
        public Task<string> FilesPutPublicImmutableAsync(
            byte[] data,
            string mediaType,
            bool dryRun)
            => AppBindings.FilesPutPublicImmutableAsync(_appPtr, data, mediaType, dryRun);

        /// <summary>
        /// Get public immutable data from the network.
        /// </summary>
        /// <param name="url">XorUrl to fetch the content.</param>
        /// <param name="start">Start index to fetch the content.</param>
        /// <param name="end">End index to fetch the content.</param>
        /// <returns>Raw data from the network in byte[] format.</returns>
        public Task<byte[]> FilesGetPublicImmutableAsync(
            string url,
            ulong start = 0,
            ulong end = 0)
            => AppBindings.FilesGetPublicImmutableAsync(_appPtr, url, start, end);
    }
}
