using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000715 RID: 1813
[Preserve]
public class MinEventActionModifyStat : MinEventActionTargetedBase
{
	// Token: 0x06003709 RID: 14089 RVA: 0x0016B540 File Offset: 0x00169740
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

	// Token: 0x0600370A RID: 14090 RVA: 0x0016B6B0 File Offset: 0x001698B0
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

	// Token: 0x04002C85 RID: 11397
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName;

	// Token: 0x04002C86 RID: 11398
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyStat.OperationTypes operation;

	// Token: 0x04002C87 RID: 11399
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002C88 RID: 11400
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C89 RID: 11401
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;

	// Token: 0x02000716 RID: 1814
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OperationTypes
	{
		// Token: 0x04002C8B RID: 11403
		set,
		// Token: 0x04002C8C RID: 11404
		setvalue,
		// Token: 0x04002C8D RID: 11405
		add,
		// Token: 0x04002C8E RID: 11406
		subtract,
		// Token: 0x04002C8F RID: 11407
		multiply,
		// Token: 0x04002C90 RID: 11408
		divide,
		// Token: 0x04002C91 RID: 11409
		randomfloat,
		// Token: 0x04002C92 RID: 11410
		randomint
	}
}
