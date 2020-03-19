using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

#if __IOS__
using ObjCRuntime;
#endif

namespace SafeApp.AppBindings
{
    internal partial class AppBindings : IAppBindings
    {
        #region Authenticate

        public Task<string> AuthAppAsync(
            string appId,
            string appName,
            string appVendor,
            string endpoint)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AuthAppNative(appId, appName, appVendor, endpoint, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_app")]
        private static extern void AuthAppNative(
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            [MarshalAs(UnmanagedType.LPStr)] string appName,
            [MarshalAs(UnmanagedType.LPStr)] string appVendor,
            [MarshalAs(UnmanagedType.LPStr)] string endpoint,
            IntPtr userData,
            FfiResultStringCb oCb);

        #endregion

        #region Connect
        public void ConnectApp(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb)
        {
            var userData = BindingUtils.ToHandlePtr(oCb);
            ConnectAppNative(appId, authCredentials, userData, DelegateOnFfiResultSafeCb);
        }

        [DllImport(DllName, EntryPoint = "connect_app")]
        private static extern void ConnectAppNative(
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            [MarshalAs(UnmanagedType.LPStr)] string authCredentials,
            IntPtr userData,
            FfiResultSafeCb oCb);

        private delegate void FfiResultSafeCb(IntPtr userData, IntPtr result, IntPtr app);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultSafeCb))]
#endif
        private static void OnFfiResultSafeCb(IntPtr userData, IntPtr result, IntPtr app)
        {
            var action = BindingUtils.FromHandlePtr<Action<FfiResult, IntPtr, GCHandle>>(userData, false);
            action(Marshal.PtrToStructure<FfiResult>(result), app, GCHandle.FromIntPtr(userData));
        }

        private static readonly FfiResultSafeCb DelegateOnFfiResultSafeCb = OnFfiResultSafeCb;

        #endregion

        #region XorUrl
        public Task<string> XorurlEncodeAsync(
            byte[] name,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            XorurlEncodeNative(
                name,
                typeTag,
                (ulong)dataType,
                (ushort)contentType,
                path,
                subNames,
                contentVersion,
                baseEncoding,
                userData,
                DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encode")]
        private static extern void XorurlEncodeNative(
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.XorNameLen)] byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string subNames,
            ulong contentVersion,
            [MarshalAs(UnmanagedType.LPStr)] string baseEncoding,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<XorUrlEncoder> XorurlEncoderAsync(
            byte[] name,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            string subNames,
            ulong contentVersion)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            XorurlEncoderNative(
                name,
                typeTag,
                (ulong)dataType,
                (ushort)contentType,
                path,
                subNames,
                contentVersion,
                userData,
                DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encoder")]
        private static extern void XorurlEncoderNative(
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.XorNameLen)] byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string subNames,
            ulong contentVersion,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        public Task<XorUrlEncoder> XorurlEncoderFromUrlAsync(string xorUrl)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            XorurlEncoderFromUrlNative(xorUrl, userData, DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encoder_from_url")]
        private static extern void XorurlEncoderFromUrlNative(
            [MarshalAs(UnmanagedType.LPStr)] string xorUrl,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        private delegate void FfiResultXorUrlEncoderCb(IntPtr userData, IntPtr result, IntPtr xorurlEncoder);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultXorUrlEncoderCb))]
