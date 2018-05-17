using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;
using SafeApp.Utilities;

// ReSharper disable AccessToDisposedClosure

namespace SafeApp.Tests {
  [TestFixture]
  internal class MiscTest {
    [Test]
    public async Task AccountOverflowTest() {
      var session = await Utils.CreateTestApp();
      var mdInfo = await session.MDataInfoActions.RandomPublicAsync(16000);
      var accountInfo = await session.GetAccountInfoAsyc();
      Assert.That(987, Is.EqualTo(accountInfo.MutationsAvailable));
      Assert.That(13, Is.EqualTo(accountInfo.MutationsDone));
      using (var permissionsHandle = await session.MDataPermissions.NewAsync())
      using (var userHandle = await session.Crypto.AppPubSignKeyAsync()) {
        await session.MDataPermissions.InsertAsync(permissionsHandle, userHandle, new PermissionSet {Insert = true, Delete = true});
        await session.MData.PutAsync(mdInfo, permissionsHandle, NativeHandle.Zero);
        for (var i = 0; i < (long)accountInfo.MutationsAvailable - 1; i++) {
          var entryHandle = await session.MDataEntryActions.NewAsync();
          await session.MDataEntryActions.InsertAsync(entryHandle, Utils.GetRandomData(10).ToList(), Utils.GetRandomData(15).ToList());
          await session.MData.MutateEntriesAsync(mdInfo, entryHandle);
          entryHandle.Dispose();
        }
      }

      accountInfo = await session.GetAccountInfoAsyc();
      Assert.That(1000, Is.EqualTo(accountInfo.MutationsDone));
      Assert.That(0, Is.EqualTo(accountInfo.MutationsAvailable));
      using (var entryActionHandle = await session.MDataEntryActions.NewAsync()) {
        await session.MDataEntryActions.InsertAsync(entryActionHandle, Utils.GetRandomData(10).ToList(), Utils.GetRandomData(15).ToList());
        Assert.That(async () => { await session.MData.MutateEntriesAsync(mdInfo, entryActionHandle); }, Throws.TypeOf<FfiException>());
      }

      session.Dispose();
    }

    [Test]
    public async Task AppConatinerTest() {
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = Utils.GetRandomString(10), Name = Utils.GetRandomString(10), Vendor = Utils.GetRandomString(10)},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var session = await Utils.CreateTestApp(authReq);
      var mDataInfo = await session.AccessContainer.GetMDataInfoAsync("apps/" + authReq.App.Id);
      var perms = await session.MData.ListUserPermissionsAsync(mDataInfo, await session.Crypto.AppPubSignKeyAsync());
      Assert.IsTrue(perms.Insert);
      Assert.IsTrue(perms.Update);
      Assert.IsTrue(perms.Delete);
      Assert.IsTrue(perms.Read);
      Assert.IsTrue(perms.ManagePermissions);
      var keys = await session.MData.ListKeysAsync(mDataInfo);
      Assert.AreEqual(0, keys.Count);
      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync()) {
        var encKey = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomData(15).ToList());
        var encVal = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomData(25).ToList());
        await session.MDataEntryActions.InsertAsync(entriesActionHandle, encKey, encVal);
        await session.MData.MutateEntriesAsync(mDataInfo, entriesActionHandle);
      }

      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync())
      using (var entryHandle = await session.MDataEntries.GetHandleAsync(mDataInfo)) {
        keys = await session.MData.ListKeysAsync(mDataInfo);
        var value = await session.MDataEntries.GetAsync(entryHandle, keys[0].Val);
        await session.MDataEntryActions.UpdateAsync(entriesActionHandle, keys[0].Val, Utils.GetRandomData(10).ToList(), value.Item2 + 1);
        await session.MData.MutateEntriesAsync(mDataInfo, entriesActionHandle);
      }

      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync())
      using (var entryHandle = await session.MDataEntries.GetHandleAsync(mDataInfo)) {
        keys = await session.MData.ListKeysAsync(mDataInfo);
        var value = await session.MDataEntries.GetAsync(entryHandle, keys[0].Val);
        await session.MDataEntryActions.DeleteAsync(entriesActionHandle, keys[0].Val, value.Item2 + 1);
        await session.MData.MutateEntriesAsync(mDataInfo, entriesActionHandle);
      }
    }

    [Test]
    public async Task AppScopeTest() {
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = "net.maidsafe.scope.test", Name = "SampleTest", Scope = "Web", Vendor = "MaidSafe.net Ltd"},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var session = await Utils.CreateTestApp(authReq);
      var mdInfoWeb = await session.AccessContainer.GetMDataInfoAsync("apps/" + authReq.App.Id);
      var keys = await session.MData.ListKeysAsync(mdInfoWeb);
      Assert.That(0, Is.EqualTo(keys.Count));
      session.Dispose();
      authReq.App.Scope = "mobile";
      session = await Utils.CreateTestApp(authReq);
      var mdInfoMobile = await session.AccessContainer.GetMDataInfoAsync("apps/" + authReq.App.Id);
      Assert.That(mdInfoMobile.Name, Is.Not.EqualTo(mdInfoWeb.Name));
      session.Dispose();
    }

    [Test]
    public void IsMockBuildTest() {
      Assert.That(Authenticator.IsMockBuild(), Is.True);
    }

    [Test]
    public async Task RustLoggerTest() {
      var configPath = string.Empty;
      var filecontent = string.Empty;
      Assert.That(async () => configPath = await Utils.InitRustLogging(), Throws.Nothing);
      Assert.That(async () => await Session.DecodeIpcMessageAsync("Some Random Invalid String"), Throws.TypeOf<IpcMsgException>());
            var fileEmpty = true;
            for (var i = 0; i < 10; ++i)
            {
                await Task.Delay(1000);
                using (var fs = new FileStream(Path.Combine(configPath, "Client.log"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {

                    fileEmpty = string.IsNullOrEmpty(sr.ReadToEnd());
                    if (!fileEmpty)
                    {
                        break;
                    }
                }
            }
            Assert.That(fileEmpty, Is.False);
        }
    }
}
