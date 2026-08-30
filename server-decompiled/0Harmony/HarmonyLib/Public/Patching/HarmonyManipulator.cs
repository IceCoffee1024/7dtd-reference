using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib.Internal.Patching;
using HarmonyLib.Internal.RuntimeFixes;
using HarmonyLib.Internal.Util;
using HarmonyLib.Tools;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace HarmonyLib.Public.Patching
{
	// Token: 0x0200005A RID: 90
	public class HarmonyManipulator
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0000FE6C File Offset: 0x0000E06C
		private Type ReturnType
		{
			get
			{
				Type type;
				if ((type = this._returnType) == null)
				{
					type = (this._returnType = AccessTools.GetReturnedType(this.original));
				}
				return type;
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000FE97 File Offset: 0x0000E097
		public HarmonyManipulator(PatchInfo patchInfo)
		{
			this.patchInfo = patchInfo;
			this.debug = patchInfo.Debugging;
			this.dumpPaths = patchInfo.DebugEmitPaths;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000FECC File Offset: 0x0000E0CC
		public static void Manipulate(MethodBase original, PatchInfo patchInfo, ILContext ctx)
		{
			HarmonyManipulator harmonyManipulator = new HarmonyManipulator(patchInfo);
			harmonyManipulator.Process(ctx, original);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000FEE8 File Offset: 0x0000E0E8
		public static void Manipulate(MethodBase original, ILContext ctx)
		{
			HarmonyManipulator.Manipulate(original, original.GetPatchInfo(), ctx);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		private static void LogPatchException(object errorObject, string patch)
		{
			Logger.LogChannel logChannel = Logger.LogChannel.Error;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Error while running ");
			defaultInterpolatedStringHandler.AppendFormatted(patch);
			defaultInterpolatedStringHandler.AppendLiteral(". Error: ");
			defaultInterpolatedStringHandler.AppendFormatted<object>(errorObject);
			Logger.LogText(logChannel, defaultInterpolatedStringHandler.ToStringAndClear(), false);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000FF48 File Offset: 0x0000E148
		public void Process(ILContext ilContext, MethodBase originalMethod)
		{
			this.ctx = ilContext;
			this.original = originalMethod;
			this.il = new ILEmitter(this.ctx.IL);
			this.SortPatches();
			Logger.Log(Logger.LogChannel.Info, delegate
			{
				HarmonyManipulator.<>c__DisplayClass32_0 CS$<>8__locals1;
				CS$<>8__locals1.sb = new StringBuilder();
				StringBuilder sb = CS$<>8__locals1.sb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 5);
				defaultInterpolatedStringHandler.AppendLiteral("Patching ");
				defaultInterpolatedStringHandler.AppendFormatted(this.original.FullDescription());
				defaultInterpolatedStringHandler.AppendLiteral(" with ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.prefixes.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" prefixes, ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.postfixes.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" postfixes, ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.transpilers.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" transpilers, ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.finalizers.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" finalizers");
				sb.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				HarmonyManipulator.<Process>g__Print|32_1(this.prefixes, "prefixes", ref CS$<>8__locals1);
				HarmonyManipulator.<Process>g__Print|32_1(this.postfixes, "postfixes", ref CS$<>8__locals1);
				HarmonyManipulator.<Process>g__Print|32_1(this.transpilers, "transpilers", ref CS$<>8__locals1);
				HarmonyManipulator.<Process>g__Print|32_1(this.finalizers, "finalizers", ref CS$<>8__locals1);
				HarmonyManipulator.<Process>g__Print|32_1(this.ilManipulators, "ilmanipulators", ref CS$<>8__locals1);
				return CS$<>8__locals1.sb.ToString();
			}, this.debug);
			this.WriteImpl();
			if (this.dumpPaths.Length != 0)
			{
				CecilEmitter.Dump(this.ctx.Method, this.dumpPaths, this.original);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000FFC4 File Offset: 0x0000E1C4
		private bool WritePrefixes(ILEmitter.Label returnLabel)
		{
			if (this.prefixes.Count == 0)
			{
				return false;
			}
			Logger.Log(Logger.LogChannel.Info, () => "Writing prefixes", this.debug);
			this.il.emitBefore = this.il.IL.Body.Instructions[0];
			Type returnType = this.ReturnType;
			VariableDefinition variableDefinition;
			if (!this.variables.TryGetValue(HarmonyManipulator.ResultVar, out variableDefinition))
			{
				variableDefinition = (this.variables[HarmonyManipulator.ResultVar] = ((returnType == typeof(void)) ? null : this.il.DeclareVariable(returnType)));
			}
			if (variableDefinition != null && returnType.IsByRef)
			{
				this.il.Emit(OpCodes.Ldc_I4_1);
				this.il.Emit(OpCodes.Newarr, returnType.GetElementType());
				this.il.Emit(OpCodes.Ldc_I4_0);
				this.il.Emit(OpCodes.Ldelema, returnType.GetElementType());
				this.il.Emit(OpCodes.Stloc, variableDefinition);
			}
			bool flag = this.prefixes.Any<HarmonyManipulator.PatchContext>(delegate(HarmonyManipulator.PatchContext p)
			{
				if (!(p.method.ReturnType == typeof(bool)))
				{
					return p.method.GetParameters().Any<ParameterInfo>((ParameterInfo pp) => pp.Name == HarmonyManipulator.RunOriginalParam && pp.ParameterType.OpenRefType() == typeof(bool));
				}
				return true;
			});
			VariableDefinition variableDefinition2 = (this.variables[HarmonyManipulator.RunOriginalParam] = this.il.DeclareVariable(typeof(bool)));
			this.il.Emit(OpCodes.Ldc_I4_1);
			this.il.Emit(OpCodes.Stloc, variableDefinition2);
			ILEmitter.Label label = ((variableDefinition != null) ? this.il.DeclareLabel() : returnLabel);
			foreach (HarmonyManipulator.PatchContext patchContext in this.prefixes)
			{
				MethodInfo method = patchContext.method;
				ILEmitter.Label label2 = this.il.DeclareLabel();
				this.il.MarkLabel(label2);
				VariableDefinition variableDefinition3;
				VariableDefinition variableDefinition4;
				MethodInfo methodInfo;
				List<HarmonyManipulator.ArgumentBoxInfo> list;
				this.EmitCallParameter(method, false, out variableDefinition3, out variableDefinition4, out methodInfo, out list);
				this.il.Emit(OpCodes.Call, method);
				if (patchContext.hasArgsArrayArg)
				{
					this.EmitAssignRefsFromArgsArray(this.variables[HarmonyManipulator.ArgsArrayVar]);
				}
				this.EmitInstanceUnbox(variableDefinition3, this.original.DeclaringType);
				this.EmitResultRefInvoke(methodInfo);
				this.EmitResultUnbox(variableDefinition4, variableDefinition);
				this.EmitArgUnbox(list);
				if (!AccessTools.IsVoid(method.ReturnType))
				{
					if (method.ReturnType != typeof(bool))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Prefix patch ");
						defaultInterpolatedStringHandler.AppendFormatted(method.FullDescription());
						defaultInterpolatedStringHandler.AppendLiteral(" has return type ");
						defaultInterpolatedStringHandler.AppendFormatted<Type>(method.ReturnType);
						defaultInterpolatedStringHandler.AppendLiteral(", but only `bool` or `void` are permitted");
						throw new InvalidHarmonyPatchArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), this.original, method);
					}
					if (flag)
					{
						this.il.Emit(OpCodes.Ldloc, variableDefinition2);
						this.il.Emit(OpCodes.And);
						this.il.Emit(OpCodes.Stloc, variableDefinition2);
					}
				}
				if (patchContext.wrapTryCatch)
				{
					this.EmitTryCatchWrapper(method, label2);
				}
			}
			if (!flag)
			{
				return false;
			}
			this.il.Emit(OpCodes.Ldloc, variableDefinition2);
			this.il.Emit(OpCodes.Brfalse, label);
			if (variableDefinition == null)
			{
				return true;
			}
			this.il.emitBefore = this.il.IL.Body.Instructions[this.il.IL.Body.Instructions.Count - 1];
			this.il.MarkLabel(label);
			this.il.Emit(OpCodes.Ldloc, variableDefinition);
			return true;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000103B8 File Offset: 0x0000E5B8
		private void EmitTryCatchWrapper(MethodBase target, ILEmitter.Label start)
		{
			ILEmitter.ExceptionBlock exceptionBlock = this.il.BeginExceptionBlock(start);
			this.il.BeginHandler(exceptionBlock, ExceptionHandlerType.Catch, typeof(object));
			this.il.Emit(OpCodes.Ldstr, target.FullDescription());
			this.il.Emit(OpCodes.Call, HarmonyManipulator.LogPatchExceptionMethod);
			this.il.EndExceptionBlock(exceptionBlock);
			this.il.Emit(OpCodes.Nop);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00010430 File Offset: 0x0000E630
		private void EmitArgUnbox(List<HarmonyManipulator.ArgumentBoxInfo> boxInfo)
		{
			if (boxInfo == null)
			{
				return;
			}
			foreach (HarmonyManipulator.ArgumentBoxInfo argumentBoxInfo in boxInfo)
			{
				if (argumentBoxInfo.isByRef)
				{
					this.il.Emit(OpCodes.Ldarg, argumentBoxInfo.index);
				}
				this.il.Emit(OpCodes.Ldloc, argumentBoxInfo.tmpVar);
				this.il.Emit(OpCodes.Unbox_Any, argumentBoxInfo.type);
				if (argumentBoxInfo.isByRef)
				{
					this.il.Emit(OpCodes.Stobj, argumentBoxInfo.type);
				}
				else
				{
					this.il.Emit(OpCodes.Starg, argumentBoxInfo.index);
				}
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010504 File Offset: 0x0000E704
		private void EmitResultUnbox(VariableDefinition tmp, VariableDefinition result)
		{
			if (tmp == null)
			{
				return;
			}
			this.il.Emit(OpCodes.Ldloc, tmp);
			this.il.Emit(OpCodes.Unbox_Any, this.ReturnType);
			this.il.Emit(OpCodes.Stloc, result);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00010544 File Offset: 0x0000E744
		private void EmitResultRefInvoke(MethodInfo resultRefInvoke)
		{
			if (resultRefInvoke == null)
			{
				return;
			}
			VariableDefinition variableDefinition = this.variables[HarmonyManipulator.ResultRefVar];
			ILEmitter.Label label = this.il.DeclareLabel();
			this.il.Emit(OpCodes.Ldloc, variableDefinition);
			this.il.Emit(OpCodes.Brfalse_S, label);
			this.il.Emit(OpCodes.Ldloc, variableDefinition);
			this.il.Emit(OpCodes.Callvirt, resultRefInvoke);
			this.il.Emit(OpCodes.Stloc, this.variables[HarmonyManipulator.ResultVar]);
			this.il.Emit(OpCodes.Ldnull);
			this.il.Emit(OpCodes.Stloc, variableDefinition);
			this.il.MarkLabel(label);
			this.il.Emit(OpCodes.Nop);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00010614 File Offset: 0x0000E814
		private void EmitInstanceUnbox(VariableDefinition box, Type declaringType)
		{
			if (box == null)
			{
				return;
			}
			this.il.Emit(OpCodes.Ldarg_0);
			this.il.Emit(OpCodes.Ldloc, box);
			this.il.Emit(OpCodes.Unbox_Any, declaringType);
			this.il.Emit(OpCodes.Stobj, declaringType);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00010668 File Offset: 0x0000E868
		private void WriteTranspilers()
		{
			if (this.transpilers.Count == 0)
			{
				return;
			}
			Logger.Log(Logger.LogChannel.Info, () => "Transpiling " + this.original.FullDescription(), this.debug);
			ILManipulator ilmanipulator = new ILManipulator(this.ctx.Body, this.debug);
			foreach (HarmonyManipulator.PatchContext patchContext in this.transpilers)
			{
				ilmanipulator.AddTranspiler(patchContext.method);
			}
			ilmanipulator.WriteTo(this.ctx.Body, this.original);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00010714 File Offset: 0x0000E914
		private ValueTuple<ILEmitter.Label, bool> MakeReturnLabel()
		{
			if (this.ctx.IL.Body.Instructions.Count == 0)
			{
				this.il.Emit(OpCodes.Nop);
			}
			ILEmitter.Label label = this.il.DeclareLabel();
			label.emitted = false;
			bool flag = false;
			foreach (Instruction instruction in this.il.IL.Body.Instructions.Where<Instruction>((Instruction ins) => ins.MatchRet()))
			{
				flag = true;
				instruction.OpCode = OpCodes.Br;
				instruction.Operand = label.instruction;
				label.targets.Add(instruction);
			}
			label.instruction = Instruction.Create(OpCodes.Ret);
			this.il.IL.Append(label.instruction);
			return new ValueTuple<ILEmitter.Label, bool>(label, flag);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00010820 File Offset: 0x0000EA20
		private void WriteImpl()
		{
			try
			{
				if (this.original == null)
				{
					throw new ArgumentException("original");
				}
				Logger.Log(Logger.LogChannel.Info, () => "Running ILHook manipulator on " + this.original.FullDescription(), this.debug);
				this.WriteTranspilers();
				if (this.prefixes.Count + this.postfixes.Count + this.finalizers.Count + this.ilManipulators.Count == 0)
				{
					Logger.Log(Logger.LogChannel.IL, () => "Generated patch (" + this.ctx.Method.FullName + "):\n" + this.ctx.Body.ToILDasmString(), this.debug);
				}
				else
				{
					ValueTuple<ILEmitter.Label, bool> valueTuple = this.MakeReturnLabel();
					ILEmitter.Label item = valueTuple.Item1;
					bool item2 = valueTuple.Item2;
					IEnumerable<HarmonyManipulator.PatchContext> enumerable = this.prefixes.Union<HarmonyManipulator.PatchContext>(this.postfixes).Union<HarmonyManipulator.PatchContext>(this.finalizers);
					foreach (HarmonyManipulator.PatchContext patchContext in enumerable)
					{
						ParameterInfo[] parameters = patchContext.method.GetParameters();
						ParameterInfo parameterInfo = parameters.FirstOrDefault<ParameterInfo>((ParameterInfo p) => p.Name == HarmonyManipulator.ArgsArrayVar);
						if (parameterInfo != null)
						{
							if (parameterInfo.ParameterType != typeof(object[]))
							{
								throw new InvalidHarmonyPatchArgumentException("Patch " + patchContext.method.FullDescription() + " defines __args list, but only type `object[]` is permitted", this.original, patchContext.method);
							}
							this.variables[HarmonyManipulator.ArgsArrayVar] = this.il.DeclareVariable(typeof(object[]));
						}
						Type declaringType = patchContext.method.DeclaringType;
						if (((declaringType != null) ? declaringType.FullName : null) != null && !this.variables.ContainsKey(patchContext.method.DeclaringType.FullName))
						{
							foreach (ParameterInfo parameterInfo2 in parameters.Where<ParameterInfo>((ParameterInfo patchParam) => patchParam.Name == HarmonyManipulator.StateVar))
							{
								this.variables[patchContext.method.DeclaringType.FullName] = this.il.DeclareVariable(parameterInfo2.ParameterType.OpenRefType());
							}
						}
					}
					bool flag = this.WritePrefixes(item);
					this.WritePostfixes(item, item2 || flag);
					this.WriteFinalizers(item, item2);
					if (!item2 && !flag && this.postfixes.Count == 0 && this.finalizers.Count == 0)
					{
						this.ctx.Instrs.RemoveAt(this.ctx.Instrs.Count - 1);
					}
					this.il.MarkLabel(item);
					this.il.SetOpenLabelsTo(this.ctx.Instrs[this.ctx.Instrs.Count - 1]);
					VariableDefinition variableDefinition;
					if (this.variables.TryGetValue(HarmonyManipulator.ArgsArrayVar, out variableDefinition))
					{
						this.il.emitBefore = this.ctx.Instrs[0];
						this.EmitInitArgsArray();
						this.il.Emit(OpCodes.Stloc, variableDefinition);
					}
					HarmonyManipulator.ApplyManipulators(this.ctx, this.original, this.ilManipulators.Select<HarmonyManipulator.PatchContext, MethodInfo>((HarmonyManipulator.PatchContext m) => m.method).ToList<MethodInfo>(), item);
					StackTraceFixes.FixStackTrace(this.ctx);
					Logger.Log(Logger.LogChannel.IL, () => "Generated patch (" + this.ctx.Method.FullName + "):\n" + this.ctx.Body.ToILDasmString(), this.debug);
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception e = ex2;
				Logger.Log(Logger.LogChannel.Error, delegate
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to patch ");
					defaultInterpolatedStringHandler.AppendFormatted(this.original.FullDescription());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}, this.debug);
				throw HarmonyException.Create(e, this.ctx.Body);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00010C54 File Offset: 0x0000EE54
		internal static void ApplyManipulators(ILContext ctx, MethodBase original, List<MethodInfo> ilManipulators, ILEmitter.Label retLabel)
		{
			ILLabel illabel = ctx.DefineLabel((retLabel != null) ? retLabel.instruction : null) ?? ctx.DefineLabel(ctx.Body.Instructions.Last<Instruction>());
			foreach (MethodInfo methodInfo in ilManipulators)
			{
				List<object> list = new List<object>();
				foreach (Type type in from p in methodInfo.GetParameters()
					select p.ParameterType)
				{
					if (type.IsAssignableFrom(typeof(ILContext)))
					{
						list.Add(ctx);
					}
					if (type.IsAssignableFrom(typeof(MethodBase)))
					{
						list.Add(original);
					}
					if (type.IsAssignableFrom(typeof(ILLabel)))
					{
						list.Add(illabel);
					}
				}
				methodInfo.Invoke(null, list.ToArray());
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00010D90 File Offset: 0x0000EF90
		private void EmitOutParameter(int arg, Type t)
		{
			t = t.OpenRefType();
			this.il.Emit(OpCodes.Ldarg, arg);
			if (AccessTools.IsStruct(t))
			{
				this.il.Emit(OpCodes.Initobj, t);
				return;
			}
			if (AccessTools.IsValue(t))
			{
				ValueTuple<double, OpCode, OpCode> valueTuple;
				if (t == typeof(float))
				{
					valueTuple = new ValueTuple<double, OpCode, OpCode>(0.0, OpCodes.Ldc_R4, OpCodes.Stind_R4);
				}
				else if (t == typeof(double))
				{
					valueTuple = new ValueTuple<double, OpCode, OpCode>(0.0, OpCodes.Ldc_R8, OpCodes.Stind_R8);
				}
				else if (t == typeof(long))
				{
					valueTuple = new ValueTuple<double, OpCode, OpCode>(0.0, OpCodes.Ldc_I8, OpCodes.Stind_I8);
				}
				else
				{
					valueTuple = new ValueTuple<double, OpCode, OpCode>(0.0, OpCodes.Ldc_I4, t.GetCecilStoreOpCode());
				}
				ValueTuple<double, OpCode, OpCode> valueTuple2 = valueTuple;
				double item = valueTuple2.Item1;
				OpCode item2 = valueTuple2.Item2;
				OpCode item3 = valueTuple2.Item3;
				this.il.EmitUnsafe(item2, item);
				this.il.Emit(item3);
				return;
			}
			this.il.Emit(OpCodes.Ldnull);
			this.il.Emit(OpCodes.Stind_Ref);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00010EDC File Offset: 0x0000F0DC
		private void EmitInitArgsArray()
		{
			ParameterInfo[] parameters = this.original.GetParameters();
			int num = ((!this.original.IsStatic) ? 1 : 0);
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				if (parameterInfo.IsOut || parameterInfo.IsRetval)
				{
					this.EmitOutParameter(i + num, parameterInfo.ParameterType.OpenRefType());
				}
			}
			this.il.Emit(OpCodes.Ldc_I4, parameters.Length);
			this.il.Emit(OpCodes.Newarr, typeof(object));
			for (int j = 0; j < parameters.Length; j++)
			{
				ParameterInfo parameterInfo2 = parameters[j];
				Type parameterType = parameterInfo2.ParameterType;
				Type type = parameterType.OpenRefType();
				this.il.Emit(OpCodes.Dup);
				this.il.Emit(OpCodes.Ldc_I4, j);
				this.il.Emit(OpCodes.Ldarg, j + num);
				if (parameterType.IsByRef)
				{
					if (AccessTools.IsStruct(type))
					{
						this.il.Emit(OpCodes.Ldobj, type);
					}
					else
					{
						this.il.Emit(type.GetCecilLoadOpCode());
					}
				}
				if (type.IsValueType)
				{
					this.il.Emit(OpCodes.Box, type);
				}
				this.il.Emit(OpCodes.Stelem_Ref);
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00011030 File Offset: 0x0000F230
		private bool WriteFinalizers(ILEmitter.Label returnLabel, bool originalHasReturn)
		{
			HarmonyManipulator.<>c__DisplayClass45_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (this.finalizers.Count == 0)
			{
				return false;
			}
			Logger.Log(Logger.LogChannel.Info, () => "Writing finalizers", this.debug);
			this.variables[HarmonyManipulator.ExceptionVar] = this.il.DeclareVariable(typeof(Exception));
			VariableDefinition variableDefinition = this.il.DeclareVariable(typeof(bool));
			this.il.emitBefore = this.il.IL.Body.Instructions[0];
			this.il.Emit(OpCodes.Ldc_I4_0);
			this.il.Emit(OpCodes.Stloc, variableDefinition);
			this.il.emitBefore = this.il.IL.Body.Instructions[this.il.IL.Body.Instructions.Count - 1];
			this.il.MarkLabel(returnLabel);
			if (!this.variables.TryGetValue(HarmonyManipulator.ResultVar, out CS$<>8__locals1.returnValueVar))
			{
				Type returnType = this.ReturnType;
				CS$<>8__locals1.returnValueVar = (this.variables[HarmonyManipulator.ResultVar] = ((returnType == typeof(void)) ? null : this.il.DeclareVariable(returnType)));
			}
			ILEmitter.ExceptionBlock exceptionBlock = this.il.BeginExceptionBlock(this.il.DeclareLabelFor(this.il.IL.Body.Instructions[0]));
			if (CS$<>8__locals1.returnValueVar != null && originalHasReturn)
			{
				this.il.Emit(OpCodes.Stloc, CS$<>8__locals1.returnValueVar);
			}
			this.<WriteFinalizers>g__WriteFinalizerCalls|45_1(false, ref CS$<>8__locals1);
			this.il.Emit(OpCodes.Ldc_I4_1);
			this.il.Emit(OpCodes.Stloc, variableDefinition);
			ILEmitter.Label label = this.il.DeclareLabel();
			this.il.Emit(OpCodes.Ldloc, this.variables[HarmonyManipulator.ExceptionVar]);
			this.il.Emit(OpCodes.Brfalse, label);
			this.il.Emit(OpCodes.Ldloc, this.variables[HarmonyManipulator.ExceptionVar]);
			this.il.Emit(OpCodes.Throw);
			this.il.MarkLabel(label);
			this.il.BeginHandler(exceptionBlock, ExceptionHandlerType.Catch, typeof(Exception));
			this.il.Emit(OpCodes.Stloc, this.variables[HarmonyManipulator.ExceptionVar]);
			this.il.Emit(OpCodes.Ldloc, variableDefinition);
			ILEmitter.Label label2 = this.il.DeclareLabel();
			this.il.Emit(OpCodes.Brtrue, label2);
			bool flag = this.<WriteFinalizers>g__WriteFinalizerCalls|45_1(true, ref CS$<>8__locals1);
			this.il.MarkLabel(label2);
			label = this.il.DeclareLabel();
			this.il.Emit(OpCodes.Ldloc, this.variables[HarmonyManipulator.ExceptionVar]);
			this.il.Emit(OpCodes.Brfalse, label);
			if (flag)
			{
				this.il.Emit(OpCodes.Rethrow);
			}
			else
			{
				this.il.Emit(OpCodes.Ldloc, this.variables[HarmonyManipulator.ExceptionVar]);
				this.il.Emit(OpCodes.Throw);
			}
			this.il.MarkLabel(label);
			this.il.EndExceptionBlock(exceptionBlock);
			if (CS$<>8__locals1.returnValueVar != null)
			{
				this.il.Emit(OpCodes.Ldloc, CS$<>8__locals1.returnValueVar);
			}
			return true;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000113E0 File Offset: 0x0000F5E0
		private void WritePostfixes(ILEmitter.Label returnLabel, bool emitResultStore)
		{
			if (this.postfixes.Count == 0)
			{
				return;
			}
			Logger.Log(Logger.LogChannel.Info, () => "Writing postfixes", this.debug);
			this.il.emitBefore = this.il.IL.Body.Instructions[this.il.IL.Body.Instructions.Count - 1];
			this.il.MarkLabel(returnLabel);
			VariableDefinition variableDefinition;
			if (!this.variables.TryGetValue(HarmonyManipulator.ResultVar, out variableDefinition))
			{
				Type returnType = this.ReturnType;
				variableDefinition = (this.variables[HarmonyManipulator.ResultVar] = ((returnType == typeof(void)) ? null : this.il.DeclareVariable(returnType)));
			}
			if (variableDefinition != null && emitResultStore)
			{
				this.il.Emit(OpCodes.Stloc, variableDefinition);
			}
			if (!this.variables.ContainsKey(HarmonyManipulator.RunOriginalParam))
			{
				VariableDefinition variableDefinition2 = (this.variables[HarmonyManipulator.RunOriginalParam] = this.il.DeclareVariable(typeof(bool)));
				this.il.Emit(OpCodes.Ldc_I4_1);
				this.il.Emit(OpCodes.Stloc, variableDefinition2);
			}
			foreach (HarmonyManipulator.PatchContext patchContext in this.postfixes.Where<HarmonyManipulator.PatchContext>((HarmonyManipulator.PatchContext p) => p.method.ReturnType == typeof(void)))
			{
				MethodInfo method = patchContext.method;
				ILEmitter.Label label = this.il.DeclareLabel();
				this.il.MarkLabel(label);
				VariableDefinition variableDefinition3;
				VariableDefinition variableDefinition4;
				MethodInfo methodInfo;
				List<HarmonyManipulator.ArgumentBoxInfo> list;
				this.EmitCallParameter(method, true, out variableDefinition3, out variableDefinition4, out methodInfo, out list);
				this.il.Emit(OpCodes.Call, method);
				if (patchContext.hasArgsArrayArg)
				{
					this.EmitAssignRefsFromArgsArray(this.variables[HarmonyManipulator.ArgsArrayVar]);
				}
				this.EmitInstanceUnbox(variableDefinition3, this.original.DeclaringType);
				this.EmitResultRefInvoke(methodInfo);
				this.EmitResultUnbox(variableDefinition4, variableDefinition);
				this.EmitArgUnbox(list);
				if (patchContext.wrapTryCatch)
				{
					this.EmitTryCatchWrapper(method, label);
				}
			}
			if (variableDefinition != null)
			{
				this.il.Emit(OpCodes.Ldloc, variableDefinition);
			}
			foreach (HarmonyManipulator.PatchContext patchContext2 in this.postfixes.Where<HarmonyManipulator.PatchContext>((HarmonyManipulator.PatchContext p) => p.method.ReturnType != typeof(void)))
			{
				MethodInfo method2 = patchContext2.method;
				if (patchContext2.wrapTryCatch)
				{
					this.il.Emit(OpCodes.Stloc, variableDefinition);
				}
				ILEmitter.Label label2 = this.il.DeclareLabel();
				this.il.MarkLabel(label2);
				if (patchContext2.wrapTryCatch)
				{
					this.il.Emit(OpCodes.Ldloc, variableDefinition);
				}
				VariableDefinition variableDefinition5;
				VariableDefinition variableDefinition6;
				MethodInfo methodInfo2;
				List<HarmonyManipulator.ArgumentBoxInfo> list2;
				this.EmitCallParameter(method2, true, out variableDefinition5, out variableDefinition6, out methodInfo2, out list2);
				this.il.Emit(OpCodes.Call, method2);
				if (patchContext2.hasArgsArrayArg)
				{
					this.EmitAssignRefsFromArgsArray(this.variables[HarmonyManipulator.ArgsArrayVar]);
				}
				this.EmitInstanceUnbox(variableDefinition5, this.original.DeclaringType);
				this.EmitResultRefInvoke(methodInfo2);
				this.EmitResultUnbox(variableDefinition6, variableDefinition);
				this.EmitArgUnbox(list2);
				ParameterInfo parameterInfo = method2.GetParameters().FirstOrDefault<ParameterInfo>();
				if (parameterInfo == null || method2.ReturnType != parameterInfo.ParameterType)
				{
					if (parameterInfo != null)
					{
						throw new InvalidHarmonyPatchArgumentException("Return type of pass through postfix " + method2.FullDescription() + " does not match type of its first parameter", this.original, method2);
					}
					throw new InvalidHarmonyPatchArgumentException("Postfix patch " + method2.FullDescription() + " must have `void` as return type", this.original, method2);
				}
				else if (patchContext2.wrapTryCatch)
				{
					this.il.Emit(OpCodes.Stloc, variableDefinition);
					this.EmitTryCatchWrapper(method2, label2);
					this.il.Emit(OpCodes.Ldloc, variableDefinition);
				}
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00011844 File Offset: 0x0000FA44
		private void SortPatches()
		{
			PatchInfo patchInfo = this.patchInfo;
			Patch[] array;
			Patch[] array2;
			Patch[] array3;
			Patch[] array4;
			Patch[] array5;
			lock (patchInfo)
			{
				array = this.patchInfo.prefixes.ToArray<Patch>();
				array2 = this.patchInfo.postfixes.ToArray<Patch>();
				array3 = this.patchInfo.transpilers.ToArray<Patch>();
				array4 = this.patchInfo.finalizers.ToArray<Patch>();
				array5 = this.patchInfo.ilmanipulators.ToArray<Patch>();
			}
			this.prefixes = HarmonyManipulator.<SortPatches>g__Sort|47_0(this.original, array, this.debug);
			this.postfixes = HarmonyManipulator.<SortPatches>g__Sort|47_0(this.original, array2, this.debug);
			this.transpilers = HarmonyManipulator.<SortPatches>g__Sort|47_0(this.original, array3, this.debug);
			this.finalizers = HarmonyManipulator.<SortPatches>g__Sort|47_0(this.original, array4, this.debug);
			this.ilManipulators = HarmonyManipulator.<SortPatches>g__Sort|47_0(this.original, array5, this.debug);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00011954 File Offset: 0x0000FB54
		private bool EmitOriginalBaseMethod()
		{
			MethodInfo methodInfo = this.original as MethodInfo;
			if (methodInfo != null)
			{
				this.il.Emit(OpCodes.Ldtoken, methodInfo);
			}
			else
			{
				ConstructorInfo constructorInfo = this.original as ConstructorInfo;
				if (constructorInfo == null)
				{
					return false;
				}
				this.il.Emit(OpCodes.Ldtoken, constructorInfo);
			}
			Type reflectedType = this.original.ReflectedType;
			if (reflectedType == null)
			{
				return false;
			}
			if (reflectedType.IsGenericType)
			{
				this.il.Emit(OpCodes.Ldtoken, reflectedType);
			}
			this.il.Emit(OpCodes.Call, reflectedType.IsGenericType ? HarmonyManipulator.GetMethodFromHandle2 : HarmonyManipulator.GetMethodFromHandle1);
			return true;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000119FC File Offset: 0x0000FBFC
		private void EmitAssignRefsFromArgsArray(VariableDefinition argsArrayVariable)
		{
			ParameterInfo[] parameters = this.original.GetParameters();
			int num = ((!this.original.IsStatic) ? 1 : 0);
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type type = parameterInfo.ParameterType;
				if (type.IsByRef)
				{
					type = type.OpenRefType();
					this.il.Emit(OpCodes.Ldarg, i + num);
					this.il.Emit(OpCodes.Ldloc, argsArrayVariable);
					this.il.Emit(OpCodes.Ldc_I4, i);
					this.il.Emit(OpCodes.Ldelem_Ref);
					if (type.IsValueType)
					{
						this.il.Emit(OpCodes.Unbox_Any, type);
						if (AccessTools.IsStruct(type))
						{
							this.il.Emit(OpCodes.Stobj, type);
						}
						else
						{
							this.il.Emit(type.GetCecilStoreOpCode());
						}
					}
					else
					{
						this.il.Emit(OpCodes.Castclass, type);
						this.il.Emit(OpCodes.Stind_Ref);
					}
				}
				else
				{
					this.il.Emit(OpCodes.Ldloc, argsArrayVariable);
					this.il.Emit(OpCodes.Ldc_I4, i);
					this.il.Emit(OpCodes.Ldelem_Ref);
					this.il.Emit(type.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, type);
					this.il.Emit(OpCodes.Starg, i + num);
				}
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00011B7C File Offset: 0x0000FD7C
		private void EmitCallParameter(MethodInfo patch, bool allowFirsParamPassthrough, out VariableDefinition tmpInstanceBoxingVar, out VariableDefinition tmpObjectVar, out MethodInfo resultRefInvoke, out List<HarmonyManipulator.ArgumentBoxInfo> tmpBoxVars)
		{
			tmpInstanceBoxingVar = null;
			tmpObjectVar = null;
			resultRefInvoke = null;
			tmpBoxVars = new List<HarmonyManipulator.ArgumentBoxInfo>();
			bool flag = !this.original.IsStatic;
			ParameterInfo[] parameters = this.original.GetParameters();
			string[] array = parameters.Select<ParameterInfo, string>((ParameterInfo p) => p.Name).ToArray<string>();
			Type declaringType = this.original.DeclaringType;
			List<ParameterInfo> list = patch.GetParameters().ToList<ParameterInfo>();
			if (allowFirsParamPassthrough && patch.ReturnType != typeof(void) && list.Count > 0 && list[0].ParameterType == patch.ReturnType)
			{
				list.RemoveRange(0, 1);
			}
			foreach (ParameterInfo parameterInfo in list)
			{
				VariableDefinition variableDefinition4;
				if (parameterInfo.Name == HarmonyManipulator.OriginalMethodParam)
				{
					if (!this.EmitOriginalBaseMethod())
					{
						this.il.Emit(OpCodes.Ldnull);
					}
				}
				else if (parameterInfo.Name == HarmonyManipulator.InstanceParam)
				{
					if (this.original.IsStatic)
					{
						this.il.Emit(OpCodes.Ldnull);
					}
					else
					{
						Type parameterType = parameterInfo.ParameterType;
						bool isByRef = parameterType.IsByRef;
						if (declaringType == null || !AccessTools.IsStruct(declaringType))
						{
							if (isByRef)
							{
								this.il.Emit(OpCodes.Ldarga, 0);
							}
							else
							{
								this.il.Emit(OpCodes.Ldarg_0);
							}
						}
						else if (parameterType != typeof(object) && parameterType != typeof(object).MakeByRefType())
						{
							this.il.Emit(OpCodes.Ldarg_0);
							if (!isByRef)
							{
								this.il.Emit(OpCodes.Ldobj, declaringType);
							}
						}
						else if (isByRef)
						{
							this.il.Emit(OpCodes.Ldarg_0);
							this.il.Emit(OpCodes.Ldobj, declaringType);
							this.il.Emit(OpCodes.Box, declaringType);
							tmpInstanceBoxingVar = this.il.DeclareVariable(typeof(object));
							this.il.Emit(OpCodes.Stloc, tmpInstanceBoxingVar);
							this.il.Emit(OpCodes.Ldloca, tmpInstanceBoxingVar);
						}
						else
						{
							this.il.Emit(OpCodes.Ldarg_0);
							this.il.Emit(OpCodes.Ldobj, declaringType);
							this.il.Emit(OpCodes.Box, declaringType);
						}
					}
				}
				else if (parameterInfo.Name == HarmonyManipulator.ArgsArrayVar)
				{
					VariableDefinition variableDefinition;
					if (this.variables.TryGetValue(HarmonyManipulator.ArgsArrayVar, out variableDefinition))
					{
						this.il.Emit(OpCodes.Ldloc, variableDefinition);
					}
					else
					{
						this.il.Emit(OpCodes.Ldnull);
					}
				}
				else if (parameterInfo.Name.StartsWith(HarmonyManipulator.InstanceFieldPrefix, StringComparison.Ordinal))
				{
					string text = parameterInfo.Name.Substring(HarmonyManipulator.InstanceFieldPrefix.Length);
					IEnumerable<char> enumerable = text;
					Func<char, bool> func;
					if ((func = HarmonyManipulator.<>O.<0>__IsDigit) == null)
					{
						func = (HarmonyManipulator.<>O.<0>__IsDigit = new Func<char, bool>(char.IsDigit));
					}
					FieldInfo fieldInfo;
					if (enumerable.All<char>(func))
					{
						fieldInfo = AccessTools.DeclaredField(declaringType, int.Parse(text));
						if (fieldInfo == null)
						{
							throw new ArgumentException("No field found at given index in class " + declaringType.FullName, text);
						}
					}
					else
					{
						fieldInfo = AccessTools.Field(declaringType, text);
						if (fieldInfo == null)
						{
							throw new ArgumentException("No such field defined in class " + declaringType.FullName, text);
						}
					}
					if (fieldInfo.IsStatic)
					{
						this.il.Emit(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldsflda : OpCodes.Ldsfld, fieldInfo);
					}
					else
					{
						this.il.Emit(OpCodes.Ldarg_0);
						this.il.Emit(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, fieldInfo);
					}
				}
				else if (parameterInfo.Name == HarmonyManipulator.StateVar)
				{
					OpCode opCode = (parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc);
					VariableDefinition variableDefinition2;
					if (this.variables.TryGetValue(patch.DeclaringType.FullName, out variableDefinition2))
					{
						this.il.Emit(opCode, variableDefinition2);
					}
					else
					{
						this.il.Emit(OpCodes.Ldnull);
					}
				}
				else if (parameterInfo.Name == HarmonyManipulator.ResultVar)
				{
					Type returnType = this.ReturnType;
					if (returnType == typeof(void))
					{
						throw new Exception("Cannot get result from void method " + this.original.FullDescription());
					}
					Type type = parameterInfo.ParameterType;
					if (type.IsByRef && !returnType.IsByRef)
					{
						type = type.GetElementType();
					}
					if (!type.IsAssignableFrom(returnType))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 4);
						defaultInterpolatedStringHandler.AppendLiteral("Cannot assign method return type ");
						defaultInterpolatedStringHandler.AppendFormatted(returnType.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(" to ");
						defaultInterpolatedStringHandler.AppendFormatted(HarmonyManipulator.ResultVar);
						defaultInterpolatedStringHandler.AppendLiteral(" type ");
						defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(" for method ");
						defaultInterpolatedStringHandler.AppendFormatted(this.original.FullDescription());
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					OpCode opCode2 = ((parameterInfo.ParameterType.IsByRef && !returnType.IsByRef) ? OpCodes.Ldloca : OpCodes.Ldloc);
					if (returnType.IsValueType && parameterInfo.ParameterType == typeof(object).MakeByRefType())
					{
						opCode2 = OpCodes.Ldloc;
					}
					this.il.Emit(opCode2, this.variables[HarmonyManipulator.ResultVar]);
					if (returnType.IsValueType)
					{
						if (parameterInfo.ParameterType == typeof(object))
						{
							this.il.Emit(OpCodes.Box, returnType);
						}
						else if (parameterInfo.ParameterType == typeof(object).MakeByRefType())
						{
							this.il.Emit(OpCodes.Box, returnType);
							tmpObjectVar = this.il.DeclareVariable(typeof(object));
							this.il.Emit(OpCodes.Stloc, tmpObjectVar);
							this.il.Emit(OpCodes.Ldloca, tmpObjectVar);
						}
					}
				}
				else if (parameterInfo.Name == HarmonyManipulator.ResultRefVar)
				{
					Type returnType2 = this.ReturnType;
					if (!returnType2.IsByRef)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Cannot use ");
						defaultInterpolatedStringHandler.AppendFormatted(HarmonyManipulator.ResultRefVar);
						defaultInterpolatedStringHandler.AppendLiteral(" with non-ref return type ");
						defaultInterpolatedStringHandler.AppendFormatted(returnType2.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(" of method ");
						defaultInterpolatedStringHandler.AppendFormatted(this.original.FullDescription());
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					Type parameterType2 = parameterInfo.ParameterType;
					Type type2 = typeof(RefResult<>).MakeGenericType(new Type[] { returnType2.GetElementType() });
					Type type3 = type2.MakeByRefType();
					if (parameterType2 != type3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 4);
						defaultInterpolatedStringHandler.AppendLiteral("Wrong type of ");
						defaultInterpolatedStringHandler.AppendFormatted(HarmonyManipulator.ResultRefVar);
						defaultInterpolatedStringHandler.AppendLiteral(" for method ");
						defaultInterpolatedStringHandler.AppendFormatted(this.original.FullDescription());
						defaultInterpolatedStringHandler.AppendLiteral(". Expected ");
						defaultInterpolatedStringHandler.AppendFormatted(type3.FullName);
						defaultInterpolatedStringHandler.AppendLiteral(", got ");
						defaultInterpolatedStringHandler.AppendFormatted(parameterType2.FullName);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					VariableDefinition variableDefinition3;
					if (!this.variables.TryGetValue(HarmonyManipulator.ResultRefVar, out variableDefinition3))
					{
						variableDefinition3 = (this.variables[HarmonyManipulator.ResultRefVar] = this.il.DeclareVariable(type3));
					}
					this.il.Emit(OpCodes.Ldloca, variableDefinition3);
					resultRefInvoke = AccessTools.Method(type2, "Invoke", null, null);
				}
				else if (this.variables.TryGetValue(parameterInfo.Name, out variableDefinition4))
				{
					OpCode opCode3 = (parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc);
					this.il.Emit(opCode3, variableDefinition4);
				}
				else
				{
					int argumentIndex;
					if (parameterInfo.Name.StartsWith(HarmonyManipulator.ParamIndexPrefix, StringComparison.Ordinal))
					{
						string text2 = parameterInfo.Name.Substring(HarmonyManipulator.ParamIndexPrefix.Length);
						if (!int.TryParse(text2, out argumentIndex))
						{
							throw new Exception("Parameter " + parameterInfo.Name + " does not contain a valid index");
						}
						if (argumentIndex < 0 || argumentIndex >= parameters.Length)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
							defaultInterpolatedStringHandler.AppendLiteral("No parameter found at index ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(argumentIndex);
							throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					else
					{
						argumentIndex = patch.GetArgumentIndex(array, parameterInfo);
						if (argumentIndex == -1)
						{
							HarmonyMethod mergedFromType = HarmonyMethodExtensions.GetMergedFromType(parameterInfo.ParameterType);
							MethodType? methodType = mergedFromType.methodType;
							if (methodType == null)
							{
								mergedFromType.methodType = new MethodType?(MethodType.Normal);
							}
							MethodBase originalMethod = mergedFromType.GetOriginalMethod();
							MethodInfo methodInfo = originalMethod as MethodInfo;
							if (methodInfo != null)
							{
								ConstructorInfo constructor = parameterInfo.ParameterType.GetConstructor(new Type[]
								{
									typeof(object),
									typeof(IntPtr)
								});
								if (constructor != null)
								{
									if (methodInfo.IsStatic)
									{
										this.il.Emit(OpCodes.Ldnull);
									}
									else
									{
										this.il.Emit(OpCodes.Ldarg_0);
										if (declaringType.IsValueType)
										{
											this.il.Emit(OpCodes.Ldobj, declaringType);
											this.il.Emit(OpCodes.Box, declaringType);
										}
									}
									if (!methodInfo.IsStatic && !mergedFromType.nonVirtualDelegate)
									{
										this.il.Emit(OpCodes.Dup);
										this.il.Emit(OpCodes.Ldvirtftn, methodInfo);
									}
									else
									{
										this.il.Emit(OpCodes.Ldftn, methodInfo);
									}
									this.il.Emit(OpCodes.Newobj, constructor);
									continue;
								}
							}
							throw new Exception("Parameter \"" + parameterInfo.Name + "\" not found in method " + this.original.FullDescription());
						}
					}
					Type parameterType3 = parameters[argumentIndex].ParameterType;
					Type type4 = (parameterType3.IsByRef ? parameterType3.GetElementType() : parameterType3);
					Type parameterType4 = parameterInfo.ParameterType;
					Type type5 = (parameterType4.IsByRef ? parameterType4.GetElementType() : parameterType4);
					bool flag2 = !parameters[argumentIndex].IsOut && !parameterType3.IsByRef;
					bool flag3 = !parameterInfo.IsOut && !parameterType4.IsByRef;
					bool flag4 = type4.IsValueType && !type5.IsValueType;
					int num = argumentIndex + ((flag > false) ? 1 : 0);
					if (flag2 == flag3)
					{
						this.il.Emit(OpCodes.Ldarg, num);
						if (flag4)
						{
							if (flag3)
							{
								this.il.Emit(OpCodes.Box, type4);
							}
							else
							{
								this.il.Emit(OpCodes.Ldobj, type4);
								this.il.Emit(OpCodes.Box, type4);
								VariableDefinition variableDefinition5 = this.il.DeclareVariable(type5);
								this.il.Emit(OpCodes.Stloc, variableDefinition5);
								this.il.Emit(OpCodes.Ldloca_S, variableDefinition5);
								tmpBoxVars.Add(new HarmonyManipulator.ArgumentBoxInfo
								{
									index = num,
									type = type4,
									tmpVar = variableDefinition5,
									isByRef = true
								});
							}
						}
					}
					else if (flag2 && !flag3)
					{
						if (flag4)
						{
							this.il.Emit(OpCodes.Ldarg, num);
							this.il.Emit(OpCodes.Box, type4);
							VariableDefinition variableDefinition6 = this.il.DeclareVariable(type5);
							this.il.Emit(OpCodes.Stloc, variableDefinition6);
							this.il.Emit(OpCodes.Ldloca_S, variableDefinition6);
							tmpBoxVars.Add(new HarmonyManipulator.ArgumentBoxInfo
							{
								index = num,
								type = type4,
								tmpVar = variableDefinition6,
								isByRef = false
							});
						}
						else
						{
							this.il.Emit(OpCodes.Ldarga, num);
						}
					}
					else
					{
						this.il.Emit(OpCodes.Ldarg, num);
						if (flag4)
						{
							this.il.Emit(OpCodes.Ldobj, type4);
							this.il.Emit(OpCodes.Box, type4);
						}
						else if (type4.IsValueType)
						{
							this.il.Emit(OpCodes.Ldobj, type4);
						}
						else
						{
							this.il.Emit(parameters[argumentIndex].ParameterType.GetCecilLoadOpCode());
						}
					}
				}
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00012AE8 File Offset: 0x00010CE8
		[CompilerGenerated]
		internal static void <Process>g__Print|32_1(ICollection<HarmonyManipulator.PatchContext> list, string type, ref HarmonyManipulator.<>c__DisplayClass32_0 A_2)
		{
			if (list.Count == 0)
			{
				return;
			}
			StringBuilder sb = A_2.sb;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(type);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			sb.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (HarmonyManipulator.PatchContext patchContext in list)
			{
				A_2.sb.AppendLine("* " + patchContext.method.FullDescription());
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00012C2C File Offset: 0x00010E2C
		[CompilerGenerated]
		private bool <WriteFinalizers>g__WriteFinalizerCalls|45_1(bool suppressExceptions, ref HarmonyManipulator.<>c__DisplayClass45_0 A_2)
		{
			bool flag = true;
			foreach (HarmonyManipulator.PatchContext patchContext in this.finalizers)
			{
				MethodInfo method = patchContext.method;
				ILEmitter.Label label = this.il.DeclareLabel();
				this.il.MarkLabel(label);
				VariableDefinition variableDefinition;
				VariableDefinition variableDefinition2;
				MethodInfo methodInfo;
				List<HarmonyManipulator.ArgumentBoxInfo> list;
				this.EmitCallParameter(method, false, out variableDefinition, out variableDefinition2, out methodInfo, out list);
				this.il.Emit(OpCodes.Call, method);
				if (patchContext.hasArgsArrayArg)
				{
					this.EmitAssignRefsFromArgsArray(this.variables[HarmonyManipulator.ArgsArrayVar]);
				}
				this.EmitInstanceUnbox(variableDefinition, this.original.DeclaringType);
				this.EmitResultRefInvoke(methodInfo);
				this.EmitResultUnbox(variableDefinition2, A_2.returnValueVar);
				this.EmitArgUnbox(list);
				if (method.ReturnType != typeof(void))
				{
					this.il.Emit(OpCodes.Stloc, this.variables[HarmonyManipulator.ExceptionVar]);
					flag = false;
				}
				if (suppressExceptions || patchContext.wrapTryCatch)
				{
					this.EmitTryCatchWrapper(method, label);
				}
			}
			return flag;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00012D60 File Offset: 0x00010F60
		[CompilerGenerated]
		internal static List<HarmonyManipulator.PatchContext> <SortPatches>g__Sort|47_0(MethodBase original, Patch[] patches, bool debug)
		{
			return (from p in PatchFunctions.GetSortedPatchMethodsAsPatches(original, patches, debug)
				select new
				{
					method = p.GetMethod(original),
					origP = p
				}).Select(delegate(p)
			{
				HarmonyManipulator.PatchContext patchContext = new HarmonyManipulator.PatchContext();
				patchContext.method = p.method;
				patchContext.wrapTryCatch = p.origP.wrapTryCatch;
				patchContext.hasArgsArrayArg = p.method.GetParameters().Any<ParameterInfo>((ParameterInfo par) => par.Name == HarmonyManipulator.ArgsArrayVar);
				return patchContext;
			}).ToList<HarmonyManipulator.PatchContext>();
		}

		// Token: 0x040000D8 RID: 216
		private static readonly MethodInfo GetMethodFromHandle1 = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) });

		// Token: 0x040000D9 RID: 217
		private static readonly MethodInfo GetMethodFromHandle2 = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[]
		{
			typeof(RuntimeMethodHandle),
			typeof(RuntimeTypeHandle)
		});

		// Token: 0x040000DA RID: 218
		private static readonly string InstanceParam = "__instance";

		// Token: 0x040000DB RID: 219
		private static readonly string OriginalMethodParam = "__originalMethod";

		// Token: 0x040000DC RID: 220
		private static readonly string RunOriginalParam = "__runOriginal";

		// Token: 0x040000DD RID: 221
		private static readonly string ResultVar = "__result";

		// Token: 0x040000DE RID: 222
		private static readonly string ResultRefVar = "__resultRef";

		// Token: 0x040000DF RID: 223
		private static readonly string ArgsArrayVar = "__args";

		// Token: 0x040000E0 RID: 224
		private static readonly string StateVar = "__state";

		// Token: 0x040000E1 RID: 225
		private static readonly string ExceptionVar = "__exception";

		// Token: 0x040000E2 RID: 226
		private static readonly string ParamIndexPrefix = "__";

		// Token: 0x040000E3 RID: 227
		private static readonly string InstanceFieldPrefix = "___";

		// Token: 0x040000E4 RID: 228
		private static readonly MethodInfo LogPatchExceptionMethod = AccessTools.Method(typeof(HarmonyManipulator), "LogPatchException", null, null);

		// Token: 0x040000E5 RID: 229
		private readonly bool debug;

		// Token: 0x040000E6 RID: 230
		private readonly string[] dumpPaths;

		// Token: 0x040000E7 RID: 231
		private readonly PatchInfo patchInfo;

		// Token: 0x040000E8 RID: 232
		private readonly Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();

		// Token: 0x040000E9 RID: 233
		private ILContext ctx;

		// Token: 0x040000EA RID: 234
		private List<HarmonyManipulator.PatchContext> finalizers;

		// Token: 0x040000EB RID: 235
		private ILEmitter il;

		// Token: 0x040000EC RID: 236
		private List<HarmonyManipulator.PatchContext> ilManipulators;

		// Token: 0x040000ED RID: 237
		private MethodBase original;

		// Token: 0x040000EE RID: 238
		private List<HarmonyManipulator.PatchContext> postfixes;

		// Token: 0x040000EF RID: 239
		private List<HarmonyManipulator.PatchContext> prefixes;

		// Token: 0x040000F0 RID: 240
		private List<HarmonyManipulator.PatchContext> transpilers;

		// Token: 0x040000F1 RID: 241
		private Type _returnType;

		// Token: 0x020001CE RID: 462
		private class PatchContext
		{
			// Token: 0x04000264 RID: 612
			public bool hasArgsArrayArg;

			// Token: 0x04000265 RID: 613
			public MethodInfo method;

			// Token: 0x04000266 RID: 614
			public bool wrapTryCatch;
		}

		// Token: 0x020001CF RID: 463
		private class ArgumentBoxInfo
		{
			// Token: 0x04000267 RID: 615
			public int index;

			// Token: 0x04000268 RID: 616
			public bool isByRef;

			// Token: 0x04000269 RID: 617
			public VariableDefinition tmpVar;

			// Token: 0x0400026A RID: 618
			public Type type;
		}

		// Token: 0x020001D0 RID: 464
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400026B RID: 619
			public static Func<char, bool> <0>__IsDigit;
		}
	}
}
