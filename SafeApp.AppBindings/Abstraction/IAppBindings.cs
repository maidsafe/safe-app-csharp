using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

namespace SafeApp.AppBindings
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial interface IAppBindings
    {
        #region App Level
        bool AppIsMock();

        Task AppSetConfigDirPathAsync(string newPath);

        #endregion

        #region Logging

        Task AppInitLoggingAsync(string outputFileNameOverride);

        Task<string> AppConfigDirPathAsync();

        #endregion

        #region IPC

        Task<(uint, string)> EncodeAuthReqAsync(ref AuthReq req);

        Task<(uint, string)> EncodeContainersReqAsync(ref ContainersReq req);

        Task<(uint, string)> EncodeShareMDataReqAsync(ref ShareMDataReq req);

        Task<(uint, string)> EncodeUnregisteredReqAsync(byte[] extraData);

        Task<IpcMsg> DecodeIpcMsgAsync(string msg);

        #endregion

        #region High Level

        Task<string> AuthAppAsync(string appId, string appName, string appVendor, string endpoint);

        void ConnectApp(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb);
        #endregion

        #region SafeUrl

        Task<string> SafeUrlEncodeAsync(
            byte[] name,
            string nrsName,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            List<string> subNames,
            string queryString,
            string fragment,
            ulong contentVersion,
            SafeUrlBase baseEncoding);

        Task<SafeUrl> NewSafeUrlAsync(
            byte[] name,
            string nrsName,
            ulong typeTag,
            DataType dataType,
            ContentType contentType,
            string path,
            List<string> subNames,
            string queryString,
            string fragment,
            ulong contentVersion);

        Task<SafeUrl> SafeUrlFromUrlAsync(string safeUrl);

        Task<string> EncodeSafekeyAsync(byte[] name, SafeUrlBase baseEncoding);

        Task<string> EncodeImmutableDataAsync(
            byte[] name,
            ContentType contentType,
            SafeUrlBase baseEncoding);

        Task<string> EncodeMutableDataAsync(
            byte[] name,
            ulong typeTag,
            ContentType contentType,
            SafeUrlBase baseEncoding);

        Task<string> EncodeSequenceDataAsync(
            byte[] name,
            ulong typeTag,
            ContentType contentType,
            SafeUrlBase baseEncoding,
            bool isPrivate);

        #endregion

        #region Fetch

        Task<ISafeData> FetchAsync(IntPtr app, string url, ulong start, ulong end);

        Task<string> InspectAsync(IntPtr app, string uri);

        #endregion

        #region Files

        Task<(string, ProcessedFiles, FilesMap)> FilesContainerCreateAsync(
            IntPtr app,
            string location,
            string dest,
            bool recursive,
            bool followLinks,
            bool dryRun);

        Task<(ulong, FilesMap)> FilesContainerGetAsync(IntPtr app, string url);

        Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerSyncAsync(
            IntPtr app,
            string location,
            string url,
            bool recursive,
            bool followLinks,
            bool delete,
            bool updateNrs,
            bool dryRun);

        Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerAddAsync(
            IntPtr app,
            string sourceFile,
            string url,
            bool force,
            bool updateNrs,
            bool followLinks,
            bool dryRun);

        Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerAddFromRawAsync(
            IntPtr app,
            byte[] data,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun);

        Task<string> FilesPutPublicImmutableAsync(IntPtr app, byte[] data, string mediaType, bool dryRun);

        Task<byte[]> FilesGetPublicImmutableAsync(IntPtr app, string url, ulong start, ulong end);

        Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerRemovePathAsync(IntPtr app, string url, bool recursive, bool updateNrs, bool dryRun);

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

        #region NRS

        Task<SafeUrl> ParseUrlAsync(string url);

        Task<(string, ProcessedEntries, string)> CreateNrsMapContainerAsync(
            IntPtr app,
            string name,
            string link,
            bool directLink,
            bool dryRun,
            bool setDefault);

        Task<(string, string, ulong)> AddToNrsMapContainerAsync(
            IntPtr app,
            string name,
            string link,
            bool setDefault,
            bool directLink,
            bool dryRun);

        Task<(string, string, ulong)> RemoveFromNrsMapContainerAsync(
            IntPtr app,
            string name,
            bool dryRun);

        Task<(string, ulong)> GetNrsMapContainerAsync(
            IntPtr app,
            string url);

        #endregion NRS

        #region SequenceData

        Task<string> CreateSequenceAsync(
            IntPtr app,
            byte[] data,
            byte[] name,
            ulong typeTag,
            bool isPrivate);

        Task<(ulong, byte[])> GetSequenceAsync(IntPtr app, string url);

        Task AppendSequenceAsync(IntPtr app, string url, byte[] data);

        #endregion
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
