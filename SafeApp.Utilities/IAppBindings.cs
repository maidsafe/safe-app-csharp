using System;

namespace SafeApp.Utilities {
  public interface IAppBindings {
    void AccessContainerGetContainerMDataInfo(IntPtr appPtr, string name, UlongCb callback);
    void AppExeFileStem(StringCb callback);
    void AppInitLogging(string fileName, ResultCb callback);
    void AppOutputLogPath(string fileName, StringCb callback);
    void AppPubSignKey(IntPtr appPtr, UlongCb callback);

    void AppRegistered(string appId, IntPtr authGrantedFfiPtr, IntCb netObsCb, IntPtrCb appRegCb);

    void AppSetAdditionalSearchPath(string path, ResultCb callback);
    void CipherOptFree(IntPtr appPtr, ulong cipherOptHandle, ResultCb callback);
    void CipherOptNewPlaintext(IntPtr appPtr, UlongCb callback);

    void DecodeIpcMessage(
      string encodedReq,
      DecodeAuthCb authCb,
      DecodeUnregCb unregCb,
      DecodeContCb contCb,
      DecodeShareMDataCb shareMDataCb,
      DecodeRevokedCb revokedCb,
      ListBasedResultCb errorCb);

    void DecryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, ulong skHandle, ByteArrayCb callback);
    void EncGenerateKeyPair(IntPtr appPtr, EncGenerateKeyPairCb callback);
    void EncodeAuthReq(IntPtr authReq, EncodeAuthReqCb callback);
    void EncPubKeyFree(IntPtr appPtr, ulong encryptPubKeyHandle, ResultCb callback);
    void EncPubKeyGet(IntPtr appPtr, ulong encryptPubKeyHandle, IntPtrCb callback);
    void EncPubKeyNew(IntPtr appPtr, IntPtr asymPublicKey, UlongCb callback);
    void EncryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, ByteArrayCb callback);
    void EncSecretKeyFree(IntPtr appPtr, ulong encryptSecKeyHandle, ResultCb callback);
    void EncSecretKeyGet(IntPtr appPtr, ulong encryptSecKeyHandle, IntPtrCb callback);
    void EncSecretKeyNew(IntPtr appPtr, IntPtr asymSecretKey, UlongCb callback);
    void FreeApp(IntPtr appPtr);

    void MDataEntriesForEach(IntPtr appPtr, ulong entriesHandle, MDataEntriesForEachCb forEachCallback, ListBasedResultCb resultCallback);

    void MDataEntriesFree(IntPtr appPtr, ulong entriesHandle, ResultCb callback);

    void MDataEntriesInsert(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      ResultCb callback);

    void MDataEntriesLen(IntPtr appPtr, ulong entriesHandle, MDataEntriesLenCb callback);
    void MDataEntriesNew(IntPtr appPtr, UlongCb callback);
    void MDataEntryActionsFree(IntPtr appPtr, ulong actionsHandle, ResultCb callback);

    void MDataEntryActionsInsert(
      IntPtr appPtr,
      ulong actionsHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      ResultCb callback);

    void MDataEntryActionsNew(IntPtr appPtr, UlongCb callback);
    void MDataGetValue(IntPtr appPtr, ulong infoHandle, IntPtr keyPtr, IntPtr keyLen, MDataGetValueCb callback);
    void MDataInfoDecrypt(IntPtr appPtr, ulong mDataInfoH, IntPtr cipherText, IntPtr cipherLen, ByteArrayCb callback);
    void MDataInfoDeserialise(IntPtr appPtr, IntPtr ptr, IntPtr len, UlongCb callback);

    void MDataInfoEncryptEntryKey(IntPtr appPtr, ulong infoH, IntPtr inputPtr, IntPtr inputLen, ByteArrayCb callback);

    void MDataInfoEncryptEntryValue(IntPtr appPtr, ulong infoH, IntPtr inputPtr, IntPtr inputLen, ByteArrayCb callback);

    void MDataInfoFree(IntPtr appPtr, ulong infoHandle, ResultCb callback);
    void MDataInfoNewPublic(IntPtr appPtr, IntPtr xorNameArr, ulong typeTag, UlongCb callback);
    void MDataInfoRandomPrivate(IntPtr appPtr, ulong typeTag, UlongCb callback);
    void MDataInfoRandomPublic(IntPtr appPtr, ulong typeTag, UlongCb callback);
    void MDataInfoSerialise(IntPtr appPtr, ulong infoHandle, ByteArrayCb callback);
    void MDataKeysForEach(IntPtr appPtr, ulong keysHandle, MDataKeysForEachCb forEachCb, ResultCb resCb);

    void MDataKeysFree(IntPtr appPtr, ulong keysHandle, ResultCb callback);

    void MDataKeysLen(IntPtr appPtr, ulong keysHandle, IntPtrCb callback);
    void MDataListEntries(IntPtr appPtr, ulong infoHandle, UlongCb callback);
    void MDataListKeys(IntPtr appPtr, ulong infoHandle, UlongCb callback);
    void MDataMutateEntries(IntPtr appPtr, ulong infoHandle, ulong actionsHandle, ResultCb callback);
    void MDataPermissionSetAllow(IntPtr appPtr, ulong setHandle, MDataAction action, ResultCb callback);
    void MDataPermissionSetFree(IntPtr appPtr, ulong setHandle, ResultCb callback);
    void MDataPermissionSetNew(IntPtr appPtr, UlongCb callback);
    void MDataPermissionsFree(IntPtr appPtr, ulong permissionsHandle, ResultCb callback);

    void MDataPermissionsInsert(IntPtr appPtr, ulong permissionsHandle, ulong userHandle, ulong permissionSetHandle, ResultCb callback);

    void MDataPermissionsNew(IntPtr appPtr, UlongCb callback);
    void MDataPut(IntPtr appPtr, ulong infoHandle, ulong permissionsHandle, ulong entriesHandle, ResultCb callback);
    void Sha3Hash(IntPtr data, IntPtr len, ByteArrayCb callback);
    void SignKeyFree(IntPtr appPtr, ulong signKeyHandle, ResultCb callback);

    // ReSharper disable InconsistentNaming
    void IDataCloseSelfEncryptor(IntPtr appPtr, ulong seH, ulong cipherOptH, IntPtrCb callback);

    void IDataFetchSelfEncryptor(IntPtr appPtr, IntPtr xorNameArr, UlongCb callback);
    void IDataNewSelfEncryptor(IntPtr appPtr, UlongCb callback);

    void IDataReadFromSelfEncryptor(IntPtr appPtr, ulong seHandle, ulong fromPos, ulong len, ByteArrayCb callback);

    void IDataSelfEncryptorReaderFree(IntPtr appPtr, ulong sEReaderHandle, ResultCb callback);
    void IDataSelfEncryptorWriterFree(IntPtr appPtr, ulong sEWriterHandle, ResultCb callback);
    void IDataSize(IntPtr appPtr, ulong seHandle, UlongCb callback);

    void IDataWriteToSelfEncryptor(IntPtr appPtr, ulong seHandle, IntPtr data, IntPtr size, ResultCb callback);
    // ReSharper restore InconsistentNaming
  }
}
