using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Misc;
using SafeApp.Utilities;

// ReSharper disable AccessToDisposedClosure

namespace SafeApp.Tests
{
    [TestFixture]
    internal class MutableDataTests
    {
        [Test]
        public async Task AddRemoveUserPermission()
        {
            var locator = Utils.GetRandomString(10);
            var secret = Utils.GetRandomString(10);
            var authReq = new AuthReq
            {
                App = new AppExchangeInfo { Id = "net.maidsafe.mdata.permission", Name = "CMS", Vendor = "MaidSafe.net Ltd" },
                AppContainer = true,
                Containers = new List<ContainerPermissions>()
            };
            var cmsApp = await Utils.CreateTestApp(locator, secret, authReq);
            var mDataInfo = await Utils.PreparePublicDirectory(cmsApp);
            authReq.App.Name = "Hosting";
            authReq.App.Id = "net.maidsafe.mdata.host";
            var ipcMsg = await Session.EncodeAuthReqAsync(authReq);
            var response = await Utils.AuthenticateAuthRequest(locator, secret, ipcMsg.Item2, true);
            var decodedResponse = await Session.DecodeIpcMessageAsync(response) as AuthIpcMsg;
            Assert.NotNull(decodedResponse);
            var hostingApp = await Session.AppRegisteredAsync(authReq.App.Id, decodedResponse.AuthGranted);
            var ipcReq = await Session.EncodeShareMDataRequestAsync(
                new ShareMDataReq
                {
                    App = authReq.App,
                    MData = new List<ShareMData>
                    {
                        new ShareMData
                        {
                            Name = mDataInfo.Name,
                            TypeTag = mDataInfo.TypeTag,
                            Perms = new PermissionSet { Insert = true, Read = true }
                        }
                    }
                });
            await Utils.AuthenticateShareMDataRequest(locator, secret, ipcReq.Item2, true);
            await hostingApp.AccessContainer.RefreshAccessInfoAsync();
            using (var entryhandle = await hostingApp.MDataEntryActions.NewAsync())
            {
                await hostingApp.MDataEntryActions.InsertAsync(
                    entryhandle,
                    Encoding.UTF8.GetBytes("default.html").ToList(),
                    Encoding.UTF8.GetBytes("<html><body>Hello Default</body></html>").ToList());
                await hostingApp.MData.MutateEntriesAsync(mDataInfo, entryhandle);
            }

            var version = await cmsApp.MData.GetVersionAsync(mDataInfo);
            using (var permissionHandle = await cmsApp.MData.ListPermissionsAsync(mDataInfo))
            {
                var userPermissions = await cmsApp.MDataPermissions.ListAsync(permissionHandle);
                Assert.That(await cmsApp.MDataPermissions.LenAsync(permissionHandle), Is.EqualTo(userPermissions.Count));
                var userPermissionToDel = userPermissions.Find(userPerm => userPerm.Item2.ManagePermissions == false);
                await cmsApp.MData.DelUserPermissionsAsync(mDataInfo, userPermissionToDel.Item1, version + 1);
                userPermissions.ForEach(perm => perm.Item1.Dispose());
            }

            using (var entryHandle = await hostingApp.MDataEntryActions.NewAsync())
            {
                await hostingApp.MDataEntryActions.InsertAsync(
                    entryHandle,
                    Encoding.UTF8.GetBytes("home.html").ToList(),
                    Encoding.UTF8.GetBytes("<html><body>Hello Home!</body></html>").ToList());
                Assert.That(
                    async () => { await hostingApp.MData.MutateEntriesAsync(mDataInfo, entryHandle); },
                    Throws.TypeOf<FfiException>());
            }

            cmsApp.Dispose();
            hostingApp.Dispose();
        }

