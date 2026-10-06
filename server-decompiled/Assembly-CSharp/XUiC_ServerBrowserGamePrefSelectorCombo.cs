using System;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DE7 RID: 3559
[Preserve]
public class XUiC_ServerBrowserGamePrefSelectorCombo : XUiController, IServerBrowserFilterControl
{
	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x06006C56 RID: 27734 RVA: 0x002A7036 File Offset: 0x002A5236
	// (set) Token: 0x06006C57 RID: 27735 RVA: 0x002A703E File Offset: 0x002A523E
	[XuiXmlAttribute("allow_custom", false)]
	public bool AllowCustom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = true;

	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x06006C58 RID: 27736 RVA: 0x002A7047 File Offset: 0x002A5247
	// (set) Token: 0x06006C59 RID: 27737 RVA: 0x002A704F File Offset: 0x002A524F
	[XuiXmlAttribute("force_custom", false)]
	public bool ForceCustom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000C08 RID: 3080
	// (get) Token: 0x06006C5A RID: 27738 RVA: 0x002A7058 File Offset: 0x002A5258
	// (set) Token: 0x06006C5B RID: 27739 RVA: 0x002A7060 File Offset: 0x002A5260
	[XuiXmlAttribute("default", false)]
	public string DefaultValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x06006C5C RID: 27740 RVA: 0x002A7069 File Offset: 0x002A5269
	// (set) Token: 0x06006C5D RID: 27741 RVA: 0x002A7071 File Offset: 0x002A5271
	[XuiXmlAttribute("default_min", false)]
	public string DefaultMin
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C0A RID: 3082
	// (get) Token: 0x06006C5E RID: 27742 RVA: 0x002A707A File Offset: 0x002A527A
	// (set) Token: 0x06006C5F RID: 27743 RVA: 0x002A7082 File Offset: 0x002A5282
	[XuiXmlAttribute("default_max", false)]
	public string DefaultMax
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C0B RID: 3083
	// (get) Token: 0x06006C60 RID: 27744 RVA: 0x002A708B File Offset: 0x002A528B
	// (set) Token: 0x06006C61 RID: 27745 RVA: 0x002A7093 File Offset: 0x002A5293
	[XuiXmlAttribute("default_string", false)]
	public string DefaultString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x06006C62 RID: 27746 RVA: 0x002A709C File Offset: 0x002A529C
	public bool IsCustomRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.controlCombo != null && this.controlCombo.Value.Type == XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Custom;
		}
	}

	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x06006C63 RID: 27747 RVA: 0x002A70BB File Offset: 0x002A52BB
	// (set) Token: 0x06006C64 RID: 27748 RVA: 0x002A70C3 File Offset: 0x002A52C3
	public GameInfoBool GameInfoBool
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoBool)(-1);

	// Token: 0x17000C0E RID: 3086
	// (get) Token: 0x06006C65 RID: 27749 RVA: 0x002A70CC File Offset: 0x002A52CC
	// (set) Token: 0x06006C66 RID: 27750 RVA: 0x002A70D4 File Offset: 0x002A52D4
	public GameInfoInt GameInfoInt
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoInt)(-1);

	// Token: 0x17000C0F RID: 3087
	// (get) Token: 0x06006C67 RID: 27751 RVA: 0x002A70DD File Offset: 0x002A52DD
	// (set) Token: 0x06006C68 RID: 27752 RVA: 0x002A70E5 File Offset: 0x002A52E5
	public GameInfoString GameInfoString
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoString)(-1);

	// Token: 0x17000C10 RID: 3088
	// (set) Token: 0x06006C69 RID: 27753 RVA: 0x002A70F0 File Offset: 0x002A52F0
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

	// Token: 0x17000C11 RID: 3089
	// (set) Token: 0x06006C6A RID: 27754 RVA: 0x002A7138 File Offset: 0x002A5338
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

	// Token: 0x17000C12 RID: 3090
	// (set) Token: 0x06006C6B RID: 27755 RVA: 0x002A717F File Offset: 0x002A537F
	public Func<int, int> CustomValuePreFilterModifierFunc
	{
		set
		{
			this.customValuePreFilterModifierFunc = value;
		}
	}

	// Token: 0x17000C13 RID: 3091
	// (set) Token: 0x06006C6C RID: 27756 RVA: 0x002A7188 File Offset: 0x002A5388
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

	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x06006C6D RID: 27757 RVA: 0x002A719F File Offset: 0x002A539F
	// (set) Token: 0x06006C6E RID: 27758 RVA: 0x002A71A7 File Offset: 0x002A53A7
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

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x06006C6F RID: 27759 RVA: 0x002A71C5 File Offset: 0x002A53C5
	[XuiXmlBinding("iscustomrange")]
	public bool BindingIsCustomRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.valueType != GamePrefs.EnumType.String && (this.ForceCustom || this.IsCustomRange);
		}
	}

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x06006C70 RID: 27760 RVA: 0x002A71E2 File Offset: 0x002A53E2
	[XuiXmlBinding("iscustomstring")]
	public bool IsCustomString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.valueType == GamePrefs.EnumType.String && (this.ForceCustom || this.IsCustomRange);
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x06006C71 RID: 27761 RVA: 0x002A71FF File Offset: 0x002A53FF
	[XuiXmlBinding("usecombo")]
	public bool UseCombo
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.ForceCustom;
		}
	}

	// Token: 0x06006C72 RID: 27762 RVA: 0x002A720C File Offset: 0x002A540C
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

	// Token: 0x06006C73 RID: 27763 RVA: 0x002A73C7 File Offset: 0x002A55C7
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.ForceCustom)
		{
			this.controlCombo.ShowButtons = false;
		}
		this.IsDirty = true;
	}

	// Token: 0x06006C74 RID: 27764 RVA: 0x002A73EC File Offset: 0x002A55EC
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

	// Token: 0x06006C75 RID: 27765 RVA: 0x002A743C File Offset: 0x002A563C
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

	// Token: 0x06006C76 RID: 27766 RVA: 0x002A748A File Offset: 0x002A568A
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006C77 RID: 27767 RVA: 0x002A74A4 File Offset: 0x002A56A4
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

	// Token: 0x06006C78 RID: 27768 RVA: 0x0029C4E0 File Offset: 0x0029A6E0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006C79 RID: 27769 RVA: 0x002A75A0 File Offset: 0x002A57A0
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

	// Token: 0x06006C7A RID: 27770 RVA: 0x002A75FC File Offset: 0x002A57FC
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

	// Token: 0x06006C7B RID: 27771 RVA: 0x002A7AF2 File Offset: 0x002A5CF2
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

	// Token: 0x06006C7C RID: 27772 RVA: 0x002A7B0C File Offset: 0x002A5D0C
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

	// Token: 0x06006C7D RID: 27773 RVA: 0x002A7C30 File Offset: 0x002A5E30
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

	// Token: 0x06006C7E RID: 27774 RVA: 0x002A7C44 File Offset: 0x002A5E44
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

	// Token: 0x06006C7F RID: 27775 RVA: 0x002A7CEE File Offset: 0x002A5EEE
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

	// Token: 0x06006C80 RID: 27776 RVA: 0x002A7D10 File Offset: 0x002A5F10
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

	// Token: 0x06006C81 RID: 27777 RVA: 0x002A7D50 File Offset: 0x002A5F50
	public XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue GetSelection()
	{
		return this.controlCombo.Value;
	}

	// Token: 0x06006C82 RID: 27778 RVA: 0x002A7D60 File Offset: 0x002A5F60
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

	// Token: 0x06006C83 RID: 27779 RVA: 0x002A7EE4 File Offset: 0x002A60E4
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

	// Token: 0x040052EA RID: 21226
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue> controlCombo;

	// Token: 0x040052EB RID: 21227
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueMin;

	// Token: 0x040052EC RID: 21228
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueMax;

	// Token: 0x040052ED RID: 21229
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueString;

	// Token: 0x040052EE RID: 21230
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowAny = true;

	// Token: 0x040052F2 RID: 21234
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue defaultOptionValue;

	// Token: 0x040052F6 RID: 21238
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] valuesFromXml;

	// Token: 0x040052F7 RID: 21239
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x040052F8 RID: 21240
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x040052F9 RID: 21241
	[PublicizedFrom(EAccessModifier.Private)]
	public int? valueRangeMin;

	// Token: 0x040052FA RID: 21242
	[PublicizedFrom(EAccessModifier.Private)]
	public int? valueRangeMax;

	// Token: 0x040052FB RID: 21243
	[PublicizedFrom(EAccessModifier.Protected)]
	public GamePrefs.EnumType valueType;

	// Token: 0x040052FC RID: 21244
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> customValuePreFilterModifierFunc;

	// Token: 0x040052FD RID: 21245
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x040052FE RID: 21246
	public Action<IServerBrowserFilterControl> OnValueChanged;

	// Token: 0x04005302 RID: 21250
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x02000DE8 RID: 3560
	public enum EOptionValueType
	{
		// Token: 0x04005304 RID: 21252
		Null,
		// Token: 0x04005305 RID: 21253
		Custom,
		// Token: 0x04005306 RID: 21254
		Any,
		// Token: 0x04005307 RID: 21255
		Int,
		// Token: 0x04005308 RID: 21256
		String
	}

	// Token: 0x02000DE9 RID: 3561
	public readonly struct GameOptionValue : IEquatable<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue>
	{
		// Token: 0x06006C85 RID: 27781 RVA: 0x002A817C File Offset: 0x002A637C
		public GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.StringValue = null;
			this.displayName = _displayName;
		}

		// Token: 0x06006C86 RID: 27782 RVA: 0x002A819A File Offset: 0x002A639A
		public GameOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.StringValue = null;
			this.displayName = _displayName;
		}

		// Token: 0x06006C87 RID: 27783 RVA: 0x002A81B8 File Offset: 0x002A63B8
		public GameOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.String;
			this.IntValue = -1;
			this.StringValue = _stringValue;
			this.displayName = _displayName;
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x002A81D6 File Offset: 0x002A63D6
		public override string ToString()
		{
			return this.displayName;
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x002A81E0 File Offset: 0x002A63E0
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

		// Token: 0x06006C8A RID: 27786 RVA: 0x002A8248 File Offset: 0x002A6448
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue)
			{
				XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue gameOptionValue = (XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue)_obj;
				return this.Equals(gameOptionValue);
			}
			return false;
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x002A8270 File Offset: 0x002A6470
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)this.IntValue) * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)((this.displayName != null) ? this.displayName.GetHashCode() : 0));
		}

		// Token: 0x04005309 RID: 21257
		public readonly XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType Type;

		// Token: 0x0400530A RID: 21258
		public readonly int IntValue;

		// Token: 0x0400530B RID: 21259
		public readonly string StringValue;

		// Token: 0x0400530C RID: 21260
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayName;
	}
}
