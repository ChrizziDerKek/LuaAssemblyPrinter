# LuaAssemblyPrinter
A nicer way to visualize lua assembly code.
## Content
- [How to use](#how-to-use)
- [Example 1](#example-1)
- [Example 2](#example-2)
- [Example 3](#example-3)
## How to use
Drag a compiled lua (.luac) file on the program or run it with these console commands
- ``LuaAssemblyPrinter input.luac output.txt``: Will read the file ``input.luac`` and stores the assembly output in ``output.txt``
- ``LuaAssemblyPrinter input.luac``: Will read the file ``input.luac`` and shows the assembly output in the console
## Example 1
Original lua code:
```lua
function factorial(n, acc)
    acc = acc or 1
    if n <= 1 then
        return acc
    else
        return factorial(n - 1, n * acc)
    end
end

local result = factorial(5)
print(result)
```
Compiled luac binary:
```
LuaS “

xV           (w@@/dev/shm/luac.nlzSLblS         	   ,     €@@ A€  $€ FÀ@ €   d@ & €    
factorial
factorial       print               
   b@    €A   ! @ @ €f   €†@@ Î @ A  ¥ €¦   & €           
factorial         
                                             n    
   acc    
      _ENV	         
   
   
                  result   	      _ENV
```
Output of the LuaAssemblyPrinter after reading the binary:
```
0 params; n varargs; 3 slots; 9 opcodes; 4 constants; 1 upvalue; 1 function; 1 local
function func0(...)
        01      CLOSURE 0 0                      ; func1
        02      SETTABUP 0 -1 0                  ; _ENV factorial
        03      GETTABUP 0 0 -2                  ; _ENV factorial
        04      LOADK 1 -3                       ; 5
        05      CALL 0 2 2
        06      GETTABUP 1 0 -4                  ; _ENV print
        07      MOVE 2 0
        08      CALL 1 2 1
        09      RETURN 0 1

        locals (1)
                name: result; startpc: 5; endpc: 9
        upvales (1)
                name: _ENV; instack: true; index: 0
        constants (4)
                id: 0; value: factorial
                id: 1; value: factorial
                id: 2; value: 5
                id: 3; value: print
end

2 params; 0 varargs; 5 slots; 13 opcodes; 2 constants; 1 upvalue; 0 functions; 2 locals
function func1(param1, param2)
        01      TEST 1 1
        02      JMP 0 1                          ; to pc 4
        03      LOADK 1 -1                       ; 1
        04      LE 0 0 -1                        ; - 1
        05      JMP 0 2                          ; to pc 8
        06      RETURN 1 2
        07      JMP 0 5                          ; to pc 13
        08      GETTABUP 2 0 -2                  ; _ENV factorial
        09      SUB 3 0 -1                       ; - 1
        10      MUL 4 0
        11      TAILCALL 2 3 0
        12      RETURN 2 0
        13      RETURN 0 1

        locals (2)
                name: n; startpc: 0; endpc: 13
                name: acc; startpc: 0; endpc: 13
        upvales (1)
                name: _ENV; instack: false; index: 0
        constants (2)
                id: 0; value: 1
                id: 1; value: factorial
end
```
## Example 2
Original lua code:
```lua
-- loadk
local a = 1337

-- gettabup, eq, jmp, loadbool, call
assert(a == 1337, "test 1 failed")

-- loadnil
a = nil
assert(a == nil, "test 2 failed")

-- shl, shr
a = 1
a = a << 8
a = a >> 2
assert(a == 64, "test 3 failed")

-- add, sub, mul, div, mod, pow, unm, idiv, band, bor, bxor, bnot
a = 10
a = a + 10
assert(a == 20, "test 4 failed")
a = a - 5
assert(a == 15, "test 5 failed")
a = a * 2
assert(a == 30, "test 6 failed")
a = a / 5
assert(a == 6, "test 7 failed")
a = a % 2
assert(a == 0, "test 8 failed")
a = 10
a = a ^ 5
assert(a == 100000, "test 9 failed")
a = -a
assert(a == -100000, "test 10 failed")
a = a // 2
assert(a == -50000, "test 11 failed")
a = a & 32
assert(a == 32, "test 12 failed")
a = a | 64
assert(a == 96, "test 13 failed")
a = a ~ 2
assert(a == 98, "test 14 failed")
a = ~a
assert(a == -99, "test 15 failed")

-- newtable, setlist, settable, gettable, move
a = { 10, 20, 30 }
a[4] = 40
assert(a[2] == 20, "test 16 failed")
assert(a[4] == 40, "test 17 failed")
a = a[4]

-- le, lt
assert(a > 39, "test 18 failed")
assert(a >= 40, "test 19 failed")
assert(a < 41, "test 20 failed")
assert(a <= 40, "test 21 failed")

-- not
a = false
assert(not a, "test 22 failed")

-- len
a = "Heyyy"
assert(#a == 5, "test 23 failed")

-- forprep, forloop, concat
for i=1,10,1 do
	a = a.."y"
end
assert(a == "Heyyyyyyyyyyyyy", "test 24 failed")

-- closure, tailcall, return
a = function()
	local function b()
    	return 1337
    end
	return b()
end
assert(a() == 1337, "test 25 failed")

-- test, testset
a = true
a = a and 10 or 100
assert(a == 10, "test 26 failed")

-- settabup, self
a = {
	c = 1337,
	b = function(this)
    	return this.c
    end
}
assert(a.c == 1337, "test 27 failed")
assert(a.c == a:b(), "test 28 failed")
local b = function(key, val)
	a[key] = val
end
b("msg", "Heyyy")
assert(a["msg"] == "Heyyy", "test 29 failed")

-- setupval, getupval
a = 6
b = function()
	a = a * 9
    a = a + 6
    a = a + 9
end
b()
assert(a == 69, "test 30 failed")

-- tforcall, tforloop
a = { 1, 2, 3 }
b = 1
for k, v in ipairs(a) do
	assert(b == v, "test 31 failed")
    b = b + 1
end

-- vararg
b = function(...)
	local vals = { ... }
    local total = 0
    for k, v in ipairs(vals) do
    	total = total + v
    end
    return total
end
assert(b(1, 2, 3, 4) == 10, "test 32 failed")
```
Compiled luac binary:
```
LuaS “

xV           (w@@/dev/shm/luac.nl0N2vX5         
;     F@@ _ @   €ƒ@  ƒ € Á€  d@€   F@@ _À@   €ƒ@  ƒ € Á  d@€@ €A ÀA F@@ _ B   €ƒ@  ƒ € Á@ d@€€ 
€B F@@ _ÀB   €ƒ@  ƒ € Á  d@€@C F@@ _€C   €ƒ@  ƒ € ÁÀ d@€ÀA F@@ _ D   €ƒ@  ƒ € Á@ d@€@C F@@ _€D   €ƒ@  ƒ € ÁÀ d@€ÀA F@@ _ E   €ƒ@  ƒ € Á@ d@€€ @C F@@ _€E   €ƒ@  ƒ € ÁÀ d@€   F@@ _ F   €ƒ@  ƒ € Á@ d@€ÀA F@@ _€F   €ƒ@  ƒ € ÁÀ d@€ G F@@ _ G   €ƒ@  ƒ € Á@ d@€ B F@@ _€G   €ƒ@  ƒ € ÁÀ d@€ÀA F@@ _ H   €ƒ@  ƒ € Á@ d@€   F@@ _€H   €ƒ@  ƒ € ÁÀ d@€K €€ ÁÀ  k@€  € 
@I’F@@ ‡ÀA _ÀB  €ƒ@  ƒ € Á€	 d@€F@@ ‡ I _@I  €ƒ@  ƒ € ÁÀ	 d@€ I F@@ `  ”  €ƒ@  ƒ € Á@
 d@€F@@ a €’  €ƒ@  ƒ € Á€
 d@€F@@ `ÀJ   €ƒ@  ƒ € Á  d@€F@@ a@I   €ƒ@  ƒ € Á@ d@€   F@@ ›   Á€ d@€À F@@ œ   _@C  €ƒ@  ƒ € Á  d@€A@ € Á@ h€ €@  A €gÀþF@@ _€L   €ƒ@  ƒ € ÁÀ d@€l     € F@@ €   ¤€€ _ @  €ƒ@  ƒ € Á 
 d@€ € "   € €A€ #@€   €@
 F@@ _€B   €ƒ@  ƒ € Á€
 d@€K€  J À›¬@  J€ œ  € F@@ ‡@N _ @  €ƒ@  ƒ € Á€ d@€F@@ ‡@N Ì N ä€ _À   €ƒ@  ƒ € ÁÀ d@€l€  € € Á  Á ¤@€†@@ Ç O _ÀË  €Ã@  Ã € A ¤@€€ ¬À  @  € € ¤@€ †@@ _€O   €Ã@  Ã € Á ¤@€‹ €Á@ Á A «@€   A@ †@P À   ¤ À€ÆA@ _€   €B  € A‚ äA€M@Á ©€  *Aý¬  @  †@@ À € A AÁ  Á	 ä€€_€Â  €Ã@  Ã € Á ¤@€& € D   9      asserttest 1 failed test 2 failed                     @       test 3 failed
              test 4 failed              test 5 failed       test 6 failed       test 7 failed        test 8 failed †     test 9 failed`yþÿÿÿÿÿtest 10 failed°<ÿÿÿÿÿÿtest 11 failed        test 12 failed`       test 13 failedb       test 14 failedÿÿÿÿÿÿÿtest 15 failed       (       test 16 failedtest 17 failed'       test 18 failedtest 19 failed)       test 20 failedtest 21 failedtest 22 failedHeyyytest 23 failedyHeyyyyyyyyyyyyytest 24 failedtest 25 failedd       test 26 failedcbctest 27 failedtest 28 failedmsgtest 29 failedE       test 30 failed       ipairstest 31 failedtest 32 failed        I   N        ,   @   e € f   & €             J   L           &  & €    9                 K   K   L              L   M   M   M   N      b           Y   [       G @ f  & €    c           Z   Z   [      this            _   a       @  & €                `   a      key       val          a g   k     
       @ 	      
@@ 	      
 @ 	   & €    	                      
   h   h   h   i   i   i   j   j   j   k          a x       
      m   +@  A   †@@ À   ¤   €M€ ©€  *ÿf  & €            ipairs         
   y   y   y   z   {   {   {   {   |   {   {   ~         vals   
   total   
   (for generator)      (for state)      (for control)      k   	   v   	      _ENV;                             	   	   	   	   	   	   	      
                                                                                                                                                                                     !   !   !   !   !   !   !   "   #   #   #   #   #   #   #   $   %   %   %   %   %   %   %   &   '   '   '   '   '   '   '   (   )   )   )   )   )   )   )   *   +   +   +   +   +   +   +   .   .   .   .   .   .   /   0   0   0   0   0   0   0   0   1   1   1   1   1   1   1   1   2   5   5   5   5   5   5   5   6   6   6   6   6   6   6   7   7   7   7   7   7   7   8   8   8   8   8   8   8   ;   <   <   <   <   ?   @   @   @   @   @   @   @   @   C   C   C   C   D   D   D   C   F   F   F   F   F   F   F   N   N   O   O   O   O   O   O   O   O   O   R   S   S   S   S   S   S   T   T   T   T   T   T   T   W   X   [   [   \   ]   ]   ]   ]   ]   ]   ]   ]   ^   ^   ^   ^   ^   ^   ^   ^   ^   ^   a   b   b   b   b   c   c   c   c   c   c   c   c   f   k   k   l   l   m   m   m   m   m   m   m   p   p   p   p   p   p   q   r   r   r   r   s   s   s   s   s   s   s   t   r   r         €   €   €   €   €   €   €   €   €   €   €   €   €   €      a   ;  (for index)Á   Æ   (for limit)Á   Æ   (for step)Á   Æ   iÂ   Å   bþ   ;  (for generator)   +  (for state)   +  (for control)   +  k!  )  v!  )     _ENV
```
Output of the LuaAssemblyPrinter after reading the binary:
```
0 params; n varargs; 10 slots; 315 opcodes; 68 constants; 1 upvalue; 5 functions; 11 locals
function func0(...)
	001	LOADK 0 -1                    ; 1337
	002	GETTABUP 1 0 -2               ; _ENV assert
	003	EQ 1 0 -1                     ; - 1337
	004	JMP 0 1                       ; to pc 6
	005	LOADBOOL 2 0 1
	006	LOADBOOL 2 1 0
	007	LOADK 3 -3                    ; test 1 failed
	008	CALL 1 3 1
	009	LOADNIL 0 0
	010	GETTABUP 1 0 -2               ; _ENV assert
	011	EQ 1 0 -4                     ; - 
	012	JMP 0 1                       ; to pc 14
	013	LOADBOOL 2 0 1
	014	LOADBOOL 2 1 0
	015	LOADK 3 -5                    ; test 2 failed
	016	CALL 1 3 1
	017	LOADK 0 -6                    ; 1
	018	SHL 0 0 -7                    ; - 8
	019	SHR 0 0 -8                    ; - 2
	020	GETTABUP 1 0 -2               ; _ENV assert
	021	EQ 1 0 -9                     ; - 64
	022	JMP 0 1                       ; to pc 24
	023	LOADBOOL 2 0 1
	024	LOADBOOL 2 1 0
	025	LOADK 3 -10                   ; test 3 failed
	026	CALL 1 3 1
	027	LOADK 0 -11                   ; 10
	028	ADD 0 0 -11                   ; - 10
	029	GETTABUP 1 0 -2               ; _ENV assert
	030	EQ 1 0 -12                    ; - 20
	031	JMP 0 1                       ; to pc 33
	032	LOADBOOL 2 0 1
	033	LOADBOOL 2 1 0
	034	LOADK 3 -13                   ; test 4 failed
	035	CALL 1 3 1
	036	SUB 0 0 -14                   ; - 5
	037	GETTABUP 1 0 -2               ; _ENV assert
	038	EQ 1 0 -15                    ; - 15
	039	JMP 0 1                       ; to pc 41
	040	LOADBOOL 2 0 1
	041	LOADBOOL 2 1 0
	042	LOADK 3 -16                   ; test 5 failed
	043	CALL 1 3 1
	044	MUL 0 0 -8                    ; - 2
	045	GETTABUP 1 0 -2               ; _ENV assert
	046	EQ 1 0 -17                    ; - 30
	047	JMP 0 1                       ; to pc 49
	048	LOADBOOL 2 0 1
	049	LOADBOOL 2 1 0
	050	LOADK 3 -18                   ; test 6 failed
	051	CALL 1 3 1
	052	DIV 0 0 -14                   ; - 5
	053	GETTABUP 1 0 -2               ; _ENV assert
	054	EQ 1 0 -19                    ; - 6
	055	JMP 0 1                       ; to pc 57
	056	LOADBOOL 2 0 1
	057	LOADBOOL 2 1 0
	058	LOADK 3 -20                   ; test 7 failed
	059	CALL 1 3 1
	060	MOD 0 0 -8                    ; - 2
	061	GETTABUP 1 0 -2               ; _ENV assert
	062	EQ 1 0 -21                    ; - 0
	063	JMP 0 1                       ; to pc 65
	064	LOADBOOL 2 0 1
	065	LOADBOOL 2 1 0
	066	LOADK 3 -22                   ; test 8 failed
	067	CALL 1 3 1
	068	LOADK 0 -11                   ; 10
	069	POW 0 0 -14                   ; - 5
	070	GETTABUP 1 0 -2               ; _ENV assert
	071	EQ 1 0 -23                    ; - 100000
	072	JMP 0 1                       ; to pc 74
	073	LOADBOOL 2 0 1
	074	LOADBOOL 2 1 0
	075	LOADK 3 -24                   ; test 9 failed
	076	CALL 1 3 1
	077	UNM 0 0
	078	GETTABUP 1 0 -2               ; _ENV assert
	079	EQ 1 0 -25                    ; - -100000
	080	JMP 0 1                       ; to pc 82
	081	LOADBOOL 2 0 1
	082	LOADBOOL 2 1 0
	083	LOADK 3 -26                   ; test 10 failed
	084	CALL 1 3 1
	085	IDIV 0 0 -8                   ; - 2
	086	GETTABUP 1 0 -2               ; _ENV assert
	087	EQ 1 0 -27                    ; - -50000
	088	JMP 0 1                       ; to pc 90
	089	LOADBOOL 2 0 1
	090	LOADBOOL 2 1 0
	091	LOADK 3 -28                   ; test 11 failed
	092	CALL 1 3 1
	093	BAND 0 0 -29                  ; - 32
	094	GETTABUP 1 0 -2               ; _ENV assert
	095	EQ 1 0 -29                    ; - 32
	096	JMP 0 1                       ; to pc 98
	097	LOADBOOL 2 0 1
	098	LOADBOOL 2 1 0
	099	LOADK 3 -30                   ; test 12 failed
	100	CALL 1 3 1
	101	BOR 0 0 -9                    ; - 64
	102	GETTABUP 1 0 -2               ; _ENV assert
	103	EQ 1 0 -31                    ; - 96
	104	JMP 0 1                       ; to pc 106
	105	LOADBOOL 2 0 1
	106	LOADBOOL 2 1 0
	107	LOADK 3 -32                   ; test 13 failed
	108	CALL 1 3 1
	109	BXOR 0 0 -8                   ; - 2
	110	GETTABUP 1 0 -2               ; _ENV assert
	111	EQ 1 0 -33                    ; - 98
	112	JMP 0 1                       ; to pc 114
	113	LOADBOOL 2 0 1
	114	LOADBOOL 2 1 0
	115	LOADK 3 -34                   ; test 14 failed
	116	CALL 1 3 1
	117	BNOT 0 0
	118	GETTABUP 1 0 -2               ; _ENV assert
	119	EQ 1 0 -35                    ; - -99
	120	JMP 0 1                       ; to pc 122
	121	LOADBOOL 2 0 1
	122	LOADBOOL 2 1 0
	123	LOADK 3 -36                   ; test 15 failed
	124	CALL 1 3 1
	125	NEWTABLE 1 3 0
	126	LOADK 2 -11                   ; 10
	127	LOADK 3 -12                   ; 20
	128	LOADK 4 -17                   ; 30
	129	SETLIST 1 3 1                 ; 1
	130	MOVE 0 1
	131	SETTABLE 0 -37 -38            ; 4 40
	132	GETTABUP 1 0 -2               ; _ENV assert
	133	GETTABLE 2 0 -8               ; 2
	134	EQ 1 2 -12                    ; - 20
	135	JMP 0 1                       ; to pc 137
	136	LOADBOOL 2 0 1
	137	LOADBOOL 2 1 0
	138	LOADK 3 -39                   ; test 16 failed
	139	CALL 1 3 1
	140	GETTABUP 1 0 -2               ; _ENV assert
	141	GETTABLE 2 0 -37              ; 4
	142	EQ 1 2 -38                    ; - 40
	143	JMP 0 1                       ; to pc 145
	144	LOADBOOL 2 0 1
	145	LOADBOOL 2 1 0
	146	LOADK 3 -40                   ; test 17 failed
	147	CALL 1 3 1
	148	GETTABLE 0 0 -37              ; 4
	149	GETTABUP 1 0 -2               ; _ENV assert
	150	LT 1 -41 0                    ; 39 -
	151	JMP 0 1                       ; to pc 153
	152	LOADBOOL 2 0 1
	153	LOADBOOL 2 1 0
	154	LOADK 3 -42                   ; test 18 failed
	155	CALL 1 3 1
	156	GETTABUP 1 0 -2               ; _ENV assert
	157	LE 1 -38 0                    ; 40 -
	158	JMP 0 1                       ; to pc 160
	159	LOADBOOL 2 0 1
	160	LOADBOOL 2 1 0
	161	LOADK 3 -43                   ; test 19 failed
	162	CALL 1 3 1
	163	GETTABUP 1 0 -2               ; _ENV assert
	164	LT 1 0 -44                    ; - 41
	165	JMP 0 1                       ; to pc 167
	166	LOADBOOL 2 0 1
	167	LOADBOOL 2 1 0
	168	LOADK 3 -45                   ; test 20 failed
	169	CALL 1 3 1
	170	GETTABUP 1 0 -2               ; _ENV assert
	171	LE 1 0 -38                    ; - 40
	172	JMP 0 1                       ; to pc 174
	173	LOADBOOL 2 0 1
	174	LOADBOOL 2 1 0
	175	LOADK 3 -46                   ; test 21 failed
	176	CALL 1 3 1
	177	LOADBOOL 0 0 0
	178	GETTABUP 1 0 -2               ; _ENV assert
	179	NOT 2 0
	180	LOADK 3 -47                   ; test 22 failed
	181	CALL 1 3 1
	182	LOADK 0 -48                   ; Heyyy
	183	GETTABUP 1 0 -2               ; _ENV assert
	184	LEN 2 0
	185	EQ 1 2 -14                    ; - 5
	186	JMP 0 1                       ; to pc 188
	187	LOADBOOL 2 0 1
	188	LOADBOOL 2 1 0
	189	LOADK 3 -49                   ; test 23 failed
	190	CALL 1 3 1
	191	LOADK 1 -6                    ; 1
	192	LOADK 2 -11                   ; 10
	193	LOADK 3 -6                    ; 1
	194	FORPREP 1 3                   ; to pc 198
	195	MOVE 5 0
	196	LOADK 6 -50                   ; y
	197	CONCAT 0 5 6
	198	FORLOOP 1 -4                  ; to pc 195
	199	GETTABUP 1 0 -2               ; _ENV assert
	200	EQ 1 0 -51                    ; - Heyyyyyyyyyyyyy
	201	JMP 0 1                       ; to pc 203
	202	LOADBOOL 2 0 1
	203	LOADBOOL 2 1 0
	204	LOADK 3 -52                   ; test 24 failed
	205	CALL 1 3 1
	206	CLOSURE 1 0                   ; func1
	207	MOVE 0 1
	208	GETTABUP 1 0 -2               ; _ENV assert
	209	MOVE 2 0
	210	CALL 2 1 2
	211	EQ 1 2 -1                     ; - 1337
	212	JMP 0 1                       ; to pc 214
	213	LOADBOOL 2 0 1
	214	LOADBOOL 2 1 0
	215	LOADK 3 -53                   ; test 25 failed
	216	CALL 1 3 1
	217	LOADBOOL 0 1 0
	218	TEST 0 0
	219	JMP 0 3                       ; to pc 223
	220	LOADK 1 -11                   ; 10
	221	TESTSET 0 1 1
	222	JMP 0 1                       ; to pc 224
	223	LOADK 0 -54                   ; 100
	224	GETTABUP 1 0 -2               ; _ENV assert
	225	EQ 1 0 -11                    ; - 10
	226	JMP 0 1                       ; to pc 228
	227	LOADBOOL 2 0 1
	228	LOADBOOL 2 1 0
	229	LOADK 3 -55                   ; test 26 failed
	230	CALL 1 3 1
	231	NEWTABLE 1 0 2
	232	SETTABLE 1 -56 -1             ; c 1337
	233	CLOSURE 2 1                   ; func3
	234	SETTABLE 1 -57 2              ; b
	235	MOVE 0 1
	236	GETTABUP 1 0 -2               ; _ENV assert
	237	GETTABLE 2 0 -58              ; c
	238	EQ 1 2 -1                     ; - 1337
	239	JMP 0 1                       ; to pc 241
	240	LOADBOOL 2 0 1
	241	LOADBOOL 2 1 0
	242	LOADK 3 -59                   ; test 27 failed
	243	CALL 1 3 1
	244	GETTABUP 1 0 -2               ; _ENV assert
	245	GETTABLE 2 0 -58              ; c
	246	SELF 3 0 -57                  ; b
	247	CALL 3 2 2
	248	EQ 1 2 
	249	JMP 0 1                       ; to pc 251
	250	LOADBOOL 2 0 1
	251	LOADBOOL 2 1 0
	252	LOADK 3 -60                   ; test 28 failed
	253	CALL 1 3 1
	254	CLOSURE 1 2                   ; func4
	255	MOVE 2 1
	256	LOADK 3 -61                   ; msg
	257	LOADK 4 -48                   ; Heyyy
	258	CALL 2 3 1
	259	GETTABUP 2 0 -2               ; _ENV assert
	260	GETTABLE 3 0 -61              ; msg
	261	EQ 1 3 -48                    ; - Heyyy
	262	JMP 0 1                       ; to pc 264
	263	LOADBOOL 3 0 1
	264	LOADBOOL 3 1 0
	265	LOADK 4 -62                   ; test 29 failed
	266	CALL 2 3 1
	267	LOADK 0 -19                   ; 6
	268	CLOSURE 2 3                   ; func5
	269	MOVE 1 2
	270	MOVE 2 1
	271	CALL 2 1 1
	272	GETTABUP 2 0 -2               ; _ENV assert
	273	EQ 1 0 -63                    ; - 69
	274	JMP 0 1                       ; to pc 276
	275	LOADBOOL 3 0 1
	276	LOADBOOL 3 1 0
	277	LOADK 4 -64                   ; test 30 failed
	278	CALL 2 3 1
	279	NEWTABLE 2 3 0
	280	LOADK 3 -6                    ; 1
	281	LOADK 4 -8                    ; 2
	282	LOADK 5 -65                   ; 3
	283	SETLIST 2 3 1                 ; 1
	284	MOVE 0 2
	285	LOADK 1 -6                    ; 1
	286	GETTABUP 2 0 -66              ; _ENV ipairs
	287	MOVE 3 0
	288	CALL 2 2 4
	289	JMP 0 8                       ; to pc 298
	290	GETTABUP 7 0 -2               ; _ENV assert
	291	EQ 1 1 
	292	JMP 0 1                       ; to pc 294
	293	LOADBOOL 8 0 1
	294	LOADBOOL 8 1 0
	295	LOADK 9 -67                   ; test 31 failed
	296	CALL 7 3 1
	297	ADD 1 1 -6                    ; - 1
	298	TFORCALL 2 2
	299	TFORLOOP 4 -10                ; to pc 290
	300	CLOSURE 2 4                   ; func6
	301	MOVE 1 2
	302	GETTABUP 2 0 -2               ; _ENV assert
	303	MOVE 3 1
	304	LOADK 4 -6                    ; 1
	305	LOADK 5 -8                    ; 2
	306	LOADK 6 -65                   ; 3
	307	LOADK 7 -37                   ; 4
	308	CALL 3 5 2
	309	EQ 1 3 -11                    ; - 10
	310	JMP 0 1                       ; to pc 312
	311	LOADBOOL 3 0 1
	312	LOADBOOL 3 1 0
	313	LOADK 4 -68                   ; test 32 failed
	314	CALL 2 3 1
	315	RETURN 0 1

	locals (11)
		name: a; startpc: 1; endpc: 315
		name: (for index); startpc: 193; endpc: 198
		name: (for limit); startpc: 193; endpc: 198
		name: (for step); startpc: 193; endpc: 198
		name: i; startpc: 194; endpc: 197
		name: b; startpc: 254; endpc: 315
		name: (for generator); startpc: 288; endpc: 299
		name: (for state); startpc: 288; endpc: 299
		name: (for control); startpc: 288; endpc: 299
		name: k; startpc: 289; endpc: 297
		name: v; startpc: 289; endpc: 297
	upvales (1)
		name: _ENV; instack: true; index: 0
	constants (68)
		id: 0; value: 1337
		id: 1; value: assert
		id: 2; value: test 1 failed
		id: 3; value: 
		id: 4; value: test 2 failed
		id: 5; value: 1
		id: 6; value: 8
		id: 7; value: 2
		id: 8; value: 64
		id: 9; value: test 3 failed
		id: 10; value: 10
		id: 11; value: 20
		id: 12; value: test 4 failed
		id: 13; value: 5
		id: 14; value: 15
		id: 15; value: test 5 failed
		id: 16; value: 30
		id: 17; value: test 6 failed
		id: 18; value: 6
		id: 19; value: test 7 failed
		id: 20; value: 0
		id: 21; value: test 8 failed
		id: 22; value: 100000
		id: 23; value: test 9 failed
		id: 24; value: -100000
		id: 25; value: test 10 failed
		id: 26; value: -50000
		id: 27; value: test 11 failed
		id: 28; value: 32
		id: 29; value: test 12 failed
		id: 30; value: 96
		id: 31; value: test 13 failed
		id: 32; value: 98
		id: 33; value: test 14 failed
		id: 34; value: -99
		id: 35; value: test 15 failed
		id: 36; value: 4
		id: 37; value: 40
		id: 38; value: test 16 failed
		id: 39; value: test 17 failed
		id: 40; value: 39
		id: 41; value: test 18 failed
		id: 42; value: test 19 failed
		id: 43; value: 41
		id: 44; value: test 20 failed
		id: 45; value: test 21 failed
		id: 46; value: test 22 failed
		id: 47; value: Heyyy
		id: 48; value: test 23 failed
		id: 49; value: y
		id: 50; value: Heyyyyyyyyyyyyy
		id: 51; value: test 24 failed
		id: 52; value: test 25 failed
		id: 53; value: 100
		id: 54; value: test 26 failed
		id: 55; value: c
		id: 56; value: b
		id: 57; value: c
		id: 58; value: test 27 failed
		id: 59; value: test 28 failed
		id: 60; value: msg
		id: 61; value: test 29 failed
		id: 62; value: 69
		id: 63; value: test 30 failed
		id: 64; value: 3
		id: 65; value: ipairs
		id: 66; value: test 31 failed
		id: 67; value: test 32 failed
end

0 params; 0 varargs; 2 slots; 5 opcodes; 0 constants; 0 upvalues; 1 function; 1 local
function func1()
	001	CLOSURE 0 0                   ; func2
	002	MOVE 1 0
	003	TAILCALL 1 1 0
	004	RETURN 1 0
	005	RETURN 0 1

	locals (1)
		name: b; startpc: 1; endpc: 5
	upvales (0)
	constants (0)
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 0 upvalues; 0 functions; 0 locals
function func2()
	001	LOADK 0 -1                    ; 1337
	002	RETURN 0 2
	003	RETURN 0 1

	locals (0)
	upvales (0)
	constants (1)
		id: 0; value: 1337
end

1 param; 0 varargs; 2 slots; 3 opcodes; 1 constant; 0 upvalues; 0 functions; 1 local
function func3(param1)
	001	GETTABLE 1 0 -1               ; c
	002	RETURN 1 2
	003	RETURN 0 1

	locals (1)
		name: this; startpc: 0; endpc: 3
	upvales (0)
	constants (1)
		id: 0; value: c
end

2 params; 0 varargs; 2 slots; 2 opcodes; 0 constants; 1 upvalue; 0 functions; 2 locals
function func4(param1, param2)
	001	SETTABUP 0 0 1                ; a
	002	RETURN 0 1

	locals (2)
		name: key; startpc: 0; endpc: 2
		name: val; startpc: 0; endpc: 2
	upvales (1)
		name: a; instack: true; index: 0
	constants (0)
end

0 params; 0 varargs; 2 slots; 10 opcodes; 2 constants; 1 upvalue; 0 functions; 0 locals
function func5()
	001	GETUPVAL 0 0                  ; a
	002	MUL 0 0 -1                    ; - 9
	003	SETUPVAL 0 0                  ; a
	004	GETUPVAL 0 0                  ; a
	005	ADD 0 0 -2                    ; - 6
	006	SETUPVAL 0 0                  ; a
	007	GETUPVAL 0 0                  ; a
	008	ADD 0 0 -1                    ; - 9
	009	SETUPVAL 0 0                  ; a
	010	RETURN 0 1

	locals (0)
	upvales (1)
		name: a; instack: true; index: 0
	constants (2)
		id: 0; value: 9
		id: 1; value: 6
end

0 params; n varargs; 8 slots; 13 opcodes; 2 constants; 1 upvalue; 0 functions; 7 locals
function func6(...)
	001	NEWTABLE 0 0 0
	002	VARARG 1 0
	003	SETLIST 0 0 1                 ; 1
	004	LOADK 1 -1                    ; 0
	005	GETTABUP 2 0 -2               ; _ENV ipairs
	006	MOVE 3 0
	007	CALL 2 2 4
	008	JMP 0 1                       ; to pc 10
	009	ADD 1 1 
	010	TFORCALL 2 2
	011	TFORLOOP 4 -3                 ; to pc 9
	012	RETURN 1 2
	013	RETURN 0 1

	locals (7)
		name: vals; startpc: 3; endpc: 13
		name: total; startpc: 4; endpc: 13
		name: (for generator); startpc: 7; endpc: 11
		name: (for state); startpc: 7; endpc: 11
		name: (for control); startpc: 7; endpc: 11
		name: k; startpc: 8; endpc: 9
		name: v; startpc: 8; endpc: 9
	upvales (1)
		name: _ENV; instack: false; index: 0
	constants (2)
		id: 0; value: 0
		id: 1; value: ipairs
end
```
## Example 3
Original lua code:
```lua
local Vector3 = {}
Vector3.New = function(x, y, z)
	local this = {}
    
    this.x = x
    this.y = y
    this.z = z
    
    return this
end

local Enemy = {}
Enemy.NextId = 1
Enemy.New = function(name, health, xp, damage)
	local this = {}
    
    local Id = Enemy.NextId
    local Name = name
    local Health = health
    local Position = Vector3.New(10.0, 10.0, 10.0)
    local Dead = false
    local Xp = xp
    local Damage = damage
    local Player = nil
    local MaxHealth = health
    
    function this.Reset()
    	Health = MaxHealth
        Dead = false
        Player = nil
    end
    
    function this.ReadValue(key)
    	local data = {}
        data["id"] = Id
        data["name"] = Name
        data["health"] = Health
        data["x"] = Position.x
        data["y"] = Position.y
        data["z"] = Position.z
        data["dead"] = Dead
        data["xp"] = Xp
        data["damage"] = Damage
        data["player"] = Player
        data["maxhealth"] = MaxHealth
        return data[key]
    end
    
    function this.PrintStatus()
    	print("-----ENEMY INFO-----")
        print("Name: "..Name)
        print("Id: "..Id)
        print("Health: "..Health)
        print("XP: "..Xp)
        print("Position: "..Position.x..", "..Position.y..", "..Position.z)
        print("Attack Damage: "..Damage)
        print("--------------------")
    end
    
    function this.GetName()
    	return Name
    end
    
    function this.SetPosition(x, y, z)
    	Position.x = x
        Position.y = y
        Position.z = z
    end
    
    function this.GetX()
    	return Position.x
    end
    
    function this.GetY()
    	return Position.y
    end
    
    function this.GetZ()
    	return Position.z
    end
    
    function this.GetAttacked(damage)
    	Health = Health - damage
        if (Health <= 0) then
        	Dead = true
        end
    end
    
    function this.IsDead()
    	return Dead
    end
    
    function this.GetXp()
    	if (this.IsDead()) then
    		return Xp
        end
        return 0
    end
    
    function this.Attack()
    	if (Player ~= nil) then
			Player.GetAttacked(Damage)
    	end
    end
    
    function this.SetPlayer(player)
    	Player = player
    end
    
    Enemy.NextId = Enemy.NextId + 1
    return this
end

local Player = {}
Player.NextId = 1
Player.New = function(name)
	local this = {}
    
    local Id = Player.NextId
    local Name = name
    local Health = 10
    local Level = 1
    local Xp = 0
    local Position = Vector3.New(0.0, 0.0, 0.0)
    local Enemy = nil
    local InBattle = false
    local Damage = 1
    local MaxHealth = 10
    local Flagged = false
    local Banned = false
    
    function this.ReadValue(key)
    	local data = {}
        data["id"] = Id
        data["name"] = Name
        data["health"] = Health
        data["level"] = Level
        data["x"] = Position.x
        data["y"] = Position.y
        data["z"] = Position.z
        data["xp"] = Xp
        data["damage"] = Damage
        data["enemy"] = Enemy
        data["maxhealth"] = MaxHealth
        data["inbattle"] = InBattle
        data["flagged"] = Flagged
        data["banned"] = Banned
        return data[key]
    end
    
    function this.PrintStatus()
    	print("-----PLAYER INFO-----")
        print("Name: "..Name)
        print("Id: "..Id)
        print("Health: "..Health)
        print("Level: "..Level)
        print("XP: "..Xp)
        print("Position: "..Position.x..", "..Position.y..", "..Position.z)
        local b = "false"
        if (InBattle) then
        	b = "true"
        end
        print("In Battle: "..b)
        print("Attack Damage: "..Damage)
        print("Max Health: "..MaxHealth)
        b = "false"
        if (Banned) then
        	b = "true"
        end
        print("Banned: "..b)
        print("---------------------")
    end
    
    function this.GetName()
    	return Name
    end
    
    function this.SetPosition(x, y, z)
    	Position.x = x
        Position.y = y
        Position.z = z
    end
    
    function this.GetX()
    	return Position.x
    end
    
    function this.GetY()
    	return Position.y
    end
    
    function this.GetZ()
    	return Position.z
    end
    
    local function AttemptLevelUp()
    	if (Level == 10 or Banned) then
        	return
        end
    	local xpTable = { 0, 5, 10, 20, 50, 100, 150, 200, 300, 500 }
        local neededXp = xpTable[Level + 1]
        while (Xp >= neededXp) do
        	Level = Level + 1
        	Xp = Xp - neededXp
            MaxHealth = MaxHealth + 5
            Health = MaxHealth
            Damage = Damage + 1
            neededXp = xpTable[Level + 1]
            if (neededXp == nil) then
            	break
            end
        end
        if (Level == 10) then
        	MaxHealth = 100
        	Health = MaxHealth
            Damage = 30
        end
    end
    
    function this.GainXp(xp)
    	Xp = Xp + xp
        AttemptLevelUp()
    end
    
    function this.BattleEnemy(enemy)
    	Enemy = enemy
        Enemy.SetPlayer(this)
        InBattle = true
    end
    
    function this.IsInBattle()
    	return InBattle
    end
    
    function this.Attack()
    	if (Banned) then
        	return
        end
    	if (this.IsInBattle()) then
        	if (Enemy ~= nil) then
            	Enemy.GetAttacked(Damage)
            end
        end
    end
    
    function this.GetAttacked(damage)
    	Health = Health - damage
        if (Health <= 0) then
			Dead = true
        end
    end
    
    function this.IsDead()
    	return Dead
    end
    
    function this.ExitBattle()
    	InBattle = false
        Enemy = nil
    end
    
    function this.Reset()
        this.ExitBattle()
        Xp = 0
        Level = 1
        Damage = 1
        MaxHealth = 10
        Health = MaxHealth
        Position = Vector3.New(0.0, 0.0, 0.0)
        Dead = false
    end
    
    function this.DonateXp(player, xp)
    	if (Xp >= Xp - xp) then
        	Xp = Xp - xp
            player.GainXp(xp)
        end
    end
    
    function this.Cheat()
    	Level = 99
        Xp = 9999
        Damage = 99
        MaxHealth = 9999
        Health = MaxHealth
        Flagged = true
    end
    
    function this.IsFlagged()
    	return Flagged
    end
    
    function this.Ban()
    	Banned = true
        this.Reset()
        
    end
    
    function this.IsBanned()
    	return Banned
    end
    
    Player.NextId = Player.NextId + 1
    return this
end

function StartBattle(player, enemy)
	if (player.IsBanned()) then
    	return false
    end
	if (player.GetX() == enemy.GetX() and
    	player.GetY() == enemy.GetY() and
        player.GetZ() == enemy.GetZ()) then
        player.BattleEnemy(enemy)
        local pname = player.GetName()
        local ename = enemy.GetName()
        print("Battle started")
        print(pname.." vs "..ename)
        while true do
        	print(pname.." attacks")
        	player.Attack()
            if (enemy.IsDead()) then
            	print(ename.. " died")
                local xp = enemy.GetXp()
                print("Gained "..xp.." XP")
            	player.GainXp(xp)
                player.ExitBattle()
            	break
            end
            print(ename.." attacks")
            enemy.Attack()
            if (player.IsDead()) then
            	print(pname.." died")
                print("Reset to Level 1")
            	player.Reset()
                break
            end
        end
        print("Battle ended")
        print(" ")
        return true
    end
    return false
end

local Administrator = {}
Administrator.New = function()
	local this = {}
    
    function this.BanPlayer(player)
    	if (this.IsCheater(player)) then
        	player.Ban()
            return true
        end
        return false
    end
    
    function this.IsCheater(player)
    	return player.IsFlagged()
    end
    
    return this
end

local Factory = {}
Factory.New = function()
	local this = {}
    
    local Players = {}
    local Enemies = {}
    local Admin = nil
    
    function this.GeneratePlayer(name)
    	local player = Player.New(name)
        Players[Player.NextId - 1] = player
    	return player
    end
    
    function this.GenerateEnemy(name, health, xp, damage)
    	local enemy = Enemy.New(name, health, xp, damage)
        Enemies[Enemy.NextId - 1] = enemy
    	return enemy
    end
    
    function this.GenerateAdmin()
    	if (Admin ~= nil) then
        	return Admin
        end
    	Admin = Administrator.New()
        return Admin
    end
    
    function this.GetPlayers()
    	return Players
    end
    
    function this.GetEnemies()
    	return Enemies
    end
    
    return this
end


-- create entity generator
local generator = Factory.New()
print("Created factory")

-- create main player
local player = generator.GeneratePlayer("Jeff")
print("Created player "..player.GetName())

-- create second player
local friend = generator.GeneratePlayer("Jay")
print("Created player "..friend.GetName())

-- create admin
local admin = generator.GenerateAdmin()
print("Created admin")

-- create enemies
local enemy1 = generator.GenerateEnemy("Joe", 10, 5, 1)
local enemy2 = generator.GenerateEnemy("Jeffrey", 30, 20, 4)
local enemy3 = generator.GenerateEnemy("John", 40, 60, 5)
local enemy4 = generator.GenerateEnemy("Jane", 60, 100, 3)
local enemy5 = generator.GenerateEnemy("Jill", 100, 300, 5)
local boss = generator.GenerateEnemy("Johnny", 200, 1000, 10)

-- set positions
enemy1.SetPosition(5.0, 5.0, 5.0)
enemy2.SetPosition(10.0, 10.0, 10.0)
enemy3.SetPosition(15.0, 15.0, 15.0)
enemy4.SetPosition(100.0, 10.0, 30.0)
enemy5.SetPosition(200.0, 0.0, 0.0)
boss.SetPosition(300.0, 200.0, 100.0)

-- try to battle an enemy that's not nearby, this shouldn't work
assert(StartBattle(player, enemy3) == false, "Test 1 failed")

-- go to enemy1
player.SetPosition(5.0, 5.0, 5.0)

-- print status before battle
player.PrintStatus()
enemy1.PrintStatus()
print(" ")

-- start battle against enemy1
assert(StartBattle(player, enemy1) == true, "Test 2 failed")

-- print status after battle
player.PrintStatus()
enemy1.PrintStatus()
print(" ")

-- go to enemy2
player.SetPosition(10.0, 10.0, 10.0)

-- start battle against enemy2
assert(StartBattle(player, enemy2) == true, "Test 3 failed")

-- print status after battle
player.PrintStatus()
enemy2.PrintStatus()
print(" ")

-- reset enemy 1 and 2
enemy1.Reset()
enemy2.Reset()

-- player is now level 1 again, gain some xp
player.GainXp(10)
print(player.GetName().." gained 10 XP")
player.PrintStatus()
print(" ")
player.GainXp(20)
print(player.GetName().." gained 20 XP")
player.PrintStatus()
print(" ")

-- go to enemy1 again
player.SetPosition(5.0, 5.0, 5.0)

-- start battle against enemy1 again
assert(StartBattle(player, enemy1) == true, "Test 4 failed")

-- print status after battle
player.PrintStatus()
enemy1.PrintStatus()
print(" ")

-- go to enemy2 again
player.SetPosition(10.0, 10.0, 10.0)

-- start battle against enemy2 again
assert(StartBattle(player, enemy2) == true, "Test 5 failed")

-- print status after battle
player.PrintStatus()
enemy2.PrintStatus()
print(" ")

-- go to enemy3
player.SetPosition(enemy3.GetX(), enemy3.GetY(), enemy3.GetZ())

-- start battle against enemy3
assert(StartBattle(player, enemy3) == true, "Test 6 failed")

-- print status after battle
player.PrintStatus()
enemy3.PrintStatus()
print(" ")

-- gain some xp
player.GainXp(50)
print(player.GetName().." gained 50 XP")
player.PrintStatus()
print(" ")

-- go to enemy4
player.SetPosition(enemy4.GetX(), enemy4.GetY(), enemy4.GetZ())

-- start battle against enemy4
assert(StartBattle(player, enemy4) == true, "Test 7 failed")

-- print status after battle
player.PrintStatus()
enemy4.PrintStatus()
print(" ")

-- gain some xp
player.GainXp(150)
print(player.GetName().." gained 150 XP")
player.PrintStatus()
print(" ")
player.GainXp(150)
print(player.GetName().." gained 150 XP")
player.PrintStatus()
print(" ")

-- go to enemy5
player.SetPosition(enemy5.GetX(), enemy5.GetY(), enemy5.GetZ())

-- start battle against enemy5
assert(StartBattle(player, enemy5) == true, "Test 8 failed")

-- print status after battle
player.PrintStatus()
enemy5.PrintStatus()
print(" ")

-- gain some xp
player.GainXp(300)
print(player.GetName().." gained 300 XP")
player.PrintStatus()
print(" ")
player.GainXp(300)
print(player.GetName().." gained 300 XP")
player.PrintStatus()
print(" ")
player.GainXp(300)
print(player.GetName().." gained 300 XP")
player.PrintStatus()
print(" ")
player.GainXp(400)
print(player.GetName().." gained 400 XP")
player.PrintStatus()
print(" ")
player.GainXp(500)
print(player.GetName().." gained 500 XP")
player.PrintStatus()
print(" ")
player.GainXp(600)
print(player.GetName().." gained 600 XP")
player.PrintStatus()
print(" ")

-- go to boss
player.SetPosition(300.0, 200.0, 100.0)

-- start battle against boss
assert(StartBattle(player, boss) == true, "Test 9 failed")

-- print status after battle
player.PrintStatus()
boss.PrintStatus()
print(" ")

-- test xp donation
print(player.GetName().." gives 1015 xp to "..friend.GetName())
player.DonateXp(friend, 1015)
player.PrintStatus()
friend.PrintStatus()
print(" ")

-- friend is a cheater
print(friend.GetName().. " cheats to level 99")
friend.Cheat()
friend.PrintStatus()
print(" ")

-- admin notices that and bans them
print("Banning "..friend.GetName())
assert(admin.BanPlayer(friend) == true, "Test 10 failed")

-- print status after ban
friend.PrintStatus()
print(" ")

-- reset enemy1
enemy1.Reset()

-- go to enemy1 again
friend.SetPosition(5.0, 5.0, 5.0)
player.SetPosition(5.0, 5.0, 5.0)

-- start battle against enemy1 again
assert(StartBattle(friend, enemy1) == false, "Test 11 failed")
assert(StartBattle(player, enemy1) == true, "Test 12 failed")

-- print status after battle
player.PrintStatus()
friend.PrintStatus()
enemy1.PrintStatus()
enemy2.PrintStatus()
enemy3.PrintStatus()
enemy4.PrintStatus()
enemy5.PrintStatus()
boss.PrintStatus()
print(" ")

-- assertions
assert(player.ReadValue("id") == 1, "Test 13 failed")
assert(player.ReadValue("name") == "Jeff", "Test 14 failed")
assert(player.ReadValue("health") == 40, "Test 15 failed")
assert(player.ReadValue("level") == 10, "Test 16 failed")
assert(player.ReadValue("x") == 5.0, "Test 17 failed")
assert(player.ReadValue("y") == 5.0, "Test 18 failed")
assert(player.ReadValue("z") == 5.0, "Test 19 failed")
assert(player.ReadValue("xp") == 1055, "Test 20 failed")
assert(player.ReadValue("damage") == 30, "Test 21 failed")
assert(player.ReadValue("maxhealth") == 100, "Test 22 failed")
assert(player.ReadValue("inbattle") == false, "Test 23 failed")
assert(player.ReadValue("banned") == false, "Test 24 failed")
assert(player.ReadValue("flagged") == false, "Test 25 failed")

assert(friend.ReadValue("id") == 2, "Test 26 failed")
assert(friend.ReadValue("name") == "Jay", "Test 27 failed")
assert(friend.ReadValue("health") == 10, "Test 28 failed")
assert(friend.ReadValue("level") == 1, "Test 29 failed")
assert(friend.ReadValue("x") == 5.0, "Test 30 failed")
assert(friend.ReadValue("y") == 5.0, "Test 31 failed")
assert(friend.ReadValue("z") == 5.0, "Test 32 failed")
assert(friend.ReadValue("xp") == 0, "Test 33 failed")
assert(friend.ReadValue("damage") == 1, "Test 34 failed")
assert(friend.ReadValue("maxhealth") == 10, "Test 35 failed")
assert(friend.ReadValue("inbattle") == false, "Test 36 failed")
assert(friend.ReadValue("banned") == true, "Test 37 failed")
assert(friend.ReadValue("flagged") == true, "Test 38 failed")

assert(enemy1.ReadValue("name") == "Joe", "Test 39 failed")
assert(enemy1.ReadValue("id") == 1, "Test 40 failed")
assert(enemy1.ReadValue("health") == -20, "Test 41 failed")
assert(enemy1.ReadValue("xp") == 5, "Test 42 failed")
assert(enemy1.ReadValue("x") == 5.0, "Test 43 failed")
assert(enemy1.ReadValue("y") == 5.0, "Test 44 failed")
assert(enemy1.ReadValue("z") == 5.0, "Test 45 failed")
assert(enemy1.ReadValue("damage") == 1, "Test 46 failed")
assert(enemy1.ReadValue("maxhealth") == 10, "Test 47 failed")

assert(enemy2.ReadValue("name") == "Jeffrey", "Test 48 failed")
assert(enemy2.ReadValue("id") == 2, "Test 49 failed")
assert(enemy2.ReadValue("health") == 2, "Test 50 failed")
assert(enemy2.ReadValue("xp") == 20, "Test 51 failed")
assert(enemy2.ReadValue("x") == 10.0, "Test 52 failed")
assert(enemy2.ReadValue("y") == 10.0, "Test 53 failed")
assert(enemy2.ReadValue("z") == 10.0, "Test 54 failed")
assert(enemy2.ReadValue("damage") == 4, "Test 55 failed")
assert(enemy2.ReadValue("maxhealth") == 30, "Test 56 failed")

assert(enemy3.ReadValue("name") == "John", "Test 57 failed")
assert(enemy3.ReadValue("id") == 3, "Test 58 failed")
assert(enemy3.ReadValue("health") == 38, "Test 59 failed")
assert(enemy3.ReadValue("xp") == 60, "Test 60 failed")
assert(enemy3.ReadValue("x") == 15.0, "Test 61 failed")
assert(enemy3.ReadValue("y") == 15.0, "Test 62 failed")
assert(enemy3.ReadValue("z") == 15.0, "Test 63 failed")
assert(enemy3.ReadValue("damage") == 5, "Test 64 failed")
assert(enemy3.ReadValue("maxhealth") == 40, "Test 65 failed")

assert(enemy4.ReadValue("name") == "Jane", "Test 66 failed")
assert(enemy4.ReadValue("id") == 4, "Test 67 failed")
assert(enemy4.ReadValue("health") == 24, "Test 68 failed")
assert(enemy4.ReadValue("xp") == 100, "Test 69 failed")
assert(enemy4.ReadValue("x") == 100.0, "Test 70 failed")
assert(enemy4.ReadValue("y") == 10.0, "Test 71 failed")
assert(enemy4.ReadValue("z") == 30.0, "Test 72 failed")
assert(enemy4.ReadValue("damage") == 3, "Test 73 failed")
assert(enemy4.ReadValue("maxhealth") == 60, "Test 74 failed")

assert(enemy5.ReadValue("name") == "Jill", "Test 75 failed")
assert(enemy5.ReadValue("id") == 5, "Test 76 failed")
assert(enemy5.ReadValue("health") == 58, "Test 77 failed")
assert(enemy5.ReadValue("xp") == 300, "Test 78 failed")
assert(enemy5.ReadValue("x") == 200.0, "Test 79 failed")
assert(enemy5.ReadValue("y") == 0.0, "Test 80 failed")
assert(enemy5.ReadValue("z") == 0.0, "Test 81 failed")
assert(enemy5.ReadValue("damage") == 5, "Test 82 failed")
assert(enemy5.ReadValue("maxhealth") == 100, "Test 83 failed")

assert(boss.ReadValue("name") == "Johnny", "Test 84 failed")
assert(boss.ReadValue("id") == 6, "Test 85 failed")
assert(boss.ReadValue("health") == -10, "Test 86 failed")
assert(boss.ReadValue("xp") == 1000, "Test 87 failed")
assert(boss.ReadValue("x") == 300.0, "Test 88 failed")
assert(boss.ReadValue("y") == 200.0, "Test 89 failed")
assert(boss.ReadValue("z") == 100.0, "Test 90 failed")
assert(boss.ReadValue("damage") == 10, "Test 91 failed")
assert(boss.ReadValue("maxhealth") == 200, "Test 92 failed")

assert(player ~= nil, "Test 93 failed")
assert(friend ~= nil, "Test 94 failed")
assert(enemy1 ~= nil, "Test 95 failed")
assert(enemy2 ~= nil, "Test 96 failed")
assert(enemy3 ~= nil, "Test 97 failed")
assert(enemy4 ~= nil, "Test 98 failed")
assert(enemy5 ~= nil, "Test 99 failed")
assert(boss ~= nil, "Test 100 failed")
```
Compiled luac binary:
```
LuaS “

xV           (w@@/dev/shm/luac.nlHyGPPv         ¥     l   
@ €K   J€À€¬@  J€ €‹   Š Áì€  ŠÀ€‚ìÀ  À ƒË   , Ê ƒ  lA 
AƒGBd€ †AB Á ¤A ‡ÁÂÁ ¤ ÆAB B G‚Cd‚€ BäA ÇÁÂÂ ä BB AB ‡‚Ã¤‚€ ]‚‚$B Ä$‚€ FBB B dB G‚ÄÂ Á C Aƒ  d‚€‡‚ÄÁ‚ Ã A C ¤‚€Ç‚Äƒ AÃ  ÁC ä‚€ƒÄAC  Áƒ Ä $ƒ€GƒÄ Áƒ D AD dƒ€‡ƒÄÁƒ Ä A	  ¤ƒ€ÇCÉ„	 A„	 „	 äC ÇCIÄ	 AÄ	 Ä	 äC ÇCÉ
 A
 
 äC ÇCID
 AÄ	 „
 äC ÇCÉÄ
 A  äC ÇCID AÄ
 D
 äC ÆƒK „A @ €€$„€_ÀK  €D  € A äC€ÇCI„	 A„	 „	 äC ÇCLäC€ ÇCÌäC€ ÆCB „ äC ÆƒK „A @ €€$„€_ÀL  €D  € A
 äC€ÇCLäC€ ÇCÌäC€ ÆCB „ äC ÇCIÄ	 AÄ	 Ä	 äC ÆƒK „A @ € $„€_ÀL  €D  € AD
 äC€ÇCLäC€ ÇCLäC€ ÆCB „ äC ÇƒÍäC€ ÇƒMäC€ ÇÃM äC ÆCB „C$„€ A DäC ÇCLäC€ ÆCB „ äC ÇÃM äC ÆCB „C$„€ AD DäC ÇCLäC€ ÆCB „ äC ÇCI„	 A„	 „	 äC ÆƒK „A @ €€$„€_ÀL  €D  € A„ äC€ÇCLäC€ ÇCÌäC€ ÆCB „ äC ÇCIÄ	 AÄ	 Ä	 äC ÆƒK „A @ € $„€_ÀL  €D  € AÄ äC€ÇCLäC€ ÇCLäC€ ÆCB „ äC ÇCIÏ$„€ GDÏd„€ ‡„Ï¤€ äC  ÆƒK „A @ €€$„€_ÀL  €D  € AÄ äC€ÇCLäC€ ÇCÌäC€ ÆCB „ äC ÇÃM äC ÆCB „C$„€ AD DäC ÇCLäC€ ÆCB „ äC ÇCIO$„€ GDOd„€ ‡„O¤€ äC  ÆƒK „A @ € $„€_ÀL  €D  € A„ äC€ÇCLäC€ ÇCLäC€ ÆCB „ äC ÇÃMÄ äC ÆCB „C$„€ A DäC ÇCLäC€ ÆCB „ äC ÇÃMÄ äC ÆCB „C$„€ A DäC ÇCLäC€ ÆCB „ äC ÇCIÏ$„€ GDÏd„€ ‡„Ï¤€ äC  ÆƒK „A @ €€$„€_ÀL  €D  € AD äC€ÇCLäC€ ÇCÌäC€ ÆCB „ äC ÇÃMD äC ÆCB „C$„€ A„ DäC ÇCLäC€ ÆCB „ äC ÇÃMD äC ÆCB „C$„€ A„ DäC ÇCLäC€ ÆCB „ äC ÇÃMD äC ÆCB „C$„€ A„ DäC ÇCLäC€ ÆCB „ äC ÇÃMÄ äC ÆCB „C$„€ A DäC ÇCLäC€ ÆCB „ äC ÇÃMD äC ÆCB „C$„€ A„ DäC ÇCLäC€ ÆCB „ äC ÇÃMÄ äC ÆCB „C$„€ A DäC ÇCLäC€ ÆCB „ äC ÇCID AÄ
 D
 äC ÆƒK „A @ € $„€_ÀL  €D  € AD äC€ÇCLäC€ ÇCLäC€ ÆCB „ äC ÆCB „C$„€ A„ ‡„Ã¤„€ „äC ÇÃS €A äC€ÇCLäC€ ÇCÌäC€ ÆCB „ äC ÆCB „Ã$„€ AD DäC ÇƒÔäC€ ÇCÌäC€ ÆCB „ äC ÆCB Ä G„Ãd„€ DäC ÆƒK U@€$„ _ÀL  €D  € AD äC€ÇCÌäC€ ÆCB „ äC ÇƒÍäC€ ÇCÉ„	 A„	 „	 äC ÇCI„	 A„	 „	 äC ÆƒK „A @€€€$„€_ÀK  €D  € A„ äC€ÆƒK „A @ €€$„€_ÀL  €D  € AÄ äC€ÇCLäC€ ÇCÌäC€ ÇCÌäC€ ÇCLäC€ ÇCÌäC€ ÇCLäC€ ÇCÌäC€ ÇCLäC€ ÆCB „ äC ÆƒK VAD $„ _€@  €D  € A„ äC€ÆƒK VAÄ $„ _ C  €D  € A äC€ÆƒK VAD $„ _ÀF  €D  € A„ äC€ÆƒK VAÄ $„ _ E  €D  € A äC€ÆƒK VAD $„ _€I  €D  € A„ äC€ÆƒK VAÄ $„ _€I  €D  € A äC€ÆƒK VAD $„ _€I  €D  € A„ äC€ÆƒK VAÄ $„ _ Z  €D  € AD äC€ÆƒK VA„ $„ _ÀE  €D  € AÄ äC€ÆƒK VA $„ _€G  €D  € AD äC€ÆƒK VA„ $„ _ÀK  €D  € AÄ äC€ÆƒK VA $„ _ÀK  €D  € AD äC€ÆƒK VA„ $„ _ÀK  €D  € AÄ äC€ÆƒK ÖAD $„ _ ]  €D  € AD äC€ÆƒK ÖAÄ $„ _ÀC  €D  € A„ äC€ÆƒK ÖAD $„ _ E  €D  € AÄ äC€ÆƒK ÖAÄ $„ _€@  €D  € A äC€ÆƒK ÖAD $„ _€I  €D  € AD äC€ÆƒK ÖAÄ $„ _€I  €D  € A„ äC€ÆƒK ÖAD $„ _€I  €D  € AÄ äC€ÆƒK ÖAÄ $„ _ _  €D  € AD äC€ÆƒK ÖA„ $„ _€@  €D  € A„ äC€ÆƒK ÖA $„ _ E  €D  € AÄ äC€ÆƒK ÖA„ $„ _ÀK  €D  € A  äC€ÆƒK ÖA $„ _ÀL  €D  € AD  äC€ÆƒK ÖA„ $„ _ÀL  €D  € A„  äC€ÆƒK ÖAÄ $„ _ÀD  €D  € AÄ  äC€ÆƒK ÖAD $„ _€@  €D  € A! äC€ÆƒK ÖAD $„ _@a  €D  € A„! äC€ÆƒK ÖAÄ $„ _@E  €D  € AÄ! äC€ÆƒK ÖAD $„ _€I  €D  € A" äC€ÆƒK ÖAÄ $„ _€I  €D  € AD" äC€ÆƒK ÖAD $„ _€I  €D  € A„" äC€ÆƒK ÖA„ $„ _€@  €D  € AÄ" äC€ÆƒK ÖA $„ _ E  €D  € A# äC€ÆƒK VAÄ $„ _€E  €D  € AD# äC€ÆƒK VAD $„ _ ]  €D  € A„# äC€ÆƒK VAD $„ _ ]  €D  € AÄ# äC€ÆƒK VAÄ $„ _ F  €D  € A$ äC€ÆƒK VAD $„ _ÀI  €D  € AD$ äC€ÆƒK VAÄ $„ _ÀI  €D  € A„$ äC€ÆƒK VAD $„ _ÀI  €D  € AÄ$ äC€ÆƒK VA„ $„ _@F  €D  € A% äC€ÆƒK VA $„ _ÀE  €D  € AD% äC€ÆƒK ÖAÄ $„ _€F  €D  € A„% äC€ÆƒK ÖAD $„ _ÀG  €D  € AÄ% äC€ÆƒK ÖAD $„ _ f  €D  € AD& äC€ÆƒK ÖAÄ $„ _ G  €D  € A„& äC€ÆƒK ÖAD $„ _ J  €D  € AÄ& äC€ÆƒK ÖAÄ $„ _ J  €D  € A' äC€ÆƒK ÖAD $„ _ J  €D  € AD' äC€ÆƒK ÖA„ $„ _@E  €D  € A„' äC€ÆƒK ÖA $„ _ÀF  €D  € AÄ' äC€ÆƒK VAÄ $„ _@G  €D  € A( äC€ÆƒK VAD $„ _@F  €D  € AD( äC€ÆƒK VAD $„ _€h  €D  € AÄ( äC€ÆƒK VAÄ $„ _€G  €D  € A) äC€ÆƒK VAD $„ _@J  €D  € AD) äC€ÆƒK VAÄ $„ _ÀI  €D  € A„) äC€ÆƒK VAD $„ _€J  €D  € AÄ) äC€ÆƒK VA„ $„ _ÀG  €D  € A* äC€ÆƒK VA $„ _ G  €D  € AD* äC€ÆƒK ÖAÄ $„ _ H  €D  € A„* äC€ÆƒK ÖAD $„ _@E  €D  € AÄ* äC€ÆƒK ÖAD $„ _ k  €D  € AD+ äC€ÆƒK ÖAÄ $„ _@H  €D  € A„+ äC€ÆƒK ÖAD $„ _ÀJ  €D  € AÄ+ äC€ÆƒK ÖAÄ $„ _ K  €D  € A, äC€ÆƒK ÖAD $„ _ K  €D  € AD, äC€ÆƒK ÖA„ $„ _@E  €D  € A„, äC€ÆƒK ÖA $„ _€G  €D  € AÄ, äC€ÆƒK VAÄ $„ _€H  €D  € A- äC€ÆƒK VAD $„ _@m  €D  € A„- äC€ÆƒK VAD $„ _Àm  €D  € A. äC€ÆƒK VAÄ $„ _ I  €D  € AD. äC€ÆƒK VAD $„ _@K  €D  € A„. äC€ÆƒK VAÄ $„ _ÀJ  €D  € AÄ. äC€ÆƒK VAD $„ _@J  €D  € A/ äC€ÆƒK VA„ $„ _ E  €D  € AD/ äC€ÆƒK VA $„ _ÀH  €D  € A„/ äC€ÆƒK Ào  €D  € A0 äC€ÆƒK Àï  €D  € AD0 äC€ÆƒK Àï  €D  € A„0 äC€ÆƒK Ào  €D  € AÄ0 äC€ÆƒK Àï  €D  € A1 äC€ÆƒK Ào  €D  € AD1 äC€ÆƒK Àï  €D  € A„1 äC€ÆƒK Ào  €D  € AÄ1 äC€& € È   NewNextId       NextId       NewStartBattleNewNewprintCreated factoryGeneratePlayerJeffCreated player GetNameJayGenerateAdminCreated adminGenerateEnemyJoe
              Jeffrey                     John(       <       Janed              Jill,      JohnnyÈ       è      SetPosition      @      $@      .@      Y@      >@      i@             Àr@assert Test 1 failedPrintStatus Test 2 failedTest 3 failedResetGainXp gained 10 XP gained 20 XPTest 4 failedTest 5 failedGetXGetYGetZTest 6 failed2        gained 50 XPTest 7 failed–        gained 150 XPTest 8 failed gained 300 XP       gained 400 XPô       gained 500 XPX       gained 600 XPTest 9 failed gives 1015 xp to 	DonateXp÷       cheats to level 99Cheat	Banning 
BanPlayerTest 10 failedTest 11 failedTest 12 failed
ReadValueidTest 13 failednameTest 14 failedhealthTest 15 failedlevelTest 16 failedxTest 17 failedyTest 18 failedzTest 19 failedxp      Test 20 faileddamageTest 21 failed
maxhealthTest 22 failed	inbattleTest 23 failedbannedTest 24 failedflaggedTest 25 failed       Test 26 failedTest 27 failedTest 28 failedTest 29 failedTest 30 failedTest 31 failedTest 32 failed        Test 33 failedTest 34 failedTest 35 failedTest 36 failedTest 37 failedTest 38 failedTest 39 failedTest 40 failedìÿÿÿÿÿÿÿTest 41 failedTest 42 failedTest 43 failedTest 44 failedTest 45 failedTest 46 failedTest 47 failedTest 48 failedTest 49 failedTest 50 failedTest 51 failedTest 52 failedTest 53 failedTest 54 failedTest 55 failedTest 56 failedTest 57 failedTest 58 failed&       Test 59 failedTest 60 failedTest 61 failedTest 62 failedTest 63 failedTest 64 failedTest 65 failedTest 66 failedTest 67 failed       Test 68 failedTest 69 failedTest 70 failedTest 71 failedTest 72 failedTest 73 failedTest 74 failedTest 75 failedTest 76 failed:       Test 77 failedTest 78 failedTest 79 failedTest 80 failedTest 81 failedTest 82 failedTest 83 failedTest 84 failed       Test 85 failedöÿÿÿÿÿÿÿTest 86 failedTest 87 failedTest 88 failedTest 89 failedTest 90 failedTest 91 failedTest 92 failed Test 93 failedTest 94 failedTest 95 failedTest 96 failedTest 97 failedTest 98 failedTest 99 failedTest 100 failed           
       Ë   Ê  €Ê@€€Ê€ æ  & €    xyz                       	   
      x       y       z       this              p    -     F@ €  À€ BÀ A‚  ‚  Á‚  $‚ C  € À€  @€ ¬  
ƒ¬C  
‚¬ƒ  
ƒ‚¬Ã  
ƒ¬ 
ƒƒ¬C 
„¬ƒ 
ƒ„¬Ã 
…¬ 
ƒ…¬C 
†¬ƒ 
ƒ†¬Ã 
‡¬ 
ƒ‡†@ D€€& & €    NextIdNew      $@Reset
ReadValuePrintStatusGetNameSetPositionGetXGetYGetZGetAttackedIsDeadGetXpAttack
SetPlayer             
                € 	      	     	 €& €        
	                                   Health
MaxHealthDeadPlayer !   /       K   …   J€ €… € J€€€…  J€ †ÀÀJ€€† ÁJ€ ‚†@ÁJ€€‚…  J€ ƒ… €J€€ƒ…  J€ „… €J€€„…  J€ …‡ € ¦  & €    idnamehealthxyzdeadxpdamageplayer
maxhealth	   	

       "   #   #   $   $   %   %   &   &   '   '   (   (   )   )   *   *   +   +   ,   ,   -   -   .   .   /      key       data      	   IdNameHealth	PositionDeadXpDamagePlayer
MaxHealth 1   :     )    @ A@  $@  @ A€  … € ]€€ $@  @ AÀ  …  ]€€ $@  @ A  … €]€€ $@  @ A@ …  ]€€ $@  @ A€ †ÀÁÁ  AÂA †Â]€ $@  @ AÀ …  ]€€ $@  @ A  $@ & € 
   print-----ENEMY INFO-----Name: Id: 	Health: XP: Position: x, yzAttack Damage: --------------------    
    )   2   2   2   3   3   3   3   3   4   4   4   4   4   5   5   5   5   5   6   6   6   6   6   7   7   7   7   7   7   7   7   7   8   8   8   8   8   9   9   9   :          _ENVNameIdHealthXp	PositionDamage <   >           &  & €               =   =   >          Name @   D         €@€€€ & €    xyz          A   B   C   D      x       y       z          	Position F   H         @ &  & €    x          G   G   H          	Position J   L         @ &  & €    y          K   K   L          	Position N   P         @ &  & €    z          O   O   P          	Position R   W    	   E   N € I   E   ! À @ €C € I € & €               	    	   S   S   S   T   T   T   U   U   W      damage    	      HealthDead Y   [           &  & €        	       Z   Z   [          Dead ]   b     	    @ $€€ "   @ € € &  @  &  & €    IsDead           
    	   ^   ^   ^   ^   _   _   a   a   b          thisXp d   h           _ @ € €@@ E € $@ & €     GetAttacked          e   e   e   f   f   f   h          PlayerDamage j   l       	   & €               k   l      player          Player-                                                   /   !   :   1   >   <   D   @   H   F   L   J   P   N   W   R   [   Y   b   ]   h   d   l   j   n   n   n   o   p      name    -   health    -   xp    -   damage    -   this   -   Id   -   Name   -   Health   -   	Position	   -   Dead
   -   Xp   -   Damage   -   Player
   -   
MaxHealth   -      EnemyVector3_ENV t   1   ?   K   † @ À   A  A  Á  ÆÁ B AB B ä   C  ‚  ÁB    C  ¬  J€ƒ¬C  J€ƒƒ¬ƒ  J€„¬Ã  J€ƒ„¬ J€…¬C J€ƒ…¬ƒ J€†¬Ã ì JÀƒ†ìC JÀ‡ìƒ JÀƒ‡ìÃ JÀˆì JÀƒˆìC JÀ‰ìƒ JÀƒ‰ìÃ JÀŠì JÀƒŠìC JÀ‹ìƒ JÀƒ‹ìÃ JÀŒì JÀƒŒÆ@ ÍƒÀÀ€f  & €    NextId
                      New        
ReadValuePrintStatusGetNameSetPositionGetXGetYGetZGainXpBattleEnemyIsInBattleAttackGetAttackedIsDeadExitBattleReset	DonateXpCheat
IsFlaggedBan	IsBanned          „   •        K   …   J€ €… € J€€€…  J€ … €J€€† AJ€ ‚†@AJ€€‚†€AJ€ ƒ… €J€€ƒ…  J€ „… €J€€„…  J€ …… €J€€……  J€ †… €J€€†‡ € ¦  & €    idnamehealthlevelxyzxpdamageenemy
maxhealth	inbattleflaggedbanned   
	
        …   †   †   ‡   ‡   ˆ   ˆ   ‰   ‰   Š   Š   ‹   ‹   Œ   Œ         Ž   Ž               ‘   ‘   ’   ’   “   “   ”   ”   •      key        data          IdNameHealthLevel	PositionXpDamageEnemy
MaxHealth	InBattleFlaggedBanned —   ¬     G    @ A@  $@  @ A€  … € ]€€ $@  @ AÀ  …  ]€€ $@  @ A  … €]€€ $@  @ A@ …  ]€€ $@  @ A€ … €]€€ $@  @ AÀ † BÁ@ BAA †ÁB]€ $@   E €b     €@ F @ € À   À d@ F @ À Å  À d@ F @   Å €À d@   E  b     €@ F @ @ À   À d@ F @ € d@ & €    print-----PLAYER INFO-----Name: Id: 	Health: Level: XP: Position: x, yzfalsetrueIn Battle: Attack Damage: 
Max Health: 	Banned: ---------------------    	

    G   ˜   ˜   ˜   ™   ™   ™   ™   ™   š   š   š   š   š   ›   ›   ›   ›   ›   œ   œ   œ   œ   œ                  ž   ž   ž   ž   ž   ž   ž   ž   ž   Ÿ               ¡   £   £   £   £   £   ¤   ¤   ¤   ¤   ¤   ¥   ¥   ¥   ¥   ¥   ¦   §   §   §   ¨   ª   ª   ª   ª   ª   «   «   «   ¬      b&   G      _ENVNameIdHealthLevelXp	Position	InBattleDamage
