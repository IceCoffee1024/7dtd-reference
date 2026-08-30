using System;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DC1 RID: 3521
[Preserve]
public class XUiC_ServerBrowserGamePrefSelector : XUiController
{
	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x06006BAF RID: 27567 RVA: 0x002A04F8 File Offset: 0x0029E6F8
	public GamePrefs.EnumType ValueType
	{
		get
		{
			return this.valueType;
		}
	}

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x06006BB0 RID: 27568 RVA: 0x002A0500 File Offset: 0x0029E700
	public GameInfoBool GameInfoBool
	{
		get
		{
			return this.gameInfoBool;
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x06006BB1 RID: 27569 RVA: 0x002A0508 File Offset: 0x0029E708
	public GameInfoInt GameInfoInt
	{
		get
		{
			return this.gameInfoInt;
		}
	}

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x06006BB2 RID: 27570 RVA: 0x002A0510 File Offset: 0x0029E710
	// (set) Token: 0x06006BB3 RID: 27571 RVA: 0x002A0518 File Offset: 0x0029E718
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
				this.setupOptions();
			}
		}
	}

	// Token: 0x06006BB4 RID: 27572 RVA: 0x002A0530 File Offset: 0x0029E730
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
		this.controlCombo = base.GetChildById("ControlCombo").GetChildByType<XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelector.GameOptionValue>>();
		this.controlCombo.OnValueChanged += this.ControlCombo_OnValueChanged;
		this.setupOptions();
	}

	// Token: 0x06006BB5 RID: 27573 RVA: 0x002A05C8 File Offset: 0x0029E7C8
	[XuiXmlAttribute("values", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValues(string _value)
	{
		if (_value.Length <= 0)
		{
			return;
		}
		string[] array = _value.Split(',', StringSplitOptions.None);
		this.valuesFromXml = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			this.valuesFromXml[i] = StringParsers.ParseSInt32(array[i], 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x06006BB6 RID: 27574 RVA: 0x002A0618 File Offset: 0x0029E818
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

	// Token: 0x06006BB7 RID: 27575 RVA: 0x002A0666 File Offset: 0x0029E866
	[XuiXmlAttribute("value_localization_prefix", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueLocalizationPrefix(string _value)
	{
		if (_value.Length > 0)
		{
			this.valueLocalizationPrefixFromXml = _value.Trim();
		}
	}

	// Token: 0x06006BB8 RID: 27576 RVA: 0x002A0680 File Offset: 0x0029E880
	[PublicizedFrom(EAccessModifier.Private)]
	public string getGameInfoName()
	{
		GamePrefs.EnumType enumType = this.valueType;
		string text;
		if (enumType != GamePrefs.EnumType.Int)
		{
			if (enumType == GamePrefs.EnumType.Bool)
			{
				text = this.gameInfoBool.ToStringCached<GameInfoBool>();
			}
			else
			{
				text = null;
			}
		}
		else
		{
			text = this.gameInfoInt.ToStringCached<GameInfoInt>();
		}
		return text;
	}

	// Token: 0x06006BB9 RID: 27577 RVA: 0x002A06BC File Offset: 0x0029E8BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupOptions()
	{
		string[] array = null;
		if (this.valueType == GamePrefs.EnumType.Int)
		{
			if (this.valuesFromXml == null)
			{
				if (this.namesFromXml == null)
				{
					throw new Exception("Illegal option setup for " + this.getGameInfoName() + " (no values and no names specified)");
				}
				this.valuesFromXml = new int[this.namesFromXml.Length];
				for (int i = 0; i < this.valuesFromXml.Length; i++)
				{
					this.valuesFromXml[i] = i;
				}
			}
			array = new string[this.valuesFromXml.Length];
			if (this.namesFromXml == null || this.namesFromXml.Length != this.valuesFromXml.Length)
			{
				for (int j = 0; j < this.valuesFromXml.Length; j++)
				{
					if (this.namesFromXml != null && j < this.namesFromXml.Length)
					{
						array[j] = Localization.Get(this.namesFromXml[j], false, null);
					}
					else
					{
						int num = this.valuesFromXml[j];
						if (this.valuePreDisplayModifierFunc != null)
						{
							num = this.valuePreDisplayModifierFunc(num);
						}
						array[j] = string.Format(Localization.Get(this.valueLocalizationPrefixFromXml + ((num == 1) ? "" : "s"), false, null), num);
					}
				}
			}
			else
			{
				for (int k = 0; k < this.namesFromXml.Length; k++)
				{
					array[k] = Localization.Get(this.namesFromXml[k], false, null);
				}
			}
		}
		else if (this.valueType == GamePrefs.EnumType.Bool)
		{
			this.valuesFromXml = new int[] { 0, 1 };
			array = new string[]
			{
				Localization.Get("goOff", false, null),
				Localization.Get("goOn", false, null)
			};
		}
		this.controlCombo.Elements.Clear();
		XUiC_ServerBrowserGamePrefSelector.GameOptionValue gameOptionValue = new XUiC_ServerBrowserGamePrefSelector.GameOptionValue(int.MinValue, Localization.Get("goAnyValue", false, null));
		this.controlCombo.Elements.Add(gameOptionValue);
		if (this.valuesFromXml == null)
		{
			throw new Exception("Illegal option setup for " + this.getGameInfoName() + " (values still null)");
		}
		if (array == null)
		{
			throw new Exception("Illegal option setup for " + this.getGameInfoName() + " (names null)");
		}
		for (int l = 0; l < this.valuesFromXml.Length; l++)
		{
			gameOptionValue = new XUiC_ServerBrowserGamePrefSelector.GameOptionValue(this.valuesFromXml[l], array[l]);
			this.controlCombo.Elements.Add(gameOptionValue);
		}
	}

	// Token: 0x06006BBA RID: 27578 RVA: 0x002A0910 File Offset: 0x0029EB10
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlCombo_OnValueChanged(XUiController _sender, XUiC_ServerBrowserGamePrefSelector.GameOptionValue _oldValue, XUiC_ServerBrowserGamePrefSelector.GameOptionValue _newValue)
	{
		Action<XUiC_ServerBrowserGamePrefSelector> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006BBB RID: 27579 RVA: 0x002A0924 File Offset: 0x0029EB24
	public void SetCurrentValue(object _value)
	{
		try
		{
			if (this.valueType == GamePrefs.EnumType.Int)
			{
				int num = (int)_value;
				bool flag = false;
				for (int i = 1; i < this.controlCombo.Elements.Count; i++)
				{
					if (this.controlCombo.Elements[i].Value == num)
					{
						this.controlCombo.SelectedIndex = i;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.controlCombo.SelectedIndex = this.controlCombo.MinIndex;
				}
			}
			else if (this.valueType == GamePrefs.EnumType.Bool)
			{
				this.controlCombo.SelectedIndex = (((bool)_value) ? 2 : 1);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
	}

	// Token: 0x06006BBC RID: 27580 RVA: 0x002A09DC File Offset: 0x0029EBDC
	public XUiC_ServersList.UiServerFilter GetFilter()
	{
		int value = this.controlCombo.Value.Value;
		bool flag = value == int.MinValue;
		GamePrefs.EnumType enumType = this.valueType;
		string text;
		if (enumType != GamePrefs.EnumType.Int)
		{
			if (enumType != GamePrefs.EnumType.Bool)
			{
				throw new ArgumentOutOfRangeException();
			}
			text = this.gameInfoBool.ToStringCached<GameInfoBool>();
		}
		else
		{
			text = this.gameInfoInt.ToStringCached<GameInfoInt>();
		}
		string text2 = text;
		if (flag)
		{
			return new XUiC_ServersList.UiServerFilter(text2, XUiC_ServersList.EnumServerLists.Regular, null, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null);
		}
		int num = 0;
		bool flag2 = false;
		enumType = this.valueType;
		Func<XUiC_ServersList.ListEntry, bool> func;
		IServerListInterface.ServerFilter.EServerFilterType eserverFilterType;
		if (enumType != GamePrefs.EnumType.Int)
		{
			if (enumType != GamePrefs.EnumType.Bool)
			{
				throw new ArgumentOutOfRangeException();
			}
			bool bValue = value == 1;
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoBool) == bValue;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.BoolValue;
			flag2 = bValue;
		}
		else
		{
			int iValue = value;
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) == iValue;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntValue;
			num = iValue;
		}
		return new XUiC_ServersList.UiServerFilter(text2, XUiC_ServersList.EnumServerLists.Regular, func, eserverFilterType, num, 0, flag2, null);
	}

	// Token: 0x04005203 RID: 20995
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_ServerBrowserGamePrefSelector.GameOptionValue> controlCombo;

	// Token: 0x04005204 RID: 20996
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] valuesFromXml;

	// Token: 0x04005205 RID: 20997
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] namesFromXml;

	// Token: 0x04005206 RID: 20998
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueLocalizationPrefixFromXml;

	// Token: 0x04005207 RID: 20999
	[PublicizedFrom(EAccessModifier.Private)]
	public GamePrefs.EnumType valueType;

	// Token: 0x04005208 RID: 21000
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoBool gameInfoBool;

	// Token: 0x04005209 RID: 21001
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x0400520A RID: 21002
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<int, int> valuePreDisplayModifierFunc;

	// Token: 0x0400520B RID: 21003
	public Action<XUiC_ServerBrowserGamePrefSelector> OnValueChanged;

	// Token: 0x02000DC2 RID: 3522
	public struct GameOptionValue
	{
		// Token: 0x06006BBE RID: 27582 RVA: 0x002A0ACE File Offset: 0x0029ECCE
		public GameOptionValue(int _value, string _displayName)
		{
			this.Value = _value;
			this.displayName = _displayName;
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x002A0ADE File Offset: 0x0029ECDE
		public override string ToString()
		{
			return this.displayName;
		}

		// Token: 0x0400520C RID: 21004
		public readonly int Value;

		// Token: 0x0400520D RID: 21005
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string displayName;
	}
}
