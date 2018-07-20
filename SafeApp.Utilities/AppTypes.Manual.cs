﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace SafeApp.Utilities
{
#pragma warning disable SA1401 // Fields should be private
    /// <summary>
    /// IPC message
    /// </summary>
    [PublicAPI]
    public abstract class IpcMsg
    {
    }

    /// <summary>
    /// Authentication IPC message
    /// </summary>
    [PublicAPI]
    public class AuthIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Authentication response.
        /// </summary>
        public AuthGranted AuthGranted;

        /// <summary>
        /// Initialize AuthIPC message.
        /// </summary>
        /// <param name="reqId">Request id.</param>
        /// <param name="authGranted">Authentication response.</param>
        public AuthIpcMsg(uint reqId, AuthGranted authGranted)
        {
            ReqId = reqId;
            AuthGranted = authGranted;
        }
    }

    /// <summary>
    /// Unregistered IPC message
    /// </summary>
    [PublicAPI]
    public class UnregisteredIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Serialised configuration.
        /// </summary>
        public List<byte> SerialisedCfg;

        /// <summary>
        /// Initialize IPCmsg.
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="serialisedCfgPtr"></param>
        /// <param name="serialisedCfgLen"></param>
        public UnregisteredIpcMsg(uint reqId, IntPtr serialisedCfgPtr, UIntPtr serialisedCfgLen)
        {
            ReqId = reqId;
            SerialisedCfg = BindingUtils.CopyToByteList(serialisedCfgPtr, (int)serialisedCfgLen);
        }
    }

    /// <summary>
    /// Containers IPC message
    /// </summary>
    [PublicAPI]
    public class ContainersIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialize Containters IPCMsg.
        /// </summary>
        /// <param name="reqId"></param>
        public ContainersIpcMsg(uint reqId)
        {
            ReqId = reqId;
        }
    }

    /// <summary>
    /// MData share IPC message
    /// </summary>
    [PublicAPI]
    public class ShareMDataIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialize ShareMData IPCMsg.
        /// </summary>
        /// <param name="reqId"></param>
        public ShareMDataIpcMsg(uint reqId)
        {
            ReqId = reqId;
        }
    }

    /// <summary>
    /// Revoke IPC message
    /// </summary>
    [PublicAPI]
    public class RevokedIpcMsg : IpcMsg
    {
    }

    /// <summary>
    /// IPC message exception
    /// </summary>
    [PublicAPI]
    public class IpcMsgException : FfiException
    {
        /// <summary>
        /// Request id.
        /// </summary>
        public readonly uint ReqId;

        /// <summary>
        /// Initialize IPCMsg exception.
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="code"></param>
        /// <param name="description"></param>
        public IpcMsgException(uint reqId, int code, string description)
            : base(code, description)
        {
            ReqId = reqId;
        }
    }
#pragma warning restore SA1401 // Fields should be private
}
