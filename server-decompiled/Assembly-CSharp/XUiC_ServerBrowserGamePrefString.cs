using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DF9 RID: 3577
[Preserve]
public class XUiC_ServerBrowserGamePrefString : XUiController, IServerBrowserFilterControl
{
	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x06006CBF RID: 27839 RVA: 0x002A9C87 File Offset: 0x002A7E87
	public GameInfoString GameInfoString
	{
		get
		{
			return this.gameInfoString;
		}
	}

	// Token: 0x06006CC0 RID: 27840 RVA: 0x002A9C90 File Offset: 0x002A7E90
	public override void Init()
	{
		base.Init();
		this.gameInfoString = EnumUtils.Parse<GameInfoString>(this.viewComponent.ID, false);
		this.value = base.GetChildById("value").GetChildByType<XUiC_TextInput>();
		this.value.OnChangeHandler += this.ControlText_OnChangeHandler;
	}

	// Token: 0x06006CC1 RID: 27841 RVA: 0x002A9CE7 File Offset: 0x002A7EE7
	[PublicizedFrom(EAccessModifier.Private)]
	public string getGameInfoName()
	{
		return this.gameInfoString.ToStringCached<GameInfoString>();
	}

	// Token: 0x06006CC2 RID: 27842 RVA: 0x002A9CF4 File Offset: 0x002A7EF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ControlText_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006CC3 RID: 27843 RVA: 0x002A9D07 File Offset: 0x002A7F07
	public void Reset()
	{
		this.value.Text = "";
		Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006CC4 RID: 27844 RVA: 0x002A9D2A File Offset: 0x002A7F2A
	public void SetValue(string _value)
	{
		this.value.Text = _value;
		Action<IServerBrowserFilterControl> onValueChanged = this.OnValueChanged;
		if (onValueChanged == null)
		{
			return;
		}
		onValueChanged(this);
	}

	// Token: 0x06006CC5 RID: 27845 RVA: 0x002A9D49 File Offset: 0x002A7F49
	public string GetValue()
	{
		return this.value.Text;
	}

	// Token: 0x06006CC6 RID: 27846 RVA: 0x002A9D58 File Offset: 0x002A7F58
	public XUiC_ServersList.UiServerFilter GetFilter()
	{
		string text = this.gameInfoString.ToStringCached<GameInfoString>();
		string input = this.value.Text.Trim();
		if (input.Length == 0)
		{
			return new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, null, IServerListInterface.ServerFilter.EServerFilterType.Any, 0, 0, false, null);
		}
		Func<XUiC_ServersList.ListEntry, bool> func = (XUiC_ServersList.ListEntry _entry) => _entry.GameServerInfo.GetValue(this.gameInfoString).ContainsCaseInsensitive(input);
		return new XUiC_ServersList.UiServerFilter(text, XUiC_ServersList.EnumServerLists.Regular, func, IServerListInterface.ServerFilter.EServerFilterType.StringContains, 0, 0, false, input);
	}

	// Token: 0x0400533B RID: 21307
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput value;

	// Token: 0x0400533C RID: 21308
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoString gameInfoString;

	// Token: 0x0400533D RID: 21309
	public Action<IServerBrowserFilterControl> OnValueChanged;
}
