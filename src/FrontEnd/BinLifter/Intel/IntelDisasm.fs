(*
  B2R2 - the Next-Generation Reversing Platform

  Copyright (c) SoftSec Lab. @ KAIST, since 2016

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*)

module internal B2R2.FrontEnd.BinLifter.Intel.Disasm

open B2R2
open B2R2.FrontEnd.BinLifter

let opCodeToString = function
  | Opcode.AAA -> "aaa"
  | Opcode.AAD -> "aad"
  | Opcode.AAM -> "aam"
  | Opcode.AAS -> "aas"
  | Opcode.ADC -> "adc"
  | Opcode.ADD -> "add"
  | Opcode.ADDPD -> "addpd"
  | Opcode.ADDPS -> "addps"
  | Opcode.ADDSD -> "addsd"
  | Opcode.ADDSS -> "addss"
  | Opcode.AND -> "and"
  | Opcode.ANDN -> "andn"
  | Opcode.ANDNPD -> "andnpd"
  | Opcode.ANDNPS -> "andnps"
  | Opcode.ANDPD -> "andpd"
  | Opcode.ANDPS -> "andps"
  | Opcode.ARPL -> "arpl"
  | Opcode.BLENDVPD -> "blendvpd"
  | Opcode.BNDCL -> "bndcl"
  | Opcode.BNDMK -> "bndmk"
  | Opcode.BNDMOV -> "bndmov"
  | Opcode.BNDSTX -> "bndstx"
  | Opcode.BOUND -> "bound"
  | Opcode.BSF -> "bsf"
  | Opcode.BSR -> "bsr"
  | Opcode.BSWAP -> "bswap"
  | Opcode.BT -> "bt"
  | Opcode.BTC -> "btc"
  | Opcode.BTR -> "btr"
  | Opcode.BTS -> "bts"
  | Opcode.CALLFar | Opcode.CALLNear -> "call"
  | Opcode.CBW -> "cbw"
  | Opcode.CDQ -> "cdq"
  | Opcode.CDQE -> "cdqe"
  | Opcode.CLAC -> "clac"
  | Opcode.CLC -> "clc"
  | Opcode.CLD -> "cld"
  | Opcode.CLFLUSH -> "clflush"
  | Opcode.CLI -> "cli"
  | Opcode.CLRSSBSY -> "clrssbsy"
  | Opcode.CLTS -> "clts"
  | Opcode.CMC -> "cmc"
  | Opcode.CMOVA -> "cmova"
  | Opcode.CMOVAE -> "cmovae"
  | Opcode.CMOVB -> "cmovb"
  | Opcode.CMOVBE -> "cmovbe"
  | Opcode.CMOVG -> "cmovg"
  | Opcode.CMOVGE -> "cmovge"
  | Opcode.CMOVL -> "cmovl"
  | Opcode.CMOVLE -> "cmovle"
  | Opcode.CMOVNO -> "cmovno"
  | Opcode.CMOVNP -> "cmovnp"
  | Opcode.CMOVNS -> "cmovns"
  | Opcode.CMOVNZ -> "cmovnz"
  | Opcode.CMOVO -> "cmovo"
  | Opcode.CMOVP -> "cmovp"
  | Opcode.CMOVS -> "cmovs"
  | Opcode.CMOVZ -> "cmovz"
  | Opcode.CMP -> "cmp"
  | Opcode.CMPPD -> "cmppd"
  | Opcode.CMPPS -> "cmpps"
  | Opcode.CMPSB -> "cmpsb"
  | Opcode.CMPSD -> "cmpsd"
  | Opcode.CMPSQ -> "cmpsq"
  | Opcode.CMPSS -> "cmpss"
  | Opcode.CMPSW -> "cmpsw"
  | Opcode.CMPXCHG -> "cmpxchg"
  | Opcode.CMPXCHG16B -> "cmpxchg16b"
  | Opcode.CMPXCHG8B -> "cmpxchg8b"
  | Opcode.COMISD -> "comisd"
  | Opcode.COMISS -> "comiss"
  | Opcode.CPUID -> "cpuid"
  | Opcode.CQO -> "cqo"
  | Opcode.CRC32 -> "crc32"
  | Opcode.CVTDQ2PD -> "cvtdq2pd"
  | Opcode.CVTDQ2PS -> "cvtdq2ps"
  | Opcode.CVTPD2DQ -> "cvtpd2dq"
  | Opcode.CVTPD2PI -> "cvtpd2pi"
  | Opcode.CVTPD2PS -> "cvtpd2ps"
  | Opcode.CVTPI2PD -> "cvtpi2pd"
  | Opcode.CVTPI2PS -> "cvtpi2ps"
  | Opcode.CVTPS2DQ -> "cvtps2dq"
  | Opcode.CVTPS2PD -> "cvtps2pd"
  | Opcode.CVTPS2PI -> "cvtps2pi"
  | Opcode.CVTSD2SI -> "cvtsd2si"
  | Opcode.CVTSD2SS -> "cvtsd2ss"
  | Opcode.CVTSI2SD -> "cvtsi2sd"
  | Opcode.CVTSI2SS -> "cvtsi2ss"
  | Opcode.CVTSS2SD -> "cvtss2sd"
  | Opcode.CVTSS2SI -> "cvtss2si"
  | Opcode.CVTTPD2DQ -> "cvttpd2dq"
  | Opcode.CVTTPD2PI -> "cvttpd2pi"
  | Opcode.CVTTPS2DQ -> "cvttps2dq"
  | Opcode.CVTTPS2PI -> "cvttps2pi"
  | Opcode.CVTTSD2SI -> "cvttsd2si"
  | Opcode.CVTTSS2SI -> "cvttss2si"
  | Opcode.CWD -> "cwd"
  | Opcode.CWDE -> "cwde"
  | Opcode.DAA -> "daa"
  | Opcode.DAS -> "das"
  | Opcode.DEC -> "dec"
  | Opcode.DIV -> "div"
  | Opcode.DIVPD -> "divpd"
  | Opcode.DIVPS -> "divps"
  | Opcode.DIVSD -> "divsd"
  | Opcode.DIVSS -> "divss"
  | Opcode.EMMS -> "emms"
  | Opcode.ENDBR32 -> "endbr32"
  | Opcode.ENDBR64 -> "endbr64"
  | Opcode.ENTER -> "enter"
  | Opcode.F2XM1 -> "f2xm1"
  | Opcode.FABS -> "fabs"
  | Opcode.FADD -> "fadd"
  | Opcode.FADDP -> "faddp"
  | Opcode.FBLD -> "fbld"
  | Opcode.FBSTP -> "fbstp"
  | Opcode.FCHS -> "fchs"
  | Opcode.FCLEX -> "fclex"
  | Opcode.FCMOVB -> "fcmovb"
  | Opcode.FCMOVBE -> "fcmovbe"
  | Opcode.FCMOVE -> "fcmove"
  | Opcode.FCMOVNB -> "fcmovnb"
  | Opcode.FCMOVNBE -> "fcmovnbe"
  | Opcode.FCMOVNE -> "fcmovne"
  | Opcode.FCMOVNU -> "fcmovnu"
  | Opcode.FCMOVU -> "fcmovu"
  | Opcode.FCOM -> "fcom"
  | Opcode.FCOMI -> "fcomi"
  | Opcode.FCOMIP -> "fcomip"
  | Opcode.FCOMP -> "fcomp"
  | Opcode.FCOMPP -> "fcompp"
  | Opcode.FCOS -> "fcos"
  | Opcode.FDECSTP -> "fdecstp"
  | Opcode.FDIV -> "fdiv"
  | Opcode.FDIVP -> "fdivp"
  | Opcode.FDIVR -> "fdivr"
  | Opcode.FDIVRP -> "fdivrp"
  | Opcode.FFREE -> "ffree"
  | Opcode.FIADD -> "fiadd"
  | Opcode.FICOM -> "ficom"
  | Opcode.FICOMP -> "ficomp"
  | Opcode.FIDIV -> "fidiv"
  | Opcode.FIDIVR -> "fidivr"
  | Opcode.FILD -> "fild"
  | Opcode.FIMUL -> "fimul"
  | Opcode.FINCSTP -> "fincstp"
  | Opcode.FINIT -> "finit"
  | Opcode.FIST -> "fist"
  | Opcode.FISTP -> "fistp"
  | Opcode.FISTTP -> "fisttp"
  | Opcode.FISUB -> "fisub"
  | Opcode.FISUBR -> "fisubr"
  | Opcode.FLD -> "fld"
  | Opcode.FLD1 -> "fld1"
  | Opcode.FLDCW -> "fldcw"
  | Opcode.FLDENV -> "fldenv"
  | Opcode.FLDL2E -> "fldl2e"
  | Opcode.FLDL2T -> "fldl2t"
  | Opcode.FLDLG2 -> "fldlg2"
  | Opcode.FLDLN2 -> "fldln2"
  | Opcode.FLDPI -> "fldpi"
  | Opcode.FLDZ -> "fldz"
  | Opcode.FMUL -> "fmul"
  | Opcode.FMULP -> "fmulp"
  | Opcode.FNOP -> "fnop"
  | Opcode.FNSTCW -> "fnstcw"
  | Opcode.FNSTSW -> "fnstsw"
  | Opcode.FPATAN -> "fpatan"
  | Opcode.FPREM -> "fprem"
  | Opcode.FPREM1 -> "fprem1"
  | Opcode.FPTAN -> "fptan"
  | Opcode.FRNDINT -> "frndint"
  | Opcode.FRSTOR -> "frstor"
  | Opcode.FSAVE -> "fsave"
  | Opcode.FSCALE -> "fscale"
  | Opcode.FSIN -> "fsin"
  | Opcode.FSINCOS -> "fsincos"
  | Opcode.FSQRT -> "fsqrt"
  | Opcode.FST -> "fst"
  | Opcode.FSTENV -> "fstenv"
  | Opcode.FSTP -> "fstp"
  | Opcode.FSTSW -> "fstsw"
  | Opcode.FSUB -> "fsub"
  | Opcode.FSUBP -> "fsubp"
  | Opcode.FSUBR -> "fsubr"
  | Opcode.FSUBRP -> "fsubrp"
  | Opcode.FTST -> "ftst"
  | Opcode.FUCOM -> "fucom"
  | Opcode.FUCOMI -> "fucomi"
  | Opcode.FUCOMIP -> "fucomip"
  | Opcode.FUCOMP -> "fucomp"
  | Opcode.FUCOMPP -> "fucompp"
  | Opcode.FXAM -> "fxam"
  | Opcode.FXCH -> "fxch"
  | Opcode.FXRSTOR -> "fxrstor"
  | Opcode.FXRSTOR64 -> "fxrstor64"
  | Opcode.FXSAVE -> "fxsave"
  | Opcode.FXSAVE64 -> "fxsave64"
  | Opcode.FXTRACT -> "fxtract"
  | Opcode.FYL2X -> "fyl2x"
  | Opcode.FYL2XP1 -> "fyl2xp1"
  | Opcode.GETSEC -> "getsec"
  | Opcode.HLT -> "hlt"
  | Opcode.IDIV -> "idiv"
  | Opcode.IMUL -> "imul"
  | Opcode.IN -> "in"
  | Opcode.INC -> "inc"
  | Opcode.INCSSPD -> "incsspd"
  | Opcode.INCSSPQ -> "incsspq"
  | Opcode.INS -> "ins"
  | Opcode.INSB -> "insb"
  | Opcode.INSD -> "insd"
  | Opcode.INSW -> "insw"
  | Opcode.INT -> "int"
  | Opcode.INT3 -> "int3"
  | Opcode.INTO -> "into"
  | Opcode.INVD -> "invd"
  | Opcode.INVLPG -> "invlpg"
  | Opcode.IRETD -> "iretd"
  | Opcode.IRETQ -> "iretq"
  | Opcode.IRETW -> "iretw"
  | Opcode.JA -> "ja"
  | Opcode.JB -> "jb"
  | Opcode.JBE -> "jbe"
  | Opcode.JCXZ -> "jcxz"
  | Opcode.JECXZ -> "jecxz"
  | Opcode.JG -> "jg"
  | Opcode.JL -> "jl"
  | Opcode.JLE -> "jle"
  | Opcode.JMPFar | Opcode.JMPNear -> "jmp"
  | Opcode.JNB -> "jnb"
  | Opcode.JNL -> "jnl"
  | Opcode.JNO -> "jno"
  | Opcode.JNP -> "jnp"
  | Opcode.JNS -> "jns"
  | Opcode.JNZ -> "jnz"
  | Opcode.JO -> "jo"
  | Opcode.JP -> "jp"
  | Opcode.JRCXZ -> "jrcxz"
  | Opcode.JS -> "js"
  | Opcode.JZ -> "jz"
  | Opcode.LAHF -> "lahf"
  | Opcode.LAR -> "lar"
  | Opcode.LDDQU -> "lddqu"
  | Opcode.LDMXCSR -> "ldmxcsr"
  | Opcode.LDS -> "lds"
  | Opcode.LEA -> "lea"
  | Opcode.LEAVE -> "leave"
  | Opcode.LES -> "les"
  | Opcode.LFENCE -> "lfence"
  | Opcode.LFS -> "lfs"
  | Opcode.LGDT -> "lgdt"
  | Opcode.LGS -> "lgs"
  | Opcode.LIDT -> "lidt"
  | Opcode.LLDT -> "lldt"
  | Opcode.LMSW -> "lmsw"
  | Opcode.LODSB -> "lodsb"
  | Opcode.LODSD -> "lodsd"
  | Opcode.LODSQ -> "lodsq"
  | Opcode.LODSW -> "lodsw"
  | Opcode.LOOP -> "loop"
  | Opcode.LOOPE -> "loope"
  | Opcode.LOOPNE -> "loopne"
  | Opcode.LSL -> "lsl"
  | Opcode.LSS -> "lss"
  | Opcode.LTR -> "ltr"
  | Opcode.LZCNT -> "lzcnt"
  | Opcode.MAXPD -> "maxpd"
  | Opcode.MAXPS -> "maxps"
  | Opcode.MAXSD -> "maxsd"
  | Opcode.MAXSS -> "maxss"
  | Opcode.MFENCE -> "mfence"
  | Opcode.MINPD -> "minpd"
  | Opcode.MINPS -> "minps"
  | Opcode.MINSD -> "minsd"
  | Opcode.MINSS -> "minss"
  | Opcode.MONITOR -> "monitor"
  | Opcode.MOV -> "mov"
  | Opcode.MOVAPD -> "movapd"
  | Opcode.MOVAPS -> "movaps"
  | Opcode.MOVBE -> "movbe"
  | Opcode.MOVD -> "movd"
  | Opcode.MOVDDUP -> "movddup"
  | Opcode.MOVDQ2Q -> "movdq2q"
  | Opcode.MOVDQA -> "movdqa"
  | Opcode.MOVDQU -> "movdqu"
  | Opcode.MOVHLPS -> "movhlps"
  | Opcode.MOVHPD -> "movhpd"
  | Opcode.MOVHPS -> "movhps"
  | Opcode.MOVLHPS -> "movlhps"
  | Opcode.MOVLPD -> "movlpd"
  | Opcode.MOVLPS -> "movlps"
  | Opcode.MOVMSKPD -> "movmskpd"
  | Opcode.MOVMSKPS -> "movmskps"
  | Opcode.MOVNTDQ -> "movntdq"
  | Opcode.MOVNTI -> "movnti"
  | Opcode.MOVNTPD -> "movntpd"
  | Opcode.MOVNTPS -> "movntps"
  | Opcode.MOVNTQ -> "movntq"
  | Opcode.MOVQ -> "movq"
  | Opcode.MOVQ2DQ -> "movq2dq"
  | Opcode.MOVSB -> "movsb"
  | Opcode.MOVSD -> "movsd"
  | Opcode.MOVSHDUP -> "movshdup"
  | Opcode.MOVSLDUP -> "movsldup"
  | Opcode.MOVSQ -> "movsq"
  | Opcode.MOVSS -> "movss"
  | Opcode.MOVSW -> "movsw"
  | Opcode.MOVSX -> "movsx"
  | Opcode.MOVSXD -> "movsxd"
  | Opcode.MOVUPD -> "movupd"
  | Opcode.MOVUPS -> "movups"
  | Opcode.MOVZX -> "movzx"
  | Opcode.MUL -> "mul"
  | Opcode.MULPD -> "mulpd"
  | Opcode.MULPS -> "mulps"
  | Opcode.MULSD -> "mulsd"
  | Opcode.MULSS -> "mulss"
  | Opcode.MULX -> "mulx"
  | Opcode.MWAIT -> "mwait"
  | Opcode.NEG -> "neg"
  | Opcode.NOP -> "nop"
  | Opcode.NOT -> "not"
  | Opcode.OR -> "or"
  | Opcode.ORPD -> "orpd"
  | Opcode.ORPS -> "orps"
  | Opcode.OUT -> "out"
  | Opcode.OUTS -> "outs"
  | Opcode.OUTSB -> "outsb"
  | Opcode.OUTSD -> "outsd"
  | Opcode.OUTSW -> "outsw"
  | Opcode.PABSB -> "pabsb"
  | Opcode.PABSD -> "pabsd"
  | Opcode.PABSW -> "pabsw"
  | Opcode.PACKSSDW -> "packssdw"
  | Opcode.PACKSSWB -> "packsswb"
  | Opcode.PACKUSDW -> "packusdw"
  | Opcode.PACKUSWB -> "packuswb"
  | Opcode.PADDB -> "paddb"
  | Opcode.PADDD -> "paddd"
  | Opcode.PADDQ -> "paddq"
  | Opcode.PADDSB -> "paddsb"
  | Opcode.PADDSW -> "paddsw"
  | Opcode.PADDUSB -> "paddusb"
  | Opcode.PADDUSW -> "paddusw"
  | Opcode.PADDW -> "paddw"
  | Opcode.PALIGNR -> "palignr"
  | Opcode.PAND -> "pand"
  | Opcode.PANDN -> "pandn"
  | Opcode.PAUSE -> "pause"
  | Opcode.PAVGB -> "pavgb"
  | Opcode.PAVGW -> "pavgw"
  | Opcode.PCMPEQB -> "pcmpeqb"
  | Opcode.PCMPEQD -> "pcmpeqd"
  | Opcode.PCMPEQQ -> "pcmpeqq"
  | Opcode.PCMPEQW -> "pcmpeqw"
  | Opcode.PCMPESTRI -> "pcmpestri"
  | Opcode.PCMPESTRM -> "pcmpestrm"
  | Opcode.PCMPGTB -> "pcmpgtb"
  | Opcode.PCMPGTD -> "pcmpgtd"
  | Opcode.PCMPGTQ -> "pcmpgtq"
  | Opcode.PCMPGTW -> "pcmpgtw"
  | Opcode.PCMPISTRI -> "pcmpistri"
  | Opcode.PCMPISTRM -> "pcmpistrm"
  | Opcode.PEXT -> "pext"
  | Opcode.PEXTRD -> "pextrd"
  | Opcode.PEXTRW -> "pextrw"
  | Opcode.PHADDD -> "phaddd"
  | Opcode.PHADDSW -> "phaddsw"
  | Opcode.PHADDW -> "phaddw"
  | Opcode.PHMINPOSUW -> "phminposuw"
  | Opcode.PHSUBD -> "phsubd"
  | Opcode.PHSUBSW -> "phsubsw"
  | Opcode.PHSUBW -> "phsubw"
  | Opcode.PINSRB -> "pinsrb"
  | Opcode.PINSRW -> "pinsrw"
  | Opcode.PMADDWD -> "pmaddwd"
  | Opcode.PMAXSB -> "pmaxsb"
  | Opcode.PMAXSD -> "pmaxsd"
  | Opcode.PMAXSW -> "pmaxsw"
  | Opcode.PMAXUB -> "pmaxub"
  | Opcode.PMAXUD -> "pmaxud"
  | Opcode.PMAXUW -> "pmaxuw"
  | Opcode.PMINSB -> "pminsb"
  | Opcode.PMINSD -> "pminsd"
  | Opcode.PMINSW -> "pminsw"
  | Opcode.PMINUB -> "pminub"
  | Opcode.PMINUD -> "pminud"
  | Opcode.PMINUW -> "pminuw"
  | Opcode.PMOVMSKB -> "pmovmskb"
  | Opcode.PMOVSXBD -> "pmovsxbd"
  | Opcode.PMOVSXBQ -> "pmovsxbq"
  | Opcode.PMOVSXBW -> "pmovsxbw"
  | Opcode.PMOVSXDQ -> "pmovsxdq"
  | Opcode.PMOVSXWD -> "pmovsxwd"
  | Opcode.PMOVSXWQ -> "pmovsxwq"
  | Opcode.PMOVZXBD -> "pmovzxbd"
  | Opcode.PMOVZXBQ -> "pmovzxbq"
  | Opcode.PMOVZXBW -> "pmovzxbw"
  | Opcode.PMOVZXDQ -> "pmovzxdq"
  | Opcode.PMOVZXWD -> "pmovzxwd"
  | Opcode.PMOVZXWQ -> "pmovzxwq"
  | Opcode.PMULDQ -> "pmuldq"
  | Opcode.PMULHRSW -> "pmulhrsw"
  | Opcode.PMULHUW -> "pmulhuw"
  | Opcode.PMULHW -> "pmulhw"
  | Opcode.PMULLD -> "pmulld"
  | Opcode.PMULLW -> "pmullw"
  | Opcode.PMULUDQ -> "pmuludq"
  | Opcode.POP -> "pop"
  | Opcode.POPA -> "popa"
  | Opcode.POPAD -> "popad"
  | Opcode.POPCNT -> "popcnt"
  | Opcode.POPF -> "popf"
  | Opcode.POPFD -> "popfd"
  | Opcode.POPFQ -> "popfq"
  | Opcode.POR -> "por"
  | Opcode.PREFETCHNTA -> "prefetchnta"
  | Opcode.PREFETCHT0 -> "prefetcht0"
  | Opcode.PREFETCHT1 -> "prefetcht1"
  | Opcode.PREFETCHT2 -> "prefetcht2"
  | Opcode.PREFETCHW -> "prefetchw"
  | Opcode.PREFETCHWT1 -> "prefetchwt1"
  | Opcode.PSADBW -> "psadbw"
  | Opcode.PSHUFB -> "pshufb"
  | Opcode.PSHUFD -> "pshufd"
  | Opcode.PSHUFHW -> "pshufhw"
  | Opcode.PSHUFLW -> "pshuflw"
  | Opcode.PSHUFW -> "pshufw"
  | Opcode.PSIGNB -> "psignb"
  | Opcode.PSIGND -> "psignd"
  | Opcode.PSIGNW -> "psignw"
  | Opcode.PSLLD -> "pslld"
  | Opcode.PSLLDQ -> "pslldq"
  | Opcode.PSLLQ -> "psllq"
  | Opcode.PSLLW -> "psllw"
  | Opcode.PSRAD -> "psrad"
  | Opcode.PSRAW -> "psraw"
  | Opcode.PSRLD -> "psrld"
  | Opcode.PSRLDQ -> "psrldq"
  | Opcode.PSRLQ -> "psrlq"
  | Opcode.PSRLW -> "psrlw"
  | Opcode.PSUBB -> "psubb"
  | Opcode.PSUBD -> "psubd"
  | Opcode.PSUBQ -> "psubq"
  | Opcode.PSUBSB -> "psubsb"
  | Opcode.PSUBSW -> "psubsw"
  | Opcode.PSUBUSB -> "psubusb"
  | Opcode.PSUBUSW -> "psubusw"
  | Opcode.PSUBW -> "psubw"
  | Opcode.PTEST -> "ptest"
  | Opcode.PUNPCKHBW -> "punpckhbw"
  | Opcode.PUNPCKHDQ -> "punpckhdq"
  | Opcode.PUNPCKHQDQ -> "punpckhqdq"
  | Opcode.PUNPCKHWD -> "punpckhwd"
  | Opcode.PUNPCKLBW -> "punpcklbw"
  | Opcode.PUNPCKLDQ -> "punpckldq"
  | Opcode.PUNPCKLQDQ -> "punpcklqdq"
  | Opcode.PUNPCKLWD -> "punpcklwd"
  | Opcode.PUSH -> "push"
  | Opcode.PUSHA -> "pusha"
  | Opcode.PUSHAD -> "pushad"
  | Opcode.PUSHF -> "pushf"
  | Opcode.PUSHFD -> "pushfd"
  | Opcode.PUSHFQ -> "pushfq"
  | Opcode.PXOR -> "pxor"
  | Opcode.RCL -> "rcl"
  | Opcode.RCPPS -> "rcpps"
  | Opcode.RCPSS -> "rcpss"
  | Opcode.RCR -> "rcr"
  | Opcode.RDFSBASE -> "rdfsbase"
  | Opcode.RDGSBASE -> "rdgsbase"
  | Opcode.RDMSR -> "rdmsr"
  | Opcode.RDPKRU -> "rdpkru"
  | Opcode.RDPMC -> "rdpmc"
  | Opcode.RDRAND -> "rdrand"
  | Opcode.RDSEED -> "rdseed"
  | Opcode.RDSSPD -> "rdsspd"
  | Opcode.RDSSPQ -> "rdsspq"
  | Opcode.RDTSC -> "rdtsc"
  | Opcode.RDTSCP -> "rdtscp"
  | Opcode.RETFar | Opcode.RETFarImm
  | Opcode.RETNear | Opcode.RETNearImm -> "ret"
  | Opcode.ROL -> "rol"
  | Opcode.ROR -> "ror"
  | Opcode.RORX -> "rorx"
  | Opcode.ROUNDPD -> "roundpd"
  | Opcode.ROUNDPS -> "roundps"
  | Opcode.ROUNDSD -> "roundsd"
  | Opcode.RSM -> "rsm"
  | Opcode.RSQRTPS -> "rsqrtps"
  | Opcode.RSQRTSS -> "rsqrtss"
  | Opcode.RSTORSSP -> "rstorssp"
  | Opcode.SAHF -> "sahf"
  | Opcode.SAR -> "sar"
  | Opcode.SARX -> "sarx"
  | Opcode.SAVEPREVSSP -> "saveprevssp"
  | Opcode.SBB -> "sbb"
  | Opcode.SCASB -> "scasb"
  | Opcode.SCASD -> "scasd"
  | Opcode.SCASQ -> "scasq"
  | Opcode.SCASW -> "scasw"
  | Opcode.SETA -> "seta"
  | Opcode.SETB -> "setb"
  | Opcode.SETBE -> "setbe"
  | Opcode.SETG -> "setg"
  | Opcode.SETL -> "setl"
  | Opcode.SETLE -> "setle"
  | Opcode.SETNB -> "setnb"
  | Opcode.SETNL -> "setnl"
  | Opcode.SETNO -> "setno"
  | Opcode.SETNP -> "setnp"
  | Opcode.SETNS -> "setns"
  | Opcode.SETNZ -> "setnz"
  | Opcode.SETO -> "seto"
  | Opcode.SETP -> "setp"
  | Opcode.SETS -> "sets"
  | Opcode.SETSSBSY -> "setssbsy"
  | Opcode.SETZ -> "setz"
  | Opcode.SFENCE -> "sfence"
  | Opcode.SGDT -> "sgdt"
  | Opcode.SHL -> "shl"
  | Opcode.SHLD -> "shld"
  | Opcode.SHLX -> "shlx"
  | Opcode.SHR -> "shr"
  | Opcode.SHRD -> "shrd"
  | Opcode.SHRX -> "shrx"
  | Opcode.SHUFPD -> "shufpd"
  | Opcode.SHUFPS -> "shufps"
  | Opcode.SIDT -> "sidt"
  | Opcode.SLDT -> "sldt"
  | Opcode.SMSW -> "smsw"
  | Opcode.SQRTPD -> "sqrtpd"
  | Opcode.SQRTPS -> "sqrtps"
  | Opcode.SQRTSD -> "sqrtsd"
  | Opcode.SQRTSS -> "sqrtss"
  | Opcode.STAC -> "stac"
  | Opcode.STC -> "stc"
  | Opcode.STD -> "std"
  | Opcode.STI -> "sti"
  | Opcode.STMXCSR -> "stmxcsr"
  | Opcode.STOSB -> "stosb"
  | Opcode.STOSD -> "stosd"
  | Opcode.STOSQ -> "stosq"
  | Opcode.STOSW -> "stosw"
  | Opcode.STR -> "str"
  | Opcode.SUB -> "sub"
  | Opcode.SUBPD -> "subpd"
  | Opcode.SUBPS -> "subps"
  | Opcode.SUBSD -> "subsd"
  | Opcode.SUBSS -> "subss"
  | Opcode.SWAPGS -> "swapgs"
  | Opcode.SYSCALL -> "syscall"
  | Opcode.SYSENTER -> "sysenter"
  | Opcode.SYSEXIT -> "sysexit"
  | Opcode.SYSRET -> "sysret"
  | Opcode.TEST -> "test"
  | Opcode.TZCNT -> "tzcnt"
  | Opcode.UCOMISD -> "ucomisd"
  | Opcode.UCOMISS -> "ucomiss"
  | Opcode.UD2 -> "ud2"
  | Opcode.UNPCKHPD -> "unpckhpd"
  | Opcode.UNPCKHPS -> "unpckhps"
  | Opcode.UNPCKLPD -> "unpcklpd"
  | Opcode.UNPCKLPS -> "unpcklps"
  | Opcode.VADDPD -> "vaddpd"
  | Opcode.VADDPS -> "vaddps"
  | Opcode.VADDSD -> "vaddsd"
  | Opcode.VADDSS -> "vaddss"
  | Opcode.VANDNPD -> "vandnpd"
  | Opcode.VANDNPS -> "vandnps"
  | Opcode.VANDPD -> "vandpd"
  | Opcode.VANDPS -> "vandps"
  | Opcode.VBLENDVPD -> "vblendvpd"
  | Opcode.VBROADCASTI128 -> "vbroadcasti128"
  | Opcode.VBROADCASTSD -> "vbroadcastsd"
  | Opcode.VBROADCASTSS -> "vbroadcastss"
  | Opcode.VCMPPD -> "vcmppd"
  | Opcode.VCMPPS -> "vcmpps"
  | Opcode.VCMPSD -> "vcmpsd"
  | Opcode.VCMPSS -> "vcmpss"
  | Opcode.VCOMISD -> "vcomisd"
  | Opcode.VCOMISS -> "vcomiss"
  | Opcode.VCVTDQ2PD -> "vcvtdq2pd"
  | Opcode.VCVTDQ2PS -> "vcvtdq2ps"
  | Opcode.VCVTPD2PS -> "vcvtpd2ps"
  | Opcode.VCVTPS2PD -> "vcvtps2pd"
  | Opcode.VCVTSD2SI -> "vcvtsd2si"
  | Opcode.VCVTSD2SS -> "vcvtsd2ss"
  | Opcode.VCVTSI2SD -> "vcvtsi2sd"
  | Opcode.VCVTSI2SS -> "vcvtsi2ss"
  | Opcode.VCVTSS2SD -> "vcvtss2sd"
  | Opcode.VCVTSS2SI -> "vcvtss2si"
  | Opcode.VCVTTPD2DQ -> "vcvttpd2dq"
  | Opcode.VCVTTSD2SI -> "vcvttsd2si"
  | Opcode.VCVTTSS2SI -> "vcvttss2si"
  | Opcode.VDIVPD -> "vdivpd"
  | Opcode.VDIVPS -> "vdivps"
  | Opcode.VDIVSD -> "vdivsd"
  | Opcode.VDIVSS -> "vdivss"
  | Opcode.VERR -> "verr"
  | Opcode.VERW -> "verw"
  | Opcode.VEXTRACTF128 -> "vextractf128"
  | Opcode.VEXTRACTF32X8 -> "vextractf32x8"
  | Opcode.VEXTRACTF64X2 -> "vextractf64x2"
  | Opcode.VEXTRACTF64X4 -> "vextractf64x4"
  | Opcode.VEXTRACTI64X4 -> "vextracti64x4"
  | Opcode.VFMADD132PD -> "vfmadd132pd"
  | Opcode.VFMADD132SD -> "vfmadd132sd"
  | Opcode.VFMADD132SS -> "vfmadd132ss"
  | Opcode.VFMADD213PD -> "vfmadd213pd"
  | Opcode.VFMADD213PS -> "vfmadd213ps"
  | Opcode.VFMADD213SD -> "vfmadd213sd"
  | Opcode.VFMADD213SS -> "vfmadd213ss"
  | Opcode.VFMADD231PD -> "vfmadd231pd"
  | Opcode.VFMADD231SD -> "vfmadd231sd"
  | Opcode.VFMADD231SS -> "vfmadd231ss"
  | Opcode.VFMSUB132SS -> "vfmsub132ss"
  | Opcode.VFMSUB213PD -> "vfmsub213pd"
  | Opcode.VFMSUB213SD -> "vfmsub213sd"
  | Opcode.VFMSUB231PD -> "vfmsub231pd"
  | Opcode.VFMSUB231SD -> "vfmsub231sd"
  | Opcode.VFNMADD132PD -> "vfnmadd132pd"
  | Opcode.VFNMADD213PD -> "vfnmadd213pd"
  | Opcode.VFNMADD231PD -> "vfnmadd231pd"
  | Opcode.VFNMADD132SD -> "vfnmadd132sd"
  | Opcode.VFNMADD213SD -> "vfnmadd213sd"
  | Opcode.VFNMADD231SD -> "vfnmadd231sd"
  | Opcode.VGATHERQPD -> "vgatherqpd"
  | Opcode.VGATHERDPS -> "vgatherdps"
  | Opcode.VGETEXPSD -> "vgetexpsd"
  | Opcode.VGETMANTSD -> "vgetmantsd"
  | Opcode.VINSERTF128 -> "vinsertf128"
  | Opcode.VINSERTF64X4 -> "vinsertf64x4"
  | Opcode.VINSERTI128 -> "vinserti128"
  | Opcode.VINSERTI64X4 -> "vinserti64x4"
  | Opcode.VLDDQU -> "vlddqu"
  | Opcode.VMAXPS -> "vmaxps"
  | Opcode.VMAXSD -> "vmaxsd"
  | Opcode.VMAXSS -> "vmaxss"
  | Opcode.VMCALL -> "vmcall"
  | Opcode.VMCLEAR -> "vmclear"
  | Opcode.VMFUNC -> "vmfunc"
  | Opcode.VMINPD -> "vminpd"
  | Opcode.VMINPS -> "vminps"
  | Opcode.VMINSS -> "vminss"
  | Opcode.VMLAUNCH -> "vmlaunch"
  | Opcode.VMOVAPD -> "vmovapd"
  | Opcode.VMOVAPS -> "vmovaps"
  | Opcode.VMOVD -> "vmovd"
  | Opcode.VMOVDDUP -> "vmovddup"
  | Opcode.VMOVDQA -> "vmovdqa"
  | Opcode.VMOVDQA32 -> "vmovdqa32"
  | Opcode.VMOVDQA64 -> "vmovdqa64"
  | Opcode.VMOVDQU -> "vmovdqu"
  | Opcode.VMOVDQU8 -> "vmovdqu8"
  | Opcode.VMOVDQU16 -> "vmovdqu16"
  | Opcode.VMOVDQU32 -> "vmovdqu32"
  | Opcode.VMOVDQU64 -> "vmovdqu64"
  | Opcode.VMOVHLPS -> "vmovhlps"
  | Opcode.VMOVHPD -> "vmovhpd"
  | Opcode.VMOVHPS -> "vmovhps"
  | Opcode.VMOVLHPS -> "vmovlhps"
  | Opcode.VMOVLPD -> "vmovlpd"
  | Opcode.VMOVLPS -> "vmovlps"
  | Opcode.VMOVMSKPD -> "vmovmskpd"
  | Opcode.VMOVMSKPS -> "vmovmskps"
  | Opcode.VMOVNTDQ -> "vmovntdq"
  | Opcode.VMOVNTPD -> "vmovntpd"
  | Opcode.VMOVNTPS -> "vmovntps"
  | Opcode.VMOVQ -> "vmovq"
  | Opcode.VMOVSD -> "vmovsd"
  | Opcode.VMOVSHDUP -> "vmovshdup"
  | Opcode.VMOVSLDUP -> "vmovsldup"
  | Opcode.VMOVSS -> "vmovss"
  | Opcode.VMOVUPD -> "vmovupd"
  | Opcode.VMOVUPS -> "vmovups"
  | Opcode.VMPTRLD -> "vmptrld"
  | Opcode.VMPTRST -> "vmptrst"
  | Opcode.VMRESUME -> "vmresume"
  | Opcode.VMULPD -> "vmulpd"
  | Opcode.VMULPS -> "vmulps"
  | Opcode.VMULSD -> "vmulsd"
  | Opcode.VMULSS -> "vmulss"
  | Opcode.VMXOFF -> "vmxoff"
  | Opcode.VMXON -> "vmxon"
  | Opcode.VORPD -> "vorpd"
  | Opcode.VORPS -> "vorps"
  | Opcode.VPABSB -> "vpabsb"
  | Opcode.VPABSD -> "vpabsd"
  | Opcode.VPABSW -> "vpabsw"
  | Opcode.VPACKSSDW -> "vpackssdw"
  | Opcode.VPACKSSWB -> "vpacksswb"
  | Opcode.VPACKUSDW -> "vpackusdw"
  | Opcode.VPACKUSWB -> "vpackuswb"
  | Opcode.VPADDB -> "vpaddb"
  | Opcode.VPADDD -> "vpaddd"
  | Opcode.VPADDQ -> "vpaddq"
  | Opcode.VPADDSB -> "vpaddsb"
  | Opcode.VPADDSW -> "vpaddsw"
  | Opcode.VPADDUSB -> "vpaddusb"
  | Opcode.VPADDUSW -> "vpaddusw"
  | Opcode.VPADDW -> "vpaddw"
  | Opcode.VPALIGNR -> "vpalignr"
  | Opcode.VPAND -> "vpand"
  | Opcode.VPANDN -> "vpandn"
  | Opcode.VPAVGB -> "vpavgb"
  | Opcode.VPAVGW -> "vpavgw"
  | Opcode.VPBROADCASTB -> "vpbroadcastb"
  | Opcode.VPBROADCASTD -> "vpbroadcastd"
  | Opcode.VPBROADCASTQ -> "vpbroadcastq"
  | Opcode.VPCMPEQB -> "vpcmpeqb"
  | Opcode.VPCMPEQD -> "vpcmpeqd"
  | Opcode.VPCMPEQQ -> "vpcmpeqq"
  | Opcode.VPCMPEQW -> "vpcmpeqw"
  | Opcode.VPCMPESTRI -> "vpcmpestri"
  | Opcode.VPCMPESTRM -> "vpcmpestrm"
  | Opcode.VPCMPGTB -> "vpcmpgtb"
  | Opcode.VPCMPGTD -> "vpcmpgtd"
  | Opcode.VPCMPGTQ -> "vpcmpgtq"
  | Opcode.VPCMPGTW -> "vpcmpgtw"
  | Opcode.VPCMPISTRI -> "vpcmpistri"
  | Opcode.VPCMPISTRM -> "vpcmpistrm"
  | Opcode.VPERMI2D -> "vpermi2d"
  | Opcode.VPERMI2PD -> "vpermi2pd"
  | Opcode.VPERMI2W -> "vpermi2w"
  | Opcode.VPEXTRD -> "vpextrd"
  | Opcode.VPEXTRW -> "vpextrw"
  | Opcode.VPGATHERDD -> "vpgatherdd"
  | Opcode.VPHADDD -> "vphaddd"
  | Opcode.VPHADDSW -> "vphaddsw"
  | Opcode.VPHADDW -> "vphaddw"
  | Opcode.VPHMINPOSUW -> "vphminposuw"
  | Opcode.VPHSUBD -> "vphsubd"
  | Opcode.VPHSUBSW -> "vphsubsw"
  | Opcode.VPHSUBW -> "vphsubw"
  | Opcode.VPINSRB -> "vpinsrb"
  | Opcode.VPINSRD -> "vpinsrd"
  | Opcode.VPINSRW -> "vpinsrw"
  | Opcode.VPINSRQ -> "vpinsrq"
  | Opcode.VPMADDWD -> "vpmaddwd"
  | Opcode.VPMAXSB -> "vpmaxsb"
  | Opcode.VPMAXSD -> "vpmaxsd"
  | Opcode.VPMAXSW -> "vpmaxsw"
  | Opcode.VPMAXUB -> "vpmaxub"
  | Opcode.VPMAXUD -> "vpmaxud"
  | Opcode.VPMAXUW -> "vpmaxuw"
  | Opcode.VPMINSB -> "vpminsb"
  | Opcode.VPMINSD -> "vpminsd"
  | Opcode.VPMINSW -> "vpminsw"
  | Opcode.VPMINUB -> "vpminub"
  | Opcode.VPMINUD -> "vpminud"
  | Opcode.VPMINUW -> "vpminuw"
  | Opcode.VPMOVMSKB -> "vpmovmskb"
  | Opcode.VPMOVSXBD -> "vpmovsxbd"
  | Opcode.VPMOVSXBQ -> "vpmovsxbq"
  | Opcode.VPMOVSXBW -> "vpmovsxbw"
  | Opcode.VPMOVSXDQ -> "vpmovsxdq"
  | Opcode.VPMOVSXWD -> "vpmovsxwd"
  | Opcode.VPMOVSXWQ -> "vpmovsxwq"
  | Opcode.VPMOVWB -> "vpmovwb"
  | Opcode.VPMOVZXBD -> "vpmovzxbd"
  | Opcode.VPMOVZXBQ -> "vpmovzxbq"
  | Opcode.VPMOVZXBW -> "vpmovzxbw"
  | Opcode.VPMOVZXDQ -> "vpmovzxdq"
  | Opcode.VPMOVZXWD -> "vpmovzxwd"
  | Opcode.VPMOVZXWQ -> "vpmovzxwq"
  | Opcode.VPMULDQ -> "vpmuldq"
  | Opcode.VPMULHRSW -> "vpmulhrsw"
  | Opcode.VPMULHUW -> "vpmulhuw"
  | Opcode.VPMULHW -> "vpmulhw"
  | Opcode.VPMULLD -> "vpmulld"
  | Opcode.VPMULLW -> "vpmullw"
  | Opcode.VPMULUDQ -> "vpmuludq"
  | Opcode.VPOR -> "vpor"
  | Opcode.VPSADBW -> "vpsadbw"
  | Opcode.VPSHUFB -> "vpshufb"
  | Opcode.VPSHUFD -> "vpshufd"
  | Opcode.VPSHUFHW -> "vpshufhw"
  | Opcode.VPSHUFLW -> "vpshuflw"
  | Opcode.VPSIGNB -> "vpsignb"
  | Opcode.VPSIGND -> "vpsignd"
  | Opcode.VPSIGNW -> "vpsignw"
  | Opcode.VPSLLD -> "vpslld"
  | Opcode.VPSLLDQ -> "vpslldq"
  | Opcode.VPSLLQ -> "vpsllq"
  | Opcode.VPSLLW -> "vpsllw"
  | Opcode.VPSRAD -> "vpsrad"
  | Opcode.VPSRAW -> "vpsraw"
  | Opcode.VPSRLD -> "vpsrld"
  | Opcode.VPSRLDQ -> "vpsrldq"
  | Opcode.VPSRLQ -> "vpsrlq"
  | Opcode.VPSRLW -> "vpsrlw"
  | Opcode.VPSUBB -> "vpsubb"
  | Opcode.VPSUBD -> "vpsubd"
  | Opcode.VPSUBQ -> "vpsubq"
  | Opcode.VPSUBSB -> "vpsubsb"
  | Opcode.VPSUBSW -> "vpsubsw"
  | Opcode.VPSUBUSB -> "vpsubusb"
  | Opcode.VPSUBUSW -> "vpsubusw"
  | Opcode.VPSUBW -> "vpsubw"
  | Opcode.VPTERNLOGD -> "vpternlogd"
  | Opcode.VPTEST -> "vptest"
  | Opcode.VPUNPCKHBW -> "vpunpckhbw"
  | Opcode.VPUNPCKHDQ -> "vpunpckhdq"
  | Opcode.VPUNPCKHQDQ -> "vpunpckhqdq"
  | Opcode.VPUNPCKHWD -> "vpunpckhwd"
  | Opcode.VPUNPCKLBW -> "vpunpcklbw"
  | Opcode.VPUNPCKLDQ -> "vpunpckldq"
  | Opcode.VPUNPCKLQDQ -> "vpunpcklqdq"
  | Opcode.VPUNPCKLWD -> "vpunpcklwd"
  | Opcode.VPXOR -> "vpxor"
  | Opcode.VPXORD -> "vpxord"
  | Opcode.VPXORQ -> "vpxorq"
  | Opcode.VRCP14SD -> "vrcp14sd"
  | Opcode.VRCP28SD -> "vrcp28sd"
  | Opcode.VRCPPS -> "vrcpps"
  | Opcode.VRCPSS -> "vrcpss"
  | Opcode.VREDUCESD -> "vreducesd"
  | Opcode.VRNDSCALESD -> "vrndscalesd"
  | Opcode.VROUNDPD -> "vroundpd"
  | Opcode.VROUNDPS -> "vroundps"
  | Opcode.VROUNDSD -> "vroundsd"
  | Opcode.VRSQRT28SD -> "vrsqrt28sd"
  | Opcode.VRSQRTPS -> "vrsqrtps"
  | Opcode.VRSQRTSS -> "vrsqrtss"
  | Opcode.VSHUFI64X2 -> "vshufi64x2"
  | Opcode.VSHUFI32X4 -> "vshufi32x4"
  | Opcode.VSHUFPD -> "vshufpd"
  | Opcode.VSHUFPS -> "vshufps"
  | Opcode.VSQRTPD -> "vsqrtpd"
  | Opcode.VSQRTPS -> "vsqrtps"
  | Opcode.VSQRTSD -> "vsqrtsd"
  | Opcode.VSQRTSS -> "vsqrtss"
  | Opcode.VSUBPD -> "vsubpd"
  | Opcode.VSUBPS -> "vsubps"
  | Opcode.VSUBSD -> "vsubsd"
  | Opcode.VSUBSS -> "vsubss"
  | Opcode.VUCOMISD -> "vucomisd"
  | Opcode.VUCOMISS -> "vucomiss"
  | Opcode.VUNPCKHPD -> "vunpckhpd"
  | Opcode.VUNPCKHPS -> "vunpckhps"
  | Opcode.VUNPCKLPD -> "vunpcklpd"
  | Opcode.VUNPCKLPS -> "vunpcklps"
  | Opcode.VXORPD -> "vxorpd"
  | Opcode.VXORPS -> "vxorps"
  | Opcode.VZEROUPPER -> "vzeroupper"
  | Opcode.WAIT -> "wait"
  | Opcode.WBINVD -> "wbinvd"
  | Opcode.WRFSBASE -> "wrfsbase"
  | Opcode.WRGSBASE -> "wrgsbase"
  | Opcode.WRMSR -> "wrmsr"
  | Opcode.WRPKRU -> "wrpkru"
  | Opcode.WRSSD -> "wrssd"
  | Opcode.WRSSQ -> "wrssq"
  | Opcode.WRUSSD -> "wrussd"
  | Opcode.WRUSSQ -> "wrussq"
  | Opcode.XABORT -> "xabort"
  | Opcode.XADD -> "xadd"
  | Opcode.XBEGIN -> "xbegin"
  | Opcode.XCHG -> "xchg"
  | Opcode.XEND -> "xend"
  | Opcode.XGETBV -> "xgetbv"
  | Opcode.XLATB -> "xlatb"
  | Opcode.XOR -> "xor"
  | Opcode.XORPD -> "xorpd"
  | Opcode.XORPS -> "xorps"
  | Opcode.XRSTOR -> "xrstor"
  | Opcode.XRSTORS -> "xrstors"
  | Opcode.XRSTORS64 -> "xrstors64"
  | Opcode.XSAVE -> "xsave"
  | Opcode.XSAVEC -> "xsavec"
  | Opcode.XSAVEC64 -> "xsavec64"
  | Opcode.XSAVES -> "xsaves"
  | Opcode.XSAVES64 -> "xsaves64"
  | Opcode.XSAVEOPT -> "xsaveopt"
  | Opcode.XSETBV -> "xsetbv"
  | Opcode.XTEST -> "xtest"
  | _ -> raise InvalidOpcodeException

