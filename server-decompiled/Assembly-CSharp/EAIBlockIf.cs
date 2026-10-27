using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000435 RID: 1077
[Preserve]
public class EAIBlockIf : EAIBase
{
	// Token: 0x060020F8 RID: 8440 RVA: 0x000C7564 File Offset: 0x000C5764
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x000C7574 File Offset: 0x000C5774
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

	// Token: 0x060020FA RID: 8442 RVA: 0x000C7660 File Offset: 0x000C5860
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

	// Token: 0x060020FB RID: 8443 RVA: 0x000C76EC File Offset: 0x000C58EC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool Compare(EAIBlockIf.eOp op, float v1, float v2)
	{
		if (op != EAIBlockIf.eOp.e)
		{
			return op == EAIBlockIf.eOp.ne && v1 != v2;
		}
		return v1 == v2;
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x000C74AD File Offset: 0x000C56AD
	public override bool Continue()
	{
		return this.CanExecute();
	}

	// Token: 0x040016A8 RID: 5800
	public bool canExecute;

	// Token: 0x040016A9 RID: 5801
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAIBlockIf.Condition> conditions;

	// Token: 0x02000436 RID: 1078
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eType
	{
		// Token: 0x040016AB RID: 5803
		None,
		// Token: 0x040016AC RID: 5804
		Alert,
		// Token: 0x040016AD RID: 5805
		Investigate
	}

	// Token: 0x02000437 RID: 1079
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eOp
	{
		// Token: 0x040016AF RID: 5807
		None,
		// Token: 0x040016B0 RID: 5808
		e,
		// Token: 0x040016B1 RID: 5809
		ne
	}

	// Token: 0x02000438 RID: 1080
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Condition
	{
		// Token: 0x040016B2 RID: 5810
		public EAIBlockIf.eType type;

		// Token: 0x040016B3 RID: 5811
		public EAIBlockIf.eOp op;

		// Token: 0x040016B4 RID: 5812
		public float value;
	}
}
