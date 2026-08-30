using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CF RID: 207
	[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
	[AttributeUsage(AttributeTargets.All, Inherited = false)]
	public sealed class PublicAPIAttribute : Attribute
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x00002059 File Offset: 0x00000259
		public PublicAPIAttribute()
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00006C16 File Offset: 0x00004E16
		public PublicAPIAttribute([NotNull] string comment)
		{
			this.Comment = comment;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00006C27 File Offset: 0x00004E27
		[CanBeNull]
		public string Comment { get; }
	}
}
