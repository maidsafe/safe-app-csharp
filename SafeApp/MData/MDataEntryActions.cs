using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataEntryActions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal MDataEntryActions(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task DeleteAsync(NativeHandle entryActionsH, List<byte> entKey, ulong version) {
      return AppBindings.MDataEntryActionsDeleteAsync(_appPtr, entryActionsH, entKey, version);
    }

    private Task FreeAsync(ulong entryActionsH) {
      return AppBindings.MDataEntryActionsFreeAsync(_appPtr, entryActionsH);
    }

    public Task InsertAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal) {
      return AppBindings.MDataEntryActionsInsertAsync(_appPtr, entryActionsH, entKey, entVal);
    }

    public async Task<NativeHandle> NewAsync() {
      var entryActionsH = await AppBindings.MDataEntryActionsNewAsync(_appPtr);
      return new NativeHandle(_appPtr, entryActionsH, FreeAsync);
    }

    public Task UpdateAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal, ulong version) {
      return AppBindings.MDataEntryActionsUpdateAsync(_appPtr, entryActionsH, entKey, entVal, version);
    }
  }
}
