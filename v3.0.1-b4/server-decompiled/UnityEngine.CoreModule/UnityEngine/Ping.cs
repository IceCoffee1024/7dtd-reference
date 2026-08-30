using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000203 RID: 515
	[NativeHeader("Runtime/Export/Networking/Ping.bindings.h")]
	public sealed class Ping
	{
		// Token: 0x06001756 RID: 5974 RVA: 0x00026F2E File Offset: 0x0002512E
		public Ping(string address)
		{
			this.m_Ptr = Ping.Internal_Create(address);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00026F44 File Offset: 0x00025144
		~Ping()
		{
			this.DestroyPing();
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x00026F74 File Offset: 0x00025174
		[ThreadAndSerializationSafe]
		public void DestroyPing()
		{
			bool flag = this.m_Ptr == IntPtr.Zero;
			if (!flag)
			{
				Ping.Internal_Destroy(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
		}

		// Token: 0x06001759 RID: 5977
		[FreeFunction("DestroyPing", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		// Token: 0x0600175A RID: 5978
		[FreeFunction("CreatePing")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create(string address);

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x00026FB0 File Offset: 0x000251B0
		public bool isDone
		{
			get
			{
				bool flag = this.m_Ptr == IntPtr.Zero;
				return !flag && this.Internal_IsDone();
			}
		}

		// Token: 0x0600175C RID: 5980
		[NativeName("GetIsDone")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_IsDone();

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600175D RID: 5981
		public extern int time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x0600175E RID: 5982
		public extern string ip
		{
			[NativeName("GetIP")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x04000868 RID: 2152
		internal IntPtr m_Ptr;
	}
}
