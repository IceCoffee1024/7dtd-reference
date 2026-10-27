using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E25 RID: 3621
[Preserve]
public class XUiC_GamePrefSelector : XUiController
{
	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x06006E3D RID: 28221 RVA: 0x002AFD78 File Offset: 0x002ADF78
	// (set) Token: 0x06006E3E RID: 28222 RVA: 0x002AFD80 File Offset: 0x002ADF80
	[XuiXmlAttribute("color_default", false)]
	public Color EnabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.green;

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x06006E3F RID: 28223 RVA: 0x002AFD89 File Offset: 0x002ADF89
	// (set) Token: 0x06006E40 RID: 28224 RVA: 0x002AFD91 File Offset: 0x002ADF91
	[XuiXmlAttribute("color_changed", false)]
	public Color NonDefaultColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.yellow;

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x06006E41 RID: 28225 RVA: 0x002AFD9A File Offset: 0x002ADF9A
	// (set) Token: 0x06006E42 RID: 28226 RVA: 0x002AFDA2 File Offset: 0x002ADFA2
	[XuiXmlAttribute("color_disabled", false)]
	public Color DisabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.black;

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x06006E43 RID: 28227 RVA: 0x002AFDAB File Offset: 0x002ADFAB
	// (set) Token: 0x06006E44 RID: 28228 RVA: 0x002AFDB3 File Offset: 0x002ADFB3
	[XuiXmlAttribute("values_enforced", false)]
	public bool ValuesEnforced
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x06006E45 RID: 28229 RVA: 0x002AFDBC File Offset: 0x002ADFBC
	// (set) Token: 0x06006E46 RID: 28230 RVA: 0x002AFDC4 File Offset: 0x002ADFC4
	[XuiXmlAttribute("is_textinput", false)]
	public bool IsTextInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x06006E47 RID: 28231 RVA: 0x002AFDCD File Offset: 0x002ADFCD
	// (set) Token: 0x06006E48 RID: 28232 RVA: 0x002AFDD5 File Offset: 0x002ADFD5
	[XuiXmlAttribute("has_default", false)]
	public bool HasDefault
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x06006E49 RID: 28233 RVA: 0x002AFDDE File Offset: 0x002ADFDE
	// (set) Token: 0x06006E4A RID: 28234 RVA: 0x002AFDE6 File Offset: 0x002ADFE6
	[XuiXmlAttribute("always_show", false)]
	public bool AlwaysShow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x06006E4B RID: 28235 RVA: 0x002AFDEF File Offset: 0x002ADFEF
	// (set) Token: 0x06006E4C RID: 28236 RVA: 0x002AFDF7 File Offset: 0x002ADFF7
	[XuiXmlAttribute("value_type", false)]
	public GamePrefs.EnumType ValueType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x06006E4D RID: 28237 RVA: 0x002AFE00 File Offset: 0x002AE000
	public EnumGamePrefs GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x06006E4E RID: 28238 RVA: 0x002AFE08 File Offset: 0x002AE008
	public XUiC_TextInput ControlText
	{
		get
		{
			return this.controlText;
		}
	}

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x06006E4F RID: 28239 RVA: 0x002AFE10 File Offset: 0x002AE010
	// (set) Token: 0x06006E50 RID: 28240 RVA: 0x002AFE18 File Offset: 0x002AE018
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

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x06006E51 RID: 28241 RVA: 0x002AFE2F File Offset: 0x002AE02F
	// (set) Token: 0x06006E52 RID: 28242 RVA: 0x002AFE37 File Offset: 0x002AE037
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

	// Token: 0x06006E53 RID: 28243 RVA: 0x002AFE68 File Offset: 0x002AE068
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

	// Token: 0x06006E54 RID: 28244 RVA: 0x002AFEB8 File Offset: 0x002AE0B8
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

	// Token: 0x06006E55 RID: 28245 RVA: 0x002AFF06 File Offset: 0x002AE106
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006E56 RID: 28246 RVA: 0x002AFF20 File Offset: 0x002AE120
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

	// Token: 0x06006E57 RID: 28247 RVA: 0x002AFFB2 File Offset: 0x002AE1B2
	public override void OnOpen()
	{
		base.OnOpen();
		this.controlCombo.ViewComponent.IsVisible = !this.IsTextInput;
		this.controlText.ViewComponent.IsVisible = this.IsTextInput;
	}

	// Token: 0x06006E58 RID: 28248 RVA: 0x002AFFEC File Offset: 0x002AE1EC
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

