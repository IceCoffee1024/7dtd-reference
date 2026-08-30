using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000C8 RID: 200
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizationRequiredAttribute : Attribute
	{
		// Token: 0x060003C3 RID: 963 RVA: 0x00006B41 File Offset: 0x00004D41
		public LocalizationRequiredAttribute()
			: this(true)
		{
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00006B4C File Offset: 0x00004D4C
		public LocalizationRequiredAttribute(bool required)
		{
			this.Required = required;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00006B5D File Offset: 0x00004D5D
		public bool Required { get; }
	}
}
