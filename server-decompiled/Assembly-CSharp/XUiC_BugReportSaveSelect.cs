using System;
using UnityEngine.Scripting;

// Token: 0x02000E0A RID: 3594
[Preserve]
public class XUiC_BugReportSaveSelect : XUiController
{
	// Token: 0x06006D57 RID: 27991 RVA: 0x002ABDD8 File Offset: 0x002A9FD8
	public override void Init()
	{
		base.Init();
		this.List = base.GetChildByType<XUiC_BugReportSavesList>();
	}

	// Token: 0x06006D58 RID: 27992 RVA: 0x002ABDEC File Offset: 0x002A9FEC
	public override void OnOpen()
	{
		base.OnOpen();
		this.rebuildList();
	}

	// Token: 0x06006D59 RID: 27993 RVA: 0x002ABDFA File Offset: 0x002A9FFA
	[PublicizedFrom(EAccessModifier.Private)]
	public void rebuildList()
	{
		this.List.RebuildList(SaveInfoProvider.Instance.SaveEntryInfos, false);
	}

	// Token: 0x040053A3 RID: 21411
	public XUiC_BugReportSavesList List;
}
