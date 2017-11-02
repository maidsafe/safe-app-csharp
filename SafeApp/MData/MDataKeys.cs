using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataKeys {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<List<List<byte>>> ForEachAsync(NativeHandle entKeysH) {
      var tcs = new TaskCompletionSource<List<List<byte>>>();
      var keys = new List<List<byte>>();
      MDataKeysForEachCb forEachCb = (_, bytePtr, len) => {
        var key = bytePtr.ToList<byte>(len);
        keys.Add(key);
      };

      ResultCb forEachResCb = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(keys);
      };

      AppBindings.MDataKeysForEach(Session.AppPtr, entKeysH, forEachCb, forEachResCb);

      return tcs.Task;
    }

    public static Task FreeAsync(ulong entKeysH) {
      var tcs = new TaskCompletionSource<object>();
      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataKeysFree(Session.AppPtr, entKeysH, callback);

      return tcs.Task;
    }

    public static Task<IntPtr> LenAsync(NativeHandle mDataInfoH) {
      var tcs = new TaskCompletionSource<IntPtr>();
      IntPtrCb callback = (_, result, len) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(len);
      };

      AppBindings.MDataKeysLen(Session.AppPtr, mDataInfoH, callback);

      return tcs.Task;
    }
  }
}
