using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E9A RID: 3738
[Preserve]
public class XUiC_ComboBoxList<TElement> : XUiC_ComboBox<TElement>
{
	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x06007331 RID: 29489 RVA: 0x002C11BA File Offset: 0x002BF3BA
	// (set) Token: 0x06007332 RID: 29490 RVA: 0x002C11C2 File Offset: 0x002BF3C2
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

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x06007333 RID: 29491 RVA: 0x002C11D1 File Offset: 0x002BF3D1
	// (set) Token: 0x06007334 RID: 29492 RVA: 0x002C11D9 File Offset: 0x002BF3D9
	[XuiXmlAttribute("localization_key_caseinsensitive", false)]
	public bool LocalizationKeyCaseInsensitive { get; set; }

	// Token: 0x17000D6F RID: 3439
	// (get) Token: 0x06007335 RID: 29493 RVA: 0x002C04F4 File Offset: 0x002BE6F4
	// (set) Token: 0x06007336 RID: 29494 RVA: 0x002C11E4 File Offset: 0x002BF3E4
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

	// Token: 0x17000D70 RID: 3440
	// (get) Token: 0x06007337 RID: 29495 RVA: 0x002C1238 File Offset: 0x002BF438
	// (set) Token: 0x06007338 RID: 29496 RVA: 0x002C1288 File Offset: 0x002BF488
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

	// Token: 0x17000D71 RID: 3441
	// (get) Token: 0x06007339 RID: 29497 RVA: 0x002C12FC File Offset: 0x002BF4FC
	// (set) Token: 0x0600733A RID: 29498 RVA: 0x002C1305 File Offset: 0x002BF505
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

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x0600733B RID: 29499 RVA: 0x002C130F File Offset: 0x002BF50F
	// (set) Token: 0x0600733C RID: 29500 RVA: 0x002C1318 File Offset: 0x002BF518
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

	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x0600733D RID: 29501 RVA: 0x002C1322 File Offset: 0x002BF522
	// (set) Token: 0x0600733E RID: 29502 RVA: 0x002C132B File Offset: 0x002BF52B
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

	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x0600733F RID: 29503 RVA: 0x002C1335 File Offset: 0x002BF535
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxIndex - this.MinIndex + 1;
		}
	}

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06007340 RID: 29504 RVA: 0x002C1346 File Offset: 0x002BF546
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

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x06007341 RID: 29505 RVA: 0x002C136B File Offset: 0x002BF56B
	// (set) Token: 0x06007342 RID: 29506 RVA: 0x002C1374 File Offset: 0x002BF574
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

	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x06007343 RID: 29507 RVA: 0x002C1487 File Offset: 0x002BF687
	// (set) Token: 0x06007344 RID: 29508 RVA: 0x002C149A File Offset: 0x002BF69A
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

	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x06007345 RID: 29509 RVA: 0x002C14D3 File Offset: 0x002BF6D3
	// (set) Token: 0x06007346 RID: 29510 RVA: 0x002C1508 File Offset: 0x002BF708
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

	// Token: 0x06007347 RID: 29511 RVA: 0x002C1558 File Offset: 0x002BF758
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

	// Token: 0x06007348 RID: 29512 RVA: 0x002C1638 File Offset: 0x002BF838
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

	// Token: 0x06007349 RID: 29513 RVA: 0x002C16CB File Offset: 0x002BF8CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TElement _oldVal, TElement _currentValue)
	{
		return (_oldVal != null || _currentValue != null) && (_oldVal == null || _currentValue == null || !_oldVal.Equals(_currentValue));
	}

	// Token: 0x0600734A RID: 29514 RVA: 0x002C1707 File Offset: 0x002BF907
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.ChangeIndex(this.reverseList ? 1 : (-1));
	}

	// Token: 0x0600734B RID: 29515 RVA: 0x002C171B File Offset: 0x002BF91B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.ChangeIndex(this.reverseList ? (-1) : 1);
	}

	// Token: 0x0600734C RID: 29516 RVA: 0x002C1730 File Offset: 0x002BF930
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

	// Token: 0x0600734D RID: 29517 RVA: 0x002C17E9 File Offset: 0x002BF9E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		if (!this.reverseList)
		{
			return this.isMaxIndex();
		}
		return this.isMinIndex();
	}

	// Token: 0x0600734E RID: 29518 RVA: 0x002C1800 File Offset: 0x002BFA00
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		if (!this.reverseList)
		{
			return this.isMinIndex();
		}
		return this.isMaxIndex();
	}

	// Token: 0x0600734F RID: 29519 RVA: 0x002C1817 File Offset: 0x002BFA17
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return this.Elements.Count == 0;
	}

	// Token: 0x06007350 RID: 29520 RVA: 0x002C1827 File Offset: 0x002BFA27
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMaxIndex()
	{
		return this.SelectedIndex == this.MaxIndex || this.SelectedIndex == this.Elements.Count - 1;
	}

	// Token: 0x06007351 RID: 29521 RVA: 0x002C184E File Offset: 0x002BFA4E
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMinIndex()
	{
		return this.SelectedIndex == this.MinIndex || this.SelectedIndex == 0;
	}

	// Token: 0x06007352 RID: 29522 RVA: 0x002C0A63 File Offset: 0x002BEC63
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

	// Token: 0x06007353 RID: 29523 RVA: 0x002C186C File Offset: 0x002BFA6C
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

	// Token: 0x06007354 RID: 29524 RVA: 0x002C19E0 File Offset: 0x002BFBE0
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

	// Token: 0x0400567B RID: 22139
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizationPrefix;

	// Token: 0x0400567D RID: 22141
	public readonly List<TElement> Elements = new List<TElement>();

	// Token: 0x0400567E RID: 22142
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex = int.MinValue;

	// Token: 0x0400567F RID: 22143
	public TElement CustomValue;

	// Token: 0x04005680 RID: 22144
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useCustomValue;

	// Token: 0x04005681 RID: 22145
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex = int.MinValue;

	// Token: 0x04005682 RID: 22146
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxIndex = int.MaxValue;

	// Token: 0x04005683 RID: 22147
	[PublicizedFrom(EAccessModifier.Private)]
	public bool reverseList;
}
