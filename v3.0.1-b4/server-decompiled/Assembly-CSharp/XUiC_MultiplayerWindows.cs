using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F83 RID: 3971
[Preserve]
public class XUiC_MultiplayerWindows : XUiController
{
	// Token: 0x06007D3D RID: 32061 RVA: 0x002F868B File Offset: 0x002F688B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		this.UpdateInput();
	}

	// Token: 0x06007D3E RID: 32062 RVA: 0x002F86B0 File Offset: 0x002F68B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "is_multiplayer")
		{
			_value = (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsSinglePlayer).ToString();
			return true;
		}
		if (_bindingName == "blocked_players_available")
		{
			_value = (BlockedPlayerList.Instance != null).ToString();
			return true;
		}
		if (!(_bindingName == "discord_ready"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = DiscordManager.Instance.IsReady.ToString();
		return true;
	}

	// Token: 0x06007D3F RID: 32063 RVA: 0x002F8734 File Offset: 0x002F6934
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		if ((this.xui.playerUI.playerInput.GUIActions.Cancel.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed) && !this.xui.PopupMenuWindow.ViewComponent.IsVisible)
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, true);
		}
	}
}
