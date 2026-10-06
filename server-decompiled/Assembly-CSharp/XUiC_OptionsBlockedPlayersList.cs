using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DB0 RID: 3504
[Preserve]
public class XUiC_OptionsBlockedPlayersList : XUiC_OptionsDialogBase
{
	// Token: 0x06006A6E RID: 27246 RVA: 0x0029E177 File Offset: 0x0029C377
	public override void Init()
	{
		base.Init();
		XUiC_OptionsBlockedPlayersList.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006A6F RID: 27247 RVA: 0x0029E18F File Offset: 0x0029C38F
	public override void OnClose()
	{
		base.OnClose();
		BlockedPlayerList instance = BlockedPlayerList.Instance;
		if (instance == null)
		{
			return;
		}
		instance.MarkForWrite();
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06006A70 RID: 27248 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x04005180 RID: 20864
	public static string ID = "";

	// Token: 0x04005181 RID: 20865
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BlockedPlayersList blockedPlayersList;
}
