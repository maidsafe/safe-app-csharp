using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MData {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<(List<byte>, ulong)> GetValueAsync(NativeHandle infoHandle, List<byte> key) {
      var tcs = new TaskCompletionSource<(List<byte>, ulong)>();
      var keyPtr = key.ToIntPtr();
      MDataGetValueCb callback = (_, result, dataPtr, dataLen, entryVersion) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        var data = dataPtr.ToList<byte>(dataLen);
        tcs.SetResult((data, entryVersion));
      };

      AppBindings.MDataGetValue(Session.AppPtr, infoHandle, keyPtr, (IntPtr)key.Count, callback);
      Marshal.FreeHGlobal(keyPtr);

      return tcs.Task;
    }

    public static Task<NativeHandle> ListEntriesAsync(NativeHandle infoHandle) {
      var tcs = new TaskCompletionSource<NativeHandle>();
      UlongCb callback = (_, result, mDataEntriesHandle) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(mDataEntriesHandle, MDataEntries.FreeAsync));
      };

      AppBindings.MDataListEntries(Session.AppPtr, infoHandle, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> ListKeysAsync(NativeHandle mDataInfoH) {
      var tcs = new TaskCompletionSource<NativeHandle>();
      UlongCb callback = (_, result, mDataEntKeysH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(mDataEntKeysH, MDataKeys.FreeAsync));
      };

      AppBindings.MDataListKeys(Session.AppPtr, mDataInfoH, callback);

      return tcs.Task;
    }

    public static Task MutateEntriesAsync(NativeHandle mDataInfoH, NativeHandle entryActionsH) {
      var tcs = new TaskCompletionSource<object>();
      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataMutateEntries(Session.AppPtr, mDataInfoH, entryActionsH, callback);

      return tcs.Task;
    }

    public static Task PutAsync(NativeHandle mDataInfoH, NativeHandle permissionsH, NativeHandle entriesH) {
      var tcs = new TaskCompletionSource<object>();
      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataPut(Session.AppPtr, mDataInfoH, permissionsH, entriesH, callback);

      return tcs.Task;
    }
  }
}
