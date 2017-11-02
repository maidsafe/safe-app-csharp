using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {
  public static class NativeUtils {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<List<byte>> Sha3HashAsync(List<byte> source) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var sourcePtr = source.ToIntPtr();
      ByteArrayCb callback = (_, result, digestPtr, digestLen) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(digestPtr.ToList<byte>(digestLen));
      };

      AppBindings.Sha3Hash(sourcePtr, (IntPtr)source.Count, callback);
      Marshal.FreeHGlobal(sourcePtr);

      return tcs.Task;
    }
  }
}
