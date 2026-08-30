using System;

namespace UnityEngine.Lumin
{
	// Token: 0x020003DB RID: 987
	[Obsolete("Lumin is no longer supported in Unity 2022.2")]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class UsesLuminPlatformLevelAttribute : Attribute
	{
		// Token: 0x06002148 RID: 8520 RVA: 0x0003755B File Offset: 0x0003575B
		public UsesLuminPlatformLevelAttribute(uint platformLevel)
		{
			this.m_PlatformLevel = platformLevel;
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x0003756C File Offset: 0x0003576C
		public uint platformLevel
		{
			get
			{
				return this.m_PlatformLevel;
			}
		}

		// Token: 0x04000B10 RID: 2832
		private readonly uint m_PlatformLevel;
	}
}
