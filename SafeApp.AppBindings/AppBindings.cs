#if !NETSTANDARD1_2

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SafeApp.Utilities;

#if __IOS__
using ObjCRuntime;

#endif

namespace SafeApp.AppBindings {
  public class AppBindings : IAppBindings {
#region Generic FFiResult with value Callbacks

#if __IOS__
      [MonoPInvokeCallback(typeof(UlongCb))]
#endif
    private static void OnUlongCb(IntPtr self, FfiResult result, ulong mDataInfoHandle) {
      self.HandlePtrToType<UlongCb>()(IntPtr.Zero, result, mDataInfoHandle);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(StringCb))]
#endif
    private static void OnStringCb(IntPtr self, FfiResult result, string exeFileStem) {
      self.HandlePtrToType<StringCb>()(IntPtr.Zero, result, exeFileStem);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(ResultCb))]
#endif
    private static void OnResultCb(IntPtr self, FfiResult result) {
      self.HandlePtrToType<ResultCb>()(IntPtr.Zero, result);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(IntPtrCb))]
#endif
    private static void OnIntPtrCb(IntPtr self, FfiResult result, IntPtr intPtr) {
      self.HandlePtrToType<IntPtrCb>()(IntPtr.Zero, result, intPtr);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(IntCb))]
#endif
    private static void OnIntCb(IntPtr self, FfiResult result, int eventType) {
      self.HandlePtrToType<IntCb>()(IntPtr.Zero, result, eventType);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(ByteArrayCb))]
#endif
    private static void OnByteArrayCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen) {
      var cb = self.HandlePtrToType<ByteArrayCb>();
      cb(IntPtr.Zero, result, data, dataLen);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(ListBasedResultCb))]
#endif
    private static void OnListBasedResultCb(IntPtr self, FfiResult result)
    {
      var list = self.HandlePtrToType<List<object>>();
      var cb = (ListBasedResultCb)list[list.Count - 1];
      cb(IntPtr.Zero, result);
    }

    #endregion

    #region AccessContainerGetContainerMDataInfo

    public void AccessContainerGetContainerMDataInfo(IntPtr appPtr, string name, UlongCb callback) {
      AccessContainerGetContainerMDataInfoNative(appPtr, name, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "access_container_get_container_mdata_info")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "access_container_get_container_mdata_info")]
#endif
    public static extern void AccessContainerGetContainerMDataInfoNative(IntPtr appPtr, string name, IntPtr self, UlongCb callback);

#endregion