        [Test]
        public async Task CrudPrivateMutableData()
        {
            var locator = Utils.GetRandomString(10);
            var secret = Utils.GetRandomString(10);
            var authReq = new AuthReq
            {
                App = new AppExchangeInfo
                {
                    Id = "net.maidsafe.mdata.permission.delete", Name = Utils.GetRandomString(5), Vendor = "MaidSafe.net Ltd"
                },
                AppContainer = true,
                Containers = new List<ContainerPermissions>()
            };
            var app = await Utils.CreateTestApp(locator, secret, authReq);
            var mdInfo = await app.MDataInfoActions.RandomPrivateAsync(16000);
            using (var entriesHandle = await app.MDataEntries.NewAsync())
            using (var permissions = await app.MDataPermissions.NewAsync())
            using (var appKey = await app.Crypto.AppPubSignKeyAsync())
            {
                await app.MDataPermissions.InsertAsync(
                    permissions,
                    appKey,
                    new PermissionSet { Insert = true, Delete = true, Update = true, Read = true });
                for (var i = 0; i < 5; i++)
                {
                    var key = await app.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, Utils.GetRandomString(10).ToUtfBytes());
                    var value = await app.MDataInfoActions.EncryptEntryValueAsync(mdInfo, Utils.GetRandomString(10).ToUtfBytes());
                    await app.MDataEntries.InsertAsync(entriesHandle, key, value);
                }

                await app.MData.PutAsync(mdInfo, permissions, entriesHandle);
            }

            var keys = await app.MData.ListKeysAsync(mdInfo);
            Assert.That(keys.Count, Is.EqualTo(5));
            var keyToDelete = keys[0];
            using (var entriesHandle = await app.MDataEntryActions.NewAsync())
            {
                var value = await app.MData.GetValueAsync(mdInfo, keyToDelete.Key);
                await app.MDataEntryActions.UpdateAsync(
                    entriesHandle,
                    keyToDelete.Key,
                    Utils.GetRandomString(5).ToUtfBytes(),
                    value.Item2 + 1);
                await app.MData.MutateEntriesAsync(mdInfo, entriesHandle);
            }

            using (var entriesHandle = await app.MDataEntryActions.NewAsync())
            {
                var value = await app.MData.GetValueAsync(mdInfo, keyToDelete.Key);
                await app.MDataEntryActions.DeleteAsync(entriesHandle, keyToDelete.Key, value.Item2 + 1);
                await app.MData.MutateEntriesAsync(mdInfo, entriesHandle);
            }

            keys = await app.MData.ListKeysAsync(mdInfo);
            Assert.That(keys.Count, Is.EqualTo(5));
            var deletedValue = await app.MData.GetValueAsync(mdInfo, keyToDelete.Key);
            using (var entriesHandle = await app.MDataEntryActions.NewAsync())
            {
                await app.MDataEntryActions.UpdateAsync(
                    entriesHandle,
                    keyToDelete.Key,
                    Utils.GetRandomString(5).ToUtfBytes(),
                    deletedValue.Item2 + 1);
                await app.MData.MutateEntriesAsync(mdInfo, entriesHandle);
            }

            deletedValue = await app.MData.GetValueAsync(mdInfo, keyToDelete.Key);
            Assert.That(deletedValue.Item2, Is.EqualTo(3));
            app.Dispose();
        }

