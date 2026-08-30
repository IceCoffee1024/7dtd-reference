using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000D2 RID: 210
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class MustUseReturnValueAttribute : Attribute
	{
		// Token: 0x060003DA RID: 986 RVA: 0x00002059 File Offset: 0x00000259
		public MustUseReturnValueAttribute()
		{
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00006C2F File Offset: 0x00004E2F
		public MustUseReturnValueAttribute([NotNull] string justification)
		{
			this.Justification = justification;
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00006C40 File Offset: 0x00004E40
		[CanBeNull]
		public string Justification { get; }
	}
}
