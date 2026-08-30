using System;
using UnityEngine.Scripting;

// Token: 0x02000FC1 RID: 4033
[Preserve]
public class XUiC_PrefabZonesEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007F21 RID: 32545 RVA: 0x00303A5B File Offset: 0x00301C5B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.IsAllowedZone(_featureName);
	}

	// Token: 0x06007F22 RID: 32546 RVA: 0x00303A69 File Offset: 0x00301C69
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.AddAllowedZone(_featureName);
	}

	// Token: 0x06007F23 RID: 32547 RVA: 0x00303A77 File Offset: 0x00301C77
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		if (this.EditPrefab.IsAllowedZone(_featureName))
		{
			this.EditPrefab.RemoveAllowedZone(_featureName);
			return;
		}
		this.EditPrefab.AddAllowedZone(_featureName);
	}

	// Token: 0x06007F24 RID: 32548 RVA: 0x00303AA0 File Offset: 0x00301CA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllZones(this.groupsResult, this.EditPrefab);
	}
}
