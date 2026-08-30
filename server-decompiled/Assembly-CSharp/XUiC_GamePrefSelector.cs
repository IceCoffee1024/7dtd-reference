using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E0B RID: 3595
[Preserve]
public class XUiC_GamePrefSelector : XUiController
{
	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x06006E20 RID: 28192 RVA: 0x002AB464 File Offset: 0x002A9664
	// (set) Token: 0x06006E21 RID: 28193 RVA: 0x002AB46C File Offset: 0x002A966C
	[XuiXmlAttribute("color_default", false)]
	public Color EnabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.green;

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x06006E22 RID: 28194 RVA: 0x002AB475 File Offset: 0x002A9675
	// (set) Token: 0x06006E23 RID: 28195 RVA: 0x002AB47D File Offset: 0x002A967D
	[XuiXmlAttribute("color_changed", false)]
	public Color NonDefaultColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.yellow;

	// Token: 0x17000C36 RID: 3126
	// (get) Token: 0x06006E24 RID: 28196 RVA: 0x002AB486 File Offset: 0x002A9686
	// (set) Token: 0x06006E25 RID: 28197 RVA: 0x002AB48E File Offset: 0x002A968E
	[XuiXmlAttribute("color_disabled", false)]
	public Color DisabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.black;

	// Token: 0x17000C37 RID: 3127
	// (get) Token: 0x06006E26 RID: 28198 RVA: 0x002AB497 File Offset: 0x002A9697
	// (set) Token: 0x06006E27 RID: 28199 RVA: 0x002AB49F File Offset: 0x002A969F
	[XuiXmlAttribute("values_enforced", false)]
	public bool ValuesEnforced
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C38 RID: 3128
	// (get) Token: 0x06006E28 RID: 28200 RVA: 0x002AB4A8 File Offset: 0x002A96A8
	// (set) Token: 0x06006E29 RID: 28201 RVA: 0x002AB4B0 File Offset: 0x002A96B0
	[XuiXmlAttribute("is_textinput", false)]
	public bool IsTextInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C39 RID: 3129
	// (get) Token: 0x06006E2A RID: 28202 RVA: 0x002AB4B9 File Offset: 0x002A96B9
	// (set) Token: 0x06006E2B RID: 28203 RVA: 0x002AB4C1 File Offset: 0x002A96C1
	[XuiXmlAttribute("has_default", false)]
	public bool HasDefault
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x06006E2C RID: 28204 RVA: 0x002AB4CA File Offset: 0x002A96CA
	// (set) Token: 0x06006E2D RID: 28205 RVA: 0x002AB4D2 File Offset: 0x002A96D2
	[XuiXmlAttribute("always_show", false)]
	public bool AlwaysShow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06006E2E RID: 28206 RVA: 0x002AB4DB File Offset: 0x002A96DB
	// (set) Token: 0x06006E2F RID: 28207 RVA: 0x002AB4E3 File Offset: 0x002A96E3
	[XuiXmlAttribute("value_type", false)]
	public GamePrefs.EnumType ValueType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06006E30 RID: 28208 RVA: 0x002AB4EC File Offset: 0x002A96EC
	public EnumGamePrefs GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06006E31 RID: 28209 RVA: 0x002AB4F4 File Offset: 0x002A96F4
	public XUiC_TextInput ControlText
	{
		get
		{
			return this.controlText;
		}
	}

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06006E32 RID: 28210 RVA: 0x002AB4FC File Offset: 0x002A96FC
	// (set) Token: 0x06006E33 RID: 28211 RVA: 0x002AB504 File Offset: 0x002A9704
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

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x06006E34 RID: 28212 RVA: 0x002AB51B File Offset: 0x002A971B
	// (set) Token: 0x06006E35 RID: 28213 RVA: 0x002AB523 File Offset: 0x002A9723
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

	// Token: 0x06006E36 RID: 28214 RVA: 0x002AB554 File Offset: 0x002A9754
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

	// Token: 0x06006E37 RID: 28215 RVA: 0x002AB5A4 File Offset: 0x002A97A4
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

	// Token: 0x06006E38 RID: 28216 RVA: 0x002AB5F2 File Offset: 0x002A97F2
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006E39 RID: 28217 RVA: 0x002AB60C File Offset: 0x002A980C
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

	// Token: 0x06006E3A RID: 28218 RVA: 0x002AB69E File Offset: 0x002A989E
	public override void OnOpen()
	{
		base.OnOpen();
		this.controlCombo.ViewComponent.IsVisible = !this.IsTextInput;
		this.controlText.ViewComponent.IsVisible = this.IsTextInput;
	}

