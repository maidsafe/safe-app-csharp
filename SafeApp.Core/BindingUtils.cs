using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SafeApp.Core
{
#pragma warning disable SA1401 // Fields should be private

    /// <summary>
    /// FFI exception
    /// </summary>
    public class FfiException : Exception
    {
        /// <summary>
        /// Unique error code
        /// </summary>
        public readonly int ErrorCode;

        internal FfiException(int code, string description)
            : base($"Error Code: {code}. Description: {description}")
        {
            ErrorCode = code;
        }
    }

    /// <summary>
    /// FFI result wrapper.
    /// </summary>
    public struct FfiResult
    {
        /// <summary>
        /// Unique error code.
        /// </summary>
        public int ErrorCode;

        /// <summary>
        /// Error description.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Description;

        /// <summary>
        /// Convert FfiResult to FfiException.
        /// </summary>
        /// <returns>New FfiException instance.</returns>
        public FfiException ToException()
        {
            return new FfiException(ErrorCode, Description);
        }
    }

    internal class BindingUtils
    {
        private static void CompleteTask<T>(TaskCompletionSource<T> tcs, FfiResult result, Func<T> argFunc)
        {
            if (result.ErrorCode != 0)
            {
                tcs.SetException(result.ToException());
            }
            else
            {
                tcs.SetResult(argFunc());
            }
        }

        public static void CompleteTask<T>(IntPtr userData, FfiResult result, Func<T> argFunc)
        {
            var tcs = FromHandlePtr<TaskCompletionSource<T>>(userData);
            CompleteTask(tcs, result, argFunc);
        }

        public static void CompleteTask(IntPtr userData, FfiResult result)
        {
            CompleteTask(userData, result, () => true);
        }

        public static IntPtr CopyFromByteList(List<byte> list)
        {
            if (list == null || list.Count == 0)
            {
                return IntPtr.Zero;
            }

            var array = list.ToArray();
            var size = Marshal.SizeOf(array[0]) * array.Length;
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, array.Length);

            return ptr;
        }

        public static IntPtr CopyFromByteArray(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                return IntPtr.Zero;
            }

            var size = Marshal.SizeOf(array[0]) * array.Length;
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, array.Length);

            return ptr;
        }

        public static IntPtr CopyFromObjectList<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return IntPtr.Zero;
            }

            var size = Marshal.SizeOf(list[0]) * list.Count;
            var ptr = Marshal.AllocHGlobal(size);
            for (var i = 0; i < list.Count; ++i)
            {
                Marshal.StructureToPtr(list[i], IntPtr.Add(ptr, Marshal.SizeOf<T>() * i), false);
            }

            return ptr;
        }

        public static byte[] CopyToByteArray(IntPtr ptr, int len)
        {
            var array = new byte[len];
            if (len > 0)
            {
                Marshal.Copy(ptr, array, 0, len);
            }

            return array;
        }

        public static List<byte> CopyToByteList(IntPtr ptr, int len)
        {
            return new List<byte>(CopyToByteArray(ptr, len));
        }

        public static List<T> CopyToObjectList<T>(IntPtr ptr, int len)
        {
            var list = new List<T>(len);
            for (var i = 0; i < len; ++i)
            {
                list.Add(Marshal.PtrToStructure<T>(IntPtr.Add(ptr, Marshal.SizeOf<T>() * i)));
            }

            return list;
        }

        public static List<string> CopyToStringList(IntPtr ptr, int len)
        {
            if (ptr == IntPtr.Zero || len <= 0)
                return null;

            var list = new List<string>();

            for (int count = 0; count < len && Marshal.ReadIntPtr(ptr, count * IntPtr.Size) != IntPtr.Zero; ++count)
            {
                list.Add(PtrToString(Marshal.ReadIntPtr(ptr, count * IntPtr.Size)));
            }
            return list;
        }

        internal static string PtrToString(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                return string.Empty;

            var data = new List<byte>();
            var offset = 0;

            while (true)
            {
                var ch = Marshal.ReadByte(handle, offset++);

                if (ch == 0)
                    break;

                data.Add(ch);
            }

            return Encoding.UTF8.GetString(data.ToArray());
        }

        public static IntPtr CopyFromStringList(List<string> str)
        {
            var unmanagedPointer = Marshal.AllocHGlobal(str.Count * IntPtr.Size);
            var ptrList = StringArrayToIntPtrArray(str.ToArray());
            Marshal.Copy(ptrList, 0, unmanagedPointer, str.Count);
            return unmanagedPointer;
        }

        public static IntPtr[] StringArrayToIntPtrArray(string[] str)
        {
            int length = str.Length;
            IntPtr[] arr = new IntPtr[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = Utf8StringToIntPtr(str[i]);
            }
            return arr;
        }

        public static IntPtr Utf8StringToIntPtr(string str)
        {
            var len = Encoding.UTF8.GetByteCount(str);
            var buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, 0);
            var nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }

        public static void FreeList(ref IntPtr ptr, ref UIntPtr len)
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptr);
            }

            ptr = IntPtr.Zero;
            len = UIntPtr.Zero;
        }

        public static T FromHandlePtr<T>(IntPtr ptr, bool free = true)
        {
            var handle = GCHandle.FromIntPtr(ptr);
            var result = (T)handle.Target;

            if (free)
            {
                handle.Free();
            }

            return result;
        }

        public static (Task<T>, IntPtr) PrepareTask<T>()
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            var userData = ToHandlePtr(tcs);

            return (tcs.Task, userData);
        }

        public static (Task, IntPtr) PrepareTask()
        {
            return PrepareTask<bool>();
        }

        public static IntPtr ToHandlePtr<T>(T obj)
        {
            return GCHandle.ToIntPtr(GCHandle.Alloc(obj));
        }
    }
#pragma warning restore SA1401 // Fields should be private
}