        [Test]
        public async Task PrivateMutableDataPermissionTest()
        {
            var session = await Utils.CreateTestApp();
            const ulong tagType = 15020;
            var maxEntriesCount = 10;
            var appName = Utils.GetRandomData(10).ToList();
            var xorName = (await Crypto.Sha3HashAsync(appName)).ToArray();
            var nonce = await Crypto.GenerateNonceAsync();
            var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
            var rawKey = await session.Crypto.EncPubKeyGetAsync(encKeyPairTuple.Item1);
            var mdInfo = await session.MDataInfoActions.NewPrivateAsync(xorName, tagType, rawKey, nonce);

            var mDataPermissionSet = new PermissionSet { Read = true, Insert = true, ManagePermissions = true };
            using (var permissionsH = await session.MDataPermissions.NewAsync())
            {
                using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync())
                {
                    await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, mDataPermissionSet);
                    await session.MData.PutAsync(mdInfo, permissionsH, NativeHandle.EmptyMDataEntries);
                }
            }

            using (var appPubSignKeyH = await session.Crypto.AppPubSignKeyAsync())
            {
                mDataPermissionSet = new PermissionSet { Read = true, Insert = true, ManagePermissions = true, Update = true };
                var mDataVersion = await session.MData.GetVersionAsync(mdInfo);
                await session.MData.SetUserPermissionsAsync(mdInfo, appPubSignKeyH, mDataPermissionSet, mDataVersion + 1);

                using (var permissionsH = await session.MData.ListPermissionsAsync(mdInfo))
                {
                    var permissionsGranted = await session.MDataPermissions.GetAsync(permissionsH, appPubSignKeyH);
                    Assert.AreEqual(mDataPermissionSet, permissionsGranted);
                }
            }

            var entriesCount = Utils.Random.Next(maxEntriesCount);
            using (var entryActionsH = await session.MDataEntryActions.NewAsync())
            {
                for (var i = 0; i < entriesCount; i++)
                {
                    var key = Encoding.ASCII.GetBytes(Utils.GetRandomString(10)).ToList();
                    var value = Encoding.ASCII.GetBytes(Utils.GetRandomString(10)).ToList();
                    await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
                }
                await session.MData.MutateEntriesAsync(mdInfo, entryActionsH);
            }

            var mdValues = await session.MData.ListValuesAsync(mdInfo);
            Assert.AreEqual(entriesCount, mdValues.Count);

            using (var entryH = await session.MDataEntries.GetHandleAsync(mdInfo))
            {
                var totalEntries = await session.MDataEntries.LenAsync(entryH);
                Assert.AreEqual(entriesCount, totalEntries);
            }
        }

        [Test]
        public async Task RandomPrivateMutableDataUpdateAction()
        {
            var session = await Utils.CreateTestApp();
            const ulong tagType = 15001;
            var actKey = Utils.GetRandomString(10);
            var actValue = Utils.GetRandomString(10);
            var mdInfo = await session.MDataInfoActions.RandomPrivateAsync(tagType);
            var mDataPermissionSet = new PermissionSet { Insert = true, ManagePermissions = true, Read = true };
            using (var permissionsH = await session.MDataPermissions.NewAsync())
            {
                using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync())
                {
                    await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, mDataPermissionSet);
                    await session.MData.PutAsync(mdInfo, permissionsH, NativeHandle.EmptyMDataEntries);
                }
            }

            using (var entryActionsH = await session.MDataEntryActions.NewAsync())
            {
                var key = Encoding.ASCII.GetBytes(actKey).ToList();
                var value = Encoding.ASCII.GetBytes(actValue).ToList();
                key = await session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, key);
                value = await session.MDataInfoActions.EncryptEntryValueAsync(mdInfo, value);
                await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
                await session.MData.MutateEntriesAsync(mdInfo, entryActionsH);
            }

            var keys = await session.MData.ListKeysAsync(mdInfo);
            Assert.That(keys.Count, Is.EqualTo(1));

            foreach (var key in keys)
            {
                var (value, _) = await session.MData.GetValueAsync(mdInfo, key.Key.ToList());
                var decryptedKey = await session.MDataInfoActions.DecryptAsync(mdInfo, key.Key.ToList());
                var decryptedValue = await session.MDataInfoActions.DecryptAsync(mdInfo, value.ToList());
                Assert.That(actKey, Is.EqualTo(Encoding.ASCII.GetString(decryptedKey.ToArray())));
                Assert.That(actValue, Is.EqualTo(Encoding.ASCII.GetString(decryptedValue.ToArray())));
            }

            await session.MData.SerialisedSizeAsync(mdInfo);
            var serialisedData = await session.MDataInfoActions.SerialiseAsync(mdInfo);
            mdInfo = await session.MDataInfoActions.DeserialiseAsync(serialisedData);

            keys = await session.MData.ListKeysAsync(mdInfo);
            Assert.That(keys.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task RandomPublicMutableDataInsertAction()
        {
            var session = await Utils.CreateTestApp();
            const ulong tagType = 15010;
            var actKey = Utils.GetRandomString(10);
            var actValue = Utils.GetRandomString(10);
            var mdInfo = await session.MDataInfoActions.RandomPublicAsync(tagType);
            var mDataPermissionSet = new PermissionSet { Insert = true, ManagePermissions = true, Read = true };
            using (var permissionsH = await session.MDataPermissions.NewAsync())
            {
                using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync())
                {
                    await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, mDataPermissionSet);
                    await session.MData.PutAsync(mdInfo, permissionsH, NativeHandle.EmptyMDataEntries);
                }
            }

            using (var entryActionsH = await session.MDataEntryActions.NewAsync())
            {
                var key = Encoding.ASCII.GetBytes(actKey).ToList();
                var value = Encoding.ASCII.GetBytes(actValue).ToList();
                await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
                await session.MData.MutateEntriesAsync(mdInfo, entryActionsH);
            }

            var keys = await session.MData.ListKeysAsync(mdInfo);
            Assert.That(keys.Count, Is.EqualTo(1));

            foreach (var key in keys)
            {
                var (value, _) = await session.MData.GetValueAsync(mdInfo, key.Key.ToList());
                Assert.That(actKey, Is.EqualTo(Encoding.ASCII.GetString(key.Key.ToArray())));
                Assert.That(actValue, Is.EqualTo(Encoding.ASCII.GetString(value.ToArray())));
            }

            await session.MData.SerialisedSizeAsync(mdInfo);
            var serialisedData = await session.MDataInfoActions.SerialiseAsync(mdInfo);
            mdInfo = await session.MDataInfoActions.DeserialiseAsync(serialisedData);

            keys = await session.MData.ListKeysAsync(mdInfo);
            Assert.That(keys.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task SharedMutableData()
        {
            var locator = Utils.GetRandomString(10);
            var secret = Utils.GetRandomString(10);
            var authReq = new AuthReq
            {
                App = new AppExchangeInfo { Id = "net.maidsafe.sample", Name = "Inbox", Scope = null, Vendor = "MaidSafe.net Ltd" },
                Containers = new List<ContainerPermissions>()
            };
            var session = await Utils.CreateTestApp(locator, secret, authReq);
            var typeTag = 16000;
            var mDataInfo = await session.MDataInfoActions.RandomPrivateAsync((ulong)typeTag);
            using (var permissionsH = await session.MDataPermissions.NewAsync())
            {
                using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync())
                {
                    var ownerPermission = new PermissionSet { Insert = true, ManagePermissions = true, Read = true };
                    await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ownerPermission);
                    var sharePermissions = new PermissionSet { Insert = true };
                    await session.MDataPermissions.InsertAsync(permissionsH, NativeHandle.EmptyMDataPermissions, sharePermissions);
                    await session.MData.PutAsync(mDataInfo, permissionsH, NativeHandle.EmptyMDataEntries);
                }
            }

            using (var entriesHandle = await session.MDataEntryActions.NewAsync())
            {
                var key = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomData(10).ToList());
                var value = await session.MDataInfoActions.EncryptEntryValueAsync(mDataInfo, Utils.GetRandomData(10).ToList());
                await session.MDataEntryActions.InsertAsync(entriesHandle, key, value);
                await session.MData.MutateEntriesAsync(mDataInfo, entriesHandle);
            }

            using (var entriesHandle = await session.MDataEntries.GetHandleAsync(mDataInfo))
            {
                var keys = await session.MData.ListKeysAsync(mDataInfo);
                foreach (var key in keys)
                {
                    var encKey = await session.MDataEntries.GetAsync(entriesHandle, key.Key);
                    await session.MDataInfoActions.DecryptAsync(mDataInfo, encKey.Item1);
                }
            }

            session.Dispose();

            authReq = new AuthReq
            {
                App = new AppExchangeInfo { Id = "net.maidsafe.share.md", Name = "Share Chat", Vendor = "MaidSafe.net Ltd" },
                AppContainer = false,
                Containers = new List<ContainerPermissions>()
            };
            var session2 = await Utils.CreateTestApp(authReq);
            using (var entriesHandle = await session2.MDataEntryActions.NewAsync())
            {
                var key = await session2.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomData(10).ToList());
                var value = await session2.MDataInfoActions.EncryptEntryValueAsync(mDataInfo, Utils.GetRandomData(10).ToList());
                await session2.MDataEntryActions.InsertAsync(entriesHandle, key, value);
                await session2.MData.MutateEntriesAsync(mDataInfo, entriesHandle);
            }

            using (var entriesHandle = await session2.MDataEntries.GetHandleAsync(mDataInfo))
            {
                var entries = await session2.MData.ListEntriesAsync(entriesHandle);
                foreach (var entry in entries)
                {
                    await session2.MDataInfoActions.DecryptAsync(mDataInfo, entry.Key.Key);
                }
            }

            using (var entryAction = await session2.MDataEntryActions.NewAsync())
            using (var entriesHandle = await session2.MDataEntries.GetHandleAsync(mDataInfo))
            {
                var keys = await session2.MData.ListKeysAsync(mDataInfo);
                foreach (var key in keys)
                {
                    var encKey = await session2.MDataEntries.GetAsync(entriesHandle, key.Key);
                    await session2.MDataEntryActions.DeleteAsync(entryAction, key.Key, encKey.Item2);
                }

                Assert.That(
                    async () => { await session2.MData.MutateEntriesAsync(mDataInfo, entryAction); },
                    Throws.TypeOf<FfiException>());
            }

            session2.Dispose();
        }
    }
}
