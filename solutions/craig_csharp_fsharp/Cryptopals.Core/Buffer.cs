using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace Cryptopals.Core
{

    public class Buffer : IEnumerable<byte>
    {
        readonly byte[] _value;
        public readonly int Length;


        public Buffer(byte[] value) {
            _value = value;
            Length = value.Length;
        }

        public byte this[int index] => _value[index];


        public static Buffer FromHex(string hex) 
            => new Buffer(SoapHexBinary.Parse(hex).Value);

        public string ToHex() 
            => BitConverter.ToString(_value).Replace("-", string.Empty).ToLower();
        


        public static Buffer FromBase64(string base64) 
            => new Buffer(Convert.FromBase64String(base64));

        public string ToBase64() 
            => Convert.ToBase64String(_value);
        


        public static Buffer FromText(string text) 
            => new Buffer(Encoding.ASCII.GetBytes(text));

        public string ToText() 
            => Encoding.ASCII.GetString(_value);



        public static implicit operator byte[] (Buffer buffer)
            => buffer._value;



        public (int Score, byte Byte, string Text) EnglishScore()
        {
            (int Score, byte Byte, string Text) best = (0, 0, "");
            
            for (var singleByte = 0; singleByte < 256; singleByte++) {
                var text = (this ^ (byte)singleByte).ToText();
                var score = text.Count(c => char.IsLetter(c) || char.IsWhiteSpace(c));

                if (score <= best.Score) continue;

                best = (score, (byte)singleByte, text);
            }

            return best;
        }


        static Buffer Xor(byte[] left, byte[] right)
        {
            var newBytes = new byte[left.Length];

            for (var index = 0; index < left.Length; index++) {
                newBytes[index] = (byte)(left[index] ^ right[index % right.Length]);
            }

            return new Buffer(newBytes);
        }

        public static Buffer operator ^ (Buffer left, Buffer right) 
            => Xor(left, right);

        public static Buffer operator ^ (Buffer left, string right) 
            => Xor(left, FromText(right)._value);
        
        public static Buffer operator ^ (Buffer left, byte right) 
            => Xor(left, new[] { right });




        public IEnumerator<byte> GetEnumerator() 
            => ((IEnumerable<byte>) _value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();



        public int NormalisedHammingDistance(Buffer right) => HammingDistance(right) / Length;
        public int HammingDistance(Buffer right) 
            => (this ^ right).Sum(current => (current >> 0 & 1) +
                                             (current >> 1 & 1) +
                                             (current >> 2 & 1) +
                                             (current >> 3 & 1) +
                                             (current >> 4 & 1) +
                                             (current >> 5 & 1) +
                                             (current >> 6 & 1) +
                                             (current >> 7 & 1));


        public Buffer GetBlock(int keysize, int index) {
            var bytes = new byte[keysize];
            System.Buffer.BlockCopy(this, keysize * index, bytes, 0, keysize);
            return new Buffer(bytes);
        }
    }
}
