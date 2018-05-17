using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataPermissions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal MDataPermissions(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    private Task FreeAsync(ulong permissionsH) {
      return AppBindings.MDataPermissionsFreeAsync(_appPtr, permissionsH);
    }

    public Task<PermissionSet> GetAsync(NativeHandle permissionsHandle, NativeHandle userPubSignKey) {
      return AppBindings.MDataPermissionsGetAsync(_appPtr, permissionsHandle, userPubSignKey);
    }

    public Task InsertAsync(NativeHandle permissionsH, NativeHandle forUserH, PermissionSet permissionSet) {
      return AppBindings.MDataPermissionsInsertAsync(_appPtr, permissionsH, forUserH, ref permissionSet);
    }

    public Task<ulong> LenAsync(NativeHandle permissionsHandle) {
      return AppBindings.MDataPermissionsLenAsync(_appPtr, permissionsHandle);
    }

    public async Task<List<(NativeHandle, PermissionSet)>> ListAsync(NativeHandle permissionHandle) {
      var userPermissions = await AppBindings.MDataListPermissionSetsAsync(_appPtr, permissionHandle);
      return userPermissions.Select(
        userPermission => {
          var userHandle = new NativeHandle(_appPtr, userPermission.UserH, handle => AppBindings.SignPubKeyFreeAsync(_appPtr, handle));
          return (userHandle, userPermission.PermSet);
        }).ToList();
    }

    public async Task<NativeHandle> NewAsync() {
      var permissionsH = await AppBindings.MDataPermissionsNewAsync(_appPtr);
      return new NativeHandle(_appPtr, permissionsH, FreeAsync);
    }
  }
}
