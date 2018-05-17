using System;
using System.Threading;

namespace SafeApp.MockAuthBindings {
  internal static class MockAuthResolver {
    private static readonly Lazy<IAuthBindings> Implementation = new Lazy<IAuthBindings>(
      CreateBindings,
      LazyThreadSafetyMode.PublicationOnly);

    internal static IAuthBindings Current {
      get {
        var ret = Implementation.Value;
        if (ret == null) {
          throw NotImplementedInReferenceAssembly();
        }

        return ret;
      }
    }

    private static IAuthBindings CreateBindings() {
#if NETSTANDARD1_2 && !__DESKTOP__
      return null;
#else
      return new AuthBindings();
#endif
    }

    private static Exception NotImplementedInReferenceAssembly() {
      return new NotImplementedException(
        "Please ensure you have SAFE_APP_MOCK defined in the application project as well. " +
        "You should also have a reference to the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