#endif
        private static void OnFfiResultXorUrlEncoderCb(IntPtr userData, IntPtr result, IntPtr xorurlEncoder)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => new XorUrlEncoder(Marshal.PtrToStructure<XorUrlEncoderNative>(xorurlEncoder)));
        }

        private static readonly FfiResultXorUrlEncoderCb DelegateOnFfiResultXorUrlEncoderCb = OnFfiResultXorUrlEncoderCb;

        #endregion

        #region Fetch

        public Task<ISafeData> FetchAsync(IntPtr app, string url, ulong start, ulong end)
        {
            var (task, userData) = BindingUtils.PrepareTask<ISafeData>();
            FetchNative(
              app,
              url,
              userData,
              start,
              end,
              DelegateOnFfiResultPublishedImmutableDataCb,
              DelegateOnFfiResultWalletCb,
              DelegateOnFfiResultSafeKeyCb,
              DelegateOnFfiResultFilesContainerCb,
              DelegateOnFfiFetchFailedCb);
            return task;
        }

        [DllImport(DllName, EntryPoint = "fetch")]
        private static extern void FetchNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            ulong start,
            ulong end,
            FfiResultPublishedImmutableDataCb oPublished,
            FfiResultWalletCb oWallet,
            FfiResultSafeKeyCb oKeys,
            FfiResultFilesContainerCb oContainer,
            FfiFetchFailedCb oErr);

        public Task<ISafeData> InspectAsync(IntPtr app, string url)
        {
            var (task, userData) = BindingUtils.PrepareTask<ISafeData>();
            InspectNative(
                app,
                url,
                userData,
                DelegateOnFfiResultPublishedImmutableDataCb,
                DelegateOnFfiResultWalletCb,
                DelegateOnFfiResultSafeKeyCb,
                DelegateOnFfiResultFilesContainerCb,
                DelegateOnFfiFetchFailedCb);
            return task;
        }

        [DllImport(DllName, EntryPoint = "inspect")]
        private static extern void InspectNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultPublishedImmutableDataCb oPublished,
            FfiResultWalletCb oWallet,
            FfiResultSafeKeyCb oKeys,
            FfiResultFilesContainerCb oContainer,
            FfiFetchFailedCb oErr);

        private delegate void FfiResultPublishedImmutableDataCb(IntPtr userData, IntPtr publishedImmutableData);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultPublishedImmutableDataCb))]
#endif
        private static void OnFfiResultPublishedImmutableDataCb(IntPtr userData, IntPtr publishedImmutableData)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new PublishedImmutableData(Marshal.PtrToStructure<PublishedImmutableDataNative>(publishedImmutableData)));
        }

        private static readonly FfiResultPublishedImmutableDataCb DelegateOnFfiResultPublishedImmutableDataCb = OnFfiResultPublishedImmutableDataCb;

        private delegate void FfiResultWalletCb(IntPtr userData, IntPtr wallet);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultWalletCb))]
#endif
        private static void OnFfiResultWalletCb(IntPtr userData, IntPtr wallet)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new Wallet(Marshal.PtrToStructure<WalletNative>(wallet)));
        }

        private static readonly FfiResultWalletCb DelegateOnFfiResultWalletCb = OnFfiResultWalletCb;

        private delegate void FfiResultSafeKeyCb(IntPtr userData, IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultSafeKeyCb))]
#endif
        private static void OnFfiResultSafeKeyCb(IntPtr userData, IntPtr safeKey)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new SafeKey(Marshal.PtrToStructure<SafeKeyNative>(safeKey)));
        }

        private static readonly FfiResultSafeKeyCb DelegateOnFfiResultSafeKeyCb = OnFfiResultSafeKeyCb;

        private delegate void FfiResultFilesContainerCb(IntPtr userData, IntPtr filesContainer);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultFilesContainerCb))]
#endif
        private static void OnFfiResultFilesContainerCb(IntPtr userData, IntPtr filesContainer)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new FilesContainer(Marshal.PtrToStructure<FilesContainerNative>(filesContainer)));
        }

        private static readonly FfiResultFilesContainerCb DelegateOnFfiResultFilesContainerCb = OnFfiResultFilesContainerCb;

        private delegate void FfiFetchFailedCb(IntPtr userData, IntPtr result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiFetchFailedCb))]
#endif
        private static void OnFfiFetchFailedCb(IntPtr userData, IntPtr result)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            var ffiResult = Marshal.PtrToStructure<FfiResult>(result);
            tcs.SetResult(new SafeDataFetchFailed(ffiResult.ErrorCode, ffiResult.Description));
        }

        private static readonly FfiFetchFailedCb DelegateOnFfiFetchFailedCb = OnFfiFetchFailedCb;

        #endregion Connect

        #region Keys

        public Task<BlsKeyPair> GenerateKeyPairAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<BlsKeyPair>();
            GenerateKeyPairNative(app, userData, DelegateOnFfiResultBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "generate_keypair")]
        private static extern void GenerateKeyPairNative(
            IntPtr app,
            IntPtr userData,
            FfiResultBlsKeyPairCb oCb);

        private delegate void FfiResultBlsKeyPairCb(IntPtr userData, IntPtr result, IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultBlsKeyPairCb))]
