using System;
using GUI_2;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A4 RID: 4260
[Preserve]
public class XUiC_TwitchInfoWindowGroup : XUiController
{
	// Token: 0x060087B6 RID: 34742 RVA: 0x0033D4DA File Offset: 0x0033B6DA
	public override void Init()
	{
		base.Init();
		this.descriptionWindow = base.GetChildByType<XUiC_TwitchEntryDescriptionWindow>();
		this.entryListWindow = base.GetChildByType<XUiC_TwitchEntryListWindow>();
		this.descriptionWindow.OwnerList = this.entryListWindow;
	}

	// Token: 0x060087B7 RID: 34743 RVA: 0x0033D50B File Offset: 0x0033B70B
	public void SetEntry(XUiC_TwitchActionEntry ta)
	{
		this.descriptionWindow.SetTwitchAction(ta);
	}

	// Token: 0x060087B8 RID: 34744 RVA: 0x0033D519 File Offset: 0x0033B719
	public void SetEntry(XUiC_TwitchVoteInfoEntry tv)
	{
		this.descriptionWindow.SetTwitchVote(tv);
	}

	// Token: 0x060087B9 RID: 34745 RVA: 0x0033D527 File Offset: 0x0033B727
	public void SetEntry(XUiC_TwitchActionHistoryEntry th)
	{
		this.descriptionWindow.SetTwitchHistory(th);
	}

	// Token: 0x060087BA RID: 34746 RVA: 0x0033D535 File Offset: 0x0033B735
	public void ClearEntries()
	{
		this.descriptionWindow.ClearEntries();
	}

	// Token: 0x060087BB RID: 34747 RVA: 0x002D2ABB File Offset: 0x002D0CBB
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x060087BC RID: 34748 RVA: 0x0033D544 File Offset: 0x0033B744
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

	// Token: 0x060087BD RID: 34749 RVA: 0x0033D5BE File Offset: 0x0033B7BE
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("twitchwindowpaging");
	}

	// Token: 0x0400651B RID: 25883
	public XUiC_TwitchEntryDescriptionWindow descriptionWindow;

	// Token: 0x0400651C RID: 25884
	public XUiC_TwitchEntryListWindow entryListWindow;
}
