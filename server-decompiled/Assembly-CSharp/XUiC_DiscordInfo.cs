using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200034A RID: 842
[Preserve]
public class XUiC_DiscordInfo : XUiController
{
	// Token: 0x06001830 RID: 6192 RVA: 0x0008A778 File Offset: 0x00088978
	public override void Init()
	{
		base.Init();
		XUiC_DiscordInfo.ID = base.WindowGroup.Id;
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x0008A790 File Offset: 0x00088990
	[XuiBindEvent("OnPress", "btnFullAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnFullAccount_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenLoginWindow();
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x0008A7A7 File Offset: 0x000889A7
	[XuiBindEvent("OnPress", "btnOk")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x0008A7A7 File Offset: 0x000889A7
	[XuiBindEvent("OnPress", "btnNotNow")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNotNow_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenMainMenu();
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x0008A7AF File Offset: 0x000889AF
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

	// Token: 0x06001835 RID: 6197 RVA: 0x0008A7E7 File Offset: 0x000889E7
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenMainMenu()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x0008A820 File Offset: 0x00088A20
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenLoginWindow()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_DiscordLogin.Open(null, true, false, false, true, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x0008A84F File Offset: 0x00088A4F
	public override void OnOpen()
	{
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		DiscordManager.Instance.Settings.DiscordFirstTimeInfoShown = true;
		DiscordManager.Instance.Settings.Save();
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06001838 RID: 6200 RVA: 0x0008A882 File Offset: 0x00088A82
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
	// (get) Token: 0x06001839 RID: 6201 RVA: 0x0008A889 File Offset: 0x00088A89
	[XuiXmlBinding("supports_provisional_accounts")]
	public bool SupportsProvisionalAccounts
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x04000F96 RID: 3990
	public static string ID = "";

	// Token: 0x04000F97 RID: 3991
	[XuiBindComponent("btnFullAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnFullAccount;

	// Token: 0x04000F98 RID: 3992
	[XuiBindComponent("btnOk", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOk;

	// Token: 0x04000F99 RID: 3993
	[XuiBindComponent("btnNotNow", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnNotNow;

	// Token: 0x04000F9A RID: 3994
	[XuiBindComponent("btnSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSettings;
}