#endif
        private static void OnFfiResultBlsKeyPairCb(IntPtr userData, IntPtr result, IntPtr safeKey)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<BlsKeyPair>(safeKey));

        private static readonly FfiResultBlsKeyPairCb DelegateOnFfiResultBlsKeyPairCb = OnFfiResultBlsKeyPairCb;

        public Task<(string, BlsKeyPair)> CreateKeysAsync(
            IntPtr app,
            string from,
            string preloadAmount,
            string pk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, BlsKeyPair)>();
            CreateKeysNative(app, from, preloadAmount, pk, userData, DelegateOnFfiResultStringBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_create")]
        private static extern void CreateKeysNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string from,
            [MarshalAs(UnmanagedType.LPStr)] string preload,
            [MarshalAs(UnmanagedType.LPStr)] string pk,
            IntPtr userData,
            FfiResultStringBlsKeyPairCb oCb);

        public Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(IntPtr app, string preloadAmount)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, BlsKeyPair)>();
            KeysCreatePreloadTestCoinsNative(app, preloadAmount, userData, DelegateOnFfiResultStringBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_create_preload_test_coins")]
        private static extern void KeysCreatePreloadTestCoinsNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string preload,
            IntPtr userData,
            FfiResultStringBlsKeyPairCb oCb);

        private delegate void FfiResultStringBlsKeyPairCb(
            IntPtr userData,
            IntPtr result,
            string xorUrl,
            IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringBlsKeyPairCb))]
#endif
        private static void OnFfiResultStringBlsKeyPairCb(
            IntPtr userData,
            IntPtr result,
            string xorUrl,
            IntPtr safeKey)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (xorUrl, Marshal.PtrToStructure<BlsKeyPair>(safeKey)));

        private static readonly FfiResultStringBlsKeyPairCb DelegateOnFfiResultStringBlsKeyPairCb = OnFfiResultStringBlsKeyPairCb;

        public Task<string> KeysBalanceFromSkAsync(IntPtr app, string sk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            KeysBalanceFromSkNative(app, sk, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_balance_from_sk")]
        private static extern void KeysBalanceFromSkNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> KeysBalanceFromUrlAsync(IntPtr app, string url, string sk)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            KeysBalanceFromUrlNative(app, url, sk, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_balance_from_url")]
        private static extern void KeysBalanceFromUrlNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> ValidateSkForUrlAsync(IntPtr app, string sk, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            ValidateSkForUrlNative(app, sk, url, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "validate_sk_for_url")]
        private static extern void ValidateSkForUrlNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string sk,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<ulong> KeysTransferAsync(IntPtr app, string amount, string fromSk, string toUrl, ulong txId)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            KeysTransferNative(app, amount, fromSk, toUrl, txId, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "keys_transfer")]
        private static extern void KeysTransferNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string amount,
            [MarshalAs(UnmanagedType.LPStr)] string from,
            [MarshalAs(UnmanagedType.LPStr)] string to,
            ulong id,
            IntPtr userData,
            FfiResultULongCb oCb);

        private delegate void FfiResultULongCb(IntPtr userData, IntPtr result, ulong handle);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongCb))]
