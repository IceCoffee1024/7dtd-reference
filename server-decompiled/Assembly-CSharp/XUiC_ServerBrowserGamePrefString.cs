using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DDF RID: 3551
[Preserve]
public class XUiC_ServerBrowserGamePrefString : XUiController, IServerBrowserFilterControl
{
	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x06006CA2 RID: 27810 RVA: 0x002A53CF File Offset: 0x002A35CF
	public GameInfoString GameInfoString
	{
		get
		{
			return this.gameInfoString;
		}
	}

	// Token: 0x06006CA3 RID: 27811 RVA: 0x002A53D8 File Offset: 0x002A35D8
	public override void Init()
	{
		base.Init();
		this.gameInfoString = EnumUtils.Parse<GameInfoString>(this.viewComponent.ID, false);
		this.value = base.GetChildById("value").GetChildByType<XUiC_TextInput>();
		this.value.OnChangeHandler += this.ControlText_OnChangeHandler;
	}

	// Token: 0x06006CA4 RID: 27812 RVA: 0x002A542F File Offset: 0x002A362F
	[PublicizedFrom(EAccessModifier.Private)]
	public string getGameInfoName()
	{
		return this.gameInfoString.ToStringCached<GameInfoString>();
	}

	// Token: 0x06006CA5 RID: 27813 RVA: 0x002A543C File Offset: 0x002A363C
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

	// Token: 0x06006CA6 RID: 27814 RVA: 0x002A544F File Offset: 0x002A364F
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

	// Token: 0x06006CA7 RID: 27815 RVA: 0x002A5472 File Offset: 0x002A3672
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

	// Token: 0x06006CA8 RID: 27816 RVA: 0x002A5491 File Offset: 0x002A3691
	public string GetValue()
	{
		return this.value.Text;
	}

	// Token: 0x06006CA9 RID: 27817 RVA: 0x002A54A0 File Offset: 0x002A36A0
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

	// Token: 0x040052DF RID: 21215
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput value;

	// Token: 0x040052E0 RID: 21216
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoString gameInfoString;

	// Token: 0x040052E1 RID: 21217
	public Action<IServerBrowserFilterControl> OnValueChanged;
}
