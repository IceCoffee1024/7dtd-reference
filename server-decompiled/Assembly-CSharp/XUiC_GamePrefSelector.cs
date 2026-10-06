using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E1A RID: 3610
[Preserve]
public class XUiC_GamePrefSelector : XUiController
{
	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06006E18 RID: 28184 RVA: 0x002AE80C File Offset: 0x002ACA0C
	// (set) Token: 0x06006E19 RID: 28185 RVA: 0x002AE814 File Offset: 0x002ACA14
	[XuiXmlAttribute("color_default", false)]
	public Color EnabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.green;

	// Token: 0x17000C50 RID: 3152
	// (get) Token: 0x06006E1A RID: 28186 RVA: 0x002AE81D File Offset: 0x002ACA1D
	// (set) Token: 0x06006E1B RID: 28187 RVA: 0x002AE825 File Offset: 0x002ACA25
	[XuiXmlAttribute("color_changed", false)]
	public Color NonDefaultColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.yellow;

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x06006E1C RID: 28188 RVA: 0x002AE82E File Offset: 0x002ACA2E
	// (set) Token: 0x06006E1D RID: 28189 RVA: 0x002AE836 File Offset: 0x002ACA36
	[XuiXmlAttribute("color_disabled", false)]
	public Color DisabledColor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = Color.black;

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x06006E1E RID: 28190 RVA: 0x002AE83F File Offset: 0x002ACA3F
	// (set) Token: 0x06006E1F RID: 28191 RVA: 0x002AE847 File Offset: 0x002ACA47
	[XuiXmlAttribute("values_enforced", false)]
	public bool ValuesEnforced
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x06006E20 RID: 28192 RVA: 0x002AE850 File Offset: 0x002ACA50
	// (set) Token: 0x06006E21 RID: 28193 RVA: 0x002AE858 File Offset: 0x002ACA58
	[XuiXmlAttribute("is_textinput", false)]
	public bool IsTextInput
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x06006E22 RID: 28194 RVA: 0x002AE861 File Offset: 0x002ACA61
	// (set) Token: 0x06006E23 RID: 28195 RVA: 0x002AE869 File Offset: 0x002ACA69
	[XuiXmlAttribute("has_default", false)]
	public bool HasDefault
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = true;

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x06006E24 RID: 28196 RVA: 0x002AE872 File Offset: 0x002ACA72
	// (set) Token: 0x06006E25 RID: 28197 RVA: 0x002AE87A File Offset: 0x002ACA7A
	[XuiXmlAttribute("always_show", false)]
	public bool AlwaysShow
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x06006E26 RID: 28198 RVA: 0x002AE883 File Offset: 0x002ACA83
	// (set) Token: 0x06006E27 RID: 28199 RVA: 0x002AE88B File Offset: 0x002ACA8B
	[XuiXmlAttribute("value_type", false)]
	public GamePrefs.EnumType ValueType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x06006E28 RID: 28200 RVA: 0x002AE894 File Offset: 0x002ACA94
	public EnumGamePrefs GamePref
	{
		get
		{
			return this.gamePref;
		}
	}

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x06006E29 RID: 28201 RVA: 0x002AE89C File Offset: 0x002ACA9C
	public XUiC_TextInput ControlText
	{
		get
		{
			return this.controlText;
		}
	}

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x06006E2A RID: 28202 RVA: 0x002AE8A4 File Offset: 0x002ACAA4
	// (set) Token: 0x06006E2B RID: 28203 RVA: 0x002AE8AC File Offset: 0x002ACAAC
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

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x06006E2C RID: 28204 RVA: 0x002AE8C3 File Offset: 0x002ACAC3
	// (set) Token: 0x06006E2D RID: 28205 RVA: 0x002AE8CB File Offset: 0x002ACACB
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

	// Token: 0x06006E2E RID: 28206 RVA: 0x002AE8FC File Offset: 0x002ACAFC
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

	// Token: 0x06006E2F RID: 28207 RVA: 0x002AE94C File Offset: 0x002ACB4C
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

	// Token: 0x06006E30 RID: 28208 RVA: 0x002AE99A File Offset: 0x002ACB9A
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006E31 RID: 28209 RVA: 0x002AE9B4 File Offset: 0x002ACBB4
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

	// Token: 0x06006E32 RID: 28210 RVA: 0x002AEA46 File Offset: 0x002ACC46
	public override void OnOpen()
	{
		base.OnOpen();
		this.controlCombo.ViewComponent.IsVisible = !this.IsTextInput;
		this.controlText.ViewComponent.IsVisible = this.IsTextInput;
	}

	// Token: 0x06006E33 RID: 28211 RVA: 0x002AEA80 File Offset: 0x002ACC80
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

	// Token: 0x06006E34 RID: 28212 RVA: 0x002AEE84 File Offset: 0x002AD084
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

	// Token: 0x06006E35 RID: 28213 RVA: 0x002AEF00 File Offset: 0x002AD100
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