let inline private iToHexStr (i: int64) (builder: DisasmBuilder<_>) =
  builder.Accumulate AsmWordKind.Value (String.i64ToHex i)

let inline private uToHexStr (i: uint64) (builder: DisasmBuilder<_>) =
  builder.Accumulate AsmWordKind.Value (String.u64ToHex i)

let private ptrDirectiveString isFar = function
  | 8<rt> -> "byte ptr"
  | 16<rt> -> "word ptr"
  | 32<rt> -> if isFar then "word far ptr" else "dword ptr"
  | 48<rt> -> "dword far ptr"
  | 64<rt> -> "qword ptr"
  | 80<rt> -> if isFar then "qword far ptr" else "tword ptr"
  | 128<rt> -> "xmmword ptr"
  | 256<rt> -> "ymmword ptr"
  | 512<rt> -> "zmmword ptr"
  | 224<rt> | 864<rt> -> "" (* x87 FPU state *)
  | _ -> Utils.impossible ()

let dispToString showSign (disp: Disp) (builder: DisasmBuilder<_>) =
  let mask = WordSize.toRegType builder.WordSize |> RegType.getMask |> uint64
  if showSign && disp < 0L then
    builder.Accumulate AsmWordKind.String "-"
    iToHexStr (- disp) builder
  elif showSign then
    builder.Accumulate AsmWordKind.String "+"
    iToHexStr disp builder
  else
    uToHexStr (uint64 disp &&& mask) builder

