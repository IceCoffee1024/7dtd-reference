using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class HideIfAttribute : PropertyAttribute
{
	// Token: 0x06002CE4 RID: 11492 RVA: 0x00120017 File Offset: 0x0011E217
	public HideIfAttribute(string conditionField, object compareValue)
	{
		this.ConditionField = conditionField;
		this.CompareValue = compareValue;
	}

	// Token: 0x0400235C RID: 9052
	public readonly string ConditionField;

	// Token: 0x0400235D RID: 9053
	public readonly object CompareValue;
}
