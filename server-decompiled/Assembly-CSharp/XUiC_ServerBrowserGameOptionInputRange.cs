using System;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DCE RID: 3534
[Preserve]
public class XUiC_ServerBrowserGameOptionInputRange : XUiController
{
	// Token: 0x06006C28 RID: 27688 RVA: 0x002A2C2C File Offset: 0x002A0E2C
	public override void Init()
	{
		base.Init();
		this.gameInfoInt = EnumUtils.Parse<GameInfoInt>(this.viewComponent.ID, false);
		this.valuemin = base.GetChildById("valuemin").GetChildByType<XUiC_TextInput>();
		this.valuemin.OnChangeHandler += this.ControlText_OnChangeHandler;
		this.valuemax = base.GetChildById("valuemax").GetChildByType<XUiC_TextInput>();
		this.valuemax.OnChangeHandler += this.ControlText_OnChangeHandler;
	}

	// Token: 0x06006C29 RID: 27689 RVA: 0x002A2CB0 File Offset: 0x002A0EB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlText_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		Action<XUiC_ServerBrowserGameOptionInputRange> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006C2A RID: 27690 RVA: 0x002A2CC4 File Offset: 0x002A0EC4
	public XUiC_ServersList.UiServerFilter GetFilter()
	{
		string text = this.gameInfoInt.ToStringCached<GameInfoInt>();
		bool flag = this.valuemin.Text.Length > 0 && this.valuemin.Text != "-";
		bool flag2 = this.valuemax.Text.Length > 0 && this.valuemax.Text != "-";
		Func<XUiC_ServersList.ListEntry, bool> func = null;
		IServerListInterface.ServerFilter.EServerFilterType eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.Any;
		int filterMin = 0;
		int filterMax = 0;
		if (flag && !flag2)
		{
			filterMin = StringParsers.ParseSInt32(this.valuemin.Text, 0, -1, NumberStyles.Integer);
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) >= filterMin;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMin;
		}
		else if (flag && flag2)
		{
			filterMin = StringParsers.ParseSInt32(this.valuemin.Text, 0, -1, NumberStyles.Integer);
			filterMax = StringParsers.ParseSInt32(this.valuemax.Text, 0, -1, NumberStyles.Integer);
			func = delegate(XUiC_ServersList.ListEntry _entry)
			{
				int value = _entry.GameServerInfo.GetValue(this.gameInfoInt);
				return value >= filterMin && value <= filterMax;
			};
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntRange;
		}
		else if (!flag && flag2)
		{
			filterMax = StringParsers.ParseSInt32(this.valuemax.Text, 0, -1, NumberStyles.Integer);
			func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoInt) <= filterMax;
			eserverFilterType = IServerListInterface.ServerFilter.EServerFilterType.IntMax;
		}
		return new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, func, eserverFilterType, filterMin, filterMax, false, null);
	}

	// Token: 0x04005284 RID: 21124
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput valuemin;

	// Token: 0x04005285 RID: 21125
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput valuemax;

	// Token: 0x04005286 RID: 21126
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x04005287 RID: 21127
	public Action<XUiC_ServerBrowserGameOptionInputRange> OnValueChanged;
}
