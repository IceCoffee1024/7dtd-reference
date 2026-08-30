using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200021E RID: 542
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class ResourceRequest : AsyncOperation
	{
		// Token: 0x060017DF RID: 6111 RVA: 0x00027A34 File Offset: 0x00025C34
		protected virtual Object GetResult()
		{
			return Resources.Load(this.m_Path, this.m_Type);
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x00027A58 File Offset: 0x00025C58
		public Object asset
		{
			get
			{
				return this.GetResult();
			}
		}

		// Token: 0x0400088B RID: 2187
		internal string m_Path;

		// Token: 0x0400088C RID: 2188
		internal Type m_Type;
	}
}
