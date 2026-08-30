using System;
using UnityEngine.Scripting;

// Token: 0x0200034B RID: 843
[Preserve]
public class XUiC_DiscordMainMenuFriends : XUiController
{
	// Token: 0x0600186E RID: 6254 RVA: 0x0008AC46 File Offset: 0x00088E46
	public override void Init()
	{
		base.Init();
		XUiC_DiscordMainMenuFriends.id = base.WindowGroup.Id;
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x0008AC60 File Offset: 0x00088E60
	public override void OnOpen()
	{
		base.OnOpen();
		DiscordManager.Instance.PendingActionsUpdate += this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining += this.discordActivityJoining;
		this.discordPendingActionsUpdate(DiscordManager.Instance.GetPendingActionsCount());
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0008ACAF File Offset: 0x00088EAF
	public override void OnClose()
	{
		DiscordManager.Instance.PendingActionsUpdate -= this.discordPendingActionsUpdate;
		DiscordManager.Instance.ActivityJoining -= this.discordActivityJoining;
		base.OnClose();
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x0008ACE3 File Offset: 0x00088EE3
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!DiscordManager.Instance.IsReady)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x0008AD14 File Offset: 0x00088F14
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordPendingActionsUpdate(int _pendingActionsCount)
	{
		this.hasPendingActions = _pendingActionsCount > 0;
		base.RefreshBindings();
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordActivityJoining()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x0008AD26 File Offset: 0x00088F26
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

	// Token: 0x06001875 RID: 6261 RVA: 0x0008AD4C File Offset: 0x00088F4C
	public static void ToggleWindow(XUi _xui)
	{
		_xui.playerUI.windowManager.SwitchVisible(XUiC_DiscordMainMenuFriends.id, false);
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x0008AD64 File Offset: 0x00088F64
	public new static bool IsOpen(XUi _xui)
	{
		return _xui.playerUI.windowManager.IsWindowOpen(XUiC_DiscordMainMenuFriends.id);
	}

	// Token: 0x04000FA4 RID: 4004
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id = "";

	// Token: 0x04000FA5 RID: 4005
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPendingActions;
}
