﻿using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

// ReSharper disable once CheckNamespace

namespace SafeApp.AppBindings
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial interface IAppBindings
    {
        void AppRegistered(
            string appId,
            ref AuthGranted authGranted,
            Action oDisconnectNotifierCb,
            Action<FfiResult, IntPtr, GCHandle> oCb);

        void AppUnregistered(byte[] bootstrapConfig, Action oDisconnectNotifierCb, Action<FfiResult, IntPtr, GCHandle> oCb);

        Task<IpcMsg> DecodeIpcMsgAsync(string msg);
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