MaxHealthBanned ®   °           &  & €               ¯   ¯   °          Name ²   ¶         €@€€€ & €    xyz          ³   ´   µ   ¶      x       y       z          	Position ¸   º         @ &  & €    x          ¹   ¹   º          	Position ¼   ¾         @ &  & €    y          ½   ½   ¾          	Position À   Â         @ &  & €    z          Á   Á   Â          	Position Ä   Ú     7      _ @ € € € "     €& €   A@  €  Á   Á  A A Á Â A B +@ E   M€Â G@  …  !€€ À€…   €B‰   …  Ž@ ‰  … €€@‰ €… €‰  … €€B‰ €…   €BG€  _ÀÂ   € ú…    @@€@ ‰ €… €‰    ‰ €& € 
   
                             2       d       –       È       ,      ô                        

    7   Å   Å   Å   Å   Å   Å   Æ   È   È   È   È   È   È   È   È   È   È   È   È   É   É   É   Ê   Ê   Ê   Ë   Ë   Ë   Ì   Ì   Ì   Í   Í   Í   Î   Î   Ï   Ï   Ï   Ð   Ð   Ð   Ñ   Ñ   Ó   Õ   Õ   Õ   Ö   Ö   ×   ×   Ø   Ø   Ú      xpTable   7   	neededXp   7      LevelBannedXp
