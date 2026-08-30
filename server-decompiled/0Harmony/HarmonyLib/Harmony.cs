using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using HarmonyLib.Internal.RuntimeFixes;
using HarmonyLib.Public.Patching;
using HarmonyLib.Tools;

namespace HarmonyLib
{
	// Token: 0x0200003B RID: 59
	public class Harmony : IDisposable
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005214 File Offset: 0x00003414
		public string Id { get; }

		// Token: 0x060000D4 RID: 212 RVA: 0x0000521C File Offset: 0x0000341C
		static Harmony()
		{
			StackTraceFixes.Install();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000522C File Offset: 0x0000342C
		public Harmony(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("id cannot be null or empty");
			}
			try
			{
				string text = Environment.GetEnvironmentVariable("HARMONY_DEBUG");
				if (text != null && text.Length > 0)
				{
					text = text.Trim();
					Harmony.DEBUG = text == "1" || bool.Parse(text);
				}
			}
			catch
			{
			}
			if (Harmony.DEBUG)
			{
				HarmonyFileLog.Enabled = true;
			}
			MethodBase callingMethod = (Logger.IsEnabledFor(Logger.LogChannel.Info) ? AccessTools.GetOutsideCaller() : null);
			Logger.Log(Logger.LogChannel.Info, delegate
			{
				StringBuilder stringBuilder = new StringBuilder();
				Assembly assembly = typeof(Harmony).Assembly;
				Version version = assembly.GetName().Version;
				string text2 = assembly.Location;
				string text3 = Environment.Version.ToString();
				string text4 = Environment.OSVersion.Platform.ToString();
				if (string.IsNullOrEmpty(text2))
				{
					text2 = new Uri(assembly.CodeBase).LocalPath;
				}
				int size = IntPtr.Size;
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 6);
				defaultInterpolatedStringHandler.AppendLiteral("### Harmony id=");
				defaultInterpolatedStringHandler.AppendFormatted(id);
				defaultInterpolatedStringHandler.AppendLiteral(", version=");
				defaultInterpolatedStringHandler.AppendFormatted<Version>(version);
				defaultInterpolatedStringHandler.AppendLiteral(", location=");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral(", env/clr=");
				defaultInterpolatedStringHandler.AppendFormatted(text3);
				defaultInterpolatedStringHandler.AppendLiteral(", platform=");
				defaultInterpolatedStringHandler.AppendFormatted(text4);
				defaultInterpolatedStringHandler.AppendLiteral(", ptrsize:runtime=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(size);
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				MethodBase callingMethod2 = callingMethod;
				if (((callingMethod2 != null) ? callingMethod2.DeclaringType : null) != null)
				{
					Assembly assembly2 = callingMethod.DeclaringType.Assembly;
					text2 = assembly2.Location;
					if (string.IsNullOrEmpty(text2))
					{
						text2 = new Uri(assembly2.CodeBase).LocalPath;
					}
					stringBuilder.AppendLine("### Started from " + callingMethod.FullDescription() + ", location " + text2);
					StringBuilder stringBuilder3 = stringBuilder;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
					defaultInterpolatedStringHandler.AppendLiteral("### At ");
					defaultInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now, "yyyy-MM-dd hh.mm.ss");
					stringBuilder3.Append(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				return stringBuilder.ToString();
			}, false);
			this.Id = id;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000052F8 File Offset: 0x000034F8
		public void PatchAll()
		{
			MethodBase method = new StackTrace().GetFrame(1).GetMethod();
			Assembly assembly = method.ReflectedType.Assembly;
			this.PatchAll(assembly);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005329 File Offset: 0x00003529
		public PatchProcessor CreateProcessor(MethodBase original)
		{
			return new PatchProcessor(this, original);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005332 File Offset: 0x00003532
		public PatchClassProcessor CreateClassProcessor(Type type)
		{
			return new PatchClassProcessor(this, type);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000533B File Offset: 0x0000353B
		public PatchClassProcessor CreateClassProcessor(Type type, bool allowUnannotatedType)
		{
			return new PatchClassProcessor(this, type, allowUnannotatedType);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005345 File Offset: 0x00003545
		public ReversePatcher CreateReversePatcher(MethodBase original, HarmonyMethod standin)
		{
			return new ReversePatcher(this, original, standin);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000534F File Offset: 0x0000354F
		public void PatchAll(Assembly assembly)
		{
			AccessTools.GetTypesFromAssembly(assembly).Do<Type>(delegate(Type type)
			{
				this.CreateClassProcessor(type).Patch();
			});
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005368 File Offset: 0x00003568
		public void PatchAll(Type type)
		{
			this.CreateClassProcessor(type, true).Patch();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005378 File Offset: 0x00003578
		public void PatchAllUncategorized()
		{
			MethodBase method = new StackTrace().GetFrame(1).GetMethod();
			Assembly assembly = method.ReflectedType.Assembly;
			this.PatchAllUncategorized(assembly);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000053AC File Offset: 0x000035AC
		public void PatchAllUncategorized(Assembly assembly)
		{
			PatchClassProcessor[] array = AccessTools.GetTypesFromAssembly(assembly).Select<Type, PatchClassProcessor>(new Func<Type, PatchClassProcessor>(this.CreateClassProcessor)).ToArray<PatchClassProcessor>();
			array.DoIf<PatchClassProcessor>((PatchClassProcessor patchClass) => string.IsNullOrEmpty(patchClass.Category), delegate(PatchClassProcessor patchClass)
			{
				patchClass.Patch();
			});
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000541C File Offset: 0x0000361C
		public void PatchCategory(string category)
		{
			MethodBase method = new StackTrace().GetFrame(1).GetMethod();
			Assembly assembly = method.ReflectedType.Assembly;
			this.PatchCategory(assembly, category);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005450 File Offset: 0x00003650
		public void PatchCategory(Assembly assembly, string category)
		{
			AccessTools.GetTypesFromAssembly(assembly).Where<Type>(delegate(Type type)
			{
				List<HarmonyMethod> fromType = HarmonyMethodExtensions.GetFromType(type);
				HarmonyMethod harmonyMethod = HarmonyMethod.Merge(fromType);
				return harmonyMethod.category == category;
			}).Do<Type>(delegate(Type type)
			{
				this.CreateClassProcessor(type).Patch();
			});
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000549C File Offset: 0x0000369C
		public MethodInfo Patch(MethodBase original, HarmonyMethod prefix = null, HarmonyMethod postfix = null, HarmonyMethod transpiler = null, HarmonyMethod finalizer = null, HarmonyMethod ilmanipulator = null)
		{
			PatchProcessor patchProcessor = this.CreateProcessor(original);
			patchProcessor.AddPrefix(prefix);
			patchProcessor.AddPostfix(postfix);
			patchProcessor.AddTranspiler(transpiler);
			patchProcessor.AddFinalizer(finalizer);
			patchProcessor.AddILManipulator(ilmanipulator);
			return patchProcessor.Patch();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000054E2 File Offset: 0x000036E2
		[Obsolete("Use newer Patch() instead", true)]
		public MethodInfo Patch(MethodBase original, HarmonyMethod prefix, HarmonyMethod postfix, HarmonyMethod transpiler, HarmonyMethod finalizer)
		{
			return this.Patch(original, prefix, postfix, transpiler, finalizer, null);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000054F2 File Offset: 0x000036F2
		public static MethodInfo ReversePatch(MethodBase original, HarmonyMethod standin, MethodInfo transpiler = null, MethodInfo ilmanipulator = null)
		{
			return PatchFunctions.ReversePatch(standin, original, transpiler, ilmanipulator);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000054FD File Offset: 0x000036FD
		[Obsolete("Use newer ReversePatch() instead", true)]
		public static MethodInfo ReversePatch(MethodBase original, HarmonyMethod standin, MethodInfo transpiler)
		{
			return PatchFunctions.ReversePatch(standin, original, transpiler, null);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005508 File Offset: 0x00003708
		public static void UnpatchID(string harmonyID)
		{
			if (string.IsNullOrEmpty(harmonyID))
			{
				throw new ArgumentNullException("harmonyID", "UnpatchID was called with a null or empty harmonyID.");
			}
			PatchFunctions.UnpatchConditional((Patch patchInfo) => patchInfo.owner == harmonyID);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005550 File Offset: 0x00003750
		void IDisposable.Dispose()
		{
			this.UnpatchSelf();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005558 File Offset: 0x00003758
		public void UnpatchSelf()
		{
			Harmony.UnpatchID(this.Id);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005568 File Offset: 0x00003768
		public static void UnpatchAll()
		{
			Logger.Log(Logger.LogChannel.Warn, () => "UnpatchAll has been called - This will remove ALL HARMONY PATCHES.", false);
			PatchFunctions.UnpatchConditional((Patch _) => true);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000055C0 File Offset: 0x000037C0
		[Obsolete("Use UnpatchSelf() to unpatch the current instance. The functionality to unpatch either other ids or EVERYTHING has been moved the static methods UnpatchID() and UnpatchAll() respectively", true)]
		public void UnpatchAll(string harmonyID = null)
		{
			if (harmonyID == null)
			{
				if (HarmonyGlobalSettings.DisallowLegacyGlobalUnpatchAll)
				{
					Logger.Log(Logger.LogChannel.Warn, () => "Legacy UnpatchAll has been called AND DisallowLegacyGlobalUnpatchAll=true. Skipping execution of UnpatchAll", false);
					return;
				}
				Harmony.UnpatchAll();
				return;
			}
			else
			{
				if (harmonyID.Length == 0)
				{
					Logger.Log(Logger.LogChannel.Warn, () => "Legacy UnpatchAll was called with harmonyID=\"\" which is an invalid id. Skipping execution of UnpatchAll", false);
					return;
				}
				Harmony.UnpatchID(harmonyID);
				return;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000563C File Offset: 0x0000383C
		public void Unpatch(MethodBase original, HarmonyPatchType type, string harmonyID = "*")
		{
			PatchProcessor patchProcessor = this.CreateProcessor(original);
			patchProcessor.Unpatch(type, harmonyID);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000565C File Offset: 0x0000385C
		public void Unpatch(MethodBase original, MethodInfo patch)
		{
			PatchProcessor patchProcessor = this.CreateProcessor(original);
			patchProcessor.Unpatch(patch);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000567C File Offset: 0x0000387C
		public void UnpatchCategory(string category)
		{
			MethodBase method = new StackTrace().GetFrame(1).GetMethod();
			Assembly assembly = method.ReflectedType.Assembly;
			this.UnpatchCategory(assembly, category);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000056B0 File Offset: 0x000038B0
		public void UnpatchCategory(Assembly assembly, string category)
		{
			AccessTools.GetTypesFromAssembly(assembly).Where<Type>(delegate(Type type)
			{
				List<HarmonyMethod> fromType = HarmonyMethodExtensions.GetFromType(type);
				HarmonyMethod harmonyMethod = HarmonyMethod.Merge(fromType);
				return harmonyMethod.category == category;
			}).Do<Type>(delegate(Type type)
			{
				this.CreateClassProcessor(type).Unpatch();
			});
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000056FC File Offset: 0x000038FC
		public static bool HasAnyPatches(string harmonyID)
		{
			IEnumerable<MethodBase> allPatchedMethods = Harmony.GetAllPatchedMethods();
			Func<MethodBase, Patches> func;
			if ((func = Harmony.<>O.<0>__GetPatchInfo) == null)
			{
				func = (Harmony.<>O.<0>__GetPatchInfo = new Func<MethodBase, Patches>(Harmony.GetPatchInfo));
			}
			return allPatchedMethods.Select<MethodBase, Patches>(func).Any<Patches>((Patches info) => info.Owners.Contains(harmonyID));
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000574C File Offset: 0x0000394C
		public static Patches GetPatchInfo(MethodBase method)
		{
			return PatchProcessor.GetPatchInfo(method);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005754 File Offset: 0x00003954
		public IEnumerable<MethodBase> GetPatchedMethods()
		{
			return from original in Harmony.GetAllPatchedMethods()
				where Harmony.GetPatchInfo(original).Owners.Contains(this.Id)
				select original;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000576C File Offset: 0x0000396C
		public static IEnumerable<MethodBase> GetAllPatchedMethods()
		{
			return PatchProcessor.GetAllPatchedMethods();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005773 File Offset: 0x00003973
		public static MethodBase GetOriginalMethod(MethodInfo replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			return PatchManager.GetRealMethod(replacement, false);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005790 File Offset: 0x00003990
		public static MethodBase GetMethodFromStackframe(StackFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame");
			}
			return PatchManager.GetStackFrameMethod(frame, true);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000057A7 File Offset: 0x000039A7
		public static MethodBase GetOriginalMethodFromStackframe(StackFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame");
			}
			return PatchManager.GetStackFrameMethod(frame, false);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000057BE File Offset: 0x000039BE
		public static Dictionary<string, Version> VersionInfo(out Version currentVersion)
		{
			return PatchProcessor.VersionInfo(out currentVersion);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000057C8 File Offset: 0x000039C8
		public static Harmony CreateAndPatchAll(Type type, string harmonyInstanceId = null)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string text = harmonyInstanceId;
			if (harmonyInstanceId == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 3);
				defaultInterpolatedStringHandler.AppendLiteral("harmony-auto-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Interlocked.Increment(ref Harmony._autoGuidCounter));
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted(type.Assembly.GetName().Name);
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			Harmony harmony = new Harmony(text);
			harmony.PatchAll(type);
			return harmony;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005868 File Offset: 0x00003A68
		public static Harmony CreateAndPatchAll(Assembly assembly, string harmonyInstanceId = null)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			string text = harmonyInstanceId;
			if (harmonyInstanceId == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
				defaultInterpolatedStringHandler.AppendLiteral("harmony-auto-");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Interlocked.Increment(ref Harmony._autoGuidCounter));
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted(assembly.GetName().Name);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			Harmony harmony = new Harmony(text);
			harmony.PatchAll(assembly);
			return harmony;
		}

		// Token: 0x0400005F RID: 95
		[Obsolete("Use HarmonyFileLog.Enabled instead")]
		public static bool DEBUG;

		// Token: 0x04000060 RID: 96
		private static int _autoGuidCounter = 100;

		// Token: 0x02000086 RID: 134
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000172 RID: 370
			public static Func<MethodBase, Patches> <0>__GetPatchInfo;
		}
	}
}
