using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006CB RID: 1739
[Preserve]
public class MinEventActionModifyCVar : MinEventActionTargetedBase
{
	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x060035D5 RID: 13781 RVA: 0x00163C86 File Offset: 0x00161E86
	// (set) Token: 0x060035D6 RID: 13782 RVA: 0x00163C8E File Offset: 0x00161E8E
	public string cvarName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x00163C98 File Offset: 0x00161E98
	public override void Execute(MinEventParams _params)
	{
		if (this.rollType == MinEventActionModifyCVar.RandomRollTypes.tierList)
		{
			if (_params.ParentType == MinEffectController.SourceParentType.ItemClass || _params.ParentType == MinEffectController.SourceParentType.ItemModifierClass)
			{
				if (!_params.ItemValue.IsEmpty())
				{
					int num = (int)(_params.ItemValue.Quality - 1);
					if (num >= 0)
					{
						this.value = this.valueList[num];
					}
				}
			}
			else if (_params.ParentType == MinEffectController.SourceParentType.ProgressionClass && _params.ProgressionValue != null)
			{
				int num2 = _params.ProgressionValue.CalculatedLevel(_params.Self);
				if (num2 >= 0)
				{
					this.value = this.valueList[num2];
				}
			}
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.cvarRef)
			{
				this.value = this.targets[i].Buffs.GetCustomVar(this.refCvarName);
			}
			else if (this.rollType == MinEventActionModifyCVar.RandomRollTypes.randomInt)
			{
				this.value = Mathf.Clamp((float)_params.Self.rand.RandomRange((int)this.minValue, (int)this.maxValue + 1), this.minValue, this.maxValue);
			}
			else if (this.rollType == MinEventActionModifyCVar.RandomRollTypes.randomFloat)
			{
				this.value = Mathf.Clamp(_params.Self.rand.RandomRange(this.minValue, this.maxValue + 1f), this.minValue, this.maxValue);
			}
			this.targets[i].Buffs.SetCustomVar(this.cvarName, this.value, (this.targets[i].isEntityRemote && !_params.Self.isEntityRemote) || _params.IsLocal, this.operation);
		}
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x00163E48 File Offset: 0x00162048
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "cvar")
			{
				this.cvarName = _attribute.Value;
				return true;
			}
			if (localName == "operation")
			{
				this.operation = EnumUtils.Parse<CVarOperation>(_attribute.Value, true);
				return true;
			}
			if (localName == "value")
			{
				this.rollType = MinEventActionModifyCVar.RandomRollTypes.none;
				this.cvarRef = false;
				if (_attribute.Value.StartsWith("randomint", StringComparison.OrdinalIgnoreCase))
				{
					Vector2 vector = StringParsers.ParseVector2(_attribute.Value.Substring(_attribute.Value.IndexOf('(') + 1, _attribute.Value.IndexOf(')') - (_attribute.Value.IndexOf('(') + 1)));
					this.minValue = (float)((int)vector.x);
					this.maxValue = (float)((int)vector.y);
					this.rollType = MinEventActionModifyCVar.RandomRollTypes.randomInt;
				}
				else if (_attribute.Value.StartsWith("randomfloat", StringComparison.OrdinalIgnoreCase))
				{
					Vector2 vector2 = StringParsers.ParseVector2(_attribute.Value.Substring(_attribute.Value.IndexOf('(') + 1, _attribute.Value.IndexOf(')') - (_attribute.Value.IndexOf('(') + 1)));
					this.minValue = vector2.x;
					this.maxValue = vector2.y;
					this.rollType = MinEventActionModifyCVar.RandomRollTypes.randomFloat;
				}
				else if (_attribute.Value.StartsWith("@"))
				{
					this.cvarRef = true;
					this.refCvarName = _attribute.Value.Substring(1);
				}
				else if (_attribute.Value.Contains(','))
				{
					string[] array = _attribute.Value.Split(',', StringSplitOptions.None);
					this.valueList = new float[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						this.valueList[i] = StringParsers.ParseFloat(array[i], 0, -1, NumberStyles.Any);
					}
					this.rollType = MinEventActionModifyCVar.RandomRollTypes.tierList;
				}
				else
				{
					this.value = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				}
				return true;
			}
			if (localName == "seed_type")
			{
				this.seedType = EnumUtils.Parse<MinEventActionModifyCVar.SeedType>(_attribute.Value, true);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x00164087 File Offset: 0x00162287
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.cvarName != null && this.cvarName.StartsWith("_"))
		{
			Log.Out("CVar '{0}' is readonly", new object[] { this.cvarName });
			return false;
		}
		return base.CanExecute(_eventType, _params);
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x001640C6 File Offset: 0x001622C6
	public float GetValueForDisplay()
	{
		if (this.operation == CVarOperation.add)
		{
			return this.value;
		}
		if (this.operation == CVarOperation.subtract)
		{
			return -this.value;
		}
		return 0f;
	}

	// Token: 0x04002B43 RID: 11075
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.SeedType seedType;

	// Token: 0x04002B45 RID: 11077
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;

	// Token: 0x04002B46 RID: 11078
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002B47 RID: 11079
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] valueList;

	// Token: 0x04002B48 RID: 11080
	[PublicizedFrom(EAccessModifier.Private)]
	public float minValue;

	// Token: 0x04002B49 RID: 11081
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxValue;

	// Token: 0x04002B4A RID: 11082
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002B4B RID: 11083
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;

	// Token: 0x04002B4C RID: 11084
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.RandomRollTypes rollType;

	// Token: 0x020006CC RID: 1740
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002B4E RID: 11086
		Item,
		// Token: 0x04002B4F RID: 11087
		Player,
		// Token: 0x04002B50 RID: 11088
		Random
	}

	// Token: 0x020006CD RID: 1741
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RandomRollTypes : byte
	{
		// Token: 0x04002B52 RID: 11090
		none,
		// Token: 0x04002B53 RID: 11091
		randomInt,
		// Token: 0x04002B54 RID: 11092
		randomFloat,
		// Token: 0x04002B55 RID: 11093
		tierList
	}
}
