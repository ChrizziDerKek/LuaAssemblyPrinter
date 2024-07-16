namespace LuaAssemblyPrinter
{
    /// <summary>
    /// Lua file header
    /// </summary>
    class Header
    {
        //True if the header is valid
        public bool Valid { get; private set; }

        //Shows the reason why the header isn't valid if that's the case
        public string LastError { get; private set; }

        /// <summary>
        /// Creates a new header and tries to read its content, also performs validation checks
        /// </summary>
        /// <param name="data">File data to read</param>
        public Header(Bytestream data)
        {
            LastError = "";
            Valid = true;
            byte[] sig = new byte[] { 0x1B, (byte)'L', (byte)'u', (byte)'a' };
            foreach (byte s in sig)
                if (data.ReadByte() != s)
                    Fail("Invalid signature part 1");
            if (data.ReadByte() != 0x53)
                Fail("Invalid lua version");
            if (data.ReadByte() != 0)
                Fail("Invalid format");
            sig = new byte[] { 0x19, 0x93, (byte)'\r', (byte)'\n', 0x1A, (byte)'\n' };
            foreach (byte s in sig)
                if (data.ReadByte() != s)
                    Fail("Invalid signature part 2");
            if (data.ReadByte() != 4)
                Fail("Invalid int size");
            if (data.ReadByte() != 4)
                Fail("Invalid size_t size");
            if (data.ReadByte() != 4)
                Fail("Invalid opcode size");
            if (data.ReadByte() != 8)
                Fail("Invalid luaint size");
            if (data.ReadByte() != 8)
                Fail("Invalid luanumber size");
            if (data.ReadLong() != 22136)
                Fail("Failed luaint test");
            if (data.ReadDouble() != 370.5)
                Fail("Failed luanumber test");
            data.ReadByte();
        }

        /// <summary>
        /// Small helper function to show the last error
        /// </summary>
        /// <param name="error">Last error</param>
        private void Fail(string error)
        {
            if (!Valid)
                return;
            LastError = error;
            Valid = false;
        }
    }
}