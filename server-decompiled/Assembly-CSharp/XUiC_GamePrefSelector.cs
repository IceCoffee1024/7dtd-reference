using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DF6 RID: 3574
[Preserve]
public class XUiC_GamePrefSelector : XUiController
{
	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06006D80 RID: 28032 RVA: 0x002A7F50 File Offset: 0x002A6150
	// (set) Token: 0x06006D81 RID: 28033 RVA: 0x002A7F58 File Offset: 0x002A6158
	[XuiXmlAttribute("color_default", false)]
	public Color EnabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.green;

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06006D82 RID: 28034 RVA: 0x002A7F61 File Offset: 0x002A6161
	// (set) Token: 0x06006D83 RID: 28035 RVA: 0x002A7F69 File Offset: 0x002A6169
	[XuiXmlAttribute("color_changed", false)]
	public Color NonDefaultColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.yellow;

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x06006D84 RID: 28036 RVA: 0x002A7F72 File Offset: 0x002A6172
	// (set) Token: 0x06006D85 RID: 28037 RVA: 0x002A7F7A File Offset: 0x002A617A
	[XuiXmlAttribute("color_disabled", false)]
	public Color DisabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.black;

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x06006D86 RID: 28038 RVA: 0x002A7F83 File Offset: 0x002A6183
	// (set) Token: 0x06006D87 RID: 28039 RVA: 0x002A7F8B File Offset: 0x002A618B
	[XuiXmlAttribute("values_enforced", false)]
	public bool ValuesEnforced
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x06006D88 RID: 28040 RVA: 0x002A7F94 File Offset: 0x002A6194
	// (set) Token: 0x06006D89 RID: 28041 RVA: 0x002A7F9C File Offset: 0x002A619C
	[XuiXmlAttribute("is_textinput", false)]
	public bool IsTextInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x06006D8A RID: 28042 RVA: 0x002A7FA5 File Offset: 0x002A61A5
	// (set) Token: 0x06006D8B RID: 28043 RVA: 0x002A7FAD File Offset: 0x002A61AD
	[XuiXmlAttribute("has_default", false)]
	public bool HasDefault
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x06006D8C RID: 28044 RVA: 0x002A7FB6 File Offset: 0x002A61B6
	// (set) Token: 0x06006D8D RID: 28045 RVA: 0x002A7FBE File Offset: 0x002A61BE
	[XuiXmlAttribute("always_show", false)]
	public bool AlwaysShow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x06006D8E RID: 28046 RVA: 0x002A7FC7 File Offset: 0x002A61C7
	// (set) Token: 0x06006D8F RID: 28047 RVA: 0x002A7FCF File Offset: 0x002A61CF
	[XuiXmlAttribute("value_type", false)]
	public GamePrefs.EnumType ValueType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x06006D90 RID: 28048 RVA: 0x002A7FD8 File Offset: 0x002A61D8
	public EnumGamePrefs GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x06006D91 RID: 28049 RVA: 0x002A7FE0 File Offset: 0x002A61E0
	public XUiC_TextInput ControlText
	{
		get
		{
			return this.controlText;
		}
	}

	// Token: 0x17000C25 RID: 3109
	// (get) Token: 0x06006D92 RID: 28050 RVA: 0x002A7FE8 File Offset: 0x002A61E8
	// (set) Token: 0x06006D93 RID: 28051 RVA: 0x002A7FF0 File Offset: 0x002A61F0
	public Func<int, int> ValuePreDisplayModifierFunc
	{
		get
		{
			return this.valuePreDisplayModifierFunc;
		}
		set
		{
			this.valuePreDisplayModifierFunc = value;
			if (this.valuePreDisplayModifierFunc != null)
			{
				this.SetupOptions();
			}
		}
	}

