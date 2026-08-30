using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CB RID: 203
	[AttributeUsage(AttributeTargets.All, Inherited = false)]
	public sealed class UsedImplicitlyAttribute : Attribute
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00006B7E File Offset: 0x00004D7E
		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00006B8A File Offset: 0x00004D8A
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00006B96 File Offset: 0x00004D96
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00006BA2 File Offset: 0x00004DA2
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00006BBA File Offset: 0x00004DBA
		public ImplicitUseKindFlags UseKindFlags { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00006BC2 File Offset: 0x00004DC2
		public ImplicitUseTargetFlags TargetFlags { get; }
	}
}
