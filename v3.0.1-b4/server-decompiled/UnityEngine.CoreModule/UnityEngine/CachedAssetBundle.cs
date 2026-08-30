using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020000FE RID: 254
	[UsedByNativeCode]
	public struct CachedAssetBundle
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x00007F26 File Offset: 0x00006126
		public CachedAssetBundle(string name, Hash128 hash)
		{
			this.m_Name = name;
			this.m_Hash = hash;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00007F38 File Offset: 0x00006138
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00007F50 File Offset: 0x00006150
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00007F5C File Offset: 0x0000615C
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x00007F74 File Offset: 0x00006174
		public Hash128 hash
		{
			get
			{
				return this.m_Hash;
			}
			set
			{
				this.m_Hash = value;
			}
		}

		// Token: 0x04000344 RID: 836
		private string m_Name;

		// Token: 0x04000345 RID: 837
		private Hash128 m_Hash;
	}
}
