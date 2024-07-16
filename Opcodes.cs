namespace LuaAssemblyPrinter
{
	/// <summary>
	/// A list of lua 5.3 opcodes with a small description
	/// </summary>
	enum EOpcode
	{
		MOVE, //R(A) := R(B)
		LOADK, //R(A) := Kst(Bx)
		LOADKX, //R(A) := Kst(EXTRAARG)
		LOADBOOL, //R(A) := (bool)B; if (C) pc++
		LOADNIL, //R(A), R(A+1), ..., R(A+B) := nil
		GETUPVAL, //R(A) := Upvalue[B]
		GETTABUP, //R(A) := Upvalue[B][RK(C)]
		GETTABLE, //R(A) := R(B)[RK(C)]
		SETTABUP, //Upvalue[A][RK(B)] := RK(C)
		SETUPVAL, //Upvalue[B] := R(A)
		SETTABLE, //R(A)[RK(B)] := RK(C)
		NEWTABLE, //R(A) := {} (size = B,C)
		SELF, //R(A+1) := R(B); R(A) := R(B)[RK(C)]
		ADD, //R(A) := RK(B) + RK(C)
		SUB, //R(A) := RK(B) - RK(C)
		MUL, //R(A) := RK(B) * RK(C)
		MOD, //R(A) := RK(B) % RK(C)
		POW, //R(A) := RK(B) ^ RK(C)
		DIV, //R(A) := RK(B) / RK(C)
		IDIV, //R(A) := RK(B) // RK(C)
		BAND, //R(A) := RK(B) & RK(C)
		BOR, //R(A) := RK(B) | RK(C)
		BXOR, //R(A) := RK(B) ~ RK(C)
		SHL, //R(A) := RK(B) << RK(C)
		SHR, //R(A) := RK(B) >> RK(C)
		UNM, //R(A) := -R(B)
		BNOT, //R(A) := ~R(B)
		NOT, //R(A) := not R(B)
		LEN, //R(A) := length of R(B)
		CONCAT, //R(A) := R(B).. ... ..R(C)
		JMP, //pc += sBx; if (A) close all upvalues >= R(A - 1)
		EQ, //if ((RK(B) == RK(C)) ~= A) pc++
		LT, //if ((RK(B) < RK(C)) ~= A) pc++
		LE, //if ((RK(B) <= RK(C)) ~= A) pc++
		TEST, //if not (R(A) <=> C) pc++
		TESTSET, //if (R(B) <=> C) R(A) := R(B) else pc++
		CALL, //R(A), ... R(A+C-2) := R(A)(R(A + 1), ..., R(A + B - 1))
		TAILCALL, //return R(A)(R(A + 1), ..., R(A + B - 1))
		RETURN, //return R(A), ..., R(A + B - 2)
		FORLOOP, //R(A) += R(A + 2); if (R(A) <= R(A + 1)) { pc += sBx; R(A + 3) := R(A) }
		FORPREP, //R(A) -= R(A + 2); pc += sBx
		TFORCALL, //R(A + 3), ... ,R(A + 2 + C) := R(A)(R(A + 1), R(A + 2))
		TFORLOOP, //if (R(A + 1)) ~= nil { R(A) := R(A + 1); pc += sBx }
		SETLIST, //R(A)[(C - 1) * FPF + i] := R(A + i), 1 <= i <= B
		CLOSURE, //R(A) := closure(KPROTO[Bx])
		VARARG, //R(A), R(A + 1), ..., R(A + B - 2) = vararg
		EXTRAARG, //extra stuff that could be anything
	};
}