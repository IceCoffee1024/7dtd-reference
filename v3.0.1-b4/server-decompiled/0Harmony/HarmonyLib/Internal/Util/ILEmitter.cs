using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MonoMod.Utils;

namespace HarmonyLib.Internal.Util
{
	// Token: 0x02000062 RID: 98
	internal class ILEmitter
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x00014965 File Offset: 0x00012B65
		public ILEmitter(ILProcessor il)
		{
			this.IL = il;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0001498A File Offset: 0x00012B8A
		private Instruction Target
		{
			get
			{
				return this.emitBefore ?? this.IL.Body.Instructions[this.IL.Body.Instructions.Count - 1];
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000149C2 File Offset: 0x00012BC2
		public ILEmitter.ExceptionBlock BeginExceptionBlock(ILEmitter.Label start)
		{
			return new ILEmitter.ExceptionBlock
			{
				start = start
			};
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000149D0 File Offset: 0x00012BD0
		public void EndExceptionBlock(ILEmitter.ExceptionBlock block)
		{
			this.EndHandler(block, block.cur);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000149E0 File Offset: 0x00012BE0
		public void BeginHandler(ILEmitter.ExceptionBlock block, ExceptionHandlerType handlerType, Type exceptionType = null)
		{
			ILEmitter.LabelledExceptionHandler labelledExceptionHandler = (block.prev = block.cur);
			if (labelledExceptionHandler != null)
			{
				this.EndHandler(block, labelledExceptionHandler);
			}
			block.skip = this.DeclareLabel();
			this.Emit(OpCodes.Leave, block.skip);
			ILEmitter.Label label = this.DeclareLabel();
			this.MarkLabel(label);
			block.cur = new ILEmitter.LabelledExceptionHandler
			{
				tryStart = block.start,
				tryEnd = label,
				handlerType = handlerType,
				handlerEnd = block.skip,
				exceptionType = ((exceptionType != null) ? this.IL.Import(exceptionType) : null)
			};
			if (handlerType == ExceptionHandlerType.Filter)
			{
				block.cur.filterStart = label;
				return;
			}
			block.cur.handlerStart = label;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00014AA0 File Offset: 0x00012CA0
		public void EndHandler(ILEmitter.ExceptionBlock block, ILEmitter.LabelledExceptionHandler handler)
		{
			ExceptionHandlerType handlerType = handler.handlerType;
			if (handlerType != ExceptionHandlerType.Filter)
			{
				if (handlerType != ExceptionHandlerType.Finally)
				{
					this.Emit(OpCodes.Leave, block.skip);
				}
				else
				{
					this.Emit(OpCodes.Endfinally);
				}
			}
			else
			{
				this.Emit(OpCodes.Endfilter);
			}
			this.MarkLabel(block.skip);
			this.pendingExceptionHandlers.Add(block.cur);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00014B08 File Offset: 0x00012D08
		public VariableDefinition DeclareVariable(Type type)
		{
			VariableDefinition variableDefinition = new VariableDefinition(this.IL.Import(type));
			this.IL.Body.Variables.Add(variableDefinition);
			return variableDefinition;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00014B3E File Offset: 0x00012D3E
		public ILEmitter.Label DeclareLabel()
		{
			return new ILEmitter.Label();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00014B45 File Offset: 0x00012D45
		public ILEmitter.Label DeclareLabelFor(Instruction ins)
		{
			return new ILEmitter.Label
			{
				emitted = true,
				instruction = ins
			};
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00014B5A File Offset: 0x00012D5A
		public void MarkLabel(ILEmitter.Label label)
		{
			if (label.emitted)
			{
				return;
			}
			this.pendingLabels.Add(label);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00014B74 File Offset: 0x00012D74
		public Instruction SetOpenLabelsTo(Instruction ins)
		{
			if (this.pendingLabels.Count != 0)
			{
				foreach (ILEmitter.Label label in this.pendingLabels)
				{
					foreach (Instruction instruction in label.targets)
					{
						if (instruction.Operand is Instruction)
						{
							instruction.Operand = ins;
						}
						else
						{
							Instruction[] array = instruction.Operand as Instruction[];
							if (array != null)
							{
								for (int i = 0; i < array.Length; i++)
								{
									if (array[i] == label.instruction)
									{
										array[i] = ins;
										break;
									}
								}
							}
						}
					}
					label.instruction = ins;
					label.emitted = true;
				}
				this.pendingLabels.Clear();
			}
			if (this.pendingExceptionHandlers.Count != 0)
			{
				foreach (ILEmitter.LabelledExceptionHandler labelledExceptionHandler in this.pendingExceptionHandlers)
				{
					Collection<ExceptionHandler> exceptionHandlers = this.IL.Body.ExceptionHandlers;
					ExceptionHandler exceptionHandler = new ExceptionHandler(labelledExceptionHandler.handlerType);
					ILEmitter.Label tryStart = labelledExceptionHandler.tryStart;
					exceptionHandler.TryStart = ((tryStart != null) ? tryStart.instruction : null);
					ILEmitter.Label tryEnd = labelledExceptionHandler.tryEnd;
					exceptionHandler.TryEnd = ((tryEnd != null) ? tryEnd.instruction : null);
					ILEmitter.Label filterStart = labelledExceptionHandler.filterStart;
					exceptionHandler.FilterStart = ((filterStart != null) ? filterStart.instruction : null);
					ILEmitter.Label handlerStart = labelledExceptionHandler.handlerStart;
					exceptionHandler.HandlerStart = ((handlerStart != null) ? handlerStart.instruction : null);
					ILEmitter.Label handlerEnd = labelledExceptionHandler.handlerEnd;
					exceptionHandler.HandlerEnd = ((handlerEnd != null) ? handlerEnd.instruction : null);
					exceptionHandler.CatchType = labelledExceptionHandler.exceptionType;
					exceptionHandlers.Add(exceptionHandler);
				}
				this.pendingExceptionHandlers.Clear();
			}
			return ins;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00014D84 File Offset: 0x00012F84
		public void Emit(OpCode opcode)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode)));
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00014DAC File Offset: 0x00012FAC
		public void Emit(OpCode opcode, ILEmitter.Label label)
		{
			Instruction instruction = this.SetOpenLabelsTo(this.IL.Create(opcode, label.instruction));
			label.targets.Add(instruction);
			this.IL.InsertBefore(this.Target, instruction);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00014DF0 File Offset: 0x00012FF0
		public void Emit(OpCode opcode, ConstructorInfo cInfo)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, this.IL.Import(cInfo))));
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00014E21 File Offset: 0x00013021
		public void Emit(OpCode opcode, MethodInfo mInfo)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, this.IL.Import(mInfo))));
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00014E52 File Offset: 0x00013052
		public void Emit(OpCode opcode, Type cls)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, this.IL.Import(cls))));
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00014E83 File Offset: 0x00013083
		public void EmitUnsafe(OpCode opcode, object arg)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, arg)));
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00014EA9 File Offset: 0x000130A9
		public void Emit(OpCode opcode, int arg)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, arg)));
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00014ECF File Offset: 0x000130CF
		public void Emit(OpCode opcode, string arg)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, arg)));
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00014EF5 File Offset: 0x000130F5
		public void Emit(OpCode opcode, FieldInfo fInfo)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, this.IL.Import(fInfo))));
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00014F26 File Offset: 0x00013126
		public void Emit(OpCode opcode, VariableDefinition varDef)
		{
			this.IL.InsertBefore(this.Target, this.SetOpenLabelsTo(this.IL.Create(opcode, varDef)));
		}

		// Token: 0x04000115 RID: 277
		public readonly ILProcessor IL;

		// Token: 0x04000116 RID: 278
		private readonly List<ILEmitter.LabelledExceptionHandler> pendingExceptionHandlers = new List<ILEmitter.LabelledExceptionHandler>();

		// Token: 0x04000117 RID: 279
		private readonly List<ILEmitter.Label> pendingLabels = new List<ILEmitter.Label>();

		// Token: 0x04000118 RID: 280
		public Instruction emitBefore;

		// Token: 0x020001DE RID: 478
		public class Label
		{
			// Token: 0x04000292 RID: 658
			public bool emitted;

			// Token: 0x04000293 RID: 659
			public Instruction instruction = Instruction.Create(OpCodes.Nop);

			// Token: 0x04000294 RID: 660
			public List<Instruction> targets = new List<Instruction>();
		}

		// Token: 0x020001DF RID: 479
		public class ExceptionBlock
		{
			// Token: 0x04000295 RID: 661
			public ILEmitter.LabelledExceptionHandler prev;

			// Token: 0x04000296 RID: 662
			public ILEmitter.LabelledExceptionHandler cur;

			// Token: 0x04000297 RID: 663
			public ILEmitter.Label start;

			// Token: 0x04000298 RID: 664
			public ILEmitter.Label skip;
		}

		// Token: 0x020001E0 RID: 480
		public class LabelledExceptionHandler
		{
			// Token: 0x04000299 RID: 665
			public TypeReference exceptionType;

			// Token: 0x0400029A RID: 666
			public ExceptionHandlerType handlerType;

			// Token: 0x0400029B RID: 667
			public ILEmitter.Label tryStart;

			// Token: 0x0400029C RID: 668
			public ILEmitter.Label tryEnd;

			// Token: 0x0400029D RID: 669
			public ILEmitter.Label filterStart;

			// Token: 0x0400029E RID: 670
			public ILEmitter.Label handlerStart;

			// Token: 0x0400029F RID: 671
			public ILEmitter.Label handlerEnd;
		}
	}
}
