using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000D8D RID: 3469
[Preserve]
public class XUiC_OptionsBlockedPlayersList : XUiC_OptionsDialogBase
{
	// Token: 0x060069DA RID: 27098 RVA: 0x00297C93 File Offset: 0x00295E93
	public override void Init()
	{
		base.Init();
		XUiC_OptionsBlockedPlayersList.ID = base.WindowGroup.Id;
	}

	// Token: 0x060069DB RID: 27099 RVA: 0x00297CAB File Offset: 0x00295EAB
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

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x060069DC RID: 27100 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x040050A7 RID: 20647
	public static string ID = "";

	// Token: 0x040050A8 RID: 20648
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BlockedPlayersList blockedPlayersList;
}
