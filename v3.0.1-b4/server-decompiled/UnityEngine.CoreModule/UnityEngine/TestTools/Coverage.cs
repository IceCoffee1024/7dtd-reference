using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.TestTools
{
	// Token: 0x020004B7 RID: 1207
	[NativeClass("ScriptingCoverage")]
	[NativeType("Runtime/Scripting/ScriptingCoverage.h")]
	public static class Coverage
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002AA8 RID: 10920
		// (set) Token: 0x06002AA9 RID: 10921
		public static extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06002AAA RID: 10922
		[FreeFunction("ScriptingCoverageGetCoverageForMethodInfoObject", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CoveredSequencePoint[] GetSequencePointsFor_Internal(MethodBase method);

		// Token: 0x06002AAB RID: 10923
		[FreeFunction("ScriptingCoverageResetForMethodInfoObject", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetFor_Internal(MethodBase method);

		// Token: 0x06002AAC RID: 10924 RVA: 0x00047904 File Offset: 0x00045B04
		[FreeFunction("ScriptingCoverageGetStatsForMethodInfoObject", ThrowsException = true)]
		private static CoveredMethodStats GetStatsFor_Internal(MethodBase method)
		{
			CoveredMethodStats coveredMethodStats;
			Coverage.GetStatsFor_Internal_Injected(method, out coveredMethodStats);
			return coveredMethodStats;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x0004791C File Offset: 0x00045B1C
		public static CoveredSequencePoint[] GetSequencePointsFor(MethodBase method)
		{
			bool flag = method == null;
			if (flag)
			{
				throw new ArgumentNullException("method");
			}
			return Coverage.GetSequencePointsFor_Internal(method);
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x0004794C File Offset: 0x00045B4C
		public static CoveredMethodStats GetStatsFor(MethodBase method)
		{
			bool flag = method == null;
			if (flag)
			{
				throw new ArgumentNullException("method");
			}
			return Coverage.GetStatsFor_Internal(method);
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x0004797C File Offset: 0x00045B7C
		public static CoveredMethodStats[] GetStatsFor(MethodBase[] methods)
		{
			bool flag = methods == null;
			if (flag)
			{
				throw new ArgumentNullException("methods");
			}
			CoveredMethodStats[] array = new CoveredMethodStats[methods.Length];
			for (int i = 0; i < methods.Length; i++)
			{
				array[i] = Coverage.GetStatsFor(methods[i]);
			}
			return array;
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000479D0 File Offset: 0x00045BD0
		public static CoveredMethodStats[] GetStatsFor(Type type)
		{
			bool flag = type == null;
			if (flag)
			{
				throw new ArgumentNullException("type");
			}
			return Coverage.GetStatsFor(type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).OfType<MethodBase>().ToArray<MethodBase>());
		}

		// Token: 0x06002AB1 RID: 10929
		[FreeFunction("ScriptingCoverageGetStatsForAllCoveredMethodsFromScripting", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CoveredMethodStats[] GetStatsForAllCoveredMethods();

		// Token: 0x06002AB2 RID: 10930 RVA: 0x00047A10 File Offset: 0x00045C10
		public static void ResetFor(MethodBase method)
		{
			bool flag = method == null;
			if (flag)
			{
				throw new ArgumentNullException("method");
			}
			Coverage.ResetFor_Internal(method);
		}

		// Token: 0x06002AB3 RID: 10931
		[FreeFunction("ScriptingCoverageResetAllFromScripting", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetAll();

		// Token: 0x06002AB4 RID: 10932
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetStatsFor_Internal_Injected(MethodBase method, out CoveredMethodStats ret);
	}
}
