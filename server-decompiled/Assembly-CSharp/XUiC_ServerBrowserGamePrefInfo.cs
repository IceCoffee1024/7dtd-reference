using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000DED RID: 3565
[Preserve]
public class XUiC_ServerBrowserGamePrefInfo : XUiController
{
	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x06006C58 RID: 27736 RVA: 0x002A7A77 File Offset: 0x002A5C77
	public GamePrefs.EnumType ValueType
	{
		get
		{
			return this.valueType;
		}
	}

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x06006C59 RID: 27737 RVA: 0x002A7A7F File Offset: 0x002A5C7F
	public GameInfoBool GameInfoBool
	{
		get
		{
			return this.gameInfoBool;
		}
	}

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x06006C5A RID: 27738 RVA: 0x002A7A87 File Offset: 0x002A5C87
	public GameInfoInt GameInfoInt
	{
		get
		{
			return this.gameInfoInt;
		}
	}

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x06006C5B RID: 27739 RVA: 0x002A7A8F File Offset: 0x002A5C8F
	public GameInfoString GameInfoString
	{
		get
		{
			return this.gameInfoString;
		}
	}

	// Token: 0x17000C05 RID: 3077
	// (set) Token: 0x06006C5C RID: 27740 RVA: 0x002A7A97 File Offset: 0x002A5C97
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

	// Token: 0x17000C06 RID: 3078
	// (set) Token: 0x06006C5D RID: 27741 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
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

	// Token: 0x17000C07 RID: 3079
	// (set) Token: 0x06006C5E RID: 27742 RVA: 0x002A7AD4 File Offset: 0x002A5CD4
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

	// Token: 0x06006C5F RID: 27743 RVA: 0x002A7AF4 File Offset: 0x002A5CF4
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

	// Token: 0x06006C60 RID: 27744 RVA: 0x002A7BA4 File Offset: 0x002A5DA4
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

	// Token: 0x06006C61 RID: 27745 RVA: 0x002A7BF2 File Offset: 0x002A5DF2
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006C62 RID: 27746 RVA: 0x002A7C0C File Offset: 0x002A5E0C
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

	// Token: 0x06006C63 RID: 27747 RVA: 0x002A7CD8 File Offset: 0x002A5ED8
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

	// Token: 0x040052F4 RID: 21236
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x040052F5 RID: 21237
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x040052F6 RID: 21238
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x040052F7 RID: 21239
	[PublicizedFrom(EAccessModifier.Private)]
	public GamePrefs.EnumType valueType;

	// Token: 0x040052F8 RID: 21240
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoBool gameInfoBool;

	// Token: 0x040052F9 RID: 21241
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x040052FA RID: 21242
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoString gameInfoString;

	// Token: 0x040052FB RID: 21243
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ServerBrowserGamePrefInfo.GameOptionValue> values;

	// Token: 0x040052FC RID: 21244
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x040052FD RID: 21245
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<GameServerInfo, int, string> customIntValueFormatter;

	// Token: 0x040052FE RID: 21246
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<GameServerInfo, string, string> customStringValueFormatter;

	// Token: 0x02000DEE RID: 3566
	public struct GameOptionValue
	{
		// Token: 0x06006C65 RID: 27749 RVA: 0x002A7F18 File Offset: 0x002A6118
		public GameOptionValue(int _value, string _displayName)
		{
			this.Value = _value;
			this.displayName = _displayName;
		}

		// Token: 0x06006C66 RID: 27750 RVA: 0x002A7F28 File Offset: 0x002A6128
		public override string ToString()
		{
			return this.displayName;
		}

		// Token: 0x040052FF RID: 21247
		public readonly int Value;

		// Token: 0x04005300 RID: 21248
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayName;
	}
}
