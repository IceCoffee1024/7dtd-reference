using System;
using InControl;
using UnityEngine.Scripting;

// Token: 0x02000353 RID: 851
[UnityEngine.Scripting.Preserve]
public class XUiC_DiscordMainMenuButton : XUiController
{
	// Token: 0x0600187E RID: 6270 RVA: 0x0008B5C8 File Offset: 0x000897C8
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

	// Token: 0x0600187F RID: 6271 RVA: 0x0008B614 File Offset: 0x00089814
	public override void OnOpen()
	{
		base.OnOpen();
		DiscordManager.Instance.StatusChanged += this.discordStatusChanged;
		DiscordManager.Instance.PendingActionsUpdate += this.discordPendingActionsUpdate;
		this.discordPendingActionsUpdate(DiscordManager.Instance.GetPendingActionsCount());
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x0008B663 File Offset: 0x00089863
	public override void OnClose()
	{
		this.clearEvents();
		base.OnClose();
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x0008B671 File Offset: 0x00089871
	public override void Cleanup()
	{
		this.clearEvents();
		base.Cleanup();
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x0008B680 File Offset: 0x00089880
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

	// Token: 0x06001883 RID: 6275 RVA: 0x0008B721 File Offset: 0x00089921
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

	// Token: 0x06001884 RID: 6276 RVA: 0x0008B741 File Offset: 0x00089941
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearEvents()
	{
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
		DiscordManager.Instance.PendingActionsUpdate -= this.discordPendingActionsUpdate;
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		base.RefreshBindings();
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x0008B777 File Offset: 0x00089977
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordPendingActionsUpdate(int _pendingActionsCount)
	{
		this.pendingActions = _pendingActionsCount;
		base.RefreshBindings();
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x0008B788 File Offset: 0x00089988
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

	// Token: 0x04000FC1 RID: 4033
	[PublicizedFrom(EAccessModifier.Private)]
	public int pendingActions;
}