MaxHealthHealthDamage Ü   ß       E   M € I   E € d@€ & €               Ý   Ý   Ý   Þ   Þ   ß      xp          XpAttemptLevelUp á   å       	   F @ … € d@ C € I  & €    
SetPlayer   	       â   ã   ã   ã   ä   ä   å      enemy          Enemythis	InBattle ç   é           &  & €        	       è   è   é          	InBattle ë   ô           "     €& €  À $€€ "   @€  _@@ € €€@E €$@ & €    IsInBattle GetAttacked   

       ì   ì   ì   í   ï   ï   ï   ï   ð   ð   ð   ñ   ñ   ñ   ô          BannedthisEnemyDamage ö   û       E   N € I   E   ! À   €H€À€& €            Dead           ÷   ÷   ÷   ø   ø   ø   ù   û      damage          Health_ENV ý   ÿ         @ &  & €    Dead           þ   þ   ÿ          _ENV             	      	 € & €        	                        	InBattleEnemy           @ $@€ @  	 € €  	  €  	 €À  	    	 € ÁA@ @ Á@ $€ 	  ÂAƒ& €    ExitBattle               
       New        Dead 	   
                 	  	  
  
          
  
  
  
  
  
          	   thisXpLevelDamage
MaxHealthHealth	PositionVector3_ENV         …   Å   Î@€!€€@€…   Ž@ ‰   ‡ @ À € ¤@ & €    GainXp                                     player       xp          Xp       
      	   @  	 €    	  @  	 € €	   € 	 €& €    c       '         
    
                                    LevelXpDamage
MaxHealthHealthFlagged !  #          &  & €               "  "  #         Flagged %  )        € 	    À $@€ & €    Reset   
       &  &  '  '  )         Bannedthis +  -          &  & €        
       ,  ,  -         Banned?   u   w   x   y   z   {   |   |   |   |   |   }   ~      €      ‚   •   „   ¬   —   °   ®   ¶   ²   º   ¸   ¾   ¼   Â   À   Ú   ß   Ü   å   á   é   ç   ô   ë   û   ö   ÿ   ý                   #  !  )  %  -  +  /  /  /  0  1     name    ?   this   ?   Id   ?   Name   ?   Health   ?   Level   ?   Xp   ?   	Position   ?   Enemy   ?   	InBattle
   ?   Damage   ?   
