using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
  [PublicAPI]
  public struct RegisteredApp {
    public AppExchangeInfo AppInfo;
    public List<ContainerPermissions> Containers;

    internal RegisteredApp(RegisteredAppNative native) {
      AppInfo = native.AppInfo;
      Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
    }

    internal RegisteredAppNative ToNative() {
      return new RegisteredAppNative {
        AppInfo = AppInfo,
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
        ContainersCap = UIntPtr.Zero
      };
    }
  }

  internal struct RegisteredAppNative {
    public AppExchangeInfo AppInfo;
    public IntPtr ContainersPtr;
    public UIntPtr ContainersLen;

    // ReSharper disable once NotAccessedField.Compiler
    public UIntPtr ContainersCap;

    // ReSharper disable once UnusedMember.Global
    internal void Free() {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }
}
