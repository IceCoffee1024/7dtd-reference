using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DBB RID: 3515
[Preserve]
public class XUiC_OptionsBlockedPlayersList : XUiC_OptionsDialogBase
{
	// Token: 0x06006A93 RID: 27283 RVA: 0x0029F69F File Offset: 0x0029D89F
	public override void Init()
	{
		base.Init();
		XUiC_OptionsBlockedPlayersList.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006A94 RID: 27284 RVA: 0x0029F6B7 File Offset: 0x0029D8B7
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

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x06006A95 RID: 27285 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x040051A5 RID: 20901
	public static string ID = "";

	// Token: 0x040051A6 RID: 20902
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BlockedPlayersList blockedPlayersList;
}