let inline private memDispToStr showSign disp builder =
  match disp with
  | None -> ()
  | Some d -> dispToString showSign d builder

let inline scaleToString (scale: Scale) (builder: DisasmBuilder<_>) =
  if scale = Scale.X1 then ()
  else
    builder.Accumulate AsmWordKind.String "*"
    builder.Accumulate AsmWordKind.Value ((int scale).ToString())

let private memScaleDispToStr emptyBase si d builder =
  match si with
  | None -> memDispToStr (not emptyBase) d builder
  | Some (i, scale) ->
    if emptyBase then () else builder.Accumulate AsmWordKind.String "+"
    builder.Accumulate AsmWordKind.Variable (Register.toString i)
    scaleToString scale builder
    memDispToStr true d builder

let private memAddrToStr b si disp builder =
  match b with
  | None -> memScaleDispToStr true si disp builder
  | Some b ->
    builder.Accumulate AsmWordKind.Variable (Register.toString b)
    memScaleDispToStr false si disp builder

let inline isFar (ins: InsInfo) =
  match ins.Opcode with
  | Opcode.JMPFar | Opcode.CALLFar -> true
  | _ -> false

let mToString (ins: InsInfo) (builder: DisasmBuilder<_>) b si d oprSz =
  let ptrDirective = ptrDirectiveString (isFar ins) oprSz
  match Helper.getSegment ins.Prefixes with
  | None ->
    builder.Accumulate AsmWordKind.String ptrDirective
    builder.Accumulate AsmWordKind.String (" [")
    memAddrToStr b si d builder
    builder.Accumulate AsmWordKind.String "]"
  | Some seg ->
    builder.Accumulate AsmWordKind.String ptrDirective
    builder.Accumulate AsmWordKind.String (" [")
    builder.Accumulate AsmWordKind.Variable (Register.toString seg)
    builder.Accumulate AsmWordKind.String ":"
    memAddrToStr b si d builder
    builder.Accumulate AsmWordKind.String "]"

