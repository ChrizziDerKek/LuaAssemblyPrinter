namespace LuaAssemblyPrinter
{
    /// <summary>
    /// Single lua upvalue
    /// </summary>
    class UpValue
    {
        //1 if it's stored in the stack, otherwise 0
        public byte InStack { get; private set; }

        //Stack index of the value
        public byte Index { get; private set; }

        /// <summary>
        /// Creates a new upvalue
        /// </summary>
        /// <param name="instack">1 if it's stored in the stack, otherwise 0</param>
        /// <param name="index">Stack index of the value</param>
        public UpValue(byte instack, byte index)
        {
            InStack = instack;
            Index = index;
        }
    }
}