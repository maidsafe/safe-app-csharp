using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("SafeApp.AppBindings")]
[assembly: InternalsVisibleTo("SafeApp.MockAuthBindings")]

namespace SafeApp.Core
{
    /// <summary>
    /// Represents the application keys.
    /// </summary>
    [PublicAPI]
    public struct AppKeys
    {
        /// <summary>
        /// Owner signing public key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.BlsPublicKeyLen)]
        public byte[] OwnerKey;

        /// <summary>
        /// Data symmetric Encryption Key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
        public byte[] EncKey;

        /// <summary>
        /// Asymmetric sign public key.
        /// This is the identity of the App in the network.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
        public byte[] SignPk;

        /// <summary>
        /// Asymmetric sign Private Key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignSecretKeyLen)]
        public byte[] SignSk;

        /// <summary>
        /// Asymmetric encryption public key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.AsymPublicKeyLen)]
        public byte[] EncPk;

        /// <summary>
        /// Asymmetric encryption Private Key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.AsymSecretKeyLen)]
        public byte[] EncSk;
    }

    /// <summary>
    /// Represents access container information.
    /// </summary>
    [PublicAPI]
    public struct AccessContInfo
    {
        /// <summary>
        /// Container Id.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Id;

        /// <summary>
        /// tagType.
        /// </summary>
        public ulong Tag;

        /// <summary>
        /// Nonce
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
        public byte[] Nonce;
    }

    /// <summary>
    /// Represents access container entry for a single app.
    /// </summary>
    [PublicAPI]
    public struct AccessContainerEntry
    {
        /// <summary>
        /// List of containers (name, 'MDataInfo' and permissions).
        /// </summary>
        public List<ContainerInfo> Containers;

        /// <summary>
        /// Initialise a new access container entry object from native access container entry
        /// </summary>
        /// <param name="native"></param>
        internal AccessContainerEntry(AccessContainerEntryNative native)
        {
            Containers = BindingUtils.CopyToObjectList<ContainerInfo>(native.ContainersPtr, (int)native.ContainersLen);
        }

