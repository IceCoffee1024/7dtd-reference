using System;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DF2 RID: 3570
[Preserve]
public class XUiC_ServerBrowserGamePrefSelectorCombo : XUiController, IServerBrowserFilterControl
{
	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x06006C7B RID: 27771 RVA: 0x002A855E File Offset: 0x002A675E
	// (set) Token: 0x06006C7C RID: 27772 RVA: 0x002A8566 File Offset: 0x002A6766
	[XuiXmlAttribute("allow_custom", false)]
	public bool AllowCustom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = true;

	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x06006C7D RID: 27773 RVA: 0x002A856F File Offset: 0x002A676F
	// (set) Token: 0x06006C7E RID: 27774 RVA: 0x002A8577 File Offset: 0x002A6777
	[XuiXmlAttribute("force_custom", false)]
	public bool ForceCustom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000C0E RID: 3086
	// (get) Token: 0x06006C7F RID: 27775 RVA: 0x002A8580 File Offset: 0x002A6780
	// (set) Token: 0x06006C80 RID: 27776 RVA: 0x002A8588 File Offset: 0x002A6788
	[XuiXmlAttribute("default", false)]
	public string DefaultValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C0F RID: 3087
	// (get) Token: 0x06006C81 RID: 27777 RVA: 0x002A8591 File Offset: 0x002A6791
	// (set) Token: 0x06006C82 RID: 27778 RVA: 0x002A8599 File Offset: 0x002A6799
	[XuiXmlAttribute("default_min", false)]
	public string DefaultMin
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C10 RID: 3088
	// (get) Token: 0x06006C83 RID: 27779 RVA: 0x002A85A2 File Offset: 0x002A67A2
	// (set) Token: 0x06006C84 RID: 27780 RVA: 0x002A85AA File Offset: 0x002A67AA
	[XuiXmlAttribute("default_max", false)]
	public string DefaultMax
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C11 RID: 3089
	// (get) Token: 0x06006C85 RID: 27781 RVA: 0x002A85B3 File Offset: 0x002A67B3
	// (set) Token: 0x06006C86 RID: 27782 RVA: 0x002A85BB File Offset: 0x002A67BB
	[XuiXmlAttribute("default_string", false)]
	public string DefaultString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x06006C87 RID: 27783 RVA: 0x002A85C4 File Offset: 0x002A67C4
	public bool IsCustomRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.controlCombo != null && this.controlCombo.Value.Type == XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Custom;
		}
	}

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x06006C88 RID: 27784 RVA: 0x002A85E3 File Offset: 0x002A67E3
	// (set) Token: 0x06006C89 RID: 27785 RVA: 0x002A85EB File Offset: 0x002A67EB
	public GameInfoBool GameInfoBool
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoBool)(-1);

	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x06006C8A RID: 27786 RVA: 0x002A85F4 File Offset: 0x002A67F4
	// (set) Token: 0x06006C8B RID: 27787 RVA: 0x002A85FC File Offset: 0x002A67FC
	public GameInfoInt GameInfoInt
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoInt)(-1);

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x06006C8C RID: 27788 RVA: 0x002A8605 File Offset: 0x002A6805
	// (set) Token: 0x06006C8D RID: 27789 RVA: 0x002A860D File Offset: 0x002A680D
	public GameInfoString GameInfoString
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoString)(-1);

	// Token: 0x17000C16 RID: 3094
	// (set) Token: 0x06006C8E RID: 27790 RVA: 0x002A8618 File Offset: 0x002A6818
	public int? ValueRangeMin
	{
		set
		{
			int? num = this.valueRangeMin;
			int? num2 = value;
			if ((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null)))
			{
				return;
			}
			this.valueRangeMin = value;
			this.setupOptions();
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (set) Token: 0x06006C8F RID: 27791 RVA: 0x002A8660 File Offset: 0x002A6860
	public int? ValueRangeMax
	{
		set
		{
			int? num = this.valueRangeMax;
			int? num2 = value;
			if ((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null)))
			{
				return;
			}
			this.valueRangeMax = value;
			this.setupOptions();
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (set) Token: 0x06006C90 RID: 27792 RVA: 0x002A86A7 File Offset: 0x002A68A7
	public Func<int, int> CustomValuePreFilterModifierFunc
	{
		set
		{
			this.customValuePreFilterModifierFunc = value;
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (set) Token: 0x06006C91 RID: 27793 RVA: 0x002A86B0 File Offset: 0x002A68B0
	public Func<int, int> ValuePreDisplayModifierFunc
	{
		set
		{
			this.valuePreDisplayModifierFunc = value;
			if (this.valuePreDisplayModifierFunc != null)
			{
				this.setupOptions();
			}
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x06006C92 RID: 27794 RVA: 0x002A86C7 File Offset: 0x002A68C7
	// (set) Token: 0x06006C93 RID: 27795 RVA: 0x002A86CF File Offset: 0x002A68CF
	public bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			if (this.enabled != value)
			{
				this.enabled = value;
				this.controlCombo.Enabled = value;
			}
		}
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06006C94 RID: 27796 RVA: 0x002A86ED File Offset: 0x002A68ED
	[XuiXmlBinding("iscustomrange")]
	public bool BindingIsCustomRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.valueType != GamePrefs.EnumType.String && (this.ForceCustom || this.IsCustomRange);
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06006C95 RID: 27797 RVA: 0x002A870A File Offset: 0x002A690A
	[XuiXmlBinding("iscustomstring")]
	public bool IsCustomString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.valueType == GamePrefs.EnumType.String && (this.ForceCustom || this.IsCustomRange);
		}
	}

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x06006C96 RID: 27798 RVA: 0x002A8727 File Offset: 0x002A6927
	[XuiXmlBinding("usecombo")]
	public bool UseCombo
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.ForceCustom;
		}
	}

	// Token: 0x06006C97 RID: 27799 RVA: 0x002A8734 File Offset: 0x002A6934
	public override void Init()
	{
		base.Init();
		GameInfoBool gameInfoBool;
		if (EnumUtils.TryParse<GameInfoBool>(this.viewComponent.ID, out gameInfoBool, false))
		{
			this.GameInfoBool = gameInfoBool;
			this.valueType = GamePrefs.EnumType.Bool;
		}
		GameInfoInt gameInfoInt;
		if (EnumUtils.TryParse<GameInfoInt>(this.viewComponent.ID, out gameInfoInt, false))
		{
			this.GameInfoInt = gameInfoInt;
			this.valueType = GamePrefs.EnumType.Int;
		}
		GameInfoString gameInfoString;
		if (EnumUtils.TryParse<GameInfoString>(this.viewComponent.ID, out gameInfoString, false))
		{
			this.GameInfoString = gameInfoString;
			this.valueType = GamePrefs.EnumType.String;
		}
		this.controlCombo = base.GetChildById("ControlCombo").GetChildByType<XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue>>();
		this.controlCombo.OnValueChanged += this.ControlCombo_OnValueChanged;
		this.txtValueMin = base.GetChildById("valuemin").GetChildByType<XUiC_TextInput>();
		this.txtValueMin.OnChangeHandler += this.ControlValue_OnChangeHandler;
		this.txtValueMin.UIInputController.OnScroll += this.controlCombo.ScrollEvent;
		this.txtValueMax = base.GetChildById("valuemax").GetChildByType<XUiC_TextInput>();
		this.txtValueMax.OnChangeHandler += this.ControlValue_OnChangeHandler;
		this.txtValueMax.UIInputController.OnScroll += this.controlCombo.ScrollEvent;
		this.txtValueString = base.GetChildById("valuestring").GetChildByType<XUiC_TextInput>();
		this.txtValueString.OnChangeHandler += this.ControlText_OnChangeHandler;
		this.txtValueString.UIInputController.OnScroll += this.controlCombo.ScrollEvent;
		this.txtValueMin.SelectOnTab = this.txtValueMax;
		this.txtValueMax.SelectOnTab = this.txtValueMin;
		this.setupOptions();
	}

	// Token: 0x06006C98 RID: 27800 RVA: 0x002A88EF File Offset: 0x002A6AEF
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.ForceCustom)
		{
			this.controlCombo.ShowButtons = false;
		}
		this.IsDirty = true;
	}

	// Token: 0x06006C99 RID: 27801 RVA: 0x002A8914 File Offset: 0x002A6B14
	[XuiXmlAttribute("values", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValues(string _value)
	{
		if (_value.Length <= 0)
		{
			return;
		}
		this.valuesFromXml = _value.Split(',', StringSplitOptions.None);
		for (int i = 0; i < this.valuesFromXml.Length; i++)
		{
			this.valuesFromXml[i] = this.valuesFromXml[i].Trim();
		}
	}

	// Token: 0x06006C9A RID: 27802 RVA: 0x002A8964 File Offset: 0x002A6B64
	[XuiXmlAttribute("display_names", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeDisplayNames(string _value)
	{
		if (_value.Length <= 0)
		{
			return;
		}
		this.namesFromXml = _value.Split(',', StringSplitOptions.None);
		for (int i = 0; i < this.namesFromXml.Length; i++)
		{
			this.namesFromXml[i] = this.namesFromXml[i].Trim();
		}
	}

	// Token: 0x06006C9B RID: 27803 RVA: 0x002A89B2 File Offset: 0x002A6BB2
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006C9C RID: 27804 RVA: 0x002A89CC File Offset: 0x002A6BCC
	[XuiXmlAttribute("allow_any", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void setAllowAny(bool _value)
	{
		if (_value == this.allowAny)
		{
			return;
		}
		this.allowAny = _value;
		if (this.controlCombo == null || this.controlCombo.Elements.Count == 0)
		{
			return;
		}
		if (_value)
		{
			if (!this.controlCombo.Elements.Contains(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Any, "")))
			{
				this.controlCombo.Elements.Insert(1, new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Any, Localization.Get("goAnyValue", false, null)));
				if (this.controlCombo.SelectedIndex > 0)
				{
					XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue> xuiC_ComboBoxList = this.controlCombo;
					int num = xuiC_ComboBoxList.SelectedIndex;
					xuiC_ComboBoxList.SelectedIndex = num + 1;
				}
			}
		}
		else if (this.controlCombo.Elements.Remove(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Any, "")) && this.controlCombo.SelectedIndex > 1)
		{
			XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue> xuiC_ComboBoxList2 = this.controlCombo;
			int num = xuiC_ComboBoxList2.SelectedIndex;
			xuiC_ComboBoxList2.SelectedIndex = num - 1;
		}
		this.controlCombo.SelectedIndex = this.controlCombo.SelectedIndex;
	}

	// Token: 0x06006C9D RID: 27805 RVA: 0x0029DA08 File Offset: 0x0029BC08
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006C9E RID: 27806 RVA: 0x002A8AC8 File Offset: 0x002A6CC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public string getGameInfoName()
	{
		switch (this.valueType)
		{
		case GamePrefs.EnumType.Int:
			return this.GameInfoInt.ToStringCached<GameInfoInt>();
		case GamePrefs.EnumType.String:
			return this.GameInfoString.ToStringCached<GameInfoString>();
		case GamePrefs.EnumType.Bool:
			return this.GameInfoBool.ToStringCached<GameInfoBool>();
		}
		return null;
	}

	// Token: 0x06006C9F RID: 27807 RVA: 0x002A8B24 File Offset: 0x002A6D24
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setupOptions()
	{
		this.controlCombo.Elements.Clear();
		this.controlCombo.Elements.Add(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Custom, ""));
		if (this.allowAny)
		{
			this.controlCombo.Elements.Add(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Any, Localization.Get("goAnyValue", false, null)));
		}
		if (!this.ForceCustom)
		{
			int[] array = null;
			string[] array2 = null;
			string[] array3;
			switch (this.valueType)
			{
			case GamePrefs.EnumType.Int:
			{
				if (!string.IsNullOrEmpty(this.DefaultValue))
				{
					this.defaultOptionValue = new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(StringParsers.ParseSInt32(this.DefaultValue, 0, -1, NumberStyles.Integer), "");
				}
				if (this.valuesFromXml != null)
				{
					array = new int[this.valuesFromXml.Length];
					for (int i = 0; i < this.valuesFromXml.Length; i++)
					{
						array[i] = StringParsers.ParseSInt32(this.valuesFromXml[i], 0, -1, NumberStyles.Integer);
					}
				}
				if (array == null)
				{
					if (this.namesFromXml == null)
					{
						throw new Exception("Illegal option setup for " + this.getGameInfoName() + " (no values and no names specified)");
					}
					array = new int[this.namesFromXml.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = j;
					}
				}
				array3 = new string[array.Length];
				if (this.namesFromXml == null || this.namesFromXml.Length != array.Length)
				{
					for (int k = 0; k < array.Length; k++)
					{
						if (this.namesFromXml != null && k < this.namesFromXml.Length)
						{
							array3[k] = Localization.Get(this.namesFromXml[k], false, null);
						}
						else
						{
							int num = array[k];
							if (this.valuePreDisplayModifierFunc != null)
							{
								num = this.valuePreDisplayModifierFunc(num);
							}
							if (string.IsNullOrEmpty(this.valueLocalizationPrefixFromXml))
							{
								array3[k] = num.ToString();
							}
							else
							{
								array3[k] = string.Format(Localization.Get(this.valueLocalizationPrefixFromXml + ((num == 1) ? "" : "s"), false, null), num);
							}
						}
					}
					goto IL_03AA;
				}
				for (int l = 0; l < this.namesFromXml.Length; l++)
				{
					array3[l] = Localization.Get(this.namesFromXml[l], false, null);
				}
				goto IL_03AA;
			}
			case GamePrefs.EnumType.String:
			{
				if (!string.IsNullOrEmpty(this.DefaultValue))
				{
					this.defaultOptionValue = new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(this.DefaultValue, "");
				}
				array2 = this.valuesFromXml;
				array3 = new string[array2.Length];
				if (this.namesFromXml == null || this.namesFromXml.Length != array2.Length)
				{
					for (int m = 0; m < array2.Length; m++)
					{
						if (this.namesFromXml != null && m < this.namesFromXml.Length)
						{
							array3[m] = Localization.Get(this.namesFromXml[m], false, null);
						}
						else
						{
							string text = array2[m];
							if (string.IsNullOrEmpty(this.valueLocalizationPrefixFromXml))
							{
								array3[m] = text;
							}
							else
							{
								array3[m] = Localization.Get(this.valueLocalizationPrefixFromXml + text, false, null);
							}
						}
					}
					goto IL_03AA;
				}
				for (int n = 0; n < this.namesFromXml.Length; n++)
				{
					array3[n] = Localization.Get(this.namesFromXml[n], false, null);
				}
				goto IL_03AA;
			}
			case GamePrefs.EnumType.Bool:
				if (!string.IsNullOrEmpty(this.DefaultValue))
				{
					this.defaultOptionValue = new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(StringParsers.ParseBool(this.DefaultValue, 0, -1, true) ? 1 : 0, "");
				}
				array = new int[] { 0, 1 };
				array3 = new string[]
				{
					Localization.Get("goOff", false, null),
					Localization.Get("goOn", false, null)
				};
				goto IL_03AA;
			}
			throw new NotSupportedException("Not a valid GameInfoX: " + this.viewComponent.ID);
			IL_03AA:
			if (array3 == null)
			{
				throw new Exception("Illegal option setup for " + this.getGameInfoName() + " (names null)");
			}
			if (this.valueType != GamePrefs.EnumType.String)
			{
				if (array == null)
				{
					throw new Exception("Illegal option setup for " + this.getGameInfoName() + " (values still null)");
				}
				for (int num2 = 0; num2 < array.Length; num2++)
				{
					if ((this.valueRangeMin == null || array[num2] >= this.valueRangeMin.Value) && (this.valueRangeMax == null || array[num2] <= this.valueRangeMax.Value))
					{
						this.controlCombo.Elements.Add(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(array[num2], array3[num2]));
					}
				}
			}
			if (this.valueType == GamePrefs.EnumType.String)
			{
				if (array2 == null)
				{
					throw new Exception("Illegal option setup for " + this.getGameInfoName() + " (values still null)");
				}
				for (int num3 = 0; num3 < array2.Length; num3++)
				{
					this.controlCombo.Elements.Add(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(array2[num3], array3[num3]));
				}
			}
		}
		this.controlCombo.MinIndex = (((this.valueType == GamePrefs.EnumType.Int || this.valueType == GamePrefs.EnumType.String) && (this.ForceCustom || this.AllowCustom)) ? 0 : 1);
		this.setDefaultValues();
	}

	// Token: 0x06006CA0 RID: 27808 RVA: 0x002A901A File Offset: 0x002A721A
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlCombo_OnValueChanged(XUiController _sender, XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue _oldValue, XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue _newValue)
	{
		this.IsDirty = true;
		Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006CA1 RID: 27809 RVA: 0x002A9034 File Offset: 0x002A7234
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void ControlValue_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		XUiC_TextInput xuiC_TextInput = _sender as XUiC_TextInput;
		if (xuiC_TextInput != null && !string.IsNullOrEmpty(_text))
		{
			ReadOnlySpan<char> readOnlySpan = _text.AsSpan().Trim();
			while (readOnlySpan.Length > 1 && *readOnlySpan[0] == 48)
			{
				ReadOnlySpan<char> readOnlySpan2 = readOnlySpan;
				readOnlySpan = readOnlySpan2.Slice(1, readOnlySpan2.Length - 1);
			}
			int num2;
			int? num = (StringParsers.TryParseSInt32(new string(readOnlySpan), out num2) ? new int?(num2) : null);
			if (num != null)
			{
				if (this.valueRangeMin != null && num.Value < this.valueRangeMin.Value)
				{
					num = this.valueRangeMin;
				}
				if (this.valueRangeMax != null && num.Value > this.valueRangeMax.Value)
				{
					num = this.valueRangeMax;
				}
			}
			string text = ((num != null) ? num.Value.ToString(CultureInfo.InvariantCulture) : "");
			if (text != _text)
			{
				xuiC_TextInput.Text = text;
			}
		}
		Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006CA2 RID: 27810 RVA: 0x002A9158 File Offset: 0x002A7358
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlText_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006CA3 RID: 27811 RVA: 0x002A916C File Offset: 0x002A736C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void setDefaultValues()
	{
		if (!this.ForceCustom && this.defaultOptionValue.Type != XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Null)
		{
			this.controlCombo.SelectedIndex = this.controlCombo.Elements.IndexOf(this.defaultOptionValue);
		}
		else
		{
			this.controlCombo.SelectedIndex = (this.ForceCustom ? 0 : 1);
		}
		this.txtValueMin.Text = this.DefaultMin ?? "";
		this.txtValueMax.Text = this.DefaultMax ?? "";
		this.txtValueString.Text = this.DefaultString ?? "";
	}

	// Token: 0x06006CA4 RID: 27812 RVA: 0x002A9216 File Offset: 0x002A7416
	public void Reset()
	{
		this.setDefaultValues();
		this.IsDirty = true;
		Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006CA5 RID: 27813 RVA: 0x002A9238 File Offset: 0x002A7438
	public void SelectEntry(XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue _value)
	{
		int num = this.controlCombo.Elements.IndexOf(_value);
		if (num >= 0)
		{
			this.controlCombo.SelectedIndex = num;
			Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged(this);
		}
	}

	// Token: 0x06006CA6 RID: 27814 RVA: 0x002A9278 File Offset: 0x002A7478
	public XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue GetSelection()
	{
		return this.controlCombo.Value;
	}

	// Token: 0x06006CA7 RID: 27815 RVA: 0x002A9288 File Offset: 0x002A7488
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.UiServerFilter getValueRangeFilter(string _filterName, int? _filterMin, int? _filterMax)
	{
		if (this.valueRangeMin != null)
		{
			if (_filterMin != null)
			{
				int? num = _filterMin;
				int num2 = this.valueRangeMin.Value;
				if (!((num.GetValueOrDefault() < num2) & (num != null)))
				{
					goto IL_0074;
				}
			}
			_filterMin = new int?(this.valueRangeMin.Value);
		}
		IL_0074:
		if (this.valueRangeMax != null)
		{
			if (_filterMax != null)
			{
				int? num = _filterMax;
				int num2 = this.valueRangeMax.Value;
				if (!((num.GetValueOrDefault() > num2) & (num != null)))
				{
					goto IL_00CD;
				}
			}
			_filterMax = new int?(this.valueRangeMax.Value);
		}
		IL_00CD:
		Func<XUiC_ServersList.ListEntry, bool> func;
		IServerListInterface.ServerFilter.EServerFilterType eserverFilterType;
		if (_filterMin != null)
		{
			if (_filterMax != null)
			{
				if (_filterMin.Value == _filterMax.Value)
				{
					func = delegate(XUiC_ServersList.ListEntry _entry)
					{
						int value = _entry.GameServerInfo.GetValue(this.GameInfoInt);
						int? filterMin = _filterMin;
						return (value == filterMin.GetValueOrDefault()) & (filterMin != null);
					};
					eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntValue;
				}
				else
				{
					func = delegate(XUiC_ServersList.ListEntry _entry)
					{
						int value2 = _entry.GameServerInfo.GetValue(this.GameInfoInt);
						int num3 = value2;
						int? num4 = _filterMin;
						if ((num3 >= num4.GetValueOrDefault()) & (num4 != null))
						{
							int num5 = value2;
							num4 = _filterMax;
							return (num5 <= num4.GetValueOrDefault()) & (num4 != null);
						}
						return false;
					};
					eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntRange;
				}
			}
			else
			{
				func = delegate(XUiC_ServersList.ListEntry _entry)
				{
					int value3 = _entry.GameServerInfo.GetValue(this.GameInfoInt);
					int? filterMin2 = _filterMin;
					return (value3 >= filterMin2.GetValueOrDefault()) & (filterMin2 != null);
				};
				eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMin;
			}
		}
		else if (_filterMax != null)
		{
			func = delegate(XUiC_ServersList.ListEntry _entry)
			{
				int value4 = _entry.GameServerInfo.GetValue(this.GameInfoInt);
				int? filterMax = _filterMax;
				return (value4 <= filterMax.GetValueOrDefault()) & (filterMax != null);
			};
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMax;
		}
		else
		{
			func = null;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.Any;
		}
		return new XUiC_ServersList.UiServerFilter(_filterName, XUiC_ServersList.EnumServerLists.Regular, func, eserverFilterType, _filterMin.GetValueOrDefault(), _filterMax.GetValueOrDefault(), false, null);
	}

	// Token: 0x06006CA8 RID: 27816 RVA: 0x002A940C File Offset: 0x002A760C
	public XUiC_ServersList.UiServerFilter GetFilter()
	{
		string gameInfoName = this.getGameInfoName();
		if (this.controlCombo.Value.Type == XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Any)
		{
			if (this.valueType != GamePrefs.EnumType.String)
			{
				return this.getValueRangeFilter(gameInfoName, null, null);
			}
			return new XUiC_ServersList.UiServerFilter(gameInfoName, XUiC_ServersList.EnumServerLists.Regular, null, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null);
		}
		else
		{
			if (!this.IsCustomRange)
			{
				switch (this.valueType)
				{
				case GamePrefs.EnumType.Int:
				{
					int intValue = this.controlCombo.Value.IntValue;
					return this.getValueRangeFilter(gameInfoName, new int?(intValue), new int?(intValue));
				}
				case GamePrefs.EnumType.String:
				{
					string sValue2 = this.controlCombo.Value.StringValue;
					Func<XUiC_ServersList.ListEntry, bool> func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.GameInfoString).EqualsCaseInsensitive(sValue2);
					IServerListInterface.ServerFilter.EServerFilterType eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.StringValue;
					return new XUiC_ServersList.UiServerFilter(gameInfoName, XUiC_ServersList.EnumServerLists.Regular, func, eserverFilterType, 0, 0, false, sValue2);
				}
				case GamePrefs.EnumType.Bool:
				{
					bool bValue = this.controlCombo.Value.IntValue == 1;
					Func<XUiC_ServersList.ListEntry, bool> func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.GameInfoBool) == bValue;
					IServerListInterface.ServerFilter.EServerFilterType eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.BoolValue;
					return new XUiC_ServersList.UiServerFilter(gameInfoName, XUiC_ServersList.EnumServerLists.Regular, func, eserverFilterType, 0, 0, bValue, null);
				}
				}
				throw new ArgumentOutOfRangeException();
			}
			if (this.valueType != GamePrefs.EnumType.String)
			{
				int num2;
				int? num = (StringParsers.TryParseSInt32(this.txtValueMin.Text, out num2) ? new int?(num2) : null);
				int num4;
				int? num3 = (StringParsers.TryParseSInt32(this.txtValueMax.Text, out num4) ? new int?(num4) : null);
				if (num != null && this.customValuePreFilterModifierFunc != null)
				{
					num = new int?(this.customValuePreFilterModifierFunc(num.Value));
				}
				if (num3 != null && this.customValuePreFilterModifierFunc != null)
				{
					num3 = new int?(this.customValuePreFilterModifierFunc(num3.Value));
				}
				return this.getValueRangeFilter(gameInfoName, num, num3);
			}
			string sValue = this.txtValueString.Text.Trim();
			if (sValue.Length == 0)
			{
				return new XUiC_ServersList.UiServerFilter(gameInfoName, XUiC_ServersList.EnumServerLists.Regular, null, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null);
			}
			Func<XUiC_ServersList.ListEntry, bool> func2 = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.GameInfoString).ContainsCaseInsensitive(sValue);
			IServerListInterface.ServerFilter.EServerFilterType eserverFilterType2 = IServerListInterface.ServerFilter.EServerFilterType.StringContains;
			return new XUiC_ServersList.UiServerFilter(gameInfoName, XUiC_ServersList.EnumServerLists.Regular, func2, eserverFilterType2, 0, 0, false, this.txtValueString.Text);
		}
	}

	// Token: 0x0400530F RID: 21263
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue> controlCombo;

	// Token: 0x04005310 RID: 21264
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueMin;

	// Token: 0x04005311 RID: 21265
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueMax;

	// Token: 0x04005312 RID: 21266
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueString;

	// Token: 0x04005313 RID: 21267
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowAny = true;

	// Token: 0x04005317 RID: 21271
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue defaultOptionValue;

	// Token: 0x0400531B RID: 21275
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] valuesFromXml;

	// Token: 0x0400531C RID: 21276
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x0400531D RID: 21277
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x0400531E RID: 21278
	[PublicizedFrom(EAccessModifier.Private)]
	public int? valueRangeMin;

	// Token: 0x0400531F RID: 21279
	[PublicizedFrom(EAccessModifier.Private)]
	public int? valueRangeMax;

	// Token: 0x04005320 RID: 21280
	[PublicizedFrom(EAccessModifier.Protected)]
	public GamePrefs.EnumType valueType;

	// Token: 0x04005321 RID: 21281
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> customValuePreFilterModifierFunc;

	// Token: 0x04005322 RID: 21282
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x04005323 RID: 21283
	public Action<IServerBrowserFilterControl> OnValueChanged;

	// Token: 0x04005327 RID: 21287
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x02000DF3 RID: 3571
	public enum EOptionValueType
	{
		// Token: 0x04005329 RID: 21289
		Null,
		// Token: 0x0400532A RID: 21290
		Custom,
		// Token: 0x0400532B RID: 21291
		Any,
		// Token: 0x0400532C RID: 21292
		Int,
		// Token: 0x0400532D RID: 21293
		String
	}

	// Token: 0x02000DF4 RID: 3572
	public readonly struct GameOptionValue : IEquatable<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue>
	{
		// Token: 0x06006CAA RID: 27818 RVA: 0x002A96A4 File Offset: 0x002A78A4
		public GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.StringValue = null;
			this.displayName = _displayName;
		}

		// Token: 0x06006CAB RID: 27819 RVA: 0x002A96C2 File Offset: 0x002A78C2
		public GameOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.StringValue = null;
			this.displayName = _displayName;
		}

		// Token: 0x06006CAC RID: 27820 RVA: 0x002A96E0 File Offset: 0x002A78E0
		public GameOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.String;
			this.IntValue = -1;
			this.StringValue = _stringValue;
			this.displayName = _displayName;
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x002A96FE File Offset: 0x002A78FE
		public override string ToString()
		{
			return this.displayName;
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x002A9708 File Offset: 0x002A7908
		public bool Equals(XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue _other)
		{
			if (this.Type != _other.Type)
			{
				return false;
			}
			switch (this.Type)
			{
			case XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Custom:
			case XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Any:
				return true;
			case XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Int:
				return this.IntValue == _other.IntValue;
			case XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.String:
				return this.StringValue == _other.StringValue;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x002A9770 File Offset: 0x002A7970
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue)
			{
				XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue gameOptionValue = (XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue)_obj;
				return this.Equals(gameOptionValue);
			}
			return false;
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x002A9798 File Offset: 0x002A7998
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)this.IntValue) * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)((this.displayName != null) ? this.displayName.GetHashCode() : 0));
		}

		// Token: 0x0400532E RID: 21294
		public readonly XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType Type;

		// Token: 0x0400532F RID: 21295
		public readonly int IntValue;

		// Token: 0x04005330 RID: 21296
		public readonly string StringValue;

		// Token: 0x04005331 RID: 21297
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayName;
	}
}
