using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace SafeApp.MockAuthBindings
{
    [PublicAPI]
    class TestUtils
    {
      private static readonly IAuthBindings Bindings = MockAuthResolver.Current;

      public static Task TestSimulateNetworkDisconnectAsync(IntPtr appPtr)
      {
        return Bindings.TestSimulateNetworkDisconnectAsync(appPtr);
      }
    }
}