#region AppExeFileStem

    public void AppExeFileStem(StringCb callback) {
      AppExeFileStemNative(callback.ToHandlePtr(), OnStringCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_exe_file_stem")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "app_exe_file_stem")]
#endif
    public static extern void AppExeFileStemNative(IntPtr self, StringCb callback);

#endregion

#region AppInitLogging

    public void AppInitLogging(string fileName, ResultCb callback) {
      AppInitLoggingNative(fileName, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_init_logging")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "app_init_logging")]
#endif
    public static extern void AppInitLoggingNative(string fileName, IntPtr userDataPtr, ResultCb callback);

#endregion

#region AppOutputLogPath

    public void AppOutputLogPath(string fileName, StringCb callback) {
      AppOutputLogPathNative(fileName, callback.ToHandlePtr(), OnStringCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_output_log_path")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "app_output_log_path")]
#endif
    public static extern void AppOutputLogPathNative(string fileName, IntPtr userDataPtr, StringCb callback);

#endregion

#region AppPubSignKey

    public void AppPubSignKey(IntPtr appPtr, UlongCb callback) {
      AppPubSignKeyNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_pub_sign_key")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "app_pub_sign_key")]
#endif
    public static extern void AppPubSignKeyNative(IntPtr appPtr, IntPtr self, UlongCb callback);

#endregion

#region AppRegistered

    public void AppRegistered(string appId, IntPtr ffiAuthGrantedPtr, IntCb netObsCb, IntPtrCb appRegCb) {
      AppRegisteredNative(appId, ffiAuthGrantedPtr, netObsCb.ToHandlePtr(), appRegCb.ToHandlePtr(), OnIntCb, OnIntPtrCb);
    }
#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_registered")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "app_registered")]
#endif
    public static extern void AppRegisteredNative(
      string appId,
      IntPtr ffiAuthGrantedPtr,
      IntPtr networkUserData,
      IntPtr userData,
      IntCb netObsCb,
      IntPtrCb appRegCb);

#endregion

#region AppSetAdditionalSearchPath

    public void AppSetAdditionalSearchPath(string path, ResultCb callback) {
      AppSetAdditionalSearchPathNative(path, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_set_additional_search_path")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "app_set_additional_search_path")]
#endif
    public static extern void AppSetAdditionalSearchPathNative(string path, IntPtr self, ResultCb callback);

#endregion

#region CipherOptFree

    public void CipherOptFree(IntPtr appPtr, ulong cipherOptHandle, ResultCb callback) {
      CipherOptFreeNative(appPtr, cipherOptHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "cipher_opt_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "cipher_opt_free")]
#endif
    public static extern void CipherOptFreeNative(IntPtr appPtr, ulong cipherOptHandle, IntPtr self, ResultCb callback);

#endregion

#region CipherOptNewPlaintext

    public void CipherOptNewPlaintext(IntPtr appPtr, UlongCb callback) {
      CipherOptNewPlaintextNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "cipher_opt_new_plaintext")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "cipher_opt_new_plaintext")]
#endif
    public static extern void CipherOptNewPlaintextNative(IntPtr appPtr, IntPtr self, UlongCb callback);

#endregion

#region DecodeIpcMessage

    public void DecodeIpcMessage(
      string encodedReq,
      DecodeAuthCb authCb,
      DecodeUnregCb unregCb,
      DecodeContCb contCb,
      DecodeShareMDataCb shareMDataCb,
      DecodeRevokedCb revokedCb,
      ListBasedResultCb errorCb) {
      var cbs = new List<object> {authCb, unregCb, contCb, shareMDataCb, revokedCb, errorCb};
      DecodeIpcMessageNative(
        encodedReq,
        cbs.ToHandlePtr(),
        OnDecodeAuthCb,
        OnDecodeUnregCb,
        OnDecodeContCb,
        OnDecodeShareMDataCb,
        OnDecodeRevokedCb,
        OnListBasedResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "decode_ipc_msg")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "decode_ipc_msg")]
#endif
    public static extern void DecodeIpcMessageNative(
      string encodedReq,
      IntPtr self,
      DecodeAuthCb authCb,
      DecodeUnregCb unregCb,
      DecodeContCb contCb,
      DecodeShareMDataCb shareMDataCb,
      DecodeRevokedCb revokedCb,
      ListBasedResultCb errorCb);

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeAuthCb))]
#endif
    private static void OnDecodeAuthCb(IntPtr self, uint reqId, IntPtr authGrantedFfiPtr) {
      var cb = (DecodeAuthCb)self.HandlePtrToType<List<object>>()[0];
      cb(IntPtr.Zero, reqId, authGrantedFfiPtr);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeUnregCb))]
#endif
    private static void OnDecodeUnregCb(IntPtr self, uint reqId, IntPtr bsConfig, IntPtr bsSize) {
      var cb = (DecodeUnregCb)self.HandlePtrToType<List<object>>()[1];
      cb(IntPtr.Zero, reqId, bsConfig, bsSize);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeContCb))]
#endif
    private static void OnDecodeContCb(IntPtr self, uint reqId) {
      var cb = (DecodeContCb)self.HandlePtrToType<List<object>>()[2];
      cb(IntPtr.Zero, reqId);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeShareMDataCb))]
#endif
    private static void OnDecodeShareMDataCb(IntPtr self, uint reqId) {
      var cb = (DecodeShareMDataCb)self.HandlePtrToType<List<object>>()[3];
      cb(IntPtr.Zero, reqId);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeRevokedCb))]
#endif
    private static void OnDecodeRevokedCb(IntPtr self) {
      var cb = (DecodeRevokedCb)self.HandlePtrToType<List<object>>()[4];
      cb(IntPtr.Zero);
    }



#endregion

#region DecryptSealedBox

    public void DecryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, ulong skHandle, ByteArrayCb callback) {
      DecryptSealedBoxNative(appPtr, data, len, pkHandle, skHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "decrypt_sealed_box")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "decrypt_sealed_box")]
