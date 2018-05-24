using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  [PublicAPI]
  public class AccessContainer {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal AccessContainer(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    public async Task<List<ContainerPermissions>> AccessContainerFetchAsync() {
      var array = await AppBindings.AccessContainerFetchAsync(_appPtr);
      return array.ToList();
    }

    public Task<MDataInfo> GetMDataInfoAsync(string containerId) {
      return AppBindings.AccessContainerGetContainerMDataInfoAsync(_appPtr, containerId);
    }

    public Task RefreshAccessInfoAsync() {
      return AppBindings.AccessContainerRefreshAccessInfoAsync(_appPtr);
    }
  }
}
