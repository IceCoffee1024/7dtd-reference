using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200034C RID: 844
[Preserve]
public class XUiC_DiscordInfo : XUiController
{
	// Token: 0x06001834 RID: 6196 RVA: 0x0008A9AC File Offset: 0x00088BAC
	public override void Init()
	{
		base.Init();
		XUiC_DiscordInfo.ID = base.WindowGroup.Id;
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x0008A9C4 File Offset: 0x00088BC4
	[XuiBindEvent("OnPress", "btnFullAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnFullAccount_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenLoginWindow();
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x0008A9DB File Offset: 0x00088BDB
	[XuiBindEvent("OnPress", "btnOk")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x0008A9DB File Offset: 0x00088BDB
	[XuiBindEvent("OnPress", "btnNotNow")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNotNow_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x0008A9E3 File Offset: 0x00088BE3
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

	// Token: 0x06001839 RID: 6201 RVA: 0x0008AA1B File Offset: 0x00088C1B
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenMainMenu()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x0008AA54 File Offset: 0x00088C54
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenLoginWindow()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_DiscordLogin.Open(null, true, false, false, true, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x0008AA83 File Offset: 0x00088C83
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		DiscordManager.Instance.Settings.DiscordFirstTimeInfoShown = true;
		DiscordManager.Instance.Settings.Save();
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x0600183C RID: 6204 RVA: 0x0008AAB6 File Offset: 0x00088CB6
	[XuiXmlBinding("supports_full_accounts")]
	public bool SupportsFullAccounts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DiscordManager.SupportsFullAccounts;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x0600183D RID: 6205 RVA: 0x0008AABD File Offset: 0x00088CBD
	[XuiXmlBinding("supports_provisional_accounts")]
	public bool SupportsProvisionalAccounts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x04000FA6 RID: 4006
	public static string ID = "";

	// Token: 0x04000FA7 RID: 4007
	[XuiBindComponent("btnFullAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnFullAccount;

	// Token: 0x04000FA8 RID: 4008
	[XuiBindComponent("btnOk", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOk;

	// Token: 0x04000FA9 RID: 4009
	[XuiBindComponent("btnNotNow", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnNotNow;

	// Token: 0x04000FAA RID: 4010
	[XuiBindComponent("btnSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSettings;
}