#endif
        private static void OnFfiResultULongCb(IntPtr userData, IntPtr result, ulong handle)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => handle);
        }

        private static readonly FfiResultULongCb DelegateOnFfiResultULongCb = OnFfiResultULongCb;

        #endregion Keys

        #region Wallet

        public Task<string> WalletCreateAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            WalletCreateNative(app, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "wallet_create")]
        private static extern void WalletCreateNative(
           IntPtr app,
           IntPtr userData,
           FfiResultStringCb oCb);

        public Task<string> WalletInsertAsync(IntPtr app, string keyUrl, string name, bool setDefault, string secretKey)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            WalletInsertNative(app, keyUrl, name, setDefault, secretKey, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "wallet_insert")]
        private static extern void WalletInsertNative(
           IntPtr app,
           [MarshalAs(UnmanagedType.LPStr)] string keyUrl,
           [MarshalAs(UnmanagedType.LPStr)] string name,
           [MarshalAs(UnmanagedType.U1)] bool setDefault,
           [MarshalAs(UnmanagedType.LPStr)] string secretKey,
           IntPtr userData,
           FfiResultStringCb oCb);

        public Task<string> WalletBalanceAsync(IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            WalletBalanceNative(app, url, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "wallet_balance")]
        private static extern void WalletBalanceNative(
           IntPtr app,
           [MarshalAs(UnmanagedType.LPStr)] string url,
           IntPtr userData,
           FfiResultStringCb oCb);

        public Task<(WalletSpendableBalance, ulong)> WalletGetDefaultBalanceAsync(IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(WalletSpendableBalance, ulong)>();
            WalletGetDefaultBalanceNative(app, url, userData, DelegateOnFfiResultWalletSpendableBalanceULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "wallet_get_default_balance")]
        private static extern void WalletGetDefaultBalanceNative(
           IntPtr app,
           [MarshalAs(UnmanagedType.LPStr)] string url,
           IntPtr userData,
           FfiResultWalletSpendableBalanceULongCb oCb);

        private delegate void FfiResultWalletSpendableBalanceULongCb(
           IntPtr userData,
           IntPtr result,
           IntPtr spendableWalletBalance,
           ulong version);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultWalletSpendableBalanceULongCb))]
#endif
        private static void OnFfiResultWalletSpendableBalanceULongCb(
           IntPtr userData,
           IntPtr result,
           IntPtr spendableWalletBalance,
           ulong version)
           => BindingUtils.CompleteTask(
               userData,
               Marshal.PtrToStructure<FfiResult>(result),
               () => (Marshal.PtrToStructure<WalletSpendableBalance>(spendableWalletBalance), version));

        private static readonly FfiResultWalletSpendableBalanceULongCb DelegateOnFfiResultWalletSpendableBalanceULongCb = OnFfiResultWalletSpendableBalanceULongCb;

        public Task<ulong> WalletTransferAsync(IntPtr app, string from, string to, string amount, ulong id)
        {
            var (ret, userData) = BindingUtils.PrepareTask<ulong>();
            WalletTransferNative(app, from, to, amount, id, userData, DelegateOnFfiResultULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "wallet_transfer")]
        private static extern void WalletTransferNative(
           IntPtr app,
           [MarshalAs(UnmanagedType.LPStr)] string from,
           [MarshalAs(UnmanagedType.LPStr)] string to,
           [MarshalAs(UnmanagedType.LPStr)] string amount,
           ulong id,
           IntPtr userData,
           FfiResultULongCb oCb);

        public Task<WalletSpendableBalances> WalletGetAsync(IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<WalletSpendableBalances>();
            WalletGetNative(app, url, userData, DelegateOnFfiResultWalletSpendableBalancesCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "wallet_get")]
        private static extern void WalletGetNative(
           IntPtr app,
           [MarshalAs(UnmanagedType.LPStr)] string url,
           IntPtr userData,
           FfiResultWalletSpendableBalancesCb oCb);

        private delegate void FfiResultWalletSpendableBalancesCb(
           IntPtr userData,
           IntPtr result,
           IntPtr spendableWalletBalance);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultWalletSpendableBalancesCb))]
