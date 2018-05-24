using System;
using System.Threading.Tasks;

namespace SafeApp {
  public class NativeHandle : IDisposable {
    public static readonly NativeHandle Zero = new NativeHandle(null, 0, null);

    private readonly Func<ulong, Task> _disposer;
    private readonly ulong _handle;
    private readonly SafeAppPtr _safeAppPtr;

    internal NativeHandle(SafeAppPtr safeAppPtr, ulong handle, Func<ulong, Task> disposer) {
      _safeAppPtr = safeAppPtr;
      _disposer = disposer;
      _handle = handle;
    }

    public void Dispose() {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~NativeHandle() {
      ReleaseUnmanagedResources();
    }

    public static implicit operator ulong(NativeHandle obj) {
      return obj._handle;
    }

    private void ReleaseUnmanagedResources() {
      if (_safeAppPtr?.Value != IntPtr.Zero) {
        _disposer?.Invoke(_handle).GetAwaiter().GetResult();
      }
    }
  }
}
