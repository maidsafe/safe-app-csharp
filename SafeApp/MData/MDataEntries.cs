using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataEntries {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal MDataEntries(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    private Task FreeAsync(ulong entriesH) {
      return AppBindings.MDataEntriesFreeAsync(_appPtr, entriesH);
    }

    public Task<(List<byte>, ulong)> GetAsync(NativeHandle entriesHandle, List<byte> key) {
      return AppBindings.MDataEntriesGetAsync(_appPtr, entriesHandle, key);
    }

    public async Task<NativeHandle> GetHandleAsync(MDataInfo mDataInfo) {
      var handle = await AppBindings.MDataEntriesAsync(_appPtr, ref mDataInfo);
      return new NativeHandle(_appPtr, handle, FreeAsync);
    }

    public Task InsertAsync(NativeHandle entriesH, List<byte> entKey, List<byte> entVal) {
      return AppBindings.MDataEntriesInsertAsync(_appPtr, entriesH, entKey, entVal);
    }

    public Task<ulong> LenAsync(NativeHandle entriesHandle) {
      return AppBindings.MDataEntriesLenAsync(_appPtr, entriesHandle);
    }

    public async Task<NativeHandle> NewAsync() {
      var entriesH = await AppBindings.MDataEntriesNewAsync(_appPtr);
      return new NativeHandle(_appPtr, entriesH, FreeAsync);
    }
  }
}
