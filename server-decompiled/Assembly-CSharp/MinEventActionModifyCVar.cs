using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006DF RID: 1759
[Preserve]
public class MinEventActionModifyCVar : MinEventActionTargetedBase
{
	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x0600367F RID: 13951 RVA: 0x00167582 File Offset: 0x00165782
	// (set) Token: 0x06003680 RID: 13952 RVA: 0x0016758A File Offset: 0x0016578A
	public string cvarName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06003681 RID: 13953 RVA: 0x00167593 File Offset: 0x00165793
	// (set) Token: 0x06003682 RID: 13954 RVA: 0x0016759B File Offset: 0x0016579B
	public float value
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06003683 RID: 13955 RVA: 0x001675A4 File Offset: 0x001657A4
	// (set) Token: 0x06003684 RID: 13956 RVA: 0x001675AC File Offset: 0x001657AC
	public bool cvarRef
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06003685 RID: 13957 RVA: 0x001675B5 File Offset: 0x001657B5
	// (set) Token: 0x06003686 RID: 13958 RVA: 0x001675BD File Offset: 0x001657BD
	public string refCvarName
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = string.Empty;

	// Token: 0x06003687 RID: 13959 RVA: 0x001675C8 File Offset: 0x001657C8
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

	// Token: 0x06003688 RID: 13960 RVA: 0x00167778 File Offset: 0x00165978
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

	// Token: 0x06003689 RID: 13961 RVA: 0x001679B7 File Offset: 0x00165BB7
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.cvarName != null && this.cvarName.StartsWith("_"))
		{
			Log.Out("CVar '{0}' is readonly", new object[] { this.cvarName });
			return false;
		}
		return base.CanExecute(_eventType, _params);
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x001679F6 File Offset: 0x00165BF6
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

	// Token: 0x04002BCC RID: 11212
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.SeedType seedType;

	// Token: 0x04002BCE RID: 11214
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;

	// Token: 0x04002BD0 RID: 11216
	public float[] valueList;

	// Token: 0x04002BD1 RID: 11217
	[PublicizedFrom(EAccessModifier.Private)]
	public float minValue;

	// Token: 0x04002BD2 RID: 11218
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxValue;

	// Token: 0x04002BD5 RID: 11221
	[PublicizedFrom(EAccessModifier.Private)]
	public MinEventActionModifyCVar.RandomRollTypes rollType;

	// Token: 0x020006E0 RID: 1760
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SeedType
	{
		// Token: 0x04002BD7 RID: 11223
		Item,
		// Token: 0x04002BD8 RID: 11224
		Player,
		// Token: 0x04002BD9 RID: 11225
		Random
	}

	// Token: 0x020006E1 RID: 1761
	[PublicizedFrom(EAccessModifier.Private)]
	public enum RandomRollTypes : byte
	{
		// Token: 0x04002BDB RID: 11227
		none,
		// Token: 0x04002BDC RID: 11228
		randomInt,
		// Token: 0x04002BDD RID: 11229
		randomFloat,
		// Token: 0x04002BDE RID: 11230
		tierList
	}
}
