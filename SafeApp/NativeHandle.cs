using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace SafeApp
{
    /// <summary>
    /// Handle to a complex object held in the SafeApp object cache.
    /// Complex objects are not moved across FFI boundary, instead a reference to the object is used.
    /// The handle acts as a reference to the object for performing operations.
    /// </summary>
    public class NativeHandle : IDisposable
    {
        /// <summary>
        /// NativeHandle to insert permissions for all users.
        /// </summary>
        [PublicAPI]
        public static readonly NativeHandle AnyOne = new NativeHandle(null, 0, null);

        /// <summary>
        /// NativeHandle representing zero Mutable Data entries.
        /// </summary>
        public static readonly NativeHandle EmptyMDataEntries = AnyOne;

        /// <summary>
        /// NativeHandle representing zero Mutable Data Permissions.
        /// </summary>
        public static readonly NativeHandle EmptyMDataPermissions = AnyOne;

        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// NativeHandle with null reference.
        /// </summary>
        [Obsolete("This property is obsolete.", false)]
        public static readonly NativeHandle Zero = new NativeHandle(null, 0, null);

        private readonly Func<ulong, Task> _disposer;
        private readonly ulong _handle;
        private readonly SafeAppPtr _safeAppPtr;

        internal NativeHandle(SafeAppPtr safeAppPtr, ulong handle, Func<ulong, Task> disposer)
        {
            _safeAppPtr = safeAppPtr;
            _disposer = disposer;
            _handle = handle;
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by developers.
        /// </summary>
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Class destructor
        /// </summary>
        ~NativeHandle()
        {
            ReleaseUnmanagedResources();
        }

        /// <summary>
        /// Convert NativeHandle to ulong.
        /// </summary>
        /// <param name="obj">NativeHandle instance.</param>
        public static implicit operator ulong(NativeHandle obj)
        {
            return obj._handle;
        }

        private void ReleaseUnmanagedResources()
        {
            if (_safeAppPtr?.Value != IntPtr.Zero)
            {
                _disposer?.Invoke(_handle).GetAwaiter().GetResult();
            }
        }
    }
}
