using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E6B RID: 3691
[Preserve]
public class XUiC_ComboBoxList<TElement> : XUiC_ComboBox<TElement>
{
	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x06007274 RID: 29300 RVA: 0x002B937A File Offset: 0x002B757A
	// (set) Token: 0x06007275 RID: 29301 RVA: 0x002B9382 File Offset: 0x002B7582
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

	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x06007276 RID: 29302 RVA: 0x002B9391 File Offset: 0x002B7591
	// (set) Token: 0x06007277 RID: 29303 RVA: 0x002B9399 File Offset: 0x002B7599
	[XuiXmlAttribute("localization_key_caseinsensitive", false)]
	public bool LocalizationKeyCaseInsensitive { get; set; }

	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x06007278 RID: 29304 RVA: 0x002B86B4 File Offset: 0x002B68B4
	// (set) Token: 0x06007279 RID: 29305 RVA: 0x002B93A4 File Offset: 0x002B75A4
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

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x0600727A RID: 29306 RVA: 0x002B93F8 File Offset: 0x002B75F8
	// (set) Token: 0x0600727B RID: 29307 RVA: 0x002B9448 File Offset: 0x002B7648
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

	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x0600727C RID: 29308 RVA: 0x002B94BC File Offset: 0x002B76BC
	// (set) Token: 0x0600727D RID: 29309 RVA: 0x002B94C5 File Offset: 0x002B76C5
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

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x0600727E RID: 29310 RVA: 0x002B94CF File Offset: 0x002B76CF
	// (set) Token: 0x0600727F RID: 29311 RVA: 0x002B94D8 File Offset: 0x002B76D8
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

	// Token: 0x17000D39 RID: 3385
	// (get) Token: 0x06007280 RID: 29312 RVA: 0x002B94E2 File Offset: 0x002B76E2
	// (set) Token: 0x06007281 RID: 29313 RVA: 0x002B94EB File Offset: 0x002B76EB
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

	// Token: 0x17000D3A RID: 3386
	// (get) Token: 0x06007282 RID: 29314 RVA: 0x002B94F5 File Offset: 0x002B76F5
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxIndex - this.MinIndex + 1;
		}
	}

	// Token: 0x17000D3B RID: 3387
	// (get) Token: 0x06007283 RID: 29315 RVA: 0x002B9506 File Offset: 0x002B7706
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

	// Token: 0x17000D3C RID: 3388
	// (get) Token: 0x06007284 RID: 29316 RVA: 0x002B952B File Offset: 0x002B772B
	// (set) Token: 0x06007285 RID: 29317 RVA: 0x002B9534 File Offset: 0x002B7734
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

	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x06007286 RID: 29318 RVA: 0x002B9647 File Offset: 0x002B7847
	// (set) Token: 0x06007287 RID: 29319 RVA: 0x002B965A File Offset: 0x002B785A
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

	// Token: 0x17000D3E RID: 3390
	// (get) Token: 0x06007288 RID: 29320 RVA: 0x002B9693 File Offset: 0x002B7893
	// (set) Token: 0x06007289 RID: 29321 RVA: 0x002B96C8 File Offset: 0x002B78C8
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

	// Token: 0x0600728A RID: 29322 RVA: 0x002B9718 File Offset: 0x002B7918
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

	// Token: 0x0600728B RID: 29323 RVA: 0x002B97F8 File Offset: 0x002B79F8
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

	// Token: 0x0600728C RID: 29324 RVA: 0x002B988B File Offset: 0x002B7A8B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TElement _oldVal, TElement _currentValue)
	{
		return (_oldVal != null || _currentValue != null) && (_oldVal == null || _currentValue == null || !_oldVal.Equals(_currentValue));
	}

	// Token: 0x0600728D RID: 29325 RVA: 0x002B98C7 File Offset: 0x002B7AC7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.ChangeIndex(this.reverseList ? 1 : (-1));
	}

	// Token: 0x0600728E RID: 29326 RVA: 0x002B98DB File Offset: 0x002B7ADB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.ChangeIndex(this.reverseList ? (-1) : 1);
	}

	// Token: 0x0600728F RID: 29327 RVA: 0x002B98F0 File Offset: 0x002B7AF0
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

	// Token: 0x06007290 RID: 29328 RVA: 0x002B99A9 File Offset: 0x002B7BA9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		if (!this.reverseList)
		{
			return this.isMaxIndex();
		}
		return this.isMinIndex();
	}

	// Token: 0x06007291 RID: 29329 RVA: 0x002B99C0 File Offset: 0x002B7BC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		if (!this.reverseList)
		{
			return this.isMinIndex();
		}
		return this.isMaxIndex();
	}

	// Token: 0x06007292 RID: 29330 RVA: 0x002B99D7 File Offset: 0x002B7BD7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return this.Elements.Count == 0;
	}

	// Token: 0x06007293 RID: 29331 RVA: 0x002B99E7 File Offset: 0x002B7BE7
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMaxIndex()
	{
		return this.SelectedIndex == this.MaxIndex || this.SelectedIndex == this.Elements.Count - 1;
	}

	// Token: 0x06007294 RID: 29332 RVA: 0x002B9A0E File Offset: 0x002B7C0E
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMinIndex()
	{
		return this.SelectedIndex == this.MinIndex || this.SelectedIndex == 0;
	}

	// Token: 0x06007295 RID: 29333 RVA: 0x002B8C23 File Offset: 0x002B6E23
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

	// Token: 0x06007296 RID: 29334 RVA: 0x002B9A2C File Offset: 0x002B7C2C
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

	// Token: 0x06007297 RID: 29335 RVA: 0x002B9BA0 File Offset: 0x002B7DA0
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

	// Token: 0x0400557B RID: 21883
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizationPrefix;

	// Token: 0x0400557D RID: 21885
	public readonly List<TElement> Elements = new List<TElement>();

	// Token: 0x0400557E RID: 21886
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentIndex = int.MinValue;

	// Token: 0x0400557F RID: 21887
	public TElement CustomValue;

	// Token: 0x04005580 RID: 21888
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useCustomValue;

	// Token: 0x04005581 RID: 21889
	[PublicizedFrom(EAccessModifier.Private)]
	public int minIndex = int.MinValue;

	// Token: 0x04005582 RID: 21890
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxIndex = int.MaxValue;

	// Token: 0x04005583 RID: 21891
	[PublicizedFrom(EAccessModifier.Private)]
	public bool reverseList;
}