	// Token: 0x06006E3B RID: 28219 RVA: 0x002AB6D8 File Offset: 0x002A98D8
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

	// Token: 0x06006E3C RID: 28220 RVA: 0x002ABADC File Offset: 0x002A9CDC
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

	// Token: 0x06006E3D RID: 28221 RVA: 0x002ABB58 File Offset: 0x002A9D58
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

	// Token: 0x06006E3E RID: 28222 RVA: 0x002ABBFC File Offset: 0x002A9DFC
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

	// Token: 0x06006E3F RID: 28223 RVA: 0x002ABFF4 File Offset: 0x002AA1F4
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

	// Token: 0x06006E40 RID: 28224 RVA: 0x002AC044 File Offset: 0x002AA244
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

	// Token: 0x06006E41 RID: 28225 RVA: 0x002AC173 File Offset: 0x002AA373
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetVisible(bool _visible)
	{
		this.viewComponent.IsVisible = _visible;
	}

	// Token: 0x06006E42 RID: 28226 RVA: 0x002AC181 File Offset: 0x002AA381
	public void SetCurrentGameMode(GameMode _gameMode)
	{
		this.currentGameMode = _gameMode;
		this.SetVisible(this.AlwaysShow || (_gameMode != null && _gameMode.GetGamePrefs().ContainsKey(this.gamePref)));
		this.CheckDefaultValue();
	}

	// Token: 0x06006E43 RID: 28227 RVA: 0x002AC1B8 File Offset: 0x002AA3B8
	[PublicizedFrom(EAccessModifier.Internal)]
	public void OverrideValues(List<string> overrideValues)
	{
		this.valuesFromXml = overrideValues.ToArray();
		this.SetupOptions();
	}

	// Token: 0x040053C4 RID: 21444
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_GamePrefSelector.GameOptionValue> controlCombo;

	// Token: 0x040053C5 RID: 21445
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput controlText;

	// Token: 0x040053C9 RID: 21449
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] valuesFromXml;

	// Token: 0x040053CA RID: 21450
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x040053CB RID: 21451
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x040053D0 RID: 21456
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGamePrefs gamePref = EnumGamePrefs.Last;

	// Token: 0x040053D2 RID: 21458
	[PublicizedFrom(EAccessModifier.Private)]
	public GameMode currentGameMode;

	// Token: 0x040053D3 RID: 21459
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x040053D4 RID: 21460
	public Action<XUiC_GamePrefSelector, EnumGamePrefs> OnValueChanged;

	// Token: 0x040053D5 RID: 21461
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x02000E0C RID: 3596
	public enum EOptionValueType
	{
		// Token: 0x040053D7 RID: 21463
		Null,
		// Token: 0x040053D8 RID: 21464
		Custom,
		// Token: 0x040053D9 RID: 21465
		Any,
		// Token: 0x040053DA RID: 21466
		Int,
		// Token: 0x040053DB RID: 21467
		String
	}

	// Token: 0x02000E0D RID: 3597
	public readonly struct GameOptionValue : IEquatable<XUiC_GamePrefSelector.GameOptionValue>
	{
		// Token: 0x06006E45 RID: 28229 RVA: 0x002AC219 File Offset: 0x002AA419
		public GameOptionValue(XUiC_GamePrefSelector.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E46 RID: 28230 RVA: 0x002AC237 File Offset: 0x002AA437
		public GameOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E47 RID: 28231 RVA: 0x002AC255 File Offset: 0x002AA455
		public GameOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.String;
			this.IntValue = -1;
			this.StringValue = _stringValue;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x002AC273 File Offset: 0x002AA473
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06006E49 RID: 28233 RVA: 0x002AC27C File Offset: 0x002AA47C
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

		// Token: 0x06006E4A RID: 28234 RVA: 0x002AC2E4 File Offset: 0x002AA4E4
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_GamePrefSelector.GameOptionValue)
			{
				XUiC_GamePrefSelector.GameOptionValue gameOptionValue = (XUiC_GamePrefSelector.GameOptionValue)_obj;
				return this.Equals(gameOptionValue);
			}
			return false;
		}

		// Token: 0x06006E4B RID: 28235 RVA: 0x002AC30C File Offset: 0x002AA50C
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)this.IntValue) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.DisplayName != null) ? this.DisplayName.GetHashCode() : 0));
		}

		// Token: 0x040053DC RID: 21468
		public readonly XUiC_GamePrefSelector.EOptionValueType Type;

		// Token: 0x040053DD RID: 21469
		public readonly int IntValue;

		// Token: 0x040053DE RID: 21470
		public readonly string StringValue;

		// Token: 0x040053DF RID: 21471
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string DisplayName;
	}
}
