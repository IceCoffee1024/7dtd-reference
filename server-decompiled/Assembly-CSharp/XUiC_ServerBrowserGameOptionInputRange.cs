using System;
using System.Globalization;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DE8 RID: 3560
[Preserve]
public class XUiC_ServerBrowserGameOptionInputRange : XUiController
{
	// Token: 0x06006C45 RID: 27717 RVA: 0x002A74E4 File Offset: 0x002A56E4
	public override void Init()
	{
		base.Init();
		this.gameInfoInt = EnumUtils.Parse<GameInfoInt>(this.viewComponent.ID, false);
		this.valuemin = base.GetChildById("valuemin").GetChildByType<XUiC_TextInput>();
		this.valuemin.OnChangeHandler += this.ControlText_OnChangeHandler;
		this.valuemax = base.GetChildById("valuemax").GetChildByType<XUiC_TextInput>();
		this.valuemax.OnChangeHandler += this.ControlText_OnChangeHandler;
	}

	// Token: 0x06006C46 RID: 27718 RVA: 0x002A7568 File Offset: 0x002A5768
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

	// Token: 0x06006C47 RID: 27719 RVA: 0x002A757C File Offset: 0x002A577C
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

	// Token: 0x040052E0 RID: 21216
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput valuemin;

	// Token: 0x040052E1 RID: 21217
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput valuemax;

	// Token: 0x040052E2 RID: 21218
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoInt gameInfoInt;

	// Token: 0x040052E3 RID: 21219
	public Action<XUiC_ServerBrowserGameOptionInputRange> OnValueChanged;
}
