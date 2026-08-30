using System;
using System.Collections;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200070D RID: 1805
[Preserve]
public class MinEventActionModifyStats : MinEventActionTargetedBase
{
	// Token: 0x060036B6 RID: 14006 RVA: 0x00169597 File Offset: 0x00167797
	public override void Execute(MinEventParams _params)
	{
		if (this.Delay > 0f)
		{
			GameManager.Instance.StartCoroutine(this.executeDelayed(this.Delay, _params));
			return;
		}
		this.execute(_params);
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x001695C6 File Offset: 0x001677C6
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator executeDelayed(float delaySeconds, MinEventParams _params)
	{
		yield return new WaitForSeconds(delaySeconds);
		this.execute(_params);
		yield break;
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x001695E4 File Offset: 0x001677E4
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

	// Token: 0x060036B9 RID: 14009 RVA: 0x00169948 File Offset: 0x00167B48
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

	// Token: 0x04002C1F RID: 11295
	[PublicizedFrom(EAccessModifier.Private)]
	public string statName;

	// Token: 0x04002C20 RID: 11296
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyStats.OperationTypes operation;

	// Token: 0x04002C21 RID: 11297
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002C22 RID: 11298
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueType;

	// Token: 0x04002C23 RID: 11299
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C24 RID: 11300
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;

	// Token: 0x0200070E RID: 1806
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OperationTypes
	{
		// Token: 0x04002C26 RID: 11302
		set,
		// Token: 0x04002C27 RID: 11303
		setvalue,
		// Token: 0x04002C28 RID: 11304
		add,
		// Token: 0x04002C29 RID: 11305
		subtract,
		// Token: 0x04002C2A RID: 11306
		multiply,
		// Token: 0x04002C2B RID: 11307
		divide,
		// Token: 0x04002C2C RID: 11308
		randomfloat,
		// Token: 0x04002C2D RID: 11309
		randomint
	}
}
