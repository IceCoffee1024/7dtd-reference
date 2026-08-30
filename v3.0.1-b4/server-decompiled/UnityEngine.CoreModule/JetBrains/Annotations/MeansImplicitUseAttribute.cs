using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CC RID: 204
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
	public sealed class MeansImplicitUseAttribute : Attribute
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00006BCA File Offset: 0x00004DCA
		public MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00006BD6 File Offset: 0x00004DD6
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00006BE2 File Offset: 0x00004DE2
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00006BEE File Offset: 0x00004DEE
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00006C06 File Offset: 0x00004E06
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00006C0E File Offset: 0x00004E0E
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; }
	}
}
