#if !NETSTANDARD1_2 || __DESKTOP__

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using SafeApp.Core;

#if __IOS__
using ObjCRuntime;
#endif

// ReSharper disable UnusedMember.Global

namespace SafeApp.AppBindings
{
    internal partial class AppBindings
    {
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
    }
}
#endif
