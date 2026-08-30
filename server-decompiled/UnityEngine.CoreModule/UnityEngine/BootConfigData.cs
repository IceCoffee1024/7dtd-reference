using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020000FD RID: 253
	[NativeHeader("Runtime/Export/Bootstrap/BootConfig.bindings.h")]
	internal class BootConfigData
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x00007E93 File Offset: 0x00006093
		public void AddKey(string key)
		{
			this.Append(key, null);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00007EA0 File Offset: 0x000060A0
		public string Get(string key)
		{
			return this.GetValue(key, 0);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00007EBC File Offset: 0x000060BC
		public string Get(string key, int index)
		{
			return this.GetValue(key, index);
		}

		// Token: 0x060004D2 RID: 1234
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Append(string key, string value);

		// Token: 0x060004D3 RID: 1235
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Set(string key, string value);

		// Token: 0x060004D4 RID: 1236
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetValue(string key, int index);

		// Token: 0x060004D5 RID: 1237 RVA: 0x00007ED8 File Offset: 0x000060D8
		[RequiredByNativeCode]
		private static BootConfigData WrapBootConfigData(IntPtr nativeHandle)
		{
			return new BootConfigData(nativeHandle);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00007EF0 File Offset: 0x000060F0
		private BootConfigData(IntPtr nativeHandle)
		{
			bool flag = nativeHandle == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("native handle can not be null");
			}
			this.m_Ptr = nativeHandle;
		}

		// Token: 0x04000343 RID: 835
		private IntPtr m_Ptr;
	}
}
