using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("SafeApp.AppBindings")]
[assembly: InternalsVisibleTo("SafeApp.MockAuthBindings")]

namespace SafeApp.Utilities {
  [PublicAPI]
  public enum MDataAction {
    Insert,
    Update,
    Delete,
    ManagePermissions
  }

  [PublicAPI]
  public struct AccountInfo {
    public ulong MutationsDone;
    public ulong MutationsAvailable;
  }

  [PublicAPI]
  public struct MDataInfo {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
    public byte[] Name;

    public ulong TypeTag;
    [MarshalAs(UnmanagedType.U1)] public bool HasEncInfo;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
    public byte[] EncKey;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
    public byte[] EncNonce;

    [MarshalAs(UnmanagedType.U1)] public bool HasNewEncInfo;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
    public byte[] NewEncKey;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
    public byte[] NewEncNonce;
  }

  [PublicAPI]
  public struct PermissionSet {
    [MarshalAs(UnmanagedType.U1)] public bool Read;
    [MarshalAs(UnmanagedType.U1)] public bool Insert;
    [MarshalAs(UnmanagedType.U1)] public bool Update;
    [MarshalAs(UnmanagedType.U1)] public bool Delete;
    [MarshalAs(UnmanagedType.U1)] public bool ManagePermissions;
  }

  [PublicAPI]
  public struct AuthReq {
    public AppExchangeInfo App;
    public bool AppContainer;
    public List<ContainerPermissions> Containers;

    internal AuthReq(AuthReqNative native) {
      App = native.App;
      AppContainer = native.AppContainer;
      Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
    }

    internal AuthReqNative ToNative() {
      return new AuthReqNative {
        App = App,
        AppContainer = AppContainer,
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
        ContainersCap = UIntPtr.Zero
      };
    }
  }

  internal struct AuthReqNative {
    public AppExchangeInfo App;
    [MarshalAs(UnmanagedType.U1)] public bool AppContainer;
    public IntPtr ContainersPtr;
    public UIntPtr ContainersLen;

    // ReSharper disable once NotAccessedField.Compiler
    public UIntPtr ContainersCap;

    internal void Free() {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }

  [PublicAPI]
  public struct ContainersReq {
    public AppExchangeInfo App;
    public List<ContainerPermissions> Containers;

    internal ContainersReq(ContainersReqNative native) {
      App = native.App;
      Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
    }

    internal ContainersReqNative ToNative() {
      return new ContainersReqNative {
        App = App,
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
        ContainersCap = UIntPtr.Zero
      };
    }
  }

  internal struct ContainersReqNative {
    public AppExchangeInfo App;
    public IntPtr ContainersPtr;
    public UIntPtr ContainersLen;

    // ReSharper disable once NotAccessedField.Compiler
    public UIntPtr ContainersCap;

    internal void Free() {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }

  [PublicAPI]
  public struct AppExchangeInfo {
    [MarshalAs(UnmanagedType.LPStr)] public string Id;
    [MarshalAs(UnmanagedType.LPStr)] public string Scope;
    [MarshalAs(UnmanagedType.LPStr)] public string Name;
    [MarshalAs(UnmanagedType.LPStr)] public string Vendor;
  }

  [PublicAPI]
  public struct ContainerPermissions {
    [MarshalAs(UnmanagedType.LPStr)] public string ContName;
    public PermissionSet Access;
  }

  [PublicAPI]
  public struct ShareMDataReq {
    public AppExchangeInfo App;
    public List<ShareMData> MData;

    internal ShareMDataReq(ShareMDataReqNative native) {
      App = native.App;
      MData = BindingUtils.CopyToObjectList<ShareMData>(native.MDataPtr, (int)native.MDataLen);
    }

    internal ShareMDataReqNative ToNative() {
      return new ShareMDataReqNative {
        App = App,
        MDataPtr = BindingUtils.CopyFromObjectList(MData),
        MDataLen = (UIntPtr)(MData?.Count ?? 0),
        MDataCap = UIntPtr.Zero
      };
    }
  }

  internal struct ShareMDataReqNative {
    public AppExchangeInfo App;
    public IntPtr MDataPtr;
    public UIntPtr MDataLen;

