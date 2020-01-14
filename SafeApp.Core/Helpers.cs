using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SafeApp.Core
{
    /// <summary>
    /// Containers SafeApp helper functions.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Returns a GC object handle for a IntPtr.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptr"></param>
        /// <param name="freePtr"></param>
        /// <returns></returns>
        public static T HandlePtrToType<T>(this IntPtr ptr, bool freePtr = true)
        {
            var cbPtr = GCHandle.FromIntPtr(ptr);
            var action = (T)cbPtr.Target;
            if (freePtr)
            {
                cbPtr.Free();
            }

            return action;
        }

        /// <summary>
        /// Allocate memory to hold the struct.
        /// </summary>
        /// <typeparam name="T">Structure to be marshaled.</typeparam>
        /// <param name="obj"></param>
        /// <returns>Pointer to the structure in unmanaged memory.</returns>
        public static IntPtr StructToPtr<T>(T obj)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        /// <summary>
        /// Allocate the object into unmanaged memory and
        /// returns the IntPtr object.
        /// </summary>
        /// <param name="obj">Object to allocate.</param>
        /// <returns>An IntPtr object representing a GChandle object.</returns>
        public static IntPtr ToHandlePtr(this object obj)
        {
            return GCHandle.ToIntPtr(GCHandle.Alloc(obj));
        }

        /// <summary>
        /// Allocate the object into unmanaged memory and
        /// returns the IntPtr object.
        /// </summary>
        /// <typeparam name="T">List type.</typeparam>
        /// <param name="list">List of objects to marshal.</param>
        /// <returns>An IntPtr object representing a GChandle object</returns>
        public static IntPtr ToIntPtr<T>(this List<T> list)
        {
            var structSize = Marshal.SizeOf<T>();
            var ptr = Marshal.AllocHGlobal(structSize * list.Count);
            for (var i = 0; i < list.Count; ++i)
            {
                // ReSharper disable once ArrangeRedundantParentheses
                Marshal.StructureToPtr(list[i], ptr + (structSize * i), false);
            }

            return ptr;
        }

        /// <summary>
        /// Fetch an object list from IntPtr object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="ptr">IntPtr to fetch.</param>
        /// <param name="length">Length</param>
        /// <returns>list of objects.</returns>
        public static List<T> ToList<T>(this IntPtr ptr, IntPtr length)
        {
            return Enumerable.Range(0, (int)length).Select(i => Marshal.PtrToStructure<T>(IntPtr.Add(ptr, Marshal.SizeOf<T>() * i))).
                ToList();
        }

        #region Encoding Extensions

        /// <summary>
        /// Convert byte list into string.
        /// </summary>
        /// <param name="input">byte list to be converted.</param>
        /// <returns>new utf-8 string.</returns>
        public static string ToUtfString(this byte[] input)
        {
            var ba = input.ToArray();
            return Encoding.UTF8.GetString(ba, 0, ba.Length);
        }

        /// <summary>
        /// Converts a string to utf-8 bytes.
        /// </summary>
        /// <param name="input">string to be converted.</param>
        /// <returns>a byte list</returns>
        public static byte[] ToUtfBytes(this string input)
        {
            var byteArray = Encoding.UTF8.GetBytes(input);
            return byteArray;
        }

        /// <summary>
        /// Convert byte list into string.
        /// </summary>
        /// <param name="byteList">byte list.</param>
        /// <returns>new String.</returns>
        public static string ToHexString(this byte[] byteList)
        {
            var ba = byteList.ToArray();
            var hex = BitConverter.ToString(ba);
            return hex.Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Convert string into hexadecimal bytes.
        /// </summary>
        /// <param name="hex">String to be converted.</param>
        /// <returns>List of bytes in hexadecimal format.</returns>
        public static byte[] ToHexBytes(this string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// Converts byte list into string using StringBuilder.
        /// </summary>
        /// <param name="bytes">byte list to be converted.</param>
        /// <returns>new String containing byte array.</returns>
        public static string PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }

            sb.Append("}");
            return sb.ToString();
        }

        #endregion
    }
}
