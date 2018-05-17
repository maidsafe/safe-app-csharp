using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataInfoActions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal MDataInfoActions(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task<List<byte>> DecryptAsync(MDataInfo mDataInfo, List<byte> cipherText) {
      return AppBindings.MDataInfoDecryptAsync(ref mDataInfo, cipherText);
    }

    public Task<MDataInfo> DeserialiseAsync(List<byte> serialisedData) {
      return AppBindings.MDataInfoDeserialiseAsync(serialisedData);
    }

    public Task<List<byte>> EncryptEntryKeyAsync(MDataInfo mDataInfo, List<byte> inputBytes) {
      return AppBindings.MDataInfoEncryptEntryKeyAsync(ref mDataInfo, inputBytes);
    }

    public Task<List<byte>> EncryptEntryValueAsync(MDataInfo mDataInfo, List<byte> inputBytes) {
      return AppBindings.MDataInfoEncryptEntryValueAsync(ref mDataInfo, inputBytes);
    }

    public Task<MDataInfo> NewPrivateAsync(byte[] xorName, ulong typeTag, byte[] secEncKey, byte[] nonce) {
      return AppBindings.MDataInfoNewPrivateAsync(xorName, typeTag, secEncKey, nonce);
    }

    public Task<MDataInfo> RandomPrivateAsync(ulong typeTag) {
      return AppBindings.MDataInfoRandomPrivateAsync(typeTag);
    }

    public Task<MDataInfo> RandomPublicAsync(ulong typeTag) {
      return AppBindings.MDataInfoRandomPublicAsync(typeTag);
    }

    public async Task<List<byte>> SerialiseAsync(MDataInfo mDataInfo) {
      var byteArray = await AppBindings.MDataInfoSerialiseAsync(ref mDataInfo);
      return new List<byte>(byteArray);
    }
  }
}
