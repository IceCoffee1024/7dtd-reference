using System;
using UnityEngine.Scripting;

// Token: 0x02000336 RID: 822
[Preserve]
public class XUiC_DiscordBlockedUsers : XUiController
{
	// Token: 0x060017DB RID: 6107 RVA: 0x00088D98 File Offset: 0x00086F98
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

	// Token: 0x04000F6D RID: 3949
	public static string ID = "";
}
