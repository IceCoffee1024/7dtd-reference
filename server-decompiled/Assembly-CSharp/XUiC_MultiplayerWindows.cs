using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000FAB RID: 4011
[Preserve]
public class XUiC_MultiplayerWindows : XUiController
{
	// Token: 0x06007E3F RID: 32319 RVA: 0x002FFEC7 File Offset: 0x002FE0C7
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

	// Token: 0x06007E40 RID: 32320 RVA: 0x002FFEEC File Offset: 0x002FE0EC
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

	// Token: 0x06007E41 RID: 32321 RVA: 0x002FFF70 File Offset: 0x002FE170
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		if ((this.xui.playerUI.playerInput.GUIActions.Cancel.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed) && !this.xui.PopupMenuWindow.ViewComponent.IsVisible)
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, true);
		}
	}
}
