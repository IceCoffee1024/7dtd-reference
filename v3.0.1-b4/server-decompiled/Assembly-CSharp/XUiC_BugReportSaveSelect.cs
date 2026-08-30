using System;
using UnityEngine.Scripting;

// Token: 0x02000DDB RID: 3547
[Preserve]
public class XUiC_BugReportSaveSelect : XUiController
{
	// Token: 0x06006C9B RID: 27803 RVA: 0x002A408C File Offset: 0x002A228C
	public override void Init()
	{
		base.Init();
		this.List = base.GetChildByType<XUiC_BugReportSavesList>();
	}

	// Token: 0x06006C9C RID: 27804 RVA: 0x002A40A0 File Offset: 0x002A22A0
	public override void OnOpen()
	{
		base.OnOpen();
		this.rebuildList();
	}

	// Token: 0x06006C9D RID: 27805 RVA: 0x002A40AE File Offset: 0x002A22AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void rebuildList()
	{
		this.List.RebuildList(SaveInfoProvider.Instance.SaveEntryInfos, false);
	}

	// Token: 0x040052A4 RID: 21156
	public XUiC_BugReportSavesList List;
}
