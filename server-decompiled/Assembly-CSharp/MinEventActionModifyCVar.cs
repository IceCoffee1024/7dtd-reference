using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006D7 RID: 1751
[Preserve]
public class MinEventActionModifyCVar : MinEventActionTargetedBase
{
	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x0600366E RID: 13934 RVA: 0x001667CE File Offset: 0x001649CE
	// (set) Token: 0x0600366F RID: 13935 RVA: 0x001667D6 File Offset: 0x001649D6
	public string cvarName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x001667E0 File Offset: 0x001649E0
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

	// Token: 0x06003671 RID: 13937 RVA: 0x00166990 File Offset: 0x00164B90
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

	// Token: 0x06003672 RID: 13938 RVA: 0x00166BCF File Offset: 0x00164DCF
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.cvarName != null && this.cvarName.StartsWith("_"))
		{
			Log.Out("CVar '{0}' is readonly", new object[] { this.cvarName });
			return false;
		}
		return base.CanExecute(_eventType, _params);
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x00166C0E File Offset: 0x00164E0E
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

	// Token: 0x04002BB0 RID: 11184
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.SeedType seedType;

	// Token: 0x04002BB2 RID: 11186
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;

	// Token: 0x04002BB3 RID: 11187
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04002BB4 RID: 11188
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] valueList;

	// Token: 0x04002BB5 RID: 11189
	[PublicizedFrom(EAccessModifier.Private)]
	public float minValue;

	// Token: 0x04002BB6 RID: 11190
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxValue;

	// Token: 0x04002BB7 RID: 11191
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BB8 RID: 11192
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;

	// Token: 0x04002BB9 RID: 11193
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.RandomRollTypes rollType;

	// Token: 0x020006D8 RID: 1752
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002BBB RID: 11195
		Item,
		// Token: 0x04002BBC RID: 11196
		Player,
		// Token: 0x04002BBD RID: 11197
		Random
	}

	// Token: 0x020006D9 RID: 1753
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RandomRollTypes : byte
	{
		// Token: 0x04002BBF RID: 11199
		none,
		// Token: 0x04002BC0 RID: 11200
		randomInt,
		// Token: 0x04002BC1 RID: 11201
		randomFloat,
		// Token: 0x04002BC2 RID: 11202
		tierList
	}
}