        /// <summary>
        /// Returns native access container entry.
        /// </summary>
        /// <returns></returns>
        internal AccessContainerEntryNative ToNative()
        {
            return new AccessContainerEntryNative
            {
                ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
                ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
                ContainersCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents native access container entry for a single app.
    /// </summary>
    internal struct AccessContainerEntryNative
    {
        /// <summary>
        /// Pointer to the array of 'ContainerInfo'.
        /// </summary>
        public IntPtr ContainersPtr;

        /// <summary>
        /// Size of the array.
        /// </summary>
        public UIntPtr ContainersLen;

        /// <summary>
        /// Internal field used by rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ContainersCap;

        /// <summary>
        /// Free the container pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
        }
    }

    /// <summary>
    /// Container information with permission set.
    /// </summary>
    [PublicAPI]
    public struct ContainerInfo
    {
        /// <summary>
        /// Container name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// Container's MDataInfo.
        /// </summary>
        public MDataInfo MDataInfo;

        /// <summary>
        /// App's permissions in the container.
        /// </summary>
        public PermissionSet Permissions;
    }

    /// <summary>
    /// Mutable Data shell.
    /// Information allowing to locate and access Mutable Data on the network.
    /// </summary>
    [PublicAPI]
    public struct MDataInfo
    {
        /// <summary>
        /// Flag indicating whether the MDataInfo is sequenced.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Seq;

        /// <summary>
        /// Name of the Mutable Data.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Name;

        /// <summary>
        /// Type tag.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// Flag indicating whether the encryption info (`enc_key` and `enc_nonce`) is set.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool HasEncInfo;

        /// <summary>
        /// Key to encrypt/decrypt the content.
        /// Meaningful only if `HasEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
        public byte[] EncKey;

        /// <summary>
        /// Nonce to be used for Encryption Keys.
        /// Meaningful only if `HasEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
        public byte[] EncNonce;

        /// <summary>
        /// Flag indicating whether the new encryption info is set.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool HasNewEncInfo;

        /// <summary>
        /// New Encryption Key(used for two-phase re-encryption).
        /// Meaningful only if `HasNewEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
        public byte[] NewEncKey;

        /// <summary>
        /// New encryption nonce (used for two-phase data re-encryption).
        /// Meaningful only if `HasNewEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
        public byte[] NewEncNonce;
    }

    /// <summary>
    /// Represents a requested set of changes to permissions of a Mutable Data.
    /// </summary>
    [PublicAPI]
    public struct PermissionSet
    {
        /// <summary>
        /// Read permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Read;

        /// <summary>
        /// Insert permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Insert;

        /// <summary>
        /// Update permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Update;

        /// <summary>
        /// Delete permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Delete;

        /// <summary>
        /// Manage permissions permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool ManagePermissions;
    }

    /// <summary>
    /// Represents an wrapper for authorisation request.
    /// </summary>
    [PublicAPI]
    public struct AuthReq
    {
        /// <summary>
        /// The application identifier for this request
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// true if the app wants dedicated container for itself,
        /// false otherwise.
        /// </summary>
        public bool AppContainer;

        /// <summary>
        /// True if the app wants to transfer coins.
        /// </summary>
        public bool AppPermissionTransferCoins;

        /// <summary>
        /// The list of containers requesting access for.
        /// </summary>
        public List<ContainerPermissions> Containers;

        /// <summary>
        /// Initialise new auth request object from native auth request.
        /// </summary>
        /// <param name="native"></param>
        internal AuthReq(AuthReqNative native)
        {
            App = native.App;
            AppContainer = native.AppContainer;
            AppPermissionTransferCoins = native.AppPermissionTransferCoins;
            Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
        }

        /// <summary>
        /// Returns native authentication request.
        /// </summary>
        /// <returns></returns>
        internal AuthReqNative ToNative()
        {
            return new AuthReqNative
            {
                App = App,
                AppContainer = AppContainer,
                AppPermissionTransferCoins = AppPermissionTransferCoins,
                ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
                ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
                ContainersCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents native authentication request.
    /// </summary>
    internal struct AuthReqNative
    {
        /// <summary>
        /// The application identifier for this request
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// true if the app wants dedicated container for itself,
        /// false otherwise.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool AppContainer;

        /// <summary>
        /// true if the app wants to transfer coins.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool AppPermissionTransferCoins;

        /// <summary>
        /// Pointer to containers.
        /// </summary>
        public IntPtr ContainersPtr;

        /// <summary>
        /// Length of containers.
        /// </summary>
        public UIntPtr ContainersLen;

        /// <summary>
        /// Internal field used by rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ContainersCap;

        /// <summary>
        /// Free the container pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
        }
    }

    /// <summary>
    /// Represents a containers request.
    /// </summary>
    [PublicAPI]
    public struct ContainersReq
    {
        /// <summary>
        /// App exchange info.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// Requested containers.
        /// </summary>
        public List<ContainerPermissions> Containers;

        /// <summary>
        /// Initialise a new container request object from native container request.
        /// </summary>
        /// <param name="native"></param>
        internal ContainersReq(ContainersReqNative native)
        {
            App = native.App;
            Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
        }

        /// <summary>
        /// Returns native container request.
        /// </summary>
        /// <returns></returns>
        internal ContainersReqNative ToNative()
        {
            return new ContainersReqNative
            {
                App = App,
                ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
                ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
                ContainersCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents native container request.
    /// </summary>
    internal struct ContainersReqNative
    {
        /// <summary>
        /// App exchange info.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// Pointer to containers.
        /// </summary>
        public IntPtr ContainersPtr;

        /// <summary>
        /// Length of containers.
        /// </summary>
        public UIntPtr ContainersLen;

        /// <summary>
        /// Internal field used by rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ContainersCap;

        /// <summary>
        /// Free the container pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
        }
    }

    /// <summary>
    /// Represents an application information.
    /// </summary>
    [PublicAPI]
    public struct AppExchangeInfo
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Id;

        /// <summary>
        /// Application scope, null if not present.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Scope;

        /// <summary>
        /// Application name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// Application provider/vendor (e.g. MaidSafe).
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Vendor;
    }

    /// <summary>
    /// Represents the set of permissions for a given container.
    /// </summary>
    [PublicAPI]
    public struct ContainerPermissions
    {
        /// <summary>
        /// Default container name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string ContName;

        /// <summary>
        /// Requested permission set.
        /// </summary>
        public PermissionSet Access;
    }

    /// <summary>
    /// Represents a request to share Mutable Data.
    /// </summary>
    [PublicAPI]
    public struct ShareMDataReq
    {
        /// <summary>
        /// Info about the app requesting shared access.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// List of ShareMData (contains MD names,
        /// type tags and permissions that need to be shared).
        /// </summary>
        public List<ShareMData> MData;

        /// <summary>
        /// Initialise a new Mutable Data share request object from native request.
        /// </summary>
        /// <param name="native"></param>
        internal ShareMDataReq(ShareMDataReqNative native)
        {
            App = native.App;
            MData = BindingUtils.CopyToObjectList<ShareMData>(native.MDataPtr, (int)native.MDataLen);
        }

        /// <summary>
        /// Returns ShareMDataReqNative
        /// </summary>
        /// <returns></returns>
        internal ShareMDataReqNative ToNative()
        {
            return new ShareMDataReqNative
            {
                App = App,
                MDataPtr = BindingUtils.CopyFromObjectList(MData),
                MDataLen = (UIntPtr)(MData?.Count ?? 0),
                MDataCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Native request to share Mutable Data.
    /// </summary>
    internal struct ShareMDataReqNative
    {
        /// <summary>
        /// Info about the app requesting shared access.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// Pointer to Mutable Data
        /// </summary>
        public IntPtr MDataPtr;

        /// <summary>
        /// Length of MData array.
        /// </summary>
        public UIntPtr MDataLen;

        /// <summary>
        /// Capacity of MData vec.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr MDataCap;

        /// <summary>
        /// Free Mutable Data pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref MDataPtr, ref MDataLen);
        }
    }

    /// <summary>
    /// Holds a Mutable Data information and requested permission set.
    /// </summary>
    [PublicAPI]
    public struct ShareMData
    {
        /// <summary>
        /// The Mutable Data type.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// The Mutable Data name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Name;

        /// <summary>
        /// The permissions being requested.
        /// </summary>
        public PermissionSet Perms;
    }

    /// <summary>
    /// Represents the authentication response.
    /// </summary>
    [PublicAPI]
    public struct AuthGranted
    {
        /// <summary>
        /// The access keys.
        /// </summary>
        public AppKeys AppKeys;

        /// <summary>
        /// Access container info.
        /// </summary>
        public AccessContInfo AccessContainerInfo;

        /// <summary>
        /// Access container entry.
        /// </summary>
        public AccessContainerEntry AccessContainerEntry;

        /// <summary>
        /// Bootstrap configuration.
        /// </summary>
        public byte[] BootstrapConfig;

        /// <summary>
        /// Initialise a new auth response object from native auth response.
        /// </summary>
        /// <param name="native"></param>
        internal AuthGranted(AuthGrantedNative native)
        {
            AppKeys = native.AppKeys;
            AccessContainerInfo = native.AccessContainerInfo;
            AccessContainerEntry = new AccessContainerEntry(native.AccessContainerEntry);
            BootstrapConfig = BindingUtils.CopyToByteList(native.BootstrapConfigPtr, (int)native.BootstrapConfigLen);
        }

        /// <summary>
        /// Returns native authentication response (AuthGrantedNative).
        /// </summary>
        /// <returns></returns>
        internal AuthGrantedNative ToNative()
        {
            return new AuthGrantedNative
            {
                AppKeys = AppKeys,
                AccessContainerInfo = AccessContainerInfo,
                AccessContainerEntry = AccessContainerEntry.ToNative(),
                BootstrapConfigPtr = BindingUtils.CopyFromByteList(BootstrapConfig),
                BootstrapConfigLen = (UIntPtr)(BootstrapConfig?.Length ?? 0),
                BootstrapConfigCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents the authentication response.
    /// </summary>
    internal struct AuthGrantedNative
    {
        /// <summary>
        /// The access keys.
        /// </summary>
        public AppKeys AppKeys;

        /// <summary>
        /// Access container info.
        /// </summary>
        public AccessContInfo AccessContainerInfo;

        /// <summary>
        /// Access container entry.
        /// </summary>
        public AccessContainerEntryNative AccessContainerEntry;

        /// <summary>
        /// Crust's bootstrap config.
        /// </summary>
        public IntPtr BootstrapConfigPtr;

        /// <summary>
        /// `bootstrap_config`'s length.
        /// </summary>
        public UIntPtr BootstrapConfigLen;

        /// <summary>
        /// Used by Rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr BootstrapConfigCap;

        /// <summary>
        /// Free bootstrap config pointer.
        /// </summary>
        internal void Free()
        {
            AccessContainerEntry.Free();
            BindingUtils.FreeList(ref BootstrapConfigPtr, ref BootstrapConfigLen);
        }
    }

    /// <summary>
    /// Information about an application that has access to a Mutable Data.
    /// </summary>
    [PublicAPI]
    public struct AppAccess
    {
        /// <summary>
        /// App's or user's public key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.BlsPublicKeyLen)]
        public byte[] SignKey;

        /// <summary>
        /// A list of permissions.
        /// </summary>
        public PermissionSet Permissions;

        /// <summary>
        /// App's user-facing name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// App Id.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string AppId;
    }

    /// <summary>
    /// User metadata response for Mutable Data.
    /// </summary>
    [PublicAPI]
    public struct MetadataResponse
    {
        /// <summary>
        /// Name or purpose of Mutable Data.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// Description of how this Mutable Data should or should not be shared.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Description;

        /// <summary>
        /// Xor name of this struct's corresponding Mutable Data
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;

        /// <summary>
        /// Type tag of this struct's corresponding Mutable Data.
        /// </summary>
        public ulong TypeTag;
    }
}
