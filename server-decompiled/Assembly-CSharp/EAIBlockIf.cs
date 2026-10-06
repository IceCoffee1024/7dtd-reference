using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000433 RID: 1075
[Preserve]
public class EAIBlockIf : EAIBase
{
	// Token: 0x060020F3 RID: 8435 RVA: 0x000C71B8 File Offset: 0x000C53B8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x000C71C8 File Offset: 0x000C53C8
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		this.conditions = new List<EAIBlockIf.Condition>();
		string text;
		if (data.TryGetValue("condition", out text))
		{
			string[] array = text.Split(' ', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i += 3)
			{
				EAIBlockIf.Condition condition = new EAIBlockIf.Condition
				{
					type = EnumUtils.Parse<EAIBlockIf.eType>(array[i], true)
				};
				if (condition.type == EAIBlockIf.eType.None)
				{
					Log.Warning("{0} BlockIf type None", new object[] { this.theEntity.EntityName });
				}
				condition.op = EnumUtils.Parse<EAIBlockIf.eOp>(array[i + 1], true);
				if (condition.op == EAIBlockIf.eOp.None)
				{
					Log.Warning("{0} BlockIf op None", new object[] { this.theEntity.EntityName });
				}
				condition.value = StringParsers.ParseFloat(array[i + 2], 0, -1, NumberStyles.Any);
				this.conditions.Add(condition);
			}
		}
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x000C72B4 File Offset: 0x000C54B4
	public override bool CanExecute()
	{
		int count = this.conditions.Count;
		for (int i = 0; i < count; i++)
		{
			EAIBlockIf.Condition condition = this.conditions[i];
			float num = 0f;
			EAIBlockIf.eType type = condition.type;
			if (type != EAIBlockIf.eType.Alert)
			{
				if (type == EAIBlockIf.eType.Investigate)
				{
					num = (float)(this.theEntity.HasInvestigatePosition ? 1 : 0);
				}
			}
			else
			{
				num = (float)(this.theEntity.IsAlert ? 1 : 0);
			}
			if (this.Compare(condition.op, num, condition.value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000C7340 File Offset: 0x000C5540
	[PublicizedFrom(EAccessModifier.Private)]
	public bool Compare(EAIBlockIf.eOp op, float v1, float v2)
	{
		if (op != EAIBlockIf.eOp.e)
		{
			return op == EAIBlockIf.eOp.ne && v1 != v2;
		}
		return v1 == v2;
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000C7101 File Offset: 0x000C5301
	public override bool Continue()
	{
		return this.CanExecute();
	}

	// Token: 0x04001698 RID: 5784
	public bool canExecute;

	// Token: 0x04001699 RID: 5785
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAIBlockIf.Condition> conditions;

	// Token: 0x02000434 RID: 1076
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eType
	{
		// Token: 0x0400169B RID: 5787
		None,
		// Token: 0x0400169C RID: 5788
		Alert,
		// Token: 0x0400169D RID: 5789
		Investigate
	}

	// Token: 0x02000435 RID: 1077
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eOp
	{
		// Token: 0x0400169F RID: 5791
		None,
		// Token: 0x040016A0 RID: 5792
		e,
		// Token: 0x040016A1 RID: 5793
		ne
	}

	// Token: 0x02000436 RID: 1078
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Condition
	{
		// Token: 0x040016A2 RID: 5794
		public EAIBlockIf.eType type;

		// Token: 0x040016A3 RID: 5795
		public EAIBlockIf.eOp op;

		// Token: 0x040016A4 RID: 5796
		public float value;
	}
}
