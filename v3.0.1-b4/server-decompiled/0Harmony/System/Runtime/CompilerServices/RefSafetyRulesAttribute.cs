using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000006 RID: 6
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(AttributeTargets.Module, AllowMultiple = false, Inherited = false)]
	internal sealed class RefSafetyRulesAttribute : Attribute
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000021A8 File Offset: 0x000003A8
		public RefSafetyRulesAttribute(int A_1)
		{
			this.Version = A_1;
		}

		// Token: 0x04000005 RID: 5
		public readonly int Version;
	}
}
