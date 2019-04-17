using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable InconsistentNaming
namespace SafeApp.MData
{
    /// <summary>
    /// Mutable Data Permission APIs.
    /// </summary>
    [PublicAPI]
    public class MDataPermissions
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initialises an MDataPermissions object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal MDataPermissions(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        private Task FreeAsync(ulong permissionsH)
        {
            return AppBindings.MDataPermissionsFreeAsync(_appPtr, permissionsH);
        }

        /// <summary>
        /// Get the permissions granted for a Public Sign Key.
        /// </summary>
        /// <param name="permissionsHandle">Permission handle.</param>
        /// <param name="userPubSignKey">The key to lookup for.</param>
        /// <returns>The permission set for the key.</returns>
        public Task<PermissionSet> GetAsync(NativeHandle permissionsHandle, NativeHandle userPubSignKey)
        {
            return AppBindings.MDataPermissionsGetAsync(_appPtr, permissionsHandle, userPubSignKey);
        }

        /// <summary>
        /// Insert a new permission set mapped to a specific Sign Key.
        /// Directly commits to the network.
        /// Requires 'ManagePermissions'-Permission for the app.
        /// </summary>
        /// <param name="permissionsH">Permission handle.</param>
        /// <param name="forUserH">User handle to insert permissions for.</param>
        /// <param name="permissionSet">The permission set to insert.</param>
        /// <returns></returns>
        public Task InsertAsync(NativeHandle permissionsH, NativeHandle forUserH, PermissionSet permissionSet)
        {
            return AppBindings.MDataPermissionsInsertAsync(_appPtr, permissionsH, forUserH, ref permissionSet);
        }

        /// <summary>
        /// Total number of permission entries.
        /// </summary>
        /// <param name="permissionsHandle">Permission handle.</param>
        /// <returns>Number of permission entries.</returns>
        public Task<ulong> LenAsync(NativeHandle permissionsHandle)
        {
            return AppBindings.MDataPermissionsLenAsync(_appPtr, permissionsHandle);
        }

        /// <summary>
        /// Returns the list of all associated permission sets.
        /// </summary>
        /// <param name="permissionHandle">Permission handle.</param>
        /// <returns>List of permission sets an associated handles.</returns>
        public async Task<List<(NativeHandle, PermissionSet)>> ListAsync(NativeHandle permissionHandle)
        {
            var userPermissions = await AppBindings.MDataListPermissionSetsAsync(_appPtr, permissionHandle);
            return userPermissions.Select(
              userPermission =>
              {
                  var userHandle = new NativeHandle(_appPtr, userPermission.UserH, handle => AppBindings.SignPubKeyFreeAsync(_appPtr, handle));
                  return (userHandle, userPermission.PermSet);
              }).ToList();
        }

        /// <summary>
        /// Create a PermissionHandle to to insert permissions.
        /// </summary>
        /// <returns>Newly create Mutable Data permissions handle.</returns>
        public async Task<NativeHandle> NewAsync()
        {
            var permissionsH = await AppBindings.MDataPermissionsNewAsync(_appPtr);
            return new NativeHandle(_appPtr, permissionsH, FreeAsync);
        }
    }
}
