using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator {
    public static class CustomExtensions {
        public static int Size(this Range range) {
            return range.End.Value - range.Start.Value;
        }
        //public static void Copy(this Array sourceArray, Range sourceRange, Array destinationArray, Range destinationRange) {
        //    if (sourceRange.Size() != destinationRange.Size()) {
        //        throw new ArgumentException("Range size mismatch!");
        //    }
        //    var (sourceOffset, sourceLength) = sourceRange.GetOffsetAndLength(sourceArray.Length);
        //    var (destOffset, destlength) = sourceRange.GetOffsetAndLength(sourceArray.Length);
        //    Array.Copy(sourceArray, sourceOffset, destinationArray, destOffset, destlength);
        //}

        public static void Copy(this Array destinationArray, Range destinationRange, Array sourceArray, Range sourceRange) {
            if (sourceRange.Size() != destinationRange.Size()) {
                throw new ArgumentException("Range size mismatch!");
            }
            var (sourceOffset, sourceLength) = sourceRange.GetOffsetAndLength(sourceArray.Length);
            var (destOffset, destlength) = sourceRange.GetOffsetAndLength(sourceArray.Length);
            Array.Copy(sourceArray, sourceOffset, destinationArray, destOffset, destlength);
        }
    }
}
