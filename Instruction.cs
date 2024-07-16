namespace LuaAssemblyPrinter
{
    /// <summary>
    /// Single lua instruction
    /// </summary>
    class Instruction
    {
        //Opcode as raw integer
        private int Data;

        //Converter for fast int conversion
        public static implicit operator Instruction(int inst) => new Instruction() { Data = inst };

        //Gets the first 6 bits which store the opcode
        public EOpcode Opcode => (EOpcode)(Data & 0x3F);

        //Gets 8 bits which store the first instruction value
        public int A => (Data >> 6) & 0xFF;

        //Gets the next 9 bits
        public int C => (Data >> 14) & 0x1FF;

        //Gets the next 9 bits
        public int B => (Data >> 23) & 0x1FF;

        //Gets B and C together
        public int Bx => (int)(((uint)Data) >> 14);

        //Gets B and C together as a signed value
        public int SBx => Bx - 0x1FFFF;

        //Gets A, B and C together
        public int Ax => (int)(((uint)Data) >> 6);
    }
}