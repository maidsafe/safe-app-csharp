using NUnit.Framework;
using System;
using SafeApp.MockAuthBindings;
using SafeApp.MData;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CrossPlatformTests {
    [SetUp]
    public void Setup() { }

    [TearDown]
    public void Tear() { }

    [Test]
    public void RandomAppCreate() {
      var appPtr = IntPtr.Zero;
      Assert.DoesNotThrow(() => appPtr = MockAuthResolver.Current.TestCreateApp());
      Assert.AreNotEqual(appPtr, IntPtr.Zero);
      Session.AppPtr = appPtr;
      Assert.DoesNotThrow(async () => await MDataEntryActions.NewAsync());
    }


  }
}