	// Token: 0x06006E59 RID: 28249 RVA: 0x002B03F0 File Offset: 0x002AE5F0
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

	// Token: 0x06006E5A RID: 28250 RVA: 0x002B046C File Offset: 0x002AE66C
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

	// Token: 0x06006E5B RID: 28251 RVA: 0x002B0510 File Offset: 0x002AE710
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

	// Token: 0x06006E5C RID: 28252 RVA: 0x002B0908 File Offset: 0x002AEB08
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

	// Token: 0x06006E5D RID: 28253 RVA: 0x002B0958 File Offset: 0x002AEB58
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

	// Token: 0x06006E5E RID: 28254 RVA: 0x002B0A87 File Offset: 0x002AEC87
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetVisible(bool _visible)
	{
		this.viewComponent.IsVisible = _visible;
	}

	// Token: 0x06006E5F RID: 28255 RVA: 0x002B0A95 File Offset: 0x002AEC95
	public void SetCurrentGameMode(GameMode _gameMode)
	{
		this.currentGameMode = _gameMode;
		this.SetVisible(this.AlwaysShow || (_gameMode != null && _gameMode.GetGamePrefs().ContainsKey(this.gamePref)));
		this.CheckDefaultValue();
	}

	// Token: 0x06006E60 RID: 28256 RVA: 0x002B0ACC File Offset: 0x002AECCC
	[PublicizedFrom(EAccessModifier.Internal)]
	public void OverrideValues(List<string> overrideValues)
	{
		this.valuesFromXml = overrideValues.ToArray();
		this.SetupOptions();
	}

	// Token: 0x04005420 RID: 21536
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_GamePrefSelector.GameOptionValue> controlCombo;

	// Token: 0x04005421 RID: 21537
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput controlText;

	// Token: 0x04005425 RID: 21541
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] valuesFromXml;

	// Token: 0x04005426 RID: 21542
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x04005427 RID: 21543
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x0400542C RID: 21548
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGamePrefs gamePref = EnumGamePrefs.Last;

	// Token: 0x0400542E RID: 21550
	[PublicizedFrom(EAccessModifier.Private)]
	public GameMode currentGameMode;

	// Token: 0x0400542F RID: 21551
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x04005430 RID: 21552
	public Action<XUiC_GamePrefSelector, EnumGamePrefs> OnValueChanged;

	// Token: 0x04005431 RID: 21553
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x02000E26 RID: 3622
	public enum EOptionValueType
	{
		// Token: 0x04005433 RID: 21555
		Null,
		// Token: 0x04005434 RID: 21556
		Custom,
		// Token: 0x04005435 RID: 21557
		Any,
		// Token: 0x04005436 RID: 21558
		Int,
		// Token: 0x04005437 RID: 21559
		String
	}

	// Token: 0x02000E27 RID: 3623
	public readonly struct GameOptionValue : IEquatable<XUiC_GamePrefSelector.GameOptionValue>
	{
		// Token: 0x06006E62 RID: 28258 RVA: 0x002B0B2D File Offset: 0x002AED2D
		public GameOptionValue(XUiC_GamePrefSelector.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E63 RID: 28259 RVA: 0x002B0B4B File Offset: 0x002AED4B
		public GameOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E64 RID: 28260 RVA: 0x002B0B69 File Offset: 0x002AED69
		public GameOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.String;
			this.IntValue = -1;
			this.StringValue = _stringValue;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x002B0B87 File Offset: 0x002AED87
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x002B0B90 File Offset: 0x002AED90
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

		// Token: 0x06006E67 RID: 28263 RVA: 0x002B0BF8 File Offset: 0x002AEDF8
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_GamePrefSelector.GameOptionValue)
			{
				XUiC_GamePrefSelector.GameOptionValue gameOptionValue = (XUiC_GamePrefSelector.GameOptionValue)_obj;
				return this.Equals(gameOptionValue);
			}
			return false;
		}

		// Token: 0x06006E68 RID: 28264 RVA: 0x002B0C20 File Offset: 0x002AEE20
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)this.IntValue) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.DisplayName != null) ? this.DisplayName.GetHashCode() : 0));
		}

		// Token: 0x04005438 RID: 21560
		public readonly XUiC_GamePrefSelector.EOptionValueType Type;

		// Token: 0x04005439 RID: 21561
		public readonly int IntValue;

		// Token: 0x0400543A RID: 21562
		public readonly string StringValue;

		// Token: 0x0400543B RID: 21563
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string DisplayName;
	}
}
