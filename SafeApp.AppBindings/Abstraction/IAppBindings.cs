﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

// ReSharper disable once CheckNamespace

namespace SafeApp.AppBindings
{
    // ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial interface IAppBindings
    {
        #region Low Level
        Task<string> AppExeFileStemAsync();

        Task AppInitLoggingAsync(string outputFileNameOverride);

        Task<string> AppOutputLogPathAsync(string outputFileName);

        Task AppSetAdditionalSearchPathAsync(string newPath);

        Task<(uint, string)> EncodeAuthReqAsync(ref AuthReq req);

        Task<(uint, string)> EncodeContainersReqAsync(ref ContainersReq req);

        Task<(uint, string)> EncodeShareMDataReqAsync(ref ShareMDataReq req);

        Task<(uint, string)> EncodeUnregisteredReqAsync(byte[] extraData);

        bool IsMockBuild();

        #endregion

        #region High Level
        void Connect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb);
        #endregion

        #region XorEncoder

        Task<string> XorurlEncodeAsync(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding);

        Task<XorUrlEncoder> XorurlEncoderAsync(
            byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion);

        Task<XorUrlEncoder> XorurlEncoderFromUrlAsync(string xorUrl);

        #endregion

        #region Fetch

        Task<ISafeData> FetchAsync(IntPtr app, string uri);

        #endregion

        #region Files

        Task<(string, ProcessedFiles, string)> FilesContainerCreateAsync(
            IntPtr app,
            string location,
            string dest,
            bool recursive,
            bool dryRun);

        Task<(ulong, string)> FilesContainerGetAsync(IntPtr app, string url);

        Task<(ulong, ProcessedFiles, string)> FilesContainerSyncAsync(
            IntPtr app,
            string location,
            string url,
            bool recursive,
            bool delete,
            bool updateNrs,
            bool dryRun);

        Task<(ulong, ProcessedFiles, string)> FilesContainerAddAsync(
            IntPtr app,
            string sourceFile,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun);

        Task<(ulong, ProcessedFiles, string)> FilesContainerAddFromRawAsync(
            IntPtr app,
            byte[] data,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun);

        Task<string> FilesPutPublishedImmutableAsync(IntPtr app, byte[] data, string mediaType);

        Task<byte[]> FilesGetPublishedImmutableAsync(IntPtr app, string url);

        #endregion Files

        #region Keys

        Task<BlsKeyPair> GenerateKeyPairAsync(IntPtr app);

        Task<(string, BlsKeyPair)> CreateKeysAsync(IntPtr app, string from, string preloadAmount, string pk);

        Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(IntPtr app, string preloadAmount);

        Task<string> KeysBalanceFromSkAsync(IntPtr app, string sk);

        Task<string> KeysBalanceFromUrlAsync(IntPtr app, string url, string sk);

        Task<string> ValidateSkForUrlAsync(IntPtr app, string sk, string url);

        Task<ulong> KeysTransferAsync(IntPtr app, string amount, string fromSk, string toUrl, ulong txId);

        #endregion Keys

        #region Wallet

        Task<string> WalletCreateAsync(IntPtr app);

        Task<string> WalletInsertAsync(IntPtr app, string keyUrl, string name, bool setDefault, string secretKey);

        Task<string> WalletBalanceAsync(IntPtr app, string url);

        Task<(WalletSpendableBalance, ulong)> WalletGetDefaultBalanceAsync(IntPtr app, string url);

        Task<ulong> WalletTransferAsync(IntPtr app, string from, string to, string amount, ulong id);

        Task<WalletSpendableBalances> WalletGetAsync(IntPtr app, string url);

        #endregion Wallet
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
