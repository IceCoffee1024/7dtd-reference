using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E7B RID: 3707
[Preserve]
public class XUiC_ComboBoxEnum<TEnum> : XUiC_ComboBox<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x060072AD RID: 29357 RVA: 0x002BBA34 File Offset: 0x002B9C34
	// (set) Token: 0x060072AE RID: 29358 RVA: 0x002BBA3C File Offset: 0x002B9C3C
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x060072AF RID: 29359 RVA: 0x002BBA48 File Offset: 0x002B9C48
	// (set) Token: 0x060072B0 RID: 29360 RVA: 0x002BBA78 File Offset: 0x002B9C78
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

	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x060072B1 RID: 29361 RVA: 0x002BBB10 File Offset: 0x002B9D10
	// (set) Token: 0x060072B2 RID: 29362 RVA: 0x002BBB40 File Offset: 0x002B9D40
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

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x060072B3 RID: 29363 RVA: 0x002BBBD8 File Offset: 0x002B9DD8
	// (set) Token: 0x060072B4 RID: 29364 RVA: 0x002BBBE0 File Offset: 0x002B9DE0
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

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x060072B5 RID: 29365 RVA: 0x002BBC79 File Offset: 0x002B9E79
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

	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x060072B6 RID: 29366 RVA: 0x002BBCA0 File Offset: 0x002B9EA0
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

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x060072B7 RID: 29367 RVA: 0x002BBCD3 File Offset: 0x002B9ED3
	// (set) Token: 0x060072B8 RID: 29368 RVA: 0x002BBD0C File Offset: 0x002B9F0C
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

	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x060072B9 RID: 29369 RVA: 0x002BBD85 File Offset: 0x002B9F85
	// (set) Token: 0x060072BA RID: 29370 RVA: 0x002BBD94 File Offset: 0x002B9F94
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

	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x060072BB RID: 29371 RVA: 0x002BBDB4 File Offset: 0x002B9FB4
	// (set) Token: 0x060072BC RID: 29372 RVA: 0x002BBDD8 File Offset: 0x002B9FD8
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

	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x060072BD RID: 29373 RVA: 0x002BBDFC File Offset: 0x002B9FFC
	// (set) Token: 0x060072BE RID: 29374 RVA: 0x002BBE20 File Offset: 0x002BA020
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

	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x060072BF RID: 29375 RVA: 0x002BBE44 File Offset: 0x002BA044
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.Count;
		}
	}

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x060072C0 RID: 29376 RVA: 0x002BBE51 File Offset: 0x002BA051
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Elements.IndexOf(this.Value);
		}
	}

	// Token: 0x060072C1 RID: 29377 RVA: 0x002BBE64 File Offset: 0x002BA064
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

	// Token: 0x060072C2 RID: 29378 RVA: 0x002BBEB4 File Offset: 0x002BA0B4
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

	// Token: 0x060072C3 RID: 29379 RVA: 0x002BBF1E File Offset: 0x002BA11E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TEnum _oldVal, TEnum _currentValue)
	{
		return _oldVal.Ordinal<TEnum>() != _currentValue.Ordinal<TEnum>();
	}

	// Token: 0x060072C4 RID: 29380 RVA: 0x002BBF31 File Offset: 0x002BA131
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.changeIndex(-1);
	}

	// Token: 0x060072C5 RID: 29381 RVA: 0x002BBF3A File Offset: 0x002BA13A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.changeIndex(1);
	}

	// Token: 0x060072C6 RID: 29382 RVA: 0x002BBF44 File Offset: 0x002BA144
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

	// Token: 0x060072C7 RID: 29383 RVA: 0x002BBFEC File Offset: 0x002BA1EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Max.Value.Ordinal<TEnum>();
	}

	// Token: 0x060072C8 RID: 29384 RVA: 0x002BC01C File Offset: 0x002BA21C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.Ordinal<TEnum>() == this.Min.Value.Ordinal<TEnum>();
	}

	// Token: 0x060072C9 RID: 29385 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x060072CA RID: 29386 RVA: 0x002BC04C File Offset: 0x002BA24C
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

	// Token: 0x060072CB RID: 29387 RVA: 0x002BC105 File Offset: 0x002BA305
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x060072CC RID: 29388 RVA: 0x002BC126 File Offset: 0x002BA326
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = new TEnum?(EnumUtils.Parse<TEnum>(_value, false));
		}
	}

	// Token: 0x060072CD RID: 29389 RVA: 0x002BC147 File Offset: 0x002BA347
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

	// Token: 0x04005613 RID: 22035
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? min;

	// Token: 0x04005614 RID: 22036
	[PublicizedFrom(EAccessModifier.Private)]
	public TEnum? max;

	// Token: 0x04005615 RID: 22037
	public readonly List<TEnum> Elements = new List<TEnum>();
}
