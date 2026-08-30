using System;

namespace JetBrains.Annotations
{
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class UsedImplicitlyAttribute : Attribute
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021B7 File Offset: 0x000003B7
		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021C1 File Offset: 0x000003C1
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021CB File Offset: 0x000003CB
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021D5 File Offset: 0x000003D5
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021EB File Offset: 0x000003EB
		public ImplicitUseKindFlags UseKindFlags { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000021F3 File Offset: 0x000003F3
		public ImplicitUseTargetFlags TargetFlags { get; }
	}
}
