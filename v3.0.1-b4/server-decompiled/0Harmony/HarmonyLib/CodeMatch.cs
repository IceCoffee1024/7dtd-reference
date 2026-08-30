using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x0200004C RID: 76
	public class CodeMatch : CodeInstruction
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000C220 File Offset: 0x0000A420
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000C246 File Offset: 0x0000A446
		[Obsolete("Use opcodeSet instead")]
		public List<OpCode> opcodes
		{
			get
			{
				HashSet<OpCode> hashSet = this.opcodeSet;
				List<OpCode> list = new List<OpCode>(hashSet.Count);
				list.AddRange(hashSet);
				return list;
			}
			set
			{
				this.opcodeSet = new HashSet<OpCode>(value);
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000C254 File Offset: 0x0000A454
		internal CodeMatch Set(object operand, string name)
		{
			if (this.operand == null)
			{
				this.operand = operand;
			}
			if (operand != null)
			{
				this.operands.Add(operand);
			}
			if (this.name == null)
			{
				this.name = name;
			}
			return this;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000C284 File Offset: 0x0000A484
		internal CodeMatch Set(OpCode opcode, object operand, string name)
		{
			this.opcode = opcode;
			this.opcodeSet.Add(opcode);
			if (this.operand == null)
			{
				this.operand = operand;
			}
			if (operand != null)
			{
				this.operands.Add(operand);
			}
			if (this.name == null)
			{
				this.name = name;
			}
			return this;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000C2D4 File Offset: 0x0000A4D4
		public CodeMatch(OpCode? opcode = null, object operand = null, string name = null)
		{
			if (opcode != null)
			{
				OpCode valueOrDefault = opcode.GetValueOrDefault();
				this.opcode = valueOrDefault;
				this.opcodeSet.Add(valueOrDefault);
			}
			if (operand != null)
			{
				this.operands.Add(operand);
			}
			this.operand = operand;
			this.name = name;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000C358 File Offset: 0x0000A558
		public static CodeMatch WithOpcodes(HashSet<OpCode> opcodes, object operand = null, string name = null)
		{
			return new CodeMatch(null, operand, name)
			{
				opcodeSet = opcodes
			};
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000C37C File Offset: 0x0000A57C
		public CodeMatch(Expression<Action> expression, string name = null)
		{
			this.opcodeSet.UnionWith(CodeInstructionExtensions.opcodesCalling);
			this.operand = SymbolExtensions.GetMethodInfo(expression);
			if (this.operand != null)
			{
				this.operands.Add(this.operand);
			}
			this.name = name;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public CodeMatch(LambdaExpression expression, string name = null)
		{
			this.opcodeSet.UnionWith(CodeInstructionExtensions.opcodesCalling);
			this.operand = SymbolExtensions.GetMethodInfo(expression);
			if (this.operand != null)
			{
				this.operands.Add(this.operand);
			}
			this.name = name;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000C473 File Offset: 0x0000A673
		public CodeMatch(CodeInstruction instruction, string name = null)
			: this(new OpCode?(instruction.opcode), instruction.operand, name)
		{
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000C490 File Offset: 0x0000A690
		public CodeMatch(Func<CodeInstruction, bool> predicate, string name = null)
		{
			this.predicate = predicate;
			this.name = name;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
		internal bool Matches(List<CodeInstruction> codes, CodeInstruction instruction)
		{
			if (this.predicate != null)
			{
				return this.predicate(instruction);
			}
			if (this.opcodeSet.Count > 0 && !this.opcodeSet.Contains(instruction.opcode))
			{
				return false;
			}
			if (this.opcode.Size != 0 && this.opcode.Value != instruction.opcode.Value)
			{
				return false;
			}
			if (this.operands.Count > 0 && !this.operands.Contains(instruction.operand))
			{
				return false;
			}
			if (this.operand != null && !this.operand.Equals(instruction.operand))
			{
				return false;
			}
			if (this.labels.Count > 0 && !this.labels.Intersect<Label>(instruction.labels).Any<Label>())
			{
				return false;
			}
			if (this.blocks.Count > 0 && !this.blocks.Intersect<ExceptionBlock>(instruction.blocks).Any<ExceptionBlock>())
			{
				return false;
			}
			if (this.jumpsFrom.Count > 0 && !this.jumpsFrom.Select<int, object>((int index) => codes[index].operand).OfType<Label>().Intersect<Label>(instruction.labels)
				.Any<Label>())
			{
				return false;
			}
			if (this.jumpsTo.Count > 0)
			{
				object operand = instruction.operand;
				if (operand == null || operand.GetType() != typeof(Label))
				{
					return false;
				}
				Label label = (Label)operand;
				IEnumerable<int> enumerable = from idx in Enumerable.Range(0, codes.Count)
					where codes[idx].labels.Contains(label)
					select idx;
				if (!this.jumpsTo.Intersect<int>(enumerable).Any<int>())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
		public static CodeMatch IsLdarg(int? n = null)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.IsLdarg(n), null);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000C6DC File Offset: 0x0000A8DC
		public static CodeMatch IsLdarga(int? n = null)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.IsLdarga(n), null);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000C708 File Offset: 0x0000A908
		public static CodeMatch IsStarg(int? n = null)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.IsStarg(n), null);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000C734 File Offset: 0x0000A934
		public static CodeMatch IsLdloc(LocalBuilder variable = null)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.IsLdloc(variable), null);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000C760 File Offset: 0x0000A960
		public static CodeMatch IsStloc(LocalBuilder variable = null)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.IsStloc(variable), null);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000C78C File Offset: 0x0000A98C
		public static CodeMatch Calls(MethodInfo method)
		{
			return CodeMatch.WithOpcodes(CodeInstructionExtensions.opcodesCalling, method, null);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000C79A File Offset: 0x0000A99A
		public static CodeMatch LoadsConstant()
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.LoadsConstant(), null);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		public static CodeMatch LoadsConstant(long number)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.LoadsConstant(number), null);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000C7F0 File Offset: 0x0000A9F0
		public static CodeMatch LoadsConstant(double number)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.LoadsConstant(number), null);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000C81C File Offset: 0x0000AA1C
		public static CodeMatch LoadsConstant(Enum e)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.LoadsConstant(e), null);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000C848 File Offset: 0x0000AA48
		public static CodeMatch LoadsConstant(string str)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.LoadsConstant(str), null);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000C874 File Offset: 0x0000AA74
		public static CodeMatch LoadsField(FieldInfo field, bool byAddress = false)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.LoadsField(field, byAddress), null);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
		public static CodeMatch StoresField(FieldInfo field)
		{
			return new CodeMatch((CodeInstruction instruction) => instruction.StoresField(field), null);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
		public static CodeMatch Calls(Expression<Action> expression)
		{
			return new CodeMatch(expression, null);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000C8DD File Offset: 0x0000AADD
		public static CodeMatch Calls(LambdaExpression expression)
		{
			return new CodeMatch(expression, null);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000C8E6 File Offset: 0x0000AAE6
		public static CodeMatch LoadsLocal(bool useAddress = false, string name = null)
		{
			return CodeMatch.WithOpcodes(useAddress ? CodeInstructionExtensions.opcodesLoadingLocalByAddress : CodeInstructionExtensions.opcodesLoadingLocalNormal, null, name);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000C8FE File Offset: 0x0000AAFE
		public static CodeMatch StoresLocal(string name = null)
		{
			return CodeMatch.WithOpcodes(CodeInstructionExtensions.opcodesStoringLocal, null, name);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000C90C File Offset: 0x0000AB0C
		public static CodeMatch LoadsArgument(bool useAddress = false, string name = null)
		{
			return CodeMatch.WithOpcodes(useAddress ? CodeInstructionExtensions.opcodesLoadingArgumentByAddress : CodeInstructionExtensions.opcodesLoadingArgumentNormal, null, name);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000C924 File Offset: 0x0000AB24
		public static CodeMatch StoresArgument(string name = null)
		{
			return CodeMatch.WithOpcodes(CodeInstructionExtensions.opcodesStoringArgument, null, name);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000C932 File Offset: 0x0000AB32
		public static CodeMatch Branches(string name = null)
		{
			return CodeMatch.WithOpcodes(CodeInstructionExtensions.opcodesBranching, null, name);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000C940 File Offset: 0x0000AB40
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.name != null)
			{
				stringBuilder.Append(this.name + ": ");
			}
			if (this.opcodeSet.Count > 0)
			{
				stringBuilder.Append("opcodes=" + this.opcodeSet.Join<OpCode>(null, ", ") + " ");
			}
			if (this.operands.Count > 0)
			{
				stringBuilder.Append("operands=" + (from o in this.operands.Union<object>(new object[] { this.operand })
					where o != null
					select o).Join<object>(null, ", ") + " ");
			}
			if (this.labels.Count > 0)
			{
				stringBuilder.Append("labels=" + this.labels.Join<Label>(null, ", ") + " ");
			}
			if (this.blocks.Count > 0)
			{
				stringBuilder.Append("blocks=" + this.blocks.Join<ExceptionBlock>(null, ", ") + " ");
			}
			if (this.jumpsFrom.Count > 0)
			{
				stringBuilder.Append("jumpsFrom=" + this.jumpsFrom.Join<int>(null, ", ") + " ");
			}
			if (this.jumpsTo.Count > 0)
			{
				stringBuilder.Append("jumpsTo=" + this.jumpsTo.Join<int>(null, ", ") + " ");
			}
			if (this.predicate != null)
			{
				stringBuilder.Append("predicate=yes ");
			}
			if (stringBuilder.Length == 0)
			{
				return base.ToString();
			}
			return "[" + stringBuilder.ToString().Trim() + "]";
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000CB25 File Offset: 0x0000AD25
		public static implicit operator CodeMatch(OpCode opcode)
		{
			return new CodeMatch(new OpCode?(opcode), null, null);
		}

		// Token: 0x040000AD RID: 173
		public string name;

		// Token: 0x040000AE RID: 174
		public HashSet<OpCode> opcodeSet = new HashSet<OpCode>();

		// Token: 0x040000AF RID: 175
		public List<object> operands = new List<object>();

		// Token: 0x040000B0 RID: 176
		public List<int> jumpsFrom = new List<int>();

		// Token: 0x040000B1 RID: 177
		public List<int> jumpsTo = new List<int>();

		// Token: 0x040000B2 RID: 178
		public Func<CodeInstruction, bool> predicate;
	}
}
