using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000224 RID: 548
	[RequiredByNativeCode]
	[NativeHeader("Runtime/GameCode/AsyncInstantiate/AsyncInstantiateOperation.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class AsyncInstantiateOperation : AsyncOperation
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x00027F04 File Offset: 0x00026104
		public Object[] Result
		{
			get
			{
				return this.m_Result;
			}
		}

		// Token: 0x06001815 RID: 6165
		[NativeMethod("IsWaitingForSceneActivation")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsWaitingForSceneActivation();

		// Token: 0x06001816 RID: 6166
		[NativeMethod("WaitForCompletion")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WaitForCompletion();

		// Token: 0x06001817 RID: 6167
		[NativeMethod("Cancel")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Cancel();

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001818 RID: 6168
		// (set) Token: 0x06001819 RID: 6169
		[StaticAccessor("GetAsyncInstantiateManager()", StaticAccessorType.Dot)]
		internal static extern float IntegrationTimeMS
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00027F28 File Offset: 0x00026128
		public static float GetIntegrationTimeMS()
		{
			return AsyncInstantiateOperation.IntegrationTimeMS;
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00027F40 File Offset: 0x00026140
		public static void SetIntegrationTimeMS(float integrationTimeMS)
		{
			bool flag = integrationTimeMS <= 0f;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("integrationTimeMS", "integrationTimeMS was out of range. Must be greater than zero.");
			}
			AsyncInstantiateOperation.IntegrationTimeMS = integrationTimeMS;
		}

		// Token: 0x04000890 RID: 2192
		internal Object[] m_Result;
	}
}
