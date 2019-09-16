#if !NETSTANDARD1_2 || __DESKTOP__
#if __IOS__
using ObjCRuntime;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Utilities;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SafeApp.AppBindings
{
    internal partial class AppBindings : IAppBindings
    {
#if __IOS__
        private const string DllName = "__Internal";
#else
        private const string DllName = "safe_app";
#endif

        public bool IsMockBuild()
        {
            var ret = AppIsMockNative();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_is_mock")]
        private static extern bool AppIsMockNative();

        [DllImport(DllName, EntryPoint = "app_unregistered")]
        private static extern void AppUnregisteredNative(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            byte[] bootstrapConfig,
            UIntPtr bootstrapConfigLen,
            IntPtr userData,
            NoneCb oDisconnectNotifierCb,
            FfiResultAppCb oCb);

        [DllImport(DllName, EntryPoint = "app_registered")]
        private static extern void AppRegisteredNative(
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            ref AuthGrantedNative authGranted,
            IntPtr userData,
            NoneCb oDisconnectNotifierCb,
            FfiResultAppCb oCb);

        public Task AppReconnectAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AppReconnectNative(app, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_reconnect")]
        private static extern void AppReconnectNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

        public Task<AccountInfo> AppAccountInfoAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<AccountInfo>();
            AppAccountInfoNative(app, userData, DelegateOnFfiResultAccountInfoCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_account_info")]
        private static extern void AppAccountInfoNative(IntPtr app, IntPtr userData, FfiResultAccountInfoCb oCb);

        public Task<string> AppExeFileStemAsync()
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AppExeFileStemNative(userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_exe_file_stem")]
        private static extern void AppExeFileStemNative(IntPtr userData, FfiResultStringCb oCb);

        public Task AppSetAdditionalSearchPathAsync(string newPath)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AppSetAdditionalSearchPathNative(newPath, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_set_additional_search_path")]
        private static extern void AppSetAdditionalSearchPathNative(
            [MarshalAs(UnmanagedType.LPStr)] string newPath,
            IntPtr userData,
            FfiResultCb oCb);

        public void AppFree(IntPtr app)
        {
            AppFreeNative(app);
        }

        [DllImport(DllName, EntryPoint = "app_free")]
        private static extern void AppFreeNative(IntPtr app);

        public Task AppResetObjectCacheAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AppResetObjectCacheNative(app, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_reset_object_cache")]
        private static extern void AppResetObjectCacheNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

        public Task<string> AppContainerNameAsync(string appId)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AppContainerNameNative(appId, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_container_name")]
        private static extern void AppContainerNameNative(
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task AccessContainerRefreshAccessInfoAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AccessContainerRefreshAccessInfoNative(app, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "access_container_refresh_access_info")]
        private static extern void AccessContainerRefreshAccessInfoNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

        public Task<List<ContainerPermissions>> AccessContainerFetchAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<ContainerPermissions>>();
            AccessContainerFetchNative(app, userData, DelegateOnFfiResultContainerPermissionsListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "access_container_fetch")]
        private static extern void AccessContainerFetchNative(IntPtr app, IntPtr userData, FfiResultContainerPermissionsListCb oCb);

        public Task<MDataInfo> AccessContainerGetContainerMDataInfoAsync(IntPtr app, string name)
        {
            var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
            AccessContainerGetContainerMDataInfoNative(app, name, userData, DelegateOnFfiResultMDataInfoCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "access_container_get_container_mdata_info")]
        private static extern void AccessContainerGetContainerMDataInfoNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            IntPtr userData,
            FfiResultMDataInfoCb oCb);

        public Task<ulong> CipherOptNewPlaintextAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            CipherOptNewPlaintextNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "cipher_opt_new_plaintext")]
        private static extern void CipherOptNewPlaintextNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task<ulong> CipherOptNewSymmetricAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            CipherOptNewSymmetricNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "cipher_opt_new_symmetric")]
        private static extern void CipherOptNewSymmetricNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task<ulong> CipherOptNewAsymmetricAsync(IntPtr app, ulong peerEncryptKeyH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            CipherOptNewAsymmetricNative(app, peerEncryptKeyH, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "cipher_opt_new_asymmetric")]
        private static extern void CipherOptNewAsymmetricNative(IntPtr app, ulong peerEncryptKeyH, IntPtr userData, FfiResultULongCb oCb);

        public Task CipherOptFreeAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            CipherOptFreeNative(app, handle, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "cipher_opt_free")]
        private static extern void CipherOptFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

        public Task<ulong> AppPubSignKeyAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            AppPubSignKeyNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_pub_sign_key")]
        private static extern void AppPubSignKeyNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task<(ulong, ulong)> SignGenerateKeyPairAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(ulong, ulong)>();
            SignGenerateKeyPairNative(app, userData, DelegateOnFfiResultULongULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign_generate_key_pair")]
        private static extern void SignGenerateKeyPairNative(IntPtr app, IntPtr userData, FfiResultULongULongCb oCb);

        public Task<ulong> SignPubKeyNewAsync(IntPtr app, byte[] data)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            SignPubKeyNewNative(app, data, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign_pub_key_new")]
        private static extern void SignPubKeyNewNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
            byte[] data,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<byte[]> SignPubKeyGetAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
            SignPubKeyGetNative(app, handle, userData, DelegateOnFfiResultByteArraySignPublicKeyLenCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign_pub_key_get")]
        private static extern void SignPubKeyGetNative(IntPtr app, ulong handle, IntPtr userData, FfiResultByteArraySignPublicKeyLenCb oCb);

        public Task SignPubKeyFreeAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            SignPubKeyFreeNative(app, handle, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign_pub_key_free")]
        private static extern void SignPubKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

        public Task<ulong> SignSecKeyNewAsync(IntPtr app, byte[] data)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            SignSecKeyNewNative(app, data, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign_sec_key_new")]
        private static extern void SignSecKeyNewNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.SignSecretKeyLen)]
            byte[] data,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<byte[]> SignSecKeyGetAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
            SignSecKeyGetNative(app, handle, userData, DelegateOnFfiResultByteArraySignSecretKeyLenCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign_sec_key_get")]
        private static extern void SignSecKeyGetNative(IntPtr app, ulong handle, IntPtr userData, FfiResultByteArraySignSecretKeyLenCb oCb);

        public Task SignSecKeyFreeAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            SignSecKeyFreeNative(app, handle, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign_sec_key_free")]
        private static extern void SignSecKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

        public Task<ulong> AppPubEncKeyAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            AppPubEncKeyNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_pub_enc_key")]
        private static extern void AppPubEncKeyNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task<(ulong, ulong)> EncGenerateKeyPairAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(ulong, ulong)>();
            EncGenerateKeyPairNative(app, userData, DelegateOnFfiResultULongULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "enc_generate_key_pair")]
        private static extern void EncGenerateKeyPairNative(IntPtr app, IntPtr userData, FfiResultULongULongCb oCb);

        public Task<ulong> EncPubKeyNewAsync(IntPtr app, byte[] data)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            EncPubKeyNewNative(app, data, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "enc_pub_key_new")]
        private static extern void EncPubKeyNewNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.AsymPublicKeyLen)]
            byte[] data,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<byte[]> EncPubKeyGetAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
            EncPubKeyGetNative(app, handle, userData, DelegateOnFfiResultByteArrayAsymPublicKeyLenCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "enc_pub_key_get")]
        private static extern void EncPubKeyGetNative(IntPtr app, ulong handle, IntPtr userData, FfiResultByteArrayAsymPublicKeyLenCb oCb);

        public Task EncPubKeyFreeAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            EncPubKeyFreeNative(app, handle, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "enc_pub_key_free")]
        private static extern void EncPubKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

        public Task<ulong> EncSecretKeyNewAsync(IntPtr app, byte[] data)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            EncSecretKeyNewNative(app, data, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "enc_secret_key_new")]
        private static extern void EncSecretKeyNewNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.AsymSecretKeyLen)]
            byte[] data,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<byte[]> EncSecretKeyGetAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
            EncSecretKeyGetNative(app, handle, userData, DelegateOnFfiResultByteArrayAsymSecretKeyLenCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "enc_secret_key_get")]
        private static extern void EncSecretKeyGetNative(
            IntPtr app,
            ulong handle,
            IntPtr userData,
            FfiResultByteArrayAsymSecretKeyLenCb oCb);

        public Task EncSecretKeyFreeAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            EncSecretKeyFreeNative(app, handle, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "enc_secret_key_free")]
        private static extern void EncSecretKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

        public Task<List<byte>> SignAsync(IntPtr app, List<byte> data, ulong signSkH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            SignNative(app, data?.ToArray(), (UIntPtr)(data?.Count ?? 0), signSkH, userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sign")]
        private static extern void SignNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] data,
            UIntPtr dataLen,
            ulong signSkH,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> VerifyAsync(IntPtr app, List<byte> signedData, ulong signPkH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            VerifyNative(app, signedData?.ToArray(), (UIntPtr)(signedData?.Count ?? 0), signPkH, userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "verify")]
        private static extern void VerifyNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] signedData,
            UIntPtr signedDataLen,
            ulong signPkH,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> EncryptAsync(IntPtr app, List<byte> data, ulong publicKeyH, ulong secretKeyH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            EncryptNative(
                app,
                data?.ToArray(),
                (UIntPtr)(data?.Count ?? 0),
                publicKeyH,
                secretKeyH,
                userData,
                DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encrypt")]
        private static extern void EncryptNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] data,
            UIntPtr dataLen,
            ulong publicKeyH,
            ulong secretKeyH,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> DecryptAsync(IntPtr app, List<byte> data, ulong publicKeyH, ulong secretKeyH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            DecryptNative(
                app,
                data?.ToArray(),
                (UIntPtr)(data?.Count ?? 0),
                publicKeyH,
                secretKeyH,
                userData,
                DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "decrypt")]
        private static extern void DecryptNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] data,
            UIntPtr dataLen,
            ulong publicKeyH,
            ulong secretKeyH,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> EncryptSealedBoxAsync(IntPtr app, List<byte> data, ulong publicKeyH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            EncryptSealedBoxNative(app, data?.ToArray(), (UIntPtr)(data?.Count ?? 0), publicKeyH, userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encrypt_sealed_box")]
        private static extern void EncryptSealedBoxNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] data,
            UIntPtr dataLen,
            ulong publicKeyH,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> DecryptSealedBoxAsync(IntPtr app, List<byte> data, ulong publicKeyH, ulong secretKeyH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            DecryptSealedBoxNative(
                app,
                data?.ToArray(),
                (UIntPtr)(data?.Count ?? 0),
                publicKeyH,
                secretKeyH,
                userData,
                DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "decrypt_sealed_box")]
        private static extern void DecryptSealedBoxNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] data,
            UIntPtr dataLen,
            ulong publicKeyH,
            ulong secretKeyH,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> Sha3HashAsync(List<byte> data)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            Sha3HashNative(data?.ToArray(), (UIntPtr)(data?.Count ?? 0), userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sha3_hash")]
        private static extern void Sha3HashNative(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            byte[] data,
            UIntPtr dataLen,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<byte[]> GenerateNonceAsync()
        {
            var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
            GenerateNonceNative(userData, DelegateOnFfiResultByteArrayAsymNonceLenCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "generate_nonce")]
        private static extern void GenerateNonceNative(IntPtr userData, FfiResultByteArrayAsymNonceLenCb oCb);

        public Task<ulong> IDataNewSelfEncryptorAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            IDataNewSelfEncryptorNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_new_self_encryptor")]
        private static extern void IDataNewSelfEncryptorNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task IDataWriteToSelfEncryptorAsync(IntPtr app, ulong seH, List<byte> data)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            IDataWriteToSelfEncryptorNative(app, seH, data?.ToArray(), (UIntPtr)(data?.Count ?? 0), userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_write_to_self_encryptor")]
        private static extern void IDataWriteToSelfEncryptorNative(
            IntPtr app,
            ulong seH,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] data,
            UIntPtr dataLen,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<byte[]> IDataCloseSelfEncryptorAsync(IntPtr app, ulong seH, ulong cipherOptH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
            IDataCloseSelfEncryptorNative(app, seH, cipherOptH, userData, DelegateOnFfiResultByteArrayXorNameLenCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_close_self_encryptor")]
        private static extern void IDataCloseSelfEncryptorNative(
            IntPtr app,
            ulong seH,
            ulong cipherOptH,
            IntPtr userData,
            FfiResultByteArrayXorNameLenCb oCb);

        public Task<ulong> IDataFetchSelfEncryptorAsync(IntPtr app, byte[] name)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            IDataFetchSelfEncryptorNative(app, name, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_fetch_self_encryptor")]
        private static extern void IDataFetchSelfEncryptorNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.XorNameLen)]
            byte[] name,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<ulong> IDataSerialisedSizeAsync(IntPtr app, byte[] name)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            IDataSerialisedSizeNative(app, name, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_serialised_size")]
        private static extern void IDataSerialisedSizeNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.XorNameLen)]
            byte[] name,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<ulong> IDataSizeAsync(IntPtr app, ulong seH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            IDataSizeNative(app, seH, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_size")]
        private static extern void IDataSizeNative(IntPtr app, ulong seH, IntPtr userData, FfiResultULongCb oCb);

        public Task<List<byte>> IDataReadFromSelfEncryptorAsync(IntPtr app, ulong seH, ulong fromPos, ulong len)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            IDataReadFromSelfEncryptorNative(app, seH, fromPos, len, userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_read_from_self_encryptor")]
        private static extern void IDataReadFromSelfEncryptorNative(
            IntPtr app,
            ulong seH,
            ulong fromPos,
            ulong len,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task IDataSelfEncryptorWriterFreeAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            IDataSelfEncryptorWriterFreeNative(app, handle, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_self_encryptor_writer_free")]
        private static extern void IDataSelfEncryptorWriterFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

        public Task IDataSelfEncryptorReaderFreeAsync(IntPtr app, ulong handle)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            IDataSelfEncryptorReaderFreeNative(app, handle, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "idata_self_encryptor_reader_free")]
        private static extern void IDataSelfEncryptorReaderFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

        public Task<(uint, string)> EncodeAuthReqAsync(ref AuthReq req)
        {
            var reqNative = req.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
            EncodeAuthReqNative(ref reqNative, userData, DelegateOnFfiResultUIntStringCb);
            reqNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_auth_req")]
        private static extern void EncodeAuthReqNative(ref AuthReqNative req, IntPtr userData, FfiResultUIntStringCb oCb);

        public Task<(uint, string)> EncodeContainersReqAsync(ref ContainersReq req)
        {
            var reqNative = req.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
            EncodeContainersReqNative(ref reqNative, userData, DelegateOnFfiResultUIntStringCb);
            reqNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_containers_req")]
        private static extern void EncodeContainersReqNative(ref ContainersReqNative req, IntPtr userData, FfiResultUIntStringCb oCb);

        public Task<(uint, string)> EncodeUnregisteredReqAsync(List<byte> extraData)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
            EncodeUnregisteredReqNative(extraData?.ToArray(), (UIntPtr)(extraData?.Count ?? 0), userData, DelegateOnFfiResultUIntStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_unregistered_req")]
        private static extern void EncodeUnregisteredReqNative(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            byte[] extraData,
            UIntPtr extraDataLen,
            IntPtr userData,
            FfiResultUIntStringCb oCb);

        public Task<(uint, string)> EncodeShareMDataReqAsync(ref ShareMDataReq req)
        {
            var reqNative = req.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
            EncodeShareMDataReqNative(ref reqNative, userData, DelegateOnFfiResultUIntStringCb);
            reqNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_share_mdata_req")]
        private static extern void EncodeShareMDataReqNative(ref ShareMDataReqNative req, IntPtr userData, FfiResultUIntStringCb oCb);

        [DllImport(DllName, EntryPoint = "decode_ipc_msg")]
        private static extern void DecodeIpcMsgNative(
            [MarshalAs(UnmanagedType.LPStr)] string msg,
            IntPtr userData,
            UIntAuthGrantedCb oAuth,
            UIntByteListCb oUnregistered,
            UIntCb oContainers,
            UIntCb oShareMData,
            NoneCb oRevoked,
            FfiResultUIntCb oErr);

        public Task AppInitLoggingAsync(string outputFileNameOverride)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AppInitLoggingNative(outputFileNameOverride, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_init_logging")]
        private static extern void AppInitLoggingNative(
            [MarshalAs(UnmanagedType.LPStr)] string outputFileNameOverride,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<string> AppOutputLogPathAsync(string outputFileName)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AppOutputLogPathNative(outputFileName, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "app_output_log_path")]
        private static extern void AppOutputLogPathNative(
            [MarshalAs(UnmanagedType.LPStr)] string outputFileName,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<MDataInfo> MDataInfoNewPrivateAsync(byte[] name, ulong typeTag, byte[] secretKey, byte[] nonce)
        {
            var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
            MDataInfoNewPrivateNative(name, typeTag, secretKey, nonce, userData, DelegateOnFfiResultMDataInfoCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_new_private")]
        private static extern void MDataInfoNewPrivateNative(
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.XorNameLen)]
            byte[] name,
            ulong typeTag,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.SymKeyLen)]
            byte[] secretKey,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.SymNonceLen)]
            byte[] nonce,
            IntPtr userData,
            FfiResultMDataInfoCb oCb);

        public Task<MDataInfo> MDataInfoRandomPublicAsync(ulong typeTag)
        {
            var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
            MDataInfoRandomPublicNative(typeTag, userData, DelegateOnFfiResultMDataInfoCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_random_public")]
        private static extern void MDataInfoRandomPublicNative(ulong typeTag, IntPtr userData, FfiResultMDataInfoCb oCb);

        public Task<MDataInfo> MDataInfoRandomPrivateAsync(ulong typeTag)
        {
            var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
            MDataInfoRandomPrivateNative(typeTag, userData, DelegateOnFfiResultMDataInfoCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_random_private")]
        private static extern void MDataInfoRandomPrivateNative(ulong typeTag, IntPtr userData, FfiResultMDataInfoCb oCb);

        public Task<List<byte>> MDataInfoEncryptEntryKeyAsync(ref MDataInfo info, List<byte> input)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            MDataInfoEncryptEntryKeyNative(
                ref info,
                input?.ToArray(),
                (UIntPtr)(input?.Count ?? 0),
                userData,
                DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_encrypt_entry_key")]
        private static extern void MDataInfoEncryptEntryKeyNative(
            ref MDataInfo info,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] input,
            UIntPtr inputLen,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> MDataInfoEncryptEntryValueAsync(ref MDataInfo info, List<byte> input)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            MDataInfoEncryptEntryValueNative(
                ref info,
                input?.ToArray(),
                (UIntPtr)(input?.Count ?? 0),
                userData,
                DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_encrypt_entry_value")]
        private static extern void MDataInfoEncryptEntryValueNative(
            ref MDataInfo info,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] input,
            UIntPtr inputLen,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> MDataInfoDecryptAsync(ref MDataInfo info, List<byte> input)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            MDataInfoDecryptNative(ref info, input?.ToArray(), (UIntPtr)(input?.Count ?? 0), userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_decrypt")]
        private static extern void MDataInfoDecryptNative(
            ref MDataInfo info,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
            byte[] input,
            UIntPtr inputLen,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task<List<byte>> MDataInfoSerialiseAsync(ref MDataInfo info)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            MDataInfoSerialiseNative(ref info, userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_serialise")]
        private static extern void MDataInfoSerialiseNative(ref MDataInfo info, IntPtr userData, FfiResultByteListCb oCb);

        public Task<MDataInfo> MDataInfoDeserialiseAsync(List<byte> encoded)
        {
            var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
            MDataInfoDeserialiseNative(encoded?.ToArray(), (UIntPtr)(encoded?.Count ?? 0), userData, DelegateOnFfiResultMDataInfoCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_info_deserialise")]
        private static extern void MDataInfoDeserialiseNative(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            byte[] encoded,
            UIntPtr encodedLen,
            IntPtr userData,
            FfiResultMDataInfoCb oCb);

        public Task MDataPutAsync(IntPtr app, ref MDataInfo info, ulong permissionsH, ulong entriesH)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataPutNative(app, ref info, permissionsH, entriesH, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_put")]
        private static extern void MDataPutNative(
            IntPtr app,
            ref MDataInfo info,
            ulong permissionsH,
            ulong entriesH,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<ulong> MDataGetVersionAsync(IntPtr app, ref MDataInfo info)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataGetVersionNative(app, ref info, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_get_version")]
        private static extern void MDataGetVersionNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

        public Task<ulong> MDataSerialisedSizeAsync(IntPtr app, ref MDataInfo info)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataSerialisedSizeNative(app, ref info, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_serialised_size")]
        private static extern void MDataSerialisedSizeNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

        public Task<(List<byte>, ulong)> MDataGetValueAsync(IntPtr app, ref MDataInfo info, List<byte> key)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(List<byte>, ulong)>();
            MDataGetValueNative(app, ref info, key?.ToArray(), (UIntPtr)(key?.Count ?? 0), userData, DelegateOnFfiResultByteListULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_get_value")]
        private static extern void MDataGetValueNative(
            IntPtr app,
            ref MDataInfo info,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] key,
            UIntPtr keyLen,
            IntPtr userData,
            FfiResultByteListULongCb oCb);

        public Task<ulong> MDataEntriesAsync(IntPtr app, ref MDataInfo info)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataEntriesNative(app, ref info, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entries")]
        private static extern void MDataEntriesNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

        public Task<List<MDataKey>> MDataListKeysAsync(IntPtr app, ref MDataInfo info)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<MDataKey>>();
            MDataListKeysNative(app, ref info, userData, DelegateOnFfiResultMDataKeyListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_list_keys")]
        private static extern void MDataListKeysNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultMDataKeyListCb oCb);

        public Task<List<MDataValue>> MDataListValuesAsync(IntPtr app, ref MDataInfo info)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<MDataValue>>();
            MDataListValuesNative(app, ref info, userData, DelegateOnFfiResultMDataValueListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_list_values")]
        private static extern void MDataListValuesNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultMDataValueListCb oCb);

        public Task MDataMutateEntriesAsync(IntPtr app, ref MDataInfo info, ulong actionsH)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataMutateEntriesNative(app, ref info, actionsH, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_mutate_entries")]
        private static extern void MDataMutateEntriesNative(
            IntPtr app,
            ref MDataInfo info,
            ulong actionsH,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<ulong> MDataListPermissionsAsync(IntPtr app, ref MDataInfo info)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataListPermissionsNative(app, ref info, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_list_permissions")]
        private static extern void MDataListPermissionsNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

        public Task<PermissionSet> MDataListUserPermissionsAsync(IntPtr app, ref MDataInfo info, ulong userH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<PermissionSet>();
            MDataListUserPermissionsNative(app, ref info, userH, userData, DelegateOnFfiResultPermissionSetCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_list_user_permissions")]
        private static extern void MDataListUserPermissionsNative(
            IntPtr app,
            ref MDataInfo info,
            ulong userH,
            IntPtr userData,
            FfiResultPermissionSetCb oCb);

        public Task MDataSetUserPermissionsAsync(
            IntPtr app,
            ref MDataInfo info,
            ulong userH,
            ref PermissionSet permissionSet,
            ulong version)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataSetUserPermissionsNative(app, ref info, userH, ref permissionSet, version, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_set_user_permissions")]
        private static extern void MDataSetUserPermissionsNative(
            IntPtr app,
            ref MDataInfo info,
            ulong userH,
            ref PermissionSet permissionSet,
            ulong version,
            IntPtr userData,
            FfiResultCb oCb);

        public Task MDataDelUserPermissionsAsync(IntPtr app, ref MDataInfo info, ulong userH, ulong version)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataDelUserPermissionsNative(app, ref info, userH, version, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_del_user_permissions")]
        private static extern void MDataDelUserPermissionsNative(
            IntPtr app,
            ref MDataInfo info,
            ulong userH,
            ulong version,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<ulong> MDataEntriesNewAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataEntriesNewNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entries_new")]
        private static extern void MDataEntriesNewNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task MDataEntriesInsertAsync(IntPtr app, ulong entriesH, List<byte> key, List<byte> value)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataEntriesInsertNative(
                app,
                entriesH,
                key?.ToArray(),
                (UIntPtr)(key?.Count ?? 0),
                value?.ToArray(),
                (UIntPtr)(value?.Count ?? 0),
                userData,
                DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entries_insert")]
        private static extern void MDataEntriesInsertNative(
            IntPtr app,
            ulong entriesH,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] key,
            UIntPtr keyLen,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] value,
            UIntPtr valueLen,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<ulong> MDataEntriesLenAsync(IntPtr app, ulong entriesH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataEntriesLenNative(app, entriesH, userData, DelegateOnFfiResultULongFromUIntPtrCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entries_len")]
        private static extern void MDataEntriesLenNative(IntPtr app, ulong entriesH, IntPtr userData, FfiResultULongFromUIntPtrCb oCb);

        public Task<(List<byte>, ulong)> MDataEntriesGetAsync(IntPtr app, ulong entriesH, List<byte> key)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(List<byte>, ulong)>();
            MDataEntriesGetNative(app, entriesH, key?.ToArray(), (UIntPtr)(key?.Count ?? 0), userData, DelegateOnFfiResultByteListULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entries_get")]
        private static extern void MDataEntriesGetNative(
            IntPtr app,
            ulong entriesH,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] key,
            UIntPtr keyLen,
            IntPtr userData,
            FfiResultByteListULongCb oCb);

        public Task<List<MDataEntry>> MDataListEntriesAsync(IntPtr app, ulong entriesH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<MDataEntry>>();
            MDataListEntriesNative(app, entriesH, userData, DelegateOnFfiResultMDataEntryListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_list_entries")]
        private static extern void MDataListEntriesNative(IntPtr app, ulong entriesH, IntPtr userData, FfiResultMDataEntryListCb oCb);

        public Task MDataEntriesFreeAsync(IntPtr app, ulong entriesH)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataEntriesFreeNative(app, entriesH, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entries_free")]
        private static extern void MDataEntriesFreeNative(IntPtr app, ulong entriesH, IntPtr userData, FfiResultCb oCb);

        public Task<ulong> MDataEntryActionsNewAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataEntryActionsNewNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entry_actions_new")]
        private static extern void MDataEntryActionsNewNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task MDataEntryActionsInsertAsync(IntPtr app, ulong actionsH, List<byte> key, List<byte> value)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataEntryActionsInsertNative(
                app,
                actionsH,
                key?.ToArray(),
                (UIntPtr)(key?.Count ?? 0),
                value?.ToArray(),
                (UIntPtr)(value?.Count ?? 0),
                userData,
                DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entry_actions_insert")]
        private static extern void MDataEntryActionsInsertNative(
            IntPtr app,
            ulong actionsH,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] key,
            UIntPtr keyLen,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] value,
            UIntPtr valueLen,
            IntPtr userData,
            FfiResultCb oCb);

        public Task MDataEntryActionsUpdateAsync(IntPtr app, ulong actionsH, List<byte> key, List<byte> value, ulong entryVersion)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataEntryActionsUpdateNative(
                app,
                actionsH,
                key?.ToArray(),
                (UIntPtr)(key?.Count ?? 0),
                value?.ToArray(),
                (UIntPtr)(value?.Count ?? 0),
                entryVersion,
                userData,
                DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entry_actions_update")]
        private static extern void MDataEntryActionsUpdateNative(
            IntPtr app,
            ulong actionsH,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] key,
            UIntPtr keyLen,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            byte[] value,
            UIntPtr valueLen,
            ulong entryVersion,
            IntPtr userData,
            FfiResultCb oCb);

        public Task MDataEntryActionsDeleteAsync(IntPtr app, ulong actionsH, List<byte> key, ulong entryVersion)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataEntryActionsDeleteNative(
                app,
                actionsH,
                key?.ToArray(),
                (UIntPtr)(key?.Count ?? 0),
                entryVersion,
                userData,
                DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entry_actions_delete")]
        private static extern void MDataEntryActionsDeleteNative(
            IntPtr app,
            ulong actionsH,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] key,
            UIntPtr keyLen,
            ulong entryVersion,
            IntPtr userData,
            FfiResultCb oCb);

        public Task MDataEntryActionsFreeAsync(IntPtr app, ulong actionsH)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataEntryActionsFreeNative(app, actionsH, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_entry_actions_free")]
        private static extern void MDataEntryActionsFreeNative(IntPtr app, ulong actionsH, IntPtr userData, FfiResultCb oCb);

        public Task<List<byte>> MDataEncodeMetadataAsync(ref MetadataResponse metadata)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            MDataEncodeMetadataNative(ref metadata, userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_encode_metadata")]
        private static extern void MDataEncodeMetadataNative(ref MetadataResponse metadata, IntPtr userData, FfiResultByteListCb oCb);

        public Task<ulong> MDataPermissionsNewAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataPermissionsNewNative(app, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_permissions_new")]
        private static extern void MDataPermissionsNewNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

        public Task<ulong> MDataPermissionsLenAsync(IntPtr app, ulong permissionsH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            MDataPermissionsLenNative(app, permissionsH, userData, DelegateOnFfiResultULongFromUIntPtrCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_permissions_len")]
        private static extern void MDataPermissionsLenNative(
            IntPtr app,
            ulong permissionsH,
            IntPtr userData,
            FfiResultULongFromUIntPtrCb oCb);

        public Task<PermissionSet> MDataPermissionsGetAsync(IntPtr app, ulong permissionsH, ulong userH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<PermissionSet>();
            MDataPermissionsGetNative(app, permissionsH, userH, userData, DelegateOnFfiResultPermissionSetCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_permissions_get")]
        private static extern void MDataPermissionsGetNative(
            IntPtr app,
            ulong permissionsH,
            ulong userH,
            IntPtr userData,
            FfiResultPermissionSetCb oCb);

        public Task<List<UserPermissionSet>> MDataListPermissionSetsAsync(IntPtr app, ulong permissionsH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<UserPermissionSet>>();
            MDataListPermissionSetsNative(app, permissionsH, userData, DelegateOnFfiResultUserPermissionSetListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_list_permission_sets")]
        private static extern void MDataListPermissionSetsNative(
            IntPtr app,
            ulong permissionsH,
            IntPtr userData,
            FfiResultUserPermissionSetListCb oCb);

        public Task MDataPermissionsInsertAsync(IntPtr app, ulong permissionsH, ulong userH, ref PermissionSet permissionSet)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataPermissionsInsertNative(app, permissionsH, userH, ref permissionSet, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_permissions_insert")]
        private static extern void MDataPermissionsInsertNative(
            IntPtr app,
            ulong permissionsH,
            ulong userH,
            ref PermissionSet permissionSet,
            IntPtr userData,
            FfiResultCb oCb);

        public Task MDataPermissionsFreeAsync(IntPtr app, ulong permissionsH)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            MDataPermissionsFreeNative(app, permissionsH, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "mdata_permissions_free")]
        private static extern void MDataPermissionsFreeNative(IntPtr app, ulong permissionsH, IntPtr userData, FfiResultCb oCb);

        public Task<(File, ulong)> DirFetchFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(File, ulong)>();
            DirFetchFileNative(app, ref parentInfo, fileName, userData, DelegateOnFfiResultFileULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "dir_fetch_file")]
        private static extern void DirFetchFileNative(
            IntPtr app,
            ref MDataInfo parentInfo,
            [MarshalAs(UnmanagedType.LPStr)] string fileName,
            IntPtr userData,
            FfiResultFileULongCb oCb);

        public Task DirInsertFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName, ref File file)
        {
            var fileNative = file.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask();
            DirInsertFileNative(app, ref parentInfo, fileName, ref fileNative, userData, DelegateOnFfiResultCb);
            fileNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "dir_insert_file")]
        private static extern void DirInsertFileNative(
            IntPtr app,
            ref MDataInfo parentInfo,
            [MarshalAs(UnmanagedType.LPStr)] string fileName,
            ref FileNative file,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<ulong> DirUpdateFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName, ref File file, ulong version)
        {
            var fileNative = file.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            DirUpdateFileNative(app, ref parentInfo, fileName, ref fileNative, version, userData, DelegateOnFfiResultULongCb);
            fileNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "dir_update_file")]
        private static extern void DirUpdateFileNative(
            IntPtr app,
            ref MDataInfo parentInfo,
            [MarshalAs(UnmanagedType.LPStr)] string fileName,
            ref FileNative file,
            ulong version,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<ulong> DirDeleteFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName, ulong version)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            DirDeleteFileNative(app, ref parentInfo, fileName, version, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "dir_delete_file")]
        private static extern void DirDeleteFileNative(
            IntPtr app,
            ref MDataInfo parentInfo,
            [MarshalAs(UnmanagedType.LPStr)] string fileName,
            ulong version,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<ulong> FileOpenAsync(IntPtr app, ref MDataInfo parentInfo, ref File file, ulong openMode)
        {
            var fileNative = file.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            FileOpenNative(app, ref parentInfo, ref fileNative, openMode, userData, DelegateOnFfiResultULongCb);
            fileNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "file_open")]
        private static extern void FileOpenNative(
            IntPtr app,
            ref MDataInfo parentInfo,
            ref FileNative file,
            ulong openMode,
            IntPtr userData,
            FfiResultULongCb oCb);

        public Task<ulong> FileSizeAsync(IntPtr app, ulong fileH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            FileSizeNative(app, fileH, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "file_size")]
        private static extern void FileSizeNative(IntPtr app, ulong fileH, IntPtr userData, FfiResultULongCb oCb);

        public Task<List<byte>> FileReadAsync(IntPtr app, ulong fileH, ulong position, ulong len)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<byte>>();
            FileReadNative(app, fileH, position, len, userData, DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "file_read")]
        private static extern void FileReadNative(
            IntPtr app,
            ulong fileH,
            ulong position,
            ulong len,
            IntPtr userData,
            FfiResultByteListCb oCb);

        public Task FileWriteAsync(IntPtr app, ulong fileH, List<byte> data)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            FileWriteNative(app, fileH, data?.ToArray(), (UIntPtr)(data?.Count ?? 0), userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "file_write")]
        private static extern void FileWriteNative(
            IntPtr app,
            ulong fileH,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            byte[] data,
            UIntPtr dataLen,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<File> FileCloseAsync(IntPtr app, ulong fileH)
        {
            var (ret, userData) = BindingUtils.PrepareTask<File>();
            FileCloseNative(app, fileH, userData, DelegateOnFfiResultFileCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "file_close")]
        private static extern void FileCloseNative(IntPtr app, ulong fileH, IntPtr userData, FfiResultFileCb oCb);

        private delegate void FfiResultAccountInfoCb(IntPtr userData, IntPtr result, IntPtr accountInfo);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultAccountInfoCb))]
#endif
        private static void OnFfiResultAccountInfoCb(IntPtr userData, IntPtr result, IntPtr accountInfo)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<AccountInfo>(accountInfo));
        }

        private static readonly FfiResultAccountInfoCb DelegateOnFfiResultAccountInfoCb = OnFfiResultAccountInfoCb;

        private delegate void FfiResultAppCb(IntPtr userData, IntPtr result, IntPtr app);

        private delegate void FfiResultByteArrayAsymNonceLenCb(IntPtr userData, IntPtr result, IntPtr nonce);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayAsymNonceLenCb))]
#endif
        private static void OnFfiResultByteArrayAsymNonceLenCb(IntPtr userData, IntPtr result, IntPtr nonce)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteArray(nonce, (int)AppConstants.AsymNonceLen));
        }

        private static readonly FfiResultByteArrayAsymNonceLenCb DelegateOnFfiResultByteArrayAsymNonceLenCb =
            OnFfiResultByteArrayAsymNonceLenCb;

        private delegate void FfiResultByteArrayAsymPublicKeyLenCb(IntPtr userData, IntPtr result, IntPtr pubEncKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayAsymPublicKeyLenCb))]
#endif
        private static void OnFfiResultByteArrayAsymPublicKeyLenCb(IntPtr userData, IntPtr result, IntPtr pubEncKey)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteArray(pubEncKey, (int)AppConstants.AsymPublicKeyLen));
        }

        private static readonly FfiResultByteArrayAsymPublicKeyLenCb DelegateOnFfiResultByteArrayAsymPublicKeyLenCb =
            OnFfiResultByteArrayAsymPublicKeyLenCb;

        private delegate void FfiResultByteArrayAsymSecretKeyLenCb(IntPtr userData, IntPtr result, IntPtr secEncKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayAsymSecretKeyLenCb))]
#endif
        private static void OnFfiResultByteArrayAsymSecretKeyLenCb(IntPtr userData, IntPtr result, IntPtr secEncKey)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteArray(secEncKey, (int)AppConstants.AsymSecretKeyLen));
        }

        private static readonly FfiResultByteArrayAsymSecretKeyLenCb DelegateOnFfiResultByteArrayAsymSecretKeyLenCb =
            OnFfiResultByteArrayAsymSecretKeyLenCb;

        private delegate void FfiResultByteArraySignPublicKeyLenCb(IntPtr userData, IntPtr result, IntPtr pubSignKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArraySignPublicKeyLenCb))]
#endif
        private static void OnFfiResultByteArraySignPublicKeyLenCb(IntPtr userData, IntPtr result, IntPtr pubSignKey)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteArray(pubSignKey, (int)AppConstants.SignPublicKeyLen));
        }

        private static readonly FfiResultByteArraySignPublicKeyLenCb DelegateOnFfiResultByteArraySignPublicKeyLenCb =
            OnFfiResultByteArraySignPublicKeyLenCb;

        private delegate void FfiResultByteArraySignSecretKeyLenCb(IntPtr userData, IntPtr result, IntPtr pubSignKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArraySignSecretKeyLenCb))]
#endif
        private static void OnFfiResultByteArraySignSecretKeyLenCb(IntPtr userData, IntPtr result, IntPtr pubSignKey)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteArray(pubSignKey, (int)AppConstants.SignSecretKeyLen));
        }

        private static readonly FfiResultByteArraySignSecretKeyLenCb DelegateOnFfiResultByteArraySignSecretKeyLenCb =
            OnFfiResultByteArraySignSecretKeyLenCb;

        private delegate void FfiResultByteArrayXorNameLenCb(IntPtr userData, IntPtr result, IntPtr name);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayXorNameLenCb))]
#endif
        private static void OnFfiResultByteArrayXorNameLenCb(IntPtr userData, IntPtr result, IntPtr name)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteArray(name, (int)AppConstants.XorNameLen));
        }

        private static readonly FfiResultByteArrayXorNameLenCb DelegateOnFfiResultByteArrayXorNameLenCb = OnFfiResultByteArrayXorNameLenCb;

        private delegate void FfiResultByteListCb(IntPtr userData, IntPtr result, IntPtr signedDataPtr, UIntPtr signedDataLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteListCb))]
#endif
        private static void OnFfiResultByteListCb(IntPtr userData, IntPtr result, IntPtr signedDataPtr, UIntPtr signedDataLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteList(signedDataPtr, (int)signedDataLen));
        }

        private static readonly FfiResultByteListCb DelegateOnFfiResultByteListCb = OnFfiResultByteListCb;

        private delegate void FfiResultByteListULongCb(
            IntPtr userData,
            IntPtr result,
            IntPtr contentPtr,
            UIntPtr contentLen,
            ulong version);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteListULongCb))]
#endif
        private static void OnFfiResultByteListULongCb(IntPtr userData, IntPtr result, IntPtr contentPtr, UIntPtr contentLen, ulong version)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (BindingUtils.CopyToByteList(contentPtr, (int)contentLen), version));
        }

        private static readonly FfiResultByteListULongCb DelegateOnFfiResultByteListULongCb = OnFfiResultByteListULongCb;

        private delegate void FfiResultCb(IntPtr userData, IntPtr result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultCb))]
