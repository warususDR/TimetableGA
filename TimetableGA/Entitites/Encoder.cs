using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableGA.Entitites
{
    internal class Encoder<T> where T : notnull  
    {
        private static string GetByteString(int intValue)
        {
            BitArray b = new BitArray(new int[] { intValue });
            bool[] bits = new bool[b.Count];
            b.CopyTo(bits, 0);
            byte[] bytes = bits.Select(bit => (byte)(bit ? 1 : 0)).ToArray();
            Array.Reverse(bytes);
            string result = BitConverter.ToString(bytes).Replace("-", "");
            return result;
        }

        public static Dictionary<string, T> EncodeList(List<T> list)
        {
            var res = new Dictionary<string, T>();
            for (int i = 0; i < list.Count; i++)
            {
                res.Add(GetByteString(i), list[i]);
            }
            return res;
        }
    }
}
