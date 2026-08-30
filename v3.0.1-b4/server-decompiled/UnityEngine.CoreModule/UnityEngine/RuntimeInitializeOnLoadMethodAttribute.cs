using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000255 RID: 597
	[RequiredByNativeCode]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class RuntimeInitializeOnLoadMethodAttribute : PreserveAttribute
	{
		// Token: 0x06001965 RID: 6501 RVA: 0x0002A6D4 File Offset: 0x000288D4
		public RuntimeInitializeOnLoadMethodAttribute()
		{
			this.loadType = RuntimeInitializeLoadType.AfterSceneLoad;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0002A6E6 File Offset: 0x000288E6
		public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType)
		{
			this.loadType = loadType;
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x0002A6F8 File Offset: 0x000288F8
		// (set) Token: 0x06001968 RID: 6504 RVA: 0x0002A710 File Offset: 0x00028910
		public RuntimeInitializeLoadType loadType
		{
			get
			{
				return this.m_LoadType;
			}
			private set
			{
				this.m_LoadType = value;
			}
		}

		// Token: 0x040008DF RID: 2271
		private RuntimeInitializeLoadType m_LoadType;
	}
}
