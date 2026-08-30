using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000D3 RID: 211
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Parameter)]
	public sealed class MustDisposeResourceAttribute : Attribute
	{
		// Token: 0x060003DD RID: 989 RVA: 0x00006C48 File Offset: 0x00004E48
		public MustDisposeResourceAttribute()
		{
			this.Value = true;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00006C59 File Offset: 0x00004E59
		public MustDisposeResourceAttribute(bool value)
		{
			this.Value = value;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00006C6A File Offset: 0x00004E6A
		public bool Value { get; }
	}
}
