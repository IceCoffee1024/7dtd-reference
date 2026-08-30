using System;
using GUI_2;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200108B RID: 4235
[Preserve]
public class XUiC_TwitchInfoWindowGroup : XUiController
{
	// Token: 0x060086C4 RID: 34500 RVA: 0x00338CB2 File Offset: 0x00336EB2
	public override void Init()
	{
		base.Init();
		this.descriptionWindow = base.GetChildByType<XUiC_TwitchEntryDescriptionWindow>();
		this.entryListWindow = base.GetChildByType<XUiC_TwitchEntryListWindow>();
		this.descriptionWindow.OwnerList = this.entryListWindow;
	}

	// Token: 0x060086C5 RID: 34501 RVA: 0x00338CE3 File Offset: 0x00336EE3
	public void SetEntry(XUiC_TwitchActionEntry ta)
	{
		this.descriptionWindow.SetTwitchAction(ta);
	}

	// Token: 0x060086C6 RID: 34502 RVA: 0x00338CF1 File Offset: 0x00336EF1
	public void SetEntry(XUiC_TwitchVoteInfoEntry tv)
	{
		this.descriptionWindow.SetTwitchVote(tv);
	}

	// Token: 0x060086C7 RID: 34503 RVA: 0x00338CFF File Offset: 0x00336EFF
	public void SetEntry(XUiC_TwitchActionHistoryEntry th)
	{
		this.descriptionWindow.SetTwitchHistory(th);
	}

	// Token: 0x060086C8 RID: 34504 RVA: 0x00338D0D File Offset: 0x00336F0D
	public void ClearEntries()
	{
		this.descriptionWindow.ClearEntries();
	}

	// Token: 0x060086C9 RID: 34505 RVA: 0x002CF437 File Offset: 0x002CD637
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x060086CA RID: 34506 RVA: 0x00338D1C File Offset: 0x00336F1C
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

	// Token: 0x060086CB RID: 34507 RVA: 0x00338D96 File Offset: 0x00336F96
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("twitchwindowpaging");
	}

	// Token: 0x04006454 RID: 25684
	public XUiC_TwitchEntryDescriptionWindow descriptionWindow;

	// Token: 0x04006455 RID: 25685
	public XUiC_TwitchEntryListWindow entryListWindow;
}