#endif
        private static void OnFfiResultWalletSpendableBalancesCb(
           IntPtr userData,
           IntPtr result,
           IntPtr spendableWalletBalance)
           => BindingUtils.CompleteTask(
               userData,
               Marshal.PtrToStructure<FfiResult>(result),
               () => new WalletSpendableBalances(Marshal.PtrToStructure<WalletSpendableBalancesNative>(spendableWalletBalance)));

        private static readonly FfiResultWalletSpendableBalancesCb DelegateOnFfiResultWalletSpendableBalancesCb = OnFfiResultWalletSpendableBalancesCb;

        #endregion Wallet

        #region Files

        public Task<(string, ProcessedFiles, string)> FilesContainerCreateAsync(
            IntPtr app,
            string location,
            string dest,
            bool recursive,
            bool dryRun)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, ProcessedFiles, string)>();
            FilesContainerCreateNative(
                app,
                location,
                dest,
                recursive,
                dryRun,
                userData,
                DelegateOnFfiResultStringProcessedFilesStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "files_container_create")]
        private static extern void FilesContainerCreateNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string location,
            [MarshalAs(UnmanagedType.LPStr)] string dest,
            [MarshalAs(UnmanagedType.U1)] bool recursive,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            IntPtr userData,
            FfiResultStringProcessedFilesStringCb oCb);

        private delegate void FfiResultStringProcessedFilesStringCb(
            IntPtr userData,
            IntPtr result,
            string xorurl,
            IntPtr processFiles,
            string filesMap);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringProcessedFilesStringCb))]
#endif
        private static void OnFfiResultStringProcessedFilesStringCb(
            IntPtr userData,
            IntPtr result,
            string xorurl,
            IntPtr processFiles,
            string filesMap)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (
                xorurl,
                new ProcessedFiles(
                    Marshal.PtrToStructure<ProcessedFilesNative>(processFiles)), filesMap));

        private static readonly FfiResultStringProcessedFilesStringCb
                                      DelegateOnFfiResultStringProcessedFilesStringCb =
                                                    OnFfiResultStringProcessedFilesStringCb;

        public Task<(ulong, string)> FilesContainerGetAsync(IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(ulong, string)>();
            FilesContainerGetNative(app, url, userData, DelegateOnFfiResultULongStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "files_container_get")]
        private static extern void FilesContainerGetNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultULongStringCb oCb);

        private delegate void FfiResultULongStringCb(
            IntPtr userData,
            IntPtr result,
            ulong version,
            string filesMap);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongStringCb))]
#endif
        private static void OnFfiResultULongStringCb(
            IntPtr userData,
            IntPtr result,
            ulong version,
            string filesMap)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (version, filesMap));

        private static readonly FfiResultULongStringCb
                                          DelegateOnFfiResultULongStringCb =
                                                                 OnFfiResultULongStringCb;

        public Task<(ulong, ProcessedFiles, string)> FilesContainerSyncAsync(
            IntPtr app,
            string location,
            string url,
            bool recursive,
            bool delete,
            bool updateNrs,
            bool dryRun)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(ulong, ProcessedFiles, string)>();
            FilesContainerSyncNative(
                app,
                location,
                url,
                recursive,
                delete,
                updateNrs,
                dryRun,
                userData,
                DelegateOnFfiResultULongProcessedFilesStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "files_container_sync")]
        private static extern void FilesContainerSyncNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string location,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            [MarshalAs(UnmanagedType.U1)] bool recursive,
            [MarshalAs(UnmanagedType.U1)] bool delete,
            [MarshalAs(UnmanagedType.U1)] bool updateNrs,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            IntPtr userData,
            FfiResultULongProcessedFilesStringCb oCb);

        private delegate void FfiResultULongProcessedFilesStringCb(
            IntPtr userData,
            IntPtr result,
            ulong version,
            IntPtr processFiles,
            string filesMap);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongProcessedFilesStringCb))]
