using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

// ReSharper disable InconsistentNaming

namespace SafeApp.IData {
  [PublicAPI]
  public class IData {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal IData(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task<byte[]> CloseSelfEncryptorAsync(ulong seH, NativeHandle cipherOptH) {
      return AppBindings.IDataCloseSelfEncryptorAsync(_appPtr, seH, cipherOptH);
    }

    public async Task<NativeHandle> FetchSelfEncryptorAsync(byte[] xorName) {
      var sEReaderHandle = await AppBindings.IDataFetchSelfEncryptorAsync(_appPtr, xorName);
      return new NativeHandle(_appPtr, sEReaderHandle, SelfEncryptorReaderFreeAsync);
    }

    public async Task<NativeHandle> NewSelfEncryptorAsync() {
      var sEWriterHandle = await AppBindings.IDataNewSelfEncryptorAsync(_appPtr);
      return new NativeHandle(_appPtr, sEWriterHandle, null);
    }

    public async Task<List<byte>> ReadFromSelfEncryptorAsync(NativeHandle seHandle, ulong fromPos, ulong len) {
      var dataArray = await AppBindings.IDataReadFromSelfEncryptorAsync(_appPtr, seHandle, fromPos, len);
      return new List<byte>(dataArray);
    }

    private Task SelfEncryptorReaderFreeAsync(ulong sEReaderHandle) {
      return AppBindings.IDataSelfEncryptorReaderFreeAsync(_appPtr, sEReaderHandle);
    }

    private Task SelfEncryptorWriterFreeAsync(ulong sEWriterHandle) {
      return AppBindings.IDataSelfEncryptorWriterFreeAsync(_appPtr, sEWriterHandle);
    }

    public Task<ulong> SerialisedSizeAsync(byte[] xorName) {
      return AppBindings.IDataSerialisedSizeAsync(_appPtr, xorName);
    }

    public Task<ulong> SizeAsync(NativeHandle seHandle) {
      return AppBindings.IDataSizeAsync(_appPtr, seHandle);
    }

    public Task WriteToSelfEncryptorAsync(NativeHandle seHandle, List<byte> data) {
      return AppBindings.IDataWriteToSelfEncryptorAsync(_appPtr, seHandle, data);
    }
  }
}
