using System;
using InControl;
using UnityEngine.Scripting;

// Token: 0x02000351 RID: 849
[UnityEngine.Scripting.Preserve]
public class XUiC_DiscordMainMenuButton : XUiController
{
	// Token: 0x0600187A RID: 6266 RVA: 0x0008B394 File Offset: 0x00089594
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("button");
		XUiV_Button xuiV_Button = ((childById != null) ? childById.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button != null)
		{
			xuiV_Button.Controller.OnPress += delegate(XUiController _, int _)
			{
				this.toggleWindow();
			};
		}
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x0008B3E0 File Offset: 0x000895E0
	public override void OnOpen()
	{
		base.OnOpen();
		DiscordManager.Instance.StatusChanged += this.discordStatusChanged;
		DiscordManager.Instance.PendingActionsUpdate += this.discordPendingActionsUpdate;
		this.discordPendingActionsUpdate(DiscordManager.Instance.GetPendingActionsCount());
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x0008B42F File Offset: 0x0008962F
	public override void OnClose()
	{
		this.clearEvents();
		base.OnClose();
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x0008B43D File Offset: 0x0008963D
	public override void Cleanup()
	{
		this.clearEvents();
		base.Cleanup();
	}

	// Token: 0x0600187E RID: 6270 RVA: 0x0008B44C File Offset: 0x0008964C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		XUi xui = this.xui;
		bool? flag;
		if (xui == null)
		{
			flag = null;
		}
		else
		{
			LocalPlayerUI playerUI = xui.playerUI;
			if (playerUI == null)
			{
				flag = null;
			}
			else
			{
				PlayerActionsLocal playerInput = playerUI.playerInput;
				if (playerInput == null)
				{
					flag = null;
				}
				else
				{
					PlayerActionsGUI guiactions = playerInput.GUIActions;
					if (guiactions == null)
					{
						flag = null;
					}
					else
					{
						PlayerAction inspect = guiactions.Inspect;
						flag = ((inspect != null) ? new bool?(inspect.WasReleased) : null);
					}
				}
			}
		}
		bool? flag2 = flag;
		if (flag2.GetValueOrDefault() && XUiUtils.HotkeysAllowedFor(this.viewComponent, true))
		{
			this.toggleWindow();
		}
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x0008B4ED File Offset: 0x000896ED
	[PublicizedFrom(EAccessModifier.Private)]
	public void toggleWindow()
	{
		if (!DiscordManager.Instance.IsReady)
		{
			return;
		}
		XUiC_DiscordMainMenuFriends.ToggleWindow(this.xui);
		base.RefreshBindings();
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x0008B50D File Offset: 0x0008970D
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearEvents()
	{
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
		DiscordManager.Instance.PendingActionsUpdate -= this.discordPendingActionsUpdate;
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		base.RefreshBindings();
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x0008B543 File Offset: 0x00089743
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordPendingActionsUpdate(int _pendingActionsCount)
	{
		this.pendingActions = _pendingActionsCount;
		base.RefreshBindings();
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x0008B554 File Offset: 0x00089754
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "discord_ready")
		{
			_value = DiscordManager.Instance.IsReady.ToString();
			return true;
		}
		if (_bindingName == "discord_open")
		{
			_value = XUiC_DiscordMainMenuFriends.IsOpen(this.xui).ToString();
			return true;
		}
		if (_bindingName == "pending_actions")
		{
			_value = this.pendingActions.ToString();
			return true;
		}
		if (!(_bindingName == "discord_name"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
		_value = ((localUser != null) ? localUser.DiscordDisplayName : null) ?? "";
		return true;
	}

	// Token: 0x04000FB1 RID: 4017
	[PublicizedFrom(EAccessModifier.Private)]
	public int pendingActions;
}
