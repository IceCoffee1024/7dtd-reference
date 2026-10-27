using System;
using UnityEngine.Scripting;

// Token: 0x0200033F RID: 831
[Preserve]
public class XUiC_DiscordBlockedUsers : XUiController
{
	// Token: 0x060017EF RID: 6127 RVA: 0x000898EC File Offset: 0x00087AEC
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

	// Token: 0x04000F87 RID: 3975
	public static string ID = "";
}
