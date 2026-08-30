using System;
using UnityEngine.Scripting;

// Token: 0x0200033A RID: 826
[Preserve]
public class XUiC_DiscordBlockedUsers : XUiController
{
	// Token: 0x060017F0 RID: 6128 RVA: 0x00089438 File Offset: 0x00087638
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

	// Token: 0x04000F7E RID: 3966
	public static string ID = "";
}
