using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200070B RID: 1803
[Preserve]
public class MinEventActionModifyStat : MinEventActionTargetedBase
{
	// Token: 0x060036B3 RID: 14003 RVA: 0x00169368 File Offset: 0x00167568
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.cvarRef)
			{
				this.value = this.targets[i].Buffs.GetCustomVar(this.refCvarName);
			}
			Stat stat = null;
			string text = this.statName;
			if (!(text == "health"))
			{
				if (!(text == "stamina"))
				{
					if (text == "water")
					{
						stat = this.targets[i].Stats.Water;
					}
				}
				else
				{
					stat = this.targets[i].Stats.Stamina;
				}
			}
			else
			{
				stat = this.targets[i].Stats.Health;
			}
			if (stat != null)
			{
				switch (this.operation)
				{
				case MinEventActionModifyStat.OperationTypes.set:
				case MinEventActionModifyStat.OperationTypes.setvalue:
					stat.Value = this.value;
					break;
				case MinEventActionModifyStat.OperationTypes.add:
					stat.Value += this.value;
					break;
				case MinEventActionModifyStat.OperationTypes.subtract:
					stat.Value -= this.value;
					break;
				case MinEventActionModifyStat.OperationTypes.multiply:
					stat.Value *= this.value;
					break;
				case MinEventActionModifyStat.OperationTypes.divide:
					stat.Value /= ((this.value == 0f) ? 0.0001f : this.value);
					break;
				}
			}
		}
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x001694D8 File Offset: 0x001676D8
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "stat")
			{
				this.statName = _attribute.Value.ToLower();
				return true;
			}
			if (localName == "operation")
			{
				this.operation = EnumUtils.Parse<MinEventActionModifyStat.OperationTypes>(_attribute.Value, true);
				return true;
			}
			if (localName == "value")
			{
				if (_attribute.Value.StartsWith("@"))
				{
					this.cvarRef = true;
					this.refCvarName = _attribute.Value.Substring(1);
				}
				else
				{
					this.value = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				}
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002C11 RID: 11281
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName;

	// Token: 0x04002C12 RID: 11282
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyStat.OperationTypes operation;

	// Token: 0x04002C13 RID: 11283
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002C14 RID: 11284
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C15 RID: 11285
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;

	// Token: 0x0200070C RID: 1804
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OperationTypes
	{
		// Token: 0x04002C17 RID: 11287
		set,
		// Token: 0x04002C18 RID: 11288
		setvalue,
		// Token: 0x04002C19 RID: 11289
		add,
		// Token: 0x04002C1A RID: 11290
		subtract,
		// Token: 0x04002C1B RID: 11291
		multiply,
		// Token: 0x04002C1C RID: 11292
		divide,
		// Token: 0x04002C1D RID: 11293
		randomfloat,
		// Token: 0x04002C1E RID: 11294
		randomint
	}
}
