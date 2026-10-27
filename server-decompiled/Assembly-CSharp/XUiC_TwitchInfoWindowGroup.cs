using System;
using GUI_2;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010C5 RID: 4293
[Preserve]
public class XUiC_TwitchInfoWindowGroup : XUiController
{
	// Token: 0x06008858 RID: 34904 RVA: 0x00343376 File Offset: 0x00341576
	public override void Init()
	{
		base.Init();
		this.descriptionWindow = base.GetChildByType<XUiC_TwitchEntryDescriptionWindow>();
		this.entryListWindow = base.GetChildByType<XUiC_TwitchEntryListWindow>();
		this.descriptionWindow.OwnerList = this.entryListWindow;
	}

	// Token: 0x06008859 RID: 34905 RVA: 0x003433A7 File Offset: 0x003415A7
	public void SetEntry(XUiC_TwitchActionEntry ta)
	{
		this.descriptionWindow.SetTwitchAction(ta);
	}

	// Token: 0x0600885A RID: 34906 RVA: 0x003433B5 File Offset: 0x003415B5
	public void SetEntry(XUiC_TwitchVoteInfoEntry tv)
	{
		this.descriptionWindow.SetTwitchVote(tv);
	}

	// Token: 0x0600885B RID: 34907 RVA: 0x003433C3 File Offset: 0x003415C3
	public void SetEntry(XUiC_TwitchActionHistoryEntry th)
	{
		this.descriptionWindow.SetTwitchHistory(th);
	}

	// Token: 0x0600885C RID: 34908 RVA: 0x003433D1 File Offset: 0x003415D1
	public void ClearEntries()
	{
		this.descriptionWindow.ClearEntries();
	}

	// Token: 0x0600885D RID: 34909 RVA: 0x002D7387 File Offset: 0x002D5587
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x0600885E RID: 34910 RVA: 0x003433E0 File Offset: 0x003415E0
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

	// Token: 0x0600885F RID: 34911 RVA: 0x0034345A File Offset: 0x0034165A
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("twitchwindowpaging");
	}

	// Token: 0x0400659C RID: 26012
	public XUiC_TwitchEntryDescriptionWindow descriptionWindow;

	// Token: 0x0400659D RID: 26013
	public XUiC_TwitchEntryListWindow entryListWindow;
}
