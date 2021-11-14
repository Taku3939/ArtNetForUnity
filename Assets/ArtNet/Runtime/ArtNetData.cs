/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */

using System;
using UnityEditorInternal;
using UnityEngine;

namespace ArtNet.Runtime
{
    public readonly struct ArtNetData
    {
        #region public filed

        public readonly ArtNetOpCode OpCode;
        public readonly int Sequence, Physical, Universe;
        public readonly int[] Channels;
        public readonly int ProtocolVersionHi, ProtocolVersionLo, LengthHi, LengthLo;

        #endregion

        #region public method

        /// <summary>
        /// コンストラクタ
        /// Art-Net Structure
        /// 0 ～ 7 : id 「Art-Net」
        /// 8 ～ 9 : Opcode ArtDMX (0x5000) little endian
        /// 10 : Protocol Version Hi (0)
        /// 11 : Protocol Version Lo (14)
        /// 12 : Sequence (255周期)
        /// 13 : Physical
        /// 14 ～ 15 : Universe little endian
        /// 16 : Length Hi
        /// 17 : Length Lo (2 to 512, even)	
        /// 18 ～ 530 : Data
        /// </summary>
        /// <param name="buf">受信したbyte列</param>
        public ArtNetData(byte[] buf)
        {
            var buffer = new byte[530];
            Buffer.BlockCopy(buf, 0, buffer, 0, buf.Length);
            OpCode = (ArtNetOpCode)BitConverter.ToInt16(new byte[2] { buffer[8], buffer[9] }, 0);
            ProtocolVersionHi = buffer[10];
            ProtocolVersionLo = buffer[11];
            Sequence = buffer[12];
            Physical = buffer[13];
            Universe = BitConverter.ToInt16(new byte[2] { buffer[14], buffer[15] }, 0);
            LengthHi = buffer[16];
            LengthLo = buffer[17];
            Channels = new int[512];
            for (int i = 18; i < buffer.Length; i++)
                Channels[i - 18] = buffer[i];
        }

        public static bool IsArtNet(byte[] buffer)
            => buffer[0] == 'A' &&
               buffer[1] == 'r' &&
               buffer[2] == 't' &&
               buffer[3] == '-' &&
               buffer[4] == 'N' &&
               buffer[5] == 'e' &&
               buffer[6] == 't' &&
               buffer[7] == 0x00;

        public void Logger()
        {
            string str = "";
            str += $"Opcode : {this.OpCode}\n";
            str += $"ProtocolVersionHi : {ProtocolVersionHi}\n";
            str += $"ProtocolVersionLo : {ProtocolVersionLo}\n";
            str += $"Sequence : {Sequence}\n";
            str += $"Physical : {Physical}\n";
            str += $"Universe : {Universe}\n";
            str += $"LengthHi : {LengthHi}\n";
            str += $"LengthLo : {LengthLo}\n";
            foreach (var t in Channels) str += t + "\n";
            Debug.Log(str);
        }

        #endregion
    }
}