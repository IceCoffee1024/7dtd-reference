using System;
using UnityEngine.Scripting;

// Token: 0x02000FE9 RID: 4073
[Preserve]
public class XUiC_PrefabZonesEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008022 RID: 32802 RVA: 0x0030B207 File Offset: 0x00309407
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.IsAllowedZone(_featureName);
	}

	// Token: 0x06008023 RID: 32803 RVA: 0x0030B215 File Offset: 0x00309415
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.AddAllowedZone(_featureName);
	}

	// Token: 0x06008024 RID: 32804 RVA: 0x0030B223 File Offset: 0x00309423
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

	// Token: 0x06008025 RID: 32805 RVA: 0x0030B24C File Offset: 0x0030944C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllZones(this.groupsResult, this.EditPrefab);
	}
}
