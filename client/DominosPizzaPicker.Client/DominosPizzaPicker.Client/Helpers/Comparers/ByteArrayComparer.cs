using System;
using System.Collections.Generic;

namespace DominosPizzaPicker.Client.Helpers.Comparers
{
    /// <summary>
    /// Used to compare 2 byte arrays
    /// </summary>
    public class ByteArrayComparer : IComparer<byte[]>
    {
        public int Compare(byte[] x, byte[] y)
        {
            if (x == null && y != null)
                return 1;

            if (y == null && x != null)
                return -1;

            if (x == null && y == null)
                return 0;


            var minCount = Math.Min(x.Length, y.Length);

            for (var i = 0; i < minCount; i++)
            {
                var result = x[i].CompareTo(y[i]);
                if (result != 0)
                    return result;
            }

            return x.Length.CompareTo(y.Length);
        }
    }
}