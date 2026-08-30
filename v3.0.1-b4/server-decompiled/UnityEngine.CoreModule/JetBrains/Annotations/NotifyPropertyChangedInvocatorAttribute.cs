using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000C6 RID: 198
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
	{
		// Token: 0x060003BC RID: 956 RVA: 0x00002059 File Offset: 0x00000259
		public NotifyPropertyChangedInvocatorAttribute()
		{
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00006AF4 File Offset: 0x00004CF4
		public NotifyPropertyChangedInvocatorAttribute([NotNull] string parameterName)
		{
			this.ParameterName = parameterName;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00006B05 File Offset: 0x00004D05
		[CanBeNull]
		public string ParameterName { get; }
	}
}
