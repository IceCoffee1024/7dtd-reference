using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000347 RID: 839
[Preserve]
public class XUiC_DiscordInfo : XUiController
{
	// Token: 0x06001835 RID: 6197 RVA: 0x0008A4F8 File Offset: 0x000886F8
	public override void Init()
	{
		base.Init();
		XUiC_DiscordInfo.ID = base.WindowGroup.Id;
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x0008A510 File Offset: 0x00088710
	[XuiBindEvent("OnPress", "btnFullAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnFullAccount_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenLoginWindow();
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x0008A527 File Offset: 0x00088727
	[XuiBindEvent("OnPress", "btnOk")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x0008A527 File Offset: 0x00088727
	[XuiBindEvent("OnPress", "btnNotNow")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNotNow_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x0008A52F File Offset: 0x0008872F
	[XuiBindEvent("OnPress", "btnSettings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSettings_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_OptionsAudio childByType = this.xui.GetChildByType<XUiC_OptionsAudio>();
		if (childByType == null)
		{
			return;
		}
		childByType.OpenAtTab("xuiOptionsAudioDiscord");
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x0008A567 File Offset: 0x00088767
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenMainMenu()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x0008A5A0 File Offset: 0x000887A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenLoginWindow()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_DiscordLogin.Open(null, true, false, false, true, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x0008A5CF File Offset: 0x000887CF
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		DiscordManager.Instance.Settings.DiscordFirstTimeInfoShown = true;
		DiscordManager.Instance.Settings.Save();
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x0600183D RID: 6205 RVA: 0x0008A602 File Offset: 0x00088802
	[XuiXmlBinding("supports_full_accounts")]
	public bool SupportsFullAccounts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DiscordManager.SupportsFullAccounts;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x0600183E RID: 6206 RVA: 0x0008A609 File Offset: 0x00088809
	[XuiXmlBinding("supports_provisional_accounts")]
	public bool SupportsProvisionalAccounts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x04000F9D RID: 3997
	public static string ID = "";

	// Token: 0x04000F9E RID: 3998
	[XuiBindComponent("btnFullAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnFullAccount;

	// Token: 0x04000F9F RID: 3999
	[XuiBindComponent("btnOk", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOk;

	// Token: 0x04000FA0 RID: 4000
	[XuiBindComponent("btnNotNow", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnNotNow;

	// Token: 0x04000FA1 RID: 4001
	[XuiBindComponent("btnSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSettings;
}