#endif
        private static void OnFfiResultCb(IntPtr userData, IntPtr result)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result));
        }

        private static readonly FfiResultCb DelegateOnFfiResultCb = OnFfiResultCb;

        private delegate void FfiResultContainerPermissionsListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr containerPermsPtr,
            UIntPtr containerPermsLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultContainerPermissionsListCb))]
#endif
        private static void OnFfiResultContainerPermissionsListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr containerPermsPtr,
            UIntPtr containerPermsLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<ContainerPermissions>(containerPermsPtr, (int)containerPermsLen));
        }

        private static readonly FfiResultContainerPermissionsListCb DelegateOnFfiResultContainerPermissionsListCb =
            OnFfiResultContainerPermissionsListCb;

        private delegate void FfiResultFileCb(IntPtr userData, IntPtr result, IntPtr file);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultFileCb))]
#endif
        private static void OnFfiResultFileCb(IntPtr userData, IntPtr result, IntPtr file)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => new File(Marshal.PtrToStructure<FileNative>(file)));
        }

        private static readonly FfiResultFileCb DelegateOnFfiResultFileCb = OnFfiResultFileCb;

        private delegate void FfiResultFileULongCb(IntPtr userData, IntPtr result, IntPtr file, ulong version);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultFileULongCb))]
