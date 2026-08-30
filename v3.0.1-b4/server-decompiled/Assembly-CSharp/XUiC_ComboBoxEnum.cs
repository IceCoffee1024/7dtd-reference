using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E66 RID: 3686
[Preserve]
public class XUiC_ComboBoxEnum<TEnum> : XUiC_ComboBox<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x0600720D RID: 29197 RVA: 0x002B8510 File Offset: 0x002B6710
	// (set) Token: 0x0600720E RID: 29198 RVA: 0x002B8518 File Offset: 0x002B6718
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x0600720F RID: 29199 RVA: 0x002B8524 File Offset: 0x002B6724
	// (set) Token: 0x06007210 RID: 29200 RVA: 0x002B8554 File Offset: 0x002B6754
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

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x06007211 RID: 29201 RVA: 0x002B85EC File Offset: 0x002B67EC
	// (set) Token: 0x06007212 RID: 29202 RVA: 0x002B861C File Offset: 0x002B681C
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

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x06007213 RID: 29203 RVA: 0x002B86B4 File Offset: 0x002B68B4
	// (set) Token: 0x06007214 RID: 29204 RVA: 0x002B86BC File Offset: 0x002B68BC
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

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x06007215 RID: 29205 RVA: 0x002B8755 File Offset: 0x002B6955
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

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x06007216 RID: 29206 RVA: 0x002B877C File Offset: 0x002B697C
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

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x06007217 RID: 29207 RVA: 0x002B87AF File Offset: 0x002B69AF
	// (set) Token: 0x06007218 RID: 29208 RVA: 0x002B87E8 File Offset: 0x002B69E8
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

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x06007219 RID: 29209 RVA: 0x002B8861 File Offset: 0x002B6A61
	// (set) Token: 0x0600721A RID: 29210 RVA: 0x002B8870 File Offset: 0x002B6A70
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

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x0600721B RID: 29211 RVA: 0x002B8890 File Offset: 0x002B6A90
	// (set) Token: 0x0600721C RID: 29212 RVA: 0x002B88B4 File Offset: 0x002B6AB4
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

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x0600721D RID: 29213 RVA: 0x002B88D8 File Offset: 0x002B6AD8
	// (set) Token: 0x0600721E RID: 29214 RVA: 0x002B88FC File Offset: 0x002B6AFC
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

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x0600721F RID: 29215 RVA: 0x002B8920 File Offset: 0x002B6B20
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.Count;
		}
	}

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x06007220 RID: 29216 RVA: 0x002B892D File Offset: 0x002B6B2D
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.IndexOf(this.Value);
		}
	}

	// Token: 0x06007221 RID: 29217 RVA: 0x002B8940 File Offset: 0x002B6B40
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

	// Token: 0x06007222 RID: 29218 RVA: 0x002B8990 File Offset: 0x002B6B90
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

	// Token: 0x06007223 RID: 29219 RVA: 0x002B89FA File Offset: 0x002B6BFA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TEnum _oldVal, TEnum _currentValue)
	{
		return _oldVal.Ordinal<TEnum>() != _currentValue.Ordinal<TEnum>();
	}

	// Token: 0x06007224 RID: 29220 RVA: 0x002B8A0D File Offset: 0x002B6C0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.changeIndex(-1);
	}

	// Token: 0x06007225 RID: 29221 RVA: 0x002B8A16 File Offset: 0x002B6C16
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.changeIndex(1);
	}

	// Token: 0x06007226 RID: 29222 RVA: 0x002B8A20 File Offset: 0x002B6C20
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

	// Token: 0x06007227 RID: 29223 RVA: 0x002B8AC8 File Offset: 0x002B6CC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Max.Value.Ordinal<TEnum>();
	}

	// Token: 0x06007228 RID: 29224 RVA: 0x002B8AF8 File Offset: 0x002B6CF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Min.Value.Ordinal<TEnum>();
	}

	// Token: 0x06007229 RID: 29225 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x0600722A RID: 29226 RVA: 0x002B8B28 File Offset: 0x002B6D28
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

	// Token: 0x0600722B RID: 29227 RVA: 0x002B8BE1 File Offset: 0x002B6DE1
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x0600722C RID: 29228 RVA: 0x002B8C02 File Offset: 0x002B6E02
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x0600722D RID: 29229 RVA: 0x002B8C23 File Offset: 0x002B6E23
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

	// Token: 0x0400556F RID: 21871
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? min;

	// Token: 0x04005570 RID: 21872
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? max;

	// Token: 0x04005571 RID: 21873
	public readonly List<TEnum> Elements = new List<TEnum>();
}
