using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E8A RID: 3722
[Preserve]
public class XUiC_ComboBoxEnum<TEnum> : XUiC_ComboBox<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x17000D46 RID: 3398
	// (get) Token: 0x060072A5 RID: 29349 RVA: 0x002BEDE4 File Offset: 0x002BCFE4
	// (set) Token: 0x060072A6 RID: 29350 RVA: 0x002BEDEC File Offset: 0x002BCFEC
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D47 RID: 3399
	// (get) Token: 0x060072A7 RID: 29351 RVA: 0x002BEDF8 File Offset: 0x002BCFF8
	// (set) Token: 0x060072A8 RID: 29352 RVA: 0x002BEE28 File Offset: 0x002BD028
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

	// Token: 0x17000D48 RID: 3400
	// (get) Token: 0x060072A9 RID: 29353 RVA: 0x002BEEC0 File Offset: 0x002BD0C0
	// (set) Token: 0x060072AA RID: 29354 RVA: 0x002BEEF0 File Offset: 0x002BD0F0
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

	// Token: 0x17000D49 RID: 3401
	// (get) Token: 0x060072AB RID: 29355 RVA: 0x002BEF88 File Offset: 0x002BD188
	// (set) Token: 0x060072AC RID: 29356 RVA: 0x002BEF90 File Offset: 0x002BD190
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

	// Token: 0x17000D4A RID: 3402
	// (get) Token: 0x060072AD RID: 29357 RVA: 0x002BF029 File Offset: 0x002BD229
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

	// Token: 0x17000D4B RID: 3403
	// (get) Token: 0x060072AE RID: 29358 RVA: 0x002BF050 File Offset: 0x002BD250
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

	// Token: 0x17000D4C RID: 3404
	// (get) Token: 0x060072AF RID: 29359 RVA: 0x002BF083 File Offset: 0x002BD283
	// (set) Token: 0x060072B0 RID: 29360 RVA: 0x002BF0BC File Offset: 0x002BD2BC
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

	// Token: 0x17000D4D RID: 3405
	// (get) Token: 0x060072B1 RID: 29361 RVA: 0x002BF135 File Offset: 0x002BD335
	// (set) Token: 0x060072B2 RID: 29362 RVA: 0x002BF144 File Offset: 0x002BD344
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

	// Token: 0x17000D4E RID: 3406
	// (get) Token: 0x060072B3 RID: 29363 RVA: 0x002BF164 File Offset: 0x002BD364
	// (set) Token: 0x060072B4 RID: 29364 RVA: 0x002BF188 File Offset: 0x002BD388
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

	// Token: 0x17000D4F RID: 3407
	// (get) Token: 0x060072B5 RID: 29365 RVA: 0x002BF1AC File Offset: 0x002BD3AC
	// (set) Token: 0x060072B6 RID: 29366 RVA: 0x002BF1D0 File Offset: 0x002BD3D0
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

	// Token: 0x17000D50 RID: 3408
	// (get) Token: 0x060072B7 RID: 29367 RVA: 0x002BF1F4 File Offset: 0x002BD3F4
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.Count;
		}
	}

	// Token: 0x17000D51 RID: 3409
	// (get) Token: 0x060072B8 RID: 29368 RVA: 0x002BF201 File Offset: 0x002BD401
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.IndexOf(this.Value);
		}
	}

	// Token: 0x060072B9 RID: 29369 RVA: 0x002BF214 File Offset: 0x002BD414
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

	// Token: 0x060072BA RID: 29370 RVA: 0x002BF264 File Offset: 0x002BD464
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

	// Token: 0x060072BB RID: 29371 RVA: 0x002BF2CE File Offset: 0x002BD4CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TEnum _oldVal, TEnum _currentValue)
	{
		return _oldVal.Ordinal<TEnum>() != _currentValue.Ordinal<TEnum>();
	}

	// Token: 0x060072BC RID: 29372 RVA: 0x002BF2E1 File Offset: 0x002BD4E1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.changeIndex(-1);
	}

	// Token: 0x060072BD RID: 29373 RVA: 0x002BF2EA File Offset: 0x002BD4EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.changeIndex(1);
	}

	// Token: 0x060072BE RID: 29374 RVA: 0x002BF2F4 File Offset: 0x002BD4F4
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

	// Token: 0x060072BF RID: 29375 RVA: 0x002BF39C File Offset: 0x002BD59C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Max.Value.Ordinal<TEnum>();
	}

	// Token: 0x060072C0 RID: 29376 RVA: 0x002BF3CC File Offset: 0x002BD5CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Min.Value.Ordinal<TEnum>();
	}

	// Token: 0x060072C1 RID: 29377 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x060072C2 RID: 29378 RVA: 0x002BF3FC File Offset: 0x002BD5FC
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

	// Token: 0x060072C3 RID: 29379 RVA: 0x002BF4B5 File Offset: 0x002BD6B5
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x060072C4 RID: 29380 RVA: 0x002BF4D6 File Offset: 0x002BD6D6
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x060072C5 RID: 29381 RVA: 0x002BF4F7 File Offset: 0x002BD6F7
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

	// Token: 0x0400564A RID: 22090
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? min;

	// Token: 0x0400564B RID: 22091
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? max;

	// Token: 0x0400564C RID: 22092
	public readonly List<TEnum> Elements = new List<TEnum>();
}
