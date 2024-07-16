using System;

namespace LuaAssemblyPrinter
{
    /// <summary>
    /// Single lua function or closure
    /// </summary>
    class FunctionBlock
    {
        //File header
        public Header Header { get; private set; }

        //Source filename
        public string SourceFile { get; private set; }

        //Line where the function begins
        public int LineStart { get; private set; }

        //Line where the function ends
        public int LineEnd { get; private set; }

        //Parameter count without varargs
        public byte NumParams { get; private set; }

        //1 if the function has varargs
        public byte IsVarArg { get; private set; }

        //Max stack size that the function can use
        public byte MaxStackSize { get; private set; }

        //Opcodes that define the function code
        public int[] Opcodes { get; private set; }

        //Constants that the function can access
        public object[] Constants { get; private set; }

        //Upvalues that the function can access
        public UpValue[] UpValues { get; private set; }

        //Child functions that can get called by the current one
        public FunctionBlock[] Functions { get; private set; }

        //Line infos, unused
        public int[] LineInfo { get; private set; }

        //Local variables that the function uses
        public Local[] Locals { get; private set; }

        //Names of the accessed upvalues
        public string[] UpValueNames { get; private set; }

        /// <summary>
        /// Creates a new functionblock from a binary stream
        /// </summary>
        /// <param name="data">Binary data</param>
        public FunctionBlock(Bytestream data)
        {
            //Create a header and check if it's valid
            Header = new Header(data);
            if (!Header.Valid)
                throw new Exception(Header.LastError);
            //Start reading
            Read(data, "");
        }

        /// <summary>
        /// Creates an empty functionblock
        /// </summary>
        /// <param name="h">File header</param>
        private FunctionBlock(Header h) => Header = h;

        /// <summary>
        /// Reads the values of the functionblock from the binary stream
        /// </summary>
        /// <param name="data">Binary data</param>
        /// <param name="source">Source file</param>
        private void Read(Bytestream data, string source)
        {
            SourceFile = data.ReadString();
            if (string.IsNullOrEmpty(SourceFile))
                SourceFile = source;
            LineStart = data.ReadInt();
            LineEnd = data.ReadInt();
            NumParams = data.ReadByte();
            IsVarArg = data.ReadByte();
            MaxStackSize = data.ReadByte();
            Opcodes = new int[data.ReadInt()];
            for (int i = 0; i < Opcodes.Length; i++)
                Opcodes[i] = data.ReadInt();
            Constants = new object[data.ReadInt()];
            for (int i = 0; i < Constants.Length; i++)
            {
                switch (data.ReadByte())
                {
                    case 0:
                        Constants[i] = null;
                        break;
                    case 1:
                        Constants[i] = data.ReadByte() != 0;
                        break;
                    case 19:
                        Constants[i] = data.ReadLong();
                        break;
                    case 3:
                        Constants[i] = data.ReadDouble();
                        break;
                    case 4:
                    case 20:
                        Constants[i] = data.ReadString();
                        break;
                }
            }
            UpValues = new UpValue[data.ReadInt()];
            for (int i = 0; i < UpValues.Length; i++)
                UpValues[i] = new UpValue(data.ReadByte(), data.ReadByte());
            Functions = new FunctionBlock[data.ReadInt()];
            for (int i = 0; i < Functions.Length; i++)
            {
                Functions[i] = new FunctionBlock(Header);
                Functions[i].Read(data, SourceFile);
            }
            LineInfo = new int[data.ReadInt()];
            for (int i = 0; i < LineInfo.Length; i++)
                LineInfo[i] = data.ReadInt();
            Locals = new Local[data.ReadInt()];
            for (int i = 0; i < Locals.Length; i++)
                Locals[i] = new Local(data.ReadString(), data.ReadInt(), data.ReadInt());
            UpValueNames = new string[data.ReadInt()];
            for (int i = 0; i < UpValueNames.Length; i++)
                UpValueNames[i] = data.ReadString();
        }
    }
}