using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace SafeApp.Utilities {
  [PublicAPI]
  public class FfiException : Exception {
    public readonly int ErrorCode;

    internal FfiException(int code, string description) : base($"Error Code: {code}. Description: {description}") {
      ErrorCode = code;
    }
  }

  [PublicAPI]
  public struct FfiResult {
    public int ErrorCode;
    [MarshalAs(UnmanagedType.LPStr)] public string Description;

    public FfiException ToException() {
      return new FfiException(ErrorCode, Description);
    }
  }

  // ReSharper disable once ClassNeverInstantiated.Global
  internal class BindingUtils {
    private static void CompleteTask<T>(TaskCompletionSource<T> tcs, FfiResult result, Func<T> argFunc) {
      if (result.ErrorCode != 0) {
        tcs.SetException(result.ToException());
      } else {
        tcs.SetResult(argFunc());
      }
    }

    public static void CompleteTask<T>(IntPtr userData, FfiResult result, Func<T> argFunc) {
      var tcs = FromHandlePtr<TaskCompletionSource<T>>(userData);
      CompleteTask(tcs, result, argFunc);
    }

    public static void CompleteTask(IntPtr userData, FfiResult result) {
      CompleteTask(userData, result, () => true);
    }

    public static IntPtr CopyFromByteList(List<byte> list) {
      if (list == null || list.Count == 0) {
        return IntPtr.Zero;
      }

      var array = list.ToArray();
      var size = Marshal.SizeOf(array[0]) * array.Length;
      var ptr = Marshal.AllocHGlobal(size);
      Marshal.Copy(array, 0, ptr, array.Length);

      return ptr;
    }

    public static IntPtr CopyFromObjectList<T>(List<T> list) {
      if (list == null || list.Count == 0) {
        return IntPtr.Zero;
      }

      var size = Marshal.SizeOf(list[0]) * list.Count;
      var ptr = Marshal.AllocHGlobal(size);
      for (var i = 0; i < list.Count; ++i) {
        Marshal.StructureToPtr(list[i], IntPtr.Add(ptr, Marshal.SizeOf<T>() * i), false);
      }

      return ptr;
    }

    public static byte[] CopyToByteArray(IntPtr ptr, int len) {
      var array = new byte[len];
      if (len > 0) {
        Marshal.Copy(ptr, array, 0, len);
      }

      return array;
    }

    public static List<byte> CopyToByteList(IntPtr ptr, int len) {
      return new List<byte>(CopyToByteArray(ptr, len));
    }

    public static List<T> CopyToObjectList<T>(IntPtr ptr, int len) {
      var list = new List<T>(len);
      for (var i = 0; i < len; ++i) {
        list.Add(Marshal.PtrToStructure<T>(IntPtr.Add(ptr, Marshal.SizeOf<T>() * i)));
      }

      return list;
    }

    // ReSharper disable once RedundantAssignment
    public static void FreeList(ref IntPtr ptr, ref UIntPtr len) {
      if (ptr != IntPtr.Zero) {
        Marshal.FreeHGlobal(ptr);
      }

      ptr = IntPtr.Zero;
      len = UIntPtr.Zero;
    }

    public static T FromHandlePtr<T>(IntPtr ptr, bool free = true) {
      var handle = GCHandle.FromIntPtr(ptr);
      var result = (T)handle.Target;

      if (free) {
        handle.Free();
      }

      return result;
    }

    public static (Task<T>, IntPtr) PrepareTask<T>() {
      var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
      var userData = ToHandlePtr(tcs);

      return (tcs.Task, userData);
    }

    public static (Task, IntPtr) PrepareTask() {
      return PrepareTask<bool>();
    }

    public static IntPtr ToHandlePtr<T>(T obj) {
      return GCHandle.ToIntPtr(GCHandle.Alloc(obj));
    }
  }
}
