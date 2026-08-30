using System;
using UnityEngine.Scripting;

// Token: 0x02000343 RID: 835
[Preserve]
public class XUiC_DiscordInfo : XUiController
{
	// Token: 0x06001820 RID: 6176 RVA: 0x00089E50 File Offset: 0x00088050
	public override void Init()
	{
		base.Init();
		XUiC_DiscordInfo.ID = base.WindowGroup.Id;
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x00089E68 File Offset: 0x00088068
	[XuiBindEvent("OnPress", "btnFullAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnFullAccount_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenLoginWindow();
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x00089E7F File Offset: 0x0008807F
	[XuiBindEvent("OnPress", "btnOk")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x00089E7F File Offset: 0x0008807F
	[XuiBindEvent("OnPress", "btnNotNow")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNotNow_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x00089E87 File Offset: 0x00088087
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

	// Token: 0x06001825 RID: 6181 RVA: 0x00089EBF File Offset: 0x000880BF
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenMainMenu()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x00089EF8 File Offset: 0x000880F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenLoginWindow()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_DiscordLogin.Open(null, true, false, false, true, false);
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x00089F21 File Offset: 0x00088121
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		DiscordManager.Instance.Settings.DiscordFirstTimeInfoShown = true;
		DiscordManager.Instance.Settings.Save();
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06001828 RID: 6184 RVA: 0x00089F54 File Offset: 0x00088154
	[XuiXmlBinding("supports_full_accounts")]
	public bool SupportsFullAccounts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DiscordManager.SupportsFullAccounts;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06001829 RID: 6185 RVA: 0x00089F5B File Offset: 0x0008815B
	[XuiXmlBinding("supports_provisional_accounts")]
	public bool SupportsProvisionalAccounts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x04000F8C RID: 3980
	public static string ID = "";

	// Token: 0x04000F8D RID: 3981
	[XuiBindComponent("btnFullAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnFullAccount;

	// Token: 0x04000F8E RID: 3982
	[XuiBindComponent("btnOk", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOk;

	// Token: 0x04000F8F RID: 3983
	[XuiBindComponent("btnNotNow", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnNotNow;

	// Token: 0x04000F90 RID: 3984
	[XuiBindComponent("btnSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSettings;
}
