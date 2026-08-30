using System;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DBC RID: 3516
[Preserve]
public class XUiC_ServerBrowserGameOptionInputSimple : XUiController
{
	// Token: 0x06006B95 RID: 27541 RVA: 0x0029FD1C File Offset: 0x0029DF1C
	public override void Init()
	{
		base.Init();
		this.gameInfoInt = EnumUtils.Parse<GameInfoInt>(this.viewComponent.ID, false);
		this.value = base.GetChildById("value").GetChildByType<XUiC_TextInput>();
		this.value.OnChangeHandler += this.ControlText_OnChangeHandler;
		XUiController childById = base.GetChildById("comparison");
		childById.OnPress += this.ComparisonLabel_OnPress;
		this.comparisonLabel = (XUiV_Label)childById.ViewComponent;
	}

	// Token: 0x06006B96 RID: 27542 RVA: 0x0029FDA4 File Offset: 0x0029DFA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComparisonLabel_OnPress(XUiController _sender, int _mouseButton)
	{
		this.currentComparison = this.currentComparison.CycleEnum(XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Smaller, XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Larger, false, true);
		XUiV_Label xuiV_Label = this.comparisonLabel;
		string text;
		switch (this.currentComparison)
		{
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Smaller:
			text = "<";
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.SmallerEquals:
			text = "<=";
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Equals:
			text = "=";
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.LargerEquals:
			text = ">=";
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Larger:
			text = ">";
			break;
		default:
			text = this.comparisonLabel.Text;
			break;
		}
		xuiV_Label.Text = text;
		Action<XUiC_ServerBrowserGameOptionInputSimple> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006B97 RID: 27543 RVA: 0x0029FE3C File Offset: 0x0029E03C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlText_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		Action<XUiC_ServerBrowserGameOptionInputSimple> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006B98 RID: 27544 RVA: 0x0029FE50 File Offset: 0x0029E050
	public XUiC_ServersList.UiServerFilter GetFilter()
	{
		string text = this.gameInfoInt.ToStringCached<GameInfoInt>();
		if (this.value.Text.Length == 0 || this.value.Text == "-")
		{
			return new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, null, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null);
		}
		int filterVal = StringParsers.ParseSInt32(this.value.Text, 0, -1, NumberStyles.Integer);
		int num = 0;
		int num2 = 0;
		Func<XUiC_ServersList.ListEntry, bool> func;
		IServerListInterface.ServerFilter.EServerFilterType eserverFilterType;
		switch (this.currentComparison)
		{
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Smaller:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) < filterVal;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMax;
			num2 = filterVal - 1;
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.SmallerEquals:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) <= filterVal;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMax;
			num2 = filterVal;
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Equals:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) == filterVal;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntValue;
			num = filterVal;
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.LargerEquals:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) >= filterVal;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMin;
			num = filterVal;
			break;
		case XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Larger:
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) > filterVal;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMin;
			num = filterVal + 1;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, func, eserverFilterType, num, num2, false, null);
	}

	// Token: 0x040051E9 RID: 20969
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label comparisonLabel;

	// Token: 0x040051EA RID: 20970
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput value;

	// Token: 0x040051EB RID: 20971
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x040051EC RID: 20972
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGameOptionInputSimple.EComparisonType currentComparison = XUiC_ServerBrowserGameOptionInputSimple.EComparisonType.Equals;

	// Token: 0x040051ED RID: 20973
	public Action<XUiC_ServerBrowserGameOptionInputSimple> OnValueChanged;

	// Token: 0x02000DBD RID: 3517
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EComparisonType
	{
		// Token: 0x040051EF RID: 20975
		Smaller,
		// Token: 0x040051F0 RID: 20976
		SmallerEquals,
		// Token: 0x040051F1 RID: 20977
		Equals,
		// Token: 0x040051F2 RID: 20978
		LargerEquals,
		// Token: 0x040051F3 RID: 20979
		Larger
	}
}
