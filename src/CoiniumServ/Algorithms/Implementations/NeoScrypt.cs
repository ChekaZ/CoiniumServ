﻿#region License
// 
//     MIT License
//
//     CoiniumServ - Crypto Currency Mining Pool Server Software
//     Copyright (C) 2013 - 2017, CoiniumServ Project
//     Hüseyin Uslu, shalafiraistlin at gmail dot com
//     https://github.com/bonesoul/CoiniumServ
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// 
#endregion

using System;
using CryptSharp.Utility;
using System.Runtime.InteropServices;

namespace CoiniumServ.Algorithms.Implementations
{
    public sealed class NeoScrypt : IHashAlgorithm
    {

        [DllImport("NeoScrypt.dll", EntryPoint = "neoscrypt_export", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int neoscrypt(byte* input, byte* output, uint inputLength, uint profile);

        public UInt32 Multiplier { get; private set; }

        /// <summary>
        /// N parameter - CPU/memory cost parameter.
        /// </summary>
        private readonly int _n;

        /// <summary>
        /// R parameter - block size.
        /// </summary>
        private readonly int _r;

        /// <summary>
        /// P - parallelization parameter -  a large value of p can increase computational 
        /// cost of scrypt without increasing the memory usage.
        /// </summary>
        private readonly int _p;

        public NeoScrypt()
        {
            _n = 128;
            _r = 2;
            _p = 1;

            Multiplier = (UInt32) Math.Pow(2, 16);
        }

        public unsafe byte[] Hash(byte[] input)
        {
            var result = new byte[32];

            fixed (byte* inputb = input)
            {
                fixed (byte* outputb = input)
                {
                    neoscrypt(inputb, outputb, (uint)input.Length, 0x1);
                }
            }
            return result;
        }
    }
}