#endif
        private static void OnFfiResultFileULongCb(IntPtr userData, IntPtr result, IntPtr file, ulong version)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (new File(Marshal.PtrToStructure<FileNative>(file)), version));
        }

        private static readonly FfiResultFileULongCb DelegateOnFfiResultFileULongCb = OnFfiResultFileULongCb;

        private delegate void FfiResultMDataEntryListCb(IntPtr userData, IntPtr result, IntPtr entriesPtr, UIntPtr entriesLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultMDataEntryListCb))]
#endif
        private static void OnFfiResultMDataEntryListCb(IntPtr userData, IntPtr result, IntPtr entriesPtr, UIntPtr entriesLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<MDataEntryNative>(entriesPtr, (int)entriesLen).Select(native => new MDataEntry(native)).
                    ToList());
        }

        private static readonly FfiResultMDataEntryListCb DelegateOnFfiResultMDataEntryListCb = OnFfiResultMDataEntryListCb;

        private delegate void FfiResultMDataInfoCb(IntPtr userData, IntPtr result, IntPtr mdataInfo);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultMDataInfoCb))]
#endif
        private static void OnFfiResultMDataInfoCb(IntPtr userData, IntPtr result, IntPtr mdataInfo)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<MDataInfo>(mdataInfo));
        }

        private static readonly FfiResultMDataInfoCb DelegateOnFfiResultMDataInfoCb = OnFfiResultMDataInfoCb;

        private delegate void FfiResultMDataKeyListCb(IntPtr userData, IntPtr result, IntPtr keysPtr, UIntPtr keysLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultMDataKeyListCb))]
