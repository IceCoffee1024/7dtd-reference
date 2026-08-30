using System;
using UnityEngine.Scripting;

// Token: 0x02000FDA RID: 4058
[Preserve]
public class XUiC_PrefabZonesEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008013 RID: 32787 RVA: 0x0030822F File Offset: 0x0030642F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.IsAllowedZone(_featureName);
	}

	// Token: 0x06008014 RID: 32788 RVA: 0x0030823D File Offset: 0x0030643D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.AddAllowedZone(_featureName);
	}

	// Token: 0x06008015 RID: 32789 RVA: 0x0030824B File Offset: 0x0030644B
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

	// Token: 0x06008016 RID: 32790 RVA: 0x00308274 File Offset: 0x00306474
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllZones(this.groupsResult, this.EditPrefab);
	}
}