let commentWithSymbol (helper: DisasmHelper) targetAddr builder =
  if (builder: DisasmBuilder<_>).ResolveSymbol then
    match helper.FindFunctionSymbol (targetAddr) with
    | Error _ ->
      (builder: DisasmBuilder<_>).Accumulate AsmWordKind.String " ; "
      uToHexStr targetAddr builder
    | Ok "" -> ()
    | Ok name ->
      builder.Accumulate AsmWordKind.String " ; <"
      builder.Accumulate AsmWordKind.Value name
      builder.Accumulate AsmWordKind.String ">"
  else ()

let inline relToString offset hlp (builder: DisasmBuilder<_>) =
  if offset < 0L then builder.Accumulate AsmWordKind.String "-"
  else builder.Accumulate AsmWordKind.String "+"
  iToHexStr (abs offset) builder
  commentWithSymbol hlp (builder.Address + uint64 offset) builder

let inline absToString selector (offset: Addr) builder =
  uToHexStr (uint64 selector) builder
  builder.Accumulate AsmWordKind.String ":"
  uToHexStr offset builder

let getOpmaskRegister = function
  | 0x0uy -> Register.K0
  | 0x1uy -> Register.K1
  | 0x2uy -> Register.K2
  | 0x3uy -> Register.K3
  | 0x4uy -> Register.K4
  | 0x5uy -> Register.K5
  | 0x6uy -> Register.K6
  | 0x7uy -> Register.K7
  | _ -> raise InvalidRegisterException

