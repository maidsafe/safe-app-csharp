using System;

namespace SafeApp {
  internal class SafeAppPtr {
    public IntPtr Value { get; private set; }

    public SafeAppPtr(IntPtr appPtr) {
      Value = appPtr;
    }

    public SafeAppPtr() : this(IntPtr.Zero) { }

    public void Clear() {
      Value = IntPtr.Zero;
    }

    public static implicit operator IntPtr(SafeAppPtr obj) {
      if (obj.Value == IntPtr.Zero) {
        throw new ArgumentNullException(nameof(SafeAppPtr));
      }

      return obj.Value;
    }
  }
}
