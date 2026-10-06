using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000EEC RID: 3820
[Preserve]
public class XUiC_ChallengeWindowGroup : XUiController
{
	// Token: 0x0600775E RID: 30558 RVA: 0x002D0AA0 File Offset: 0x002CECA0
	public override void Init()
	{
		base.Init();
		this.descriptionWindow = base.GetChildByType<XUiC_ChallengeEntryDescriptionWindow>();
	}

	// Token: 0x0600775F RID: 30559 RVA: 0x002D0AB4 File Offset: 0x002CECB4
	public void SetEntry(XUiC_ChallengeEntry je)
	{
		this.descriptionWindow.SetChallenge(je);
	}

	// Token: 0x06007760 RID: 30560 RVA: 0x002D0AC4 File Offset: 0x002CECC4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.xui.playerUI.playerInput.GUIActions.Inspect.WasPressed)
		{
			this.descriptionWindow.CompleteCurrentChallenege();
		}
		if (this.xui.playerUI.playerInput.GUIActions.HalfStack.WasPressed)
		{
			this.descriptionWindow.TrackCurrentChallenege();
		}
	}

	// Token: 0x06007761 RID: 30561 RVA: 0x002D0B30 File Offset: 0x002CED30
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.windowManager.Open("windowpaging", false);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSelect", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonWest, "igcoTrack", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonNorth, "igcoComplete", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
		XUiC_WindowSelector childByType = this.xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>();
		if (childByType != null)
		{
			childByType.SetSelected("challenges");
		}
	}

	// Token: 0x06007762 RID: 30562 RVA: 0x002D0C05 File Offset: 0x002CEE05
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x04005857 RID: 22615
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeEntryDescriptionWindow descriptionWindow;
}