#endif
        private static void OnFfiResultULongProcessedFilesStringCb(
            IntPtr userData,
            IntPtr result,
            ulong version,
            IntPtr processFiles,
            string filesMap)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (
                version,
                new ProcessedFiles(
                    Marshal.PtrToStructure<ProcessedFilesNative>(processFiles)), filesMap));

        private static readonly FfiResultULongProcessedFilesStringCb
                                     DelegateOnFfiResultULongProcessedFilesStringCb =
                                                         OnFfiResultULongProcessedFilesStringCb;

        public Task<(ulong, ProcessedFiles, string)> FilesContainerAddAsync(
            IntPtr app,
            string sourceFile,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(ulong, ProcessedFiles, string)>();
            FilesContainerAddNative(
                app,
                sourceFile,
                url,
                force,
                updateNrs,
                dryRun,
                userData,
                DelegateOnFfiResultULongProcessedFilesStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "files_container_add")]
        private static extern void FilesContainerAddNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string sourceFile,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            [MarshalAs(UnmanagedType.U1)] bool force,
            [MarshalAs(UnmanagedType.U1)] bool updateNrs,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            IntPtr userData,
            FfiResultULongProcessedFilesStringCb oCb);

        public Task<(ulong, ProcessedFiles, string)> FilesContainerAddFromRawAsync(
            IntPtr app,
            byte[] data,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(ulong, ProcessedFiles, string)>();
            FilesContainerAddFromRawNative(
                app,
                data,
                (UIntPtr)data.Length,
                url,
                force,
                updateNrs,
                dryRun,
                userData,
                DelegateOnFfiResultULongProcessedFilesStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "files_container_add_from_raw")]
        private static extern void FilesContainerAddFromRawNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] data,
            UIntPtr dataLen,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            [MarshalAs(UnmanagedType.U1)] bool force,
            [MarshalAs(UnmanagedType.U1)] bool updateNrs,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            IntPtr userData,
            FfiResultULongProcessedFilesStringCb oCb);

        public Task<string> FilesPutPublishedImmutableAsync(
            IntPtr app,
            byte[] data,
            string mediaType,
            bool dryRun)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            FilesPutPublishedImmutableNative(
                app,
                data,
                (UIntPtr)data.Length,
                mediaType,
                dryRun,
                userData,
                DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "files_put_published_immutable")]
        private static extern void FilesPutPublishedImmutableNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] data,
            UIntPtr dataLen,
            [MarshalAs(UnmanagedType.LPStr)] string mediaType,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<byte[]> FilesGetPublishedImmutableAsync(IntPtr app, string url, ulong start, ulong end)
        {
            var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
            FilesGetPublishedImmutableNative(
                app,
                url,
                start,
                end,
                userData,
                DelegateOnFfiResultByteListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "files_get_published_immutable")]
        private static extern void FilesGetPublishedImmutableNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            ulong start,
            ulong end,
            IntPtr userData,
            FfiResultByteListCb oCb);

        private delegate void FfiResultByteListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr imDataPtr,
            UIntPtr imDataLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteListCb))]
#endif
        private static void OnFfiResultByteListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr imDataPtr,
            UIntPtr imDataLen)
            => BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToByteArray(imDataPtr, (int)imDataLen));

        private static readonly FfiResultByteListCb DelegateOnFfiResultByteListCb =
                                                                   OnFfiResultByteListCb;

        #endregion Files

        #region NRS

        public Task<XorUrlEncoder> ParseUrlAsync(string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            ParseUrlNative(url, userData, DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "parse_url")]
        private static extern void ParseUrlNative(
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        public Task<(XorUrlEncoder, XorUrlEncoder)> ParseAndResolveUrlAsync(IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(XorUrlEncoder, XorUrlEncoder)>();
            ParseAndResolveUrlNative(app, url, userData, DelegateOnFfiResultXorUrlEncoderXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "parse_and_resolve_url")]
        private static extern void ParseAndResolveUrlNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultXorUrlEncoderXorUrlEncoderCb oCb);

        private delegate void FfiResultXorUrlEncoderXorUrlEncoderCb(
            IntPtr userData,
            IntPtr result,
            IntPtr xorUrlEncoder,
            IntPtr resolvedFrom);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultXorUrlEncoderXorUrlEncoderCb))]
