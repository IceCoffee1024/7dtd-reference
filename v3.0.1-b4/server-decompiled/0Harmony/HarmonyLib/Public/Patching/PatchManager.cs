using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib.Public.Patching
{
	// Token: 0x0200005E RID: 94
	public static class PatchManager
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x00013348 File Offset: 0x00011548
		static PatchManager()
		{
			if (AccessTools.IsMonoRuntime)
			{
				FieldInfo fieldInfo = AccessTools.Field(typeof(StackFrame), "methodAddress");
				if (fieldInfo != null)
				{
					PatchManager.MethodAddressRef = AccessTools.FieldRefAccess<StackFrame, long>(fieldInfo);
				}
			}
			PatchManager.ResolvePatcher += ManagedMethodPatcher.TryResolve;
			PatchManager.ResolvePatcher += NativeDetourMethodPatcher.TryResolve;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000415 RID: 1045 RVA: 0x000133CC File Offset: 0x000115CC
		// (remove) Token: 0x06000416 RID: 1046 RVA: 0x00013400 File Offset: 0x00011600
		public static event EventHandler<PatchManager.PatcherResolverEventArgs> ResolvePatcher;

		// Token: 0x06000417 RID: 1047 RVA: 0x00013434 File Offset: 0x00011634
		public static MethodPatcher GetMethodPatcher(this MethodBase methodBase)
		{
			Dictionary<MethodBase, MethodPatcher> methodPatchers = PatchManager.MethodPatchers;
			MethodPatcher methodPatcher2;
			lock (methodPatchers)
			{
				MethodPatcher methodPatcher;
				if (PatchManager.MethodPatchers.TryGetValue(methodBase, out methodPatcher))
				{
					methodPatcher2 = methodPatcher;
				}
				else
				{
					PatchManager.PatcherResolverEventArgs e = new PatchManager.PatcherResolverEventArgs
					{
						Original = methodBase
					};
					EventHandler<PatchManager.PatcherResolverEventArgs> resolvePatcher = PatchManager.ResolvePatcher;
					if (resolvePatcher != null)
					{
						resolvePatcher(null, e);
					}
					if (e.MethodPatcher == null)
					{
						throw new NullReferenceException("No suitable patcher found for " + methodBase.FullDescription());
					}
					methodPatcher2 = (PatchManager.MethodPatchers[methodBase] = e.MethodPatcher);
				}
			}
			return methodPatcher2;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000134D8 File Offset: 0x000116D8
		public static PatchInfo GetPatchInfo(this MethodBase methodBase)
		{
			Dictionary<MethodBase, PatchInfo> patchInfos = PatchManager.PatchInfos;
			PatchInfo valueSafe;
			lock (patchInfos)
			{
				valueSafe = PatchManager.PatchInfos.GetValueSafe(methodBase);
			}
			return valueSafe;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00013520 File Offset: 0x00011720
		public static PatchInfo ToPatchInfo(this MethodBase methodBase)
		{
			Dictionary<MethodBase, PatchInfo> patchInfos = PatchManager.PatchInfos;
			PatchInfo patchInfo2;
			lock (patchInfos)
			{
				PatchInfo patchInfo;
				if (PatchManager.PatchInfos.TryGetValue(methodBase, out patchInfo))
				{
					patchInfo2 = patchInfo;
				}
				else
				{
					patchInfo2 = (PatchManager.PatchInfos[methodBase] = new PatchInfo());
				}
			}
			return patchInfo2;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00013584 File Offset: 0x00011784
		public static IEnumerable<MethodBase> GetPatchedMethods()
		{
			Dictionary<MethodBase, PatchInfo> patchInfos = PatchManager.PatchInfos;
			IEnumerable<MethodBase> enumerable;
			lock (patchInfos)
			{
				enumerable = PatchManager.PatchInfos.Keys.ToArray<MethodBase>();
			}
			return enumerable;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000135D0 File Offset: 0x000117D0
		internal static MethodBase GetRealMethod(MethodInfo method, bool useReplacement)
		{
			MethodInfo methodInfo = method.Identifiable();
			ConditionalWeakTable<MethodBase, MethodBase> replacementToOriginals = PatchManager.ReplacementToOriginals;
			lock (replacementToOriginals)
			{
				MethodBase methodBase;
				if (PatchManager.ReplacementToOriginals.TryGetValue(methodInfo, out methodBase))
				{
					return methodBase;
				}
			}
			if (AccessTools.IsMonoRuntime)
			{
				long num = (long)method.MethodHandle.GetFunctionPointer();
				Dictionary<long, MethodBase[]> replacementToOriginalsMono = PatchManager.ReplacementToOriginalsMono;
				lock (replacementToOriginalsMono)
				{
					MethodBase[] array;
					if (PatchManager.ReplacementToOriginalsMono.TryGetValue(num, out array))
					{
						return useReplacement ? array[1] : array[0];
					}
				}
			}
			return method;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00013694 File Offset: 0x00011894
		internal unsafe static MethodBase GetStackFrameMethod(StackFrame frame, bool useReplacement)
		{
			MethodInfo methodInfo = frame.GetMethod() as MethodInfo;
			if (methodInfo != null)
			{
				return PatchManager.GetRealMethod(methodInfo, useReplacement);
			}
			if (PatchManager.MethodAddressRef != null)
			{
				long num = *PatchManager.MethodAddressRef(frame);
				Dictionary<long, MethodBase[]> replacementToOriginalsMono = PatchManager.ReplacementToOriginalsMono;
				lock (replacementToOriginalsMono)
				{
					MethodBase[] array;
					if (PatchManager.ReplacementToOriginalsMono.TryGetValue(num, out array))
					{
						return useReplacement ? array[1] : array[0];
					}
				}
			}
			return null;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001371C File Offset: 0x0001191C
		internal static void AddReplacementOriginal(MethodBase original, MethodBase replacement)
		{
			MethodInfo methodInfo = replacement as MethodInfo;
			replacement = ((methodInfo != null) ? methodInfo.Identifiable() : null) ?? replacement;
			if (replacement == null)
			{
				return;
			}
			ConditionalWeakTable<MethodBase, MethodBase> replacementToOriginals = PatchManager.ReplacementToOriginals;
			lock (replacementToOriginals)
			{
				if (!PatchManager.ReplacementToOriginals.TryAdd(replacement, original))
				{
					PatchManager.ReplacementToOriginals.Remove(replacement);
					PatchManager.ReplacementToOriginals.Add(replacement, original);
				}
			}
			if (AccessTools.IsMonoRuntime)
			{
				long num = (long)replacement.MethodHandle.GetFunctionPointer();
				Dictionary<long, MethodBase[]> replacementToOriginalsMono = PatchManager.ReplacementToOriginalsMono;
				lock (replacementToOriginalsMono)
				{
					PatchManager.ReplacementToOriginalsMono[num] = new MethodBase[] { original, replacement };
				}
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00013800 File Offset: 0x00011A00
		public static void ClearAllPatcherResolvers()
		{
			PatchManager.ResolvePatcher = null;
		}

		// Token: 0x04000100 RID: 256
		private static readonly Dictionary<MethodBase, PatchInfo> PatchInfos = new Dictionary<MethodBase, PatchInfo>();

		// Token: 0x04000101 RID: 257
		private static readonly Dictionary<MethodBase, MethodPatcher> MethodPatchers = new Dictionary<MethodBase, MethodPatcher>();

		// Token: 0x04000102 RID: 258
		private static readonly ConditionalWeakTable<MethodBase, MethodBase> ReplacementToOriginals = new ConditionalWeakTable<MethodBase, MethodBase>();

		// Token: 0x04000103 RID: 259
		private static readonly Dictionary<long, MethodBase[]> ReplacementToOriginalsMono = new Dictionary<long, MethodBase[]>();

		// Token: 0x04000104 RID: 260
		private static readonly AccessTools.FieldRef<StackFrame, long> MethodAddressRef;

		// Token: 0x020001D6 RID: 470
		public class PatcherResolverEventArgs : EventArgs
		{
			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0001C47A File Offset: 0x0001A67A
			// (set) Token: 0x060007DA RID: 2010 RVA: 0x0001C482 File Offset: 0x0001A682
			public MethodBase Original { get; internal set; }

			// Token: 0x170001FA RID: 506
			// (get) Token: 0x060007DB RID: 2011 RVA: 0x0001C48B File Offset: 0x0001A68B
			// (set) Token: 0x060007DC RID: 2012 RVA: 0x0001C493 File Offset: 0x0001A693
			public MethodPatcher MethodPatcher { get; set; }
		}
	}
}
