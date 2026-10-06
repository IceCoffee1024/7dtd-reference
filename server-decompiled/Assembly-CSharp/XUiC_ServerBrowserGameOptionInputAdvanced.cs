using System;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DDA RID: 3546
[Preserve]
public class XUiC_ServerBrowserGameOptionInputAdvanced : XUiController
{
	// Token: 0x06006C11 RID: 27665 RVA: 0x002A5A50 File Offset: 0x002A3C50
	public override void Init()
	{
		base.Init();
		this.gameInfoInt = EnumUtils.Parse<GameInfoInt>(this.viewComponent.ID, false);
		this.valueField = base.GetChildById("value").GetChildByType<XUiC_TextInput>();
		this.valueField.OnChangeHandler += this.ControlText_OnChangeHandler;
	}

	// Token: 0x06006C12 RID: 27666 RVA: 0x002A5AA7 File Offset: 0x002A3CA7
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlText_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (!this.parse())
		{
			this.valueField.ActiveTextColor = Color.red;
			return;
		}
		this.valueField.ActiveTextColor = Color.white;
		Action<XUiC_ServerBrowserGameOptionInputAdvanced> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006C13 RID: 27667 RVA: 0x002A5AE4 File Offset: 0x002A3CE4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool parse()
	{
		string text = this.gameInfoInt.ToStringCached<GameInfoInt>();
		string text2 = this.valueField.Text;
		if (text2.Length == 0)
		{
			this.filter = new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, null, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null);
			return true;
		}
		Match match = XUiC_ServerBrowserGameOptionInputAdvanced.rangeMatcher.Match(text2);
		if (match.Success)
		{
			int minVal;
			if (!StringParsers.TryParseSInt32(match.Groups[1].Value, out minVal))
			{
				return false;
			}
			int maxVal;
			if (!StringParsers.TryParseSInt32(match.Groups[2].Value, out maxVal))
			{
				return false;
			}
			this.filter = new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, delegate(XUiC_ServersList.ListEntry _entry)
			{
				int value2 = _entry.GameServerInfo.GetValue(this.gameInfoInt);
				return value2 >= minVal && value2 <= maxVal;
			}, IServerListInterface.ServerFilter.EServerFilterType.IntRange, minVal, maxVal, false, null);
			return true;
		}
		else
		{
			match = XUiC_ServerBrowserGameOptionInputAdvanced.comparisonMatcher.Match(text2);
			if (!match.Success)
			{
				return false;
			}
			string value3 = match.Groups[1].Value;
			int value;
			if (!StringParsers.TryParseSInt32(match.Groups[2].Value, out value))
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			uint num3 = <PrivateImplementationDetails>.ComputeStringHash(value3);
			Func<XUiC_ServersList.ListEntry, bool> func;
			IServerListInterface.ServerFilter.EServerFilterType eserverFilterType;
			if (num3 > 990687777U)
			{
				if (num3 <= 2428715011U)
				{
					if (num3 != 2166136261U)
					{
						if (num3 != 2415188796U)
						{
							if (num3 != 2428715011U)
							{
								goto IL_037C;
							}
							if (!(value3 == "!="))
							{
								goto IL_037C;
							}
							goto IL_035F;
						}
						else if (!(value3 == "=<"))
						{
							goto IL_037C;
						}
					}
					else
					{
						if (value3 == null)
						{
							goto IL_037C;
						}
						if (value3.Length != 0)
						{
							goto IL_037C;
						}
						goto IL_0306;
					}
				}
				else if (num3 != 2431966415U)
				{
					if (num3 != 2448744034U)
					{
						if (num3 != 2499223986U)
						{
							goto IL_037C;
						}
						if (!(value3 == "<="))
						{
							goto IL_037C;
						}
					}
					else
					{
						if (!(value3 == "=>"))
						{
							goto IL_037C;
						}
						goto IL_0323;
					}
				}
				else
				{
					if (!(value3 == "=="))
					{
						goto IL_037C;
					}
					goto IL_0306;
				}
				func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) <= value;
				eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMax;
				num2 = value;
				goto IL_0382;
			}
			if (num3 <= 604802540U)
			{
				if (num3 != 284975636U)
				{
					if (num3 != 604802540U)
					{
						goto IL_037C;
					}
					if (!(value3 == "!"))
					{
						goto IL_037C;
					}
					goto IL_035F;
				}
				else
				{
					if (!(value3 == ">="))
					{
						goto IL_037C;
					}
					goto IL_0323;
				}
			}
			else if (num3 != 940354920U)
			{
				if (num3 != 957132539U)
				{
					if (num3 != 990687777U)
					{
						goto IL_037C;
					}
					if (!(value3 == ">"))
					{
						goto IL_037C;
					}
					func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) > value;
					eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMin;
					num = value + 1;
					goto IL_0382;
				}
				else
				{
					if (!(value3 == "<"))
					{
						goto IL_037C;
					}
					func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) < value;
					eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMax;
					num2 = value - 1;
					goto IL_0382;
				}
			}
			else if (!(value3 == "="))
			{
				goto IL_037C;
			}
			IL_0306:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) == value;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntValue;
			num = value;
			goto IL_0382;
			IL_0323:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) >= value;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMin;
			num = value;
			goto IL_0382;
			IL_035F:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) != value;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntNotValue;
			num = value;
			goto IL_0382;
			IL_037C:
			throw new ArgumentOutOfRangeException();
			IL_0382:
			this.filter = new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, func, eserverFilterType, num, num2, false, null);
			return true;
		}
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x002A5E8D File Offset: 0x002A408D
	public XUiC_ServersList.UiServerFilter GetFilter()
	{
		return this.filter;
	}

	// Token: 0x040052B0 RID: 21168
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput valueField;

	// Token: 0x040052B1 RID: 21169
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x040052B2 RID: 21170
	public Action<XUiC_ServerBrowserGameOptionInputAdvanced> OnValueChanged;

	// Token: 0x040052B3 RID: 21171
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServersList.UiServerFilter filter;

	// Token: 0x040052B4 RID: 21172
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex rangeMatcher = new Regex("^\\s*(\\d+)\\s*-\\s*(\\d+)\\s*$");

	// Token: 0x040052B5 RID: 21173
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex comparisonMatcher = new Regex("^\\s*(|<|<=|=|==|>=|>|!|!=)\\s*(\\d+)\\s*$");
}