/// Zeroing/Merging (EVEX.z)
let maskZtoString ev (builder: DisasmBuilder<_>) =
  if ev.Z = Zeroing then ()
  else builder.Accumulate AsmWordKind.String "{z}"

/// Opmask register
let maskRegToString ePrx (builder: DisasmBuilder<_>) =
  if ePrx.AAA = 0uy then ()
  else
    builder.Accumulate AsmWordKind.String " {"
    builder.Accumulate AsmWordKind.Variable
      (getOpmaskRegister ePrx.AAA |> Register.toString)
    builder.Accumulate AsmWordKind.String "}"

let buildMask (ins: InsInfo) builder =
  match ins.VEXInfo with
  | Some { EVEXPrx = Some ePrx }->
    maskRegToString ePrx builder
    maskZtoString ePrx builder
  | _ -> ()

let inline private getMask sz =
  match sz with
  | 8<rt> -> 0xFFL
  | 16<rt> -> 0xFFFFL
  | 32<rt> -> 0xFFFFFFFFL
  | _ -> 0xFFFFFFFFFFFFFFFFL

let oprToString ins hlp opr isFst (builder: DisasmBuilder<_>) =
  match opr with
  | OprReg reg ->
    builder.Accumulate AsmWordKind.Variable (Register.toString reg)
    if isFst then buildMask ins builder else ()
  | OprMem (b, si, disp, oprSz) ->
    mToString ins builder b si disp oprSz
    if isFst then buildMask ins builder else ()
  | OprImm (imm, _) -> iToHexStr (imm &&& getMask ins.MainOperationSize) builder
  | OprDirAddr (Absolute (sel, offset, _)) -> absToString sel offset builder
  | OprDirAddr (Relative (offset)) -> relToString offset hlp builder
  | Label _ -> Utils.impossible ()

