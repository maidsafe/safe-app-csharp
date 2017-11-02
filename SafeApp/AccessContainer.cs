using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.MData;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  public static class AccessContainer {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<NativeHandle> GetMDataInfoAsync(string containerId) {
      var tcs = new TaskCompletionSource<NativeHandle>();

      UlongCb callback = (_, result, mdataInfoH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(mdataInfoH, MDataInfo.FreeAsync));
      };

      AppBindings.AccessContainerGetContainerMDataInfo(Session.AppPtr, containerId, callback);

      return tcs.Task;
    }
  }
}
