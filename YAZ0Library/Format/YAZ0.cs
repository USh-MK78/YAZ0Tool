using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace YAZ0Library.Format
{
    public class YAZ0
    {
        public char[] YAZ0Header { get; set; }
        public uint UncompressedSize { get; set; }
        public uint UnknownData { get; set; } //Always 0 in Mario Kart Wii
        public uint UnknownData2 { get; set; }
        public byte[] YAZ0_DataArray { get; set; }

        public void ReadYAZ0(BinaryReader br, EndianConvert.Endian endian)
        {
            YAZ0Header = br.ReadChars(4);
            if (new string(YAZ0Header) != "Yaz0") throw new Exception("Error : YAZ0");

            EndianConvert endianConvert = new EndianConvert(endian);

            UncompressedSize = BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData = BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData2 = BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            YAZ0_DataArray = new byte[br.BaseStream.Length - 16];

            for (int i = 0; i < YAZ0_DataArray.Length; i++)
            {
                YAZ0_DataArray[i] = br.ReadByte();
            }
        }

        public void WriteYAZ0(BinaryWriter bw, EndianConvert.Endian endian)
        {
            bw.Write(YAZ0Header);

            EndianConvert endianConvert = new EndianConvert(endian);

            bw.Write(endianConvert.Convert(BitConverter.GetBytes(UncompressedSize)));
            bw.Write(endianConvert.Convert(BitConverter.GetBytes(UnknownData)));
            bw.Write(endianConvert.Convert(BitConverter.GetBytes(UnknownData2)));
            bw.Write(YAZ0_DataArray);
        }

        public enum DataTarget
        {
            CompressData = 0,
            NoCompressData = 1
        }

        /// <summary>
        /// Initialize YAZ0
        /// </summary>
        /// <param name="DataSize"></param>
        /// <param name="Data">byte[]</param>
        /// <param name="dataTarget">byte[] Data Type</param>
        /// <param name="CompLevel">Compress level</param>
        public YAZ0(uint DataSize, byte[] Data, DataTarget dataTarget, int SearchRange, int CompLevel)
        {
            YAZ0Header = "Yaz0".ToCharArray();
            UncompressedSize = DataSize;
            UnknownData = 0;
            UnknownData2 = 0;

            if (dataTarget == DataTarget.CompressData) YAZ0_DataArray = Data;
            else if (dataTarget == DataTarget.NoCompressData)
            {
                YAZ0_DataArray = Converter.Compress(Data, SearchRange, CompLevel);
            }
        }

        public YAZ0()
        {
            YAZ0Header = "Yaz0".ToCharArray();
            UncompressedSize = 0;
            UnknownData = 0;
            UnknownData2 = 0;
            YAZ0_DataArray = new List<byte>().ToArray();
        }
    }

    /// <summary>
    /// YAZ0 data I/O
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// Compress YAZ0
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Range">検索範囲 (既定は [0x400(1024byte)] )</param>
        /// <param name="level">Compress level, 1 - 9 or Zero</param>
        /// <returns>YAZ0 byte[] Data</returns>
        public static unsafe byte[] Compress(byte[] Data, int Range = 0x400, int level = 1)
        {
            byte* dataptr = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(Data, 0);

            byte[] result = new byte[Data.Length + Data.Length / 8];
            byte* resultptr = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(result, 0);
            int length = Data.Length;
            int dstoffs = 0;
            int Offs = 0;
            while (true)
            {
                int headeroffs = dstoffs++;
                resultptr++;
                byte header = 0;
                for (int i = 0; i < 8; i++)
                {
                    int comp = 0;
                    int back = 1;
                    int nr = 2;
                    {
                        byte* ptr = dataptr - 1;
                        int maxnum = 0x111;
                        if (length - Offs < maxnum) maxnum = length - Offs;

                        int SearchRange = 0;
                        if (level == 0) SearchRange = Range; //0x1000;
                        else if (level <= 9) SearchRange = 0x10e0 * level;

                        //int SearchRange = 0x400;//0x1000;
                        if (Offs < SearchRange) SearchRange = Offs;
                        SearchRange = (int)dataptr - SearchRange;
                        int tmpnr;
                        while (SearchRange <= (int)ptr)
                        {
                            if (*(ushort*)ptr == *(ushort*)dataptr && ptr[2] == dataptr[2])
                            {
                                tmpnr = 3;
                                while (tmpnr < maxnum && ptr[tmpnr] == dataptr[tmpnr]) tmpnr++;
                                if (tmpnr > nr)
                                {
                                    if (Offs + tmpnr > length)
                                    {
                                        nr = length - Offs;
                                        back = (int)(dataptr - ptr);
                                        break;
                                    }
                                    nr = tmpnr;
                                    back = (int)(dataptr - ptr);
                                    if (nr == maxnum) break;
                                }
                            }
                            --ptr;
                        }
                    }
                    if (nr > 2)
                    {
                        Offs += nr;
                        dataptr += nr;
                        if (nr >= 0x12)
                        {
                            *resultptr++ = (byte)(((back - 1) >> 8) & 0xF);
                            *resultptr++ = (byte)((back - 1) & 0xFF);
                            *resultptr++ = (byte)((nr - 0x12) & 0xFF);
                            dstoffs += 3;
                        }
                        else
                        {
                            *resultptr++ = (byte)((((back - 1) >> 8) & 0xF) | (((nr - 2) & 0xF) << 4));
                            *resultptr++ = (byte)((back - 1) & 0xFF);
                            dstoffs += 2;
                        }
                        comp = 1;
                    }
                    else
                    {
                        *resultptr++ = *dataptr++;
                        dstoffs++;
                        Offs++;
                    }
                    header = (byte)((header << 1) | ((comp == 1) ? 0 : 1));
                    if (Offs >= length)
                    {
                        header = (byte)(header << (7 - i));
                        break;
                    }
                }
                result[headeroffs] = header;
                if (Offs >= length) break;
            }
            while ((dstoffs % 4) != 0) dstoffs++;
            byte[] realresult = new byte[dstoffs];
            Array.Copy(result, realresult, dstoffs);

            return realresult;
        }

        /// <summary>
        /// Decompress YAZ0
        /// </summary>
        /// <param name="yaz0"></param>
        /// <returns>Decompress Data</returns>
        public static byte[] Decompress(YAZ0 yaz0)
        {
            uint dataSize = yaz0.UncompressedSize;
            byte[] Result = new byte[(uint)dataSize];

            int dstoffs = 0;
            int Offs = 0;
            while (true)
            {
                byte header = yaz0.YAZ0_DataArray[Offs++];
                for (int i = 0; i < 8; i++)
                {
                    if ((header & 0x80) != 0) Result[dstoffs++] = yaz0.YAZ0_DataArray[Offs++];
                    else
                    {
                        byte b = yaz0.YAZ0_DataArray[Offs++];
                        int offs = ((b & 0xF) << 8 | yaz0.YAZ0_DataArray[Offs++]) + 1;
                        int length = (b >> 4) + 2;
                        if (length == 2) length = yaz0.YAZ0_DataArray[Offs++] + 0x12;
                        for (int j = 0; j < length; j++)
                        {
                            Result[dstoffs] = Result[dstoffs - offs];
                            dstoffs++;
                        }
                    }
                    if (dstoffs >= dataSize) return Result;
                    header <<= 1;
                }
            }
        }
    }
}