	// Token: 0x06006E36 RID: 28214 RVA: 0x002AEFA4 File Offset: 0x002AD1A4
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

	// Token: 0x06006E37 RID: 28215 RVA: 0x002AF39C File Offset: 0x002AD59C
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

	// Token: 0x06006E38 RID: 28216 RVA: 0x002AF3EC File Offset: 0x002AD5EC
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

	// Token: 0x06006E39 RID: 28217 RVA: 0x002AF51B File Offset: 0x002AD71B
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetVisible(bool _visible)
	{
		this.viewComponent.IsVisible = _visible;
	}

	// Token: 0x06006E3A RID: 28218 RVA: 0x002AF529 File Offset: 0x002AD729
	public void SetCurrentGameMode(GameMode _gameMode)
	{
		this.currentGameMode = _gameMode;
		this.SetVisible(this.AlwaysShow || (_gameMode != null && _gameMode.GetGamePrefs().ContainsKey(this.gamePref)));
		this.CheckDefaultValue();
	}

	// Token: 0x06006E3B RID: 28219 RVA: 0x002AF560 File Offset: 0x002AD760
	[PublicizedFrom(EAccessModifier.Internal)]
	public void OverrideValues(List<string> overrideValues)
	{
		this.valuesFromXml = overrideValues.ToArray();
		this.SetupOptions();
	}

	// Token: 0x040053FB RID: 21499
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_GamePrefSelector.GameOptionValue> controlCombo;

	// Token: 0x040053FC RID: 21500
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput controlText;

	// Token: 0x04005400 RID: 21504
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] valuesFromXml;

	// Token: 0x04005401 RID: 21505
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x04005402 RID: 21506
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x04005407 RID: 21511
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGamePrefs gamePref = EnumGamePrefs.Last;

	// Token: 0x04005409 RID: 21513
	[PublicizedFrom(EAccessModifier.Private)]
	public GameMode currentGameMode;

	// Token: 0x0400540A RID: 21514
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x0400540B RID: 21515
	public Action<XUiC_GamePrefSelector, EnumGamePrefs> OnValueChanged;

	// Token: 0x0400540C RID: 21516
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x02000E1B RID: 3611
	public enum EOptionValueType
	{
		// Token: 0x0400540E RID: 21518
		Null,
		// Token: 0x0400540F RID: 21519
		Custom,
		// Token: 0x04005410 RID: 21520
		Any,
		// Token: 0x04005411 RID: 21521
		Int,
		// Token: 0x04005412 RID: 21522
		String
	}

	// Token: 0x02000E1C RID: 3612
	public readonly struct GameOptionValue : IEquatable<XUiC_GamePrefSelector.GameOptionValue>
	{
		// Token: 0x06006E3D RID: 28221 RVA: 0x002AF5C1 File Offset: 0x002AD7C1
		public GameOptionValue(XUiC_GamePrefSelector.EOptionValueType _type, string _displayName)
		{
			this.Type = _type;
			this.IntValue = -1;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x002AF5DF File Offset: 0x002AD7DF
		public GameOptionValue(int _intValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.Int;
			this.IntValue = _intValue;
			this.StringValue = null;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E3F RID: 28223 RVA: 0x002AF5FD File Offset: 0x002AD7FD
		public GameOptionValue(string _stringValue, string _displayName)
		{
			this.Type = XUiC_GamePrefSelector.EOptionValueType.String;
			this.IntValue = -1;
			this.StringValue = _stringValue;
			this.DisplayName = _displayName;
		}

		// Token: 0x06006E40 RID: 28224 RVA: 0x002AF61B File Offset: 0x002AD81B
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06006E41 RID: 28225 RVA: 0x002AF624 File Offset: 0x002AD824
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

		// Token: 0x06006E42 RID: 28226 RVA: 0x002AF68C File Offset: 0x002AD88C
		public override bool Equals(object _obj)
		{
			if (_obj is XUiC_GamePrefSelector.GameOptionValue)
			{
				XUiC_GamePrefSelector.GameOptionValue gameOptionValue = (XUiC_GamePrefSelector.GameOptionValue)_obj;
				return this.Equals(gameOptionValue);
			}
			return false;
		}

		// Token: 0x06006E43 RID: 28227 RVA: 0x002AF6B4 File Offset: 0x002AD8B4
		public override int GetHashCode()
		{
			return (int)((((((this.Type * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)this.IntValue) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.StringValue != null) ? this.StringValue.GetHashCode() : 0)) * (XUiC_GamePrefSelector.EOptionValueType)397) ^ (XUiC_GamePrefSelector.EOptionValueType)((this.DisplayName != null) ? this.DisplayName.GetHashCode() : 0));
		}

		// Token: 0x04005413 RID: 21523
		public readonly XUiC_GamePrefSelector.EOptionValueType Type;

		// Token: 0x04005414 RID: 21524
		public readonly int IntValue;

		// Token: 0x04005415 RID: 21525
		public readonly string StringValue;

		// Token: 0x04005416 RID: 21526
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string DisplayName;
	}
}
