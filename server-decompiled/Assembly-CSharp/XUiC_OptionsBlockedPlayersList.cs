using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DA1 RID: 3489
[Preserve]
public class XUiC_OptionsBlockedPlayersList : XUiC_OptionsDialogBase
{
	// Token: 0x06006A76 RID: 27254 RVA: 0x0029ADE7 File Offset: 0x00298FE7
	public override void Init()
	{
		base.Init();
		XUiC_OptionsBlockedPlayersList.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006A77 RID: 27255 RVA: 0x0029ADFF File Offset: 0x00298FFF
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

	// Token: 0x17000B7C RID: 2940
	// (get) Token: 0x06006A78 RID: 27256 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x04005149 RID: 20809
	public static string ID = "";

	// Token: 0x0400514A RID: 20810
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BlockedPlayersList blockedPlayersList;
}
