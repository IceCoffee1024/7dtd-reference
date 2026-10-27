using System;
using UnityEngine;

// Token: 0x02000572 RID: 1394
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowIfAttribute : PropertyAttribute
{
	// Token: 0x06002CE6 RID: 11494 RVA: 0x00120035 File Offset: 0x0011E235
	public ShowIfAttribute(string conditionField, object compareValue)
	{
		this.ConditionField = conditionField;
		this.CompareValue = compareValue;
	}

	// Token: 0x0400235E RID: 9054
	public readonly string ConditionField;

	// Token: 0x0400235F RID: 9055
	public readonly object CompareValue;
}
