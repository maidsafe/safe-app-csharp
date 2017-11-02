using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataInfo {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<List<byte>> DecryptAsync(NativeHandle mDataInfoH, List<byte> cipherText) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var cipherPtr = cipherText.ToIntPtr();
      var cipherLen = (IntPtr)cipherText.Count;

      ByteArrayCb callback = (_, result, plainText, len) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        var byteList = plainText.ToList<byte>(len);
        tcs.SetResult(byteList);
      };

      AppBindings.MDataInfoDecrypt(Session.AppPtr, mDataInfoH, cipherPtr, cipherLen, callback);
      Marshal.FreeHGlobal(cipherPtr);

      return tcs.Task;
    }

    public static Task<NativeHandle> DeserialiseAsync(List<byte> serialisedData) {
      var tcs = new TaskCompletionSource<NativeHandle>();
      UlongCb callback = (_, result, mdataInfoH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(mdataInfoH, FreeAsync));
      };

      var serialisedDataPtr = serialisedData.ToIntPtr();
      AppBindings.MDataInfoDeserialise(Session.AppPtr, serialisedDataPtr, (IntPtr)serialisedData.Count, callback);

      Marshal.FreeHGlobal(serialisedDataPtr);

      return tcs.Task;
    }

    public static Task<List<byte>> EncryptEntryKeyAsync(NativeHandle infoH, List<byte> inputBytes) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var inputBytesPtr = inputBytes.ToIntPtr();
      ByteArrayCb callback = (_, result, dataPtr, dataLen) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }
        var data = dataPtr.ToList<byte>(dataLen);
        tcs.SetResult(data);
      };

      AppBindings.MDataInfoEncryptEntryKey(Session.AppPtr, infoH, inputBytesPtr, (IntPtr)inputBytes.Count, callback);
      Marshal.FreeHGlobal(inputBytesPtr);

      return tcs.Task;
    }

    public static Task<List<byte>> EncryptEntryValueAsync(NativeHandle infoH, List<byte> inputBytes) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var inputBytesPtr = inputBytes.ToIntPtr();
      ByteArrayCb callback = (_, result, dataPtr, dataLen) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }
        var data = dataPtr.ToList<byte>(dataLen);
        tcs.SetResult(data);
      };

      AppBindings.MDataInfoEncryptEntryValue(Session.AppPtr, infoH, inputBytesPtr, (IntPtr)inputBytes.Count, callback);
      Marshal.FreeHGlobal(inputBytesPtr);

      return tcs.Task;
    }

    public static Task FreeAsync(ulong mDataInfoH) {
      var tcs = new TaskCompletionSource<object>();

      ResultCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataInfoFree(Session.AppPtr, mDataInfoH, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewPublicAsync(List<byte> xorName, ulong typeTag) {
      var tcs = new TaskCompletionSource<NativeHandle>();

      UlongCb callback = (_, result, pubMDataInfoH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(pubMDataInfoH, FreeAsync));
      };

      var xorNamePtr = xorName.ToIntPtr();
      AppBindings.MDataInfoNewPublic(Session.AppPtr, xorNamePtr, typeTag, callback);
      Marshal.FreeHGlobal(xorNamePtr);

      return tcs.Task;
    }

    public static Task<NativeHandle> RandomPrivateAsync(ulong typeTag) {
      var tcs = new TaskCompletionSource<NativeHandle>();

      UlongCb callback = (_, result, privateMDataInfoH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(privateMDataInfoH, FreeAsync));
      };

      AppBindings.MDataInfoRandomPrivate(Session.AppPtr, typeTag, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> RandomPublicAsync(ulong typeTag) {
      var tcs = new TaskCompletionSource<NativeHandle>();

      UlongCb callback = (_, result, pubMDataInfoH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(pubMDataInfoH, FreeAsync));
      };

      AppBindings.MDataInfoRandomPublic(Session.AppPtr, typeTag, callback);

      return tcs.Task;
    }

    public static Task<List<byte>> SerialiseAsync(NativeHandle mdataInfoH) {
      var tcs = new TaskCompletionSource<List<byte>>();
      ByteArrayCb callback = (_, result, bytesPtr, len) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(bytesPtr.ToList<byte>(len));
      };

      AppBindings.MDataInfoSerialise(Session.AppPtr, mdataInfoH, callback);

      return tcs.Task;
    }
  }
}
