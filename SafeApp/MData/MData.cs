using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MData {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal MData(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task DelUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey, ulong version) {
      return AppBindings.MDataDelUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, version);
    }

    public Task<List<byte>> EncodeMetadata(MetadataResponse metadataResponse) {
      return AppBindings.MDataEncodeMetadataAsync(ref metadataResponse);
    }

    public Task<(List<byte>, ulong)> GetValueAsync(MDataInfo mDataInfo, List<byte> key) {
      return AppBindings.MDataGetValueAsync(_appPtr, ref mDataInfo, key);
    }

    public Task<ulong> GetVersionAsync(MDataInfo mDataInfo) {
      return AppBindings.MDataGetVersionAsync(_appPtr, ref mDataInfo);
    }

    public Task<List<MDataEntry>> ListEntriesAsync(NativeHandle entriesHandle) {
      return AppBindings.MDataListEntriesAsync(_appPtr, entriesHandle);
    }

    public Task<List<MDataKey>> ListKeysAsync(MDataInfo mDataInfo) {
      return AppBindings.MDataListKeysAsync(_appPtr, ref mDataInfo);
    }

    public async Task<NativeHandle> ListPermissionsAsync(MDataInfo mDataInfo) {
      var handle = await AppBindings.MDataListPermissionsAsync(_appPtr, ref mDataInfo);
      return new NativeHandle(_appPtr, handle, freeHandle => AppBindings.MDataPermissionsFreeAsync(_appPtr, freeHandle));
    }

    public Task<PermissionSet> ListUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey) {
      return AppBindings.MDataListUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey);
    }

    public Task<List<MDataValue>> ListValuesAsync(MDataInfo mDataInfo) {
      return AppBindings.MDataListValuesAsync(_appPtr, ref mDataInfo);
    }

    public Task MutateEntriesAsync(MDataInfo mDataInfo, NativeHandle entryActionsH) {
      return AppBindings.MDataMutateEntriesAsync(_appPtr, ref mDataInfo, entryActionsH);
    }

    public Task PutAsync(MDataInfo mDataInfo, NativeHandle permissionsH, NativeHandle entriesH) {
      return AppBindings.MDataPutAsync(_appPtr, ref mDataInfo, permissionsH, entriesH);
    }

    public Task<ulong> SerialisedSizeAsync(MDataInfo mDataInfo) {
      return AppBindings.MDataSerialisedSizeAsync(_appPtr, ref mDataInfo);
    }

    public Task SetUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey, PermissionSet permissionSet, ulong version) {
      return AppBindings.MDataSetUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, ref permissionSet, version);
    }
  }
}
