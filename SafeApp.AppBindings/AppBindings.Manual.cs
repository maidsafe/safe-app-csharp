using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

#if __IOS__
using ObjCRuntime;
#endif

namespace SafeApp.AppBindings
{
    internal partial class AppBindings
    {
        #region Decode IPC Msg
        public Task<IpcMsg> DecodeIpcMsgAsync(string msg)
        {
            var (task, userData) = BindingUtils.PrepareTask<IpcMsg>();
            DecodeIpcMsgNative(
              msg,
              userData,
              DelegateOnDecodeIpcMsgAuthCb,
              DelegateOnDecodeIpcMsgUnregisteredCb,
              DelegateOnDecodeIpcMsgContainersCb,
              DelegateOnDecodeIpcMsgShareMdataCb,
              DelegateOnDecodeIpcMsgRevokedCb,
              DelegateOnDecodeIpcMsgErrCb);

            return task;
        }

        #if __IOS__
        [MonoPInvokeCallback(typeof(UIntAuthGrantedCb))]
        #endif
        private static void OnDecodeIpcMsgAuthCb(IntPtr userData, uint reqId, IntPtr authGranted)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcMsg>>(userData);
            tcs.SetResult(new AuthIpcMsg(reqId, new AuthGranted(Marshal.PtrToStructure<AuthGrantedNative>(authGranted))));
        }

        private static readonly UIntAuthGrantedCb DelegateOnDecodeIpcMsgAuthCb = OnDecodeIpcMsgAuthCb;

        #if __IOS__
        [MonoPInvokeCallback(typeof(UIntByteListCb))]
        #endif
        private static void OnDecodeIpcMsgUnregisteredCb(IntPtr userData, uint reqId, IntPtr serialisedCfgPtr, UIntPtr serialisedCfgLen)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcMsg>>(userData);
            tcs.SetResult(new UnregisteredIpcMsg(reqId, serialisedCfgPtr, serialisedCfgLen));
        }

        private static readonly UIntByteListCb DelegateOnDecodeIpcMsgUnregisteredCb = OnDecodeIpcMsgUnregisteredCb;

        #if __IOS__
        [MonoPInvokeCallback(typeof(UIntCb))]
        #endif
        private static void OnDecodeIpcMsgContainersCb(IntPtr userData, uint reqId)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcMsg>>(userData);
            tcs.SetResult(new ContainersIpcMsg(reqId));
        }

        private static readonly UIntCb DelegateOnDecodeIpcMsgContainersCb = OnDecodeIpcMsgContainersCb;

        #if __IOS__
        [MonoPInvokeCallback(typeof(UIntCb))]
        #endif
        private static void OnDecodeIpcMsgShareMdataCb(IntPtr userData, uint reqId)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcMsg>>(userData);
            tcs.SetResult(new ShareMDataIpcMsg(reqId));
        }

        private static readonly UIntCb DelegateOnDecodeIpcMsgShareMdataCb = OnDecodeIpcMsgShareMdataCb;

        #if __IOS__
        [MonoPInvokeCallback(typeof(NoneCb))]
        #endif
        private static void OnDecodeIpcMsgRevokedCb(IntPtr userData)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcMsg>>(userData);
            tcs.SetResult(new RevokedIpcMsg());
        }

        private static readonly NoneCb DelegateOnDecodeIpcMsgRevokedCb = OnDecodeIpcMsgRevokedCb;

        #if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultUIntCb))]
        #endif
        private static void OnDecodeIpcMsgErrCb(IntPtr userData, IntPtr result, uint reqId)
        {
            var res = Marshal.PtrToStructure<FfiResult>(result);
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcMsg>>(userData);
            tcs.SetException(new IpcMsgException(reqId, res.ErrorCode, res.Description));
        }

        private static readonly FfiResultUIntCb DelegateOnDecodeIpcMsgErrCb = OnDecodeIpcMsgErrCb;

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
              DelegateOnFfiResultPublicImmutableDataCb,
              DelegateOnFfiResultSequenceDataCb,
              DelegateOnFfiResultWalletCb,
              DelegateOnFfiResultSafeKeyCb,
              DelegateOnFfiResultFilesContainerCb,
              DelegateOnFfiResultNrsContainerCb,
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
            FfiResultPublicImmutableDataCb oPublished,
            FfiResultSequenceDataCb osequence,
            FfiResultWalletCb oWallet,
            FfiResultSafeKeyCb oKeys,
            FfiResultFilesContainerCb oContainer,
            FfiResultNrsContainerCb nNrsContainer,
            FfiFetchFailedCb oErr);

        public Task<string> InspectAsync(IntPtr app, string url)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            InspectNative(app, url, userData, DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "inspect")]
        private static extern void InspectNative(
            IntPtr app,
            [MarshalAs(UnmanagedType.LPStr)] string url,
            IntPtr userData,
            FfiResultStringCb oCb);

        private delegate void FfiResultPublicImmutableDataCb(IntPtr userData, IntPtr publicImmutableData);

        #if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultPublicImmutableDataCb))]
        #endif
        private static void OnFfiResultPublicImmutableDataCb(IntPtr userData, IntPtr publicImmutableData)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new PublicImmutableData(Marshal.PtrToStructure<PublicImmutableDataNative>(publicImmutableData)));
        }

        private static readonly FfiResultPublicImmutableDataCb DelegateOnFfiResultPublicImmutableDataCb = OnFfiResultPublicImmutableDataCb;

        private delegate void FfiResultSequenceDataCb(IntPtr userData, IntPtr sequenceData);

        #if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultSequenceDataCb))]
        #endif
        private static void OnFfiResultSequenceDataCb(IntPtr userData, IntPtr sequenceData)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new SequenceData(Marshal.PtrToStructure<SequenceDataNative>(sequenceData)));
        }

        private static readonly FfiResultSequenceDataCb DelegateOnFfiResultSequenceDataCb = OnFfiResultSequenceDataCb;

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
            tcs.SetResult(Marshal.PtrToStructure<SafeKey>(safeKey));
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

        private delegate void FfiResultNrsContainerCb(IntPtr userData, IntPtr nrsContainer);

        #if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultNrsContainerCb))]
        #endif
        private static void OnFfiResultNrsContainerCb(IntPtr userData, IntPtr nrsContainer)
        {
            var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<ISafeData>>(userData);
            tcs.SetResult(new NrsMapContainer(Marshal.PtrToStructure<NrsMapContainerNative>(nrsContainer)));
        }

        private static readonly FfiResultNrsContainerCb DelegateOnFfiResultNrsContainerCb = OnFfiResultNrsContainerCb;

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
    }
}
