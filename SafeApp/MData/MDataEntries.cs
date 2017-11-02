using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataEntries {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<List<(List<byte>, List<byte>, ulong)>> ForEachAsync(NativeHandle entH) {
      var tcs = new TaskCompletionSource<List<(List<byte>, List<byte>, ulong)>>();
      var entries = new List<(List<byte>, List<byte>, ulong)>();

      MDataEntriesForEachCb forEachCb = (_, entryKeyPtr, entryKeyLen, entryValPtr, entryValLen, entryVersion) => {
        var entryKey = entryKeyPtr.ToList<byte>(entryKeyLen);
        var entryVal = entryValPtr.ToList<byte>(entryValLen);
        entries.Add((entryKey, entryVal, entryVersion));
      };

      ListBasedResultCb forEachResCb = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(entries);
      };

      AppBindings.MDataEntriesForEach(Session.AppPtr, entH, forEachCb, forEachResCb);

      return tcs.Task;
    }

    public static Task FreeAsync(ulong entriesH) {
      var tcs = new TaskCompletionSource<object>();
      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataEntriesFree(Session.AppPtr, entriesH, callback);

      return tcs.Task;
    }

    public static Task InsertAsync(NativeHandle entriesH, List<byte> entKey, List<byte> entVal) {
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

      AppBindings.MDataEntriesInsert(Session.AppPtr, entriesH, entKeyPtr, (IntPtr)entKey.Count, entValPtr, (IntPtr)entVal.Count, callback);

      Marshal.FreeHGlobal(entKeyPtr);
      Marshal.FreeHGlobal(entValPtr);

      return tcs.Task;
    }

    public static Task<ulong> LenAsync(NativeHandle entriesHandle) {
      var tcs = new TaskCompletionSource<ulong>();
      MDataEntriesLenCb callback = (_, len) => {
        // TODO: no result?

        tcs.SetResult(len);
      };

      AppBindings.MDataEntriesLen(Session.AppPtr, entriesHandle, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewAsync() {
      var tcs = new TaskCompletionSource<NativeHandle>();

      UlongCb callback = (_, result, entriesH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(entriesH, FreeAsync));
      };

      AppBindings.MDataEntriesNew(Session.AppPtr, callback);

      return tcs.Task;
    }
  }
}