    // ReSharper disable once NotAccessedField.Compiler
    public UIntPtr MDataCap;

    internal void Free() {
      BindingUtils.FreeList(ref MDataPtr, ref MDataLen);
    }
  }

  [PublicAPI]
  public struct ShareMData {
    public ulong TypeTag;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
    public byte[] Name;

    public PermissionSet Perms;
  }

  [PublicAPI]
  public struct AuthGranted {
    public AppKeys AppKeys;
    public AccessContInfo AccessContainerInfo;
    public AccessContainerEntry AccessContainerEntry;
    public List<byte> BootstrapConfig;

    internal AuthGranted(AuthGrantedNative native) {
      AppKeys = native.AppKeys;
      AccessContainerInfo = native.AccessContainerInfo;
      AccessContainerEntry = new AccessContainerEntry(native.AccessContainerEntry);
      BootstrapConfig = BindingUtils.CopyToByteList(native.BootstrapConfigPtr, (int)native.BootstrapConfigLen);
    }

    internal AuthGrantedNative ToNative() {
      return new AuthGrantedNative {
        AppKeys = AppKeys,
        AccessContainerInfo = AccessContainerInfo,
        AccessContainerEntry = AccessContainerEntry.ToNative(),
        BootstrapConfigPtr = BindingUtils.CopyFromByteList(BootstrapConfig),
        BootstrapConfigLen = (UIntPtr)(BootstrapConfig?.Count ?? 0),
        BootstrapConfigCap = UIntPtr.Zero
      };
    }
  }

  internal struct AuthGrantedNative {
    public AppKeys AppKeys;
    public AccessContInfo AccessContainerInfo;
    public AccessContainerEntryNative AccessContainerEntry;
    public IntPtr BootstrapConfigPtr;
    public UIntPtr BootstrapConfigLen;

    // ReSharper disable once NotAccessedField.Compiler
    public UIntPtr BootstrapConfigCap;

    internal void Free() {
      AccessContainerEntry.Free();
      BindingUtils.FreeList(ref BootstrapConfigPtr, ref BootstrapConfigLen);
    }
  }

