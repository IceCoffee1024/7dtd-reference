using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000C3 RID: 195
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
	public sealed class StringFormatMethodAttribute : Attribute
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x00006AC2 File Offset: 0x00004CC2
		public StringFormatMethodAttribute([NotNull] string formatParameterName)
		{
			this.FormatParameterName = formatParameterName;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00006AD3 File Offset: 0x00004CD3
		[NotNull]
		public string FormatParameterName { get; }
	}
}