#endif
        private static void OnFfiResultMDataKeyListCb(IntPtr userData, IntPtr result, IntPtr keysPtr, UIntPtr keysLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<MDataKeyNative>(keysPtr, (int)keysLen).Select(native => new MDataKey(native)).ToList());
        }

        private static readonly FfiResultMDataKeyListCb DelegateOnFfiResultMDataKeyListCb = OnFfiResultMDataKeyListCb;

        private delegate void FfiResultMDataValueListCb(IntPtr userData, IntPtr result, IntPtr valuesPtr, UIntPtr valuesLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultMDataValueListCb))]
#endif
        private static void OnFfiResultMDataValueListCb(IntPtr userData, IntPtr result, IntPtr valuesPtr, UIntPtr valuesLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<MDataValueNative>(valuesPtr, (int)valuesLen).Select(native => new MDataValue(native)).
                    ToList());
        }

        private static readonly FfiResultMDataValueListCb DelegateOnFfiResultMDataValueListCb = OnFfiResultMDataValueListCb;

        private delegate void FfiResultPermissionSetCb(IntPtr userData, IntPtr result, IntPtr permSet);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultPermissionSetCb))]
#endif
        private static void OnFfiResultPermissionSetCb(IntPtr userData, IntPtr result, IntPtr permSet)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<PermissionSet>(permSet));
        }

        private static readonly FfiResultPermissionSetCb DelegateOnFfiResultPermissionSetCb = OnFfiResultPermissionSetCb;

        private delegate void FfiResultStringCb(IntPtr userData, IntPtr result, string logPath);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringCb))]
