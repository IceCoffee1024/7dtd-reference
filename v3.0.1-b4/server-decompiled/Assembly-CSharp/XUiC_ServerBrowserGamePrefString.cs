using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DCA RID: 3530
[Preserve]
public class XUiC_ServerBrowserGamePrefString : XUiController, IServerBrowserFilterControl
{
	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x06006C03 RID: 27651 RVA: 0x002A1F3C File Offset: 0x002A013C
	public GameInfoString GameInfoString
	{
		get
		{
			return this.gameInfoString;
		}
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x002A1F44 File Offset: 0x002A0144
	public override void Init()
	{
		base.Init();
		this.gameInfoString = EnumUtils.Parse<GameInfoString>(this.viewComponent.ID, false);
		this.value = base.GetChildById("value").GetChildByType<XUiC_TextInput>();
		this.value.OnChangeHandler += this.ControlText_OnChangeHandler;
	}

	// Token: 0x06006C05 RID: 27653 RVA: 0x002A1F9B File Offset: 0x002A019B
	[PublicizedFrom(EAccessModifier.Private)]
	public string getGameInfoName()
	{
		return this.gameInfoString.ToStringCached<GameInfoString>();
	}

	// Token: 0x06006C06 RID: 27654 RVA: 0x002A1FA8 File Offset: 0x002A01A8
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

	// Token: 0x06006C07 RID: 27655 RVA: 0x002A1FBB File Offset: 0x002A01BB
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

	// Token: 0x06006C08 RID: 27656 RVA: 0x002A1FDE File Offset: 0x002A01DE
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

	// Token: 0x06006C09 RID: 27657 RVA: 0x002A1FFD File Offset: 0x002A01FD
	public string GetValue()
	{
		return this.value.Text;
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x002A200C File Offset: 0x002A020C
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

	// Token: 0x0400523C RID: 21052
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput value;

	// Token: 0x0400523D RID: 21053
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoString gameInfoString;

	// Token: 0x0400523E RID: 21054
	public Action<IServerBrowserFilterControl> OnValueChanged;
}
