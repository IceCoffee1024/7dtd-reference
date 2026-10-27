using System;
using UnityEngine.Scripting;

// Token: 0x02000354 RID: 852
[Preserve]
public class XUiC_DiscordMainMenuFriends : XUiController
{
	// Token: 0x0600188A RID: 6282 RVA: 0x0008B83A File Offset: 0x00089A3A
	public override void Init()
	{
		base.Init();
		XUiC_DiscordMainMenuFriends.id = base.WindowGroup.Id;
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x0008B854 File Offset: 0x00089A54
	public override void OnOpen()
	{
		base.OnOpen();
		DiscordManager.Instance.PendingActionsUpdate += this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining += this.discordActivityJoining;
		this.discordPendingActionsUpdate(DiscordManager.Instance.GetPendingActionsCount());
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x0008B8A3 File Offset: 0x00089AA3
	public override void OnClose()
	{
		DiscordManager.Instance.PendingActionsUpdate -= this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining -= this.discordActivityJoining;
		base.OnClose();
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x0008B8D7 File Offset: 0x00089AD7
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!DiscordManager.Instance.IsReady)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x0600188E RID: 6286 RVA: 0x0008B908 File Offset: 0x00089B08
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordPendingActionsUpdate(int _pendingActionsCount)
	{
		this.hasPendingActions = _pendingActionsCount > 0;
		base.RefreshBindings();
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordActivityJoining()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x0008B91A File Offset: 0x00089B1A
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

	// Token: 0x06001891 RID: 6289 RVA: 0x0008B940 File Offset: 0x00089B40
	public static void ToggleWindow(XUi _xui)
	{
		_xui.playerUI.windowManager.SwitchVisible(XUiC_DiscordMainMenuFriends.id, false);
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x0008B958 File Offset: 0x00089B58
	public new static bool IsOpen(XUi _xui)
	{
		return _xui.playerUI.windowManager.IsWindowOpen(XUiC_DiscordMainMenuFriends.id);
	}

	// Token: 0x04000FC2 RID: 4034
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04000FC3 RID: 4035
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPendingActions;
}
