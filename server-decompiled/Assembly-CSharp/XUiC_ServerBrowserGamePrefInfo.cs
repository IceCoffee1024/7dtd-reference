using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000DE2 RID: 3554
[Preserve]
public class XUiC_ServerBrowserGamePrefInfo : XUiController
{
	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x06006C33 RID: 27699 RVA: 0x002A654F File Offset: 0x002A474F
	public GamePrefs.EnumType ValueType
	{
		get
		{
			return this.valueType;
		}
	}

	// Token: 0x17000BFC RID: 3068
	// (get) Token: 0x06006C34 RID: 27700 RVA: 0x002A6557 File Offset: 0x002A4757
	public GameInfoBool GameInfoBool
	{
		get
		{
			return this.gameInfoBool;
		}
	}

	// Token: 0x17000BFD RID: 3069
	// (get) Token: 0x06006C35 RID: 27701 RVA: 0x002A655F File Offset: 0x002A475F
	public GameInfoInt GameInfoInt
	{
		get
		{
			return this.gameInfoInt;
		}
	}

	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x06006C36 RID: 27702 RVA: 0x002A6567 File Offset: 0x002A4767
	public GameInfoString GameInfoString
	{
		get
		{
			return this.gameInfoString;
		}
	}

	// Token: 0x17000BFF RID: 3071
	// (set) Token: 0x06006C37 RID: 27703 RVA: 0x002A656F File Offset: 0x002A476F
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

	// Token: 0x17000C00 RID: 3072
	// (set) Token: 0x06006C38 RID: 27704 RVA: 0x002A658D File Offset: 0x002A478D
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

	// Token: 0x17000C01 RID: 3073
	// (set) Token: 0x06006C39 RID: 27705 RVA: 0x002A65AC File Offset: 0x002A47AC
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

	// Token: 0x06006C3A RID: 27706 RVA: 0x002A65CC File Offset: 0x002A47CC
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

	// Token: 0x06006C3B RID: 27707 RVA: 0x002A667C File Offset: 0x002A487C
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

	// Token: 0x06006C3C RID: 27708 RVA: 0x002A66CA File Offset: 0x002A48CA
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006C3D RID: 27709 RVA: 0x002A66E4 File Offset: 0x002A48E4
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

	// Token: 0x06006C3E RID: 27710 RVA: 0x002A67B0 File Offset: 0x002A49B0
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

	// Token: 0x040052CF RID: 21199
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label label;

	// Token: 0x040052D0 RID: 21200
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x040052D1 RID: 21201
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x040052D2 RID: 21202
	[PublicizedFrom(EAccessModifier.Private)]
	public GamePrefs.EnumType valueType;

	// Token: 0x040052D3 RID: 21203
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoBool gameInfoBool;

	// Token: 0x040052D4 RID: 21204
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x040052D5 RID: 21205
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoString gameInfoString;

	// Token: 0x040052D6 RID: 21206
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ServerBrowserGamePrefInfo.GameOptionValue> values;

	// Token: 0x040052D7 RID: 21207
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x040052D8 RID: 21208
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<GameServerInfo, int, string> customIntValueFormatter;

	// Token: 0x040052D9 RID: 21209
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<GameServerInfo, string, string> customStringValueFormatter;

	// Token: 0x02000DE3 RID: 3555
	public struct GameOptionValue
	{
		// Token: 0x06006C40 RID: 27712 RVA: 0x002A69F0 File Offset: 0x002A4BF0
		public GameOptionValue(int _value, string _displayName)
		{
			this.Value = _value;
			this.displayName = _displayName;
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x002A6A00 File Offset: 0x002A4C00
		public override string ToString()
		{
			return this.displayName;
		}

		// Token: 0x040052DA RID: 21210
		public readonly int Value;

		// Token: 0x040052DB RID: 21211
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayName;
	}
}