#endif
    public static extern void DecryptSealedBoxNative(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong pkHandle,
      ulong skHandle,
      IntPtr self,
      ByteArrayCb callback);

#endregion

#region EncGenerateKeyPair

    public void EncGenerateKeyPair(IntPtr appPtr, EncGenerateKeyPairCb callback) {
      EncGenerateKeyPairNative(appPtr, callback.ToHandlePtr(), OnEncGenerateKeyPairCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_generate_key_pair")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "enc_generate_key_pair")]
#endif
    public static extern void EncGenerateKeyPairNative(IntPtr appPtr, IntPtr self, EncGenerateKeyPairCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(EncGenerateKeyPairCb))]
#endif
    private static void OnEncGenerateKeyPairCb(IntPtr self, FfiResult result, ulong encPubKeyHandle, ulong encSecKeyHandle) {
      var cb = self.HandlePtrToType<EncGenerateKeyPairCb>();
      cb(IntPtr.Zero, result, encPubKeyHandle, encSecKeyHandle);
    }

#endregion

#region EncodeAuthReq

    public void EncodeAuthReq(IntPtr authReq, EncodeAuthReqCb callback) {
      EncodeAuthReqNative(authReq, callback.ToHandlePtr(), OnEncodeAuthReqCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "encode_auth_req")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "encode_auth_req")]
#endif
    public static extern void EncodeAuthReqNative(IntPtr authReq, IntPtr userDataPtr, EncodeAuthReqCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(EncodeAuthReqCb))]
#endif
    private static void OnEncodeAuthReqCb(IntPtr self, FfiResult result, uint requestId, string encodedReq) {
      var cb = self.HandlePtrToType<EncodeAuthReqCb>();
      cb(IntPtr.Zero, result, requestId, encodedReq);
    }

#endregion

#region EncPubKeyFree

    public void EncPubKeyFree(IntPtr appPtr, ulong encryptPubKeyHandle, ResultCb callback) {
      EncPubKeyFreeNative(appPtr, encryptPubKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_pub_key_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "enc_pub_key_free")]
#endif
    public static extern void EncPubKeyFreeNative(IntPtr appPtr, ulong encryptPubKeyHandle, IntPtr self, ResultCb callback);

#endregion

