using System;
using System.Collections;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000719 RID: 1817
[Preserve]
public class MinEventActionModifyStats : MinEventActionTargetedBase
{
	// Token: 0x0600374F RID: 14159 RVA: 0x0016C0D3 File Offset: 0x0016A2D3
	public override void Execute(MinEventParams _params)
	{
		if (this.Delay > 0f)
		{
			GameManager.Instance.StartCoroutine(this.executeDelayed(this.Delay, _params));
			return;
		}
		this.execute(_params);
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0016C102 File Offset: 0x0016A302
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator executeDelayed(float delaySeconds, MinEventParams _params)
	{
		yield return new WaitForSeconds(delaySeconds);
		this.execute(_params);
		yield break;
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x0016C120 File Offset: 0x0016A320
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

	// Token: 0x06003752 RID: 14162 RVA: 0x0016C484 File Offset: 0x0016A684
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

	// Token: 0x04002C8C RID: 11404
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName;

	// Token: 0x04002C8D RID: 11405
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyStats.OperationTypes operation;

	// Token: 0x04002C8E RID: 11406
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002C8F RID: 11407
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueType;

	// Token: 0x04002C90 RID: 11408
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C91 RID: 11409
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;

	// Token: 0x0200071A RID: 1818
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OperationTypes
	{
		// Token: 0x04002C93 RID: 11411
		set,
		// Token: 0x04002C94 RID: 11412
		setvalue,
		// Token: 0x04002C95 RID: 11413
		add,
		// Token: 0x04002C96 RID: 11414
		subtract,
		// Token: 0x04002C97 RID: 11415
		multiply,
		// Token: 0x04002C98 RID: 11416
		divide,
		// Token: 0x04002C99 RID: 11417
		randomfloat,
		// Token: 0x04002C9A RID: 11418
		randomint
	}
}
