using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200042D RID: 1069
[Preserve]
public class EAIBlockIf : EAIBase
{
	// Token: 0x060020E7 RID: 8423 RVA: 0x000C68F8 File Offset: 0x000C4AF8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000C6908 File Offset: 0x000C4B08
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

	// Token: 0x060020E9 RID: 8425 RVA: 0x000C69F4 File Offset: 0x000C4BF4
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

	// Token: 0x060020EA RID: 8426 RVA: 0x000C6A80 File Offset: 0x000C4C80
	[PublicizedFrom(EAccessModifier.Private)]
	public bool Compare(EAIBlockIf.eOp op, float v1, float v2)
	{
		if (op != EAIBlockIf.eOp.e)
		{
			return op == EAIBlockIf.eOp.ne && v1 != v2;
		}
		return v1 == v2;
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x000C6841 File Offset: 0x000C4A41
	public override bool Continue()
	{
		return this.CanExecute();
	}

	// Token: 0x0400168C RID: 5772
	public bool canExecute;

	// Token: 0x0400168D RID: 5773
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAIBlockIf.Condition> conditions;

	// Token: 0x0200042E RID: 1070
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eType
	{
		// Token: 0x0400168F RID: 5775
		None,
		// Token: 0x04001690 RID: 5776
		Alert,
		// Token: 0x04001691 RID: 5777
		Investigate
	}

	// Token: 0x0200042F RID: 1071
	[PublicizedFrom(EAccessModifier.Private)]
	public enum eOp
	{
		// Token: 0x04001693 RID: 5779
		None,
		// Token: 0x04001694 RID: 5780
		e,
		// Token: 0x04001695 RID: 5781
		ne
	}

	// Token: 0x02000430 RID: 1072
	[PublicizedFrom(EAccessModifier.Private)]
	public struct Condition
	{
		// Token: 0x04001696 RID: 5782
		public EAIBlockIf.eType type;

		// Token: 0x04001697 RID: 5783
		public EAIBlockIf.eOp op;

		// Token: 0x04001698 RID: 5784
		public float value;
	}
}
