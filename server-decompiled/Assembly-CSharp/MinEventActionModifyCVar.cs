using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006D5 RID: 1749
[Preserve]
public class MinEventActionModifyCVar : MinEventActionTargetedBase
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x0600362B RID: 13867 RVA: 0x00165E5E File Offset: 0x0016405E
	// (set) Token: 0x0600362C RID: 13868 RVA: 0x00165E66 File Offset: 0x00164066
	public string cvarName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x00165E70 File Offset: 0x00164070
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
			this.targets[i].Buffs.SetCustomVar(this.cvarName, this.value, (this.targets[i].isEntityRemote && !_params.Self.isEntityRemote) || _params.IsLocal, this.operation, false);
		}
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x00166020 File Offset: 0x00164220
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

	// Token: 0x0600362F RID: 13871 RVA: 0x0016625F File Offset: 0x0016445F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.cvarName != null && this.cvarName.StartsWith("_"))
		{
			Log.Out("CVar '{0}' is readonly", new object[] { this.cvarName });
			return false;
		}
		return base.CanExecute(_eventType, _params);
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x0016629E File Offset: 0x0016449E
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

	// Token: 0x04002BB7 RID: 11191
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.SeedType seedType;

	// Token: 0x04002BB9 RID: 11193
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;

	// Token: 0x04002BBA RID: 11194
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002BBB RID: 11195
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] valueList;

	// Token: 0x04002BBC RID: 11196
	[PublicizedFrom(EAccessModifier.Private)]
	public float minValue;

	// Token: 0x04002BBD RID: 11197
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxValue;

	// Token: 0x04002BBE RID: 11198
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BBF RID: 11199
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;

	// Token: 0x04002BC0 RID: 11200
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.RandomRollTypes rollType;

	// Token: 0x020006D6 RID: 1750
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002BC2 RID: 11202
		Item,
		// Token: 0x04002BC3 RID: 11203
		Player,
		// Token: 0x04002BC4 RID: 11204
		Random
	}

	// Token: 0x020006D7 RID: 1751
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RandomRollTypes : byte
	{
		// Token: 0x04002BC6 RID: 11206
		none,
		// Token: 0x04002BC7 RID: 11207
		randomInt,
		// Token: 0x04002BC8 RID: 11208
		randomFloat,
		// Token: 0x04002BC9 RID: 11209
		tierList
	}
}
