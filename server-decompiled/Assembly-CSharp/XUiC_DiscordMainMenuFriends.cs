using System;
using UnityEngine.Scripting;

// Token: 0x0200034F RID: 847
[Preserve]
public class XUiC_DiscordMainMenuFriends : XUiController
{
	// Token: 0x0600188B RID: 6283 RVA: 0x0008B386 File Offset: 0x00089586
	public override void Init()
	{
		base.Init();
		XUiC_DiscordMainMenuFriends.id = base.WindowGroup.Id;
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x0008B3A0 File Offset: 0x000895A0
	public override void OnOpen()
	{
		base.OnOpen();
		DiscordManager.Instance.PendingActionsUpdate += this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining += this.discordActivityJoining;
		this.discordPendingActionsUpdate(DiscordManager.Instance.GetPendingActionsCount());
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x0008B3EF File Offset: 0x000895EF
	public override void OnClose()
	{
		DiscordManager.Instance.PendingActionsUpdate -= this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining -= this.discordActivityJoining;
		base.OnClose();
	}

	// Token: 0x0600188E RID: 6286 RVA: 0x0008B423 File Offset: 0x00089623
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!DiscordManager.Instance.IsReady)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x0008B454 File Offset: 0x00089654
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordPendingActionsUpdate(int _pendingActionsCount)
	{
		this.hasPendingActions = _pendingActionsCount > 0;
		base.RefreshBindings();
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordActivityJoining()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x0008B466 File Offset: 0x00089666
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "has_pending_actions")
		{
			_value = this.hasPendingActions.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x0008B48C File Offset: 0x0008968C
	public static void ToggleWindow(XUi _xui)
	{
		_xui.playerUI.windowManager.SwitchVisible(XUiC_DiscordMainMenuFriends.id, false);
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x0008B4A4 File Offset: 0x000896A4
	public new static bool IsOpen(XUi _xui)
	{
		return _xui.playerUI.windowManager.IsWindowOpen(XUiC_DiscordMainMenuFriends.id);
	}

	// Token: 0x04000FB9 RID: 4025
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04000FBA RID: 4026
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPendingActions;
}
