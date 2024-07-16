namespace LuaAssemblyPrinter
{
    /// <summary>
    /// A local variable in a lua closure
    /// </summary>
    class Local
    {
        //Name of the variable
        public string Name { get; private set; }

        //PC value where the variable gets in scope
        public int StartPC { get; private set; }

        //PC value where the variable gets out of scope
        public int EndPC { get; private set; }

        /// <summary>
        /// Creates a new local variable
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="startpc">PC value where the variable gets in scope</param>
        /// <param name="endpc">PC value where the variable gets out of scope</param>
        public Local(string name, int startpc, int endpc)
        {
            Name = name;
            StartPC = startpc;
            EndPC = endpc;
        }
    }
}