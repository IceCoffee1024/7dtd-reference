using System;
using UnityEngine.Scripting;

// Token: 0x02000352 RID: 850
[Preserve]
public class XUiC_DiscordMainMenuFriends : XUiController
{
	// Token: 0x06001886 RID: 6278 RVA: 0x0008B606 File Offset: 0x00089806
	public override void Init()
	{
		base.Init();
		XUiC_DiscordMainMenuFriends.id = base.WindowGroup.Id;
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x0008B620 File Offset: 0x00089820
	public override void OnOpen()
	{
		base.OnOpen();
		DiscordManager.Instance.PendingActionsUpdate += this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining += this.discordActivityJoining;
		this.discordPendingActionsUpdate(DiscordManager.Instance.GetPendingActionsCount());
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x0008B66F File Offset: 0x0008986F
	public override void OnClose()
	{
		DiscordManager.Instance.PendingActionsUpdate -= this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining -= this.discordActivityJoining;
		base.OnClose();
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x0008B6A3 File Offset: 0x000898A3
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!DiscordManager.Instance.IsReady)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x0008B6D4 File Offset: 0x000898D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordPendingActionsUpdate(int _pendingActionsCount)
	{
		this.hasPendingActions = _pendingActionsCount > 0;
		base.RefreshBindings();
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x00089715 File Offset: 0x00087915
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordActivityJoining()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x0008B6E6 File Offset: 0x000898E6
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

	// Token: 0x0600188D RID: 6285 RVA: 0x0008B70C File Offset: 0x0008990C
	public static void ToggleWindow(XUi _xui)
	{
		_xui.playerUI.windowManager.SwitchVisible(XUiC_DiscordMainMenuFriends.id, false);
	}

	// Token: 0x0600188E RID: 6286 RVA: 0x0008B724 File Offset: 0x00089924
	public new static bool IsOpen(XUi _xui)
	{
		return _xui.playerUI.windowManager.IsWindowOpen(XUiC_DiscordMainMenuFriends.id);
	}

	// Token: 0x04000FB2 RID: 4018
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04000FB3 RID: 4019
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPendingActions;
}
