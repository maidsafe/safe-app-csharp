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

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

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

        public Task<(uint, string)> EncodeUnregisteredReqAsync(byte[] extraData)
        {
            var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
            EncodeUnregisteredReqNative(extraData?.ToArray(), (UIntPtr)(extraData?.Length ?? 0), userData, DelegateOnFfiResultUIntStringCb);
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

        private delegate void FfiResultCb(IntPtr userData, IntPtr result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultCb))]
#endif
        private static void OnFfiResultCb(IntPtr userData, IntPtr result)
        {
            BindingUtils.CompleteTask(userData, Marshal.PtrToStructure<FfiResult>(result));
        }

        private static readonly FfiResultCb DelegateOnFfiResultCb = OnFfiResultCb;

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

        private delegate void NoneCb(IntPtr userData);

        private delegate void UIntAuthGrantedCb(IntPtr userData, uint reqId, IntPtr authGranted);

        private delegate void UIntByteListCb(IntPtr userData, uint reqId, IntPtr serialisedCfgPtr, UIntPtr serialisedCfgLen);

        private delegate void UIntCb(IntPtr userData, uint reqId);
    }
}
#endif