#endif
        private static void OnFfiResultXorUrlEncoderXorUrlEncoderCb(IntPtr userData, IntPtr result, IntPtr xorUrlEncoder, IntPtr resolvedFrom)
        {
            var resolved = resolvedFrom == IntPtr.Zero ?
                default :
                new XorUrlEncoder(Marshal.PtrToStructure<XorUrlEncoderNative>(resolvedFrom));

            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (
                    new XorUrlEncoder(Marshal.PtrToStructure<XorUrlEncoderNative>(xorUrlEncoder)),
                    resolved));
        }

        private static readonly FfiResultXorUrlEncoderXorUrlEncoderCb DelegateOnFfiResultXorUrlEncoderXorUrlEncoderCb =
            OnFfiResultXorUrlEncoderXorUrlEncoderCb;

        public Task<(string, ProcessedEntries, string)> CreateNrsMapContainerAsync(
            IntPtr app,
            string name,
            string link,
            bool directLink,
            bool dryRun,
            bool setDefault)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, ProcessedEntries, string)>();
            CreateNrsMapContainerNative(
                app,
                name,
                link,
                directLink,
                dryRun,
                setDefault,
                userData,
                DelegateOnFfiResultStringProcessedEntriesStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "nrs_map_container_create")]
        private static extern void CreateNrsMapContainerNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string link,
            [MarshalAs(UnmanagedType.U1)] bool directLink,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            [MarshalAs(UnmanagedType.U1)] bool setDefault,
            IntPtr userData,
            FfiResultStringProcessedEntriesStringCb oCb);

        private delegate void FfiResultStringProcessedEntriesStringCb(
            IntPtr userData,
            IntPtr result,
            string nrsMap,
            IntPtr processedEntries,
            string xorurl);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringProcessedEntriesStringCb))]
#endif
        private static void OnFfiResultStringProcessedEntriesStringCb(
            IntPtr userData,
            IntPtr result,
            string nrsMap,
            IntPtr processedEntries,
            string xorurl)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (
                nrsMap,
                new ProcessedEntries(Marshal.PtrToStructure<ProcessedEntriesNative>(processedEntries)), xorurl));
        }

        private static readonly FfiResultStringProcessedEntriesStringCb DelegateOnFfiResultStringProcessedEntriesStringCb =
            OnFfiResultStringProcessedEntriesStringCb;

        public Task<(string, string, ulong)> AddToNrsMapContainerAsync(
            IntPtr app,
            string name,
            string link,
            bool setDefault,
            bool directLink,
            bool dryRun)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, string, ulong)>();
            AddToNrsMapContainerNative(
                app,
                name,
                link,
                setDefault,
                directLink,
                dryRun,
                userData,
                DelegateOnFfiResultStringStringULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "nrs_map_container_add")]
        private static extern void AddToNrsMapContainerNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string link,
            [MarshalAs(UnmanagedType.U1)] bool setDefault,
            [MarshalAs(UnmanagedType.U1)] bool directLink,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            IntPtr userData,
            FfiResultStringStringULongCb oCb);

        private delegate void FfiResultStringStringULongCb(
            IntPtr userData,
            IntPtr result,
            string nrsMap,
            string xorurl,
            ulong version);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringStringULongCb))]
#endif
        private static void OnFfiResultStringStringULongCb(
            IntPtr userData,
            IntPtr result,
            string nrsMap,
            string xorurl,
            ulong version)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => (nrsMap, xorurl, version));
        }

        private static readonly FfiResultStringStringULongCb DelegateOnFfiResultStringStringULongCb =
            OnFfiResultStringStringULongCb;

        public Task<(string, string, ulong)> RemoveFromNrsMapContainerAsync(
            IntPtr app,
            string name,
            bool dryRun)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, string, ulong)>();
            RemoveFromNrsMapContainerNative(
                app,
                name,
                dryRun,
                userData,
                DelegateOnFfiResultStringStringULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "nrs_map_container_remove")]
        private static extern void RemoveFromNrsMapContainerNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.U1)] bool dryRun,
            IntPtr userData,
            FfiResultStringStringULongCb oCb);

        public Task<(string, ulong)> GetNrsMapContainerAsync(
            IntPtr app,
            string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, ulong)>();
            GetNrsMapContainerNative(
                app,
                url,
                userData,
                DelegateOnFfiResultStringULongCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "nrs_map_container_get")]
        private static extern void GetNrsMapContainerNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultStringULongCb oCb);

        private delegate void FfiResultStringULongCb(IntPtr userData, IntPtr result, string nrsMap, ulong version);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringULongCb))]
#endif
        private static void OnFfiResultStringULongCb(IntPtr userData, IntPtr result, string nrsMap, ulong version)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => (nrsMap, version));
        }

        private static readonly FfiResultStringULongCb DelegateOnFfiResultStringULongCb = OnFfiResultStringULongCb;

        #endregion NRS
    }
}
