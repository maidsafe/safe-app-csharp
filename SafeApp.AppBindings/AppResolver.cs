using System;
using System.Threading;
using SafeApp.Utilities;

namespace SafeApp.AppBindings {
  public static class AppResolver {
    private static readonly Lazy<IAppBindings> Implementation = new Lazy<IAppBindings>(
      CreateBindings,
      LazyThreadSafetyMode.PublicationOnly);

    public static IAppBindings Current {
      get {
        var ret = Implementation.Value;
        if (ret == null) {
          throw NotImplementedInReferenceAssembly();
        }

        return ret;
      }
    }

    private static IAppBindings CreateBindings() {
#if NETSTANDARD1_2 && !__DESKTOP__
      return null;
#else
      return new AppBindings();
#endif
    }

    private static Exception NotImplementedInReferenceAssembly() {
      return new NotImplementedException(
        "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
