using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200061A RID: 1562
[Preserve]
public class BaseOperationLootEntryRequirement : BaseLootEntryRequirement
{
	// Token: 0x060032B7 RID: 12983 RVA: 0x0014A234 File Offset: 0x00148434
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("operation", ref text))
		{
			this.operation = Enum.Parse<BaseOperationLootEntryRequirement.OperationTypes>(text);
		}
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x0014A270 File Offset: 0x00148470
	public override bool CheckRequirement(EntityPlayer player)
	{
		float num = this.LeftSide(player);
		float num2 = this.RightSide(player);
		switch (this.operation)
		{
		case BaseOperationLootEntryRequirement.OperationTypes.Equals:
		case BaseOperationLootEntryRequirement.OperationTypes.EQ:
		case BaseOperationLootEntryRequirement.OperationTypes.E:
			return num == num2;
		case BaseOperationLootEntryRequirement.OperationTypes.NotEquals:
		case BaseOperationLootEntryRequirement.OperationTypes.NEQ:
		case BaseOperationLootEntryRequirement.OperationTypes.NE:
			return num != num2;
		case BaseOperationLootEntryRequirement.OperationTypes.Less:
		case BaseOperationLootEntryRequirement.OperationTypes.LessThan:
		case BaseOperationLootEntryRequirement.OperationTypes.LT:
			return num < num2;
		case BaseOperationLootEntryRequirement.OperationTypes.Greater:
		case BaseOperationLootEntryRequirement.OperationTypes.GreaterThan:
		case BaseOperationLootEntryRequirement.OperationTypes.GT:
			return num > num2;
		case BaseOperationLootEntryRequirement.OperationTypes.LessOrEqual:
		case BaseOperationLootEntryRequirement.OperationTypes.LessThanOrEqualTo:
		case BaseOperationLootEntryRequirement.OperationTypes.LTE:
			return num <= num2;
		case BaseOperationLootEntryRequirement.OperationTypes.GreaterOrEqual:
		case BaseOperationLootEntryRequirement.OperationTypes.GreaterThanOrEqualTo:
		case BaseOperationLootEntryRequirement.OperationTypes.GTE:
			return num >= num2;
		default:
			return true;
		}
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x0003D066 File Offset: 0x0003B266
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float LeftSide(EntityPlayer player)
	{
		return 0f;
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x0003D066 File Offset: 0x0003B266
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float RightSide(EntityPlayer player)
	{
		return 0f;
	}

	// Token: 0x0400284F RID: 10319
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseOperationLootEntryRequirement.OperationTypes operation = BaseOperationLootEntryRequirement.OperationTypes.Equals;

	// Token: 0x04002850 RID: 10320
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOperation = "operation";

	// Token: 0x0200061B RID: 1563
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum OperationTypes
	{
		// Token: 0x04002852 RID: 10322
		None,
		// Token: 0x04002853 RID: 10323
		Equals,
		// Token: 0x04002854 RID: 10324
		EQ,
		// Token: 0x04002855 RID: 10325
		E,
		// Token: 0x04002856 RID: 10326
		NotEquals,
		// Token: 0x04002857 RID: 10327
		NEQ,
		// Token: 0x04002858 RID: 10328
		NE,
		// Token: 0x04002859 RID: 10329
		Less,
		// Token: 0x0400285A RID: 10330
		LessThan,
		// Token: 0x0400285B RID: 10331
		LT,
		// Token: 0x0400285C RID: 10332
		Greater,
		// Token: 0x0400285D RID: 10333
		GreaterThan,
		// Token: 0x0400285E RID: 10334
		GT,
		// Token: 0x0400285F RID: 10335
		LessOrEqual,
		// Token: 0x04002860 RID: 10336
		LessThanOrEqualTo,
		// Token: 0x04002861 RID: 10337
		LTE,
		// Token: 0x04002862 RID: 10338
		GreaterOrEqual,
		// Token: 0x04002863 RID: 10339
		GreaterThanOrEqualTo,
		// Token: 0x04002864 RID: 10340
		GTE
	}
}
