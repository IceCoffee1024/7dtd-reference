using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02000EC8 RID: 3784
[Preserve]
public class XUiC_ChallengeWindowGroup : XUiController
{
	// Token: 0x060076C2 RID: 30402 RVA: 0x002CA1A4 File Offset: 0x002C83A4
	public override void Init()
	{
		base.Init();
		this.descriptionWindow = base.GetChildByType<XUiC_ChallengeEntryDescriptionWindow>();
	}

	// Token: 0x060076C3 RID: 30403 RVA: 0x002CA1B8 File Offset: 0x002C83B8
	public void SetEntry(XUiC_ChallengeEntry je)
	{
		this.descriptionWindow.SetChallenge(je);
	}

	// Token: 0x060076C4 RID: 30404 RVA: 0x002CA1C8 File Offset: 0x002C83C8
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

	// Token: 0x060076C5 RID: 30405 RVA: 0x002CA234 File Offset: 0x002C8434
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

	// Token: 0x060076C6 RID: 30406 RVA: 0x002CA309 File Offset: 0x002C8509
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.windowManager.Close("windowpaging");
	}

	// Token: 0x0400577A RID: 22394
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeEntryDescriptionWindow descriptionWindow;
}
