using System;
using System.Collections.Generic;
using System.IO;

namespace LuaAssemblyPrinter
{
    /// <summary>
    /// Prints and decompiles the lua assembly code
    /// </summary>
    class AssemblyPrinter
    {
        //Main function block
        private FunctionBlock Block;

        //All functions in the file
        private Dictionary<FunctionBlock, string> Funcs;

        //Storage for the full decompilation
        private string Storage;

        /// <summary>
        /// Creates a printer from a file
        /// </summary>
        /// <param name="file">File to read</param>
        public AssemblyPrinter(string file) : this(new Bytestream(File.ReadAllBytes(file))) { }

        /// <summary>
        /// Creates a printer from binary data
        /// </summary>
        /// <param name="stream">Binary data to read</param>
        public AssemblyPrinter(Bytestream stream) => Block = new FunctionBlock(stream);

        /// <summary>
        /// Prints the decompiled assembly code to the console
        /// </summary>
        public void Print() => Console.WriteLine(ToString());

        /// <summary>
        /// Returns the decompiled assembly code as a string
        /// </summary>
        /// <returns>Decompiled assembly code</returns>
        public override string ToString()
        {
            //If the decompilation is already stored, just return it
            if (!string.IsNullOrEmpty(Storage))
                return Storage;
            //Initialize the function dict
            if (Funcs == null)
            {
                Funcs = new Dictionary<FunctionBlock, string>();
                int it = 0;
                InitRec(ref it, Block);
                //Delete the main block as it's in the dict now
                Block = null;
            }
            string linestart = "\t";
            List<string> lines = new List<string>();
            //Find the maximum amount of digits of the pc value to make formatting a lot easier
            int maxdigit = 0;
            foreach (FunctionBlock block in Funcs.Keys)
                if (block.Opcodes.Length.ToString().Length > maxdigit)
                    maxdigit = block.Opcodes.Length.ToString().Length;
            //Repeat this for every function
            foreach (FunctionBlock block in Funcs.Keys)
            {
                //Add basic function infos
                lines.Add(GetFuncHeader(block, Funcs[block]));
                //Add the opcodes one by one
                for (int pc = 0; pc < block.Opcodes.Length; pc++)
                {
                    string str = GetOpcode(block, block.Opcodes[pc], pc, Funcs);
                    if (string.IsNullOrEmpty(str))
                        continue;
                    //Add leading zeros to the pc address to align all opcodes
                    string pcstr = (pc + 1).ToString();
                    while (pcstr.Length != maxdigit)
                        pcstr = "0" + pcstr;
                    lines.Add(linestart + pcstr + "\t" + str);
                }
                //Add infos about locals, upvalues and constants
                lines.Add("");
                lines.Add("\tlocals (" + block.Locals.Length + ")");
                for (int i = 0; i < block.Locals.Length; i++)
                    lines.Add("\t\tname: " + block.Locals[i].Name + "; startpc: " + block.Locals[i].StartPC + "; endpc: " + block.Locals[i].EndPC);
                lines.Add("\tupvales (" + block.UpValues.Length + ")");
                for (int i = 0; i < block.UpValues.Length; i++)
                    lines.Add("\t\tname: " + block.UpValueNames[i] + "; instack: " + (block.UpValues[i].InStack == 1 ? "true" : "false") + "; index: " + block.UpValues[i].Index);
                lines.Add("\tconstants (" + block.Constants.Length + ")");
                for (int i = 0; i < block.Constants.Length; i++)
                    lines.Add("\t\tid: " + i + "; value: " + block.Constants[i]);
                lines.Add("end");
                lines.Add("");
            }
            //Align the comments that may appear after an opcode
            string comment = "\x00";
            string maxline = "";
            //Get the longest line
            foreach (string line in lines)
                if (line.Length > maxline.Length && line.StartsWith(linestart) && !line.StartsWith(linestart + linestart))
                    maxline = line + "   ";
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].Contains(comment))
                    continue;
                //Get the opcode line before the comment
                string line = lines[i].Split(comment[0])[0];
                //Add a space until we reached the maximum line size
                while (maxline.Length != line.Length)
                {
                    lines[i] = lines[i].Replace(comment, " " + comment);
                    line = lines[i].Split(comment[0])[0];
                }
                lines[i] = lines[i].Replace(comment, "; ");
            }
            //Merge the lines into one string
            string s = "";
            foreach (string line in lines)
                s += line + "\n";
            //Store the string for later
            Storage = s;
            //We can delete our function infos now
            Funcs = null;
            return s;
        }

        /// <summary>
        /// Populates the function dictionary and names them
        /// </summary>
        /// <param name="id"></param>
        /// <param name="block"></param>
        private void InitRec(ref int id, FunctionBlock block)
        {
            //Check if the function is valid
            if (block == null)
                return;
            //Add the function to our dict
            if (!Funcs.ContainsKey(block))
                Funcs.Add(block, "func" + id++);
            foreach (FunctionBlock func in block.Functions)
            {
                //Also do the same with the child functions
                if (!Funcs.ContainsKey(func))
                    Funcs.Add(func, "func" + id++);
                //Repeat this for every other function
                InitRec(ref id, func);
            }
        }

        /// <summary>
        /// Small helper function to format constant indices
        /// </summary>
        /// <param name="val">Constant value or register</param>
        /// <returns>Formatted index</returns>
        private static int RegConst(int val) => val > 0xFF ? -((val & 0xFF) + 1) : val;

        /// <summary>
        /// Turns a single lua instruction into a string
        /// </summary>
        /// <param name="block">Functionblock where the instruction is</param>
        /// <param name="i">The instruction itself</param>
        /// <param name="pc">Address of the instruction</param>
        /// <param name="names">Function names, used by the closure instruction</param>
        /// <returns>Instruction as string</returns>
        private static string GetOpcode(FunctionBlock block, Instruction i, int pc, Dictionary<FunctionBlock, string> names)
        {
            string s = "";
            string comment = "\x00";
            switch (i.Opcode)
            {
                case EOpcode.MOVE:
                    s += "MOVE " + i.A + " " + i.B;
                    break;
                case EOpcode.LOADK:
                    s += "LOADK " + i.A + " " + -(i.Bx + 1);
                    s += comment + block.Constants[i.Bx];
                    break;
                case EOpcode.LOADKX:
                    i = block.Opcodes[pc + 1];
                    s += "LOADKX " + i.A + " " + -(i.Ax + 1);
                    s += comment + block.Constants[i.Ax];
                    break;
                case EOpcode.LOADBOOL:
                    s += "LOADBOOL " + i.A + " " + i.B + " " + i.C;
                    break;
                case EOpcode.LOADNIL:
                    s += "LOADNIL " + i.A + " " + i.B;
                    break;
                case EOpcode.GETUPVAL:
                    s += "GETUPVAL " + i.A + " " + i.B;
                    s += comment + block.UpValueNames[i.B];
                    break;
                case EOpcode.GETTABUP:
                    s += "GETTABUP " + i.A + " " + i.B + " " + RegConst(i.C);
                    s += comment + block.UpValueNames[i.B];
                    if (i.C > 0xFF)
                        s += " " + block.Constants[i.C & 0xFF];
                    break;
                case EOpcode.GETTABLE:
                    s += "GETTABLE " + i.A + " " + i.B + " " + RegConst(i.C);
                    if (i.C > 0xFF)
                        s += comment + block.Constants[i.C & 0xFF];
                    break;
                case EOpcode.SETTABUP:
                    s += "SETTABUP " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment + block.UpValueNames[i.A];
                    if (i.B > 0xFF)
                        s += " " + block.Constants[i.B & 0xFF];
                    if (i.C > 0xFF)
                        s += " " + block.Constants[i.C & 0xFF];
                    break;
                case EOpcode.SETUPVAL:
                    s += "SETUPVAL " + i.A + " " + i.B;
                    s += comment + block.UpValueNames[i.B];
                    break;
                case EOpcode.SETTABLE:
                    s += "SETTABLE " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    if (i.B > 0xFF && i.C > 0xFF)
                    {
                        s += comment + block.Constants[i.B & 0xFF];
                        s += " " + block.Constants[i.C & 0xFF];
                    }
                    else if (i.B > 0xFF)
                        s += comment + block.Constants[i.B & 0xFF];
                    else if (i.C > 0xFF)
                        s += comment + block.Constants[i.C & 0xFF];
                    break;
                case EOpcode.NEWTABLE:
                    s += "NEWTABLE " + i.A + " " + i.B + " " + i.C;
                    break;
                case EOpcode.SELF:
                    s += "SELF " + i.A + " " + i.B + " " + RegConst(i.C);
                    if (i.C > 0xFF)
                        s += comment + block.Constants[i.C & 0xFF];
                    break;
                case EOpcode.ADD:
                    s += "ADD " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.SUB:
                    s += "SUB " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.MUL:
                    s += "MUL " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.MOD:
                    s += "MOD " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.POW:
                    s += "POW " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.DIV:
                    s += "DIV " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.IDIV:
                    s += "IDIV " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.BAND:
                    s += "BAND " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.BOR:
                    s += "BOR " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.BXOR:
                    s += "BXOR " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.SHL:
                    s += "SHL " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.SHR:
                    s += "SHR " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.UNM:
                    s += "UNM " + i.A + " " + i.B;
                    break;
                case EOpcode.BNOT:
                    s += "BNOT " + i.A + " " + i.B;
                    break;
                case EOpcode.NOT:
                    s += "NOT " + i.A + " " + i.B;
                    break;
                case EOpcode.LEN:
                    s += "LEN " + i.A + " " + i.B;
                    break;
                case EOpcode.CONCAT:
                    s += "CONCAT " + i.A + " " + i.B + " " + i.C;
                    break;
                case EOpcode.JMP:
                    s += "JMP " + i.A + " " + i.SBx;
                    s += comment + "to pc " + (pc + 2 + i.SBx);
                    break;
                case EOpcode.EQ:
                    s += "EQ " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.LT:
                    s += "LT " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.LE:
                    s += "LE " + i.A + " " + RegConst(i.B) + " " + RegConst(i.C);
                    s += comment;
                    if (i.B > 0xFF)
                        s += block.Constants[i.B & 0xFF];
                    else
                        s += "-";
                    s += " ";
                    if (i.C > 0xFF)
                        s += block.Constants[i.C & 0xFF];
                    else
                        s += "-";
                    s += comment;
                    s = s.Replace(comment + "- -" + comment, "");
                    s = s.Substring(0, s.Length - 1);
                    break;
                case EOpcode.TEST:
                    s += "TEST " + i.A + " " + i.C;
                    break;
                case EOpcode.TESTSET:
                    s += "TESTSET " + i.A + " " + i.B + " " + i.C;
                    break;
                case EOpcode.CALL:
                    s += "CALL " + i.A + " " + i.B + " " + i.C;
                    break;
                case EOpcode.TAILCALL:
                    s += "TAILCALL " + i.A + " " + i.B + " " + i.C;
                    break;
                case EOpcode.RETURN:
                    s += "RETURN " + i.A + " " + i.B;
                    break;
                case EOpcode.FORLOOP:
                    s += "FORLOOP " + i.A + " " + i.SBx;
                    s += comment + "to pc " + (pc + 2 + i.SBx);
                    break;
                case EOpcode.FORPREP:
                    s += "FORPREP " + i.A + " " + i.SBx;
                    s += comment + "to pc " + (pc + 2 + i.SBx);
                    break;
                case EOpcode.TFORCALL:
                    s += "TFORCALL " + i.A + " " + i.C;
                    break;
                case EOpcode.TFORLOOP:
                    s += "TFORLOOP " + i.A + " " + i.SBx;
                    s += comment + "to pc " + (pc + 2 + i.SBx);
                    break;
                case EOpcode.SETLIST:
                    s += "SETLIST " + i.A + " " + i.B + " " + i.C;
                    if (i.C > 0)
                        s += comment + i.C;
                    else
                    {
                        i = block.Opcodes[pc + 1];
                        s += comment + i.Ax;
                    }
                    break;
                case EOpcode.CLOSURE:
                    s += "CLOSURE " + i.A + " " + i.Bx;
                    s += comment + names[block.Functions[i.Bx]];
                    break;
                case EOpcode.VARARG:
                    s += "VARARG " + i.A + " " + i.B;
                    break;
                case EOpcode.EXTRAARG:
                    s += "EXTRAARG " + i.Ax;
                    break;
            }
            return s;
        }

        /// <summary>
        /// Returns some basic info about the function
        /// </summary>
        /// <param name="block">Functionblock to get the infos from</param>
        /// <param name="name">Name of the block</param>
        /// <returns></returns>
        private static string GetFuncHeader(FunctionBlock block, string name)
        {
            //Read the basic infos
            string param = " param";
            string s = block.NumParams + param;
            if (block.NumParams != 1)
                s += "s";
            s += "; ";
            if (block.IsVarArg == 1)
                s += "n varargs; ";
            else
                s += "0 varargs; ";
            s += block.MaxStackSize + " slot";
            if (block.MaxStackSize != 1)
                s += "s";
            s += "; ";
            s += block.Opcodes.Length + " opcode";
            if (block.Opcodes.Length != 1)
                s += "s";
            s += "; ";
            s += block.Constants.Length + " constant";
            if (block.Constants.Length != 1)
                s += "s";
            s += "; ";
            s += block.UpValues.Length + " upvalue";
            if (block.UpValues.Length != 1)
                s += "s";
            s += "; ";
            s += block.Functions.Length + " function";
            if (block.Functions.Length != 1)
                s += "s";
            s += "; ";
            s += block.Locals.Length + " local";
            if (block.Locals.Length != 1)
                s += "s";
            s += "\n";
            s += "function " + name + "(";
            //Add a param name into the brackets for each param
            for (int i = 1; i <= block.NumParams; i++)
                s += "param" + i + ", ";
            //Add ... if there are varargs
            if (block.IsVarArg == 1)
                s += "...";
            //Remove  trailing commas
            s += "\x00";
            s = s.Replace(", \x00", "");
            s = s.Replace("\x00", "");
            s += ")";
            return s;
        }
    }
}