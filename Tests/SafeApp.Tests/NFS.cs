using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Utilities;

namespace SafeApp.Tests
{
    [TestFixture]

    // ReSharper disable once InconsistentNaming
    internal class NFS
    {
        [Test]
        public async Task CrudUsingNfsApi()
        {
            var session = await Utils.CreateTestApp();
            var mDataInfo = await session.MDataInfoActions.RandomPrivateAsync(18000);
            using (var signPubKey = await session.Crypto.AppPubSignKeyAsync())
            using (var entryhandle = await session.MDataEntries.NewAsync())
            using (var permissionHandle = await session.MDataPermissions.NewAsync())
            {
                var permissions = new PermissionSet
                {
                    Read = true,
                    ManagePermissions = true,
                    Insert = true,
                    Update = true,
                    Delete = true
                };
                await session.MDataEntries.InsertAsync(
                    entryhandle,
                    Encoding.UTF8.GetBytes("index.html").ToList(),
                    Encoding.UTF8.GetBytes("<html><body>Hello</body></html>").ToList());
                await session.MDataPermissions.InsertAsync(permissionHandle, signPubKey, permissions);
                await session.MData.PutAsync(mDataInfo, permissionHandle, NativeHandle.EmptyMDataEntries);
            }

            var fileHandle = await session.NFS.FileOpenAsync(mDataInfo, default(File), Misc.NFS.OpenMode.Overwrite);
            var fileContent = Encoding.UTF8.GetBytes(Utils.GetRandomString(20)).ToList();
            await session.NFS.FileWriteAsync(fileHandle, fileContent);
            var createdFile = await session.NFS.FileCloseAsync(fileHandle);
            var fileName = Utils.GetRandomString(10);
            await session.NFS.DirInsertFileAsync(mDataInfo, fileName, createdFile);
            var (file, version) = await session.NFS.DirFetchFileAsync(mDataInfo, fileName);
            fileHandle = await session.NFS.FileOpenAsync(mDataInfo, file, Misc.NFS.OpenMode.Read);
            var fetchedContent = await session.NFS.FileReadAsync(fileHandle, 0, 0);
            Assert.That(fileContent, Is.EqualTo(fetchedContent));
            fileHandle = await session.NFS.FileOpenAsync(mDataInfo, file, Misc.NFS.OpenMode.Append);
            var appendedContent = Encoding.UTF8.GetBytes(Utils.GetRandomString(10)).ToList();
            await session.NFS.FileWriteAsync(fileHandle, appendedContent);
            file = await session.NFS.FileCloseAsync(fileHandle);
            await session.NFS.DirUpdateFileAsync(mDataInfo, fileName, file, version + 1);
            (file, version) = await session.NFS.DirFetchFileAsync(mDataInfo, fileName);
            fileHandle = await session.NFS.FileOpenAsync(mDataInfo, file, Misc.NFS.OpenMode.Read);
            fetchedContent = await session.NFS.FileReadAsync(fileHandle, 0, 0);
            Assert.That(fileContent.Count + appendedContent.Count, Is.EqualTo(fetchedContent.Count));
            await session.NFS.DirDeleteFileAsync(mDataInfo, fileName, version + 1);
            Assert.That(async () => await session.NFS.DirFetchFileAsync(mDataInfo, fileName), Throws.TypeOf<FfiException>());
        }
    }
}
