#if !NETSTANDARD1_2 || __DESKTOP__
#if __IOS__
using ObjCRuntime;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

namespace SafeApp.MockAuthBindings
{
    internal partial class AuthBindings : IAuthBindings
    {
#if __IOS__
        private const string DllName = "__Internal";
#else
        private const string DllName = "safe_app";
#endif

        public bool IsMockBuild()
        {
            var ret = AuthIsMockNative();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_is_mock")]
        private static extern bool AuthIsMockNative();

        [DllImport(DllName, EntryPoint = "create_acc")]
        private static extern void CreateAccNative(
            [MarshalAs(UnmanagedType.LPStr)] string accountLocator,
            [MarshalAs(UnmanagedType.LPStr)] string accountPassword,
            IntPtr userData,
            NoneCb oDisconnectNotifierCb,
            FfiResultAuthenticatorCb oCb);

        [DllImport(DllName, EntryPoint = "login")]
        private static extern void LoginNative(
            [MarshalAs(UnmanagedType.LPStr)] string accountLocator,
            [MarshalAs(UnmanagedType.LPStr)] string accountPassword,
            IntPtr userData,
            NoneCb oDisconnectNotifierCb,
            FfiResultAuthenticatorCb oCb);

        public Task AuthReconnectAsync(IntPtr auth)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AuthReconnectNative(auth, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_reconnect")]
        private static extern void AuthReconnectNative(IntPtr auth, IntPtr userData, FfiResultCb oCb);

        public Task<string> AuthExeFileStemAsync()
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AuthExeFileStemNative(userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_exe_file_stem")]
        private static extern void AuthExeFileStemNative(IntPtr userData, FfiResultStringCb oCb);

        public Task AuthSetAdditionalSearchPathAsync(string newPath)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AuthSetAdditionalSearchPathNative(newPath, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_set_additional_search_path")]
        private static extern void AuthSetAdditionalSearchPathNative(
            [MarshalAs(UnmanagedType.LPStr)] string newPath,
            IntPtr userData,
            FfiResultCb oCb);

        public void AuthFree(IntPtr auth)
        {
            AuthFreeNative(auth);
        }

        [DllImport(DllName, EntryPoint = "auth_free")]
        private static extern void AuthFreeNative(IntPtr auth);

        public Task AuthRmRevokedAppAsync(IntPtr auth, string appId)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AuthRmRevokedAppNative(auth, appId, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_rm_revoked_app")]
        private static extern void AuthRmRevokedAppNative(
            IntPtr auth,
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<List<AppExchangeInfo>> AuthRevokedAppsAsync(IntPtr auth)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<AppExchangeInfo>>();
            AuthRevokedAppsNative(auth, userData, DelegateOnFfiResultAppExchangeInfoListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_revoked_apps")]
        private static extern void AuthRevokedAppsNative(IntPtr auth, IntPtr userData, FfiResultAppExchangeInfoListCb oCb);

        public Task<List<RegisteredApp>> AuthRegisteredAppsAsync(IntPtr auth)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<RegisteredApp>>();
            AuthRegisteredAppsNative(auth, userData, DelegateOnFfiResultRegisteredAppListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_registered_apps")]
        private static extern void AuthRegisteredAppsNative(IntPtr auth, IntPtr userData, FfiResultRegisteredAppListCb oCb);

        public Task<List<AppAccess>> AuthAppsAccessingMutableDataAsync(IntPtr auth, byte[] mdName, ulong mdTypeTag)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<AppAccess>>();
            AuthAppsAccessingMutableDataNative(auth, mdName, mdTypeTag, userData, DelegateOnFfiResultAppAccessListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_apps_accessing_mutable_data")]
        private static extern void AuthAppsAccessingMutableDataNative(
            IntPtr auth,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = (int)AppConstants.XorNameLen)]
            byte[] mdName,
            ulong mdTypeTag,
            IntPtr userData,
            FfiResultAppAccessListCb oCb);

        [DllImport(DllName, EntryPoint = "auth_unregistered_decode_ipc_msg")]
        private static extern void AuthUnregisteredDecodeIpcMsgNative(
            [MarshalAs(UnmanagedType.LPStr)] string msg,
            IntPtr userData,
            UIntByteListCb oUnregistered,
            FfiResultStringCb oErr);

        [DllImport(DllName, EntryPoint = "auth_decode_ipc_msg")]
        private static extern void AuthDecodeIpcMsgNative(
            IntPtr auth,
            [MarshalAs(UnmanagedType.LPStr)] string msg,
            IntPtr userData,
            UIntAuthReqCb oAuth,
            UIntContainersReqCb oContainers,
            UIntByteListCb oUnregistered,
            UIntShareMDataReqMetadataResponseListCb oShareMData,
            FfiResultStringCb oErr);

        public Task<string> EncodeShareMDataRespAsync(IntPtr auth, ref ShareMDataReq req, uint reqId, bool isGranted)
        {
            var reqNative = req.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            EncodeShareMDataRespNative(auth, ref reqNative, reqId, isGranted, userData, DelegateOnFfiResultStringCb);
            reqNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_share_mdata_resp")]
        private static extern void EncodeShareMDataRespNative(
            IntPtr auth,
            ref ShareMDataReqNative req,
            uint reqId,
            [MarshalAs(UnmanagedType.U1)] bool isGranted,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> AuthRevokeAppAsync(IntPtr auth, string appId)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AuthRevokeAppNative(auth, appId, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_revoke_app")]
        private static extern void AuthRevokeAppNative(
            IntPtr auth,
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task AuthFlushAppRevocationQueueAsync(IntPtr auth)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AuthFlushAppRevocationQueueNative(auth, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_flush_app_revocation_queue")]
        private static extern void AuthFlushAppRevocationQueueNative(IntPtr auth, IntPtr userData, FfiResultCb oCb);

        public Task<string> EncodeUnregisteredRespAsync(uint reqId, bool isGranted)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            EncodeUnregisteredRespNative(reqId, isGranted, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_unregistered_resp")]
        private static extern void EncodeUnregisteredRespNative(
            uint reqId,
            [MarshalAs(UnmanagedType.U1)] bool isGranted,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> EncodeAuthRespAsync(IntPtr auth, ref AuthReq req, uint reqId, bool isGranted)
        {
            var reqNative = req.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            EncodeAuthRespNative(auth, ref reqNative, reqId, isGranted, userData, DelegateOnFfiResultStringCb);
            reqNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_auth_resp")]
        private static extern void EncodeAuthRespNative(
            IntPtr auth,
            ref AuthReqNative req,
            uint reqId,
            [MarshalAs(UnmanagedType.U1)] bool isGranted,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<string> EncodeContainersRespAsync(IntPtr auth, ref ContainersReq req, uint reqId, bool isGranted)
        {
            var reqNative = req.ToNative();
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            EncodeContainersRespNative(auth, ref reqNative, reqId, isGranted, userData, DelegateOnFfiResultStringCb);
            reqNative.Free();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_containers_resp")]
        private static extern void EncodeContainersRespNative(
            IntPtr auth,
            ref ContainersReqNative req,
            uint reqId,
            [MarshalAs(UnmanagedType.U1)] bool isGranted,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task AuthInitLoggingAsync(string outputFileNameOverride)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            AuthInitLoggingNative(outputFileNameOverride, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_init_logging")]
        private static extern void AuthInitLoggingNative(
            [MarshalAs(UnmanagedType.LPStr)] string outputFileNameOverride,
            IntPtr userData,
            FfiResultCb oCb);

        public Task<string> AuthOutputLogPathAsync(string outputFileName)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AuthOutputLogPathNative(outputFileName, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_output_log_path")]
        private static extern void AuthOutputLogPathNative(
            [MarshalAs(UnmanagedType.LPStr)] string outputFileName,
            IntPtr userData,
            FfiResultStringCb oCb);

        private delegate void FfiResultAppAccessListCb(IntPtr userData, IntPtr result, IntPtr appAccessPtr, UIntPtr appAccessLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultAppAccessListCb))]
#endif
        private static void OnFfiResultAppAccessListCb(IntPtr userData, IntPtr result, IntPtr appAccessPtr, UIntPtr appAccessLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<AppAccess>(appAccessPtr, (int)appAccessLen));
        }

        private static readonly FfiResultAppAccessListCb DelegateOnFfiResultAppAccessListCb = OnFfiResultAppAccessListCb;

        private delegate void FfiResultAppExchangeInfoListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr appExchangeInfoPtr,
            UIntPtr appExchangeInfoLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultAppExchangeInfoListCb))]
#endif
        private static void OnFfiResultAppExchangeInfoListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr appExchangeInfoPtr,
            UIntPtr appExchangeInfoLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<AppExchangeInfo>(appExchangeInfoPtr, (int)appExchangeInfoLen));
        }

        private static readonly FfiResultAppExchangeInfoListCb DelegateOnFfiResultAppExchangeInfoListCb = OnFfiResultAppExchangeInfoListCb;

        private delegate void FfiResultAuthenticatorCb(IntPtr userData, IntPtr result, IntPtr authenticator);

        private delegate void FfiResultCb(IntPtr userData, IntPtr result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultCb))]
#endif
        private static void OnFfiResultCb(IntPtr userData, IntPtr result)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result));
        }

        private static readonly FfiResultCb DelegateOnFfiResultCb = OnFfiResultCb;

        private delegate void FfiResultRegisteredAppListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr registeredAppPtr,
            UIntPtr registeredAppLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultRegisteredAppListCb))]
#endif
        private static void OnFfiResultRegisteredAppListCb(
            IntPtr userData,
            IntPtr result,
            IntPtr registeredAppPtr,
            UIntPtr registeredAppLen)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => BindingUtils.CopyToObjectList<RegisteredAppNative>(registeredAppPtr, (int)registeredAppLen).
                    Select(native => new RegisteredApp(native)).ToList());
        }

        private static readonly FfiResultRegisteredAppListCb DelegateOnFfiResultRegisteredAppListCb = OnFfiResultRegisteredAppListCb;

        private delegate void FfiResultStringCb(IntPtr userData, IntPtr result, string response);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringCb))]
#endif
        private static void OnFfiResultStringCb(IntPtr userData, IntPtr result, string response)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => response);
        }

        private static readonly FfiResultStringCb DelegateOnFfiResultStringCb = OnFfiResultStringCb;

        public Task TestSimulateNetworkDisconnectAsync(IntPtr authPtr)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            TestSimulateNetworkDisconnectNative(authPtr, userData, OnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "test_simulate_network_disconnect")]
        private static extern void TestSimulateNetworkDisconnectNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

        private delegate void NoneCb(IntPtr userData);

        private delegate void UIntAuthReqCb(IntPtr userData, uint reqId, IntPtr req);

        private delegate void UIntByteListCb(IntPtr userData, uint reqId, IntPtr extraDataPtr, UIntPtr extraDataLen);

        private delegate void UIntContainersReqCb(IntPtr userData, uint reqId, IntPtr req);

        private delegate void UIntShareMDataReqMetadataResponseListCb(
            IntPtr userData,
            uint reqId,
            IntPtr req,
            IntPtr metadataPtr,
            UIntPtr metadataLen);
    }
}
#endif
