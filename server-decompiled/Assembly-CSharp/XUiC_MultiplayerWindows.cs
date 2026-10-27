using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000FBB RID: 4027
[Preserve]
public class XUiC_MultiplayerWindows : XUiController
{
	// Token: 0x06007E8E RID: 32398 RVA: 0x003019E3 File Offset: 0x002FFBE3
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

	// Token: 0x06007E8F RID: 32399 RVA: 0x00301A08 File Offset: 0x002FFC08
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

	// Token: 0x06007E90 RID: 32400 RVA: 0x00301A8C File Offset: 0x002FFC8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		if ((this.xui.playerUI.playerInput.GUIActions.Cancel.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed) && !this.xui.PopupMenuWindow.ViewComponent.IsVisible)
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, true);
		}
	}
}