	// Token: 0x17000C26 RID: 3110
	// (get) Token: 0x06006D94 RID: 28052 RVA: 0x002A8007 File Offset: 0x002A6207
	// (set) Token: 0x06006D95 RID: 28053 RVA: 0x002A800F File Offset: 0x002A620F
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
				this.controlText.Enabled = value;
				this.CheckDefaultValue();
			}
		}
	}

	// Token: 0x06006D96 RID: 28054 RVA: 0x002A8040 File Offset: 0x002A6240
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

	// Token: 0x06006D97 RID: 28055 RVA: 0x002A8090 File Offset: 0x002A6290
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

	// Token: 0x06006D98 RID: 28056 RVA: 0x002A80DE File Offset: 0x002A62DE
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006D99 RID: 28057 RVA: 0x002A80F8 File Offset: 0x002A62F8
	public override void Init()
	{
		base.Init();
		this.gamePref = EnumUtils.Parse<EnumGamePrefs>(this.viewComponent.ID, false);
		this.controlCombo = base.GetChildById("ControlCombo").GetChildByType<XUiC_ComboBoxList<XUiC_GamePrefSelector.GameOptionValue>>();
		this.controlCombo.OnValueChanged += this.ControlCombo_OnValueChanged;
		this.controlText = base.GetChildById("ControlText").GetChildByType<XUiC_TextInput>();
		this.controlText.OnChangeHandler += this.ControlText_OnChangeHandler;
		if (!this.IsTextInput)
		{
			this.SetupOptions();
		}
	}

	// Token: 0x06006D9A RID: 28058 RVA: 0x002A818A File Offset: 0x002A638A
	public override void OnOpen()
	{
		base.OnOpen();
		this.controlCombo.ViewComponent.IsVisible = !this.IsTextInput;
		this.controlText.ViewComponent.IsVisible = this.IsTextInput;
	}

	// Token: 0x06006D9B RID: 28059 RVA: 0x002A81C4 File Offset: 0x002A63C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupOptions()
	{
		int[] array = null;
		string[] array2 = null;
		string[] array3;
		switch (this.ValueType)
		{
		case GamePrefs.EnumType.Int:
		{
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
					throw new Exception("Illegal option setup for " + this.gamePref.ToStringCached<EnumGamePrefs>() + " (no values and no names specified)");
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
				goto IL_030C;
			}
			for (int l = 0; l < this.namesFromXml.Length; l++)
			{
				array3[l] = Localization.Get(this.namesFromXml[l], false, null);
			}
			goto IL_030C;
		}
		case GamePrefs.EnumType.String:
		{
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
				goto IL_030C;
			}
			for (int n = 0; n < this.namesFromXml.Length; n++)
			{
				array3[n] = Localization.Get(this.namesFromXml[n], false, null);
			}
			goto IL_030C;
		}
		case GamePrefs.EnumType.Bool:
			array = new int[] { 0, 1 };
			array3 = new string[2];
			if (this.namesFromXml != null && this.namesFromXml.Length == 2)
			{
				array3[0] = Localization.Get(this.namesFromXml[0], false, null);
				array3[1] = Localization.Get(this.namesFromXml[1], false, null);
				goto IL_030C;
			}
			array3[0] = Localization.Get("goOff", false, null);
			array3[1] = Localization.Get("goOn", false, null);
			goto IL_030C;
		}
		throw new NotSupportedException("Not a valid GamePref: " + this.viewComponent.ID);
		IL_030C:
		this.controlCombo.Elements.Clear();
		if (array3 == null)
		{
			throw new Exception("Illegal option setup for " + this.gamePref.ToStringCached<EnumGamePrefs>() + " (names null)");
		}
		if (this.ValueType != GamePrefs.EnumType.String)
		{
			if (array == null)
			{
				throw new Exception("Illegal option setup for " + this.gamePref.ToStringCached<EnumGamePrefs>() + " (values still null)");
			}
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				this.controlCombo.Elements.Add(new XUiC_GamePrefSelector.GameOptionValue(array[num2], array3[num2]));
			}
		}
		if (this.ValueType == GamePrefs.EnumType.String)
		{
			if (array2 == null)
			{
				throw new Exception("Illegal option setup for " + this.gamePref.ToStringCached<EnumGamePrefs>() + " (values still null)");
			}
			for (int num3 = 0; num3 < array2.Length; num3++)
			{
				this.controlCombo.Elements.Add(new XUiC_GamePrefSelector.GameOptionValue(array2[num3], array3[num3]));
			}
		}
	}

	// Token: 0x06006D9C RID: 28060 RVA: 0x002A85C8 File Offset: 0x002A67C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlText_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		GamePrefs.EnumType valueType = this.ValueType;
		int num;
		if (valueType != GamePrefs.EnumType.Int)
		{
			if (valueType != GamePrefs.EnumType.String)
			{
				throw new Exception("Illegal option setup for " + this.gamePref.ToStringCached<EnumGamePrefs>());
			}
			GamePrefs.Set(this.gamePref, _text);
		}
		else if (int.TryParse(_text, out num))
		{
			GamePrefs.Set(this.gamePref, num);
		}
		Action<XUiC_GamePrefSelector, EnumGamePrefs> onValueChanged = this.OnValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(this, this.gamePref);
		}
		this.CheckDefaultValue();
	}

	// Token: 0x06006D9D RID: 28061 RVA: 0x002A8644 File Offset: 0x002A6844
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlCombo_OnValueChanged(XUiController _sender, XUiC_GamePrefSelector.GameOptionValue _oldValue, XUiC_GamePrefSelector.GameOptionValue _newValue)
	{
		switch (this.ValueType)
		{
		case GamePrefs.EnumType.Int:
			GamePrefs.Set(this.gamePref, _newValue.IntValue);
			goto IL_0076;
		case GamePrefs.EnumType.String:
			GamePrefs.Set(this.gamePref, _newValue.StringValue);
			goto IL_0076;
		case GamePrefs.EnumType.Bool:
			GamePrefs.Set(this.gamePref, _newValue.IntValue == 1);
			goto IL_0076;
		}
		throw new Exception("Illegal option setup for " + this.gamePref.ToStringCached<EnumGamePrefs>());
		IL_0076:
		Action<XUiC_GamePrefSelector, EnumGamePrefs> onValueChanged = this.OnValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(this, this.gamePref);
		}
		this.CheckDefaultValue();
	}

	// Token: 0x06006D9E RID: 28062 RVA: 0x002A86E8 File Offset: 0x002A68E8
	public void SetCurrentValue()
	{
		try
		{
			switch (this.ValueType)
			{
			case GamePrefs.EnumType.Int:
			{
				int @int = GamePrefs.GetInt(this.gamePref);
				if (this.IsTextInput)
				{
					this.controlText.Text = @int.ToString();
					goto IL_03A8;
				}
				bool flag = false;
				for (int i = 1; i < this.controlCombo.Elements.Count; i++)
				{
					if (this.controlCombo.Elements[i].IntValue == @int)
					{
						this.controlCombo.SelectedIndex = i;
						flag = true;
						break;
					}
				}
				if (this.ValuesEnforced && !flag)
				{
					int num = -1;
					int num2 = int.MaxValue;
					for (int j = 0; j < this.controlCombo.Elements.Count; j++)
					{
						int num3 = Math.Abs(this.controlCombo.Elements[j].IntValue - @int);
						if (num < 0 || num3 < num2)
						{
							num = j;
							num2 = num3;
							if (num2 <= 0)
							{
								break;
							}
						}
					}
					if (num >= 0)
					{
						this.controlCombo.SelectedIndex = num;
						GamePrefs.Set(this.gamePref, this.controlCombo.Value.IntValue);
						flag = true;
					}
				}
				if (flag)
				{
					goto IL_03A8;
				}
				if (string.IsNullOrEmpty(this.valueLocalizationPrefixFromXml))
				{
					XUiC_GamePrefSelector.GameOptionValue gameOptionValue = new XUiC_GamePrefSelector.GameOptionValue(@int, string.Format("{0} {1}", @int.ToString(), Localization.Get("goCustomValueSuffix", false, null)));
					this.controlCombo.Value = gameOptionValue;
					goto IL_03A8;
				}
				XUiC_GamePrefSelector.GameOptionValue gameOptionValue2 = new XUiC_GamePrefSelector.GameOptionValue(@int, string.Format(Localization.Get(this.valueLocalizationPrefixFromXml + ((@int == 1) ? "" : "s"), false, null), @int.ToString()) + " " + Localization.Get("goCustomValueSuffix", false, null));
				this.controlCombo.Value = gameOptionValue2;
				goto IL_03A8;
			}
			case GamePrefs.EnumType.String:
			{
				string @string = GamePrefs.GetString(this.gamePref);
				if (this.IsTextInput)
				{
					this.controlText.Text = GamePrefs.GetString(this.gamePref);
					goto IL_03A8;
				}
				bool flag2 = false;
				for (int k = 1; k < this.controlCombo.Elements.Count; k++)
				{
					if (this.controlCombo.Elements[k].StringValue == @string)
					{
						this.controlCombo.SelectedIndex = k;
						flag2 = true;
						break;
					}
				}
				if (this.ValuesEnforced && !flag2 && this.controlCombo.Elements.Count > 0)
				{
					this.controlCombo.SelectedIndex = 0;
					GamePrefs.Set(this.gamePref, this.controlCombo.Value.StringValue);
					flag2 = true;
				}
				if (flag2)
				{
					goto IL_03A8;
				}
				if (string.IsNullOrEmpty(@string))
				{
					this.controlCombo.SelectedIndex = 0;
					GamePrefs.Set(this.gamePref, this.controlCombo.Value.StringValue);
					goto IL_03A8;
				}
				if (string.IsNullOrEmpty(this.valueLocalizationPrefixFromXml))
				{
					XUiC_GamePrefSelector.GameOptionValue gameOptionValue3 = new XUiC_GamePrefSelector.GameOptionValue(@string, string.Format("{0} {1}", @string, Localization.Get("goCustomValueSuffix", false, null)));
					this.controlCombo.Value = gameOptionValue3;
					goto IL_03A8;
				}
				XUiC_GamePrefSelector.GameOptionValue gameOptionValue4 = new XUiC_GamePrefSelector.GameOptionValue(@string, string.Format(Localization.Get(this.valueLocalizationPrefixFromXml ?? "", false, null), @string) + " " + Localization.Get("goCustomValueSuffix", false, null));
				this.controlCombo.Value = gameOptionValue4;
				goto IL_03A8;
			}
			case GamePrefs.EnumType.Bool:
				this.controlCombo.SelectedIndex = (GamePrefs.GetBool(this.gamePref) ? 1 : 0);
				goto IL_03A8;
			}
			throw new Exception("Illegal option setup for " + this.gamePref.ToStringCached<EnumGamePrefs>());
			IL_03A8:;
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		Action<XUiC_GamePrefSelector, EnumGamePrefs> onValueChanged = this.OnValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(this, this.gamePref);
		}
		this.CheckDefaultValue();
	}

	// Token: 0x06006D9F RID: 28063 RVA: 0x002A8AE0 File Offset: 0x002A6CE0
	public void CheckDefaultValue()
	{
		Color color;
		if (this.enabled)
		{
			color = (this.IsDefaultValueForGameMode() ? this.EnabledColor : this.NonDefaultColor);
		}
		else
		{
			color = this.DisabledColor;
		}
		this.controlText.ActiveTextColor = color;
		this.controlCombo.TextColor = color;
	}

	// Token: 0x06006DA0 RID: 28064 RVA: 0x002A8B30 File Offset: 0x002A6D30
	public bool IsDefaultValueForGameMode()
	{
		if (!this.HasDefault)
		{
			return true;
		}
		if (this.currentGameMode == null)
		{
			return true;
		}
		Dictionary<EnumGamePrefs, GameMode.ModeGamePref> gamePrefs = this.currentGameMode.GetGamePrefs();
		if (!gamePrefs.ContainsKey(this.gamePref))
		{
			return false;
		}
		switch (this.ValueType)
		{
		case GamePrefs.EnumType.Int:
		{
			int intValue;
			if (this.IsTextInput)
			{
				StringParsers.TryParseSInt32(this.controlText.Text, out intValue);
			}
			else
			{
				intValue = this.controlCombo.Value.IntValue;
			}
			return intValue == (int)gamePrefs[this.gamePref].DefaultValue;
		}
		case GamePrefs.EnumType.String:
			if (this.IsTextInput)
			{
				return this.controlText.Text == (string)gamePrefs[this.gamePref].DefaultValue;
			}
			return this.controlCombo.Value.StringValue == (string)gamePrefs[this.gamePref].DefaultValue;
		case GamePrefs.EnumType.Bool:
			return this.controlCombo.Value.IntValue == 1 == (bool)gamePrefs[this.gamePref].DefaultValue;
		}
		return false;
	}

	// Token: 0x06006DA1 RID: 28065 RVA: 0x002A8C5F File Offset: 0x002A6E5F
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetVisible(bool _visible)
	{
		this.viewComponent.IsVisible = _visible;
	}

	// Token: 0x06006DA2 RID: 28066 RVA: 0x002A8C6D File Offset: 0x002A6E6D
	public void SetCurrentGameMode(GameMode _gameMode)
	{
		this.currentGameMode = _gameMode;
		this.SetVisible(this.AlwaysShow || (_gameMode != null && _gameMode.GetGamePrefs().ContainsKey(this.gamePref)));
		this.CheckDefaultValue();
	}

	// Token: 0x06006DA3 RID: 28067 RVA: 0x002A8CA4 File Offset: 0x002A6EA4
	[PublicizedFrom(EAccessModifier.Internal)]
	public void OverrideValues(List<string> overrideValues)
	{
		this.valuesFromXml = overrideValues.ToArray();
		this.SetupOptions();
	}

	// Token: 0x04005321 RID: 21281
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_GamePrefSelector.GameOptionValue> controlCombo;

	// Token: 0x04005322 RID: 21282
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput controlText;

	// Token: 0x04005326 RID: 21286
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] valuesFromXml;

	// Token: 0x04005327 RID: 21287
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x04005328 RID: 21288
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x0400532D RID: 21293
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGamePrefs gamePref = EnumGamePrefs.Last;

	// Token: 0x0400532F RID: 21295
	[PublicizedFrom(EAccessModifier.Private)]
	public GameMode currentGameMode;

	// Token: 0x04005330 RID: 21296
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x04005331 RID: 21297
	public Action<XUiC_GamePrefSelector, EnumGamePrefs> OnValueChanged;

	// Token: 0x04005332 RID: 21298
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x02000DF7 RID: 3575
	public enum EOptionValueType
	{
		// Token: 0x04005334 RID: 21300
		Null,
		// Token: 0x04005335 RID: 21301
		Custom,
		// Token: 0x04005336 RID: 21302
		Any,
		// Token: 0x04005337 RID: 21303
		Int,
		// Token: 0x04005338 RID: 21304
		String
	}

	// Token: 0x02000DF8 RID: 3576
	public readonly struct GameOptionValue : IEquatable<XUiC_GamePrefSelector.GameOptionValue>
	{
		// Token: 0x06006DA5 RID: 28069 RVA: 0x002A8D05 File Offset: 0x002A6F05
		public GameOptionValue(XUiC_GamePrefSelector.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x002A8D23 File Offset: 0x002A6F23
		public GameOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x002A8D41 File Offset: 0x002A6F41
		public GameOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.String;
			this.IntValue = -1;
			this.StringValue = _stringValue;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006DA8 RID: 28072 RVA: 0x002A8D5F File Offset: 0x002A6F5F
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06006DA9 RID: 28073 RVA: 0x002A8D68 File Offset: 0x002A6F68
		public bool Equals(XUiC_GamePrefSelector.GameOptionValue _other)
		{
			if (this.Type != _other.Type)
			{
				return false;
			}
			switch (this.Type)
			{
			case XUiC_GamePrefSelector.EOptionValueType.Custom:
			case XUiC_GamePrefSelector.EOptionValueType.Any:
				return true;
			case XUiC_GamePrefSelector.EOptionValueType.Int:
				return this.IntValue == _other.IntValue;
			case XUiC_GamePrefSelector.EOptionValueType.String:
				return this.StringValue == _other.StringValue;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x002A8DD0 File Offset: 0x002A6FD0
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_GamePrefSelector.GameOptionValue)
			{
				XUiC_GamePrefSelector.GameOptionValue gameOptionValue = (XUiC_GamePrefSelector.GameOptionValue)_obj;
				return this.Equals(gameOptionValue);
			}
			return false;
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x002A8DF8 File Offset: 0x002A6FF8
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)this.IntValue) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.DisplayName != null) ? this.DisplayName.GetHashCode() : 0));
		}

		// Token: 0x04005339 RID: 21305
		public readonly XUiC_GamePrefSelector.EOptionValueType Type;

		// Token: 0x0400533A RID: 21306
		public readonly int IntValue;

		// Token: 0x0400533B RID: 21307
		public readonly string StringValue;

		// Token: 0x0400533C RID: 21308
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string DisplayName;
	}
}
