using System;
using System.Collections;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000721 RID: 1825
[Preserve]
public class MinEventActionModifyStats : MinEventActionTargetedBase
{
	// Token: 0x06003766 RID: 14182 RVA: 0x0016D06B File Offset: 0x0016B26B
	public override void Execute(MinEventParams _params)
	{
		if (this.Delay > 0f)
		{
			GameManager.Instance.StartCoroutine(this.executeDelayed(this.Delay, _params));
			return;
		}
		this.execute(_params);
	}

	// Token: 0x06003767 RID: 14183 RVA: 0x0016D09A File Offset: 0x0016B29A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator executeDelayed(float delaySeconds, MinEventParams _params)
	{
		yield return new WaitForSeconds(delaySeconds);
		this.execute(_params);
		yield break;
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x0016D0B8 File Offset: 0x0016B2B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void execute(MinEventParams _params)
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
					if (!(text == "food"))
					{
						if (text == "water")
						{
							stat = this.targets[i].Stats.Water;
						}
					}
					else
					{
						stat = this.targets[i].Stats.Food;
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
				case MinEventActionModifyStats.OperationTypes.set:
				case MinEventActionModifyStats.OperationTypes.setvalue:
					if (this.valueType == "max")
					{
						stat.BaseMax = this.value;
					}
					else if (this.valueType == "modifiedmax")
					{
						stat.MaxModifier = this.value - stat.BaseMax;
					}
					else
					{
						stat.Value = this.value;
					}
					break;
				case MinEventActionModifyStats.OperationTypes.add:
					if (this.valueType == "max")
					{
						stat.BaseMax += this.value;
					}
					else if (this.valueType == "modifiedmax")
					{
						stat.MaxModifier += this.value;
					}
					else
					{
						stat.Value += this.value;
					}
					break;
				case MinEventActionModifyStats.OperationTypes.subtract:
					if (this.valueType == "max")
					{
						stat.BaseMax -= this.value;
					}
					else if (this.valueType == "modifiedmax")
					{
						stat.MaxModifier -= this.value;
					}
					else
					{
						stat.Value -= this.value;
					}
					break;
				case MinEventActionModifyStats.OperationTypes.multiply:
					if (this.valueType == "max")
					{
						stat.BaseMax *= this.value;
					}
					else if (this.valueType == "modifiedmax")
					{
						stat.MaxModifier *= this.value;
					}
					else
					{
						stat.Value *= this.value;
					}
					break;
				case MinEventActionModifyStats.OperationTypes.divide:
					if (this.valueType == "max")
					{
						stat.BaseMax = stat.Value / ((this.value == 0f) ? 0.0001f : this.value);
					}
					else if (this.valueType == "modifiedmax")
					{
						stat.MaxModifier = stat.Value / ((this.value == 0f) ? 0.0001f : this.value);
					}
					else
					{
						stat.Value /= ((this.value == 0f) ? 0.0001f : this.value);
					}
					break;
				}
			}
		}
	}

	// Token: 0x06003769 RID: 14185 RVA: 0x0016D41C File Offset: 0x0016B61C
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
				this.operation = EnumUtils.Parse<MinEventActionModifyStats.OperationTypes>(_attribute.Value, true);
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
			if (localName == "value_type")
			{
				this.valueType = _attribute.Value.ToLower();
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002CA8 RID: 11432
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName;

	// Token: 0x04002CA9 RID: 11433
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyStats.OperationTypes operation;

	// Token: 0x04002CAA RID: 11434
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002CAB RID: 11435
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueType;

	// Token: 0x04002CAC RID: 11436
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002CAD RID: 11437
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;

	// Token: 0x02000722 RID: 1826
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OperationTypes
	{
		// Token: 0x04002CAF RID: 11439
		set,
		// Token: 0x04002CB0 RID: 11440
		setvalue,
		// Token: 0x04002CB1 RID: 11441
		add,
		// Token: 0x04002CB2 RID: 11442
		subtract,
		// Token: 0x04002CB3 RID: 11443
		multiply,
		// Token: 0x04002CB4 RID: 11444
		divide,
		// Token: 0x04002CB5 RID: 11445
		randomfloat,
		// Token: 0x04002CB6 RID: 11446
		randomint
	}
}
