using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E80 RID: 3712
[Preserve]
public class XUiC_ComboBoxList<TElement> : XUiC_ComboBox<TElement>
{
	// Token: 0x17000D4C RID: 3404
	// (get) Token: 0x06007314 RID: 29460 RVA: 0x002BC89E File Offset: 0x002BAA9E
	// (set) Token: 0x06007315 RID: 29461 RVA: 0x002BC8A6 File Offset: 0x002BAAA6
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

	// Token: 0x17000D4D RID: 3405
	// (get) Token: 0x06007316 RID: 29462 RVA: 0x002BC8B5 File Offset: 0x002BAAB5
	// (set) Token: 0x06007317 RID: 29463 RVA: 0x002BC8BD File Offset: 0x002BAABD
	[XuiXmlAttribute("localization_key_caseinsensitive", false)]
	public bool LocalizationKeyCaseInsensitive { get; set; }

	// Token: 0x17000D4E RID: 3406
	// (get) Token: 0x06007318 RID: 29464 RVA: 0x002BBBD8 File Offset: 0x002B9DD8
	// (set) Token: 0x06007319 RID: 29465 RVA: 0x002BC8C8 File Offset: 0x002BAAC8
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

	// Token: 0x17000D4F RID: 3407
	// (get) Token: 0x0600731A RID: 29466 RVA: 0x002BC91C File Offset: 0x002BAB1C
	// (set) Token: 0x0600731B RID: 29467 RVA: 0x002BC96C File Offset: 0x002BAB6C
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

	// Token: 0x17000D50 RID: 3408
	// (get) Token: 0x0600731C RID: 29468 RVA: 0x002BC9E0 File Offset: 0x002BABE0
	// (set) Token: 0x0600731D RID: 29469 RVA: 0x002BC9E9 File Offset: 0x002BABE9
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

	// Token: 0x17000D51 RID: 3409
	// (get) Token: 0x0600731E RID: 29470 RVA: 0x002BC9F3 File Offset: 0x002BABF3
	// (set) Token: 0x0600731F RID: 29471 RVA: 0x002BC9FC File Offset: 0x002BABFC
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

	// Token: 0x17000D52 RID: 3410
	// (get) Token: 0x06007320 RID: 29472 RVA: 0x002BCA06 File Offset: 0x002BAC06
	// (set) Token: 0x06007321 RID: 29473 RVA: 0x002BCA0F File Offset: 0x002BAC0F
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

	// Token: 0x17000D53 RID: 3411
	// (get) Token: 0x06007322 RID: 29474 RVA: 0x002BCA19 File Offset: 0x002BAC19
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxIndex - this.MinIndex + 1;
		}
	}

	// Token: 0x17000D54 RID: 3412
	// (get) Token: 0x06007323 RID: 29475 RVA: 0x002BCA2A File Offset: 0x002BAC2A
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

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x06007324 RID: 29476 RVA: 0x002BCA4F File Offset: 0x002BAC4F
	// (set) Token: 0x06007325 RID: 29477 RVA: 0x002BCA58 File Offset: 0x002BAC58
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

	// Token: 0x17000D56 RID: 3414
	// (get) Token: 0x06007326 RID: 29478 RVA: 0x002BCB6B File Offset: 0x002BAD6B
	// (set) Token: 0x06007327 RID: 29479 RVA: 0x002BCB7E File Offset: 0x002BAD7E
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

	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x06007328 RID: 29480 RVA: 0x002BCBB7 File Offset: 0x002BADB7
	// (set) Token: 0x06007329 RID: 29481 RVA: 0x002BCBEC File Offset: 0x002BADEC
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

	// Token: 0x0600732A RID: 29482 RVA: 0x002BCC3C File Offset: 0x002BAE3C
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

	// Token: 0x0600732B RID: 29483 RVA: 0x002BCD1C File Offset: 0x002BAF1C
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

	// Token: 0x0600732C RID: 29484 RVA: 0x002BCDAF File Offset: 0x002BAFAF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TElement _oldVal, TElement _currentValue)
	{
		return (_oldVal != null || _currentValue != null) && (_oldVal == null || _currentValue == null || !_oldVal.Equals(_currentValue));
	}

	// Token: 0x0600732D RID: 29485 RVA: 0x002BCDEB File Offset: 0x002BAFEB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.ChangeIndex(this.reverseList ? 1 : (-1));
	}

	// Token: 0x0600732E RID: 29486 RVA: 0x002BCDFF File Offset: 0x002BAFFF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.ChangeIndex(this.reverseList ? (-1) : 1);
	}

	// Token: 0x0600732F RID: 29487 RVA: 0x002BCE14 File Offset: 0x002BB014
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

	// Token: 0x06007330 RID: 29488 RVA: 0x002BCECD File Offset: 0x002BB0CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		if (!this.reverseList)
		{
			return this.isMaxIndex();
		}
		return this.isMinIndex();
	}

	// Token: 0x06007331 RID: 29489 RVA: 0x002BCEE4 File Offset: 0x002BB0E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		if (!this.reverseList)
		{
			return this.isMinIndex();
		}
		return this.isMaxIndex();
	}

	// Token: 0x06007332 RID: 29490 RVA: 0x002BCEFB File Offset: 0x002BB0FB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return this.Elements.Count == 0;
	}

	// Token: 0x06007333 RID: 29491 RVA: 0x002BCF0B File Offset: 0x002BB10B
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMaxIndex()
	{
		return this.SelectedIndex == this.MaxIndex || this.SelectedIndex == this.Elements.Count - 1;
	}

	// Token: 0x06007334 RID: 29492 RVA: 0x002BCF32 File Offset: 0x002BB132
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMinIndex()
	{
		return this.SelectedIndex == this.MinIndex || this.SelectedIndex == 0;
	}

	// Token: 0x06007335 RID: 29493 RVA: 0x002BC147 File Offset: 0x002BA347
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

	// Token: 0x06007336 RID: 29494 RVA: 0x002BCF50 File Offset: 0x002BB150
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

	// Token: 0x06007337 RID: 29495 RVA: 0x002BD0C4 File Offset: 0x002BB2C4
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

	// Token: 0x0400561F RID: 22047
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizationPrefix;

	// Token: 0x04005621 RID: 22049
	public readonly List<TElement> Elements = new List<TElement>();

	// Token: 0x04005622 RID: 22050
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex = int.MinValue;

	// Token: 0x04005623 RID: 22051
	public TElement CustomValue;

	// Token: 0x04005624 RID: 22052
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useCustomValue;

	// Token: 0x04005625 RID: 22053
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex = int.MinValue;

	// Token: 0x04005626 RID: 22054
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxIndex = int.MaxValue;

	// Token: 0x04005627 RID: 22055
	[PublicizedFrom(EAccessModifier.Private)]
	public bool reverseList;
}
