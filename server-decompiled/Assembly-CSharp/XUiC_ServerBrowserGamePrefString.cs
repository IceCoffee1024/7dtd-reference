using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DEE RID: 3566
[Preserve]
public class XUiC_ServerBrowserGamePrefString : XUiController, IServerBrowserFilterControl
{
	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x06006C9A RID: 27802 RVA: 0x002A875F File Offset: 0x002A695F
	public GameInfoString GameInfoString
	{
		get
		{
			return this.gameInfoString;
		}
	}

	// Token: 0x06006C9B RID: 27803 RVA: 0x002A8768 File Offset: 0x002A6968
	public override void Init()
	{
		base.Init();
		this.gameInfoString = EnumUtils.Parse<GameInfoString>(this.viewComponent.ID, false);
		this.value = base.GetChildById("value").GetChildByType<XUiC_TextInput>();
		this.value.OnChangeHandler += this.ControlText_OnChangeHandler;
	}

	// Token: 0x06006C9C RID: 27804 RVA: 0x002A87BF File Offset: 0x002A69BF
	[PublicizedFrom(EAccessModifier.Private)]
	public string getGameInfoName()
	{
		return this.gameInfoString.ToStringCached<GameInfoString>();
	}

	// Token: 0x06006C9D RID: 27805 RVA: 0x002A87CC File Offset: 0x002A69CC
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

	// Token: 0x06006C9E RID: 27806 RVA: 0x002A87DF File Offset: 0x002A69DF
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

	// Token: 0x06006C9F RID: 27807 RVA: 0x002A8802 File Offset: 0x002A6A02
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

	// Token: 0x06006CA0 RID: 27808 RVA: 0x002A8821 File Offset: 0x002A6A21
	public string GetValue()
	{
		return this.value.Text;
	}

	// Token: 0x06006CA1 RID: 27809 RVA: 0x002A8830 File Offset: 0x002A6A30
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

	// Token: 0x04005316 RID: 21270
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput value;

	// Token: 0x04005317 RID: 21271
	[PublicizedFrom(EAccessModifier.Private)]
	public GameInfoString gameInfoString;

	// Token: 0x04005318 RID: 21272
	public Action<IServerBrowserFilterControl> OnValueChanged;
}
