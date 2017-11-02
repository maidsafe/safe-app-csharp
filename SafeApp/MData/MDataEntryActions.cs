using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataEntryActions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong entryActionsH) {
      var tcs = new TaskCompletionSource<object>();
      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataEntryActionsFree(Session.AppPtr, entryActionsH, callback);

      return tcs.Task;
    }

    public static Task InsertAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal) {
      var tcs = new TaskCompletionSource<object>();

      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      var entKeyPtr = entKey.ToIntPtr();
      var entValPtr = entVal.ToIntPtr();

      AppBindings.MDataEntryActionsInsert(
        Session.AppPtr,
        entryActionsH,
        entKeyPtr,
        (IntPtr)entKey.Count,
        entValPtr,
        (IntPtr)entVal.Count,
        callback);

      Marshal.FreeHGlobal(entKeyPtr);
      Marshal.FreeHGlobal(entValPtr);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewAsync() {
      var tcs = new TaskCompletionSource<NativeHandle>();

      UlongCb callback = (_, result, entryActionsH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(entryActionsH, FreeAsync));
      };

      AppBindings.MDataEntryActionsNew(Session.AppPtr, callback);

      return tcs.Task;
    }
  }
}
