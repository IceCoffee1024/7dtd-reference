using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib.Internal.Patching;
using HarmonyLib.Public.Patching;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000045 RID: 69
	public class PatchProcessor
	{
		// Token: 0x06000155 RID: 341 RVA: 0x00007B2D File Offset: 0x00005D2D
		public PatchProcessor(Harmony instance, MethodBase original)
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007B43 File Offset: 0x00005D43
		public PatchProcessor AddPrefix(HarmonyMethod prefix)
		{
			this.prefix = prefix;
			return this;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007B4D File Offset: 0x00005D4D
		public PatchProcessor AddPrefix(MethodInfo fixMethod)
		{
			this.prefix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007B5C File Offset: 0x00005D5C
		public PatchProcessor AddPostfix(HarmonyMethod postfix)
		{
			this.postfix = postfix;
			return this;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007B66 File Offset: 0x00005D66
		public PatchProcessor AddPostfix(MethodInfo fixMethod)
		{
			this.postfix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007B75 File Offset: 0x00005D75
		public PatchProcessor AddTranspiler(HarmonyMethod transpiler)
		{
			this.transpiler = transpiler;
			return this;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007B7F File Offset: 0x00005D7F
		public PatchProcessor AddTranspiler(MethodInfo fixMethod)
		{
			this.transpiler = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007B8E File Offset: 0x00005D8E
		public PatchProcessor AddFinalizer(HarmonyMethod finalizer)
		{
			this.finalizer = finalizer;
			return this;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007B98 File Offset: 0x00005D98
		public PatchProcessor AddFinalizer(MethodInfo fixMethod)
		{
			this.finalizer = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007BA7 File Offset: 0x00005DA7
		public PatchProcessor AddILManipulator(HarmonyMethod ilmanipulator)
		{
			this.ilmanipulator = ilmanipulator;
			return this;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007BB1 File Offset: 0x00005DB1
		public PatchProcessor AddILManipulator(MethodInfo fixMethod)
		{
			this.ilmanipulator = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007BC0 File Offset: 0x00005DC0
		public static IEnumerable<MethodBase> GetAllPatchedMethods()
		{
			return PatchManager.GetPatchedMethods();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007BC8 File Offset: 0x00005DC8
		public MethodInfo Patch()
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			if (!this.original.IsDeclaredMember<MethodBase>())
			{
				MethodBase declaredMember = this.original.GetDeclaredMember<MethodBase>();
				throw new ArgumentException("You can only patch implemented methods/constructors. Patch the declared method " + declaredMember.FullDescription() + " instead.");
			}
			object obj = PatchProcessor.locker;
			MethodInfo methodInfo2;
			lock (obj)
			{
				PatchInfo patchInfo = this.original.ToPatchInfo();
				patchInfo.AddPrefixes(this.instance.Id, new HarmonyMethod[] { this.prefix });
				patchInfo.AddPostfixes(this.instance.Id, new HarmonyMethod[] { this.postfix });
				patchInfo.AddTranspilers(this.instance.Id, new HarmonyMethod[] { this.transpiler });
				patchInfo.AddFinalizers(this.instance.Id, new HarmonyMethod[] { this.finalizer });
				patchInfo.AddILManipulators(this.instance.Id, new HarmonyMethod[] { this.ilmanipulator });
				MethodInfo methodInfo = PatchFunctions.UpdateWrapper(this.original, patchInfo);
				PatchManager.AddReplacementOriginal(this.original, methodInfo);
				methodInfo2 = methodInfo;
			}
			return methodInfo2;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007D28 File Offset: 0x00005F28
		public PatchProcessor Unpatch(HarmonyPatchType type, string harmonyID)
		{
			object obj = PatchProcessor.locker;
			lock (obj)
			{
				PatchInfo patchInfo = this.original.ToPatchInfo();
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Prefix)
				{
					patchInfo.RemovePrefix(harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Postfix)
				{
					patchInfo.RemovePostfix(harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Transpiler)
				{
					patchInfo.RemoveTranspiler(harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Finalizer)
				{
					patchInfo.RemoveFinalizer(harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.ILManipulator)
				{
					patchInfo.RemoveILManipulator(harmonyID);
				}
				MethodInfo methodInfo = PatchFunctions.UpdateWrapper(this.original, patchInfo);
				PatchManager.AddReplacementOriginal(this.original, methodInfo);
			}
			return this;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007DD4 File Offset: 0x00005FD4
		public PatchProcessor Unpatch(MethodInfo patch)
		{
			object obj = PatchProcessor.locker;
			lock (obj)
			{
				PatchInfo patchInfo = this.original.ToPatchInfo();
				patchInfo.RemovePatch(patch);
				MethodInfo methodInfo = PatchFunctions.UpdateWrapper(this.original, patchInfo);
				PatchManager.AddReplacementOriginal(this.original, methodInfo);
			}
			return this;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007E40 File Offset: 0x00006040
		public static Patches GetPatchInfo(MethodBase method)
		{
			PatchInfo patchInfo = method.GetPatchInfo();
			if (patchInfo == null)
			{
				return null;
			}
			return new Patches(patchInfo.prefixes, patchInfo.postfixes, patchInfo.transpilers, patchInfo.finalizers, patchInfo.ilmanipulators);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007E7C File Offset: 0x0000607C
		public static List<MethodInfo> GetSortedPatchMethods(MethodBase original, Patch[] patches)
		{
			return PatchFunctions.GetSortedPatchMethods(original, patches, false);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007E88 File Offset: 0x00006088
		public static Dictionary<string, Version> VersionInfo(out Version currentVersion)
		{
			currentVersion = typeof(Harmony).Assembly.GetName().Version;
			Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
			Action<Patch> <>9__2;
			Action<Patch> <>9__3;
			Action<Patch> <>9__4;
			Action<Patch> <>9__5;
			Action<Patch> <>9__6;
			PatchProcessor.GetAllPatchedMethods().Do<MethodBase>(delegate(MethodBase method)
			{
				PatchInfo patchInfo = method.GetPatchInfo();
				IEnumerable<Patch> prefixes = patchInfo.prefixes;
				Action<Patch> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				prefixes.Do<Patch>(action);
				IEnumerable<Patch> postfixes = patchInfo.postfixes;
				Action<Patch> action2;
				if ((action2 = <>9__3) == null)
				{
					action2 = (<>9__3 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				postfixes.Do<Patch>(action2);
				IEnumerable<Patch> transpilers = patchInfo.transpilers;
				Action<Patch> action3;
				if ((action3 = <>9__4) == null)
				{
					action3 = (<>9__4 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				transpilers.Do<Patch>(action3);
				IEnumerable<Patch> finalizers = patchInfo.finalizers;
				Action<Patch> action4;
				if ((action4 = <>9__5) == null)
				{
					action4 = (<>9__5 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				finalizers.Do<Patch>(action4);
				IEnumerable<Patch> ilmanipulators = patchInfo.ilmanipulators;
				Action<Patch> action5;
				if ((action5 = <>9__6) == null)
				{
					action5 = (<>9__6 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				ilmanipulators.Do<Patch>(action5);
			});
			Dictionary<string, Version> result = new Dictionary<string, Version>();
			assemblies.Do<KeyValuePair<string, Assembly>>(delegate(KeyValuePair<string, Assembly> info)
			{
				AssemblyName assemblyName = info.Value.GetReferencedAssemblies().FirstOrDefault<AssemblyName>((AssemblyName a) => a.FullName.StartsWith("0Harmony, Version", StringComparison.Ordinal));
				if (assemblyName != null)
				{
					result[info.Key] = assemblyName.Version;
				}
			});
			return result;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007F00 File Offset: 0x00006100
		public static ILGenerator CreateILGenerator()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ILGenerator_");
			defaultInterpolatedStringHandler.AppendFormatted<Guid>(Guid.NewGuid());
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(defaultInterpolatedStringHandler.ToStringAndClear(), typeof(void), new Type[0]);
			return dynamicMethodDefinition.GetILGenerator();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007F54 File Offset: 0x00006154
		public static ILGenerator CreateILGenerator(MethodBase original)
		{
			MethodInfo methodInfo = original as MethodInfo;
			Type type = ((methodInfo != null) ? methodInfo.ReturnType : typeof(void));
			List<Type> list = (from pi in original.GetParameters()
				select pi.ParameterType).ToList<Type>();
			if (!original.IsStatic)
			{
				list.Insert(0, original.DeclaringType);
			}
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("ILGenerator_" + original.Name, type, list.ToArray());
			return dynamicMethodDefinition.GetILGenerator();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007FE6 File Offset: 0x000061E6
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, ILGenerator generator = null)
		{
			return PatchFunctions.ApplyTranspilers(original, generator ?? PatchProcessor.CreateILGenerator(original), 0).ToList<CodeInstruction>();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007FFF File Offset: 0x000061FF
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, out ILGenerator generator)
		{
			generator = PatchProcessor.CreateILGenerator(original);
			return PatchFunctions.ApplyTranspilers(original, generator, 0).ToList<CodeInstruction>();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008017 File Offset: 0x00006217
		public static List<CodeInstruction> GetCurrentInstructions(MethodBase original, int maxTranspilers = 2147483647, ILGenerator generator = null)
		{
			return PatchFunctions.ApplyTranspilers(original, generator ?? PatchProcessor.CreateILGenerator(original), maxTranspilers).ToList<CodeInstruction>();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008030 File Offset: 0x00006230
		public static List<CodeInstruction> GetCurrentInstructions(MethodBase original, out ILGenerator generator, int maxTranspilers = 2147483647)
		{
			generator = PatchProcessor.CreateILGenerator(original);
			return PatchFunctions.ApplyTranspilers(original, generator, maxTranspilers).ToList<CodeInstruction>();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008048 File Offset: 0x00006248
		public static IEnumerable<KeyValuePair<OpCode, object>> ReadMethodBody(MethodBase method)
		{
			DynamicMethodDefinition dynamicMethodDefinition = method.GetMethodPatcher().CopyOriginal();
			if (dynamicMethodDefinition == null)
			{
				return null;
			}
			return new ILManipulator(dynamicMethodDefinition.Definition.Body, false).GetRawInstructions();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000807C File Offset: 0x0000627C
		public static IEnumerable<KeyValuePair<OpCode, object>> ReadMethodBody(MethodBase method, ILGenerator generator)
		{
			DynamicMethodDefinition dynamicMethodDefinition = method.GetMethodPatcher().CopyOriginal();
			if (dynamicMethodDefinition == null)
			{
				return null;
			}
			ILManipulator ilmanipulator = new ILManipulator(dynamicMethodDefinition.Definition.Body, false);
			ilmanipulator.GetInstructions(generator, null);
			return ilmanipulator.GetRawInstructions();
		}

		// Token: 0x04000090 RID: 144
		private readonly Harmony instance = instance;

		// Token: 0x04000091 RID: 145
		private readonly MethodBase original = original;

		// Token: 0x04000092 RID: 146
		private HarmonyMethod prefix;

		// Token: 0x04000093 RID: 147
		private HarmonyMethod postfix;

		// Token: 0x04000094 RID: 148
		private HarmonyMethod transpiler;

		// Token: 0x04000095 RID: 149
		private HarmonyMethod finalizer;

		// Token: 0x04000096 RID: 150
		private HarmonyMethod ilmanipulator;

		// Token: 0x04000097 RID: 151
		internal static readonly object locker = new object();
	}
}
