using System;

namespace JetBrains.Annotations
{
	// Token: 0x02000008 RID: 8
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
	internal sealed class MeansImplicitUseAttribute : Attribute
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000021FB File Offset: 0x000003FB
		public MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002205 File Offset: 0x00000405
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000220F File Offset: 0x0000040F
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002219 File Offset: 0x00000419
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000222F File Offset: 0x0000042F
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002237 File Offset: 0x00000437
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; }
	}
}
