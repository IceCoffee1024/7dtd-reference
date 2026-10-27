using System;
using UnityEngine.Scripting;

// Token: 0x02000FEC RID: 4076
[Preserve]
public class XUiC_PrefabGroupsEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x0600801A RID: 32794 RVA: 0x0030B3E7 File Offset: 0x003095E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.editorGroups.Contains(_featureName);
	}

	// Token: 0x0600801B RID: 32795 RVA: 0x0030B3FA File Offset: 0x003095FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.editorGroups.Add(_featureName);
	}

	// Token: 0x0600801C RID: 32796 RVA: 0x0030B40D File Offset: 0x0030960D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		if (this.EditPrefab.editorGroups.Contains(_featureName))
		{
			this.EditPrefab.editorGroups.Remove(_featureName);
			return;
		}
		this.EditPrefab.editorGroups.Add(_featureName);
	}

	// Token: 0x0600801D RID: 32797 RVA: 0x0030B446 File Offset: 0x00309646
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllGroups(this.groupsResult, this.EditPrefab);
	}
}
