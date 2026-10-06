using System;
using UnityEngine.Scripting;

// Token: 0x02000DFF RID: 3583
[Preserve]
public class XUiC_BugReportSaveSelect : XUiController
{
	// Token: 0x06006D32 RID: 27954 RVA: 0x002AA8B0 File Offset: 0x002A8AB0
	public override void Init()
	{
		base.Init();
		this.List = base.GetChildByType<XUiC_BugReportSavesList>();
	}

	// Token: 0x06006D33 RID: 27955 RVA: 0x002AA8C4 File Offset: 0x002A8AC4
	public override void OnOpen()
	{
		base.OnOpen();
		this.rebuildList();
	}

	// Token: 0x06006D34 RID: 27956 RVA: 0x002AA8D2 File Offset: 0x002A8AD2
	[PublicizedFrom(EAccessModifier.Private)]
	public void rebuildList()
	{
		this.List.RebuildList(SaveInfoProvider.Instance.SaveEntryInfos, false);
	}

	// Token: 0x0400537E RID: 21374
	public XUiC_BugReportSavesList List;
}
