using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace SafeApp.Utilities
{
#pragma warning disable SA1401 // Fields should be private

    public struct XorName
    {
    }

    public struct XorNameArray
    {
    }

    public struct XorUrlEncoder
    {
        public ulong EncodingVersion { get; set; } // currently only v1 supported

        public XorName XorName { get; set; }

        public ulong TypeTag { get; set; }

        public SafeDataType DataType { get; set; }

        public SafeContentType ContentType { get; set; }

        public string Path { get; set; }

        public string[] SubNames { get; set; }

        public ulong? ContentVersion { get; set; }
    }

    public enum SafeContentType
    {
        Raw = 0x0000,
        Wallet = 0x0001,
        FilesContainer = 0x0002,
        NrsMapContainer = 0x0003,
    }

    public enum SafeDataType
    {
        SafeKey = 0x00,
        PublishedImmutableData = 0x01,
        UnpublishedImmutableData = 0x02,
        SeqMutableData = 0x03,
        UnseqMutableData = 0x04,
        PublishedSeqAppendOnlyData = 0x05,
        PublishedUnseqAppendOnlyData = 0x06,
        UnpublishedSeqAppendOnlyData = 0x07,
        UnpublishedUnseqAppendOnlyData = 0x08,
    }

    // -------------

    public struct NrsMap
    {
        public string SubNamesMap { get; set; }

        public DefaultRDF Default { get; set; }
    }

    public enum DefaultRDF // todo: solve this (rust enum)
    {
    }

    public enum SubNameRDF // todo: solve this (rust enum)
    {
    }

    public class SubNamesMap : Dictionary<string, SubNameRDF>
    {
    }

    public class DefinitionData : Dictionary<string, string>
    {
    }

    public struct BlsKeyPair
    {
        public string PK { get; set; }

        public string SK { get; set; }
    }

    /// <summary>
    /// Base IPC response message.
    /// </summary>
    [PublicAPI]
    public abstract class IpcMsg
    {
    }

    /// <summary>
    /// Authentication IPC response message.
    /// </summary>
    [PublicAPI]
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
    [PublicAPI]
    public class UnregisteredIpcMsg : IpcMsg
    {
        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Serialised configuration.
        /// </summary>
        public List<byte> SerialisedCfg;

        /// <summary>
        /// Initialise IPC response message.
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
    /// Containers permission IPC response message.
    /// </summary>
    [PublicAPI]
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
    [PublicAPI]
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
    [PublicAPI]
    public class RevokedIpcMsg : IpcMsg
    {
    }

    /// <summary>
    /// IPC response message exception
    /// </summary>
    [PublicAPI]
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
