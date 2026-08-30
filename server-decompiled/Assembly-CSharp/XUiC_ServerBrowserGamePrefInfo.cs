using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000DD3 RID: 3539
[Preserve]
public class XUiC_ServerBrowserGamePrefInfo : XUiController
{
	// Token: 0x17000BE0 RID: 3040
	// (get) Token: 0x06006C3B RID: 27707 RVA: 0x002A31BF File Offset: 0x002A13BF
	public GamePrefs.EnumType ValueType
	{
		get
		{
			return this.valueType;
		}
	}

	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x06006C3C RID: 27708 RVA: 0x002A31C7 File Offset: 0x002A13C7
	public GameInfoBool GameInfoBool
	{
		get
		{
			return this.gameInfoBool;
		}
	}

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x06006C3D RID: 27709 RVA: 0x002A31CF File Offset: 0x002A13CF
	public GameInfoInt GameInfoInt
	{
		get
		{
			return this.gameInfoInt;
		}
	}

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x06006C3E RID: 27710 RVA: 0x002A31D7 File Offset: 0x002A13D7
	public GameInfoString GameInfoString
	{
		get
		{
			return this.gameInfoString;
		}
	}

	// Token: 0x17000BE4 RID: 3044
	// (set) Token: 0x06006C3F RID: 27711 RVA: 0x002A31DF File Offset: 0x002A13DF
	public Func<int, int> ValuePreDisplayModifierFunc
	{
		set
		{
			if (this.valuePreDisplayModifierFunc == value)
			{
				return;
			}
			this.valuePreDisplayModifierFunc = value;
			this.setupOptions();
		}
	}

