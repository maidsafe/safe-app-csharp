using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable InconsistentNaming
namespace SafeApp.MData
{
    /// <summary>
    /// Provides the Mutable Data APIs.
    /// </summary>
    [PublicAPI]
    public class MData
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initialises an MData object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal MData(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Delete the permissions of a specific key.
        /// Directly commits to the networks.
        /// Required 'ManagePermissions'-Permission for the app.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <param name="userSignPubKey">The key to lookup for.</param>
        /// <param name="version">The version successor, to handle the concurrency issue.</param>
        /// <returns></returns>
        public Task DelUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey, ulong version)
        {
            return AppBindings.MDataDelUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, version);
        }

        /// <summary>
        /// Encode the Mutable Data metadata response.
        /// </summary>
        /// <param name="metadataResponse">Metadata for user Mutable Data.</param>
        /// <returns>Encoded metadata.</returns>
        public Task<List<byte>> EncodeMetadata(MetadataResponse metadataResponse)
        {
            return AppBindings.MDataEncodeMetadataAsync(ref metadataResponse);
        }

        /// <summary>
        /// Get the value of a specific key.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <param name="key">Mutable Data entry key.</param>
        /// <returns>Mutable Data entry value and its current version.</returns>
        public Task<(List<byte>, ulong)> GetValueAsync(MDataInfo mDataInfo, List<byte> key)
        {
            return AppBindings.MDataGetValueAsync(_appPtr, ref mDataInfo, key);
        }

        /// <summary>
        /// Get the Mutable Data version on the network.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <returns>Mutable Data version.</returns>
        public Task<ulong> GetVersionAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataGetVersionAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Get the list of Mutable Data entries associated with the handle.
        /// </summary>
        /// <param name="entriesHandle">Mutable Data entry handle.</param>
        /// <returns>List of Mutable Data Entries.</returns>
        public Task<List<MDataEntry>> ListEntriesAsync(NativeHandle entriesHandle)
        {
            return AppBindings.MDataListEntriesAsync(_appPtr, entriesHandle);
        }

        /// <summary>
        /// Get the list of keys contained in Mutable Data.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <returns>List of Mutable Data keys.</returns>
        public Task<List<MDataKey>> ListKeysAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataListKeysAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Get a permission handle for the permissions associated with Mutable Data.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <returns>Permission handle for Mutable Data.</returns>
        public async Task<NativeHandle> ListPermissionsAsync(MDataInfo mDataInfo)
        {
            var handle = await AppBindings.MDataListPermissionsAsync(_appPtr, ref mDataInfo);
            return new NativeHandle(_appPtr, handle, freeHandle => AppBindings.MDataPermissionsFreeAsync(_appPtr, freeHandle));
        }

        /// <summary>
        /// Get a permission handle for the permissions associated with MutableData for a specific Public Sign Key.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <param name="userSignPubKey">User public signing key.</param>
        /// <returns>Permission set for user for a Mutable Data.</returns>
        public Task<PermissionSet> ListUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey)
        {
            return AppBindings.MDataListUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey);
        }

        /// <summary>
        /// Get the list of values from a MutableData.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <returns>List of Mutable Data values.</returns>
        public Task<List<MDataValue>> ListValuesAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataListValuesAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Commit the transaction to the network.
        /// </summary>
        /// <param name="mDataInfo">MData info of Mutable Data.</param>
        /// <param name="entryActionsH">Entry action handle.</param>
        /// <returns></returns>
        public Task MutateEntriesAsync(MDataInfo mDataInfo, NativeHandle entryActionsH)
        {
            return AppBindings.MDataMutateEntriesAsync(_appPtr, ref mDataInfo, entryActionsH);
        }

        /// <summary>
        /// Commit this Mutable Data to the network.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <param name="permissionsH">Permissions to create the Mutable Data with.</param>
        /// <param name="entriesH">Mutable Data entries to create the Mutable Data with.</param>
        /// <returns></returns>
        public Task PutAsync(MDataInfo mDataInfo, NativeHandle permissionsH, NativeHandle entriesH)
        {
            return AppBindings.MDataPutAsync(_appPtr, ref mDataInfo, permissionsH, entriesH);
        }

        /// <summary>
        /// Get the size of serialised Mutable Data.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <returns>Size of Mutable Data.</returns>
        public Task<ulong> SerialisedSizeAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataSerialisedSizeAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Set the permissions of a specific key. Directly commits to the network.
        /// Requires 'ManagePermissions'-Permission for the app.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access Mutable Data.</param>
        /// <param name="userSignPubKey">User Public Sign Key.</param>
        /// <param name="permissionSet">PermissionSet to set to.</param>
        /// <param name="version">The version successor, to handle the concurrency issue.</param>
        /// <returns></returns>
        public Task SetUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey, PermissionSet permissionSet, ulong version)
        {
            return AppBindings.MDataSetUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, ref permissionSet, version);
        }
    }
}
