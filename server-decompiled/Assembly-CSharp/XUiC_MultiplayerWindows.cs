using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F9C RID: 3996
[Preserve]
public class XUiC_MultiplayerWindows : XUiController
{
	// Token: 0x06007E2F RID: 32303 RVA: 0x002FCE5F File Offset: 0x002FB05F
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

	// Token: 0x06007E30 RID: 32304 RVA: 0x002FCE84 File Offset: 0x002FB084
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

	// Token: 0x06007E31 RID: 32305 RVA: 0x002FCF08 File Offset: 0x002FB108
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		if ((this.xui.playerUI.playerInput.GUIActions.Cancel.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed) && !this.xui.PopupMenuWindow.ViewComponent.IsVisible)
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, true);
		}
	}
}
