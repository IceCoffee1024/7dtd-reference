using System;
using UnityEngine.Scripting;

// Token: 0x0200033D RID: 829
[Preserve]
public class XUiC_DiscordBlockedUsers : XUiController
{
	// Token: 0x060017EB RID: 6123 RVA: 0x000896B8 File Offset: 0x000878B8
	public override void Init()
	{
		base.Init();
		XUiC_DiscordBlockedUsers.ID = base.WindowGroup.Id;
		XUiC_Button xuiC_Button = base.GetChildById("btnBack") as XUiC_Button;
		if (xuiC_Button != null)
		{
			xuiC_Button.OnPress += delegate(XUiController _, int _)
			{
				this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			};
		}
	}

	// Token: 0x04000F77 RID: 3959
	public static string ID = "";
}