	// Token: 0x17000BE5 RID: 3045
	// (set) Token: 0x06006C40 RID: 27712 RVA: 0x002A31FD File Offset: 0x002A13FD
	public Func<GameServerInfo, int, string> CustomIntValueFormatter
	{
		set
		{
			if (this.customIntValueFormatter == value)
			{
				return;
			}
			this.customIntValueFormatter = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000BE6 RID: 3046
	// (set) Token: 0x06006C41 RID: 27713 RVA: 0x002A321C File Offset: 0x002A141C
	public Func<GameServerInfo, string, string> CustomStringValueFormatter
	{
		set
		{
			if (this.customStringValueFormatter == value)
			{
				return;
			}
			this.customStringValueFormatter = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006C42 RID: 27714 RVA: 0x002A323C File Offset: 0x002A143C
	public override void Init()
	{
		base.Init();
		if (EnumUtils.TryParse<GameInfoBool>(this.viewComponent.ID, out this.gameInfoBool, false))
		{
			this.valueType = GamePrefs.EnumType.Bool;
		}
		else
		{
			this.gameInfoBool = (GameInfoBool)(-1);
		}
		if (EnumUtils.TryParse<GameInfoInt>(this.viewComponent.ID, out this.gameInfoInt, false))
		{
			this.valueType = GamePrefs.EnumType.Int;
		}
		else
		{
			this.gameInfoInt = (GameInfoInt)(-1);
		}
		if (EnumUtils.TryParse<GameInfoString>(this.viewComponent.ID, out this.gameInfoString, false))
		{
			this.valueType = GamePrefs.EnumType.String;
		}
		else
		{
			this.gameInfoString = (GameInfoString)(-1);
		}
		this.label = (XUiV_Label)base.GetChildById("value").ViewComponent;
		this.setupOptions();
	}

	// Token: 0x06006C43 RID: 27715 RVA: 0x002A32EC File Offset: 0x002A14EC
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

	// Token: 0x06006C44 RID: 27716 RVA: 0x002A333A File Offset: 0x002A153A
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006C45 RID: 27717 RVA: 0x002A3354 File Offset: 0x002A1554
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupOptions()
	{
		if (this.valueType == GamePrefs.EnumType.Int)
		{
			if (this.namesFromXml != null)
			{
				this.values = new List<XUiC_ServerBrowserGamePrefInfo.GameOptionValue>();
				for (int i = 0; i < this.namesFromXml.Length; i++)
				{
					string text = this.namesFromXml[i];
					int num = i;
					if (text.IndexOf('=') > 0)
					{
						string[] array = text.Split('=', StringSplitOptions.None);
						text = array[1];
						num = StringParsers.ParseSInt32(array[0], 0, -1, NumberStyles.Integer);
					}
					this.values.Add(new XUiC_ServerBrowserGamePrefInfo.GameOptionValue(num, Localization.Get(text, false, null)));
				}
				return;
			}
		}
		else if (this.valueType == GamePrefs.EnumType.Bool)
		{
			this.values = new List<XUiC_ServerBrowserGamePrefInfo.GameOptionValue>
			{
				new XUiC_ServerBrowserGamePrefInfo.GameOptionValue(0, Localization.Get("goOff", false, null)),
				new XUiC_ServerBrowserGamePrefInfo.GameOptionValue(1, Localization.Get("goOn", false, null))
			};
		}
	}

	// Token: 0x06006C46 RID: 27718 RVA: 0x002A3420 File Offset: 0x002A1620
	public void SetCurrentValue(GameServerInfo _gameInfo)
	{
		try
		{
			if (_gameInfo != null)
			{
				if (this.valueType == GamePrefs.EnumType.Int)
				{
					int value = _gameInfo.GetValue(this.gameInfoInt);
					bool flag = false;
					if (this.values != null)
					{
						foreach (XUiC_ServerBrowserGamePrefInfo.GameOptionValue gameOptionValue in this.values)
						{
							if (gameOptionValue.Value == value)
							{
								this.label.Text = gameOptionValue.ToString();
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						if (this.customIntValueFormatter != null)
						{
							this.label.Text = this.customIntValueFormatter(_gameInfo, value);
						}
						else if (this.valueLocalizationPrefixFromXml != null)
						{
							this.label.Text = string.Format(Localization.Get(this.valueLocalizationPrefixFromXml + ((value == 1) ? "" : "s"), false, null), value);
						}
						else
						{
							this.label.Text = value.ToString();
						}
					}
				}
				else if (this.valueType == GamePrefs.EnumType.Bool)
				{
					bool value2 = _gameInfo.GetValue(this.gameInfoBool);
					this.label.Text = (value2 ? this.values[1].ToString() : this.values[0].ToString());
				}
				else if (this.valueType == GamePrefs.EnumType.String)
				{
					string value3 = _gameInfo.GetValue(this.gameInfoString);
					if (this.customStringValueFormatter != null)
					{
						this.label.Text = this.customStringValueFormatter(_gameInfo, value3);
					}
					else if (this.valueLocalizationPrefixFromXml != null && !string.IsNullOrEmpty(value3))
					{
						this.label.Text = Localization.Get(this.valueLocalizationPrefixFromXml + value3, false, null);
					}
					else
					{
						this.label.Text = value3;
					}
				}
			}
			else
			{
				this.label.Text = "";
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
	}

	// Token: 0x04005298 RID: 21144
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x04005299 RID: 21145
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x0400529A RID: 21146
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x0400529B RID: 21147
	[PublicizedFrom(EAccessModifier.Private)]
	public GamePrefs.EnumType valueType;

	// Token: 0x0400529C RID: 21148
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoBool gameInfoBool;

	// Token: 0x0400529D RID: 21149
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x0400529E RID: 21150
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoString gameInfoString;

	// Token: 0x0400529F RID: 21151
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ServerBrowserGamePrefInfo.GameOptionValue> values;

	// Token: 0x040052A0 RID: 21152
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x040052A1 RID: 21153
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<GameServerInfo, int, string> customIntValueFormatter;

	// Token: 0x040052A2 RID: 21154
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<GameServerInfo, string, string> customStringValueFormatter;

	// Token: 0x02000DD4 RID: 3540
	public struct GameOptionValue
	{
		// Token: 0x06006C48 RID: 27720 RVA: 0x002A3660 File Offset: 0x002A1860
		public GameOptionValue(int _value, string _displayName)
		{
			this.Value = _value;
			this.displayName = _displayName;
		}

		// Token: 0x06006C49 RID: 27721 RVA: 0x002A3670 File Offset: 0x002A1870
		public override string ToString()
		{
			return this.displayName;
		}

		// Token: 0x040052A3 RID: 21155
		public readonly int Value;

		// Token: 0x040052A4 RID: 21156
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayName;
	}
}
