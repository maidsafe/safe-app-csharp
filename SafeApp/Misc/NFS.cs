using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

namespace SafeApp.Misc {
  [PublicAPI]
  // ReSharper disable once InconsistentNaming
  public class NFS {
    [PublicAPI]
    public enum OpenMode {
      Append = 2,
      Overwrite = 1,
      Read = 4
    }

    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private readonly SafeAppPtr _appPtr;

    internal NFS(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task DirDeleteFileAsync(MDataInfo mDataInfo, string fileName, ulong version) {
      return AppBindings.DirDeleteFileAsync(_appPtr, ref mDataInfo, fileName, version);
    }

    public Task<(File, ulong)> DirFetchFileAsync(MDataInfo mDataInfo, string fileName) {
      return AppBindings.DirFetchFileAsync(_appPtr, ref mDataInfo, fileName);
    }

    public Task DirInsertFileAsync(MDataInfo mDataInfo, string fileName, File file) {
      return AppBindings.DirInsertFileAsync(_appPtr, ref mDataInfo, fileName, ref file);
    }

    public Task DirUpdateFileAsync(MDataInfo mDataInfo, string fileName, File file, ulong version) {
      return AppBindings.DirUpdateFileAsync(_appPtr, ref mDataInfo, fileName, ref file, version);
    }

    public Task<File> FileCloseAsync(NativeHandle fileContextHandle) {
      return AppBindings.FileCloseAsync(_appPtr, fileContextHandle);
    }

    public async Task<NativeHandle> FileOpenAsync(MDataInfo mDataInfo, File file, OpenMode openMode) {
      var fileContextHandle = await AppBindings.FileOpenAsync(_appPtr, ref mDataInfo, ref file, (ulong)openMode);
      return new NativeHandle(
        _appPtr,
        fileContextHandle,
        handle => { return openMode == OpenMode.Read ? AppBindings.FileCloseAsync(_appPtr, handle) : Task.Run(() => { }); });
    }

    public Task<List<byte>> FileReadAsync(NativeHandle fileContextHandle, ulong start, ulong end) {
      return AppBindings.FileReadAsync(_appPtr, fileContextHandle, start, end);
    }

    public Task<ulong> FileSizeAsync(NativeHandle fileContextHandle) {
      return AppBindings.FileSizeAsync(_appPtr, fileContextHandle);
    }

    public Task FileWriteAsync(NativeHandle fileContextHandle, List<byte> data) {
      return AppBindings.FileWriteAsync(_appPtr, fileContextHandle, data);
    }
  }
}
