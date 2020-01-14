using System;
using System.Collections.Generic;
using SafeApp.Core;

#pragma warning disable SA1401 // Fields should be private
namespace SafeApp.MockAuthBindings
{
    /// <summary>
    /// Base IPC request
    /// </summary>
    public abstract class IpcReq
    {
    }

    /// <summary>
    /// Authentication IPC Request
    /// </summary>
    public class AuthIpcReq : IpcReq
    {
        /// <summary>
        /// Authentication request.
        /// </summary>
        public AuthReq AuthReq;

        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialise Authentication IPC request.
        /// </summary>
        /// <param name="reqId">Request Id.</param>
        /// <param name="authReq">Authentication request.</param>
        public AuthIpcReq(uint reqId, AuthReq authReq)
        {
            ReqId = reqId;
            AuthReq = authReq;
        }
    }

    /// <summary>
    /// Unregistered IPC request.
    /// </summary>
    public class UnregisteredIpcReq : IpcReq
    {
        /// <summary>
        /// Extra arbitrary data.
        /// </summary>
        public byte[] ExtraData;

        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialises an Unregistered request instance.
        /// </summary>
        /// <param name="reqId">Request Id.</param>
        /// <param name="extraDataPtr">Extra arbitrary data.</param>
        /// <param name="extraDataLength">Extra data length.</param>
        public UnregisteredIpcReq(uint reqId, IntPtr extraDataPtr, ulong extraDataLength)
        {
            ReqId = reqId;
            ExtraData = BindingUtils.CopyToByteArray(extraDataPtr, (int)extraDataLength);
        }
    }

    /// <summary>
    /// Containers IPC request.
    /// </summary>
    public class ContainersIpcReq : IpcReq
    {
        /// <summary>
        /// Container request.
        /// </summary>
        public ContainersReq ContainersReq;

        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialises a Containers request instance.
        /// </summary>
        /// <param name="reqId">Request Id.</param>
        /// <param name="containersReq">Containers request instance.</param>
        public ContainersIpcReq(uint reqId, ContainersReq containersReq)
        {
            ReqId = reqId;
            ContainersReq = containersReq;
        }
    }

    /// <summary>
    /// Share Mutable Data IPC request.
    /// </summary>
    public class ShareMDataIpcReq : IpcReq
    {
        /// <summary>
        /// Metadata response.
        /// </summary>
        public List<MetadataResponse> MetadataResponse;

        /// <summary>
        /// Request Id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Share Mutable Data request.
        /// </summary>
        public ShareMDataReq ShareMDataReq;

        /// <summary>
        /// Initialises a ShareMDataReq instance.
        /// </summary>
        /// <param name="reqId">Request Id.</param>
        /// <param name="shareMDataReq">Share Mutable Data request.</param>
        /// <param name="metadataResponseList">Mutable Data MetaDataResonse list.</param>
        public ShareMDataIpcReq(uint reqId, ShareMDataReq shareMDataReq, List<MetadataResponse> metadataResponseList)
        {
            ReqId = reqId;
            ShareMDataReq = shareMDataReq;
            MetadataResponse = metadataResponseList;
        }
    }

    /// <summary>
    /// IPC request rejected.
    /// </summary>
    public class IpcReqRejected : IpcReq
    {
        /// <summary>
        /// Message string.
        /// </summary>
        public readonly string Msg;

        /// <summary>
        /// Initialise a rejected IPC Request.
        /// </summary>
        /// <param name="msg">Error message.</param>
        public IpcReqRejected(string msg)
        {
            Msg = msg;
        }
    }

    /// <summary>
    /// IPC Request Error.
    /// </summary>
    public class IpcReqError : IpcReq
    {
        /// <summary>
        /// Error code is represented by negative value.
        /// </summary>
        public readonly int Code;

        /// <summary>
        /// Error description.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Error message.
        /// </summary>
        public readonly string Msg;

        /// <summary>
        /// Initialises an IpcReqError object.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="description">Error description.</param>
        /// <param name="msg">Error message.</param>
        public IpcReqError(int code, string description, string msg)
        {
            Code = code;
            Description = description;
            Msg = msg;
        }
    }
}
#pragma warning restore SA1401 // Fields should be private