MaxHealth   ?   Flagged   ?   Banned   ?   AttemptLevelUp    ?      PlayerVector3_ENV 3  X   	h   ‡ @ ¤€€ ¢   @ €ƒ   ¦  ‡@@ ¤€€ Ç@À ä€€ À  €‡€@ ¤€€ Ç€À ä€€ À €€‡À@ ¤€€ ÇÀÀ ä€€ À  €‡ A À € ¤@ ‡@A ¤€€ Ç@Á ä€€ A AÁ $A A @  À€]Á$A A @ A ]$A B $A€ ÁÂ $€ "  €€A @€ ]$A AÃ $€ FA  À Â dA GD € dA GAD dA€ €€A @€A ]$A Â $A€ ÁB $€ "  €õA @  ]$A A A $A ÁD $A€   €€òA A $A A AA $A € & ƒ   ¦  & €    	IsBannedGetXGetYGetZBattleEnemyGetNameprintBattle started vs 	 attacksAttackIsDead diedGetXpGained  XPGainXpExitBattleReset to Level 1Reset
Battle ended          h   4  4  4  4  5  5  7  7  7  7  7  7  8  8  8  8  8  8  9  9  9  9  9  9  :  :  :  ;  ;  <  <  =  =  =  >  >  >  >  >  >  @  @  @  @  @  A  A  B  B  B  B  C  C  C  C  C  D  D  E  E  E  E  E  E  F  F  F  G  G  G  J  J  J  J  J  K  K  L  L  L  L  M  M  M  M  M  N  N  N  O  O  O  Q  S  S  S  T  T  T  U  U  W  W  X     player    h   enemy    h   pname   e   ename   e   xp:   F      _ENV [  k          l   
@ €l@  
@€€&  & €    
BanPlayer
IsCheater        ^  d      F @ €   d€ b   À €G@@ d@€ C € f  C   f  & €    
IsCheaterBan           _  _  _  _  _  `  `  a  a  c  c  d     player          this f  h      G @ e € f   & €    
IsFlagged           g  g  g  h     player              \  d  ^  h  f  j  k     this           n  ’          K   ‹   Ä   ,  
 €,A  
 €,  
 ,Á  
 , 
 ‚&  & €    GeneratePlayerGenerateEnemyGenerateAdminGetPlayersGetEnemies       u  y      F @ €   d€ †@@ Ž€@H@ f  & €    NewNextId                   v  v  v  w  w  w  x  y     name       player         PlayerPlayers {     	   @ @  €€ À  €$€FA@ NÀH & & €    NewNextId                  |  |  |  |  |  |  }  }  }  ~       name       health       xp       damage       enemy         EnemyEnemies   ‡          _ @ @ €   &  @À $€€ 	      &  & €     New           ‚  ‚  ‚  ƒ  ƒ  …  …  …  †  †  ‡         AdminAdministrator ‰  ‹          &  & €               Š  Š  ‹         Players             &  & €               Ž  Ž           Enemies   o  q  r  s  y  u    {  ‡    ‹  ‰      ‘  ’     this      Players      Enemies      Admin         PlayerEnemyAdministrator¥     
   
      
   p   p   r   s   1  1  X  3  Z  k  k  m  ’  ’  –  –  —  —  —  š  š  š  ›  ›  ›  ›  ›  ›  ž  ž  ž  Ÿ  Ÿ  Ÿ  Ÿ  Ÿ  Ÿ  ¢  ¢  £  £  £  ¦  ¦  ¦  ¦  ¦  ¦  §  §  §  §  §  §  ¨  ¨  ¨  ¨  ¨  ¨  ©  ©  ©  ©  ©  ©  ª  ª  ª  ª  ª  ª  «  «  «  «  «  «  ®  ®  ®  ®  ®  ¯  ¯  ¯  ¯  ¯  °  °  °  °  °  ±  ±  ±  ±  ±  ²  ²  ²  ²  ²  ³  ³  ³  ³  ³  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¹  ¹  ¹  ¹  ¹  ¼  ¼  ½  ½  ¾  ¾  ¾  Á  Á  Á  Á  Á  Á  Á  Á  Á  Á  Á  Ä  Ä  Å  Å  Æ  Æ  Æ  É  É  É  É  É  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ï  Ï  Ð  Ð  Ñ  Ñ  Ñ  Ô  Ô  Õ  Õ  Ø  Ø  Ø  Ù  Ù  Ù  Ù  Ù  Ù  Ú  Ú  Û  Û  Û  Ü  Ü  Ü  Ý  Ý  Ý  Ý  Ý  Ý  Þ  Þ  ß  ß  ß  â  â  â  â  â  å  å  å  å  å  å  å  å  å  å  å  è  è  é  é  ê  ê  ê  í  í  í  í  í  ð  ð  ð  ð  ð  ð  ð  ð  ð  ð  ð  ó  ó  ô  ô  õ  õ  õ  ø  ø  ø  ø  ø  ø  ø  ø  û  û  û  û  û  û  û  û  û  û  û  þ  þ  ÿ  ÿ                                       	  	  	  	  	  	  	  	                                                                                                              !  !  !  !  !  !  !  !  !  !  !  $  $  %  %  &  &  &  )  )  )  *  *  *  *  *  *  +  +  ,  ,  ,  -  -  -  .  .  .  .  .  .  /  /  0  0  0  1  1  1  2  2  2  2  2  2  3  3  4  4  4  5  5  5  6  6  6  6  6  6  7  7  8  8  8  9  9  9  :  :  :  :  :  :  ;  ;  <  <  <  =  =  =  >  >  >  >  >  >  ?  ?  @  @  @  C  C  C  C  C  F  F  F  F  F  F  F  F  F  F  F  I  I  J  J  K  K  K  N  N  N  N  N  N  N  N  O  O  O  O  P  P  Q  Q  R  R  R  U  U  U  U  U  U  V  V  W  W  X  X  X  [  [  [  [  [  [  \  \  \  \  \  \  \  \  \  \  _  _  `  `  `  c  c  f  f  f  f  f  g  g  g  g  g  j  j  j  j  j  j  j  j  j  j  j  k  k  k  k  k  k  k  k  k  k  k  n  n  o  o  p  p  q  q  r  r  s  s  t  t  u  u  v  v  v  y  y  y  y  y  y  y  y  y  y  z  z  z  z  z  z  z  z  z  z  {  {  {  {  {  {  {  {  {  {  |  |  |  |  |  |  |  |  |  |  }  }  }  }  }  }  }  }  }  }  ~  ~  ~  ~  ~  ~  ~  ~  ~  ~                      €  €  €  €  €  €  €  €  €  €                      ‚  ‚  ‚  ‚  ‚  ‚  ‚  ‚  ‚  ‚  ƒ  ƒ  ƒ  ƒ  ƒ  ƒ  ƒ  ƒ  ƒ  ƒ  „  „  „  „  „  „  „  „  „  „  …  …  …  …  …  …  …  …  …  …  ‡  ‡  ‡  ‡  ‡  ‡  ‡  ‡  ‡  ‡  ˆ  ˆ  ˆ  ˆ  ˆ  ˆ  ˆ  ˆ  ˆ  ˆ  ‰  ‰  ‰  ‰  ‰  ‰  ‰  ‰  ‰  ‰  Š  Š  Š  Š  Š  Š  Š  Š  Š  Š  ‹  ‹  ‹  ‹  ‹  ‹  ‹  ‹  ‹  ‹  Œ  Œ  Œ  Œ  Œ  Œ  Œ  Œ  Œ  Œ                      Ž  Ž  Ž  Ž  Ž  Ž  Ž  Ž  Ž  Ž                                          ‘  ‘  ‘  ‘  ‘  ‘  ‘  ‘  ‘  ‘  ’  ’  ’  ’  ’  ’  ’  ’  ’  ’  “  “  “  “  “  “  “  “  “  “  •  •  •  •  •  •  •  •  •  •  –  –  –  –  –  –  –  –  –  –  —  —  —  —  —  —  —  —  —  —  ˜  ˜  ˜  ˜  ˜  ˜  ˜  ˜  ˜  ˜  ™  ™  ™  ™  ™  ™  ™  ™  ™  ™  š  š  š  š  š  š  š  š  š  š  ›  ›  ›  ›  ›  ›  ›  ›  ›  ›  œ  œ  œ  œ  œ  œ  œ  œ  œ  œ                      Ÿ  Ÿ  Ÿ  Ÿ  Ÿ  Ÿ  Ÿ  Ÿ  Ÿ  Ÿ                                ¡  ¡  ¡  ¡  ¡  ¡  ¡  ¡  ¡  ¡  ¢  ¢  ¢  ¢  ¢  ¢  ¢  ¢  ¢  ¢  £  £  £  £  £  £  £  £  £  £  ¤  ¤  ¤  ¤  ¤  ¤  ¤  ¤  ¤  ¤  ¥  ¥  ¥  ¥  ¥  ¥  ¥  ¥  ¥  ¥  ¦  ¦  ¦  ¦  ¦  ¦  ¦  ¦  ¦  ¦  §  §  §  §  §  §  §  §  §  §  ©  ©  ©  ©  ©  ©  ©  ©  ©  ©  ª  ª  ª  ª  ª  ª  ª  ª  ª  ª  «  «  «  «  «  «  «  «  «  «  ¬  ¬  ¬  ¬  ¬  ¬  ¬  ¬  ¬  ¬  ­  ­  ­  ­  ­  ­  ­  ­  ­  ­  ®  ®  ®  ®  ®  ®  ®  ®  ®  ®  ¯  ¯  ¯  ¯  ¯  ¯  ¯  ¯  ¯  ¯  °  °  °  °  °  °  °  °  °  °  ±  ±  ±  ±  ±  ±  ±  ±  ±  ±  ³  ³  ³  ³  ³  ³  ³  ³  ³  ³  ´  ´  ´  ´  ´  ´  ´  ´  ´  ´  µ  µ  µ  µ  µ  µ  µ  µ  µ  µ  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ¶  ·  ·  ·  ·  ·  ·  ·  ·  ·  ·  ¸  ¸  ¸  ¸  ¸  ¸  ¸  ¸  ¸  ¸  ¹  ¹  ¹  ¹  ¹  ¹  ¹  ¹  ¹  ¹  º  º  º  º  º  º  º  º  º  º  »  »  »  »  »  »  »  »  »  »  ½  ½  ½  ½  ½  ½  ½  ½  ½  ½  ¾  ¾  ¾  ¾  ¾  ¾  ¾  ¾  ¾  ¾  ¿  ¿  ¿  ¿  ¿  ¿  ¿  ¿  ¿  ¿  À  À  À  À  À  À  À  À  À  À  Á  Á  Á  Á  Á  Á  Á  Á  Á  Á  Â  Â  Â  Â  Â  Â  Â  Â  Â  Â  Ã  Ã  Ã  Ã  Ã  Ã  Ã  Ã  Ã  Ã  Ä  Ä  Ä  Ä  Ä  Ä  Ä  Ä  Ä  Ä  Å  Å  Å  Å  Å  Å  Å  Å  Å  Å  Ç  Ç  Ç  Ç  Ç  Ç  Ç  Ç  Ç  Ç  È  È  È  È  È  È  È  È  È  È  É  É  É  É  É  É  É  É  É  É  Ê  Ê  Ê  Ê  Ê  Ê  Ê  Ê  Ê  Ê  Ë  Ë  Ë  Ë  Ë  Ë  Ë  Ë  Ë  Ë  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Ì  Í  Í  Í  Í  Í  Í  Í  Í  Í  Í  Î  Î  Î  Î  Î  Î  Î  Î  Î  Î  Ï  Ï  Ï  Ï  Ï  Ï  Ï  Ï  Ï  Ï  Ñ  Ñ  Ñ  Ñ  Ñ  Ñ  Ñ  Ò  Ò  Ò  Ò  Ò  Ò  Ò  Ó  Ó  Ó  Ó  Ó  Ó  Ó  Ô  Ô  Ô  Ô  Ô  Ô  Ô  Õ  Õ  Õ  Õ  Õ  Õ  Õ  Ö  Ö  Ö  Ö  Ö  Ö  Ö  ×  ×  ×  ×  ×  ×  ×  Ø  Ø  Ø  Ø  Ø  Ø  Ø  Ø     Vector3   ¥  Enemy   ¥  Player   ¥  Administrator   ¥  Factory   ¥  
generator   ¥  player   ¥  friend$   ¥  admin,   ¥  enemy15   ¥  enemy2;   ¥  enemy3A   ¥  enemy4G   ¥  enemy5M   ¥  bossS   ¥     _ENV
```
Output of the LuaAssemblyPrinter after reading the binary:
```
0 params; n varargs; 19 slots; 1445 opcodes; 200 constants; 1 upvalue; 6 functions; 15 locals
function func0(...)
	0001	NEWTABLE 0 0 0
	0002	CLOSURE 1 0                        ; func1
	0003	SETTABLE 0 -1 1                    ; New
	0004	NEWTABLE 1 0 0
	0005	SETTABLE 1 -2 -3                   ; NextId 1
	0006	CLOSURE 2 1                        ; func2
	0007	SETTABLE 1 -1 2                    ; New
	0008	NEWTABLE 2 0 0
	0009	SETTABLE 2 -4 -5                   ; NextId 1
	0010	CLOSURE 3 2                        ; func16
	0011	SETTABLE 2 -6 3                    ; New
	0012	CLOSURE 3 3                        ; func38
	0013	SETTABUP 0 -7 3                    ; _ENV StartBattle
	0014	NEWTABLE 3 0 0
	0015	CLOSURE 4 4                        ; func39
	0016	SETTABLE 3 -8 4                    ; New
	0017	NEWTABLE 4 0 0
	0018	CLOSURE 5 5                        ; func42
	0019	SETTABLE 4 -8 5                    ; New
	0020	GETTABLE 5 4 -9                    ; New
	0021	CALL 5 1 2
	0022	GETTABUP 6 0 -10                   ; _ENV print
	0023	LOADK 7 -11                        ; Created factory
	0024	CALL 6 2 1
	0025	GETTABLE 6 5 -12                   ; GeneratePlayer
	0026	LOADK 7 -13                        ; Jeff
	0027	CALL 6 2 2
	0028	GETTABUP 7 0 -10                   ; _ENV print
	0029	LOADK 8 -14                        ; Created player 
	0030	GETTABLE 9 6 -15                   ; GetName
	0031	CALL 9 1 2
	0032	CONCAT 8 8 9
	0033	CALL 7 2 1
	0034	GETTABLE 7 5 -12                   ; GeneratePlayer
	0035	LOADK 8 -16                        ; Jay
	0036	CALL 7 2 2
	0037	GETTABUP 8 0 -10                   ; _ENV print
	0038	LOADK 9 -14                        ; Created player 
	0039	GETTABLE 10 7 -15                  ; GetName
	0040	CALL 10 1 2
	0041	CONCAT 9 9 10
	0042	CALL 8 2 1
	0043	GETTABLE 8 5 -17                   ; GenerateAdmin
	0044	CALL 8 1 2
	0045	GETTABUP 9 0 -10                   ; _ENV print
	0046	LOADK 10 -18                       ; Created admin
	0047	CALL 9 2 1
	0048	GETTABLE 9 5 -19                   ; GenerateEnemy
	0049	LOADK 10 -20                       ; Joe
	0050	LOADK 11 -21                       ; 10
	0051	LOADK 12 -22                       ; 5
	0052	LOADK 13 -3                        ; 1
	0053	CALL 9 5 2
	0054	GETTABLE 10 5 -19                  ; GenerateEnemy
	0055	LOADK 11 -23                       ; Jeffrey
	0056	LOADK 12 -24                       ; 30
	0057	LOADK 13 -25                       ; 20
	0058	LOADK 14 -26                       ; 4
	0059	CALL 10 5 2
	0060	GETTABLE 11 5 -19                  ; GenerateEnemy
	0061	LOADK 12 -27                       ; John
	0062	LOADK 13 -28                       ; 40
	0063	LOADK 14 -29                       ; 60
	0064	LOADK 15 -22                       ; 5
	0065	CALL 11 5 2
	0066	GETTABLE 12 5 -19                  ; GenerateEnemy
	0067	LOADK 13 -30                       ; Jane
	0068	LOADK 14 -29                       ; 60
	0069	LOADK 15 -31                       ; 100
	0070	LOADK 16 -32                       ; 3
	0071	CALL 12 5 2
	0072	GETTABLE 13 5 -19                  ; GenerateEnemy
	0073	LOADK 14 -33                       ; Jill
	0074	LOADK 15 -31                       ; 100
	0075	LOADK 16 -34                       ; 300
	0076	LOADK 17 -22                       ; 5
	0077	CALL 13 5 2
	0078	GETTABLE 14 5 -19                  ; GenerateEnemy
	0079	LOADK 15 -35                       ; Johnny
	0080	LOADK 16 -36                       ; 200
	0081	LOADK 17 -37                       ; 1000
	0082	LOADK 18 -21                       ; 10
	0083	CALL 14 5 2
	0084	GETTABLE 15 9 -38                  ; SetPosition
	0085	LOADK 16 -39                       ; 5
	0086	LOADK 17 -39                       ; 5
	0087	LOADK 18 -39                       ; 5
	0088	CALL 15 4 1
	0089	GETTABLE 15 10 -38                 ; SetPosition
	0090	LOADK 16 -40                       ; 10
	0091	LOADK 17 -40                       ; 10
	0092	LOADK 18 -40                       ; 10
	0093	CALL 15 4 1
	0094	GETTABLE 15 11 -38                 ; SetPosition
	0095	LOADK 16 -41                       ; 15
	0096	LOADK 17 -41                       ; 15
	0097	LOADK 18 -41                       ; 15
	0098	CALL 15 4 1
	0099	GETTABLE 15 12 -38                 ; SetPosition
	0100	LOADK 16 -42                       ; 100
	0101	LOADK 17 -40                       ; 10
	0102	LOADK 18 -43                       ; 30
	0103	CALL 15 4 1
	0104	GETTABLE 15 13 -38                 ; SetPosition
	0105	LOADK 16 -44                       ; 200
	0106	LOADK 17 -45                       ; 0
	0107	LOADK 18 -45                       ; 0
	0108	CALL 15 4 1
	0109	GETTABLE 15 14 -38                 ; SetPosition
	0110	LOADK 16 -46                       ; 300
	0111	LOADK 17 -44                       ; 200
	0112	LOADK 18 -42                       ; 100
	0113	CALL 15 4 1
	0114	GETTABUP 15 0 -47                  ; _ENV assert
	0115	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0116	MOVE 17 6
	0117	MOVE 18 11
	0118	CALL 16 3 2
	0119	EQ 1 16 -48                        ; - False
	0120	JMP 0 1                            ; to pc 122
	0121	LOADBOOL 16 0 1
	0122	LOADBOOL 16 1 0
	0123	LOADK 17 -49                       ; Test 1 failed
	0124	CALL 15 3 1
	0125	GETTABLE 15 6 -38                  ; SetPosition
	0126	LOADK 16 -39                       ; 5
	0127	LOADK 17 -39                       ; 5
	0128	LOADK 18 -39                       ; 5
	0129	CALL 15 4 1
	0130	GETTABLE 15 6 -50                  ; PrintStatus
	0131	CALL 15 1 1
	0132	GETTABLE 15 9 -50                  ; PrintStatus
	0133	CALL 15 1 1
	0134	GETTABUP 15 0 -10                  ; _ENV print
	0135	LOADK 16 -51                       ;  
	0136	CALL 15 2 1
	0137	GETTABUP 15 0 -47                  ; _ENV assert
	0138	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0139	MOVE 17 6
	0140	MOVE 18 9
	0141	CALL 16 3 2
	0142	EQ 1 16 -52                        ; - True
	0143	JMP 0 1                            ; to pc 145
	0144	LOADBOOL 16 0 1
	0145	LOADBOOL 16 1 0
	0146	LOADK 17 -53                       ; Test 2 failed
	0147	CALL 15 3 1
	0148	GETTABLE 15 6 -50                  ; PrintStatus
	0149	CALL 15 1 1
	0150	GETTABLE 15 9 -50                  ; PrintStatus
	0151	CALL 15 1 1
	0152	GETTABUP 15 0 -10                  ; _ENV print
	0153	LOADK 16 -51                       ;  
	0154	CALL 15 2 1
	0155	GETTABLE 15 6 -38                  ; SetPosition
	0156	LOADK 16 -40                       ; 10
	0157	LOADK 17 -40                       ; 10
	0158	LOADK 18 -40                       ; 10
	0159	CALL 15 4 1
	0160	GETTABUP 15 0 -47                  ; _ENV assert
	0161	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0162	MOVE 17 6
	0163	MOVE 18 10
	0164	CALL 16 3 2
	0165	EQ 1 16 -52                        ; - True
	0166	JMP 0 1                            ; to pc 168
	0167	LOADBOOL 16 0 1
	0168	LOADBOOL 16 1 0
	0169	LOADK 17 -54                       ; Test 3 failed
	0170	CALL 15 3 1
	0171	GETTABLE 15 6 -50                  ; PrintStatus
	0172	CALL 15 1 1
	0173	GETTABLE 15 10 -50                 ; PrintStatus
	0174	CALL 15 1 1
	0175	GETTABUP 15 0 -10                  ; _ENV print
	0176	LOADK 16 -51                       ;  
	0177	CALL 15 2 1
	0178	GETTABLE 15 9 -55                  ; Reset
	0179	CALL 15 1 1
	0180	GETTABLE 15 10 -55                 ; Reset
	0181	CALL 15 1 1
	0182	GETTABLE 15 6 -56                  ; GainXp
	0183	LOADK 16 -21                       ; 10
	0184	CALL 15 2 1
	0185	GETTABUP 15 0 -10                  ; _ENV print
	0186	GETTABLE 16 6 -15                  ; GetName
	0187	CALL 16 1 2
	0188	LOADK 17 -57                       ;  gained 10 XP
	0189	CONCAT 16 16 17
	0190	CALL 15 2 1
	0191	GETTABLE 15 6 -50                  ; PrintStatus
	0192	CALL 15 1 1
	0193	GETTABUP 15 0 -10                  ; _ENV print
	0194	LOADK 16 -51                       ;  
	0195	CALL 15 2 1
	0196	GETTABLE 15 6 -56                  ; GainXp
	0197	LOADK 16 -25                       ; 20
	0198	CALL 15 2 1
	0199	GETTABUP 15 0 -10                  ; _ENV print
	0200	GETTABLE 16 6 -15                  ; GetName
	0201	CALL 16 1 2
	0202	LOADK 17 -58                       ;  gained 20 XP
	0203	CONCAT 16 16 17
	0204	CALL 15 2 1
	0205	GETTABLE 15 6 -50                  ; PrintStatus
	0206	CALL 15 1 1
	0207	GETTABUP 15 0 -10                  ; _ENV print
	0208	LOADK 16 -51                       ;  
	0209	CALL 15 2 1
	0210	GETTABLE 15 6 -38                  ; SetPosition
	0211	LOADK 16 -39                       ; 5
	0212	LOADK 17 -39                       ; 5
	0213	LOADK 18 -39                       ; 5
	0214	CALL 15 4 1
	0215	GETTABUP 15 0 -47                  ; _ENV assert
	0216	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0217	MOVE 17 6
	0218	MOVE 18 9
	0219	CALL 16 3 2
	0220	EQ 1 16 -52                        ; - True
	0221	JMP 0 1                            ; to pc 223
	0222	LOADBOOL 16 0 1
	0223	LOADBOOL 16 1 0
	0224	LOADK 17 -59                       ; Test 4 failed
	0225	CALL 15 3 1
	0226	GETTABLE 15 6 -50                  ; PrintStatus
	0227	CALL 15 1 1
	0228	GETTABLE 15 9 -50                  ; PrintStatus
	0229	CALL 15 1 1
	0230	GETTABUP 15 0 -10                  ; _ENV print
	0231	LOADK 16 -51                       ;  
	0232	CALL 15 2 1
	0233	GETTABLE 15 6 -38                  ; SetPosition
	0234	LOADK 16 -40                       ; 10
	0235	LOADK 17 -40                       ; 10
	0236	LOADK 18 -40                       ; 10
	0237	CALL 15 4 1
	0238	GETTABUP 15 0 -47                  ; _ENV assert
	0239	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0240	MOVE 17 6
	0241	MOVE 18 10
	0242	CALL 16 3 2
	0243	EQ 1 16 -52                        ; - True
	0244	JMP 0 1                            ; to pc 246
	0245	LOADBOOL 16 0 1
	0246	LOADBOOL 16 1 0
	0247	LOADK 17 -60                       ; Test 5 failed
	0248	CALL 15 3 1
	0249	GETTABLE 15 6 -50                  ; PrintStatus
	0250	CALL 15 1 1
	0251	GETTABLE 15 10 -50                 ; PrintStatus
	0252	CALL 15 1 1
	0253	GETTABUP 15 0 -10                  ; _ENV print
	0254	LOADK 16 -51                       ;  
	0255	CALL 15 2 1
	0256	GETTABLE 15 6 -38                  ; SetPosition
	0257	GETTABLE 16 11 -61                 ; GetX
	0258	CALL 16 1 2
	0259	GETTABLE 17 11 -62                 ; GetY
	0260	CALL 17 1 2
	0261	GETTABLE 18 11 -63                 ; GetZ
	0262	CALL 18 1 0
	0263	CALL 15 0 1
	0264	GETTABUP 15 0 -47                  ; _ENV assert
	0265	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0266	MOVE 17 6
	0267	MOVE 18 11
	0268	CALL 16 3 2
	0269	EQ 1 16 -52                        ; - True
	0270	JMP 0 1                            ; to pc 272
	0271	LOADBOOL 16 0 1
	0272	LOADBOOL 16 1 0
	0273	LOADK 17 -64                       ; Test 6 failed
	0274	CALL 15 3 1
	0275	GETTABLE 15 6 -50                  ; PrintStatus
	0276	CALL 15 1 1
	0277	GETTABLE 15 11 -50                 ; PrintStatus
	0278	CALL 15 1 1
	0279	GETTABUP 15 0 -10                  ; _ENV print
	0280	LOADK 16 -51                       ;  
	0281	CALL 15 2 1
	0282	GETTABLE 15 6 -56                  ; GainXp
	0283	LOADK 16 -65                       ; 50
	0284	CALL 15 2 1
	0285	GETTABUP 15 0 -10                  ; _ENV print
	0286	GETTABLE 16 6 -15                  ; GetName
	0287	CALL 16 1 2
	0288	LOADK 17 -66                       ;  gained 50 XP
	0289	CONCAT 16 16 17
	0290	CALL 15 2 1
	0291	GETTABLE 15 6 -50                  ; PrintStatus
	0292	CALL 15 1 1
	0293	GETTABUP 15 0 -10                  ; _ENV print
	0294	LOADK 16 -51                       ;  
	0295	CALL 15 2 1
	0296	GETTABLE 15 6 -38                  ; SetPosition
	0297	GETTABLE 16 12 -61                 ; GetX
	0298	CALL 16 1 2
	0299	GETTABLE 17 12 -62                 ; GetY
	0300	CALL 17 1 2
	0301	GETTABLE 18 12 -63                 ; GetZ
	0302	CALL 18 1 0
	0303	CALL 15 0 1
	0304	GETTABUP 15 0 -47                  ; _ENV assert
	0305	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0306	MOVE 17 6
	0307	MOVE 18 12
	0308	CALL 16 3 2
	0309	EQ 1 16 -52                        ; - True
	0310	JMP 0 1                            ; to pc 312
	0311	LOADBOOL 16 0 1
	0312	LOADBOOL 16 1 0
	0313	LOADK 17 -67                       ; Test 7 failed
	0314	CALL 15 3 1
	0315	GETTABLE 15 6 -50                  ; PrintStatus
	0316	CALL 15 1 1
	0317	GETTABLE 15 12 -50                 ; PrintStatus
	0318	CALL 15 1 1
	0319	GETTABUP 15 0 -10                  ; _ENV print
	0320	LOADK 16 -51                       ;  
	0321	CALL 15 2 1
	0322	GETTABLE 15 6 -56                  ; GainXp
	0323	LOADK 16 -68                       ; 150
	0324	CALL 15 2 1
	0325	GETTABUP 15 0 -10                  ; _ENV print
	0326	GETTABLE 16 6 -15                  ; GetName
	0327	CALL 16 1 2
	0328	LOADK 17 -69                       ;  gained 150 XP
	0329	CONCAT 16 16 17
	0330	CALL 15 2 1
	0331	GETTABLE 15 6 -50                  ; PrintStatus
	0332	CALL 15 1 1
	0333	GETTABUP 15 0 -10                  ; _ENV print
	0334	LOADK 16 -51                       ;  
	0335	CALL 15 2 1
	0336	GETTABLE 15 6 -56                  ; GainXp
	0337	LOADK 16 -68                       ; 150
	0338	CALL 15 2 1
	0339	GETTABUP 15 0 -10                  ; _ENV print
	0340	GETTABLE 16 6 -15                  ; GetName
	0341	CALL 16 1 2
	0342	LOADK 17 -69                       ;  gained 150 XP
	0343	CONCAT 16 16 17
	0344	CALL 15 2 1
	0345	GETTABLE 15 6 -50                  ; PrintStatus
	0346	CALL 15 1 1
	0347	GETTABUP 15 0 -10                  ; _ENV print
	0348	LOADK 16 -51                       ;  
	0349	CALL 15 2 1
	0350	GETTABLE 15 6 -38                  ; SetPosition
	0351	GETTABLE 16 13 -61                 ; GetX
	0352	CALL 16 1 2
	0353	GETTABLE 17 13 -62                 ; GetY
	0354	CALL 17 1 2
	0355	GETTABLE 18 13 -63                 ; GetZ
	0356	CALL 18 1 0
	0357	CALL 15 0 1
	0358	GETTABUP 15 0 -47                  ; _ENV assert
	0359	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0360	MOVE 17 6
	0361	MOVE 18 13
	0362	CALL 16 3 2
	0363	EQ 1 16 -52                        ; - True
	0364	JMP 0 1                            ; to pc 366
	0365	LOADBOOL 16 0 1
	0366	LOADBOOL 16 1 0
	0367	LOADK 17 -70                       ; Test 8 failed
	0368	CALL 15 3 1
	0369	GETTABLE 15 6 -50                  ; PrintStatus
	0370	CALL 15 1 1
	0371	GETTABLE 15 13 -50                 ; PrintStatus
	0372	CALL 15 1 1
	0373	GETTABUP 15 0 -10                  ; _ENV print
	0374	LOADK 16 -51                       ;  
	0375	CALL 15 2 1
	0376	GETTABLE 15 6 -56                  ; GainXp
	0377	LOADK 16 -34                       ; 300
	0378	CALL 15 2 1
	0379	GETTABUP 15 0 -10                  ; _ENV print
	0380	GETTABLE 16 6 -15                  ; GetName
	0381	CALL 16 1 2
	0382	LOADK 17 -71                       ;  gained 300 XP
	0383	CONCAT 16 16 17
	0384	CALL 15 2 1
	0385	GETTABLE 15 6 -50                  ; PrintStatus
	0386	CALL 15 1 1
	0387	GETTABUP 15 0 -10                  ; _ENV print
	0388	LOADK 16 -51                       ;  
	0389	CALL 15 2 1
	0390	GETTABLE 15 6 -56                  ; GainXp
	0391	LOADK 16 -34                       ; 300
	0392	CALL 15 2 1
	0393	GETTABUP 15 0 -10                  ; _ENV print
	0394	GETTABLE 16 6 -15                  ; GetName
	0395	CALL 16 1 2
	0396	LOADK 17 -71                       ;  gained 300 XP
	0397	CONCAT 16 16 17
	0398	CALL 15 2 1
	0399	GETTABLE 15 6 -50                  ; PrintStatus
	0400	CALL 15 1 1
	0401	GETTABUP 15 0 -10                  ; _ENV print
	0402	LOADK 16 -51                       ;  
	0403	CALL 15 2 1
	0404	GETTABLE 15 6 -56                  ; GainXp
	0405	LOADK 16 -34                       ; 300
	0406	CALL 15 2 1
	0407	GETTABUP 15 0 -10                  ; _ENV print
	0408	GETTABLE 16 6 -15                  ; GetName
	0409	CALL 16 1 2
	0410	LOADK 17 -71                       ;  gained 300 XP
	0411	CONCAT 16 16 17
	0412	CALL 15 2 1
	0413	GETTABLE 15 6 -50                  ; PrintStatus
	0414	CALL 15 1 1
	0415	GETTABUP 15 0 -10                  ; _ENV print
	0416	LOADK 16 -51                       ;  
	0417	CALL 15 2 1
	0418	GETTABLE 15 6 -56                  ; GainXp
	0419	LOADK 16 -72                       ; 400
	0420	CALL 15 2 1
	0421	GETTABUP 15 0 -10                  ; _ENV print
	0422	GETTABLE 16 6 -15                  ; GetName
	0423	CALL 16 1 2
	0424	LOADK 17 -73                       ;  gained 400 XP
	0425	CONCAT 16 16 17
	0426	CALL 15 2 1
	0427	GETTABLE 15 6 -50                  ; PrintStatus
	0428	CALL 15 1 1
	0429	GETTABUP 15 0 -10                  ; _ENV print
	0430	LOADK 16 -51                       ;  
	0431	CALL 15 2 1
	0432	GETTABLE 15 6 -56                  ; GainXp
	0433	LOADK 16 -74                       ; 500
	0434	CALL 15 2 1
	0435	GETTABUP 15 0 -10                  ; _ENV print
	0436	GETTABLE 16 6 -15                  ; GetName
	0437	CALL 16 1 2
	0438	LOADK 17 -75                       ;  gained 500 XP
	0439	CONCAT 16 16 17
	0440	CALL 15 2 1
	0441	GETTABLE 15 6 -50                  ; PrintStatus
	0442	CALL 15 1 1
	0443	GETTABUP 15 0 -10                  ; _ENV print
	0444	LOADK 16 -51                       ;  
	0445	CALL 15 2 1
	0446	GETTABLE 15 6 -56                  ; GainXp
	0447	LOADK 16 -76                       ; 600
	0448	CALL 15 2 1
	0449	GETTABUP 15 0 -10                  ; _ENV print
	0450	GETTABLE 16 6 -15                  ; GetName
	0451	CALL 16 1 2
	0452	LOADK 17 -77                       ;  gained 600 XP
	0453	CONCAT 16 16 17
	0454	CALL 15 2 1
	0455	GETTABLE 15 6 -50                  ; PrintStatus
	0456	CALL 15 1 1
	0457	GETTABUP 15 0 -10                  ; _ENV print
	0458	LOADK 16 -51                       ;  
	0459	CALL 15 2 1
	0460	GETTABLE 15 6 -38                  ; SetPosition
	0461	LOADK 16 -46                       ; 300
	0462	LOADK 17 -44                       ; 200
	0463	LOADK 18 -42                       ; 100
	0464	CALL 15 4 1
	0465	GETTABUP 15 0 -47                  ; _ENV assert
	0466	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0467	MOVE 17 6
	0468	MOVE 18 14
	0469	CALL 16 3 2
	0470	EQ 1 16 -52                        ; - True
	0471	JMP 0 1                            ; to pc 473
	0472	LOADBOOL 16 0 1
	0473	LOADBOOL 16 1 0
	0474	LOADK 17 -78                       ; Test 9 failed
	0475	CALL 15 3 1
	0476	GETTABLE 15 6 -50                  ; PrintStatus
	0477	CALL 15 1 1
	0478	GETTABLE 15 14 -50                 ; PrintStatus
	0479	CALL 15 1 1
	0480	GETTABUP 15 0 -10                  ; _ENV print
	0481	LOADK 16 -51                       ;  
	0482	CALL 15 2 1
	0483	GETTABUP 15 0 -10                  ; _ENV print
	0484	GETTABLE 16 6 -15                  ; GetName
	0485	CALL 16 1 2
	0486	LOADK 17 -79                       ;  gives 1015 xp to 
	0487	GETTABLE 18 7 -15                  ; GetName
	0488	CALL 18 1 2
	0489	CONCAT 16 16 18
	0490	CALL 15 2 1
	0491	GETTABLE 15 6 -80                  ; DonateXp
	0492	MOVE 16 7
	0493	LOADK 17 -81                       ; 1015
	0494	CALL 15 3 1
	0495	GETTABLE 15 6 -50                  ; PrintStatus
	0496	CALL 15 1 1
	0497	GETTABLE 15 7 -50                  ; PrintStatus
	0498	CALL 15 1 1
	0499	GETTABUP 15 0 -10                  ; _ENV print
	0500	LOADK 16 -51                       ;  
	0501	CALL 15 2 1
	0502	GETTABUP 15 0 -10                  ; _ENV print
	0503	GETTABLE 16 7 -15                  ; GetName
	0504	CALL 16 1 2
	0505	LOADK 17 -82                       ;  cheats to level 99
	0506	CONCAT 16 16 17
	0507	CALL 15 2 1
	0508	GETTABLE 15 7 -83                  ; Cheat
	0509	CALL 15 1 1
	0510	GETTABLE 15 7 -50                  ; PrintStatus
	0511	CALL 15 1 1
	0512	GETTABUP 15 0 -10                  ; _ENV print
	0513	LOADK 16 -51                       ;  
	0514	CALL 15 2 1
	0515	GETTABUP 15 0 -10                  ; _ENV print
	0516	LOADK 16 -84                       ; Banning 
	0517	GETTABLE 17 7 -15                  ; GetName
	0518	CALL 17 1 2
	0519	CONCAT 16 16 17
	0520	CALL 15 2 1
	0521	GETTABUP 15 0 -47                  ; _ENV assert
	0522	GETTABLE 16 8 -85                  ; BanPlayer
	0523	MOVE 17 7
	0524	CALL 16 2 2
	0525	EQ 1 16 -52                        ; - True
	0526	JMP 0 1                            ; to pc 528
	0527	LOADBOOL 16 0 1
	0528	LOADBOOL 16 1 0
	0529	LOADK 17 -86                       ; Test 10 failed
	0530	CALL 15 3 1
	0531	GETTABLE 15 7 -50                  ; PrintStatus
	0532	CALL 15 1 1
	0533	GETTABUP 15 0 -10                  ; _ENV print
	0534	LOADK 16 -51                       ;  
	0535	CALL 15 2 1
	0536	GETTABLE 15 9 -55                  ; Reset
	0537	CALL 15 1 1
	0538	GETTABLE 15 7 -38                  ; SetPosition
	0539	LOADK 16 -39                       ; 5
	0540	LOADK 17 -39                       ; 5
	0541	LOADK 18 -39                       ; 5
	0542	CALL 15 4 1
	0543	GETTABLE 15 6 -38                  ; SetPosition
	0544	LOADK 16 -39                       ; 5
	0545	LOADK 17 -39                       ; 5
	0546	LOADK 18 -39                       ; 5
	0547	CALL 15 4 1
	0548	GETTABUP 15 0 -47                  ; _ENV assert
	0549	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0550	MOVE 17 7
	0551	MOVE 18 9
	0552	CALL 16 3 2
	0553	EQ 1 16 -48                        ; - False
	0554	JMP 0 1                            ; to pc 556
	0555	LOADBOOL 16 0 1
	0556	LOADBOOL 16 1 0
	0557	LOADK 17 -87                       ; Test 11 failed
	0558	CALL 15 3 1
	0559	GETTABUP 15 0 -47                  ; _ENV assert
	0560	GETTABUP 16 0 -7                   ; _ENV StartBattle
	0561	MOVE 17 6
	0562	MOVE 18 9
	0563	CALL 16 3 2
	0564	EQ 1 16 -52                        ; - True
	0565	JMP 0 1                            ; to pc 567
	0566	LOADBOOL 16 0 1
	0567	LOADBOOL 16 1 0
	0568	LOADK 17 -88                       ; Test 12 failed
	0569	CALL 15 3 1
	0570	GETTABLE 15 6 -50                  ; PrintStatus
	0571	CALL 15 1 1
	0572	GETTABLE 15 7 -50                  ; PrintStatus
	0573	CALL 15 1 1
	0574	GETTABLE 15 9 -50                  ; PrintStatus
	0575	CALL 15 1 1
	0576	GETTABLE 15 10 -50                 ; PrintStatus
	0577	CALL 15 1 1
	0578	GETTABLE 15 11 -50                 ; PrintStatus
	0579	CALL 15 1 1
	0580	GETTABLE 15 12 -50                 ; PrintStatus
	0581	CALL 15 1 1
	0582	GETTABLE 15 13 -50                 ; PrintStatus
	0583	CALL 15 1 1
	0584	GETTABLE 15 14 -50                 ; PrintStatus
	0585	CALL 15 1 1
	0586	GETTABUP 15 0 -10                  ; _ENV print
	0587	LOADK 16 -51                       ;  
	0588	CALL 15 2 1
	0589	GETTABUP 15 0 -47                  ; _ENV assert
	0590	GETTABLE 16 6 -89                  ; ReadValue
	0591	LOADK 17 -90                       ; id
	0592	CALL 16 2 2
	0593	EQ 1 16 -3                         ; - 1
	0594	JMP 0 1                            ; to pc 596
	0595	LOADBOOL 16 0 1
	0596	LOADBOOL 16 1 0
	0597	LOADK 17 -91                       ; Test 13 failed
	0598	CALL 15 3 1
	0599	GETTABUP 15 0 -47                  ; _ENV assert
	0600	GETTABLE 16 6 -89                  ; ReadValue
	0601	LOADK 17 -92                       ; name
	0602	CALL 16 2 2
	0603	EQ 1 16 -13                        ; - Jeff
	0604	JMP 0 1                            ; to pc 606
	0605	LOADBOOL 16 0 1
	0606	LOADBOOL 16 1 0
	0607	LOADK 17 -93                       ; Test 14 failed
	0608	CALL 15 3 1
	0609	GETTABUP 15 0 -47                  ; _ENV assert
	0610	GETTABLE 16 6 -89                  ; ReadValue
	0611	LOADK 17 -94                       ; health
	0612	CALL 16 2 2
	0613	EQ 1 16 -28                        ; - 40
	0614	JMP 0 1                            ; to pc 616
	0615	LOADBOOL 16 0 1
	0616	LOADBOOL 16 1 0
	0617	LOADK 17 -95                       ; Test 15 failed
	0618	CALL 15 3 1
	0619	GETTABUP 15 0 -47                  ; _ENV assert
	0620	GETTABLE 16 6 -89                  ; ReadValue
	0621	LOADK 17 -96                       ; level
	0622	CALL 16 2 2
	0623	EQ 1 16 -21                        ; - 10
	0624	JMP 0 1                            ; to pc 626
	0625	LOADBOOL 16 0 1
	0626	LOADBOOL 16 1 0
	0627	LOADK 17 -97                       ; Test 16 failed
	0628	CALL 15 3 1
	0629	GETTABUP 15 0 -47                  ; _ENV assert
	0630	GETTABLE 16 6 -89                  ; ReadValue
	0631	LOADK 17 -98                       ; x
	0632	CALL 16 2 2
	0633	EQ 1 16 -39                        ; - 5
	0634	JMP 0 1                            ; to pc 636
	0635	LOADBOOL 16 0 1
	0636	LOADBOOL 16 1 0
	0637	LOADK 17 -99                       ; Test 17 failed
	0638	CALL 15 3 1
	0639	GETTABUP 15 0 -47                  ; _ENV assert
	0640	GETTABLE 16 6 -89                  ; ReadValue
	0641	LOADK 17 -100                      ; y
	0642	CALL 16 2 2
	0643	EQ 1 16 -39                        ; - 5
	0644	JMP 0 1                            ; to pc 646
	0645	LOADBOOL 16 0 1
	0646	LOADBOOL 16 1 0
	0647	LOADK 17 -101                      ; Test 18 failed
	0648	CALL 15 3 1
	0649	GETTABUP 15 0 -47                  ; _ENV assert
	0650	GETTABLE 16 6 -89                  ; ReadValue
	0651	LOADK 17 -102                      ; z
	0652	CALL 16 2 2
	0653	EQ 1 16 -39                        ; - 5
	0654	JMP 0 1                            ; to pc 656
	0655	LOADBOOL 16 0 1
	0656	LOADBOOL 16 1 0
	0657	LOADK 17 -103                      ; Test 19 failed
	0658	CALL 15 3 1
	0659	GETTABUP 15 0 -47                  ; _ENV assert
	0660	GETTABLE 16 6 -89                  ; ReadValue
	0661	LOADK 17 -104                      ; xp
	0662	CALL 16 2 2
	0663	EQ 1 16 -105                       ; - 1055
	0664	JMP 0 1                            ; to pc 666
	0665	LOADBOOL 16 0 1
	0666	LOADBOOL 16 1 0
	0667	LOADK 17 -106                      ; Test 20 failed
	0668	CALL 15 3 1
	0669	GETTABUP 15 0 -47                  ; _ENV assert
	0670	GETTABLE 16 6 -89                  ; ReadValue
	0671	LOADK 17 -107                      ; damage
	0672	CALL 16 2 2
	0673	EQ 1 16 -24                        ; - 30
	0674	JMP 0 1                            ; to pc 676
	0675	LOADBOOL 16 0 1
	0676	LOADBOOL 16 1 0
	0677	LOADK 17 -108                      ; Test 21 failed
	0678	CALL 15 3 1
	0679	GETTABUP 15 0 -47                  ; _ENV assert
	0680	GETTABLE 16 6 -89                  ; ReadValue
	0681	LOADK 17 -109                      ; maxhealth
	0682	CALL 16 2 2
	0683	EQ 1 16 -31                        ; - 100
	0684	JMP 0 1                            ; to pc 686
	0685	LOADBOOL 16 0 1
	0686	LOADBOOL 16 1 0
	0687	LOADK 17 -110                      ; Test 22 failed
	0688	CALL 15 3 1
	0689	GETTABUP 15 0 -47                  ; _ENV assert
	0690	GETTABLE 16 6 -89                  ; ReadValue
	0691	LOADK 17 -111                      ; inbattle
	0692	CALL 16 2 2
	0693	EQ 1 16 -48                        ; - False
	0694	JMP 0 1                            ; to pc 696
	0695	LOADBOOL 16 0 1
	0696	LOADBOOL 16 1 0
	0697	LOADK 17 -112                      ; Test 23 failed
	0698	CALL 15 3 1
	0699	GETTABUP 15 0 -47                  ; _ENV assert
	0700	GETTABLE 16 6 -89                  ; ReadValue
	0701	LOADK 17 -113                      ; banned
	0702	CALL 16 2 2
	0703	EQ 1 16 -48                        ; - False
	0704	JMP 0 1                            ; to pc 706
	0705	LOADBOOL 16 0 1
	0706	LOADBOOL 16 1 0
	0707	LOADK 17 -114                      ; Test 24 failed
	0708	CALL 15 3 1
	0709	GETTABUP 15 0 -47                  ; _ENV assert
	0710	GETTABLE 16 6 -89                  ; ReadValue
	0711	LOADK 17 -115                      ; flagged
	0712	CALL 16 2 2
	0713	EQ 1 16 -48                        ; - False
	0714	JMP 0 1                            ; to pc 716
	0715	LOADBOOL 16 0 1
	0716	LOADBOOL 16 1 0
	0717	LOADK 17 -116                      ; Test 25 failed
	0718	CALL 15 3 1
	0719	GETTABUP 15 0 -47                  ; _ENV assert
	0720	GETTABLE 16 7 -89                  ; ReadValue
	0721	LOADK 17 -90                       ; id
	0722	CALL 16 2 2
	0723	EQ 1 16 -117                       ; - 2
	0724	JMP 0 1                            ; to pc 726
	0725	LOADBOOL 16 0 1
	0726	LOADBOOL 16 1 0
	0727	LOADK 17 -118                      ; Test 26 failed
	0728	CALL 15 3 1
	0729	GETTABUP 15 0 -47                  ; _ENV assert
	0730	GETTABLE 16 7 -89                  ; ReadValue
	0731	LOADK 17 -92                       ; name
	0732	CALL 16 2 2
	0733	EQ 1 16 -16                        ; - Jay
	0734	JMP 0 1                            ; to pc 736
	0735	LOADBOOL 16 0 1
	0736	LOADBOOL 16 1 0
	0737	LOADK 17 -119                      ; Test 27 failed
	0738	CALL 15 3 1
	0739	GETTABUP 15 0 -47                  ; _ENV assert
	0740	GETTABLE 16 7 -89                  ; ReadValue
	0741	LOADK 17 -94                       ; health
	0742	CALL 16 2 2
	0743	EQ 1 16 -21                        ; - 10
	0744	JMP 0 1                            ; to pc 746
	0745	LOADBOOL 16 0 1
	0746	LOADBOOL 16 1 0
	0747	LOADK 17 -120                      ; Test 28 failed
	0748	CALL 15 3 1
	0749	GETTABUP 15 0 -47                  ; _ENV assert
	0750	GETTABLE 16 7 -89                  ; ReadValue
	0751	LOADK 17 -96                       ; level
	0752	CALL 16 2 2
	0753	EQ 1 16 -3                         ; - 1
	0754	JMP 0 1                            ; to pc 756
	0755	LOADBOOL 16 0 1
	0756	LOADBOOL 16 1 0
	0757	LOADK 17 -121                      ; Test 29 failed
	0758	CALL 15 3 1
	0759	GETTABUP 15 0 -47                  ; _ENV assert
	0760	GETTABLE 16 7 -89                  ; ReadValue
	0761	LOADK 17 -98                       ; x
	0762	CALL 16 2 2
	0763	EQ 1 16 -39                        ; - 5
	0764	JMP 0 1                            ; to pc 766
	0765	LOADBOOL 16 0 1
	0766	LOADBOOL 16 1 0
	0767	LOADK 17 -122                      ; Test 30 failed
	0768	CALL 15 3 1
	0769	GETTABUP 15 0 -47                  ; _ENV assert
	0770	GETTABLE 16 7 -89                  ; ReadValue
	0771	LOADK 17 -100                      ; y
	0772	CALL 16 2 2
	0773	EQ 1 16 -39                        ; - 5
	0774	JMP 0 1                            ; to pc 776
	0775	LOADBOOL 16 0 1
	0776	LOADBOOL 16 1 0
	0777	LOADK 17 -123                      ; Test 31 failed
	0778	CALL 15 3 1
	0779	GETTABUP 15 0 -47                  ; _ENV assert
	0780	GETTABLE 16 7 -89                  ; ReadValue
	0781	LOADK 17 -102                      ; z
	0782	CALL 16 2 2
	0783	EQ 1 16 -39                        ; - 5
	0784	JMP 0 1                            ; to pc 786
	0785	LOADBOOL 16 0 1
	0786	LOADBOOL 16 1 0
	0787	LOADK 17 -124                      ; Test 32 failed
	0788	CALL 15 3 1
	0789	GETTABUP 15 0 -47                  ; _ENV assert
	0790	GETTABLE 16 7 -89                  ; ReadValue
	0791	LOADK 17 -104                      ; xp
	0792	CALL 16 2 2
	0793	EQ 1 16 -125                       ; - 0
	0794	JMP 0 1                            ; to pc 796
	0795	LOADBOOL 16 0 1
	0796	LOADBOOL 16 1 0
	0797	LOADK 17 -126                      ; Test 33 failed
	0798	CALL 15 3 1
	0799	GETTABUP 15 0 -47                  ; _ENV assert
	0800	GETTABLE 16 7 -89                  ; ReadValue
	0801	LOADK 17 -107                      ; damage
	0802	CALL 16 2 2
	0803	EQ 1 16 -3                         ; - 1
	0804	JMP 0 1                            ; to pc 806
	0805	LOADBOOL 16 0 1
	0806	LOADBOOL 16 1 0
	0807	LOADK 17 -127                      ; Test 34 failed
	0808	CALL 15 3 1
	0809	GETTABUP 15 0 -47                  ; _ENV assert
	0810	GETTABLE 16 7 -89                  ; ReadValue
	0811	LOADK 17 -109                      ; maxhealth
	0812	CALL 16 2 2
	0813	EQ 1 16 -21                        ; - 10
	0814	JMP 0 1                            ; to pc 816
	0815	LOADBOOL 16 0 1
	0816	LOADBOOL 16 1 0
	0817	LOADK 17 -128                      ; Test 35 failed
	0818	CALL 15 3 1
	0819	GETTABUP 15 0 -47                  ; _ENV assert
	0820	GETTABLE 16 7 -89                  ; ReadValue
	0821	LOADK 17 -111                      ; inbattle
	0822	CALL 16 2 2
	0823	EQ 1 16 -48                        ; - False
	0824	JMP 0 1                            ; to pc 826
	0825	LOADBOOL 16 0 1
	0826	LOADBOOL 16 1 0
	0827	LOADK 17 -129                      ; Test 36 failed
	0828	CALL 15 3 1
	0829	GETTABUP 15 0 -47                  ; _ENV assert
	0830	GETTABLE 16 7 -89                  ; ReadValue
	0831	LOADK 17 -113                      ; banned
	0832	CALL 16 2 2
	0833	EQ 1 16 -52                        ; - True
	0834	JMP 0 1                            ; to pc 836
	0835	LOADBOOL 16 0 1
	0836	LOADBOOL 16 1 0
	0837	LOADK 17 -130                      ; Test 37 failed
	0838	CALL 15 3 1
	0839	GETTABUP 15 0 -47                  ; _ENV assert
	0840	GETTABLE 16 7 -89                  ; ReadValue
	0841	LOADK 17 -115                      ; flagged
	0842	CALL 16 2 2
	0843	EQ 1 16 -52                        ; - True
	0844	JMP 0 1                            ; to pc 846
	0845	LOADBOOL 16 0 1
	0846	LOADBOOL 16 1 0
	0847	LOADK 17 -131                      ; Test 38 failed
	0848	CALL 15 3 1
	0849	GETTABUP 15 0 -47                  ; _ENV assert
	0850	GETTABLE 16 9 -89                  ; ReadValue
	0851	LOADK 17 -92                       ; name
	0852	CALL 16 2 2
	0853	EQ 1 16 -20                        ; - Joe
	0854	JMP 0 1                            ; to pc 856
	0855	LOADBOOL 16 0 1
	0856	LOADBOOL 16 1 0
	0857	LOADK 17 -132                      ; Test 39 failed
	0858	CALL 15 3 1
	0859	GETTABUP 15 0 -47                  ; _ENV assert
	0860	GETTABLE 16 9 -89                  ; ReadValue
	0861	LOADK 17 -90                       ; id
	0862	CALL 16 2 2
	0863	EQ 1 16 -3                         ; - 1
	0864	JMP 0 1                            ; to pc 866
	0865	LOADBOOL 16 0 1
	0866	LOADBOOL 16 1 0
	0867	LOADK 17 -133                      ; Test 40 failed
	0868	CALL 15 3 1
	0869	GETTABUP 15 0 -47                  ; _ENV assert
	0870	GETTABLE 16 9 -89                  ; ReadValue
	0871	LOADK 17 -94                       ; health
	0872	CALL 16 2 2
	0873	EQ 1 16 -134                       ; - -20
	0874	JMP 0 1                            ; to pc 876
	0875	LOADBOOL 16 0 1
	0876	LOADBOOL 16 1 0
	0877	LOADK 17 -135                      ; Test 41 failed
	0878	CALL 15 3 1
	0879	GETTABUP 15 0 -47                  ; _ENV assert
	0880	GETTABLE 16 9 -89                  ; ReadValue
	0881	LOADK 17 -104                      ; xp
	0882	CALL 16 2 2
	0883	EQ 1 16 -22                        ; - 5
	0884	JMP 0 1                            ; to pc 886
	0885	LOADBOOL 16 0 1
	0886	LOADBOOL 16 1 0
	0887	LOADK 17 -136                      ; Test 42 failed
	0888	CALL 15 3 1
	0889	GETTABUP 15 0 -47                  ; _ENV assert
	0890	GETTABLE 16 9 -89                  ; ReadValue
	0891	LOADK 17 -98                       ; x
	0892	CALL 16 2 2
	0893	EQ 1 16 -39                        ; - 5
	0894	JMP 0 1                            ; to pc 896
	0895	LOADBOOL 16 0 1
	0896	LOADBOOL 16 1 0
	0897	LOADK 17 -137                      ; Test 43 failed
	0898	CALL 15 3 1
	0899	GETTABUP 15 0 -47                  ; _ENV assert
	0900	GETTABLE 16 9 -89                  ; ReadValue
	0901	LOADK 17 -100                      ; y
	0902	CALL 16 2 2
	0903	EQ 1 16 -39                        ; - 5
	0904	JMP 0 1                            ; to pc 906
	0905	LOADBOOL 16 0 1
	0906	LOADBOOL 16 1 0
	0907	LOADK 17 -138                      ; Test 44 failed
	0908	CALL 15 3 1
	0909	GETTABUP 15 0 -47                  ; _ENV assert
	0910	GETTABLE 16 9 -89                  ; ReadValue
	0911	LOADK 17 -102                      ; z
	0912	CALL 16 2 2
	0913	EQ 1 16 -39                        ; - 5
	0914	JMP 0 1                            ; to pc 916
	0915	LOADBOOL 16 0 1
	0916	LOADBOOL 16 1 0
	0917	LOADK 17 -139                      ; Test 45 failed
	0918	CALL 15 3 1
	0919	GETTABUP 15 0 -47                  ; _ENV assert
	0920	GETTABLE 16 9 -89                  ; ReadValue
	0921	LOADK 17 -107                      ; damage
	0922	CALL 16 2 2
	0923	EQ 1 16 -3                         ; - 1
	0924	JMP 0 1                            ; to pc 926
	0925	LOADBOOL 16 0 1
	0926	LOADBOOL 16 1 0
	0927	LOADK 17 -140                      ; Test 46 failed
	0928	CALL 15 3 1
	0929	GETTABUP 15 0 -47                  ; _ENV assert
	0930	GETTABLE 16 9 -89                  ; ReadValue
	0931	LOADK 17 -109                      ; maxhealth
	0932	CALL 16 2 2
	0933	EQ 1 16 -21                        ; - 10
	0934	JMP 0 1                            ; to pc 936
	0935	LOADBOOL 16 0 1
	0936	LOADBOOL 16 1 0
	0937	LOADK 17 -141                      ; Test 47 failed
	0938	CALL 15 3 1
	0939	GETTABUP 15 0 -47                  ; _ENV assert
	0940	GETTABLE 16 10 -89                 ; ReadValue
	0941	LOADK 17 -92                       ; name
	0942	CALL 16 2 2
	0943	EQ 1 16 -23                        ; - Jeffrey
	0944	JMP 0 1                            ; to pc 946
	0945	LOADBOOL 16 0 1
	0946	LOADBOOL 16 1 0
	0947	LOADK 17 -142                      ; Test 48 failed
	0948	CALL 15 3 1
	0949	GETTABUP 15 0 -47                  ; _ENV assert
	0950	GETTABLE 16 10 -89                 ; ReadValue
	0951	LOADK 17 -90                       ; id
	0952	CALL 16 2 2
	0953	EQ 1 16 -117                       ; - 2
	0954	JMP 0 1                            ; to pc 956
	0955	LOADBOOL 16 0 1
	0956	LOADBOOL 16 1 0
	0957	LOADK 17 -143                      ; Test 49 failed
	0958	CALL 15 3 1
	0959	GETTABUP 15 0 -47                  ; _ENV assert
	0960	GETTABLE 16 10 -89                 ; ReadValue
	0961	LOADK 17 -94                       ; health
	0962	CALL 16 2 2
	0963	EQ 1 16 -117                       ; - 2
	0964	JMP 0 1                            ; to pc 966
	0965	LOADBOOL 16 0 1
	0966	LOADBOOL 16 1 0
	0967	LOADK 17 -144                      ; Test 50 failed
	0968	CALL 15 3 1
	0969	GETTABUP 15 0 -47                  ; _ENV assert
	0970	GETTABLE 16 10 -89                 ; ReadValue
	0971	LOADK 17 -104                      ; xp
	0972	CALL 16 2 2
	0973	EQ 1 16 -25                        ; - 20
	0974	JMP 0 1                            ; to pc 976
	0975	LOADBOOL 16 0 1
	0976	LOADBOOL 16 1 0
	0977	LOADK 17 -145                      ; Test 51 failed
	0978	CALL 15 3 1
	0979	GETTABUP 15 0 -47                  ; _ENV assert
	0980	GETTABLE 16 10 -89                 ; ReadValue
	0981	LOADK 17 -98                       ; x
	0982	CALL 16 2 2
	0983	EQ 1 16 -40                        ; - 10
	0984	JMP 0 1                            ; to pc 986
	0985	LOADBOOL 16 0 1
	0986	LOADBOOL 16 1 0
	0987	LOADK 17 -146                      ; Test 52 failed
	0988	CALL 15 3 1
	0989	GETTABUP 15 0 -47                  ; _ENV assert
	0990	GETTABLE 16 10 -89                 ; ReadValue
	0991	LOADK 17 -100                      ; y
	0992	CALL 16 2 2
	0993	EQ 1 16 -40                        ; - 10
	0994	JMP 0 1                            ; to pc 996
	0995	LOADBOOL 16 0 1
	0996	LOADBOOL 16 1 0
	0997	LOADK 17 -147                      ; Test 53 failed
	0998	CALL 15 3 1
	0999	GETTABUP 15 0 -47                  ; _ENV assert
	1000	GETTABLE 16 10 -89                 ; ReadValue
	1001	LOADK 17 -102                      ; z
	1002	CALL 16 2 2
	1003	EQ 1 16 -40                        ; - 10
	1004	JMP 0 1                            ; to pc 1006
	1005	LOADBOOL 16 0 1
	1006	LOADBOOL 16 1 0
	1007	LOADK 17 -148                      ; Test 54 failed
	1008	CALL 15 3 1
	1009	GETTABUP 15 0 -47                  ; _ENV assert
	1010	GETTABLE 16 10 -89                 ; ReadValue
	1011	LOADK 17 -107                      ; damage
	1012	CALL 16 2 2
	1013	EQ 1 16 -26                        ; - 4
	1014	JMP 0 1                            ; to pc 1016
	1015	LOADBOOL 16 0 1
	1016	LOADBOOL 16 1 0
	1017	LOADK 17 -149                      ; Test 55 failed
	1018	CALL 15 3 1
	1019	GETTABUP 15 0 -47                  ; _ENV assert
	1020	GETTABLE 16 10 -89                 ; ReadValue
	1021	LOADK 17 -109                      ; maxhealth
	1022	CALL 16 2 2
	1023	EQ 1 16 -24                        ; - 30
	1024	JMP 0 1                            ; to pc 1026
	1025	LOADBOOL 16 0 1
	1026	LOADBOOL 16 1 0
	1027	LOADK 17 -150                      ; Test 56 failed
	1028	CALL 15 3 1
	1029	GETTABUP 15 0 -47                  ; _ENV assert
	1030	GETTABLE 16 11 -89                 ; ReadValue
	1031	LOADK 17 -92                       ; name
	1032	CALL 16 2 2
	1033	EQ 1 16 -27                        ; - John
	1034	JMP 0 1                            ; to pc 1036
	1035	LOADBOOL 16 0 1
	1036	LOADBOOL 16 1 0
	1037	LOADK 17 -151                      ; Test 57 failed
	1038	CALL 15 3 1
	1039	GETTABUP 15 0 -47                  ; _ENV assert
	1040	GETTABLE 16 11 -89                 ; ReadValue
	1041	LOADK 17 -90                       ; id
	1042	CALL 16 2 2
	1043	EQ 1 16 -32                        ; - 3
	1044	JMP 0 1                            ; to pc 1046
	1045	LOADBOOL 16 0 1
	1046	LOADBOOL 16 1 0
	1047	LOADK 17 -152                      ; Test 58 failed
	1048	CALL 15 3 1
	1049	GETTABUP 15 0 -47                  ; _ENV assert
	1050	GETTABLE 16 11 -89                 ; ReadValue
	1051	LOADK 17 -94                       ; health
	1052	CALL 16 2 2
	1053	EQ 1 16 -153                       ; - 38
	1054	JMP 0 1                            ; to pc 1056
	1055	LOADBOOL 16 0 1
	1056	LOADBOOL 16 1 0
	1057	LOADK 17 -154                      ; Test 59 failed
	1058	CALL 15 3 1
	1059	GETTABUP 15 0 -47                  ; _ENV assert
	1060	GETTABLE 16 11 -89                 ; ReadValue
	1061	LOADK 17 -104                      ; xp
	1062	CALL 16 2 2
	1063	EQ 1 16 -29                        ; - 60
	1064	JMP 0 1                            ; to pc 1066
	1065	LOADBOOL 16 0 1
	1066	LOADBOOL 16 1 0
	1067	LOADK 17 -155                      ; Test 60 failed
	1068	CALL 15 3 1
	1069	GETTABUP 15 0 -47                  ; _ENV assert
	1070	GETTABLE 16 11 -89                 ; ReadValue
	1071	LOADK 17 -98                       ; x
	1072	CALL 16 2 2
	1073	EQ 1 16 -41                        ; - 15
	1074	JMP 0 1                            ; to pc 1076
	1075	LOADBOOL 16 0 1
	1076	LOADBOOL 16 1 0
	1077	LOADK 17 -156                      ; Test 61 failed
	1078	CALL 15 3 1
	1079	GETTABUP 15 0 -47                  ; _ENV assert
	1080	GETTABLE 16 11 -89                 ; ReadValue
	1081	LOADK 17 -100                      ; y
	1082	CALL 16 2 2
	1083	EQ 1 16 -41                        ; - 15
	1084	JMP 0 1                            ; to pc 1086
	1085	LOADBOOL 16 0 1
	1086	LOADBOOL 16 1 0
	1087	LOADK 17 -157                      ; Test 62 failed
	1088	CALL 15 3 1
	1089	GETTABUP 15 0 -47                  ; _ENV assert
	1090	GETTABLE 16 11 -89                 ; ReadValue
	1091	LOADK 17 -102                      ; z
	1092	CALL 16 2 2
	1093	EQ 1 16 -41                        ; - 15
	1094	JMP 0 1                            ; to pc 1096
	1095	LOADBOOL 16 0 1
	1096	LOADBOOL 16 1 0
	1097	LOADK 17 -158                      ; Test 63 failed
	1098	CALL 15 3 1
	1099	GETTABUP 15 0 -47                  ; _ENV assert
	1100	GETTABLE 16 11 -89                 ; ReadValue
	1101	LOADK 17 -107                      ; damage
	1102	CALL 16 2 2
	1103	EQ 1 16 -22                        ; - 5
	1104	JMP 0 1                            ; to pc 1106
	1105	LOADBOOL 16 0 1
	1106	LOADBOOL 16 1 0
	1107	LOADK 17 -159                      ; Test 64 failed
	1108	CALL 15 3 1
	1109	GETTABUP 15 0 -47                  ; _ENV assert
	1110	GETTABLE 16 11 -89                 ; ReadValue
	1111	LOADK 17 -109                      ; maxhealth
	1112	CALL 16 2 2
	1113	EQ 1 16 -28                        ; - 40
	1114	JMP 0 1                            ; to pc 1116
	1115	LOADBOOL 16 0 1
	1116	LOADBOOL 16 1 0
	1117	LOADK 17 -160                      ; Test 65 failed
	1118	CALL 15 3 1
	1119	GETTABUP 15 0 -47                  ; _ENV assert
	1120	GETTABLE 16 12 -89                 ; ReadValue
	1121	LOADK 17 -92                       ; name
	1122	CALL 16 2 2
	1123	EQ 1 16 -30                        ; - Jane
	1124	JMP 0 1                            ; to pc 1126
	1125	LOADBOOL 16 0 1
	1126	LOADBOOL 16 1 0
	1127	LOADK 17 -161                      ; Test 66 failed
	1128	CALL 15 3 1
	1129	GETTABUP 15 0 -47                  ; _ENV assert
	1130	GETTABLE 16 12 -89                 ; ReadValue
	1131	LOADK 17 -90                       ; id
	1132	CALL 16 2 2
	1133	EQ 1 16 -26                        ; - 4
	1134	JMP 0 1                            ; to pc 1136
	1135	LOADBOOL 16 0 1
	1136	LOADBOOL 16 1 0
	1137	LOADK 17 -162                      ; Test 67 failed
	1138	CALL 15 3 1
	1139	GETTABUP 15 0 -47                  ; _ENV assert
	1140	GETTABLE 16 12 -89                 ; ReadValue
	1141	LOADK 17 -94                       ; health
	1142	CALL 16 2 2
	1143	EQ 1 16 -163                       ; - 24
	1144	JMP 0 1                            ; to pc 1146
	1145	LOADBOOL 16 0 1
	1146	LOADBOOL 16 1 0
	1147	LOADK 17 -164                      ; Test 68 failed
	1148	CALL 15 3 1
	1149	GETTABUP 15 0 -47                  ; _ENV assert
	1150	GETTABLE 16 12 -89                 ; ReadValue
	1151	LOADK 17 -104                      ; xp
	1152	CALL 16 2 2
	1153	EQ 1 16 -31                        ; - 100
	1154	JMP 0 1                            ; to pc 1156
	1155	LOADBOOL 16 0 1
	1156	LOADBOOL 16 1 0
	1157	LOADK 17 -165                      ; Test 69 failed
	1158	CALL 15 3 1
	1159	GETTABUP 15 0 -47                  ; _ENV assert
	1160	GETTABLE 16 12 -89                 ; ReadValue
	1161	LOADK 17 -98                       ; x
	1162	CALL 16 2 2
	1163	EQ 1 16 -42                        ; - 100
	1164	JMP 0 1                            ; to pc 1166
	1165	LOADBOOL 16 0 1
	1166	LOADBOOL 16 1 0
	1167	LOADK 17 -166                      ; Test 70 failed
	1168	CALL 15 3 1
	1169	GETTABUP 15 0 -47                  ; _ENV assert
	1170	GETTABLE 16 12 -89                 ; ReadValue
	1171	LOADK 17 -100                      ; y
	1172	CALL 16 2 2
	1173	EQ 1 16 -40                        ; - 10
	1174	JMP 0 1                            ; to pc 1176
	1175	LOADBOOL 16 0 1
	1176	LOADBOOL 16 1 0
	1177	LOADK 17 -167                      ; Test 71 failed
	1178	CALL 15 3 1
	1179	GETTABUP 15 0 -47                  ; _ENV assert
	1180	GETTABLE 16 12 -89                 ; ReadValue
	1181	LOADK 17 -102                      ; z
	1182	CALL 16 2 2
	1183	EQ 1 16 -43                        ; - 30
	1184	JMP 0 1                            ; to pc 1186
	1185	LOADBOOL 16 0 1
	1186	LOADBOOL 16 1 0
	1187	LOADK 17 -168                      ; Test 72 failed
	1188	CALL 15 3 1
	1189	GETTABUP 15 0 -47                  ; _ENV assert
	1190	GETTABLE 16 12 -89                 ; ReadValue
	1191	LOADK 17 -107                      ; damage
	1192	CALL 16 2 2
	1193	EQ 1 16 -32                        ; - 3
	1194	JMP 0 1                            ; to pc 1196
	1195	LOADBOOL 16 0 1
	1196	LOADBOOL 16 1 0
	1197	LOADK 17 -169                      ; Test 73 failed
	1198	CALL 15 3 1
	1199	GETTABUP 15 0 -47                  ; _ENV assert
	1200	GETTABLE 16 12 -89                 ; ReadValue
	1201	LOADK 17 -109                      ; maxhealth
	1202	CALL 16 2 2
	1203	EQ 1 16 -29                        ; - 60
	1204	JMP 0 1                            ; to pc 1206
	1205	LOADBOOL 16 0 1
	1206	LOADBOOL 16 1 0
	1207	LOADK 17 -170                      ; Test 74 failed
	1208	CALL 15 3 1
	1209	GETTABUP 15 0 -47                  ; _ENV assert
	1210	GETTABLE 16 13 -89                 ; ReadValue
	1211	LOADK 17 -92                       ; name
	1212	CALL 16 2 2
	1213	EQ 1 16 -33                        ; - Jill
	1214	JMP 0 1                            ; to pc 1216
	1215	LOADBOOL 16 0 1
	1216	LOADBOOL 16 1 0
	1217	LOADK 17 -171                      ; Test 75 failed
	1218	CALL 15 3 1
	1219	GETTABUP 15 0 -47                  ; _ENV assert
	1220	GETTABLE 16 13 -89                 ; ReadValue
	1221	LOADK 17 -90                       ; id
	1222	CALL 16 2 2
	1223	EQ 1 16 -22                        ; - 5
	1224	JMP 0 1                            ; to pc 1226
	1225	LOADBOOL 16 0 1
	1226	LOADBOOL 16 1 0
	1227	LOADK 17 -172                      ; Test 76 failed
	1228	CALL 15 3 1
	1229	GETTABUP 15 0 -47                  ; _ENV assert
	1230	GETTABLE 16 13 -89                 ; ReadValue
	1231	LOADK 17 -94                       ; health
	1232	CALL 16 2 2
	1233	EQ 1 16 -173                       ; - 58
	1234	JMP 0 1                            ; to pc 1236
	1235	LOADBOOL 16 0 1
	1236	LOADBOOL 16 1 0
	1237	LOADK 17 -174                      ; Test 77 failed
	1238	CALL 15 3 1
	1239	GETTABUP 15 0 -47                  ; _ENV assert
	1240	GETTABLE 16 13 -89                 ; ReadValue
	1241	LOADK 17 -104                      ; xp
	1242	CALL 16 2 2
	1243	EQ 1 16 -34                        ; - 300
	1244	JMP 0 1                            ; to pc 1246
	1245	LOADBOOL 16 0 1
	1246	LOADBOOL 16 1 0
	1247	LOADK 17 -175                      ; Test 78 failed
	1248	CALL 15 3 1
	1249	GETTABUP 15 0 -47                  ; _ENV assert
	1250	GETTABLE 16 13 -89                 ; ReadValue
	1251	LOADK 17 -98                       ; x
	1252	CALL 16 2 2
	1253	EQ 1 16 -44                        ; - 200
	1254	JMP 0 1                            ; to pc 1256
	1255	LOADBOOL 16 0 1
	1256	LOADBOOL 16 1 0
	1257	LOADK 17 -176                      ; Test 79 failed
	1258	CALL 15 3 1
	1259	GETTABUP 15 0 -47                  ; _ENV assert
	1260	GETTABLE 16 13 -89                 ; ReadValue
	1261	LOADK 17 -100                      ; y
	1262	CALL 16 2 2
	1263	EQ 1 16 -45                        ; - 0
	1264	JMP 0 1                            ; to pc 1266
	1265	LOADBOOL 16 0 1
	1266	LOADBOOL 16 1 0
	1267	LOADK 17 -177                      ; Test 80 failed
	1268	CALL 15 3 1
	1269	GETTABUP 15 0 -47                  ; _ENV assert
	1270	GETTABLE 16 13 -89                 ; ReadValue
	1271	LOADK 17 -102                      ; z
	1272	CALL 16 2 2
	1273	EQ 1 16 -45                        ; - 0
	1274	JMP 0 1                            ; to pc 1276
	1275	LOADBOOL 16 0 1
	1276	LOADBOOL 16 1 0
	1277	LOADK 17 -178                      ; Test 81 failed
	1278	CALL 15 3 1
	1279	GETTABUP 15 0 -47                  ; _ENV assert
	1280	GETTABLE 16 13 -89                 ; ReadValue
	1281	LOADK 17 -107                      ; damage
	1282	CALL 16 2 2
	1283	EQ 1 16 -22                        ; - 5
	1284	JMP 0 1                            ; to pc 1286
	1285	LOADBOOL 16 0 1
	1286	LOADBOOL 16 1 0
	1287	LOADK 17 -179                      ; Test 82 failed
	1288	CALL 15 3 1
	1289	GETTABUP 15 0 -47                  ; _ENV assert
	1290	GETTABLE 16 13 -89                 ; ReadValue
	1291	LOADK 17 -109                      ; maxhealth
	1292	CALL 16 2 2
	1293	EQ 1 16 -31                        ; - 100
	1294	JMP 0 1                            ; to pc 1296
	1295	LOADBOOL 16 0 1
	1296	LOADBOOL 16 1 0
	1297	LOADK 17 -180                      ; Test 83 failed
	1298	CALL 15 3 1
	1299	GETTABUP 15 0 -47                  ; _ENV assert
	1300	GETTABLE 16 14 -89                 ; ReadValue
	1301	LOADK 17 -92                       ; name
	1302	CALL 16 2 2
	1303	EQ 1 16 -35                        ; - Johnny
	1304	JMP 0 1                            ; to pc 1306
	1305	LOADBOOL 16 0 1
	1306	LOADBOOL 16 1 0
	1307	LOADK 17 -181                      ; Test 84 failed
	1308	CALL 15 3 1
	1309	GETTABUP 15 0 -47                  ; _ENV assert
	1310	GETTABLE 16 14 -89                 ; ReadValue
	1311	LOADK 17 -90                       ; id
	1312	CALL 16 2 2
	1313	EQ 1 16 -182                       ; - 6
	1314	JMP 0 1                            ; to pc 1316
	1315	LOADBOOL 16 0 1
	1316	LOADBOOL 16 1 0
	1317	LOADK 17 -183                      ; Test 85 failed
	1318	CALL 15 3 1
	1319	GETTABUP 15 0 -47                  ; _ENV assert
	1320	GETTABLE 16 14 -89                 ; ReadValue
	1321	LOADK 17 -94                       ; health
	1322	CALL 16 2 2
	1323	EQ 1 16 -184                       ; - -10
	1324	JMP 0 1                            ; to pc 1326
	1325	LOADBOOL 16 0 1
	1326	LOADBOOL 16 1 0
	1327	LOADK 17 -185                      ; Test 86 failed
	1328	CALL 15 3 1
	1329	GETTABUP 15 0 -47                  ; _ENV assert
	1330	GETTABLE 16 14 -89                 ; ReadValue
	1331	LOADK 17 -104                      ; xp
	1332	CALL 16 2 2
	1333	EQ 1 16 -37                        ; - 1000
	1334	JMP 0 1                            ; to pc 1336
	1335	LOADBOOL 16 0 1
	1336	LOADBOOL 16 1 0
	1337	LOADK 17 -186                      ; Test 87 failed
	1338	CALL 15 3 1
	1339	GETTABUP 15 0 -47                  ; _ENV assert
	1340	GETTABLE 16 14 -89                 ; ReadValue
	1341	LOADK 17 -98                       ; x
	1342	CALL 16 2 2
	1343	EQ 1 16 -46                        ; - 300
	1344	JMP 0 1                            ; to pc 1346
	1345	LOADBOOL 16 0 1
	1346	LOADBOOL 16 1 0
	1347	LOADK 17 -187                      ; Test 88 failed
	1348	CALL 15 3 1
	1349	GETTABUP 15 0 -47                  ; _ENV assert
	1350	GETTABLE 16 14 -89                 ; ReadValue
	1351	LOADK 17 -100                      ; y
	1352	CALL 16 2 2
	1353	EQ 1 16 -44                        ; - 200
	1354	JMP 0 1                            ; to pc 1356
	1355	LOADBOOL 16 0 1
	1356	LOADBOOL 16 1 0
	1357	LOADK 17 -188                      ; Test 89 failed
	1358	CALL 15 3 1
	1359	GETTABUP 15 0 -47                  ; _ENV assert
	1360	GETTABLE 16 14 -89                 ; ReadValue
	1361	LOADK 17 -102                      ; z
	1362	CALL 16 2 2
	1363	EQ 1 16 -42                        ; - 100
	1364	JMP 0 1                            ; to pc 1366
	1365	LOADBOOL 16 0 1
	1366	LOADBOOL 16 1 0
	1367	LOADK 17 -189                      ; Test 90 failed
	1368	CALL 15 3 1
	1369	GETTABUP 15 0 -47                  ; _ENV assert
	1370	GETTABLE 16 14 -89                 ; ReadValue
	1371	LOADK 17 -107                      ; damage
	1372	CALL 16 2 2
	1373	EQ 1 16 -21                        ; - 10
	1374	JMP 0 1                            ; to pc 1376
	1375	LOADBOOL 16 0 1
	1376	LOADBOOL 16 1 0
	1377	LOADK 17 -190                      ; Test 91 failed
	1378	CALL 15 3 1
	1379	GETTABUP 15 0 -47                  ; _ENV assert
	1380	GETTABLE 16 14 -89                 ; ReadValue
	1381	LOADK 17 -109                      ; maxhealth
	1382	CALL 16 2 2
	1383	EQ 1 16 -36                        ; - 200
	1384	JMP 0 1                            ; to pc 1386
	1385	LOADBOOL 16 0 1
	1386	LOADBOOL 16 1 0
	1387	LOADK 17 -191                      ; Test 92 failed
	1388	CALL 15 3 1
	1389	GETTABUP 15 0 -47                  ; _ENV assert
	1390	EQ 0 6 -192                        ; - 
	1391	JMP 0 1                            ; to pc 1393
	1392	LOADBOOL 16 0 1
	1393	LOADBOOL 16 1 0
	1394	LOADK 17 -193                      ; Test 93 failed
	1395	CALL 15 3 1
	1396	GETTABUP 15 0 -47                  ; _ENV assert
	1397	EQ 0 7 -192                        ; - 
	1398	JMP 0 1                            ; to pc 1400
	1399	LOADBOOL 16 0 1
	1400	LOADBOOL 16 1 0
	1401	LOADK 17 -194                      ; Test 94 failed
	1402	CALL 15 3 1
	1403	GETTABUP 15 0 -47                  ; _ENV assert
	1404	EQ 0 9 -192                        ; - 
	1405	JMP 0 1                            ; to pc 1407
	1406	LOADBOOL 16 0 1
	1407	LOADBOOL 16 1 0
	1408	LOADK 17 -195                      ; Test 95 failed
	1409	CALL 15 3 1
	1410	GETTABUP 15 0 -47                  ; _ENV assert
	1411	EQ 0 10 -192                       ; - 
	1412	JMP 0 1                            ; to pc 1414
	1413	LOADBOOL 16 0 1
	1414	LOADBOOL 16 1 0
	1415	LOADK 17 -196                      ; Test 96 failed
	1416	CALL 15 3 1
	1417	GETTABUP 15 0 -47                  ; _ENV assert
	1418	EQ 0 11 -192                       ; - 
	1419	JMP 0 1                            ; to pc 1421
	1420	LOADBOOL 16 0 1
	1421	LOADBOOL 16 1 0
	1422	LOADK 17 -197                      ; Test 97 failed
	1423	CALL 15 3 1
	1424	GETTABUP 15 0 -47                  ; _ENV assert
	1425	EQ 0 12 -192                       ; - 
	1426	JMP 0 1                            ; to pc 1428
	1427	LOADBOOL 16 0 1
	1428	LOADBOOL 16 1 0
	1429	LOADK 17 -198                      ; Test 98 failed
	1430	CALL 15 3 1
	1431	GETTABUP 15 0 -47                  ; _ENV assert
	1432	EQ 0 13 -192                       ; - 
	1433	JMP 0 1                            ; to pc 1435
	1434	LOADBOOL 16 0 1
	1435	LOADBOOL 16 1 0
	1436	LOADK 17 -199                      ; Test 99 failed
	1437	CALL 15 3 1
	1438	GETTABUP 15 0 -47                  ; _ENV assert
	1439	EQ 0 14 -192                       ; - 
	1440	JMP 0 1                            ; to pc 1442
	1441	LOADBOOL 16 0 1
	1442	LOADBOOL 16 1 0
	1443	LOADK 17 -200                      ; Test 100 failed
	1444	CALL 15 3 1
	1445	RETURN 0 1

	locals (15)
		name: Vector3; startpc: 1; endpc: 1445
		name: Enemy; startpc: 4; endpc: 1445
		name: Player; startpc: 8; endpc: 1445
		name: Administrator; startpc: 14; endpc: 1445
		name: Factory; startpc: 17; endpc: 1445
		name: generator; startpc: 21; endpc: 1445
		name: player; startpc: 27; endpc: 1445
		name: friend; startpc: 36; endpc: 1445
		name: admin; startpc: 44; endpc: 1445
		name: enemy1; startpc: 53; endpc: 1445
		name: enemy2; startpc: 59; endpc: 1445
		name: enemy3; startpc: 65; endpc: 1445
		name: enemy4; startpc: 71; endpc: 1445
		name: enemy5; startpc: 77; endpc: 1445
		name: boss; startpc: 83; endpc: 1445
	upvales (1)
		name: _ENV; instack: true; index: 0
	constants (200)
		id: 0; value: New
		id: 1; value: NextId
		id: 2; value: 1
		id: 3; value: NextId
		id: 4; value: 1
		id: 5; value: New
		id: 6; value: StartBattle
		id: 7; value: New
		id: 8; value: New
		id: 9; value: print
		id: 10; value: Created factory
		id: 11; value: GeneratePlayer
		id: 12; value: Jeff
		id: 13; value: Created player 
		id: 14; value: GetName
		id: 15; value: Jay
		id: 16; value: GenerateAdmin
		id: 17; value: Created admin
		id: 18; value: GenerateEnemy
		id: 19; value: Joe
		id: 20; value: 10
		id: 21; value: 5
		id: 22; value: Jeffrey
		id: 23; value: 30
		id: 24; value: 20
		id: 25; value: 4
		id: 26; value: John
		id: 27; value: 40
		id: 28; value: 60
		id: 29; value: Jane
		id: 30; value: 100
		id: 31; value: 3
		id: 32; value: Jill
		id: 33; value: 300
		id: 34; value: Johnny
		id: 35; value: 200
		id: 36; value: 1000
		id: 37; value: SetPosition
		id: 38; value: 5
		id: 39; value: 10
		id: 40; value: 15
		id: 41; value: 100
		id: 42; value: 30
		id: 43; value: 200
		id: 44; value: 0
		id: 45; value: 300
		id: 46; value: assert
		id: 47; value: False
		id: 48; value: Test 1 failed
		id: 49; value: PrintStatus
		id: 50; value:  
		id: 51; value: True
		id: 52; value: Test 2 failed
		id: 53; value: Test 3 failed
		id: 54; value: Reset
		id: 55; value: GainXp
		id: 56; value:  gained 10 XP
		id: 57; value:  gained 20 XP
		id: 58; value: Test 4 failed
		id: 59; value: Test 5 failed
		id: 60; value: GetX
		id: 61; value: GetY
		id: 62; value: GetZ
		id: 63; value: Test 6 failed
		id: 64; value: 50
		id: 65; value:  gained 50 XP
		id: 66; value: Test 7 failed
		id: 67; value: 150
		id: 68; value:  gained 150 XP
		id: 69; value: Test 8 failed
		id: 70; value:  gained 300 XP
		id: 71; value: 400
		id: 72; value:  gained 400 XP
		id: 73; value: 500
		id: 74; value:  gained 500 XP
		id: 75; value: 600
		id: 76; value:  gained 600 XP
		id: 77; value: Test 9 failed
		id: 78; value:  gives 1015 xp to 
		id: 79; value: DonateXp
		id: 80; value: 1015
		id: 81; value:  cheats to level 99
		id: 82; value: Cheat
		id: 83; value: Banning 
		id: 84; value: BanPlayer
		id: 85; value: Test 10 failed
		id: 86; value: Test 11 failed
		id: 87; value: Test 12 failed
		id: 88; value: ReadValue
		id: 89; value: id
		id: 90; value: Test 13 failed
		id: 91; value: name
		id: 92; value: Test 14 failed
		id: 93; value: health
		id: 94; value: Test 15 failed
		id: 95; value: level
		id: 96; value: Test 16 failed
		id: 97; value: x
		id: 98; value: Test 17 failed
		id: 99; value: y
		id: 100; value: Test 18 failed
		id: 101; value: z
		id: 102; value: Test 19 failed
		id: 103; value: xp
		id: 104; value: 1055
		id: 105; value: Test 20 failed
		id: 106; value: damage
		id: 107; value: Test 21 failed
		id: 108; value: maxhealth
		id: 109; value: Test 22 failed
		id: 110; value: inbattle
		id: 111; value: Test 23 failed
		id: 112; value: banned
		id: 113; value: Test 24 failed
		id: 114; value: flagged
		id: 115; value: Test 25 failed
		id: 116; value: 2
		id: 117; value: Test 26 failed
		id: 118; value: Test 27 failed
		id: 119; value: Test 28 failed
		id: 120; value: Test 29 failed
		id: 121; value: Test 30 failed
		id: 122; value: Test 31 failed
		id: 123; value: Test 32 failed
		id: 124; value: 0
		id: 125; value: Test 33 failed
		id: 126; value: Test 34 failed
		id: 127; value: Test 35 failed
		id: 128; value: Test 36 failed
		id: 129; value: Test 37 failed
		id: 130; value: Test 38 failed
		id: 131; value: Test 39 failed
		id: 132; value: Test 40 failed
		id: 133; value: -20
		id: 134; value: Test 41 failed
		id: 135; value: Test 42 failed
		id: 136; value: Test 43 failed
		id: 137; value: Test 44 failed
		id: 138; value: Test 45 failed
		id: 139; value: Test 46 failed
		id: 140; value: Test 47 failed
		id: 141; value: Test 48 failed
		id: 142; value: Test 49 failed
		id: 143; value: Test 50 failed
		id: 144; value: Test 51 failed
		id: 145; value: Test 52 failed
		id: 146; value: Test 53 failed
		id: 147; value: Test 54 failed
		id: 148; value: Test 55 failed
		id: 149; value: Test 56 failed
		id: 150; value: Test 57 failed
		id: 151; value: Test 58 failed
		id: 152; value: 38
		id: 153; value: Test 59 failed
		id: 154; value: Test 60 failed
		id: 155; value: Test 61 failed
		id: 156; value: Test 62 failed
		id: 157; value: Test 63 failed
		id: 158; value: Test 64 failed
		id: 159; value: Test 65 failed
		id: 160; value: Test 66 failed
		id: 161; value: Test 67 failed
		id: 162; value: 24
		id: 163; value: Test 68 failed
		id: 164; value: Test 69 failed
		id: 165; value: Test 70 failed
		id: 166; value: Test 71 failed
		id: 167; value: Test 72 failed
		id: 168; value: Test 73 failed
		id: 169; value: Test 74 failed
		id: 170; value: Test 75 failed
		id: 171; value: Test 76 failed
		id: 172; value: 58
		id: 173; value: Test 77 failed
		id: 174; value: Test 78 failed
		id: 175; value: Test 79 failed
		id: 176; value: Test 80 failed
		id: 177; value: Test 81 failed
		id: 178; value: Test 82 failed
		id: 179; value: Test 83 failed
		id: 180; value: Test 84 failed
		id: 181; value: 6
		id: 182; value: Test 85 failed
		id: 183; value: -10
		id: 184; value: Test 86 failed
		id: 185; value: Test 87 failed
		id: 186; value: Test 88 failed
		id: 187; value: Test 89 failed
		id: 188; value: Test 90 failed
		id: 189; value: Test 91 failed
		id: 190; value: Test 92 failed
		id: 191; value: 
		id: 192; value: Test 93 failed
		id: 193; value: Test 94 failed
		id: 194; value: Test 95 failed
		id: 195; value: Test 96 failed
		id: 196; value: Test 97 failed
		id: 197; value: Test 98 failed
		id: 198; value: Test 99 failed
		id: 199; value: Test 100 failed
end

3 params; 0 varargs; 4 slots; 6 opcodes; 3 constants; 0 upvalues; 0 functions; 4 locals
function func1(param1, param2, param3)
	0001	NEWTABLE 3 0 0
	0002	SETTABLE 3 -1 0                    ; x
	0003	SETTABLE 3 -2 1                    ; y
	0004	SETTABLE 3 -3 2                    ; z
	0005	RETURN 3 2
	0006	RETURN 0 1

	locals (4)
		name: x; startpc: 0; endpc: 6
		name: y; startpc: 0; endpc: 6
		name: z; startpc: 0; endpc: 6
		name: this; startpc: 1; endpc: 6
	upvales (0)
	constants (3)
		id: 0; value: x
		id: 1; value: y
		id: 2; value: z
end

4 params; 0 varargs; 15 slots; 45 opcodes; 17 constants; 3 upvalues; 13 functions; 14 locals
function func2(param1, param2, param3, param4)
	0001	NEWTABLE 4 0 0
	0002	GETTABUP 5 0 -1                    ; Enemy NextId
	0003	MOVE 6 0
	0004	MOVE 7 1
	0005	GETTABUP 8 1 -2                    ; Vector3 New
	0006	LOADK 9 -3                         ; 10
	0007	LOADK 10 -3                        ; 10
	0008	LOADK 11 -3                        ; 10
	0009	CALL 8 4 2
	0010	LOADBOOL 9 0 0
	0011	MOVE 10 2
	0012	MOVE 11 3
	0013	LOADNIL 12 0
	0014	MOVE 13 1
	0015	CLOSURE 14 0                       ; func3
	0016	SETTABLE 4 -4 14                   ; Reset
	0017	CLOSURE 14 1                       ; func4
	0018	SETTABLE 4 -5 14                   ; ReadValue
	0019	CLOSURE 14 2                       ; func5
	0020	SETTABLE 4 -6 14                   ; PrintStatus
	0021	CLOSURE 14 3                       ; func6
	0022	SETTABLE 4 -7 14                   ; GetName
	0023	CLOSURE 14 4                       ; func7
	0024	SETTABLE 4 -8 14                   ; SetPosition
	0025	CLOSURE 14 5                       ; func8
	0026	SETTABLE 4 -9 14                   ; GetX
	0027	CLOSURE 14 6                       ; func9
	0028	SETTABLE 4 -10 14                  ; GetY
	0029	CLOSURE 14 7                       ; func10
	0030	SETTABLE 4 -11 14                  ; GetZ
	0031	CLOSURE 14 8                       ; func11
	0032	SETTABLE 4 -12 14                  ; GetAttacked
	0033	CLOSURE 14 9                       ; func12
	0034	SETTABLE 4 -13 14                  ; IsDead
	0035	CLOSURE 14 10                      ; func13
	0036	SETTABLE 4 -14 14                  ; GetXp
	0037	CLOSURE 14 11                      ; func14
	0038	SETTABLE 4 -15 14                  ; Attack
	0039	CLOSURE 14 12                      ; func15
	0040	SETTABLE 4 -16 14                  ; SetPlayer
	0041	GETTABUP 14 0 -1                   ; Enemy NextId
	0042	ADD 14 14 -17                      ; - 1
	0043	SETTABUP 0 -1 14                   ; Enemy NextId
	0044	RETURN 4 2
	0045	RETURN 0 1

	locals (14)
		name: name; startpc: 0; endpc: 45
		name: health; startpc: 0; endpc: 45
		name: xp; startpc: 0; endpc: 45
		name: damage; startpc: 0; endpc: 45
		name: this; startpc: 1; endpc: 45
		name: Id; startpc: 2; endpc: 45
		name: Name; startpc: 3; endpc: 45
		name: Health; startpc: 4; endpc: 45
		name: Position; startpc: 9; endpc: 45
		name: Dead; startpc: 10; endpc: 45
		name: Xp; startpc: 11; endpc: 45
		name: Damage; startpc: 12; endpc: 45
		name: Player; startpc: 13; endpc: 45
		name: MaxHealth; startpc: 14; endpc: 45
	upvales (3)
		name: Enemy; instack: true; index: 1
		name: Vector3; instack: true; index: 0
		name: _ENV; instack: false; index: 0
	constants (17)
		id: 0; value: NextId
		id: 1; value: New
		id: 2; value: 10
		id: 3; value: Reset
		id: 4; value: ReadValue
		id: 5; value: PrintStatus
		id: 6; value: GetName
		id: 7; value: SetPosition
		id: 8; value: GetX
		id: 9; value: GetY
		id: 10; value: GetZ
		id: 11; value: GetAttacked
		id: 12; value: IsDead
		id: 13; value: GetXp
		id: 14; value: Attack
		id: 15; value: SetPlayer
		id: 16; value: 1
end

0 params; 0 varargs; 2 slots; 7 opcodes; 0 constants; 4 upvalues; 0 functions; 0 locals
function func3()
	0001	GETUPVAL 0 1                       ; MaxHealth
	0002	SETUPVAL 0 0                       ; Health
	0003	LOADBOOL 0 0 0
	0004	SETUPVAL 0 2                       ; Dead
	0005	LOADNIL 0 0
	0006	SETUPVAL 0 3                       ; Player
	0007	RETURN 0 1

	locals (0)
	upvales (4)
		name: Health; instack: true; index: 7
		name: MaxHealth; instack: true; index: 13
		name: Dead; instack: true; index: 9
		name: Player; instack: true; index: 12
	constants (0)
end

1 param; 0 varargs; 3 slots; 26 opcodes; 11 constants; 9 upvalues; 0 functions; 2 locals
function func4(param1)
	0001	NEWTABLE 1 0 0
	0002	GETUPVAL 2 0                       ; Id
	0003	SETTABLE 1 -1 2                    ; id
	0004	GETUPVAL 2 1                       ; Name
	0005	SETTABLE 1 -2 2                    ; name
	0006	GETUPVAL 2 2                       ; Health
	0007	SETTABLE 1 -3 2                    ; health
	0008	GETTABUP 2 3 -4                    ; Position x
	0009	SETTABLE 1 -4 2                    ; x
	0010	GETTABUP 2 3 -5                    ; Position y
	0011	SETTABLE 1 -5 2                    ; y
	0012	GETTABUP 2 3 -6                    ; Position z
	0013	SETTABLE 1 -6 2                    ; z
	0014	GETUPVAL 2 4                       ; Dead
	0015	SETTABLE 1 -7 2                    ; dead
	0016	GETUPVAL 2 5                       ; Xp
	0017	SETTABLE 1 -8 2                    ; xp
	0018	GETUPVAL 2 6                       ; Damage
	0019	SETTABLE 1 -9 2                    ; damage
	0020	GETUPVAL 2 7                       ; Player
	0021	SETTABLE 1 -10 2                   ; player
	0022	GETUPVAL 2 8                       ; MaxHealth
	0023	SETTABLE 1 -11 2                   ; maxhealth
	0024	GETTABLE 2 1 0
	0025	RETURN 2 2
	0026	RETURN 0 1

	locals (2)
		name: key; startpc: 0; endpc: 26
		name: data; startpc: 1; endpc: 26
	upvales (9)
		name: Id; instack: true; index: 5
		name: Name; instack: true; index: 6
		name: Health; instack: true; index: 7
		name: Position; instack: true; index: 8
		name: Dead; instack: true; index: 9
		name: Xp; instack: true; index: 10
		name: Damage; instack: true; index: 11
		name: Player; instack: true; index: 12
		name: MaxHealth; instack: true; index: 13
	constants (11)
		id: 0; value: id
		id: 1; value: name
		id: 2; value: health
		id: 3; value: x
		id: 4; value: y
		id: 5; value: z
		id: 6; value: dead
		id: 7; value: xp
		id: 8; value: damage
		id: 9; value: player
		id: 10; value: maxhealth
end

0 params; 0 varargs; 7 slots; 41 opcodes; 13 constants; 7 upvalues; 0 functions; 0 locals
function func5()
	0001	GETTABUP 0 0 -1                    ; _ENV print
	0002	LOADK 1 -2                         ; -----ENEMY INFO-----
	0003	CALL 0 2 1
	0004	GETTABUP 0 0 -1                    ; _ENV print
	0005	LOADK 1 -3                         ; Name: 
	0006	GETUPVAL 2 1                       ; Name
	0007	CONCAT 1 1 2
	0008	CALL 0 2 1
	0009	GETTABUP 0 0 -1                    ; _ENV print
	0010	LOADK 1 -4                         ; Id: 
	0011	GETUPVAL 2 2                       ; Id
	0012	CONCAT 1 1 2
	0013	CALL 0 2 1
	0014	GETTABUP 0 0 -1                    ; _ENV print
	0015	LOADK 1 -5                         ; Health: 
	0016	GETUPVAL 2 3                       ; Health
	0017	CONCAT 1 1 2
	0018	CALL 0 2 1
	0019	GETTABUP 0 0 -1                    ; _ENV print
	0020	LOADK 1 -6                         ; XP: 
	0021	GETUPVAL 2 4                       ; Xp
	0022	CONCAT 1 1 2
	0023	CALL 0 2 1
	0024	GETTABUP 0 0 -1                    ; _ENV print
	0025	LOADK 1 -7                         ; Position: 
	0026	GETTABUP 2 5 -8                    ; Position x
	0027	LOADK 3 -9                         ; , 
	0028	GETTABUP 4 5 -10                   ; Position y
	0029	LOADK 5 -9                         ; , 
	0030	GETTABUP 6 5 -11                   ; Position z
	0031	CONCAT 1 1 6
	0032	CALL 0 2 1
	0033	GETTABUP 0 0 -1                    ; _ENV print
	0034	LOADK 1 -12                        ; Attack Damage: 
	0035	GETUPVAL 2 6                       ; Damage
	0036	CONCAT 1 1 2
	0037	CALL 0 2 1
	0038	GETTABUP 0 0 -1                    ; _ENV print
	0039	LOADK 1 -13                        ; --------------------
	0040	CALL 0 2 1
	0041	RETURN 0 1

	locals (0)
	upvales (7)
		name: _ENV; instack: false; index: 2
		name: Name; instack: true; index: 6
		name: Id; instack: true; index: 5
		name: Health; instack: true; index: 7
		name: Xp; instack: true; index: 10
		name: Position; instack: true; index: 8
		name: Damage; instack: true; index: 11
	constants (13)
		id: 0; value: print
		id: 1; value: -----ENEMY INFO-----
		id: 2; value: Name: 
		id: 3; value: Id: 
		id: 4; value: Health: 
		id: 5; value: XP: 
		id: 6; value: Position: 
		id: 7; value: x
		id: 8; value: , 
		id: 9; value: y
		id: 10; value: z
		id: 11; value: Attack Damage: 
		id: 12; value: --------------------
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func6()
	0001	GETUPVAL 0 0                       ; Name
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Name; instack: true; index: 6
	constants (0)
end

3 params; 0 varargs; 3 slots; 4 opcodes; 3 constants; 1 upvalue; 0 functions; 3 locals
function func7(param1, param2, param3)
	0001	SETTABUP 0 -1 0                    ; Position x
	0002	SETTABUP 0 -2 1                    ; Position y
	0003	SETTABUP 0 -3 2                    ; Position z
	0004	RETURN 0 1

	locals (3)
		name: x; startpc: 0; endpc: 4
		name: y; startpc: 0; endpc: 4
		name: z; startpc: 0; endpc: 4
	upvales (1)
		name: Position; instack: true; index: 8
	constants (3)
		id: 0; value: x
		id: 1; value: y
		id: 2; value: z
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 1 upvalue; 0 functions; 0 locals
function func8()
	0001	GETTABUP 0 0 -1                    ; Position x
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Position; instack: true; index: 8
	constants (1)
		id: 0; value: x
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 1 upvalue; 0 functions; 0 locals
function func9()
	0001	GETTABUP 0 0 -1                    ; Position y
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Position; instack: true; index: 8
	constants (1)
		id: 0; value: y
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 1 upvalue; 0 functions; 0 locals
function func10()
	0001	GETTABUP 0 0 -1                    ; Position z
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Position; instack: true; index: 8
	constants (1)
		id: 0; value: z
end

1 param; 0 varargs; 2 slots; 9 opcodes; 1 constant; 2 upvalues; 0 functions; 1 local
function func11(param1)
	0001	GETUPVAL 1 0                       ; Health
	0002	SUB 1 1 
	0003	SETUPVAL 1 0                       ; Health
	0004	GETUPVAL 1 0                       ; Health
	0005	LE 0 1 -1                          ; - 0
	0006	JMP 0 2                            ; to pc 9
	0007	LOADBOOL 1 1 0
	0008	SETUPVAL 1 1                       ; Dead
	0009	RETURN 0 1

	locals (1)
		name: damage; startpc: 0; endpc: 9
	upvales (2)
		name: Health; instack: true; index: 7
		name: Dead; instack: true; index: 9
	constants (1)
		id: 0; value: 0
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func12()
	0001	GETUPVAL 0 0                       ; Dead
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Dead; instack: true; index: 9
	constants (0)
end

0 params; 0 varargs; 2 slots; 9 opcodes; 2 constants; 2 upvalues; 0 functions; 0 locals
function func13()
	0001	GETTABUP 0 0 -1                    ; this IsDead
	0002	CALL 0 1 2
	0003	TEST 0 0
	0004	JMP 0 2                            ; to pc 7
	0005	GETUPVAL 0 1                       ; Xp
	0006	RETURN 0 2
	0007	LOADK 0 -2                         ; 0
	0008	RETURN 0 2
	0009	RETURN 0 1

	locals (0)
	upvales (2)
		name: this; instack: true; index: 4
		name: Xp; instack: true; index: 10
	constants (2)
		id: 0; value: IsDead
		id: 1; value: 0
end

0 params; 0 varargs; 2 slots; 7 opcodes; 2 constants; 2 upvalues; 0 functions; 0 locals
function func14()
	0001	GETUPVAL 0 0                       ; Player
	0002	EQ 1 0 -1                          ; - 
	0003	JMP 0 3                            ; to pc 7
	0004	GETTABUP 0 0 -2                    ; Player GetAttacked
	0005	GETUPVAL 1 1                       ; Damage
	0006	CALL 0 2 1
	0007	RETURN 0 1

	locals (0)
	upvales (2)
		name: Player; instack: true; index: 12
		name: Damage; instack: true; index: 11
	constants (2)
		id: 0; value: 
		id: 1; value: GetAttacked
end

1 param; 0 varargs; 2 slots; 2 opcodes; 0 constants; 1 upvalue; 0 functions; 1 local
function func15(param1)
	0001	SETUPVAL 0 0                       ; Player
	0002	RETURN 0 1

	locals (1)
		name: player; startpc: 0; endpc: 2
	upvales (1)
		name: Player; instack: true; index: 12
	constants (0)
end

1 param; 0 varargs; 16 slots; 63 opcodes; 26 constants; 3 upvalues; 21 functions; 15 locals
function func16(param1)
	0001	NEWTABLE 1 0 0
	0002	GETTABUP 2 0 -1                    ; Player NextId
	0003	MOVE 3 0
	0004	LOADK 4 -2                         ; 10
	0005	LOADK 5 -3                         ; 1
	0006	LOADK 6 -4                         ; 0
	0007	GETTABUP 7 1 -5                    ; Vector3 New
	0008	LOADK 8 -6                         ; 0
	0009	LOADK 9 -6                         ; 0
	0010	LOADK 10 -6                        ; 0
	0011	CALL 7 4 2
	0012	LOADNIL 8 0
	0013	LOADBOOL 9 0 0
	0014	LOADK 10 -3                        ; 1
	0015	LOADK 11 -2                        ; 10
	0016	LOADBOOL 12 0 0
	0017	LOADBOOL 13 0 0
	0018	CLOSURE 14 0                       ; func17
	0019	SETTABLE 1 -7 14                   ; ReadValue
	0020	CLOSURE 14 1                       ; func18
	0021	SETTABLE 1 -8 14                   ; PrintStatus
	0022	CLOSURE 14 2                       ; func19
	0023	SETTABLE 1 -9 14                   ; GetName
	0024	CLOSURE 14 3                       ; func20
	0025	SETTABLE 1 -10 14                  ; SetPosition
	0026	CLOSURE 14 4                       ; func21
	0027	SETTABLE 1 -11 14                  ; GetX
	0028	CLOSURE 14 5                       ; func22
	0029	SETTABLE 1 -12 14                  ; GetY
	0030	CLOSURE 14 6                       ; func23
	0031	SETTABLE 1 -13 14                  ; GetZ
	0032	CLOSURE 14 7                       ; func24
	0033	CLOSURE 15 8                       ; func25
	0034	SETTABLE 1 -14 15                  ; GainXp
	0035	CLOSURE 15 9                       ; func26
	0036	SETTABLE 1 -15 15                  ; BattleEnemy
	0037	CLOSURE 15 10                      ; func27
	0038	SETTABLE 1 -16 15                  ; IsInBattle
	0039	CLOSURE 15 11                      ; func28
	0040	SETTABLE 1 -17 15                  ; Attack
	0041	CLOSURE 15 12                      ; func29
	0042	SETTABLE 1 -18 15                  ; GetAttacked
	0043	CLOSURE 15 13                      ; func30
	0044	SETTABLE 1 -19 15                  ; IsDead
	0045	CLOSURE 15 14                      ; func31
	0046	SETTABLE 1 -20 15                  ; ExitBattle
	0047	CLOSURE 15 15                      ; func32
	0048	SETTABLE 1 -21 15                  ; Reset
	0049	CLOSURE 15 16                      ; func33
	0050	SETTABLE 1 -22 15                  ; DonateXp
	0051	CLOSURE 15 17                      ; func34
	0052	SETTABLE 1 -23 15                  ; Cheat
	0053	CLOSURE 15 18                      ; func35
	0054	SETTABLE 1 -24 15                  ; IsFlagged
	0055	CLOSURE 15 19                      ; func36
	0056	SETTABLE 1 -25 15                  ; Ban
	0057	CLOSURE 15 20                      ; func37
	0058	SETTABLE 1 -26 15                  ; IsBanned
	0059	GETTABUP 15 0 -1                   ; Player NextId
	0060	ADD 15 15 -3                       ; - 1
	0061	SETTABUP 0 -1 15                   ; Player NextId
	0062	RETURN 1 2
	0063	RETURN 0 1

	locals (15)
		name: name; startpc: 0; endpc: 63
		name: this; startpc: 1; endpc: 63
		name: Id; startpc: 2; endpc: 63
		name: Name; startpc: 3; endpc: 63
		name: Health; startpc: 4; endpc: 63
		name: Level; startpc: 5; endpc: 63
		name: Xp; startpc: 6; endpc: 63
		name: Position; startpc: 11; endpc: 63
		name: Enemy; startpc: 12; endpc: 63
		name: InBattle; startpc: 13; endpc: 63
		name: Damage; startpc: 14; endpc: 63
		name: MaxHealth; startpc: 15; endpc: 63
		name: Flagged; startpc: 16; endpc: 63
		name: Banned; startpc: 17; endpc: 63
		name: AttemptLevelUp; startpc: 32; endpc: 63
	upvales (3)
		name: Player; instack: true; index: 2
		name: Vector3; instack: true; index: 0
		name: _ENV; instack: false; index: 0
	constants (26)
		id: 0; value: NextId
		id: 1; value: 10
		id: 2; value: 1
		id: 3; value: 0
		id: 4; value: New
		id: 5; value: 0
		id: 6; value: ReadValue
		id: 7; value: PrintStatus
		id: 8; value: GetName
		id: 9; value: SetPosition
		id: 10; value: GetX
		id: 11; value: GetY
		id: 12; value: GetZ
		id: 13; value: GainXp
		id: 14; value: BattleEnemy
		id: 15; value: IsInBattle
		id: 16; value: Attack
		id: 17; value: GetAttacked
		id: 18; value: IsDead
		id: 19; value: ExitBattle
		id: 20; value: Reset
		id: 21; value: DonateXp
		id: 22; value: Cheat
		id: 23; value: IsFlagged
		id: 24; value: Ban
		id: 25; value: IsBanned
end

1 param; 0 varargs; 3 slots; 32 opcodes; 14 constants; 12 upvalues; 0 functions; 2 locals
function func17(param1)
	0001	NEWTABLE 1 0 0
	0002	GETUPVAL 2 0                       ; Id
	0003	SETTABLE 1 -1 2                    ; id
	0004	GETUPVAL 2 1                       ; Name
	0005	SETTABLE 1 -2 2                    ; name
	0006	GETUPVAL 2 2                       ; Health
	0007	SETTABLE 1 -3 2                    ; health
	0008	GETUPVAL 2 3                       ; Level
	0009	SETTABLE 1 -4 2                    ; level
	0010	GETTABUP 2 4 -5                    ; Position x
	0011	SETTABLE 1 -5 2                    ; x
	0012	GETTABUP 2 4 -6                    ; Position y
	0013	SETTABLE 1 -6 2                    ; y
	0014	GETTABUP 2 4 -7                    ; Position z
	0015	SETTABLE 1 -7 2                    ; z
	0016	GETUPVAL 2 5                       ; Xp
	0017	SETTABLE 1 -8 2                    ; xp
	0018	GETUPVAL 2 6                       ; Damage
	0019	SETTABLE 1 -9 2                    ; damage
	0020	GETUPVAL 2 7                       ; Enemy
	0021	SETTABLE 1 -10 2                   ; enemy
	0022	GETUPVAL 2 8                       ; MaxHealth
	0023	SETTABLE 1 -11 2                   ; maxhealth
	0024	GETUPVAL 2 9                       ; InBattle
	0025	SETTABLE 1 -12 2                   ; inbattle
	0026	GETUPVAL 2 10                      ; Flagged
	0027	SETTABLE 1 -13 2                   ; flagged
	0028	GETUPVAL 2 11                      ; Banned
	0029	SETTABLE 1 -14 2                   ; banned
	0030	GETTABLE 2 1 0
	0031	RETURN 2 2
	0032	RETURN 0 1

	locals (2)
		name: key; startpc: 0; endpc: 32
		name: data; startpc: 1; endpc: 32
	upvales (12)
		name: Id; instack: true; index: 2
		name: Name; instack: true; index: 3
		name: Health; instack: true; index: 4
		name: Level; instack: true; index: 5
		name: Position; instack: true; index: 7
		name: Xp; instack: true; index: 6
		name: Damage; instack: true; index: 10
		name: Enemy; instack: true; index: 8
		name: MaxHealth; instack: true; index: 11
		name: InBattle; instack: true; index: 9
		name: Flagged; instack: true; index: 12
		name: Banned; instack: true; index: 13
	constants (14)
		id: 0; value: id
		id: 1; value: name
		id: 2; value: health
		id: 3; value: level
		id: 4; value: x
		id: 5; value: y
		id: 6; value: z
		id: 7; value: xp
		id: 8; value: damage
		id: 9; value: enemy
		id: 10; value: maxhealth
		id: 11; value: inbattle
		id: 12; value: flagged
		id: 13; value: banned
end

0 params; 0 varargs; 7 slots; 71 opcodes; 19 constants; 11 upvalues; 0 functions; 1 local
function func18()
	0001	GETTABUP 0 0 -1                    ; _ENV print
	0002	LOADK 1 -2                         ; -----PLAYER INFO-----
	0003	CALL 0 2 1
	0004	GETTABUP 0 0 -1                    ; _ENV print
	0005	LOADK 1 -3                         ; Name: 
	0006	GETUPVAL 2 1                       ; Name
	0007	CONCAT 1 1 2
	0008	CALL 0 2 1
	0009	GETTABUP 0 0 -1                    ; _ENV print
	0010	LOADK 1 -4                         ; Id: 
	0011	GETUPVAL 2 2                       ; Id
	0012	CONCAT 1 1 2
	0013	CALL 0 2 1
	0014	GETTABUP 0 0 -1                    ; _ENV print
	0015	LOADK 1 -5                         ; Health: 
	0016	GETUPVAL 2 3                       ; Health
	0017	CONCAT 1 1 2
	0018	CALL 0 2 1
	0019	GETTABUP 0 0 -1                    ; _ENV print
	0020	LOADK 1 -6                         ; Level: 
	0021	GETUPVAL 2 4                       ; Level
	0022	CONCAT 1 1 2
	0023	CALL 0 2 1
	0024	GETTABUP 0 0 -1                    ; _ENV print
	0025	LOADK 1 -7                         ; XP: 
	0026	GETUPVAL 2 5                       ; Xp
	0027	CONCAT 1 1 2
	0028	CALL 0 2 1
	0029	GETTABUP 0 0 -1                    ; _ENV print
	0030	LOADK 1 -8                         ; Position: 
	0031	GETTABUP 2 6 -9                    ; Position x
	0032	LOADK 3 -10                        ; , 
	0033	GETTABUP 4 6 -11                   ; Position y
	0034	LOADK 5 -10                        ; , 
	0035	GETTABUP 6 6 -12                   ; Position z
	0036	CONCAT 1 1 6
	0037	CALL 0 2 1
	0038	LOADK 0 -13                        ; false
	0039	GETUPVAL 1 7                       ; InBattle
	0040	TEST 1 0
	0041	JMP 0 1                            ; to pc 43
	0042	LOADK 0 -14                        ; true
	0043	GETTABUP 1 0 -1                    ; _ENV print
	0044	LOADK 2 -15                        ; In Battle: 
	0045	MOVE 3 0
	0046	CONCAT 2 2 3
	0047	CALL 1 2 1
	0048	GETTABUP 1 0 -1                    ; _ENV print
	0049	LOADK 2 -16                        ; Attack Damage: 
	0050	GETUPVAL 3 8                       ; Damage
	0051	CONCAT 2 2 3
	0052	CALL 1 2 1
	0053	GETTABUP 1 0 -1                    ; _ENV print
	0054	LOADK 2 -17                        ; Max Health: 
	0055	GETUPVAL 3 9                       ; MaxHealth
	0056	CONCAT 2 2 3
	0057	CALL 1 2 1
	0058	LOADK 0 -13                        ; false
	0059	GETUPVAL 1 10                      ; Banned
	0060	TEST 1 0
	0061	JMP 0 1                            ; to pc 63
	0062	LOADK 0 -14                        ; true
	0063	GETTABUP 1 0 -1                    ; _ENV print
	0064	LOADK 2 -18                        ; Banned: 
	0065	MOVE 3 0
	0066	CONCAT 2 2 3
	0067	CALL 1 2 1
	0068	GETTABUP 1 0 -1                    ; _ENV print
	0069	LOADK 2 -19                        ; ---------------------
	0070	CALL 1 2 1
	0071	RETURN 0 1

	locals (1)
		name: b; startpc: 38; endpc: 71
	upvales (11)
		name: _ENV; instack: false; index: 2
		name: Name; instack: true; index: 3
		name: Id; instack: true; index: 2
		name: Health; instack: true; index: 4
		name: Level; instack: true; index: 5
		name: Xp; instack: true; index: 6
		name: Position; instack: true; index: 7
		name: InBattle; instack: true; index: 9
		name: Damage; instack: true; index: 10
		name: MaxHealth; instack: true; index: 11
		name: Banned; instack: true; index: 13
	constants (19)
		id: 0; value: print
		id: 1; value: -----PLAYER INFO-----
		id: 2; value: Name: 
		id: 3; value: Id: 
		id: 4; value: Health: 
		id: 5; value: Level: 
		id: 6; value: XP: 
		id: 7; value: Position: 
		id: 8; value: x
		id: 9; value: , 
		id: 10; value: y
		id: 11; value: z
		id: 12; value: false
		id: 13; value: true
		id: 14; value: In Battle: 
		id: 15; value: Attack Damage: 
		id: 16; value: Max Health: 
		id: 17; value: Banned: 
		id: 18; value: ---------------------
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func19()
	0001	GETUPVAL 0 0                       ; Name
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Name; instack: true; index: 3
	constants (0)
end

3 params; 0 varargs; 3 slots; 4 opcodes; 3 constants; 1 upvalue; 0 functions; 3 locals
function func20(param1, param2, param3)
	0001	SETTABUP 0 -1 0                    ; Position x
	0002	SETTABUP 0 -2 1                    ; Position y
	0003	SETTABUP 0 -3 2                    ; Position z
	0004	RETURN 0 1

	locals (3)
		name: x; startpc: 0; endpc: 4
		name: y; startpc: 0; endpc: 4
		name: z; startpc: 0; endpc: 4
	upvales (1)
		name: Position; instack: true; index: 7
	constants (3)
		id: 0; value: x
		id: 1; value: y
		id: 2; value: z
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 1 upvalue; 0 functions; 0 locals
function func21()
	0001	GETTABUP 0 0 -1                    ; Position x
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Position; instack: true; index: 7
	constants (1)
		id: 0; value: x
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 1 upvalue; 0 functions; 0 locals
function func22()
	0001	GETTABUP 0 0 -1                    ; Position y
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Position; instack: true; index: 7
	constants (1)
		id: 0; value: y
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 1 upvalue; 0 functions; 0 locals
function func23()
	0001	GETTABUP 0 0 -1                    ; Position z
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Position; instack: true; index: 7
	constants (1)
		id: 0; value: z
end

0 params; 0 varargs; 11 slots; 55 opcodes; 13 constants; 6 upvalues; 0 functions; 2 locals
function func24()
	0001	GETUPVAL 0 0                       ; Level
	0002	EQ 1 0 -1                          ; - 10
	0003	JMP 0 3                            ; to pc 7
	0004	GETUPVAL 0 1                       ; Banned
	0005	TEST 0 0
	0006	JMP 0 1                            ; to pc 8
	0007	RETURN 0 1
	0008	NEWTABLE 0 10 0
	0009	LOADK 1 -2                         ; 0
	0010	LOADK 2 -3                         ; 5
	0011	LOADK 3 -1                         ; 10
	0012	LOADK 4 -4                         ; 20
	0013	LOADK 5 -5                         ; 50
	0014	LOADK 6 -6                         ; 100
	0015	LOADK 7 -7                         ; 150
	0016	LOADK 8 -8                         ; 200
	0017	LOADK 9 -9                         ; 300
	0018	LOADK 10 -10                       ; 500
	0019	SETLIST 0 10 1                     ; 1
	0020	GETUPVAL 1 0                       ; Level
	0021	ADD 1 1 -11                        ; - 1
	0022	GETTABLE 1 0 1
	0023	GETUPVAL 2 2                       ; Xp
	0024	LE 0 1 
	0025	JMP 0 20                           ; to pc 46
	0026	GETUPVAL 2 0                       ; Level
	0027	ADD 2 2 -11                        ; - 1
	0028	SETUPVAL 2 0                       ; Level
	0029	GETUPVAL 2 2                       ; Xp
	0030	SUB 2 2 
	0031	SETUPVAL 2 2                       ; Xp
	0032	GETUPVAL 2 3                       ; MaxHealth
	0033	ADD 2 2 -3                         ; - 5
	0034	SETUPVAL 2 3                       ; MaxHealth
	0035	GETUPVAL 2 3                       ; MaxHealth
	0036	SETUPVAL 2 4                       ; Health
	0037	GETUPVAL 2 5                       ; Damage
	0038	ADD 2 2 -11                        ; - 1
	0039	SETUPVAL 2 5                       ; Damage
	0040	GETUPVAL 2 0                       ; Level
	0041	ADD 2 2 -11                        ; - 1
	0042	GETTABLE 1 0 2
	0043	EQ 1 1 -12                         ; - 
	0044	JMP 0 1                            ; to pc 46
	0045	JMP 0 -23                          ; to pc 23
	0046	GETUPVAL 2 0                       ; Level
	0047	EQ 0 2 -1                          ; - 10
	0048	JMP 0 6                            ; to pc 55
	0049	LOADK 2 -6                         ; 100
	0050	SETUPVAL 2 3                       ; MaxHealth
	0051	GETUPVAL 2 3                       ; MaxHealth
	0052	SETUPVAL 2 4                       ; Health
	0053	LOADK 2 -13                        ; 30
	0054	SETUPVAL 2 5                       ; Damage
	0055	RETURN 0 1

	locals (2)
		name: xpTable; startpc: 19; endpc: 55
		name: neededXp; startpc: 22; endpc: 55
	upvales (6)
		name: Level; instack: true; index: 5
		name: Banned; instack: true; index: 13
		name: Xp; instack: true; index: 6
		name: MaxHealth; instack: true; index: 11
		name: Health; instack: true; index: 4
		name: Damage; instack: true; index: 10
	constants (13)
		id: 0; value: 10
		id: 1; value: 0
		id: 2; value: 5
		id: 3; value: 20
		id: 4; value: 50
		id: 5; value: 100
		id: 6; value: 150
		id: 7; value: 200
		id: 8; value: 300
		id: 9; value: 500
		id: 10; value: 1
		id: 11; value: 
		id: 12; value: 30
end

1 param; 0 varargs; 2 slots; 6 opcodes; 0 constants; 2 upvalues; 0 functions; 1 local
function func25(param1)
	0001	GETUPVAL 1 0                       ; Xp
	0002	ADD 1 1 
	0003	SETUPVAL 1 0                       ; Xp
	0004	GETUPVAL 1 1                       ; AttemptLevelUp
	0005	CALL 1 1 1
	0006	RETURN 0 1

	locals (1)
		name: xp; startpc: 0; endpc: 6
	upvales (2)
		name: Xp; instack: true; index: 6
		name: AttemptLevelUp; instack: true; index: 14
	constants (0)
end

1 param; 0 varargs; 3 slots; 7 opcodes; 1 constant; 3 upvalues; 0 functions; 1 local
function func26(param1)
	0001	SETUPVAL 0 0                       ; Enemy
	0002	GETTABUP 1 0 -1                    ; Enemy SetPlayer
	0003	GETUPVAL 2 1                       ; this
	0004	CALL 1 2 1
	0005	LOADBOOL 1 1 0
	0006	SETUPVAL 1 2                       ; InBattle
	0007	RETURN 0 1

	locals (1)
		name: enemy; startpc: 0; endpc: 7
	upvales (3)
		name: Enemy; instack: true; index: 8
		name: this; instack: true; index: 1
		name: InBattle; instack: true; index: 9
	constants (1)
		id: 0; value: SetPlayer
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func27()
	0001	GETUPVAL 0 0                       ; InBattle
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: InBattle; instack: true; index: 9
	constants (0)
end

0 params; 0 varargs; 2 slots; 15 opcodes; 3 constants; 4 upvalues; 0 functions; 0 locals
function func28()
	0001	GETUPVAL 0 0                       ; Banned
	0002	TEST 0 0
	0003	JMP 0 1                            ; to pc 5
	0004	RETURN 0 1
	0005	GETTABUP 0 1 -1                    ; this IsInBattle
	0006	CALL 0 1 2
	0007	TEST 0 0
	0008	JMP 0 6                            ; to pc 15
	0009	GETUPVAL 0 2                       ; Enemy
	0010	EQ 1 0 -2                          ; - 
	0011	JMP 0 3                            ; to pc 15
	0012	GETTABUP 0 2 -3                    ; Enemy GetAttacked
	0013	GETUPVAL 1 3                       ; Damage
	0014	CALL 0 2 1
	0015	RETURN 0 1

	locals (0)
	upvales (4)
		name: Banned; instack: true; index: 13
		name: this; instack: true; index: 1
		name: Enemy; instack: true; index: 8
		name: Damage; instack: true; index: 10
	constants (3)
		id: 0; value: IsInBattle
		id: 1; value: 
		id: 2; value: GetAttacked
end

1 param; 0 varargs; 2 slots; 8 opcodes; 3 constants; 2 upvalues; 0 functions; 1 local
function func29(param1)
	0001	GETUPVAL 1 0                       ; Health
	0002	SUB 1 1 
	0003	SETUPVAL 1 0                       ; Health
	0004	GETUPVAL 1 0                       ; Health
	0005	LE 0 1 -1                          ; - 0
	0006	JMP 0 1                            ; to pc 8
	0007	SETTABUP 1 -2 -3                   ; _ENV Dead True
	0008	RETURN 0 1

	locals (1)
		name: damage; startpc: 0; endpc: 8
	upvales (2)
		name: Health; instack: true; index: 4
		name: _ENV; instack: false; index: 2
	constants (3)
		id: 0; value: 0
		id: 1; value: Dead
		id: 2; value: True
end

0 params; 0 varargs; 2 slots; 3 opcodes; 1 constant; 1 upvalue; 0 functions; 0 locals
function func30()
	0001	GETTABUP 0 0 -1                    ; _ENV Dead
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: _ENV; instack: false; index: 2
	constants (1)
		id: 0; value: Dead
end

0 params; 0 varargs; 2 slots; 5 opcodes; 0 constants; 2 upvalues; 0 functions; 0 locals
function func31()
	0001	LOADBOOL 0 0 0
	0002	SETUPVAL 0 0                       ; InBattle
	0003	LOADNIL 0 0
	0004	SETUPVAL 0 1                       ; Enemy
	0005	RETURN 0 1

	locals (0)
	upvales (2)
		name: InBattle; instack: true; index: 9
		name: Enemy; instack: true; index: 8
	constants (0)
end

0 params; 0 varargs; 4 slots; 20 opcodes; 8 constants; 9 upvalues; 0 functions; 0 locals
function func32()
	0001	GETTABUP 0 0 -1                    ; this ExitBattle
	0002	CALL 0 1 1
	0003	LOADK 0 -2                         ; 0
	0004	SETUPVAL 0 1                       ; Xp
	0005	LOADK 0 -3                         ; 1
	0006	SETUPVAL 0 2                       ; Level
	0007	LOADK 0 -3                         ; 1
	0008	SETUPVAL 0 3                       ; Damage
	0009	LOADK 0 -4                         ; 10
	0010	SETUPVAL 0 4                       ; MaxHealth
	0011	GETUPVAL 0 4                       ; MaxHealth
	0012	SETUPVAL 0 5                       ; Health
	0013	GETTABUP 0 7 -5                    ; Vector3 New
	0014	LOADK 1 -6                         ; 0
	0015	LOADK 2 -6                         ; 0
	0016	LOADK 3 -6                         ; 0
	0017	CALL 0 4 2
	0018	SETUPVAL 0 6                       ; Position
	0019	SETTABUP 8 -7 -8                   ; _ENV Dead False
	0020	RETURN 0 1

	locals (0)
	upvales (9)
		name: this; instack: true; index: 1
		name: Xp; instack: true; index: 6
		name: Level; instack: true; index: 5
		name: Damage; instack: true; index: 10
		name: MaxHealth; instack: true; index: 11
		name: Health; instack: true; index: 4
		name: Position; instack: true; index: 7
		name: Vector3; instack: false; index: 1
		name: _ENV; instack: false; index: 2
	constants (8)
		id: 0; value: ExitBattle
		id: 1; value: 0
		id: 2; value: 1
		id: 3; value: 10
		id: 4; value: New
		id: 5; value: 0
		id: 6; value: Dead
		id: 7; value: False
end

2 params; 0 varargs; 4 slots; 12 opcodes; 1 constant; 1 upvalue; 0 functions; 2 locals
function func33(param1, param2)
	0001	GETUPVAL 2 0                       ; Xp
	0002	GETUPVAL 3 0                       ; Xp
	0003	SUB 3 3 
	0004	LE 0 3 
	0005	JMP 0 6                            ; to pc 12
	0006	GETUPVAL 2 0                       ; Xp
	0007	SUB 2 2 
	0008	SETUPVAL 2 0                       ; Xp
	0009	GETTABLE 2 0 -1                    ; GainXp
	0010	MOVE 3 1
	0011	CALL 2 2 1
	0012	RETURN 0 1

	locals (2)
		name: player; startpc: 0; endpc: 12
		name: xp; startpc: 0; endpc: 12
	upvales (1)
		name: Xp; instack: true; index: 6
	constants (1)
		id: 0; value: GainXp
end

0 params; 0 varargs; 2 slots; 13 opcodes; 2 constants; 6 upvalues; 0 functions; 0 locals
function func34()
	0001	LOADK 0 -1                         ; 99
	0002	SETUPVAL 0 0                       ; Level
	0003	LOADK 0 -2                         ; 9999
	0004	SETUPVAL 0 1                       ; Xp
	0005	LOADK 0 -1                         ; 99
	0006	SETUPVAL 0 2                       ; Damage
	0007	LOADK 0 -2                         ; 9999
	0008	SETUPVAL 0 3                       ; MaxHealth
	0009	GETUPVAL 0 3                       ; MaxHealth
	0010	SETUPVAL 0 4                       ; Health
	0011	LOADBOOL 0 1 0
	0012	SETUPVAL 0 5                       ; Flagged
	0013	RETURN 0 1

	locals (0)
	upvales (6)
		name: Level; instack: true; index: 5
		name: Xp; instack: true; index: 6
		name: Damage; instack: true; index: 10
		name: MaxHealth; instack: true; index: 11
		name: Health; instack: true; index: 4
		name: Flagged; instack: true; index: 12
	constants (2)
		id: 0; value: 99
		id: 1; value: 9999
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func35()
	0001	GETUPVAL 0 0                       ; Flagged
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Flagged; instack: true; index: 12
	constants (0)
end

0 params; 0 varargs; 2 slots; 5 opcodes; 1 constant; 2 upvalues; 0 functions; 0 locals
function func36()
	0001	LOADBOOL 0 1 0
	0002	SETUPVAL 0 0                       ; Banned
	0003	GETTABUP 0 1 -1                    ; this Reset
	0004	CALL 0 1 1
	0005	RETURN 0 1

	locals (0)
	upvales (2)
		name: Banned; instack: true; index: 13
		name: this; instack: true; index: 1
	constants (1)
		id: 0; value: Reset
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func37()
	0001	GETUPVAL 0 0                       ; Banned
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Banned; instack: true; index: 13
	constants (0)
end

2 params; 0 varargs; 9 slots; 104 opcodes; 22 constants; 1 upvalue; 0 functions; 5 locals
function func38(param1, param2)
	0001	GETTABLE 2 0 -1                    ; IsBanned
	0002	CALL 2 1 2
	0003	TEST 2 0
	0004	JMP 0 2                            ; to pc 7
	0005	LOADBOOL 2 0 0
	0006	RETURN 2 2
	0007	GETTABLE 2 0 -2                    ; GetX
	0008	CALL 2 1 2
	0009	GETTABLE 3 1 -2                    ; GetX
	0010	CALL 3 1 2
	0011	EQ 0 2 
	0012	JMP 0 89                           ; to pc 102
	0013	GETTABLE 2 0 -3                    ; GetY
	0014	CALL 2 1 2
	0015	GETTABLE 3 1 -3                    ; GetY
	0016	CALL 3 1 2
	0017	EQ 0 2 
	0018	JMP 0 83                           ; to pc 102
	0019	GETTABLE 2 0 -4                    ; GetZ
	0020	CALL 2 1 2
	0021	GETTABLE 3 1 -4                    ; GetZ
	0022	CALL 3 1 2
	0023	EQ 0 2 
	0024	JMP 0 77                           ; to pc 102
	0025	GETTABLE 2 0 -5                    ; BattleEnemy
	0026	MOVE 3 1
	0027	CALL 2 2 1
	0028	GETTABLE 2 0 -6                    ; GetName
	0029	CALL 2 1 2
	0030	GETTABLE 3 1 -6                    ; GetName
	0031	CALL 3 1 2
	0032	GETTABUP 4 0 -7                    ; _ENV print
	0033	LOADK 5 -8                         ; Battle started
	0034	CALL 4 2 1
	0035	GETTABUP 4 0 -7                    ; _ENV print
	0036	MOVE 5 2
	0037	LOADK 6 -9                         ;  vs 
	0038	MOVE 7 3
	0039	CONCAT 5 5 7
	0040	CALL 4 2 1
	0041	GETTABUP 4 0 -7                    ; _ENV print
	0042	MOVE 5 2
	0043	LOADK 6 -10                        ;  attacks
	0044	CONCAT 5 5 6
	0045	CALL 4 2 1
	0046	GETTABLE 4 0 -11                   ; Attack
	0047	CALL 4 1 1
	0048	GETTABLE 4 1 -12                   ; IsDead
	0049	CALL 4 1 2
	0050	TEST 4 0
	0051	JMP 0 19                           ; to pc 71
	0052	GETTABUP 4 0 -7                    ; _ENV print
	0053	MOVE 5 3
	0054	LOADK 6 -13                        ;  died
	0055	CONCAT 5 5 6
	0056	CALL 4 2 1
	0057	GETTABLE 4 1 -14                   ; GetXp
	0058	CALL 4 1 2
	0059	GETTABUP 5 0 -7                    ; _ENV print
	0060	LOADK 6 -15                        ; Gained 
	0061	MOVE 7 4
	0062	LOADK 8 -16                        ;  XP
	0063	CONCAT 6 6 8
	0064	CALL 5 2 1
	0065	GETTABLE 5 0 -17                   ; GainXp
	0066	MOVE 6 4
	0067	CALL 5 2 1
	0068	GETTABLE 5 0 -18                   ; ExitBattle
	0069	CALL 5 1 1
	0070	JMP 0 23                           ; to pc 94
	0071	GETTABUP 4 0 -7                    ; _ENV print
	0072	MOVE 5 3
	0073	LOADK 6 -10                        ;  attacks
	0074	CONCAT 5 5 6
	0075	CALL 4 2 1
	0076	GETTABLE 4 1 -11                   ; Attack
	0077	CALL 4 1 1
	0078	GETTABLE 4 0 -12                   ; IsDead
	0079	CALL 4 1 2
	0080	TEST 4 0
	0081	JMP 0 -41                          ; to pc 41
	0082	GETTABUP 4 0 -7                    ; _ENV print
	0083	MOVE 5 2
	0084	LOADK 6 -13                        ;  died
	0085	CONCAT 5 5 6
	0086	CALL 4 2 1
	0087	GETTABUP 4 0 -7                    ; _ENV print
	0088	LOADK 5 -19                        ; Reset to Level 1
	0089	CALL 4 2 1
	0090	GETTABLE 4 0 -20                   ; Reset
	0091	CALL 4 1 1
	0092	JMP 0 1                            ; to pc 94
	0093	JMP 0 -53                          ; to pc 41
	0094	GETTABUP 4 0 -7                    ; _ENV print
	0095	LOADK 5 -21                        ; Battle ended
	0096	CALL 4 2 1
	0097	GETTABUP 4 0 -7                    ; _ENV print
	0098	LOADK 5 -22                        ;  
	0099	CALL 4 2 1
	0100	LOADBOOL 4 1 0
	0101	RETURN 4 2
	0102	LOADBOOL 2 0 0
	0103	RETURN 2 2
	0104	RETURN 0 1

	locals (5)
		name: player; startpc: 0; endpc: 104
		name: enemy; startpc: 0; endpc: 104
		name: pname; startpc: 29; endpc: 101
		name: ename; startpc: 31; endpc: 101
		name: xp; startpc: 58; endpc: 70
	upvales (1)
		name: _ENV; instack: false; index: 0
	constants (22)
		id: 0; value: IsBanned
		id: 1; value: GetX
		id: 2; value: GetY
		id: 3; value: GetZ
		id: 4; value: BattleEnemy
		id: 5; value: GetName
		id: 6; value: print
		id: 7; value: Battle started
		id: 8; value:  vs 
		id: 9; value:  attacks
		id: 10; value: Attack
		id: 11; value: IsDead
		id: 12; value:  died
		id: 13; value: GetXp
		id: 14; value: Gained 
		id: 15; value:  XP
		id: 16; value: GainXp
		id: 17; value: ExitBattle
		id: 18; value: Reset to Level 1
		id: 19; value: Reset
		id: 20; value: Battle ended
		id: 21; value:  
end

0 params; 0 varargs; 2 slots; 7 opcodes; 2 constants; 0 upvalues; 2 functions; 1 local
function func39()
	0001	NEWTABLE 0 0 0
	0002	CLOSURE 1 0                        ; func40
	0003	SETTABLE 0 -1 1                    ; BanPlayer
	0004	CLOSURE 1 1                        ; func41
	0005	SETTABLE 0 -2 1                    ; IsCheater
	0006	RETURN 0 2
	0007	RETURN 0 1

	locals (1)
		name: this; startpc: 1; endpc: 7
	upvales (0)
	constants (2)
		id: 0; value: BanPlayer
		id: 1; value: IsCheater
end

1 param; 0 varargs; 3 slots; 12 opcodes; 2 constants; 1 upvalue; 0 functions; 1 local
function func40(param1)
	0001	GETTABUP 1 0 -1                    ; this IsCheater
	0002	MOVE 2 0
	0003	CALL 1 2 2
	0004	TEST 1 0
	0005	JMP 0 4                            ; to pc 10
	0006	GETTABLE 1 0 -2                    ; Ban
	0007	CALL 1 1 1
	0008	LOADBOOL 1 1 0
	0009	RETURN 1 2
	0010	LOADBOOL 1 0 0
	0011	RETURN 1 2
	0012	RETURN 0 1

	locals (1)
		name: player; startpc: 0; endpc: 12
	upvales (1)
		name: this; instack: true; index: 0
	constants (2)
		id: 0; value: IsCheater
		id: 1; value: Ban
end

1 param; 0 varargs; 2 slots; 4 opcodes; 1 constant; 0 upvalues; 0 functions; 1 local
function func41(param1)
	0001	GETTABLE 1 0 -1                    ; IsFlagged
	0002	TAILCALL 1 1 0
	0003	RETURN 1 0
	0004	RETURN 0 1

	locals (1)
		name: player; startpc: 0; endpc: 4
	upvales (0)
	constants (1)
		id: 0; value: IsFlagged
end

0 params; 0 varargs; 5 slots; 16 opcodes; 5 constants; 3 upvalues; 5 functions; 4 locals
function func42()
	0001	NEWTABLE 0 0 0
	0002	NEWTABLE 1 0 0
	0003	NEWTABLE 2 0 0
	0004	LOADNIL 3 0
	0005	CLOSURE 4 0                        ; func43
	0006	SETTABLE 0 -1 4                    ; GeneratePlayer
	0007	CLOSURE 4 1                        ; func44
	0008	SETTABLE 0 -2 4                    ; GenerateEnemy
	0009	CLOSURE 4 2                        ; func45
	0010	SETTABLE 0 -3 4                    ; GenerateAdmin
	0011	CLOSURE 4 3                        ; func46
	0012	SETTABLE 0 -4 4                    ; GetPlayers
	0013	CLOSURE 4 4                        ; func47
	0014	SETTABLE 0 -5 4                    ; GetEnemies
	0015	RETURN 0 2
	0016	RETURN 0 1

	locals (4)
		name: this; startpc: 1; endpc: 16
		name: Players; startpc: 2; endpc: 16
		name: Enemies; startpc: 3; endpc: 16
		name: Admin; startpc: 4; endpc: 16
	upvales (3)
		name: Player; instack: true; index: 2
		name: Enemy; instack: true; index: 1
		name: Administrator; instack: true; index: 3
	constants (5)
		id: 0; value: GeneratePlayer
		id: 1; value: GenerateEnemy
		id: 2; value: GenerateAdmin
		id: 3; value: GetPlayers
		id: 4; value: GetEnemies
end

1 param; 0 varargs; 3 slots; 8 opcodes; 3 constants; 2 upvalues; 0 functions; 2 locals
function func43(param1)
	0001	GETTABUP 1 0 -1                    ; Player New
	0002	MOVE 2 0
	0003	CALL 1 2 2
	0004	GETTABUP 2 0 -2                    ; Player NextId
	0005	SUB 2 2 -3                         ; - 1
	0006	SETTABUP 1 2 1                     ; Players
	0007	RETURN 1 2
	0008	RETURN 0 1

	locals (2)
		name: name; startpc: 0; endpc: 8
		name: player; startpc: 3; endpc: 8
	upvales (2)
		name: Player; instack: false; index: 0
		name: Players; instack: true; index: 1
	constants (3)
		id: 0; value: New
		id: 1; value: NextId
		id: 2; value: 1
end

4 params; 0 varargs; 9 slots; 11 opcodes; 3 constants; 2 upvalues; 0 functions; 5 locals
function func44(param1, param2, param3, param4)
	0001	GETTABUP 4 0 -1                    ; Enemy New
	0002	MOVE 5 0
	0003	MOVE 6 1
	0004	MOVE 7 2
	0005	MOVE 8 3
	0006	CALL 4 5 2
	0007	GETTABUP 5 0 -2                    ; Enemy NextId
	0008	SUB 5 5 -3                         ; - 1
	0009	SETTABUP 1 5 4                     ; Enemies
	0010	RETURN 4 2
	0011	RETURN 0 1

	locals (5)
		name: name; startpc: 0; endpc: 11
		name: health; startpc: 0; endpc: 11
		name: xp; startpc: 0; endpc: 11
		name: damage; startpc: 0; endpc: 11
		name: enemy; startpc: 6; endpc: 11
	upvales (2)
		name: Enemy; instack: false; index: 1
		name: Enemies; instack: true; index: 2
	constants (3)
		id: 0; value: New
		id: 1; value: NextId
		id: 2; value: 1
end

0 params; 0 varargs; 2 slots; 11 opcodes; 2 constants; 2 upvalues; 0 functions; 0 locals
function func45()
	0001	GETUPVAL 0 0                       ; Admin
	0002	EQ 1 0 -1                          ; - 
	0003	JMP 0 2                            ; to pc 6
	0004	GETUPVAL 0 0                       ; Admin
	0005	RETURN 0 2
	0006	GETTABUP 0 1 -2                    ; Administrator New
	0007	CALL 0 1 2
	0008	SETUPVAL 0 0                       ; Admin
	0009	GETUPVAL 0 0                       ; Admin
	0010	RETURN 0 2
	0011	RETURN 0 1

	locals (0)
	upvales (2)
		name: Admin; instack: true; index: 3
		name: Administrator; instack: false; index: 2
	constants (2)
		id: 0; value: 
		id: 1; value: New
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func46()
	0001	GETUPVAL 0 0                       ; Players
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Players; instack: true; index: 1
	constants (0)
end

0 params; 0 varargs; 2 slots; 3 opcodes; 0 constants; 1 upvalue; 0 functions; 0 locals
function func47()
	0001	GETUPVAL 0 0                       ; Enemies
	0002	RETURN 0 2
	0003	RETURN 0 1

	locals (0)
	upvales (1)
		name: Enemies; instack: true; index: 2
	constants (0)
end
```
