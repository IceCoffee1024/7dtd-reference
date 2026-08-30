using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using HarmonyLib.Tools;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;

namespace HarmonyLib.Internal.Util
{
	// Token: 0x02000060 RID: 96
	internal static class CecilEmitter
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x00013A5C File Offset: 0x00011C5C
		public static void Dump(MethodDefinition md, IEnumerable<string> dumpPaths, MethodBase original = null)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
			defaultInterpolatedStringHandler.AppendLiteral("HarmonyDump.");
			defaultInterpolatedStringHandler.AppendFormatted(CecilEmitter.SanitizeTypeName(md.GetID(null, null, false, true)));
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<int>(Guid.NewGuid().GetHashCode(), "X8");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			string text2 = (((original != null) ? original.Name : null) ?? md.Name).Replace('.', '_');
			using (ModuleDefinition module = ModuleDefinition.CreateModule(text, new ModuleParameters
			{
				Kind = ModuleKind.Dll,
				ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault
			}))
			{
				module.Assembly.CustomAttributes.Add(new CustomAttribute(module.ImportReference(CecilEmitter.UnverifiableCodeAttributeConstructor)));
				int hashCode = Guid.NewGuid().GetHashCode();
				string text3 = "";
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
				defaultInterpolatedStringHandler.AppendLiteral("HarmonyDump<");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral(">?");
				defaultInterpolatedStringHandler.AppendFormatted<int>(hashCode);
				TypeDefinition typeDefinition = new TypeDefinition(text3, defaultInterpolatedStringHandler.ToStringAndClear(), Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed)
				{
					BaseType = module.TypeSystem.Object
				};
				module.Types.Add(typeDefinition);
				MethodDefinition clone = null;
				TypeReference typeReference = new TypeReference("System.Runtime.CompilerServices", "IsVolatile", module, module.TypeSystem.CoreLibrary);
				Relinker relinker = delegate(IMetadataTokenProvider mtp, [Nullable(2)] IGenericParameterProvider _)
				{
					if (mtp != md)
					{
						return module.ImportReference(mtp);
					}
					return clone;
				};
				clone = new MethodDefinition(((original != null) ? original.Name : null) ?? ("_" + md.Name.Replace(".", "_")), md.Attributes, module.TypeSystem.Void)
				{
					MethodReturnType = md.MethodReturnType,
					Attributes = (Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.HideBySig),
					ImplAttributes = Mono.Cecil.MethodImplAttributes.IL,
					DeclaringType = typeDefinition,
					HasThis = false
				};
				typeDefinition.Methods.Add(clone);
				foreach (ParameterDefinition parameterDefinition in md.Parameters)
				{
					clone.Parameters.Add(parameterDefinition.Clone().Relink(relinker, clone));
				}
				clone.ReturnType = md.ReturnType.Relink(relinker, clone);
				Mono.Cecil.Cil.MethodBody methodBody = (clone.Body = md.Body.Clone(clone));
				foreach (VariableDefinition variableDefinition in clone.Body.Variables)
				{
					variableDefinition.VariableType = variableDefinition.VariableType.Relink(relinker, clone);
				}
				foreach (ExceptionHandler exceptionHandler in clone.Body.ExceptionHandlers.Where<ExceptionHandler>((ExceptionHandler handler) => handler.CatchType != null))
				{
					exceptionHandler.CatchType = exceptionHandler.CatchType.Relink(relinker, clone);
				}
				foreach (Instruction instruction in methodBody.Instructions)
				{
					object obj = instruction.Operand;
					ParameterDefinition parameterDefinition2 = obj as ParameterDefinition;
					object obj2;
					if (parameterDefinition2 == null)
					{
						ILLabel illabel = obj as ILLabel;
						if (illabel == null)
						{
							IMetadataTokenProvider metadataTokenProvider = obj as IMetadataTokenProvider;
							if (metadataTokenProvider == null)
							{
								obj2 = obj;
							}
							else
							{
								obj2 = metadataTokenProvider.Relink(relinker, clone);
							}
						}
						else
						{
							obj2 = illabel.Target;
						}
					}
					else
					{
						obj2 = clone.Parameters[parameterDefinition2.Index];
					}
					obj = obj2;
					Instruction previous = instruction.Previous;
					if (((previous != null) ? new OpCode?(previous.OpCode) : null) == OpCodes.Volatile)
					{
						FieldReference fieldReference = obj as FieldReference;
						if (fieldReference != null)
						{
							RequiredModifierType requiredModifierType = fieldReference.FieldType as RequiredModifierType;
							if (((requiredModifierType != null) ? requiredModifierType.ModifierType : null) != typeReference)
							{
								fieldReference.FieldType = new RequiredModifierType(typeReference, fieldReference.FieldType);
							}
						}
					}
					instruction.Operand = obj;
				}
				if (md.HasThis)
				{
					TypeReference typeReference2 = md.DeclaringType;
					if (typeReference2.IsValueType)
					{
						typeReference2 = new ByReferenceType(typeReference2);
					}
					clone.Parameters.Insert(0, new ParameterDefinition("<>_this", Mono.Cecil.ParameterAttributes.None, typeReference2.Relink(relinker, clone)));
				}
				foreach (string text4 in dumpPaths)
				{
					string fullPath = Path.GetFullPath(text4);
					try
					{
						Directory.CreateDirectory(fullPath);
						using (FileStream fileStream = File.OpenWrite(Path.Combine(fullPath, module.Name + ".dll")))
						{
							module.Write(fileStream);
						}
					}
					catch (Exception ex)
					{
						Exception e = ex;
						Logger.Log(Logger.LogChannel.Error, delegate
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(21, 3);
							defaultInterpolatedStringHandler2.AppendLiteral("Failed to dump ");
							defaultInterpolatedStringHandler2.AppendFormatted(md.GetID(null, null, true, true));
							defaultInterpolatedStringHandler2.AppendLiteral(" to ");
							defaultInterpolatedStringHandler2.AppendFormatted(fullPath);
							defaultInterpolatedStringHandler2.AppendLiteral(": ");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(e);
							return defaultInterpolatedStringHandler2.ToStringAndClear();
						}, false);
					}
				}
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00014158 File Offset: 0x00012358
		private static string SanitizeTypeName(string typeName)
		{
			return typeName.Replace(":", "_").Replace(" ", "_").Replace("<", "{")
				.Replace(">", "}");
		}

		// Token: 0x0400010B RID: 267
		private static readonly ConstructorInfo UnverifiableCodeAttributeConstructor = typeof(UnverifiableCodeAttribute).GetConstructor(Type.EmptyTypes);
	}
}
