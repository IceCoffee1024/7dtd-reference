using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000EDD RID: 3805
[Preserve]
public class XUiC_ChallengeWindowGroup : XUiController
{
	// Token: 0x06007763 RID: 30563 RVA: 0x002CD6C8 File Offset: 0x002CB8C8
	public override void Init()
	{
		base.Init();
		this.descriptionWindow = base.GetChildByType<XUiC_ChallengeEntryDescriptionWindow>();
	}

	// Token: 0x06007764 RID: 30564 RVA: 0x002CD6DC File Offset: 0x002CB8DC
	public void SetEntry(XUiC_ChallengeEntry je)
	{
		this.descriptionWindow.SetChallenge(je);
	}

	// Token: 0x06007765 RID: 30565 RVA: 0x002CD6EC File Offset: 0x002CB8EC
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

	// Token: 0x06007766 RID: 30566 RVA: 0x002CD758 File Offset: 0x002CB958
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

	// Token: 0x06007767 RID: 30567 RVA: 0x002CD82D File Offset: 0x002CBA2D
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x0400581F RID: 22559
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeEntryDescriptionWindow descriptionWindow;
}