#endif
        private static void OnFfiResultStringCb(IntPtr userData, IntPtr result, string logPath)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => logPath);
        }

        private static readonly FfiResultStringCb DelegateOnFfiResultStringCb = OnFfiResultStringCb;

        private delegate void FfiResultUIntCb(IntPtr userData, IntPtr result, uint reqId);

        private delegate void FfiResultUIntStringCb(IntPtr userData, IntPtr result, uint reqId, string encoded);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultUIntStringCb))]
#endif
        private static void OnFfiResultUIntStringCb(IntPtr userData, IntPtr result, uint reqId, string encoded)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => (reqId, encoded));
        }

        private static readonly FfiResultUIntStringCb DelegateOnFfiResultUIntStringCb = OnFfiResultUIntStringCb;

        private delegate void FfiResultULongCb(IntPtr userData, IntPtr result, ulong handle);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongCb))]
#endif
        private static void OnFfiResultULongCb(IntPtr userData, IntPtr result, ulong handle)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => handle);
        }

        private static readonly FfiResultULongCb DelegateOnFfiResultULongCb = OnFfiResultULongCb;

        private delegate void FfiResultULongFromUIntPtrCb(IntPtr userData, IntPtr result, UIntPtr len);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongFromUIntPtrCb))]
