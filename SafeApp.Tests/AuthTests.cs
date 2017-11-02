using System.Collections.Generic;
using NUnit.Framework;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class AuthTest {
    private const string AuthUri =
        "safe-bmV0Lm1haWRzYWZlLnRlc3Qud2ViYXBwLmlk:AQAAAGSv7oQAAAAAAAAAACAAAAAAAAAAGQ1zYg9iFKof2TVkAPp0R2kjU9DDWmmR_uAXBYvaeIAgAAAAAAAAAKecZc5pOSeoU53v43RdoTscGQbuAO0hF6HA_4ou9GJnIAAAAAAAAADsycX-1RCaNJxnYf6ka1pLncSez4w4PmPIS5lau_IblkAAAAAAAAAAbZdkFJ6Ydhh_OwA7mfYcnta_95k2xRazJsDSeMFGj3vsycX-1RCaNJxnYf6ka1pLncSez4w4PmPIS5lau_IbliAAAAAAAAAAx559E774w-6AWnIXBSm0NWOBW2zr8TOPThmdIeEsoFEgAAAAAAAAAHRNdser-WDOLIBGsDfRbNI304vnYILXI1JZC96tiFvzAAAAAAAAAAAAAAAAAAAAAG7Di2O1ssjN0izb88iclOKj7WD5LtaVriMIrLBbVRHimDoAAAAAAAAYAAAAAAAAAH2p2f2I4yuQPLkSJE_u9-PtM1WD7E65ZA=="
      ;

    private AppExchangeInfo GetExchangeInfo() {
      return new AppExchangeInfo {Id = "net.maidsafe.example", Name = "Test App", Vendor = "Maidsafe Ltd."};
    }

    [Test]
    public async void ConnectAsRegisteredApp() {
      var result = await Session.DecodeIpcMessageAsync(AuthUri);
      Assert.NotNull(result.AuthGranted);
      var isConnected = result.AuthGranted != null &&
                        await Session.AppRegisteredAsync("bmV0Lm1haWRzYWZlLnRlc3Qud2ViYXBwLmlk", result.AuthGranted.Value);
      Assert.IsTrue(isConnected);
    }

    [Test]
    public async void DecodeAuthRequest() {
      var result = await Session.DecodeIpcMessageAsync(AuthUri);
      Assert.NotNull(result.AuthGranted);
    }

    [Test]
    public void EncodeAuthRequestWithContainersAsNull() {
      var authReq = new AuthReq {AppContainer = false, AppExchangeInfo = GetExchangeInfo(), Containers = null};
      Assert.DoesNotThrow(async () => await Session.EncodeAuthReqAsync(authReq));
    }

    [Test]
    public void EncodeAuthRequestWithEmptyContainers() {
      var authReq = new AuthReq {AppContainer = false, AppExchangeInfo = GetExchangeInfo(), Containers = new List<ContainerPermissions>()};
      Assert.DoesNotThrow(async () => await Session.EncodeAuthReqAsync(authReq));
    }

    [Test]
    public void EncodeAuthRequestWithEmptyContainersAndAppContainer() {
      var authReq = new AuthReq {AppContainer = true, AppExchangeInfo = GetExchangeInfo(), Containers = new List<ContainerPermissions>()};
      Assert.DoesNotThrow(async () => await Session.EncodeAuthReqAsync(authReq));
    }

    [Test]
    public void EncodeAuthRequestWithInvalidExchangeInfoThrowsException() {
      var authReq = new AuthReq {AppContainer = false, AppExchangeInfo = new AppExchangeInfo(), Containers = null};
      Assert.Catch(async () => await Session.EncodeAuthReqAsync(authReq));
    }
  }
}
