using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;

namespace SafeApp.Utilities {
  [PublicAPI]
  public static class Helpers {
    public static T HandlePtrToType<T>(this IntPtr ptr, bool freePtr = true) {
      var cbPtr = GCHandle.FromIntPtr(ptr);
      var action = (T)cbPtr.Target;
      if (freePtr) {
        cbPtr.Free();
      }

      return action;
    }

    public static IntPtr StructToPtr<T>(T obj) {
      var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
      Marshal.StructureToPtr(obj, ptr, false);
      return ptr;
    }

    public static IntPtr ToHandlePtr(this object obj) {
      return GCHandle.ToIntPtr(GCHandle.Alloc(obj));
    }

    public static IntPtr ToIntPtr<T>(this List<T> list) {
      var structSize = Marshal.SizeOf<T>();
      var ptr = Marshal.AllocHGlobal(structSize * list.Count);
      for (var i = 0; i < list.Count; ++i) {
        Marshal.StructureToPtr(list[i], ptr + structSize * i, false);
      }

      return ptr;
    }

    public static List<T> ToList<T>(this IntPtr ptr, IntPtr length) {
      return Enumerable.Range(0, (int)length).Select(i => Marshal.PtrToStructure<T>(IntPtr.Add(ptr, Marshal.SizeOf<T>() * i))).ToList();
    }

    #region Encoding Extensions

    public static string ToUtfString(this List<byte> input) {
      var ba = input.ToArray();
      return Encoding.UTF8.GetString(ba, 0, ba.Length);
    }

    public static List<byte> ToUtfBytes(this string input) {
      var byteArray = Encoding.UTF8.GetBytes(input);
      return byteArray.ToList();
    }

    public static string ToHexString(this List<byte> byteList) {
      var ba = byteList.ToArray();
      var hex = BitConverter.ToString(ba);
      return hex.Replace("-", "").ToLower();
    }

    public static List<byte> ToHexBytes(this string hex) {
      var numberChars = hex.Length;
      var bytes = new byte[numberChars / 2];
      for (var i = 0; i < numberChars; i += 2) {
        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
      }

      return bytes.ToList();
    }

    public static string PrintByteArray(List<byte> bytes) {
      var sb = new StringBuilder("new byte[] { ");
      foreach (var b in bytes) {
        sb.Append(b + ", ");
      }

      sb.Append("}");
      return sb.ToString();
    }

    #endregion
  }
}