let inline buildPref (prefs: Prefix) (builder: DisasmBuilder<_>) =
  if prefs = Prefix.PrxNone then ()
  elif (prefs &&& Prefix.PrxLOCK) <> Prefix.PrxNone then
    builder.Accumulate AsmWordKind.String "lock "
  elif (prefs &&& Prefix.PrxREPNZ) <> Prefix.PrxNone then
    builder.Accumulate AsmWordKind.String "repnz "
  elif (prefs &&& Prefix.PrxREPZ) <> Prefix.PrxNone then
    builder.Accumulate AsmWordKind.String "repz "
  elif (prefs &&& Prefix.PrxBND) <> Prefix.PrxNone then
    builder.Accumulate AsmWordKind.String "bnd "
  else ()

let inline buildOpcode opcode (builder: DisasmBuilder<_>) =
  builder.Accumulate AsmWordKind.Mnemonic (opCodeToString opcode)

let recomputeRIPRel disp oprSize builder =
  let dir = ptrDirectiveString false oprSize
  (builder: DisasmBuilder<_>).Accumulate AsmWordKind.String dir
  builder.Accumulate AsmWordKind.String " ["
  uToHexStr (builder.Address + uint64 disp + uint64 builder.InsLength) builder
  builder.Accumulate AsmWordKind.String "]"

