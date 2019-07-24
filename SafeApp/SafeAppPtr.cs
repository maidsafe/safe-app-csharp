using System;

namespace SafeApp
{
    internal class SafeAppPtr
    {
        public IntPtr Value { get; private set; }

        public SafeAppPtr(IntPtr appPtr)
        {
            Value = appPtr;
        }

        public SafeAppPtr()
            : this(IntPtr.Zero)
        {
        }

        public static implicit operator IntPtr(SafeAppPtr obj)
        {
            return obj.Value;
        }

        public void Clear()
        {
            Value = IntPtr.Zero;
        }
    }
}
