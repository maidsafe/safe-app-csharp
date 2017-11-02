using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataPermissionSet {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task AllowAsync(NativeHandle permissionSetH, MDataAction allowAction) {
      var tcs = new TaskCompletionSource<object>();
      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataPermissionSetAllow(Session.AppPtr, permissionSetH, allowAction, callback);

      return tcs.Task;
    }

    public static Task FreeAsync(ulong permissionSetH) {
      var tcs = new TaskCompletionSource<object>();
      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataPermissionSetFree(Session.AppPtr, permissionSetH, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewAsync() {
      var tcs = new TaskCompletionSource<NativeHandle>();

      UlongCb callback = (_, result, permissionSetH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(permissionSetH, FreeAsync));
      };

      AppBindings.MDataPermissionSetNew(Session.AppPtr, callback);

      return tcs.Task;
    }
  }
}