let buildOprs (ins: InsInfo) hlp (builder: DisasmBuilder<_>) =
  match ins.Operands with
  | NoOperand -> ()
  | OneOperand (OprMem (Some Register.RIP, None, Some off, 64<rt>)) ->
    builder.Accumulate AsmWordKind.String (" ")
    mToString ins builder (Some Register.RIP) None (Some off) 64<rt>
    commentWithSymbol hlp
      (builder.Address + uint64 builder.InsLength + uint64 off) builder
  | OneOperand opr ->
    builder.Accumulate AsmWordKind.String " "
    oprToString ins hlp opr true builder
  | TwoOperands (OprMem (Some R.RIP, None, Some disp, sz), opr) ->
    builder.Accumulate AsmWordKind.String " "
    recomputeRIPRel disp sz builder
    builder.Accumulate AsmWordKind.String ", "
    oprToString ins hlp opr false builder
  | TwoOperands (opr, OprMem (Some R.RIP, None, Some disp, sz)) ->
    builder.Accumulate AsmWordKind.String " "
    oprToString ins hlp opr true builder
    builder.Accumulate AsmWordKind.String ", "
    recomputeRIPRel disp sz builder
  | TwoOperands (opr1, opr2) ->
    builder.Accumulate AsmWordKind.String " "
    oprToString ins hlp opr1 true builder
    builder.Accumulate AsmWordKind.String ", "
    oprToString ins hlp opr2 false builder
  | ThreeOperands (opr1, opr2, opr3) ->
    builder.Accumulate AsmWordKind.String " "
    oprToString ins hlp opr1 true builder
    builder.Accumulate AsmWordKind.String ", "
    oprToString ins hlp opr2 false builder
    builder.Accumulate AsmWordKind.String ", "
    oprToString ins hlp opr3 false builder
  | FourOperands (opr1, opr2, opr3, opr4) ->
    builder.Accumulate AsmWordKind.String " "
    oprToString ins hlp opr1 true builder
    builder.Accumulate AsmWordKind.String ", "
    oprToString ins hlp opr2 false builder
    builder.Accumulate AsmWordKind.String ", "
    oprToString ins hlp opr3 false builder
    builder.Accumulate AsmWordKind.String ", "
    oprToString ins hlp opr4 false builder

let disasm hlp ins (builder: DisasmBuilder<_>) =
  if builder.ShowAddr then builder.AccumulateAddr () else ()
  buildPref (ins: InsInfo).Prefixes builder
  buildOpcode ins.Opcode builder
  buildOprs ins hlp builder

// vim: set tw=80 sts=2 sw=2:
