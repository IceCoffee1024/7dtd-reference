using System;
using UnityEngine.Scripting;

// Token: 0x02000DF0 RID: 3568
[Preserve]
public class XUiC_BugReportSaveSelect : XUiController
{
	// Token: 0x06006D3A RID: 27962 RVA: 0x002A7520 File Offset: 0x002A5720
	public override void Init()
	{
		base.Init();
		this.List = base.GetChildByType<XUiC_BugReportSavesList>();
	}

	// Token: 0x06006D3B RID: 27963 RVA: 0x002A7534 File Offset: 0x002A5734
	public override void OnOpen()
	{
		base.OnOpen();
		this.rebuildList();
	}

	// Token: 0x06006D3C RID: 27964 RVA: 0x002A7542 File Offset: 0x002A5742
	[PublicizedFrom(EAccessModifier.Private)]
	public void rebuildList()
	{
		this.List.RebuildList(SaveInfoProvider.Instance.SaveEntryInfos, false);
	}

	// Token: 0x04005347 RID: 21319
	public XUiC_BugReportSavesList List;
}
