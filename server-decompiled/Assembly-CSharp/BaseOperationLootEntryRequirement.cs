using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000625 RID: 1573
[Preserve]
public class BaseOperationLootEntryRequirement : BaseLootEntryRequirement
{
	// Token: 0x0600334D RID: 13133 RVA: 0x0014CCC0 File Offset: 0x0014AEC0
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("operation", ref text))
		{
			this.operation = Enum.Parse<BaseOperationLootEntryRequirement.OperationTypes>(text);
		}
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x0014CCFC File Offset: 0x0014AEFC
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

	// Token: 0x0600334F RID: 13135 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float LeftSide(EntityPlayer player)
	{
		return 0f;
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float RightSide(EntityPlayer player)
	{
		return 0f;
	}

	// Token: 0x040028BB RID: 10427
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseOperationLootEntryRequirement.OperationTypes operation = BaseOperationLootEntryRequirement.OperationTypes.Equals;

	// Token: 0x040028BC RID: 10428
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOperation = "operation";

	// Token: 0x02000626 RID: 1574
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum OperationTypes
	{
		// Token: 0x040028BE RID: 10430
		None,
		// Token: 0x040028BF RID: 10431
		Equals,
		// Token: 0x040028C0 RID: 10432
		EQ,
		// Token: 0x040028C1 RID: 10433
		E,
		// Token: 0x040028C2 RID: 10434
		NotEquals,
		// Token: 0x040028C3 RID: 10435
		NEQ,
		// Token: 0x040028C4 RID: 10436
		NE,
		// Token: 0x040028C5 RID: 10437
		Less,
		// Token: 0x040028C6 RID: 10438
		LessThan,
		// Token: 0x040028C7 RID: 10439
		LT,
		// Token: 0x040028C8 RID: 10440
		Greater,
		// Token: 0x040028C9 RID: 10441
		GreaterThan,
		// Token: 0x040028CA RID: 10442
		GT,
		// Token: 0x040028CB RID: 10443
		LessOrEqual,
		// Token: 0x040028CC RID: 10444
		LessThanOrEqualTo,
		// Token: 0x040028CD RID: 10445
		LTE,
		// Token: 0x040028CE RID: 10446
		GreaterOrEqual,
		// Token: 0x040028CF RID: 10447
		GreaterThanOrEqualTo,
		// Token: 0x040028D0 RID: 10448
		GTE
	}
}
