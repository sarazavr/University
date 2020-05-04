using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UnivirsityModels
{
    public class Utils
    {
        public static object Clone(object element)
        {
            return element is ICloneable ? ((ICloneable)element).Clone() : element;
        }
    }

    public static class Extension
    {
        public static byte[] GetBytesGeneric<T>(this T input)
        {
            int size = Marshal.SizeOf(typeof(T));
            var result = new byte[size];
            var gcHandle = GCHandle.Alloc(input, GCHandleType.Pinned);
            Marshal.Copy(gcHandle.AddrOfPinnedObject(), result, 0, size);
            gcHandle.Free();
            return result;
        }
    }
}
