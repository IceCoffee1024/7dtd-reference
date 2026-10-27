using System;
using UnityEngine.Scripting;

// Token: 0x02000FF9 RID: 4089
[Preserve]
public class XUiC_PrefabZonesEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008071 RID: 32881 RVA: 0x0030CD23 File Offset: 0x0030AF23
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.IsAllowedZone(_featureName);
	}

	// Token: 0x06008072 RID: 32882 RVA: 0x0030CD31 File Offset: 0x0030AF31
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.AddAllowedZone(_featureName);
	}

	// Token: 0x06008073 RID: 32883 RVA: 0x0030CD3F File Offset: 0x0030AF3F
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

	// Token: 0x06008074 RID: 32884 RVA: 0x0030CD68 File Offset: 0x0030AF68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllZones(this.groupsResult, this.EditPrefab);
	}
}