#region EncPubKeyGet

    public void EncPubKeyGet(IntPtr appPtr, ulong encryptPubKeyHandle, IntPtrCb callback) {
      EncPubKeyGetNative(appPtr, encryptPubKeyHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_pub_key_get")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "enc_pub_key_get")]
#endif
    public static extern void EncPubKeyGetNative(IntPtr appPtr, ulong encryptPubKeyHandle, IntPtr self, IntPtrCb callback);

#endregion

#region EncPubKeyNew

    public void EncPubKeyNew(IntPtr appPtr, IntPtr asymPublicKey, UlongCb callback) {
      EncPubKeyNewNative(appPtr, asymPublicKey, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_pub_key_new")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "enc_pub_key_new")]
#endif
    public static extern void EncPubKeyNewNative(IntPtr appPtr, IntPtr asymPublicKey, IntPtr self, UlongCb callback);

#endregion

#region EncryptSealedBox

    public void EncryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, ByteArrayCb callback) {
      EncryptSealedBoxNative(appPtr, data, len, pkHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "encrypt_sealed_box")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "encrypt_sealed_box")]
#endif
    public static extern void EncryptSealedBoxNative(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong pkHandle,
      IntPtr self,
      ByteArrayCb callback);

#endregion

#region EncSecretKeyFree

    public void EncSecretKeyFree(IntPtr appPtr, ulong encryptSecKeyHandle, ResultCb callback) {
      EncSecretKeyFreeNative(appPtr, encryptSecKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_secret_key_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "enc_secret_key_free")]
#endif
    public static extern void EncSecretKeyFreeNative(IntPtr appPtr, ulong encryptSecKeyHandle, IntPtr self, ResultCb callback);

#endregion

#region EncSecretKeyGet

    public void EncSecretKeyGet(IntPtr appPtr, ulong encryptSecKeyHandle, IntPtrCb callback) {
      EncSecretKeyGetNative(appPtr, encryptSecKeyHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_secret_key_get")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "enc_secret_key_get")]
#endif
    public static extern void EncSecretKeyGetNative(IntPtr appPtr, ulong encryptSecKeyHandle, IntPtr self, IntPtrCb callback);

#endregion

#region EncSecretKeyNew

    public void EncSecretKeyNew(IntPtr appPtr, IntPtr asymSecretKey, UlongCb callback) {
      EncSecretKeyNewNative(appPtr, asymSecretKey, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_secret_key_new")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "enc_secret_key_new")]
#endif
    public static extern void EncSecretKeyNewNative(IntPtr appPtr, IntPtr asymSecretKey, IntPtr self, UlongCb callback);

#endregion

#region FreeAppNative

    public void FreeApp(IntPtr appPtr) {
      FreeAppNative(appPtr);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "app_free")]
#endif
    public static extern void FreeAppNative(IntPtr appPtr);

#endregion

#region IDataCloseSelfEncryptor

    public void IDataCloseSelfEncryptor(IntPtr appPtr, ulong seHandle, ulong cipherOptHandle, IntPtrCb callback) {
      IDataCloseSelfEncryptorNative(appPtr, seHandle, cipherOptHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_close_self_encryptor")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_close_self_encryptor")]
#endif
    public static extern void IDataCloseSelfEncryptorNative(
      IntPtr appPtr,
      ulong seHandle,
      ulong cipherOptHandle,
      IntPtr self,
      IntPtrCb callback);

#endregion

#region IDataFetchSelfEncryptor

    public void IDataFetchSelfEncryptor(IntPtr appPtr, IntPtr xorNameArr, UlongCb callback) {
      IDataFetchSelfEncryptorNative(appPtr, xorNameArr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_fetch_self_encryptor")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_fetch_self_encryptor")]
#endif
    public static extern void IDataFetchSelfEncryptorNative(IntPtr appPtr, IntPtr xorNameArr, IntPtr self, UlongCb callback);

#endregion

#region IDataNewSelfEncryptor

    public void IDataNewSelfEncryptor(IntPtr appPtr, UlongCb callback) {
      IDataNewSelfEncryptorNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_new_self_encryptor")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_new_self_encryptor")]
#endif
    public static extern void IDataNewSelfEncryptorNative(IntPtr appPtr, IntPtr self, UlongCb callback);

#endregion

#region IDataReadFromSelfEncryptor

    public void IDataReadFromSelfEncryptor(IntPtr appPtr, ulong seHandle, ulong fromPos, ulong len, ByteArrayCb callback) {
      IDataReadFromSelfEncryptorNative(appPtr, seHandle, fromPos, len, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_read_from_self_encryptor")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_read_from_self_encryptor")]
#endif
    public static extern void IDataReadFromSelfEncryptorNative(
      IntPtr appPtr,
      ulong seHandle,
      ulong fromPos,
      ulong len,
      IntPtr self,
      ByteArrayCb callback);

#endregion

#region IDataSelfEncryptorReaderFree

    public void IDataSelfEncryptorReaderFree(IntPtr appPtr, ulong sEReaderHandle, ResultCb callback) {
      IDataSelfEncryptorReaderFreeNative(appPtr, sEReaderHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_self_encryptor_reader_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_self_encryptor_reader_free")]
#endif
    public static extern void IDataSelfEncryptorReaderFreeNative(IntPtr appPtr, ulong sEReaderHandle, IntPtr self, ResultCb callback);

#endregion

#region IDataSelfEncryptorWriterFree

    public void IDataSelfEncryptorWriterFree(IntPtr appPtr, ulong sEWriterHandle, ResultCb callback) {
      IDataSelfEncryptorWriterFreeNative(appPtr, sEWriterHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_self_encryptor_writer_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_self_encryptor_writer_free")]
#endif
    public static extern void IDataSelfEncryptorWriterFreeNative(IntPtr appPtr, ulong sEWriterHandle, IntPtr self, ResultCb callback);

#endregion

#region IDataSize

    public void IDataSize(IntPtr appPtr, ulong seHandle, UlongCb callback) {
      IDataSizeNative(appPtr, seHandle, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_size")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_size")]
#endif
    public static extern void IDataSizeNative(IntPtr appPtr, ulong seHandle, IntPtr self, UlongCb callback);

#endregion

#region IDataWriteToSelfEncryptor

    public void IDataWriteToSelfEncryptor(IntPtr appPtr, ulong seHandle, IntPtr data, IntPtr size, ResultCb callback) {
      IDataWriteToSelfEncryptorNative(appPtr, seHandle, data, size, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_write_to_self_encryptor")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "idata_write_to_self_encryptor")]
#endif
    public static extern void IDataWriteToSelfEncryptorNative(
      IntPtr appPtr,
      ulong seHandle,
      IntPtr data,
      IntPtr size,
      IntPtr self,
      ResultCb callback);

#endregion

#region MDataEntriesForEach

    public void MDataEntriesForEach(IntPtr appPtr, ulong entriesHandle, MDataEntriesForEachCb forEachCallback, ListBasedResultCb resultCallback) {
      var cbs = new List<object> {forEachCallback, resultCallback};
      MDataEntriesForEachNative(appPtr, entriesHandle, cbs.ToHandlePtr(), OnMDataEntriesForEachCb, OnListBasedResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_for_each")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entries_for_each")]
#endif
    public static extern void MDataEntriesForEachNative(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr self,
      MDataEntriesForEachCb forEachCallback,
      ListBasedResultCb resultCallback);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataEntriesForEachCb))]
#endif
    private static void OnMDataEntriesForEachCb(
      IntPtr self,
      IntPtr entryKey,
      IntPtr entryKeyLen,
      IntPtr entryVal,
      IntPtr entryValLen,
      ulong entryVersion) {
      var cb = (MDataEntriesForEachCb)self.HandlePtrToType<List<object>>(false)[0];
      cb(IntPtr.Zero, entryKey, entryKeyLen, entryVal, entryValLen, entryVersion);
    }

#endregion

#region MDataEntriesFree

    public void MDataEntriesFree(IntPtr appPtr, ulong entriesHandle, ResultCb callback) {
      MDataEntriesFreeNative(appPtr, entriesHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entries_free")]
#endif
    public static extern void MDataEntriesFreeNative(IntPtr appPtr, ulong entriesHandle, IntPtr self, ResultCb callback);

#endregion

#region MDataEntriesInsert

    public void MDataEntriesInsert(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      ResultCb callback) {
      MDataEntriesInsertNative(appPtr, entriesHandle, keyPtr, keyLen, valuePtr, valueLen, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_insert")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entries_insert")]
#endif
    public static extern void MDataEntriesInsertNative(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      IntPtr self,
      ResultCb callback);

#endregion

#region MDataEntriesLen

    public void MDataEntriesLen(IntPtr appPtr, ulong entriesHandle, MDataEntriesLenCb callback) {
      MDataEntriesLenNative(appPtr, entriesHandle, callback.ToHandlePtr(), OnMDataEntriesLenCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_len")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entries_len")]
#endif
    public static extern void MDataEntriesLenNative(IntPtr appPtr, ulong entriesHandle, IntPtr self, MDataEntriesLenCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataEntriesLenCb))]
#endif
    private static void OnMDataEntriesLenCb(IntPtr self, ulong len) {
      var cb = self.HandlePtrToType<MDataEntriesLenCb>();
      cb(IntPtr.Zero, len);
    }

#endregion

#region MDataEntriesNew

    public void MDataEntriesNew(IntPtr appPtr, UlongCb callback) {
      MDataEntriesNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_new")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entries_new")]
#endif
    public static extern void MDataEntriesNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

#endregion

#region MDataEntryActionsFree

    public void MDataEntryActionsFree(IntPtr appPtr, ulong actionsHandle, ResultCb callback) {
      MDataEntryActionsFreeNative(appPtr, actionsHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entry_actions_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entry_actions_free")]
#endif
    public static extern void MDataEntryActionsFreeNative(IntPtr appPtr, ulong actionsHandle, IntPtr self, ResultCb callback);

#endregion

#region MDataEntryActionsInsert

    public void MDataEntryActionsInsert(
      IntPtr appPtr,
      ulong actionsHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      ResultCb callback) {
      MDataEntryActionsInsertNative(appPtr, actionsHandle, keyPtr, keyLen, valuePtr, valueLen, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entry_actions_insert")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entry_actions_insert")]
#endif
    public static extern void MDataEntryActionsInsertNative(
      IntPtr appPtr,
      ulong actionsHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      IntPtr self,
      ResultCb callback);

#endregion

#region MDataEntryActionsNew

    public void MDataEntryActionsNew(IntPtr appPtr, UlongCb callback) {
      MDataEntryActionsNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entry_actions_new")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_entry_actions_new")]
#endif
    public static extern void MDataEntryActionsNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

#endregion

#region MDataGetValue

    public void MDataGetValue(IntPtr appPtr, ulong infoHandle, IntPtr keyPtr, IntPtr keyLen, MDataGetValueCb callback) {
      MDataGetValueNative(appPtr, infoHandle, keyPtr, keyLen, callback.ToHandlePtr(), OnMDataGetValueCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_get_value")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_get_value")]
#endif
    public static extern void MDataGetValueNative(
      IntPtr appPtr,
      ulong infoHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr self,
      MDataGetValueCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataGetValueCb))]
#endif
    private static void OnMDataGetValueCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen, ulong entryVersion) {
      var cb = self.HandlePtrToType<MDataGetValueCb>();
      cb(IntPtr.Zero, result, data, dataLen, entryVersion);
    }

#endregion

#region MDataInfoDecrypt

    public void MDataInfoDecrypt(IntPtr appPtr, ulong mDataInfoH, IntPtr cipherText, IntPtr cipherLen, ByteArrayCb callback) {
      MDataInfoDecryptNative(appPtr, mDataInfoH, cipherText, cipherLen, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_decrypt")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_decrypt")]
#endif
    public static extern void MDataInfoDecryptNative(
      IntPtr appPtr,
      ulong mDataInfoH,
      IntPtr cipherText,
      IntPtr cipherLen,
      IntPtr self,
      ByteArrayCb callback);

#endregion

#region MDataInfoDeserialise

    public void MDataInfoDeserialise(IntPtr appPtr, IntPtr ptr, IntPtr len, UlongCb callback) {
      MDataInfoDeserialiseNative(appPtr, ptr, len, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_deserialise")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_deserialise")]
#endif
    public static extern void MDataInfoDeserialiseNative(IntPtr appPtr, IntPtr ptr, IntPtr len, IntPtr self, UlongCb callback);

#endregion

#region MDataInfoEncryptEntryKey

    public void MDataInfoEncryptEntryKey(IntPtr appPtr, ulong infoH, IntPtr inputPtr, IntPtr inputLen, ByteArrayCb callback) {
      MDataInfoEncryptEntryKeyNative(appPtr, infoH, inputPtr, inputLen, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_encrypt_entry_key")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_encrypt_entry_key")]
#endif
    public static extern void MDataInfoEncryptEntryKeyNative(
      IntPtr appPtr,
      ulong infoH,
      IntPtr inputPtr,
      IntPtr inputLen,
      IntPtr self,
      ByteArrayCb callback);

#endregion

#region MDataInfoEncryptEntryValue

    public void MDataInfoEncryptEntryValue(IntPtr appPtr, ulong infoH, IntPtr inputPtr, IntPtr inputLen, ByteArrayCb callback) {
      MDataInfoEncryptEntryValueNative(appPtr, infoH, inputPtr, inputLen, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_encrypt_entry_value")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_encrypt_entry_value")]
#endif
    public static extern void MDataInfoEncryptEntryValueNative(
      IntPtr appPtr,
      ulong infoH,
      IntPtr inputPtr,
      IntPtr inputLen,
      IntPtr self,
      ByteArrayCb callback);

#endregion

#region MDataInfoFree

    public void MDataInfoFree(IntPtr appPtr, ulong infoHandle, ResultCb callback) {
      MDataInfoFreeNative(appPtr, infoHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_free")]
#endif
    public static extern void MDataInfoFreeNative(IntPtr appPtr, ulong infoHandle, IntPtr self, ResultCb callback);

#endregion

#region MDataInfoNewPublic

    public void MDataInfoNewPublic(IntPtr appPtr, IntPtr xorNameArr, ulong typeTag, UlongCb callback) {
      MDataInfoNewPublicNative(appPtr, xorNameArr, typeTag, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_new_public")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_new_public")]
#endif
    public static extern void MDataInfoNewPublicNative(IntPtr appPtr, IntPtr xorNameArr, ulong typeTag, IntPtr self, UlongCb callback);

#endregion

#region MDataInfoRandomPrivate

    public void MDataInfoRandomPrivate(IntPtr appPtr, ulong typeTag, UlongCb callback) {
      MDataInfoRandomPrivateNative(appPtr, typeTag, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_random_private")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_random_private")]
#endif
    public static extern void MDataInfoRandomPrivateNative(IntPtr appPtr, ulong typeTag, IntPtr self, UlongCb callback);

#endregion

#region MDataInfoRandomPublic

    public void MDataInfoRandomPublic(IntPtr appPtr, ulong typeTag, UlongCb callback) {
      MDataInfoRandomPublicNative(appPtr, typeTag, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_random_public")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_random_public")]
#endif
    public static extern void MDataInfoRandomPublicNative(IntPtr appPtr, ulong typeTag, IntPtr self, UlongCb callback);

#endregion

#region MDataInfoSerialise

    public void MDataInfoSerialise(IntPtr appPtr, ulong infoHandle, ByteArrayCb callback) {
      MDataInfoSerialiseNative(appPtr, infoHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_serialise")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_info_serialise")]
#endif
    public static extern void MDataInfoSerialiseNative(IntPtr appPtr, ulong infoHandle, IntPtr self, ByteArrayCb callback);

#endregion

#region MDataKeysForEach

    public void MDataKeysForEach(IntPtr appPtr, ulong keysHandle, MDataKeysForEachCb forEachCb, ResultCb resCb) {
      var cbs = new List<object> {forEachCb, resCb};
      var a = cbs.ToHandlePtr();
      Debug.WriteLine(a);
      MDataKeysForEachNative(appPtr, keysHandle, a, OnMDataKeysForEachCb, OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_keys_for_each")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_keys_for_each")]
#endif
    public static extern void MDataKeysForEachNative(
      IntPtr appPtr,
      ulong keysHandle,
      IntPtr self,
      MDataKeysForEachCb forEachCb,
      ResultCb resCb);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataKeysForEachCb))]
#endif
    private static void OnMDataKeysForEachCb(IntPtr self, IntPtr bytePtr, IntPtr byteLen) {
      var cb = (MDataKeysForEachCb)self.HandlePtrToType<List<object>>(false)[0];
      cb(IntPtr.Zero, bytePtr, byteLen);
    }

#endregion

#region MDataKeysFree

    public void MDataKeysFree(IntPtr appPtr, ulong keysHandle, ResultCb callback) {
      MDataKeysFreeNative(appPtr, keysHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_keys_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_keys_free")]
#endif
    public static extern void MDataKeysFreeNative(IntPtr appPtr, ulong keysHandle, IntPtr self, ResultCb callback);

#endregion

#region MDataKeysLen

    public void MDataKeysLen(IntPtr appPtr, ulong keysHandle, IntPtrCb callback) {
      MDataKeysLenNative(appPtr, keysHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_keys_len")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_keys_len")]
#endif
    public static extern void MDataKeysLenNative(IntPtr appPtr, ulong keysHandle, IntPtr self, IntPtrCb callback);

#endregion

#region MDataListEntries

    public void MDataListEntries(IntPtr appPtr, ulong infoHandle, UlongCb callback) {
      MDataListEntriesNative(appPtr, infoHandle, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_list_entries")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_list_entries")]
#endif
    public static extern void MDataListEntriesNative(IntPtr appPtr, ulong infoHandle, IntPtr self, UlongCb callback);

#endregion

#region MDataListKeys

    public void MDataListKeys(IntPtr appPtr, ulong infoHandle, UlongCb callback) {
      MDataListKeysNative(appPtr, infoHandle, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_list_keys")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_list_keys")]
#endif
    public static extern void MDataListKeysNative(IntPtr appPtr, ulong infoHandle, IntPtr self, UlongCb callback);

#endregion

#region MDataMutateEntries

    public void MDataMutateEntries(IntPtr appPtr, ulong infoHandle, ulong actionsHandle, ResultCb callback) {
      MDataMutateEntriesNative(appPtr, infoHandle, actionsHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_mutate_entries")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_mutate_entries")]
#endif
    public static extern void MDataMutateEntriesNative(
      IntPtr appPtr,
      ulong infoHandle,
      ulong actionsHandle,
      IntPtr self,
      ResultCb callback);

#endregion

#region MDataPermissionSetAllow

    public void MDataPermissionSetAllow(IntPtr appPtr, ulong setHandle, MDataAction action, ResultCb callback) {
      MDataPermissionSetAllowNative(appPtr, setHandle, action, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permission_set_allow")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_permission_set_allow")]
#endif
    public static extern void MDataPermissionSetAllowNative(
      IntPtr appPtr,
      ulong setHandle,
      MDataAction action,
      IntPtr self,
      ResultCb callback);

#endregion

#region MDataPermissionSetFree

    public void MDataPermissionSetFree(IntPtr appPtr, ulong setHandle, ResultCb callback) {
      MDataPermissionSetFreeNative(appPtr, setHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permission_set_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_permission_set_free")]
#endif
    public static extern void MDataPermissionSetFreeNative(IntPtr appPtr, ulong setHandle, IntPtr self, ResultCb callback);

#endregion

#region MDataPermissionSetNew

    public void MDataPermissionSetNew(IntPtr appPtr, UlongCb callback) {
      MDataPermissionSetNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permission_set_new")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_permission_set_new")]
#endif
    public static extern void MDataPermissionSetNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

#endregion

#region MDataPermissionsFree

    public void MDataPermissionsFree(IntPtr appPtr, ulong permissionsHandle, ResultCb callback) {
      MDataPermissionsFreeNative(appPtr, permissionsHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permissions_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_permissions_free")]
#endif
    public static extern void MDataPermissionsFreeNative(IntPtr appPtr, ulong permissionsHandle, IntPtr self, ResultCb callback);

#endregion

#region MDataPermissionsInsert

    public void MDataPermissionsInsert(
      IntPtr appPtr,
      ulong permissionsHandle,
      ulong userHandle,
      ulong permissionSetHandle,
      ResultCb callback) {
      MDataPermissionsInsertNative(appPtr, permissionsHandle, userHandle, permissionSetHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permissions_insert")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_permissions_insert")]
#endif
    public static extern void MDataPermissionsInsertNative(
      IntPtr appPtr,
      ulong permissionsHandle,
      ulong userHandle,
      ulong permissionSetHandle,
      IntPtr self,
      ResultCb callback);

#endregion

#region MDataPermissionsNew

    public void MDataPermissionsNew(IntPtr appPtr, UlongCb callback) {
      MDataPermissionsNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permissions_new")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_permissions_new")]
#endif
    public static extern void MDataPermissionsNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

#endregion

#region MDataPut

    public void MDataPut(IntPtr appPtr, ulong infoHandle, ulong permissionsHandle, ulong entriesHandle, ResultCb callback) {
      MDataPutNative(appPtr, infoHandle, permissionsHandle, entriesHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_put")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "mdata_put")]
#endif
    public static extern void MDataPutNative(
      IntPtr appPtr,
      ulong infoHandle,
      ulong permissionsHandle,
      ulong entriesHandle,
      IntPtr self,
      ResultCb callback);

#endregion

#region Sha3Hash

    public void Sha3Hash(IntPtr data, IntPtr len, ByteArrayCb callback) {
      Sha3HashNative(data, len, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sha3_hash")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "sha3_hash")]
#endif
    public static extern void Sha3HashNative(IntPtr data, IntPtr len, IntPtr self, ByteArrayCb callback);

#endregion

#region SignKeyFree

    public void SignKeyFree(IntPtr appPtr, ulong signKeyHandle, ResultCb callback) {
      SignKeyFreeNative(appPtr, signKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_key_free")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "sign_key_free")]
#endif
    public static extern void SignKeyFreeNative(IntPtr appPtr, ulong signKeyHandle, IntPtr self, ResultCb callback);

#endregion
  }
}

#endif
