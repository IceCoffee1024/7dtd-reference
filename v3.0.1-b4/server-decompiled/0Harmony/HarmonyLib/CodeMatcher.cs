using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x0200004D RID: 77
	public class CodeMatcher
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000CB34 File Offset: 0x0000AD34
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		public int Pos { get; private set; } = -1;

		// Token: 0x06000317 RID: 791 RVA: 0x0000CB45 File Offset: 0x0000AD45
		private void FixStart()
		{
			this.Pos = Math.Max(0, this.Pos);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000CB59 File Offset: 0x0000AD59
		private void SetOutOfBounds(int direction)
		{
			this.Pos = ((direction > 0) ? this.Length : (-1));
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000CB6E File Offset: 0x0000AD6E
		private int PosOrThrow()
		{
			if (this.IsInvalid)
			{
				throw new InvalidOperationException("Current position is out of bounds");
			}
			return this.Pos;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000CB89 File Offset: 0x0000AD89
		public int Length
		{
			get
			{
				return this.codes.Count;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000CB96 File Offset: 0x0000AD96
		public bool IsValid
		{
			get
			{
				return this.Pos >= 0 && this.Pos < this.Length;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000CBB1 File Offset: 0x0000ADB1
		public bool IsInvalid
		{
			get
			{
				return this.Pos < 0 || this.Pos >= this.Length;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000CBCF File Offset: 0x0000ADCF
		public int Remaining
		{
			get
			{
				return this.Length - this.PosOrThrow();
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000CBDE File Offset: 0x0000ADDE
		public ref OpCode Opcode
		{
			get
			{
				return ref this.codes[this.PosOrThrow()].opcode;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000CBF6 File Offset: 0x0000ADF6
		public ref object Operand
		{
			get
			{
				return ref this.codes[this.PosOrThrow()].operand;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000CC0E File Offset: 0x0000AE0E
		public ref List<Label> Labels
		{
			get
			{
				return ref this.codes[this.PosOrThrow()].labels;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000CC26 File Offset: 0x0000AE26
		public ref List<ExceptionBlock> Blocks
		{
			get
			{
				return ref this.codes[this.PosOrThrow()].blocks;
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000CC3E File Offset: 0x0000AE3E
		public CodeMatcher()
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000CC64 File Offset: 0x0000AE64
		public CodeMatcher(IEnumerable<CodeInstruction> instructions, ILGenerator generator = null)
		{
			if (instructions == null)
			{
				throw new ArgumentNullException("instructions");
			}
			this.generator = generator;
			this.codes = instructions.Select<CodeInstruction, CodeInstruction>((CodeInstruction c) => new CodeInstruction(c)).ToList<CodeInstruction>();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		public CodeMatcher Clone()
		{
			return new CodeMatcher(this.codes, this.generator)
			{
				Pos = this.Pos,
				lastMatches = this.lastMatches,
				lastError = this.lastError,
				lastMatchCall = this.lastMatchCall
			};
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000CD2A File Offset: 0x0000AF2A
		public CodeInstruction Instruction
		{
			get
			{
				return this.codes[this.PosOrThrow()];
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000CD40 File Offset: 0x0000AF40
		public CodeInstruction InstructionAt(int offset)
		{
			int num = this.PosOrThrow();
			int num2 = num + offset;
			if (num2 < 0 || num2 >= this.Length)
			{
				throw new ArgumentOutOfRangeException("offset", "offset causes position to go out of bounds");
			}
			return this.codes[num2];
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000CD81 File Offset: 0x0000AF81
		public List<CodeInstruction> Instructions()
		{
			return this.codes;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000CD89 File Offset: 0x0000AF89
		public IEnumerable<CodeInstruction> InstructionEnumeration()
		{
			return this.codes.AsEnumerable<CodeInstruction>();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000CD96 File Offset: 0x0000AF96
		public List<CodeInstruction> Instructions(int count)
		{
			return (from c in this.codes.GetRange(this.PosOrThrow(), count)
				select new CodeInstruction(c)).ToList<CodeInstruction>();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000CDD4 File Offset: 0x0000AFD4
		public List<CodeInstruction> InstructionsInRange(int start, int end)
		{
			List<CodeInstruction> range = this.codes;
			if (start > end)
			{
				int num = start;
				start = end;
				end = num;
			}
			range = range.GetRange(start, end - start + 1);
			return range.Select<CodeInstruction, CodeInstruction>((CodeInstruction c) => new CodeInstruction(c)).ToList<CodeInstruction>();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000CE2C File Offset: 0x0000B02C
		public List<CodeInstruction> InstructionsWithOffsets(int startOffset, int endOffset)
		{
			int num = this.PosOrThrow();
			int num2 = num + startOffset;
			if (num2 < 0 || num2 >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startOffset", "startOffset causes position to go out of bounds");
			}
			int num3 = num + endOffset;
			if (num3 < 0 || num3 >= this.Length)
			{
				throw new ArgumentOutOfRangeException("endOffset", "endOffset causes position to go out of bounds");
			}
			return this.InstructionsInRange(num2, num3);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000CE8A File Offset: 0x0000B08A
		public List<Label> DistinctLabels(IEnumerable<CodeInstruction> instructions)
		{
			return instructions.SelectMany<CodeInstruction, Label>((CodeInstruction instruction) => instruction.labels).Distinct<Label>().ToList<Label>();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		public bool ReportFailure(MethodBase method, Action<string> logger)
		{
			if (this.IsValid)
			{
				return false;
			}
			string text = this.lastError ?? "Unexpected code";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" in ");
			defaultInterpolatedStringHandler.AppendFormatted<MethodBase>(method);
			logger(defaultInterpolatedStringHandler.ToStringAndClear());
			return true;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000CF18 File Offset: 0x0000B118
		public CodeMatcher ThrowIfInvalid(string explanation)
		{
			if (explanation == null)
			{
				throw new ArgumentNullException("explanation");
			}
			if (this.IsInvalid)
			{
				string text = explanation + " - Current state is invalid.";
				if (this.lastError != null)
				{
					text = text + " Details: " + this.lastError;
				}
				throw new InvalidOperationException(text);
			}
			return this;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000CF69 File Offset: 0x0000B169
		public CodeMatcher ThrowIfNotMatch(string explanation, params CodeMatch[] matches)
		{
			this.ThrowIfInvalid(explanation);
			if (!this.MatchSequence(this.Pos, matches))
			{
				throw new InvalidOperationException(explanation + " - Match failed");
			}
			return this;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000CF94 File Offset: 0x0000B194
		private void ThrowIfNotMatch(string explanation, int direction, CodeMatch[] matches)
		{
			this.ThrowIfInvalid(explanation);
			int pos = this.Pos;
			try
			{
				if (this.Match(matches, direction, false).IsInvalid)
				{
					throw new InvalidOperationException(explanation + " - Match failed");
				}
			}
			finally
			{
				this.Pos = pos;
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000CFEC File Offset: 0x0000B1EC
		public CodeMatcher ThrowIfNotMatchForward(string explanation, params CodeMatch[] matches)
		{
			this.ThrowIfNotMatch(explanation, 1, matches);
			return this;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000CFF8 File Offset: 0x0000B1F8
		public CodeMatcher ThrowIfNotMatchBack(string explanation, params CodeMatch[] matches)
		{
			this.ThrowIfNotMatch(explanation, -1, matches);
			return this;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000D004 File Offset: 0x0000B204
		public CodeMatcher ThrowIfFalse(string explanation, Func<CodeMatcher, bool> stateCheckFunc)
		{
			if (stateCheckFunc == null)
			{
				throw new ArgumentNullException("stateCheckFunc");
			}
			this.ThrowIfInvalid(explanation);
			if (!stateCheckFunc(this))
			{
				throw new InvalidOperationException(explanation + " - Check function returned false");
			}
			return this;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000D037 File Offset: 0x0000B237
		public CodeMatcher SetInstruction(CodeInstruction instruction)
		{
			this.codes[this.PosOrThrow()] = instruction;
			return this;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000D04C File Offset: 0x0000B24C
		public CodeMatcher SetInstructionAndAdvance(CodeInstruction instruction)
		{
			this.SetInstruction(instruction);
			int pos = this.Pos;
			this.Pos = pos + 1;
			return this;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000D072 File Offset: 0x0000B272
		public unsafe CodeMatcher Set(OpCode opcode, object operand)
		{
			*this.Opcode = opcode;
			*this.Operand = operand;
			return this;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000D08C File Offset: 0x0000B28C
		public CodeMatcher SetAndAdvance(OpCode opcode, object operand)
		{
			this.Set(opcode, operand);
			int pos = this.Pos;
			this.Pos = pos + 1;
			return this;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000D0B4 File Offset: 0x0000B2B4
		public unsafe CodeMatcher SetOpcodeAndAdvance(OpCode opcode)
		{
			*this.Opcode = opcode;
			int pos = this.Pos;
			this.Pos = pos + 1;
			return this;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000D0E0 File Offset: 0x0000B2E0
		public unsafe CodeMatcher SetOperandAndAdvance(object operand)
		{
			*this.Operand = operand;
			int pos = this.Pos;
			this.Pos = pos + 1;
			return this;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000D106 File Offset: 0x0000B306
		public CodeMatcher DeclareLocal(Type variableType, out LocalBuilder localVariable)
		{
			localVariable = this.generator.DeclareLocal(variableType);
			return this;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000D117 File Offset: 0x0000B317
		public CodeMatcher DefineLabel(out Label label)
		{
			label = this.generator.DefineLabel();
			return this;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000D12B File Offset: 0x0000B32B
		public unsafe CodeMatcher CreateLabel(out Label label)
		{
			label = this.generator.DefineLabel();
			this.Labels->Add(label);
			return this;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000D151 File Offset: 0x0000B351
		public CodeMatcher CreateLabelAt(int position, out Label label)
		{
			label = this.generator.DefineLabel();
			this.AddLabelsAt(position, new Label[] { label });
			return this;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000D180 File Offset: 0x0000B380
		public CodeMatcher CreateLabelWithOffsets(int offset, out Label label)
		{
			label = this.generator.DefineLabel();
			return this.AddLabelsAt(this.Pos + offset, new Label[] { label });
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
		public unsafe CodeMatcher AddLabels(IEnumerable<Label> labels)
		{
			this.Labels->AddRange(labels);
			return this;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		public CodeMatcher AddLabelsAt(int position, IEnumerable<Label> labels)
		{
			if (position < 0 || position >= this.Length)
			{
				throw new ArgumentOutOfRangeException("position", "position is out of bounds");
			}
			this.codes[position].labels.AddRange(labels);
			return this;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000D1FB File Offset: 0x0000B3FB
		public CodeMatcher SetJumpTo(OpCode opcode, int destination, out Label label)
		{
			this.CreateLabelAt(destination, out label);
			return this.Set(opcode, label);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000D218 File Offset: 0x0000B418
		public CodeMatcher Insert(params CodeInstruction[] instructions)
		{
			if (this.Pos == this.Length)
			{
				this.codes.AddRange(instructions);
			}
			else
			{
				this.codes.InsertRange(this.PosOrThrow(), instructions);
			}
			return this;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000D249 File Offset: 0x0000B449
		public CodeMatcher Insert(IEnumerable<CodeInstruction> instructions)
		{
			if (this.Pos == this.Length)
			{
				this.codes.AddRange(instructions);
			}
			else
			{
				this.codes.InsertRange(this.PosOrThrow(), instructions);
			}
			return this;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000D27C File Offset: 0x0000B47C
		public CodeMatcher InsertBranch(OpCode opcode, int destination)
		{
			int num = this.PosOrThrow();
			Label label;
			this.CreateLabelAt(destination, out label);
			this.codes.Insert(num, new CodeInstruction(opcode, label));
			return this;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		public CodeMatcher InsertAndAdvance(params CodeInstruction[] instructions)
		{
			foreach (CodeInstruction codeInstruction in instructions)
			{
				this.Insert(new CodeInstruction[] { codeInstruction });
				int pos = this.Pos;
				this.Pos = pos + 1;
			}
			return this;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		public CodeMatcher InsertAndAdvance(IEnumerable<CodeInstruction> instructions)
		{
			foreach (CodeInstruction codeInstruction in instructions)
			{
				this.InsertAndAdvance(new CodeInstruction[] { codeInstruction });
			}
			return this;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000D34C File Offset: 0x0000B54C
		public CodeMatcher InsertBranchAndAdvance(OpCode opcode, int destination)
		{
			this.InsertBranch(opcode, destination);
			int pos = this.Pos;
			this.Pos = pos + 1;
			return this;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000D373 File Offset: 0x0000B573
		public CodeMatcher RemoveInstruction()
		{
			this.codes.RemoveAt(this.PosOrThrow());
			return this;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000D387 File Offset: 0x0000B587
		public CodeMatcher RemoveInstructions(int count)
		{
			this.codes.RemoveRange(this.PosOrThrow(), count);
			return this;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000D39C File Offset: 0x0000B59C
		public CodeMatcher RemoveInstructionsInRange(int start, int end)
		{
			if (start < 0 || start >= this.Length)
			{
				throw new ArgumentOutOfRangeException("start", "start is out of bounds");
			}
			if (end < 0 || end >= this.Length)
			{
				throw new ArgumentOutOfRangeException("end", "end is out of bounds");
			}
			if (start > end)
			{
				int num = start;
				start = end;
				end = num;
			}
			this.codes.RemoveRange(start, end - start + 1);
			return this;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000D3FF File Offset: 0x0000B5FF
		public CodeMatcher RemoveInstructionsWithOffsets(int startOffset, int endOffset)
		{
			return this.RemoveInstructionsInRange(this.Pos + startOffset, this.Pos + endOffset);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000D417 File Offset: 0x0000B617
		public CodeMatcher Advance(int offset)
		{
			this.Pos += offset;
			if (!this.IsValid)
			{
				this.SetOutOfBounds(offset);
			}
			return this;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000D437 File Offset: 0x0000B637
		public CodeMatcher Start()
		{
			this.Pos = 0;
			return this;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000D441 File Offset: 0x0000B641
		public CodeMatcher End()
		{
			this.Pos = this.Length - 1;
			return this;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000D452 File Offset: 0x0000B652
		public CodeMatcher SearchForward(Func<CodeInstruction, bool> predicate)
		{
			return this.Search(predicate, 1);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000D45C File Offset: 0x0000B65C
		[Obsolete("Use SearchBackwards instead")]
		public CodeMatcher SearchBack(Func<CodeInstruction, bool> predicate)
		{
			return this.Search(predicate, -1);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000D466 File Offset: 0x0000B666
		public CodeMatcher SearchBackwards(Func<CodeInstruction, bool> predicate)
		{
			return this.Search(predicate, -1);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000D470 File Offset: 0x0000B670
		private CodeMatcher Search(Func<CodeInstruction, bool> predicate, int direction)
		{
			this.FixStart();
			while (this.IsValid && !predicate(this.Instruction))
			{
				this.Pos += direction;
			}
			string text;
			if (!this.IsInvalid)
			{
				text = null;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot find ");
				defaultInterpolatedStringHandler.AppendFormatted<Func<CodeInstruction, bool>>(predicate);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this.lastError = text;
			return this;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		public CodeMatcher MatchForward(bool useEnd, params CodeMatch[] matches)
		{
			return this.Match(matches, 1, useEnd);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000D4EB File Offset: 0x0000B6EB
		public CodeMatcher MatchBack(bool useEnd, params CodeMatch[] matches)
		{
			return this.Match(matches, -1, useEnd);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000D4F6 File Offset: 0x0000B6F6
		public CodeMatcher MatchStartForward(params CodeMatch[] matches)
		{
			return this.Match(matches, 1, false);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000D501 File Offset: 0x0000B701
		public CodeMatcher MatchEndForward(params CodeMatch[] matches)
		{
			return this.Match(matches, 1, true);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000D50C File Offset: 0x0000B70C
		public CodeMatcher MatchStartBackwards(params CodeMatch[] matches)
		{
			return this.Match(matches, -1, false);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000D517 File Offset: 0x0000B717
		public CodeMatcher MatchEndBackwards(params CodeMatch[] matches)
		{
			return this.Match(matches, -1, true);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000D524 File Offset: 0x0000B724
		private CodeMatcher Match(CodeMatch[] matches, int direction, bool useEnd)
		{
			this.lastMatchCall = delegate
			{
				this.FixStart();
				while (this.IsValid)
				{
					if (this.MatchSequence(this.Pos, matches))
					{
						if (useEnd)
						{
							this.Pos += matches.Length - 1;
							break;
						}
						break;
					}
					else
					{
						this.Pos += direction;
					}
				}
				this.lastError = (this.IsInvalid ? ("Cannot find " + matches.Join<CodeMatch>(null, ", ")) : null);
				return this;
			};
			return this.lastMatchCall();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000D570 File Offset: 0x0000B770
		public CodeMatcher Repeat(Action<CodeMatcher> matchAction, Action<string> notFoundAction = null)
		{
			int num = 0;
			if (this.lastMatchCall == null)
			{
				throw new InvalidOperationException("No previous Match operation - cannot repeat");
			}
			while (this.IsValid)
			{
				matchAction(this);
				this.lastMatchCall();
				num++;
			}
			this.lastMatchCall = null;
			if (num == 0 && notFoundAction != null)
			{
				notFoundAction(this.lastError);
			}
			return this;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000D5CB File Offset: 0x0000B7CB
		public CodeInstruction NamedMatch(string name)
		{
			return this.lastMatches[name];
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000D5DC File Offset: 0x0000B7DC
		private bool MatchSequence(int start, CodeMatch[] matches)
		{
			if (start < 0)
			{
				return false;
			}
			this.lastMatches = new Dictionary<string, CodeInstruction>();
			foreach (CodeMatch codeMatch in matches)
			{
				if (start >= this.Length || !codeMatch.Matches(this.codes, this.codes[start]))
				{
					return false;
				}
				if (codeMatch.name != null)
				{
					this.lastMatches.Add(codeMatch.name, this.codes[start]);
				}
				start++;
			}
			return true;
		}

		// Token: 0x040000B3 RID: 179
		private readonly ILGenerator generator;

		// Token: 0x040000B4 RID: 180
		private readonly List<CodeInstruction> codes = new List<CodeInstruction>();

		// Token: 0x040000B6 RID: 182
		private Dictionary<string, CodeInstruction> lastMatches = new Dictionary<string, CodeInstruction>();

		// Token: 0x040000B7 RID: 183
		private string lastError;

		// Token: 0x040000B8 RID: 184
		private CodeMatcher.MatchDelegate lastMatchCall;

		// Token: 0x020001BE RID: 446
		// (Invoke) Token: 0x06000799 RID: 1945
		private delegate CodeMatcher MatchDelegate();
	}
}
