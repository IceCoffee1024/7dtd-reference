using System;
using GUI_2;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B5 RID: 4277
[Preserve]
public class XUiC_TwitchInfoWindowGroup : XUiController
{
	// Token: 0x06008805 RID: 34821 RVA: 0x003412F2 File Offset: 0x0033F4F2
	public override void Init()
	{
		base.Init();
		this.descriptionWindow = base.GetChildByType<XUiC_TwitchEntryDescriptionWindow>();
		this.entryListWindow = base.GetChildByType<XUiC_TwitchEntryListWindow>();
		this.descriptionWindow.OwnerList = this.entryListWindow;
	}

	// Token: 0x06008806 RID: 34822 RVA: 0x00341323 File Offset: 0x0033F523
	public void SetEntry(XUiC_TwitchActionEntry ta)
	{
		this.descriptionWindow.SetTwitchAction(ta);
	}

	// Token: 0x06008807 RID: 34823 RVA: 0x00341331 File Offset: 0x0033F531
	public void SetEntry(XUiC_TwitchVoteInfoEntry tv)
	{
		this.descriptionWindow.SetTwitchVote(tv);
	}

	// Token: 0x06008808 RID: 34824 RVA: 0x0034133F File Offset: 0x0033F53F
	public void SetEntry(XUiC_TwitchActionHistoryEntry th)
	{
		this.descriptionWindow.SetTwitchHistory(th);
	}

	// Token: 0x06008809 RID: 34825 RVA: 0x0034134D File Offset: 0x0033F54D
	public void ClearEntries()
	{
		this.descriptionWindow.ClearEntries();
	}

	// Token: 0x0600880A RID: 34826 RVA: 0x002D5E2F File Offset: 0x002D402F
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x0600880B RID: 34827 RVA: 0x0034135C File Offset: 0x0033F55C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSelect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
		base.GetChildByType<XUiC_WindowNonPagingHeader>();
		TwitchManager.Current.HasViewedSettings = true;
	}

	// Token: 0x0600880C RID: 34828 RVA: 0x003413D6 File Offset: 0x0033F5D6
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("twitchwindowpaging");
	}

	// Token: 0x0400655D RID: 25949
	public XUiC_TwitchEntryDescriptionWindow descriptionWindow;

	// Token: 0x0400655E RID: 25950
	public XUiC_TwitchEntryListWindow entryListWindow;
}
