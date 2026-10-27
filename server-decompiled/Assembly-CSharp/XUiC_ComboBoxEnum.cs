using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E95 RID: 3733
[Preserve]
public class XUiC_ComboBoxEnum<TEnum> : XUiC_ComboBox<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x17000D4C RID: 3404
	// (get) Token: 0x060072CA RID: 29386 RVA: 0x002C0350 File Offset: 0x002BE550
	// (set) Token: 0x060072CB RID: 29387 RVA: 0x002C0358 File Offset: 0x002BE558
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D4D RID: 3405
	// (get) Token: 0x060072CC RID: 29388 RVA: 0x002C0364 File Offset: 0x002BE564
	// (set) Token: 0x060072CD RID: 29389 RVA: 0x002C0394 File Offset: 0x002BE594
	public TEnum? Min
	{
		get
		{
			return new TEnum?(this.min ?? EnumUtils.MinValue<TEnum>());
		}
		set
		{
			if (value != null == (this.min != null) && (value == null || value.Value.Ordinal<TEnum>() == this.min.Value.Ordinal<TEnum>()))
			{
				return;
			}
			this.min = value;
			if (this.min != null && this.Value.Ordinal<TEnum>() < this.min.Value.Ordinal<TEnum>())
			{
				this.Value = this.min.Value;
			}
			base.UpdateIndexMarkerPositions();
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D4E RID: 3406
	// (get) Token: 0x060072CE RID: 29390 RVA: 0x002C042C File Offset: 0x002BE62C
	// (set) Token: 0x060072CF RID: 29391 RVA: 0x002C045C File Offset: 0x002BE65C
	public TEnum? Max
	{
		get
		{
			return new TEnum?(this.max ?? EnumUtils.MaxValue<TEnum>());
		}
		set
		{
			if (value != null == (this.max != null) && (value == null || value.Value.Ordinal<TEnum>() == this.max.Value.Ordinal<TEnum>()))
			{
				return;
			}
			this.max = value;
			if (this.max != null && this.Value.Ordinal<TEnum>() > this.max.Value.Ordinal<TEnum>())
			{
				this.Value = this.max.Value;
			}
			base.UpdateIndexMarkerPositions();
			this.IsDirty = true;
		}
	}

	// Token: 0x17000D4F RID: 3407
	// (get) Token: 0x060072D0 RID: 29392 RVA: 0x002C04F4 File Offset: 0x002BE6F4
	// (set) Token: 0x060072D1 RID: 29393 RVA: 0x002C04FC File Offset: 0x002BE6FC
	public override TEnum Value
	{
		get
		{
			return this.currentValue;
		}
		set
		{
			if (this.currentValue.Ordinal<TEnum>() == value.Ordinal<TEnum>())
			{
				return;
			}
			if (this.max != null && value.Ordinal<TEnum>() > this.max.Value.Ordinal<TEnum>())
			{
				value = this.max.Value;
			}
			if (this.min != null && value.Ordinal<TEnum>() < this.min.Value.Ordinal<TEnum>())
			{
				value = this.min.Value;
			}
			this.currentValue = value;
			this.IsDirty = true;
			this.UpdateLabel();
		}
	}

	// Token: 0x17000D50 RID: 3408
	// (get) Token: 0x060072D2 RID: 29394 RVA: 0x002C0595 File Offset: 0x002BE795
	public int MinIndex
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.min == null)
			{
				return 0;
			}
			return this.Elements.IndexOf(this.min.Value);
		}
	}

	// Token: 0x17000D51 RID: 3409
	// (get) Token: 0x060072D3 RID: 29395 RVA: 0x002C05BC File Offset: 0x002BE7BC
	public int MaxIndex
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.max == null)
			{
				return this.Elements.Count - 1;
			}
			return this.Elements.IndexOf(this.max.Value);
		}
	}

	// Token: 0x17000D52 RID: 3410
	// (get) Token: 0x060072D4 RID: 29396 RVA: 0x002C05EF File Offset: 0x002BE7EF
	// (set) Token: 0x060072D5 RID: 29397 RVA: 0x002C0628 File Offset: 0x002BE828
	public override double RelativeValue
	{
		get
		{
			return ((double)((float)this.Elements.IndexOf(this.Value)) + 0.5 - (double)this.MinIndex) / (double)(this.MaxIndex - this.MinIndex + 1);
		}
		set
		{
			if (!this.UsesIndexMarkers)
			{
				return;
			}
			int num;
			if (value >= 1.0)
			{
				num = this.MaxIndex;
			}
			else if (value <= 0.0)
			{
				num = this.MinIndex;
			}
			else
			{
				num = Mathf.RoundToInt((float)value * (float)(this.MaxIndex - this.MinIndex + 1) + (float)this.MinIndex - 0.5f);
			}
			this.Value = this.Elements[num];
		}
	}

	// Token: 0x17000D53 RID: 3411
	// (get) Token: 0x060072D6 RID: 29398 RVA: 0x002C06A1 File Offset: 0x002BE8A1
	// (set) Token: 0x060072D7 RID: 29399 RVA: 0x002C06B0 File Offset: 0x002BE8B0
	public override long ValueGeneric
	{
		get
		{
			return (long)this.Value.Ordinal<TEnum>();
		}
		set
		{
			TEnum tenum;
			if (EnumUtils.TryFromOrdinal<TEnum>((int)value, out tenum))
			{
				this.Value = tenum;
			}
		}
	}

	// Token: 0x17000D54 RID: 3412
	// (get) Token: 0x060072D8 RID: 29400 RVA: 0x002C06D0 File Offset: 0x002BE8D0
	// (set) Token: 0x060072D9 RID: 29401 RVA: 0x002C06F4 File Offset: 0x002BE8F4
	public override long ValueMinGeneric
	{
		get
		{
			return (long)this.Min.Value.Ordinal<TEnum>();
		}
		set
		{
			TEnum tenum;
			if (EnumUtils.TryFromOrdinal<TEnum>((int)value, out tenum))
			{
				this.Min = new TEnum?(tenum);
			}
		}
	}

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x060072DA RID: 29402 RVA: 0x002C0718 File Offset: 0x002BE918
	// (set) Token: 0x060072DB RID: 29403 RVA: 0x002C073C File Offset: 0x002BE93C
	public override long ValueMaxGeneric
	{
		get
		{
			return (long)this.Max.Value.Ordinal<TEnum>();
		}
		set
		{
			TEnum tenum;
			if (EnumUtils.TryFromOrdinal<TEnum>((int)value, out tenum))
			{
				this.Max = new TEnum?(tenum);
			}
		}
	}

	// Token: 0x17000D56 RID: 3414
	// (get) Token: 0x060072DC RID: 29404 RVA: 0x002C0760 File Offset: 0x002BE960
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.Count;
		}
	}

	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x060072DD RID: 29405 RVA: 0x002C076D File Offset: 0x002BE96D
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.IndexOf(this.Value);
		}
	}

	// Token: 0x060072DE RID: 29406 RVA: 0x002C0780 File Offset: 0x002BE980
	public override void Init()
	{
		base.Init();
		if (this.Elements.Count == 0)
		{
			this.Elements.AddRange(EnumUtils.Values<TEnum>());
			if (this.Elements.Count > 0)
			{
				this.Value = this.Elements[0];
			}
		}
	}

	// Token: 0x060072DF RID: 29407 RVA: 0x002C07D0 File Offset: 0x002BE9D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateLabel()
	{
		if (this.CustomValueFormatter != null)
		{
			base.ValueText = this.CustomValueFormatter(this.currentValue);
			return;
		}
		base.ValueText = ((!string.IsNullOrEmpty(this.LocalizationPrefix)) ? Localization.Get(this.LocalizationPrefix + this.currentValue.ToStringCached<TEnum>(), false, null) : this.currentValue.ToStringCached<TEnum>());
	}

	// Token: 0x060072E0 RID: 29408 RVA: 0x002C083A File Offset: 0x002BEA3A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TEnum _oldVal, TEnum _currentValue)
	{
		return _oldVal.Ordinal<TEnum>() != _currentValue.Ordinal<TEnum>();
	}

	// Token: 0x060072E1 RID: 29409 RVA: 0x002C084D File Offset: 0x002BEA4D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.changeIndex(-1);
	}

	// Token: 0x060072E2 RID: 29410 RVA: 0x002C0856 File Offset: 0x002BEA56
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.changeIndex(1);
	}

	// Token: 0x060072E3 RID: 29411 RVA: 0x002C0860 File Offset: 0x002BEA60
	[PublicizedFrom(EAccessModifier.Private)]
	public void changeIndex(int _direction)
	{
		int num = this.Elements.IndexOf(this.Value);
		int num2 = num + _direction;
		if (num2 < 0 || num2 < this.MinIndex)
		{
			if (this.Wrap)
			{
				num2 = this.Elements.IndexOf(this.Max.Value);
			}
			else
			{
				num2 = num;
			}
		}
		if (num2 >= this.Elements.Count || num2 > this.MaxIndex)
		{
			if (this.Wrap)
			{
				num2 = this.Elements.IndexOf(this.Min.Value);
			}
			else
			{
				num2 = num;
			}
		}
		this.Value = this.Elements[num2];
	}

	// Token: 0x060072E4 RID: 29412 RVA: 0x002C0908 File Offset: 0x002BEB08
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Max.Value.Ordinal<TEnum>();
	}

	// Token: 0x060072E5 RID: 29413 RVA: 0x002C0938 File Offset: 0x002BEB38
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Min.Value.Ordinal<TEnum>();
	}

	// Token: 0x060072E6 RID: 29414 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x060072E7 RID: 29415 RVA: 0x002C0968 File Offset: 0x002BEB68
	[XuiXmlAttribute("values", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValues(string _value)
	{
		if (_value.EqualsCaseInsensitive("@def"))
		{
			return;
		}
		this.Elements.Clear();
		foreach (string text in _value.Split(',', StringSplitOptions.None))
		{
			TEnum tenum;
			if (!EnumUtils.TryParse<TEnum>(text, out tenum, true))
			{
				Log.Error(string.Concat(new string[]
				{
					"[XUi] Value \"",
					text,
					"\" is not a member of the ",
					typeof(TEnum).FullName,
					" enum."
				}));
			}
			else
			{
				this.Elements.Add(tenum);
			}
		}
		if (this.Elements.Count > 0)
		{
			this.Value = this.Elements[0];
		}
	}

	// Token: 0x060072E8 RID: 29416 RVA: 0x002C0A21 File Offset: 0x002BEC21
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x060072E9 RID: 29417 RVA: 0x002C0A42 File Offset: 0x002BEC42
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x060072EA RID: 29418 RVA: 0x002C0A63 File Offset: 0x002BEC63
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void incrementalChangeValue(double _value)
	{
		if (_value > 0.0)
		{
			this.TryPageUp();
			return;
		}
		if (_value < 0.0)
		{
			this.TryPageDown();
		}
	}

	// Token: 0x0400566F RID: 22127
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? min;

	// Token: 0x04005670 RID: 22128
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? max;

	// Token: 0x04005671 RID: 22129
	public readonly List<TEnum> Elements = new List<TEnum>();
}