#endif
        private static void OnFfiResultULongFromUIntPtrCb(IntPtr userData, IntPtr result, UIntPtr len)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => (ulong)len);
        }

        private static readonly FfiResultULongFromUIntPtrCb DelegateOnFfiResultULongFromUIntPtrCb = OnFfiResultULongFromUIntPtrCb;

        private delegate void FfiResultULongULongCb(IntPtr userData, IntPtr result, ulong publicKeyH, ulong secretKeyH);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongULongCb))]
#endif
        private static void OnFfiResultULongULongCb(IntPtr userData, IntPtr result, ulong publicKeyH, ulong secretKeyH)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => (publicKeyH, secretKeyH));
        }

        private static readonly FfiResultULongULongCb DelegateOnFfiResultULongULongCb = OnFfiResultULongULongCb;

        private delegate void FfiResultUserPermissionSetListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr userPermSetsPtr,
            UIntPtr userPermSetsLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultUserPermissionSetListCb))]
#endif
        private static void OnFfiResultUserPermissionSetListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr userPermSetsPtr,
            UIntPtr userPermSetsLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<UserPermissionSet>(userPermSetsPtr, (int)userPermSetsLen));
        }

        private static readonly FfiResultUserPermissionSetListCb DelegateOnFfiResultUserPermissionSetListCb =
            OnFfiResultUserPermissionSetListCb;

        private delegate void NoneCb(IntPtr userData);

        private delegate void UIntAuthGrantedCb(IntPtr userData, uint reqId, IntPtr authGranted);

        private delegate void UIntByteListCb(IntPtr userData, uint reqId, IntPtr serialisedCfgPtr, UIntPtr serialisedCfgLen);

        private delegate void UIntCb(IntPtr userData, uint reqId);
    }
}
#endif
