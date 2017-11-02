using System.Linq;
using System.Text;
using NUnit.Framework;
using SafeApp.MData;
using SafeApp.Misc;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class MutableDataTests {
    [Test]
    public async void RandomPrivateMutableDataUpdateAction() {
      Utils.InitialiseSessionForRandomTestApp();
      const ulong tagType = 15001;
      const string actKey = "sample_key";
      const string actValue = "sample_value";
      using (var mdInfoHandle = await MDataInfo.RandomPrivateAsync(tagType)) {
        using (var permissionSetH = await MDataPermissionSet.NewAsync()) {
          await MDataPermissionSet.AllowAsync(permissionSetH, MDataAction.kInsert);
          await MDataPermissionSet.AllowAsync(permissionSetH, MDataAction.kManagePermissions);
          using (var permissionsH = await MDataPermissions.NewAsync()) {
            using (var appSignKeyH = await Crypto.AppPubSignKeyAsync()) {
              await MDataPermissions.InsertAsync(permissionsH, appSignKeyH, permissionSetH);
              await MData.MData.PutAsync(mdInfoHandle, permissionsH, NativeHandle.Zero);
            }
          }
        }
        using (var entryActionsH = await MDataEntryActions.NewAsync()) {
          var key = Encoding.Default.GetBytes(actKey).ToList();
          var value = Encoding.Default.GetBytes(actValue).ToList();
          key = await MDataInfo.EncryptEntryKeyAsync(mdInfoHandle, key);
          value = await MDataInfo.EncryptEntryValueAsync(mdInfoHandle, value);
          await MDataEntryActions.InsertAsync(entryActionsH, key, value);
          await MData.MData.MutateEntriesAsync(mdInfoHandle, entryActionsH);
        }

        using (var keysHandle = await MData.MData.ListKeysAsync(mdInfoHandle)) {
          var len = await MDataKeys.LenAsync(keysHandle);
          Assert.AreEqual(1, len.ToInt32());
        }

        using (var entriesHandle = await MData.MData.ListEntriesAsync(mdInfoHandle)) {
          var entries = await MDataEntries.ForEachAsync(entriesHandle);
          Assert.AreEqual(1, entries.Count);
          var entry = entries.First();
          var key = await MDataInfo.DecryptAsync(mdInfoHandle, entry.Item1);
          var value = await MDataInfo.DecryptAsync(mdInfoHandle, entry.Item2);
          var encoding = new ASCIIEncoding();
          Assert.AreEqual(actKey, encoding.GetString(key.ToArray()));
          Assert.AreEqual(actValue, encoding.GetString(value.ToArray()));
        }
      }
    }

    [Test]
    public async void RandomPublicMutableDataInsertAction() {
      Utils.InitialiseSessionForRandomTestApp();
      const ulong tagType = 15001;
      using (var mdInfoHandle = await MDataInfo.RandomPublicAsync(tagType)) {
        using (var permissionSetH = await MDataPermissionSet.NewAsync()) {
          await MDataPermissionSet.AllowAsync(permissionSetH, MDataAction.kInsert);
          await MDataPermissionSet.AllowAsync(permissionSetH, MDataAction.kManagePermissions);
          using (var permissionsH = await MDataPermissions.NewAsync()) {
            using (var appSignKeyH = await Crypto.AppPubSignKeyAsync()) {
              await MDataPermissions.InsertAsync(permissionsH, appSignKeyH, permissionSetH);
              await MData.MData.PutAsync(mdInfoHandle, permissionsH, NativeHandle.Zero);
            }
          }
        }
        using (var entryActionsH = await MDataEntryActions.NewAsync()) {
          var key = Encoding.Default.GetBytes("sample_key").ToList();
          var value = Encoding.Default.GetBytes("sample_value").ToList();
          await MDataEntryActions.InsertAsync(entryActionsH, key, value);
          await MData.MData.MutateEntriesAsync(mdInfoHandle, entryActionsH);
        }

        using (var entryActionsH = await MDataEntryActions.NewAsync()) {
          var key = Encoding.Default.GetBytes("sample_key_2").ToList();
          var value = Encoding.Default.GetBytes("sample_value_2").ToList();
          await MDataEntryActions.InsertAsync(entryActionsH, key, value);
          await MData.MData.MutateEntriesAsync(mdInfoHandle, entryActionsH);
        }

        using (var keysHandle = await MData.MData.ListKeysAsync(mdInfoHandle)) {
          var len = await MDataKeys.LenAsync(keysHandle);
          Assert.AreEqual(2, len.ToInt32());
        }
      }
    }
  }
}
