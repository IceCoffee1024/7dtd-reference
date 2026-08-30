using System;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DC4 RID: 3524
[Preserve]
public class XUiC_ServerBrowserGamePrefSelectorCombo : XUiController, IServerBrowserFilterControl
{
	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x06006BC3 RID: 27587 RVA: 0x002A0B26 File Offset: 0x0029ED26
	// (set) Token: 0x06006BC4 RID: 27588 RVA: 0x002A0B2E File Offset: 0x0029ED2E
	[XuiXmlAttribute("allow_custom", false)]
	public bool AllowCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x06006BC5 RID: 27589 RVA: 0x002A0B37 File Offset: 0x0029ED37
	// (set) Token: 0x06006BC6 RID: 27590 RVA: 0x002A0B3F File Offset: 0x0029ED3F
	[XuiXmlAttribute("force_custom", false)]
	public bool ForceCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x06006BC7 RID: 27591 RVA: 0x002A0B48 File Offset: 0x0029ED48
	// (set) Token: 0x06006BC8 RID: 27592 RVA: 0x002A0B50 File Offset: 0x0029ED50
	[XuiXmlAttribute("default", false)]
	public string DefaultValue
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x06006BC9 RID: 27593 RVA: 0x002A0B59 File Offset: 0x0029ED59
	// (set) Token: 0x06006BCA RID: 27594 RVA: 0x002A0B61 File Offset: 0x0029ED61
	[XuiXmlAttribute("default_min", false)]
	public string DefaultMin
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x06006BCB RID: 27595 RVA: 0x002A0B6A File Offset: 0x0029ED6A
	// (set) Token: 0x06006BCC RID: 27596 RVA: 0x002A0B72 File Offset: 0x0029ED72
	[XuiXmlAttribute("default_max", false)]
	public string DefaultMax
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x06006BCD RID: 27597 RVA: 0x002A0B7B File Offset: 0x0029ED7B
	// (set) Token: 0x06006BCE RID: 27598 RVA: 0x002A0B83 File Offset: 0x0029ED83
	[XuiXmlAttribute("default_string", false)]
	public string DefaultString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06006BCF RID: 27599 RVA: 0x002A0B8C File Offset: 0x0029ED8C
	public bool IsCustomRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.controlCombo != null && this.controlCombo.Value.Type == XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Custom;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x002A0BAB File Offset: 0x0029EDAB
	// (set) Token: 0x06006BD1 RID: 27601 RVA: 0x002A0BB3 File Offset: 0x0029EDB3
	public GameInfoBool GameInfoBool
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoBool)(-1);

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x06006BD2 RID: 27602 RVA: 0x002A0BBC File Offset: 0x0029EDBC
	// (set) Token: 0x06006BD3 RID: 27603 RVA: 0x002A0BC4 File Offset: 0x0029EDC4
	public GameInfoInt GameInfoInt
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoInt)(-1);

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x06006BD4 RID: 27604 RVA: 0x002A0BCD File Offset: 0x0029EDCD
	// (set) Token: 0x06006BD5 RID: 27605 RVA: 0x002A0BD5 File Offset: 0x0029EDD5
	public GameInfoString GameInfoString
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = (GameInfoString)(-1);

	// Token: 0x17000BDD RID: 3037
	// (set) Token: 0x06006BD6 RID: 27606 RVA: 0x002A0BE0 File Offset: 0x0029EDE0
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

	// Token: 0x17000BDE RID: 3038
	// (set) Token: 0x06006BD7 RID: 27607 RVA: 0x002A0C28 File Offset: 0x0029EE28
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

	// Token: 0x17000BDF RID: 3039
	// (set) Token: 0x06006BD8 RID: 27608 RVA: 0x002A0C6F File Offset: 0x0029EE6F
	public Func<int, int> CustomValuePreFilterModifierFunc
	{
		set
		{
			this.customValuePreFilterModifierFunc = value;
		}
	}

	// Token: 0x17000BE0 RID: 3040
	// (set) Token: 0x06006BD9 RID: 27609 RVA: 0x002A0C78 File Offset: 0x0029EE78
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

	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x06006BDA RID: 27610 RVA: 0x002A0C8F File Offset: 0x0029EE8F
	// (set) Token: 0x06006BDB RID: 27611 RVA: 0x002A0C97 File Offset: 0x0029EE97
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

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x06006BDC RID: 27612 RVA: 0x002A0CB5 File Offset: 0x0029EEB5
	[XuiXmlBinding("iscustomrange")]
	public bool BindingIsCustomRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.valueType != GamePrefs.EnumType.String && (this.ForceCustom || this.IsCustomRange);
		}
	}

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x06006BDD RID: 27613 RVA: 0x002A0CD2 File Offset: 0x0029EED2
	[XuiXmlBinding("iscustomstring")]
	public bool IsCustomString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.valueType == GamePrefs.EnumType.String && (this.ForceCustom || this.IsCustomRange);
		}
	}

	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x06006BDE RID: 27614 RVA: 0x002A0CEF File Offset: 0x0029EEEF
	[XuiXmlBinding("usecombo")]
	public bool UseCombo
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.ForceCustom;
		}
	}

	// Token: 0x06006BDF RID: 27615 RVA: 0x002A0CFC File Offset: 0x0029EEFC
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

	// Token: 0x06006BE0 RID: 27616 RVA: 0x002A0EB7 File Offset: 0x0029F0B7
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.ForceCustom)
		{
			this.controlCombo.ShowButtons = false;
		}
		this.IsDirty = true;
	}

	// Token: 0x06006BE1 RID: 27617 RVA: 0x002A0EDC File Offset: 0x0029F0DC
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

	// Token: 0x06006BE2 RID: 27618 RVA: 0x002A0F2C File Offset: 0x0029F12C
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

	// Token: 0x06006BE3 RID: 27619 RVA: 0x002A0F7A File Offset: 0x0029F17A
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x002A0F94 File Offset: 0x0029F194
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

	// Token: 0x06006BE5 RID: 27621 RVA: 0x00296000 File Offset: 0x00294200
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006BE6 RID: 27622 RVA: 0x002A1090 File Offset: 0x0029F290
	[PublicizedFrom(EAccessModifier.Private)]
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

	// Token: 0x06006BE7 RID: 27623 RVA: 0x002A10EC File Offset: 0x0029F2EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupOptions()
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

	// Token: 0x06006BE8 RID: 27624 RVA: 0x002A15E2 File Offset: 0x0029F7E2
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

	// Token: 0x06006BE9 RID: 27625 RVA: 0x002A15FC File Offset: 0x0029F7FC
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

	// Token: 0x06006BEA RID: 27626 RVA: 0x002A1720 File Offset: 0x0029F920
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

	// Token: 0x06006BEB RID: 27627 RVA: 0x002A1734 File Offset: 0x0029F934
	[PublicizedFrom(EAccessModifier.Private)]
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

	// Token: 0x06006BEC RID: 27628 RVA: 0x002A17DE File Offset: 0x0029F9DE
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

	// Token: 0x06006BED RID: 27629 RVA: 0x002A1800 File Offset: 0x0029FA00
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

	// Token: 0x06006BEE RID: 27630 RVA: 0x002A1840 File Offset: 0x0029FA40
	public XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue GetSelection()
	{
		return this.controlCombo.Value;
	}

	// Token: 0x06006BEF RID: 27631 RVA: 0x002A1850 File Offset: 0x0029FA50
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

	// Token: 0x06006BF0 RID: 27632 RVA: 0x002A19D4 File Offset: 0x0029FBD4
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

	// Token: 0x04005211 RID: 21009
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue> controlCombo;

	// Token: 0x04005212 RID: 21010
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueMin;

	// Token: 0x04005213 RID: 21011
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueMax;

	// Token: 0x04005214 RID: 21012
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtValueString;

	// Token: 0x04005215 RID: 21013
	[PublicizedFrom(EAccessModifier.Private)]
	public bool allowAny = true;

	// Token: 0x04005219 RID: 21017
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue defaultOptionValue;

	// Token: 0x0400521D RID: 21021
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] valuesFromXml;

	// Token: 0x0400521E RID: 21022
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x0400521F RID: 21023
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x04005220 RID: 21024
	[PublicizedFrom(EAccessModifier.Private)]
	public int? valueRangeMin;

	// Token: 0x04005221 RID: 21025
	[PublicizedFrom(EAccessModifier.Private)]
	public int? valueRangeMax;

	// Token: 0x04005222 RID: 21026
	[PublicizedFrom(EAccessModifier.Private)]
	public GamePrefs.EnumType valueType;

	// Token: 0x04005223 RID: 21027
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> customValuePreFilterModifierFunc;

	// Token: 0x04005224 RID: 21028
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x04005225 RID: 21029
	public Action<IServerBrowserFilterControl> OnValueChanged;

	// Token: 0x04005229 RID: 21033
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x02000DC5 RID: 3525
	public enum EOptionValueType
	{
		// Token: 0x0400522B RID: 21035
		Null,
		// Token: 0x0400522C RID: 21036
		Custom,
		// Token: 0x0400522D RID: 21037
		Any,
		// Token: 0x0400522E RID: 21038
		Int,
		// Token: 0x0400522F RID: 21039
		String
	}

	// Token: 0x02000DC6 RID: 3526
	public readonly struct GameOptionValue : IEquatable<XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue>
	{
		// Token: 0x06006BF2 RID: 27634 RVA: 0x002A1C6C File Offset: 0x0029FE6C
		public GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.StringValue = null;
			this.displayName = _displayName;
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x002A1C8A File Offset: 0x0029FE8A
		public GameOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.StringValue = null;
			this.displayName = _displayName;
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x002A1CA8 File Offset: 0x0029FEA8
		public GameOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.String;
			this.IntValue = -1;
			this.StringValue = _stringValue;
			this.displayName = _displayName;
		}

		// Token: 0x06006BF5 RID: 27637 RVA: 0x002A1CC6 File Offset: 0x0029FEC6
		public override string ToString()
		{
			return this.displayName;
		}

		// Token: 0x06006BF6 RID: 27638 RVA: 0x002A1CD0 File Offset: 0x0029FED0
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

		// Token: 0x06006BF7 RID: 27639 RVA: 0x002A1D38 File Offset: 0x0029FF38
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue)
			{
				XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue gameOptionValue = (XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue)_obj;
				return this.Equals(gameOptionValue);
			}
			return false;
		}

		// Token: 0x06006BF8 RID: 27640 RVA: 0x002A1D60 File Offset: 0x0029FF60
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)this.IntValue) * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)397) ^ (XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType)((this.displayName != null) ? this.displayName.GetHashCode() : 0));
		}

		// Token: 0x04005230 RID: 21040
		public readonly XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType Type;

		// Token: 0x04005231 RID: 21041
		public readonly int IntValue;

		// Token: 0x04005232 RID: 21042
		public readonly string StringValue;

		// Token: 0x04005233 RID: 21043
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayName;
	}
}
