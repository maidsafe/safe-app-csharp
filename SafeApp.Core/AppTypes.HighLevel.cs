using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SafeApp.Core
{
    /// <summary>
    /// Public and secret BLS key.
    /// </summary>
    public struct BlsKeyPair
    {
        /// <summary>
        /// Public key.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string PK;

        /// <summary>
        /// Secret key.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string SK;
    }

    /// <summary>
    /// XorUrl Encoder
    /// </summary>
    public struct XorUrlEncoder
    {
        /// <summary>
        /// Encoding version.
        /// </summary>
        public ulong EncodingVersion;

        /// <summary>
        /// XorName for the data.
        /// </summary>
        public byte[] XorName;

        /// <summary>
        /// TypeTag for the mutable data type.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// Stored data type.
        /// </summary>
        public DataType DataType;

        /// <summary>
        /// Stored content type.
        /// </summary>
        public ContentType ContentType;

        /// <summary>
        /// XorUrl path for the data.
        /// </summary>
        public string Path;

        /// <summary>
        /// XorUrl sub names for the data.
        /// </summary>
        public List<string> SubNames;

        /// <summary>
        /// Content version on the network.
        /// </summary>
        public ulong ContentVersion;

        /// <summary>
        /// XorUrl query.
        /// </summary>
        public string QueryParams;

        /// <summary>
        /// XorUrl fragment.
        /// </summary>
        public string Fragment;

        internal XorUrlEncoder(XorUrlEncoderNative native)
        {
            EncodingVersion = native.EncodingVersion;
            XorName = native.XorName;
            TypeTag = native.TypeTag;
            DataType = (DataType)native.DataType;
            ContentType = (ContentType)native.ContentType;
            Path = native.Path;
            SubNames = BindingUtils.CopyToStringList(native.SubNamesPtr, (int)native.SubNamesLen);
            ContentVersion = native.ContentVersion;
            QueryParams = native.QueryParams;
            Fragment = native.Fragment;
        }

        internal XorUrlEncoderNative ToNative()
        {
            return new XorUrlEncoderNative
            {
                EncodingVersion = EncodingVersion,
                XorName = XorName,
                TypeTag = TypeTag,
                DataType = (ulong)DataType,
                ContentType = (ushort)ContentType,
                Path = Path,
                SubNamesPtr = BindingUtils.CopyFromStringList(SubNames),
                SubNamesLen = (UIntPtr)(SubNames?.Count ?? 0),
                ContentVersion = ContentVersion,
                QueryParams = QueryParams,
                Fragment = Fragment
            };
        }
    }

    internal struct XorUrlEncoderNative
    {
        public ulong EncodingVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;
        public ulong TypeTag;
        public ulong DataType;
        public ushort ContentType;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Path;
        public IntPtr SubNamesPtr;
        public UIntPtr SubNamesLen;
        public ulong ContentVersion;
        [MarshalAs(UnmanagedType.LPStr)]
        public string QueryParams;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Fragment;

        internal void Free()
        {
            BindingUtils.FreeList(ref SubNamesPtr, ref SubNamesLen);
        }
    }

    /// <summary>
    /// Base interface for the different data types stored on the network.
    /// </summary>
    public interface ISafeData
    {
    }

    /// <summary>
    /// SafeKey data type.
    /// </summary>
    public struct SafeKey : ISafeData
    {
        /// <summary>
        /// SafeKey's XorUrl.
        /// </summary>
        public string XorUrl;

        /// <summary>
        /// SafeKey's XorName.
        /// </summary>
        public byte[] XorName;

        /// <summary>
        /// NrsMapContainerInfo for the SafeKey stored on the network.
        /// </summary>
        public NrsMapContainerInfo ResolvedFrom;

        internal SafeKey(SafeKeyNative native)
        {
            XorUrl = native.XorUrl;
            XorName = native.XorName;
            ResolvedFrom = new NrsMapContainerInfo(native.ResolvedFrom);
        }

        internal SafeKeyNative ToNative()
        {
            return new SafeKeyNative
            {
                XorUrl = XorUrl,
                XorName = XorName,
                ResolvedFrom = ResolvedFrom.ToNative(),
            };
        }
    }

    internal struct SafeKeyNative : ISafeData
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string XorUrl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;
        public NrsMapContainerInfoNative ResolvedFrom;
    }

    /// <summary>
    /// SafeWallet data type.
    /// </summary>
    public struct Wallet : ISafeData
    {
        /// <summary>
        /// Wallet's XorUrl.
        /// </summary>
        public string XorUrl;

        /// <summary>
        /// Wallet' XorName.
        /// </summary>
        public byte[] XorName;

        /// <summary>
        /// TypeTag used by the wallet.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// List of all spendable balances.
        /// </summary>
        public WalletSpendableBalances Balances;

        /// <summary>
        /// Wallet data type identifier.
        /// </summary>
        public DataType DataType;

        /// <summary>
        /// NrsMapContainerInfo for the wallet stored on the network.
        /// </summary>
        public NrsMapContainerInfo ResolvedFrom;

        internal Wallet(WalletNative native)
        {
            XorUrl = native.XorUrl;
            XorName = native.XorName;
            TypeTag = native.TypeTag;
            Balances = new WalletSpendableBalances(native.Balances);
            DataType = (DataType)native.DataType;
            ResolvedFrom = new NrsMapContainerInfo(native.ResolvedFrom);
        }

        internal WalletNative ToNative()
        {
            return new WalletNative
            {
                XorUrl = XorUrl,
                XorName = XorName,
                TypeTag = TypeTag,
                Balances = Balances.ToNative(),
                DataType = (ulong)DataType,
                ResolvedFrom = ResolvedFrom.ToNative(),
            };
        }
    }

    internal struct WalletNative
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string XorUrl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;
        public ulong TypeTag;
        public WalletSpendableBalancesNative Balances;
        public ulong DataType;
        public NrsMapContainerInfoNative ResolvedFrom;

        internal void Free()
        {
            Balances.Free();
        }
    }

    /// <summary>
    /// FilesContainer data type.
    /// </summary>
    public struct FilesContainer : ISafeData
    {
        /// <summary>
         /// FilesContainer's XorUrl.
         /// </summary>
        public string XorUrl;

        /// <summary>
        /// FilesContainer's XorName.
        /// </summary>
        public byte[] XorName;

        /// <summary>
        /// TypeTag used by the FileContainer.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// FilesContainer's current version.
        /// </summary>
        public ulong Version;

        /// <summary>
        /// FilesMap in JSON format.
        /// </summary>
        public FilesMap FilesMap;

        /// <summary>
        /// FilesContainer data type identifier.
        /// </summary>
        public DataType DataType;

        /// <summary>
        /// NrsMapContainerInfo for the FilesContainer.
        /// </summary>
        public NrsMapContainerInfo ResolvedFrom;

        internal FilesContainer(FilesContainerNative native)
        {
            XorUrl = native.XorUrl;
            XorName = native.XorName;
            TypeTag = native.TypeTag;
            Version = native.Version;
            FilesMap = new FilesMap(native.FilesMap);
            DataType = (DataType)native.DataType;
            ResolvedFrom = new NrsMapContainerInfo(native.ResolvedFrom);
        }

        internal FilesContainerNative ToNative()
        {
            return new FilesContainerNative
            {
                XorUrl = XorUrl,
                XorName = XorName,
                TypeTag = TypeTag,
                Version = Version,
                FilesMap = FilesMap.ToNative(),
                DataType = (ulong)DataType,
                ResolvedFrom = ResolvedFrom.ToNative(),
            };
        }
    }

    internal struct FilesContainerNative : ISafeData
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string XorUrl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;
        public ulong TypeTag;
        public ulong Version;
        public FilesMapNative FilesMap;
        public ulong DataType;
        public NrsMapContainerInfoNative ResolvedFrom;

        internal void Free()
        {
            FilesMap.Free();
        }
    }

    /// <summary>
    /// PublishedImmutableData data type.
    /// </summary>
    public struct PublishedImmutableData : ISafeData
    {
        /// <summary>
        /// XorUrl
        /// </summary>
        public string XorUrl;

        /// <summary>
        /// XorName
        /// </summary>
        public byte[] XorName;

        /// <summary>
        /// Raw data in byte[] format
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// NrsMapContainerInfo for the PublishedImmutableData.
        /// </summary>
        public NrsMapContainerInfo ResolvedFrom;

        /// <summary>
        /// MIME type for the stored data/file.
        /// </summary>
        public string MediaType;

        internal PublishedImmutableData(PublishedImmutableDataNative native)
        {
            XorUrl = native.XorUrl;
            XorName = native.XorName;
            Data = BindingUtils.CopyToByteArray(native.DataPtr, (int)native.DataLen);
            ResolvedFrom = new NrsMapContainerInfo(native.ResolvedFrom);
            MediaType = native.MediaType;
        }

        internal PublishedImmutableDataNative ToNative()
        {
            return new PublishedImmutableDataNative
            {
                XorUrl = XorUrl,
                XorName = XorName,
                DataPtr = BindingUtils.CopyFromByteArray(Data),
                DataLen = (UIntPtr)(Data?.Length ?? 0),
                ResolvedFrom = ResolvedFrom.ToNative(),
                MediaType = MediaType
            };
        }
    }

    internal struct PublishedImmutableDataNative
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string XorUrl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;
        public IntPtr DataPtr;
        public UIntPtr DataLen;
        public NrsMapContainerInfoNative ResolvedFrom;
        [MarshalAs(UnmanagedType.LPStr)]
        public string MediaType;

        internal void Free()
        {
            BindingUtils.FreeList(ref DataPtr, ref DataLen);
        }
    }

    /// <summary>
    /// Data type used to indicate fetch failure.
    /// </summary>
    public struct SafeDataFetchFailed : ISafeData
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public readonly int Code;

        /// <summary>
        /// Error description.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Initialise new instance.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="description">Error description.</param>
        public SafeDataFetchFailed(int code, string description)
        {
            Code = code;
            Description = description;
        }
    }

    /// <summary>
    /// File metadata entry.
    /// </summary>
    public struct FileMetaDataItem
    {
        /// <summary>
        /// File metadata entry key.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string MetaDataKey;

        /// <summary>
        /// File metadata entry value.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string MetaDataValue;
    }

    /// <summary>
    /// File information contains filename and metadata.
    /// </summary>
    public struct FileInfo
    {
        /// <summary>
        /// File name.
        /// </summary>
        public string FileName;

        /// <summary>
        /// File metadata entries.
        /// </summary>
        public List<FileMetaDataItem> FileMetaData;

        internal FileInfo(FileInfoNative native)
        {
            FileName = native.FileName;
            FileMetaData = BindingUtils.CopyToObjectList<FileMetaDataItem>(native.FileMetaDataPtr, (int)native.FileMetaDataLen);
        }

        internal FileInfoNative ToNative()
        {
            return new FileInfoNative
            {
                FileName = FileName,
                FileMetaDataPtr = BindingUtils.CopyFromObjectList(FileMetaData),
                FileMetaDataLen = (UIntPtr)(FileMetaData?.Count ?? 0)
            };
        }
    }

    internal struct FileInfoNative
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileName;
        public IntPtr FileMetaDataPtr;
        public UIntPtr FileMetaDataLen;

        internal void Free()
        {
            BindingUtils.FreeList(ref FileMetaDataPtr, ref FileMetaDataLen);
        }
    }

    /// <summary>
    /// Files Containet file map.
    /// </summary>
    public struct FilesMap
    {
        /// <summary>
        /// Files exists in files container.
        /// </summary>
        public List<FileInfo> Files;

        internal FilesMap(FilesMapNative native)
        {
            var nativeFiles = new List<FileInfoNative>();
            nativeFiles = BindingUtils.CopyToObjectList<FileInfoNative>(native.FilesPtr, (int)native.FilesLen);
            Files = new List<FileInfo>();
            foreach (var item in nativeFiles)
                Files.Add(new FileInfo(item));
        }

        internal FilesMapNative ToNative()
        {
            return new FilesMapNative
            {
                FilesPtr = BindingUtils.CopyFromObjectList(Files),
                FilesLen = (UIntPtr)(Files?.Count ?? 0)
            };
        }
    }

    internal struct FilesMapNative
    {
        public IntPtr FilesPtr;
        public UIntPtr FilesLen;

        internal void Free()
        {
            BindingUtils.FreeList(ref FilesPtr, ref FilesLen);
        }
    }

    /// <summary>
    /// Wallet Spendable balance.
    /// </summary>
    public struct WalletSpendableBalance
    {
        /// <summary>
        /// XOR Url
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string XorUrl;

        /// <summary>
        /// Secret Key
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Sk;
    }

    /// <summary>
    /// Spendable Wallet balance.
    /// </summary>
    public struct WalletSpendableBalanceInfo
    {
        /// <summary>
        /// Wallet name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string WalletName;

        /// <summary>
        /// Flag indicating whether the wallet is default.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool IsDefault;

        /// <summary>
        /// Spendable wallet balance.
        /// </summary>
        public WalletSpendableBalance Balance;
    }

    /// <summary>
    /// Wallet spendable balances.
    /// </summary>
    public struct WalletSpendableBalances
    {
        /// <summary>
        /// List of spendable wallet balances.
        /// </summary>
        public List<WalletSpendableBalanceInfo> WalletBalances;

        /// <summary>
        /// Initialise a new wallet spendable balances object from native wallet spendable balances.
        /// </summary>
        /// <param name="native"></param>
        internal WalletSpendableBalances(WalletSpendableBalancesNative native)
        {
            WalletBalances = BindingUtils.CopyToObjectList<WalletSpendableBalanceInfo>(
                native.WalletBalancesPtr,
                (int)native.WalletBalancesLen);
        }

        /// <summary>
        /// Returns a native wallet spendable balance.
        /// </summary>
        /// <returns></returns>
        internal WalletSpendableBalancesNative ToNative()
        {
            return new WalletSpendableBalancesNative
            {
                WalletBalancesPtr = BindingUtils.CopyFromObjectList(WalletBalances),
                WalletBalancesLen = (UIntPtr)(WalletBalances?.Count ?? 0)
            };
        }
    }

    /// <summary>
    /// Represents native wallet spendable balances.
    /// </summary>
    internal struct WalletSpendableBalancesNative
    {
        /// <summary>
        /// Wallet spendable balances pointer.
        /// </summary>
        public IntPtr WalletBalancesPtr;

        /// <summary>
        /// Wallet balances length.
        /// </summary>
        public UIntPtr WalletBalancesLen;

        /// <summary>
        /// Free the wallet spendable balances pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref WalletBalancesPtr, ref WalletBalancesLen);
        }
    }

    /// <summary>
    /// Represents the processed file.
    /// </summary>
    public struct ProcessedFile
    {
        /// <summary>
        /// File name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileName;

        /// <summary>
        /// File meta data.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileMetaData;

        /// <summary>
        /// File XorUrl.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileXorUrl;
    }

    /// <summary>
    /// Processed files.
    /// </summary>
    public struct ProcessedFiles
    {
        /// <summary>
        /// Files.
        /// </summary>
        public List<ProcessedFile> Files;

        /// <summary>
        /// Initialise the processed file from native file info.
        /// </summary>
        /// <param name="native"></param>
        internal ProcessedFiles(ProcessedFilesNative native)
        {
            Files = BindingUtils.CopyToObjectList<ProcessedFile>(
                native.ProcessedFilesPtr,
                (int)native.ProcessedFilesLen);
        }

        /// <summary>
        /// Returns the native processed file.
        /// </summary>
        /// <returns></returns>
        internal ProcessedFilesNative ToNative()
        {
            return new ProcessedFilesNative
            {
                ProcessedFilesPtr = BindingUtils.CopyFromObjectList(Files),
                ProcessedFilesLen = (UIntPtr)(Files?.Count ?? 0)
            };
        }
    }

    /// <summary>
    /// Represents the native processed file.
    /// </summary>
    internal struct ProcessedFilesNative
    {
        /// <summary>
        /// Processed files pointer.
        /// </summary>
        public IntPtr ProcessedFilesPtr;

        /// <summary>
        /// Processed files length.
        /// </summary>
        public UIntPtr ProcessedFilesLen;

        /// <summary>
        /// Free the processed file pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ProcessedFilesPtr, ref ProcessedFilesLen);
        }
    }

    /// <summary>
    /// Contains the information required to work with NRS.
    /// </summary>
    public struct NrsMapContainerInfo
    {
        /// <summary>
        /// Public name for the container.
        /// </summary>
        public string PublicName;

        /// <summary>
        /// Container's XorUrl.
        /// </summary>
        public string XorUrl;

        /// <summary>
        /// Container's XorName.
        /// </summary>
        public byte[] XorName;

        /// <summary>
        /// TypeTag used when storing on the network.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// Current version.
        /// </summary>
        public ulong Version;

        /// <summary>
        /// NrsMap in JSON format.
        /// </summary>
        public string NrsMap;

        /// <summary>
        /// DataType identifier for the NrsMapContainer.
        /// </summary>
        public DataType DataType;

        internal NrsMapContainerInfo(NrsMapContainerInfoNative native)
        {
            PublicName = native.PublicName;
            XorUrl = native.XorUrl;
            XorName = native.XorName;
            TypeTag = native.TypeTag;
            Version = native.Version;
            NrsMap = native.NrsMap;
            DataType = (DataType)native.DataType;
        }

        internal NrsMapContainerInfoNative ToNative()
        {
            return new NrsMapContainerInfoNative
            {
                PublicName = PublicName,
                XorUrl = XorUrl,
                XorName = XorName,
                TypeTag = TypeTag,
                Version = Version,
                NrsMap = NrsMap,
                DataType = (ulong)DataType,
            };
        }
    }

    internal struct NrsMapContainerInfoNative
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string PublicName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string XorUrl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;
        public ulong TypeTag;
        public ulong Version;
        [MarshalAs(UnmanagedType.LPStr)]
        public string NrsMap;
        public ulong DataType;
    }

    /// <summary>
    /// Represents metadata and operation info for a processed public name entry.
    /// </summary>
    public struct ProcessedEntry
    {
        /// <summary>
        /// Public name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// Operation performed on the entry.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Action;

        /// <summary>
        /// XorUrl for the entry.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Link;
    }

    /// <summary>
    /// Holds list of all the processed public name entries.
    /// </summary>
    public struct ProcessedEntries
    {
        /// <summary>
        /// List of all the public name entries on which the operation was performed.
        /// </summary>
        public List<ProcessedEntry> Entries;

        internal ProcessedEntries(ProcessedEntriesNative native)
        {
            Entries = BindingUtils.CopyToObjectList<ProcessedEntry>(native.ProcessedEntriesPtr, (int)native.ProcessedEntriesLen);
        }

        internal ProcessedEntriesNative ToNative()
        {
            return new ProcessedEntriesNative
            {
                ProcessedEntriesPtr = BindingUtils.CopyFromObjectList(Entries),
                ProcessedEntriesLen = (UIntPtr)(Entries?.Count ?? 0)
            };
        }
    }

    internal struct ProcessedEntriesNative
    {
        public IntPtr ProcessedEntriesPtr;
        public UIntPtr ProcessedEntriesLen;

        internal void Free()
        {
            BindingUtils.FreeList(ref ProcessedEntriesPtr, ref ProcessedEntriesLen);
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public struct NrsMap
    {
        public Dictionary<string, SubNamesMapEntry> SubNamesMap;
        public Dictionary<string, Rdf> Default;
    }

    public struct Rdf
    {
        public DateTime Created;
        public string Link;
        public DateTime Modified;
    }

    public struct SubNamesMapEntry
    {
        public string SubName;
        public string SubNameRdf;
    }

    public enum DataType
    {
        SafeKey,
        PublishedImmutableData,
        UnpublishedImmutableData,
        SeqMutableData,
        UnseqMutableData,
        PublishedSeqAppendOnlyData,
        PublishedUnseqAppendOnlyData,
        UnpublishedSeqAppendOnlyData,
        UnpublishedUnseqAppendOnlyData,
    }

    public enum ContentType
    {
        Raw,
        Wallet,
        FilesContainer,
        NrsMapContainer,
        MediaType, // nb: we're missing the variant value of the rust enum here (the actual media type)
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
