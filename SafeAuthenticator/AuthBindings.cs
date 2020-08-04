#if __IOS__
using ObjCRuntime;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

namespace SafeAuthenticator
{
    internal partial class AuthBindings : IAuthBindings
    {
#if __IOS__
        private const string DllName = "__Internal";
#else
        private const string DllName = "safe_api";
#endif

        public bool AuthIsMock()
        {
            var ret = AuthIsMockNative();
            return ret;
        }

        [DllImport(DllName, EntryPoint = "auth_is_mock")]
        private static extern bool AuthIsMockNative();

        public Task<(string, BlsKeyPair)> AllocateTestCoinsAsync(string preload)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(string, BlsKeyPair)>();
            AllocateTestCoinsNative(preload, userData, DelegateOnFfiResultStringBlsKeyPairCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "allocate_test_coins")]
        private static extern void AllocateTestCoinsNative([MarshalAs(UnmanagedType.LPStr)] string preload, IntPtr userData, FfiResultStringBlsKeyPairCb oCb);

        [DllImport(DllName, EntryPoint = "log_in")]
        private static extern void LogInNative([MarshalAs(UnmanagedType.LPStr)] string passphrase, [MarshalAs(UnmanagedType.LPStr)] string password, IntPtr userData, FfiResultSafeAuthenticatorCb oCb);

        public Task LogOutAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            LogOutNative(app, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "log_out")]
        private static extern void LogOutNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

        public Task<bool> IsLoggedInAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<bool>();
            IsLoggedInNative(app, userData, DelegateOnFfiResultBoolCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "is_logged_in")]
        private static extern void IsLoggedInNative(IntPtr app, IntPtr userData, FfiResultBoolCb oCb);

        [DllImport(DllName, EntryPoint = "create_acc")]
        private static extern void CreateAccNative(
            [MarshalAs(UnmanagedType.LPStr)] string secretKey,
            [MarshalAs(UnmanagedType.LPStr)] string passphrase,
            [MarshalAs(UnmanagedType.LPStr)] string password,
            IntPtr userData,
            FfiResultSafeAuthenticatorCb oCb);

        public Task<string> AutheriseAppAsync(IntPtr app, string request, bool isGranted)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AutheriseAppNative(app, request, isGranted, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "authorise_app")]
        private static extern void AutheriseAppNative(IntPtr app, [MarshalAs(UnmanagedType.LPStr)] string request, [MarshalAs(UnmanagedType.U1)] bool isGranted, IntPtr userData, FfiResultStringCb oCb);

        public Task RevokeAppAsync(IntPtr app, string appId)
        {
            var (ret, userData) = BindingUtils.PrepareTask();
            RevokeAppNative(app, appId, userData, DelegateOnFfiResultCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "revoke_app")]
        private static extern void RevokeAppNative(IntPtr app, [MarshalAs(UnmanagedType.LPStr)] string appId, IntPtr userData, FfiResultCb oCb);

        public Task<List<AuthedApp>> AuthdAppAsync(IntPtr app)
        {
            var (ret, userData) = BindingUtils.PrepareTask<List<AuthedApp>>();
            AuthdAppNative(app, userData, DelegateOnFfiResultAuthedAppListCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "authd_app")]
        private static extern void AuthdAppNative(IntPtr app, IntPtr userData, FfiResultAuthedAppListCb oCb);

        [DllImport(DllName, EntryPoint = "decode_req")]
        private static extern void DecodeReqNative(IntPtr app, [MarshalAs(UnmanagedType.LPStr)] string msg, IntPtr userData, UIntAuthReqCb oAuth, UIntContainersReqCb oContainers, UIntByteListCb oUnregistered, FfiResultCb oErr);

        [DllImport(DllName, EntryPoint = "decode_auth_unregistered_req")]
        private static extern void DecodeAuthUnregisteredReqNative([MarshalAs(UnmanagedType.LPStr)] string msg, IntPtr userData, UIntByteListCb oUnregistered, FfiResultCb oErr);

        public Task<string> AutheriseUnregisteredAppAsync(uint reqId, bool isGranted)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            AutheriseUnregisteredAppNative(reqId, isGranted, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "encode_connection_info")]
        private static extern void AutheriseUnregisteredAppNative(uint reqId, [MarshalAs(UnmanagedType.U1)] bool isGranted, IntPtr userData, FfiResultStringCb oCb);

        private delegate void FfiResultCb(IntPtr userData, IntPtr result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultCb))]
#endif
        private static void OnFfiResultCb(IntPtr userData, IntPtr result)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result));
        }

        private static readonly FfiResultCb DelegateOnFfiResultCb = OnFfiResultCb;

        private delegate void FfiResultAuthedAppListCb(IntPtr userData, IntPtr result, IntPtr appsPtr, UIntPtr appsLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultAuthedAppListCb))]
#endif
        private static void OnFfiResultAuthedAppListCb(IntPtr userData, IntPtr result, IntPtr appsPtr, UIntPtr appsLen)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => BindingUtils.CopyToObjectList<AuthedAppNative>(appsPtr, (int)appsLen).Select(native => new AuthedApp(native)).ToList());
        }

        private static readonly FfiResultAuthedAppListCb DelegateOnFfiResultAuthedAppListCb = OnFfiResultAuthedAppListCb;

        private delegate void FfiResultStringCb(IntPtr userData, IntPtr result, string authResponse);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringCb))]
#endif
        private static void OnFfiResultStringCb(IntPtr userData, IntPtr result, string authResponse)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => authResponse);
        }

        private static readonly FfiResultStringCb DelegateOnFfiResultStringCb = OnFfiResultStringCb;

        private delegate void FfiResultBoolCb(IntPtr userData, IntPtr result, bool loggedIn);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultBoolCb))]
#endif
        private static void OnFfiResultBoolCb(IntPtr userData, IntPtr result, bool loggedIn)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => loggedIn);
        }

        private delegate void FfiResultStringBlsKeyPairCb(IntPtr userData, IntPtr result, string xorurl, IntPtr safeKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringBlsKeyPairCb))]
#endif
        private static void OnFfiResultStringBlsKeyPairCb(IntPtr userData, IntPtr result, string xorurl, IntPtr safeKey)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result), () => (xorurl, Marshal.PtrToStructure<BlsKeyPair>(safeKey)));
        }

        private static readonly FfiResultStringBlsKeyPairCb DelegateOnFfiResultStringBlsKeyPairCb = OnFfiResultStringBlsKeyPairCb;

        private delegate void NoneCb(IntPtr userData);

        private static readonly FfiResultBoolCb DelegateOnFfiResultBoolCb = OnFfiResultBoolCb;

        private delegate void UIntAuthReqCb(IntPtr userData, uint reqId, IntPtr req);

        private delegate void UIntByteListCb(IntPtr userData, uint reqId, IntPtr serialisedCfgPtr, UIntPtr serialisedCfgLen);

        private delegate void UIntCb(IntPtr userData, uint reqId);

        private delegate void UIntContainersReqCb(IntPtr userData, uint reqId, IntPtr req);
    }
}