  [PublicAPI]
  public struct AppKeys {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
    public byte[] OwnerKey;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
    public byte[] EncKey;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
    public byte[] SignPk;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignSecretKeyLen)]
    public byte[] SignSk;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.AsymPublicKeyLen)]
    public byte[] EncPk;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.AsymSecretKeyLen)]
    public byte[] EncSk;
  }

  [PublicAPI]
  public struct AccessContInfo {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
    public byte[] Id;

    public ulong Tag;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
    public byte[] Nonce;
  }

  [PublicAPI]
  public struct AccessContainerEntry {
    public List<ContainerInfo> Containers;

    internal AccessContainerEntry(AccessContainerEntryNative native) {
      Containers = BindingUtils.CopyToObjectList<ContainerInfo>(native.ContainersPtr, (int)native.ContainersLen);
    }

    internal AccessContainerEntryNative ToNative() {
      return new AccessContainerEntryNative {
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
        ContainersCap = UIntPtr.Zero
      };
    }
  }

  internal struct AccessContainerEntryNative {
    public IntPtr ContainersPtr;
    public UIntPtr ContainersLen;

    // ReSharper disable once NotAccessedField.Compiler
    public UIntPtr ContainersCap;

    internal void Free() {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }

  [PublicAPI]
  public struct ContainerInfo {
    [MarshalAs(UnmanagedType.LPStr)] public string Name;
    public MDataInfo MDataInfo;
    public PermissionSet Permissions;
  }

  [PublicAPI]
  public struct AppAccess {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
    public byte[] SignKey;

    public PermissionSet Permissions;
    [MarshalAs(UnmanagedType.LPStr)] public string Name;
    [MarshalAs(UnmanagedType.LPStr)] public string AppId;
  }

  [PublicAPI]
  public struct MetadataResponse {
    [MarshalAs(UnmanagedType.LPStr)] public string Name;
    [MarshalAs(UnmanagedType.LPStr)] public string Description;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
    public byte[] XorName;

    public ulong TypeTag;
  }

  [PublicAPI]
  public struct MDataKey {
    public List<byte> Val;

    internal MDataKey(MDataKeyNative native) {
      Val = BindingUtils.CopyToByteList(native.ValPtr, (int)native.ValLen);
    }

    internal MDataKeyNative ToNative() {
      return new MDataKeyNative {ValPtr = BindingUtils.CopyFromByteList(Val), ValLen = (UIntPtr)(Val?.Count ?? 0)};
    }
  }

  internal struct MDataKeyNative {
    public IntPtr ValPtr;
    public UIntPtr ValLen;

    internal void Free() {
      BindingUtils.FreeList(ref ValPtr, ref ValLen);
    }
  }

  [PublicAPI]
  public struct MDataValue {
    public List<byte> Content;
    public ulong EntryVersion;

    internal MDataValue(MDataValueNative native) {
      Content = BindingUtils.CopyToByteList(native.ContentPtr, (int)native.ContentLen);
      EntryVersion = native.EntryVersion;
    }

    internal MDataValueNative ToNative() {
      return new MDataValueNative {
        ContentPtr = BindingUtils.CopyFromByteList(Content),
        ContentLen = (UIntPtr)(Content?.Count ?? 0),
        EntryVersion = EntryVersion
      };
    }
  }

  internal struct MDataValueNative {
    public IntPtr ContentPtr;
    public UIntPtr ContentLen;
    public ulong EntryVersion;

    internal void Free() {
      BindingUtils.FreeList(ref ContentPtr, ref ContentLen);
    }
  }

  [PublicAPI]
  public struct MDataEntry {
    public MDataKey Key;
    public MDataValue Value;

    internal MDataEntry(MDataEntryNative native) {
      Key = new MDataKey(native.Key);
      Value = new MDataValue(native.Value);
    }

    internal MDataEntryNative ToNative() {
      return new MDataEntryNative {Key = Key.ToNative(), Value = Value.ToNative()};
    }
  }

  internal struct MDataEntryNative {
    public MDataKeyNative Key;
    public MDataValueNative Value;

    //ReSharper disable once UnusedMember.Global
    internal void Free() {
      Key.Free();
      Value.Free();
    }
  }

  [PublicAPI]
  public struct File {
    public ulong Size;
    public long CreatedSec;
    public uint CreatedNsec;
    public long ModifiedSec;
    public uint ModifiedNsec;
    public List<byte> UserMetadata;
    public byte[] DataMapName;

    internal File(FileNative native) {
      Size = native.Size;
      CreatedSec = native.CreatedSec;
      CreatedNsec = native.CreatedNsec;
      ModifiedSec = native.ModifiedSec;
      ModifiedNsec = native.ModifiedNsec;
      UserMetadata = BindingUtils.CopyToByteList(native.UserMetadataPtr, (int)native.UserMetadataLen);
      DataMapName = native.DataMapName;
    }

    internal FileNative ToNative() {
      return new FileNative {
        Size = Size,
        CreatedSec = CreatedSec,
        CreatedNsec = CreatedNsec,
        ModifiedSec = ModifiedSec,
        ModifiedNsec = ModifiedNsec,
        UserMetadataPtr = BindingUtils.CopyFromByteList(UserMetadata),
        UserMetadataLen = (UIntPtr)(UserMetadata?.Count ?? 0),
        UserMetadataCap = UIntPtr.Zero,
        DataMapName = DataMapName
      };
    }
  }

  internal struct FileNative {
    public ulong Size;
    public long CreatedSec;
    public uint CreatedNsec;
    public long ModifiedSec;
    public uint ModifiedNsec;
    public IntPtr UserMetadataPtr;
    public UIntPtr UserMetadataLen;

    // ReSharper disable once NotAccessedField.Compiler
    public UIntPtr UserMetadataCap;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
    public byte[] DataMapName;

    internal void Free() {
      BindingUtils.FreeList(ref UserMetadataPtr, ref UserMetadataLen);
    }
  }

  [PublicAPI]
  public struct UserPermissionSet {
    public ulong UserH;
    public PermissionSet PermSet;
  }
}
