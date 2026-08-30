using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000DB RID: 219
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AssertionConditionAttribute : Attribute
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x00006CD7 File Offset: 0x00004ED7
		public AssertionConditionAttribute(AssertionConditionType conditionType)
		{
			this.ConditionType = conditionType;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00006CE8 File Offset: 0x00004EE8
		public AssertionConditionType ConditionType { get; }
	}
}
