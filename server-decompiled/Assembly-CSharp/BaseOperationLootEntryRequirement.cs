using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200062C RID: 1580
[Preserve]
public class BaseOperationLootEntryRequirement : BaseLootEntryRequirement
{
	// Token: 0x0600335B RID: 13147 RVA: 0x0014D8B4 File Offset: 0x0014BAB4
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("operation", ref text))
		{
			this.operation = Enum.Parse<BaseOperationLootEntryRequirement.OperationTypes>(text);
		}
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x0014D8F0 File Offset: 0x0014BAF0
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

	// Token: 0x0600335D RID: 13149 RVA: 0x0003D372 File Offset: 0x0003B572
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float LeftSide(EntityPlayer player)
	{
		return 0f;
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x0003D372 File Offset: 0x0003B572
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float RightSide(EntityPlayer player)
	{
		return 0f;
	}

	// Token: 0x040028D6 RID: 10454
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseOperationLootEntryRequirement.OperationTypes operation = BaseOperationLootEntryRequirement.OperationTypes.Equals;

	// Token: 0x040028D7 RID: 10455
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOperation = "operation";

	// Token: 0x0200062D RID: 1581
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum OperationTypes
	{
		// Token: 0x040028D9 RID: 10457
		None,
		// Token: 0x040028DA RID: 10458
		Equals,
		// Token: 0x040028DB RID: 10459
		EQ,
		// Token: 0x040028DC RID: 10460
		E,
		// Token: 0x040028DD RID: 10461
		NotEquals,
		// Token: 0x040028DE RID: 10462
		NEQ,
		// Token: 0x040028DF RID: 10463
		NE,
		// Token: 0x040028E0 RID: 10464
		Less,
		// Token: 0x040028E1 RID: 10465
		LessThan,
		// Token: 0x040028E2 RID: 10466
		LT,
		// Token: 0x040028E3 RID: 10467
		Greater,
		// Token: 0x040028E4 RID: 10468
		GreaterThan,
		// Token: 0x040028E5 RID: 10469
		GT,
		// Token: 0x040028E6 RID: 10470
		LessOrEqual,
		// Token: 0x040028E7 RID: 10471
		LessThanOrEqualTo,
		// Token: 0x040028E8 RID: 10472
		LTE,
		// Token: 0x040028E9 RID: 10473
		GreaterOrEqual,
		// Token: 0x040028EA RID: 10474
		GreaterThanOrEqualTo,
		// Token: 0x040028EB RID: 10475
		GTE
	}
}
