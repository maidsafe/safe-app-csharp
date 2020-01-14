using System;

namespace SafeApp.Core
{
#pragma warning disable SA1401 // Fields should be private

    /// <summary>
    /// Base IPC response message.
    /// </summary>
    public abstract class IpcMsg
    {
    }

    /// <summary>
    /// Authentication IPC response message.
    /// </summary>
    public class AuthIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Authentication response.
        /// </summary>
        public AuthGranted AuthGranted;

        /// <summary>
        /// Initialise AuthIPC message.
        /// </summary>
        /// <param name="reqId">Request Id.</param>
        /// <param name="authGranted">Authentication response.</param>
        public AuthIpcMsg(uint reqId, AuthGranted authGranted)
        {
            ReqId = reqId;
            AuthGranted = authGranted;
        }
    }

    /// <summary>
    /// Unregistered access IPC response message.
    /// </summary>
    public class UnregisteredIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Serialised configuration.
        /// </summary>
        public byte[] SerialisedCfg;

        /// <summary>
        /// Initialise IPC response message.
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="serialisedCfgPtr"></param>
        /// <param name="serialisedCfgLen"></param>
        public UnregisteredIpcMsg(uint reqId, IntPtr serialisedCfgPtr, UIntPtr serialisedCfgLen)
        {
            ReqId = reqId;
            SerialisedCfg = BindingUtils.CopyToByteArray(serialisedCfgPtr, (int)serialisedCfgLen);
        }
    }

    /// <summary>
    /// Containers permission IPC response message.
    /// </summary>
    public class ContainersIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialise Containers permission IPC response message.
        /// </summary>
        /// <param name="reqId"></param>
        public ContainersIpcMsg(uint reqId)
        {
            ReqId = reqId;
        }
    }

    /// <summary>
    /// MData share IPC response message.
    /// </summary>
    public class ShareMDataIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialise ShareMData IPC response message.
        /// </summary>
        /// <param name="reqId"></param>
        public ShareMDataIpcMsg(uint reqId)
        {
            ReqId = reqId;
        }
    }

    /// <summary>
    /// Revoke IPC response message.
    /// </summary>
    public class RevokedIpcMsg : IpcMsg
    {
    }

    /// <summary>
    /// IPC response message exception
    /// </summary>
    public class IpcMsgException : FfiException
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public readonly uint ReqId;

        /// <summary>
        /// Initialise IPCMsg exception.
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
