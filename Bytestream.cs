using System;

namespace LuaAssemblyPrinter
{
    /// <summary>
    /// Class for reading binary data
    /// </summary>
    class Bytestream
    {
        //Data to read
        private readonly byte[] Data;

        //Current read location
        private int Iterator;

        /// <summary>
        /// Creates a new bytestream from a byte array
        /// </summary>
        /// <param name="data">Data to read</param>
        public Bytestream(byte[] data)
        {
            Data = data;
            Iterator = 0;
        }

        /// <summary>
        /// Reads 4 bytes as an unsigned integer
        /// </summary>
        /// <returns>Unsigned integer</returns>
        public uint ReadUns() => BitConverter.ToUInt32(Data, Seek(4));

        /// <summary>
        /// Reads 4 bytes as a signed integer
        /// </summary>
        /// <returns>Signed integer</returns>
        public int ReadInt() => BitConverter.ToInt32(Data, Seek(4));

        /// <summary>
        /// Reads 8 bytes as an unsigned integer
        /// </summary>
        /// <returns>Unsigned integer</returns>
        public ulong ReadULong() => BitConverter.ToUInt64(Data, Seek(8));

        /// <summary>
        /// Reads 8 bytes as a signed integer
        /// </summary>
        /// <returns>Signed integer</returns>
        public long ReadLong() => BitConverter.ToInt64(Data, Seek(8));

        /// <summary>
        /// Reads 8 bytes as a float
        /// </summary>
        /// <returns>Float</returns>
        public double ReadDouble() => BitConverter.ToDouble(Data, Seek(8));

        /// <summary>
        /// Reads a single byte
        /// </summary>
        /// <returns>Byte</returns>
        public byte ReadByte() => Data[Seek(1)];

        /// <summary>
        /// Reads a string
        /// </summary>
        /// <returns>String</returns>
        public string ReadString()
        {
            string val = "";
            //First get the string length
            int size = ReadByte() & 0xFF;
            if (size == 0)
                return val;
            if (size == 0xFF)
                size = (int)ReadLong();
            //Read n bytes to get the string
            for (int i = 0; i < size - 1; i++)
                val += (char)ReadByte();
            return val;
        }

        /// <summary>
        /// Increments the read position by n
        /// </summary>
        /// <param name="bytes">Value to increment</param>
        /// <returns>Old value</returns>
        private int Seek(int bytes)
        {
            int it = Iterator;
            Iterator += bytes;
            return it;
        }
    }
}