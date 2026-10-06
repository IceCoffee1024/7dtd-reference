using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E8F RID: 3727
[Preserve]
public class XUiC_ComboBoxList<TElement> : XUiC_ComboBox<TElement>
{
	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x0600730C RID: 29452 RVA: 0x002BFC4E File Offset: 0x002BDE4E
	// (set) Token: 0x0600730D RID: 29453 RVA: 0x002BFC56 File Offset: 0x002BDE56
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix
	{
		get
		{
			return this.localizationPrefix;
		}
		set
		{
			this.localizationPrefix = value;
			this.UpdateLabel();
		}
	}

	// Token: 0x17000D68 RID: 3432
	// (get) Token: 0x0600730E RID: 29454 RVA: 0x002BFC65 File Offset: 0x002BDE65
	// (set) Token: 0x0600730F RID: 29455 RVA: 0x002BFC6D File Offset: 0x002BDE6D
	[XuiXmlAttribute("localization_key_caseinsensitive", false)]
	public bool LocalizationKeyCaseInsensitive { get; set; }

	// Token: 0x17000D69 RID: 3433
	// (get) Token: 0x06007310 RID: 29456 RVA: 0x002BEF88 File Offset: 0x002BD188
	// (set) Token: 0x06007311 RID: 29457 RVA: 0x002BFC78 File Offset: 0x002BDE78
	public override TElement Value
	{
		get
		{
			return this.currentValue;
		}
		set
		{
			if (this.Elements.Contains(value))
			{
				int num = this.Elements.IndexOf(value);
				this.SelectedIndex = num;
				this.useCustomValue = false;
				return;
			}
			if (value != null)
			{
				this.CustomValue = value;
				this.useCustomValue = true;
				this.SelectedIndex = -1;
			}
		}
	}

	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06007312 RID: 29458 RVA: 0x002BFCCC File Offset: 0x002BDECC
	// (set) Token: 0x06007313 RID: 29459 RVA: 0x002BFD1C File Offset: 0x002BDF1C
	public override double RelativeValue
	{
		get
		{
			if (this.MaxIndex <= this.MinIndex)
			{
				return 0.0;
			}
			return ((double)((float)this.SelectedIndex) + 0.5 - (double)this.MinIndex) / (double)(this.MaxIndex - this.MinIndex + 1);
		}
		set
		{
			if (!this.UsesIndexMarkers)
			{
				return;
			}
			if (value >= 1.0)
			{
				this.SelectedIndex = this.MaxIndex;
				return;
			}
			if (value <= 0.0)
			{
				this.SelectedIndex = this.MinIndex;
				return;
			}
			this.SelectedIndex = Mathf.RoundToInt((float)value * (float)(this.MaxIndex - this.MinIndex + 1) + (float)this.MinIndex - 0.5f);
		}
	}

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06007314 RID: 29460 RVA: 0x002BFD90 File Offset: 0x002BDF90
	// (set) Token: 0x06007315 RID: 29461 RVA: 0x002BFD99 File Offset: 0x002BDF99
	public override long ValueGeneric
	{
		get
		{
			return (long)this.SelectedIndex;
		}
		set
		{
			this.SelectedIndex = (int)value;
		}
	}

	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x06007316 RID: 29462 RVA: 0x002BFDA3 File Offset: 0x002BDFA3
	// (set) Token: 0x06007317 RID: 29463 RVA: 0x002BFDAC File Offset: 0x002BDFAC
	public override long ValueMinGeneric
	{
		get
		{
			return (long)this.MinIndex;
		}
		set
		{
			this.MinIndex = (int)value;
		}
	}

	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x06007318 RID: 29464 RVA: 0x002BFDB6 File Offset: 0x002BDFB6
	// (set) Token: 0x06007319 RID: 29465 RVA: 0x002BFDBF File Offset: 0x002BDFBF
	public override long ValueMaxGeneric
	{
		get
		{
			return (long)this.MaxIndex;
		}
		set
		{
			this.MaxIndex = (int)value;
		}
	}

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x0600731A RID: 29466 RVA: 0x002BFDC9 File Offset: 0x002BDFC9
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxIndex - this.MinIndex + 1;
		}
	}

	// Token: 0x17000D6F RID: 3439
	// (get) Token: 0x0600731B RID: 29467 RVA: 0x002BFDDA File Offset: 0x002BDFDA
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (!this.reverseList)
			{
				return this.SelectedIndex - this.MinIndex;
			}
			return this.MaxIndex - this.SelectedIndex;
		}
	}

	// Token: 0x17000D70 RID: 3440
	// (get) Token: 0x0600731C RID: 29468 RVA: 0x002BFDFF File Offset: 0x002BDFFF
	// (set) Token: 0x0600731D RID: 29469 RVA: 0x002BFE08 File Offset: 0x002BE008
	public int SelectedIndex
	{
		get
		{
			return this.currentIndex;
		}
		set
		{
			if (value < 0)
			{
				if (this.useCustomValue)
				{
					this.currentIndex = value;
					this.currentValue = this.CustomValue;
				}
				else
				{
					this.currentIndex = this.MinIndex;
					this.currentValue = ((this.currentIndex < 0 || this.currentIndex >= this.Elements.Count) ? default(TElement) : this.Elements[this.currentIndex]);
				}
			}
			else
			{
				if (value >= this.Elements.Count)
				{
					value = this.Elements.Count - 1;
				}
				if (value < this.MinIndex)
				{
					value = this.MinIndex;
				}
				else if (value > this.MaxIndex)
				{
					value = this.MaxIndex;
				}
				this.currentIndex = value;
				this.currentValue = ((this.currentIndex < 0 || this.currentIndex >= this.Elements.Count) ? default(TElement) : this.Elements[this.currentIndex]);
			}
			this.IsDirty = true;
			this.UpdateLabel();
		}
	}

	// Token: 0x17000D71 RID: 3441
	// (get) Token: 0x0600731E RID: 29470 RVA: 0x002BFF1B File Offset: 0x002BE11B
	// (set) Token: 0x0600731F RID: 29471 RVA: 0x002BFF2E File Offset: 0x002BE12E
	[XuiXmlAttribute("index_min", false)]
	public int MinIndex
	{
		get
		{
			if (this.minIndex >= 0)
			{
				return this.minIndex;
			}
			return 0;
		}
		set
		{
			if (value != this.minIndex)
			{
				this.minIndex = value;
				if (this.SelectedIndex < this.minIndex)
				{
					this.SelectedIndex = this.minIndex;
				}
				base.UpdateIndexMarkerPositions();
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x06007320 RID: 29472 RVA: 0x002BFF67 File Offset: 0x002BE167
	// (set) Token: 0x06007321 RID: 29473 RVA: 0x002BFF9C File Offset: 0x002BE19C
	[XuiXmlAttribute("index_max", false)]
	public int MaxIndex
	{
		get
		{
			if (this.maxIndex < this.Elements.Count && this.maxIndex >= 0)
			{
				return this.maxIndex;
			}
			return this.Elements.Count - 1;
		}
		set
		{
			if (value != this.maxIndex)
			{
				this.maxIndex = value;
				if (this.maxIndex >= 0 && this.SelectedIndex > this.maxIndex)
				{
					this.SelectedIndex = this.maxIndex;
				}
				base.UpdateIndexMarkerPositions();
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x06007322 RID: 29474 RVA: 0x002BFFEC File Offset: 0x002BE1EC
	public override void OnOpen()
	{
		if (this.Elements.Count > 0 && !this.useCustomValue)
		{
			if (this.SelectedIndex < 0 && !this.useCustomValue)
			{
				this.SelectedIndex = 0;
			}
			if (this.SelectedIndex > this.Elements.Count)
			{
				this.SelectedIndex = this.Elements.Count - 1;
			}
			if (this.SelectedIndex < this.MinIndex)
			{
				this.SelectedIndex = this.MinIndex;
			}
			if (this.SelectedIndex > this.MaxIndex)
			{
				this.SelectedIndex = this.MaxIndex;
			}
			if (!this.useCustomValue)
			{
				this.currentValue = ((this.SelectedIndex < this.Elements.Count) ? this.Elements[this.SelectedIndex] : default(TElement));
			}
		}
		base.OnOpen();
	}

	// Token: 0x06007323 RID: 29475 RVA: 0x002C00CC File Offset: 0x002BE2CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateLabel()
	{
		if (this.isEmpty())
		{
			base.ValueText = "";
			return;
		}
		TElement currentValue = this.currentValue;
		if (this.CustomValueFormatter != null)
		{
			base.ValueText = this.CustomValueFormatter(this.currentValue);
			return;
		}
		string text = ((currentValue == null) ? "" : currentValue.ToString());
		base.ValueText = ((!string.IsNullOrEmpty(this.LocalizationPrefix)) ? Localization.Get(this.LocalizationPrefix + text, this.LocalizationKeyCaseInsensitive, null) : text);
	}

	// Token: 0x06007324 RID: 29476 RVA: 0x002C015F File Offset: 0x002BE35F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TElement _oldVal, TElement _currentValue)
	{
		return (_oldVal != null || _currentValue != null) && (_oldVal == null || _currentValue == null || !_oldVal.Equals(_currentValue));
	}

	// Token: 0x06007325 RID: 29477 RVA: 0x002C019B File Offset: 0x002BE39B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.ChangeIndex(this.reverseList ? 1 : (-1));
	}

	// Token: 0x06007326 RID: 29478 RVA: 0x002C01AF File Offset: 0x002BE3AF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.ChangeIndex(this.reverseList ? (-1) : 1);
	}

	// Token: 0x06007327 RID: 29479 RVA: 0x002C01C4 File Offset: 0x002BE3C4
	public void ChangeIndex(int _direction)
	{
		int num = this.SelectedIndex + _direction;
		if (num < this.MinIndex)
		{
			num = (this.Wrap ? Utils.FastMin(this.Elements.Count - 1, this.MaxIndex) : this.MinIndex);
		}
		if (num > this.MaxIndex)
		{
			num = (this.Wrap ? Utils.FastMax(0, this.MinIndex) : this.MaxIndex);
		}
		if (num < 0)
		{
			num = (this.Wrap ? (this.Elements.Count - 1) : 0);
		}
		if (num >= this.Elements.Count)
		{
			num = (this.Wrap ? 0 : (this.Elements.Count - 1));
		}
		this.SelectedIndex = num;
	}

	// Token: 0x06007328 RID: 29480 RVA: 0x002C027D File Offset: 0x002BE47D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		if (!this.reverseList)
		{
			return this.isMaxIndex();
		}
		return this.isMinIndex();
	}

	// Token: 0x06007329 RID: 29481 RVA: 0x002C0294 File Offset: 0x002BE494
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		if (!this.reverseList)
		{
			return this.isMinIndex();
		}
		return this.isMaxIndex();
	}

	// Token: 0x0600732A RID: 29482 RVA: 0x002C02AB File Offset: 0x002BE4AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return this.Elements.Count == 0;
	}

	// Token: 0x0600732B RID: 29483 RVA: 0x002C02BB File Offset: 0x002BE4BB
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMaxIndex()
	{
		return this.SelectedIndex == this.MaxIndex || this.SelectedIndex == this.Elements.Count - 1;
	}

	// Token: 0x0600732C RID: 29484 RVA: 0x002C02E2 File Offset: 0x002BE4E2
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMinIndex()
	{
		return this.SelectedIndex == this.MinIndex || this.SelectedIndex == 0;
	}

	// Token: 0x0600732D RID: 29485 RVA: 0x002BF4F7 File Offset: 0x002BD6F7
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

	// Token: 0x0600732E RID: 29486 RVA: 0x002C0300 File Offset: 0x002BE500
	[XuiXmlAttribute("values", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValues(string _value)
	{
		if (_value.EqualsCaseInsensitive("@def"))
		{
			return;
		}
		string[] array = _value.Split(',', StringSplitOptions.None);
		Type typeFromHandle = typeof(TElement);
		if (typeFromHandle == typeof(string))
		{
			foreach (string text in array)
			{
				this.Elements.Add((TElement)((object)text.Trim()));
			}
			return;
		}
		if (typeof(IConvertible).IsAssignableFrom(typeFromHandle))
		{
			foreach (string text2 in array)
			{
				try
				{
					this.Elements.Add((TElement)((object)Convert.ChangeType(text2, typeFromHandle)));
				}
				catch (Exception ex)
				{
					Log.Error(string.Format("[XUi] Value \"{0}\" not supported for the ComboBox type {1}", text2, typeFromHandle));
					Log.Exception(ex);
				}
			}
			return;
		}
		ConstructorInfo constructor = typeFromHandle.GetConstructor(new Type[] { typeof(string) });
		if (constructor == null)
		{
			Log.Error(string.Format("[XUi] ComboBox type {0} does not support values from string elements", typeFromHandle));
			return;
		}
		foreach (string text3 in array)
		{
			object[] array3 = new object[1];
			try
			{
				array3[0] = text3;
				this.Elements.Add((TElement)((object)constructor.Invoke(array3)));
			}
			catch (Exception ex2)
			{
				Log.Error(string.Format("[XUi] Value \"{0}\" not supported for the ComboBox type {1}", text3, typeFromHandle));
				Log.Exception(ex2);
			}
		}
	}

	// Token: 0x0600732F RID: 29487 RVA: 0x002C0474 File Offset: 0x002BE674
	[XuiXmlAttribute("reverse_list", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeReverseList(string _value)
	{
		if (_value.EqualsCaseInsensitive("@def"))
		{
			return;
		}
		this.reverseList = StringParsers.ParseBool(_value, 0, -1, true);
	}

	// Token: 0x04005656 RID: 22102
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizationPrefix;

	// Token: 0x04005658 RID: 22104
	public readonly List<TElement> Elements = new List<TElement>();

	// Token: 0x04005659 RID: 22105
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex = int.MinValue;

	// Token: 0x0400565A RID: 22106
	public TElement CustomValue;

	// Token: 0x0400565B RID: 22107
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useCustomValue;

	// Token: 0x0400565C RID: 22108
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex = int.MinValue;

	// Token: 0x0400565D RID: 22109
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxIndex = int.MaxValue;

	// Token: 0x0400565E RID: 22110
	[PublicizedFrom(EAccessModifier.Private)]
	public bool reverseList;
}
