using System;
using System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SafeApp.Utilities
{
#pragma warning disable SA1401 // Fields should be private

    /// <summary>
    /// XorName stub.
    /// </summary>
    [PublicAPI]
    public struct XorName
    {
    }

    /// <summary>
    /// XorNameArray stub.
    /// </summary>
    [PublicAPI]
    public struct XorNameArray
    {
    }

    /// <summary>
    /// XorUrlEncoder stub.
    /// </summary>
    [PublicAPI]
    public struct XorUrlEncoder
    {
        /// <summary>
        /// Wallet, member comment stub.
        /// </summary>
        public ulong EncodingVersion { get; set; } // currently only v1 supported

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public XorName XorName { get; set; }

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public ulong TypeTag { get; set; }

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public SafeDataType DataType { get; set; }

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public SafeContentType ContentType { get; set; }

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public string[] SubNames { get; set; }

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public ulong? ContentVersion { get; set; }
    }

    /// <summary>
    /// SafeContentType stub.
    /// </summary>
    [PublicAPI]
    public enum SafeContentType
    {
        /// <summary>
        /// Member comment stub.
        /// </summary>
        Raw = 0x0000,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        Wallet = 0x0001,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        FilesContainer = 0x0002,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        NrsMapContainer = 0x0003,
    }

    /// <summary>
    /// SafeDataType stub.
    /// </summary>
    [PublicAPI]
    public enum SafeDataType
    {
        /// <summary>
        /// Member comment stub.
        /// </summary>
        SafeKey = 0x00,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        PublishedImmutableData = 0x01,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        UnpublishedImmutableData = 0x02,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        SeqMutableData = 0x03,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        UnseqMutableData = 0x04,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        PublishedSeqAppendOnlyData = 0x05,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        PublishedUnseqAppendOnlyData = 0x06,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        UnpublishedSeqAppendOnlyData = 0x07,

        /// <summary>
        /// Member comment stub.
        /// </summary>
        UnpublishedUnseqAppendOnlyData = 0x08,
    }

    /// <summary>
    /// NrsMap stub.
    /// </summary>
    [PublicAPI]
    public struct NrsMap
    {
        /// <summary>
        /// Member comment stub.
        /// </summary>
        public string SubNamesMap { get; set; }

        /// <summary>
        /// Member comment stub.
        /// </summary>
        public DefaultRDF Default { get; set; }
    }

    /// <summary>
    /// DefaultRDF stub.
    /// </summary>
    [PublicAPI]
    public enum DefaultRDF // todo: solve this (rust enum)
    {
    }

    /// <summary>
    /// SubNameRDF stub.
    /// </summary>
    [PublicAPI]
    public enum SubNameRDF // todo: solve this (rust enum)
    {
    }

    /// <summary>
    /// SubNamesMap stub.
    /// </summary>
    [PublicAPI]
    public class SubNamesMap : Dictionary<string, SubNameRDF>
    {
    }

    /// <summary>
    /// DefinitionData stub.
    /// </summary>
    [PublicAPI]
    public class DefinitionData : Dictionary<string, string>
    {
    }

    /// <summary>
    /// BlsKeyPair stub.
    /// </summary>
    [PublicAPI]
    public struct BlsKeyPair
    {
        /// <summary>
        /// Public key.
        /// </summary>
        public string PK { get; set; }

        /// <summary>
        /// Secret key.
        /// </summary>
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
