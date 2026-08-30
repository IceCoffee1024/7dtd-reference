using System;
using InControl;
using UnityEngine.Scripting;

// Token: 0x0200034A RID: 842
[UnityEngine.Scripting.Preserve]
public class XUiC_DiscordMainMenuButton : XUiController
{
	// Token: 0x06001862 RID: 6242 RVA: 0x0008A9D4 File Offset: 0x00088BD4
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

	// Token: 0x06001863 RID: 6243 RVA: 0x0008AA20 File Offset: 0x00088C20
	public override void OnOpen()
	{
		base.OnOpen();
		DiscordManager.Instance.StatusChanged += this.discordStatusChanged;
		DiscordManager.Instance.PendingActionsUpdate += this.discordPendingActionsUpdate;
		this.discordPendingActionsUpdate(DiscordManager.Instance.GetPendingActionsCount());
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x0008AA6F File Offset: 0x00088C6F
	public override void OnClose()
	{
		this.clearEvents();
		base.OnClose();
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x0008AA7D File Offset: 0x00088C7D
	public override void Cleanup()
	{
		this.clearEvents();
		base.Cleanup();
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x0008AA8C File Offset: 0x00088C8C
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

	// Token: 0x06001867 RID: 6247 RVA: 0x0008AB2D File Offset: 0x00088D2D
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

	// Token: 0x06001868 RID: 6248 RVA: 0x0008AB4D File Offset: 0x00088D4D
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearEvents()
	{
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
		DiscordManager.Instance.PendingActionsUpdate -= this.discordPendingActionsUpdate;
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		base.RefreshBindings();
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x0008AB83 File Offset: 0x00088D83
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordPendingActionsUpdate(int _pendingActionsCount)
	{
		this.pendingActions = _pendingActionsCount;
		base.RefreshBindings();
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x0008AB94 File Offset: 0x00088D94
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

	// Token: 0x04000FA3 RID: 4003
	[PublicizedFrom(EAccessModifier.Private)]
	public int pendingActions;
}
