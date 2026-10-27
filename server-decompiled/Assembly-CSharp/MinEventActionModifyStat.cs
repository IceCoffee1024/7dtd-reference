using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200071F RID: 1823
[Preserve]
public class MinEventActionModifyStat : MinEventActionTargetedBase
{
	// Token: 0x06003763 RID: 14179 RVA: 0x0016CE3C File Offset: 0x0016B03C
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

	// Token: 0x06003764 RID: 14180 RVA: 0x0016CFAC File Offset: 0x0016B1AC
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

	// Token: 0x04002C9A RID: 11418
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName;

	// Token: 0x04002C9B RID: 11419
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyStat.OperationTypes operation;

	// Token: 0x04002C9C RID: 11420
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002C9D RID: 11421
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C9E RID: 11422
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;

	// Token: 0x02000720 RID: 1824
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OperationTypes
	{
		// Token: 0x04002CA0 RID: 11424
		set,
		// Token: 0x04002CA1 RID: 11425
		setvalue,
		// Token: 0x04002CA2 RID: 11426
		add,
		// Token: 0x04002CA3 RID: 11427
		subtract,
		// Token: 0x04002CA4 RID: 11428
		multiply,
		// Token: 0x04002CA5 RID: 11429
		divide,
		// Token: 0x04002CA6 RID: 11430
		randomfloat,
		// Token: 0x04002CA7 RID: 11431
		randomint
	}
}
