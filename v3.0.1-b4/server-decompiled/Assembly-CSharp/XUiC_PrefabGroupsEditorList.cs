using System;
using UnityEngine.Scripting;

// Token: 0x02000FB4 RID: 4020
[Preserve]
public class XUiC_PrefabGroupsEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007ECA RID: 32458 RVA: 0x0030211F File Offset: 0x0030031F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.editorGroups.Contains(_featureName);
	}

	// Token: 0x06007ECB RID: 32459 RVA: 0x00302132 File Offset: 0x00300332
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.editorGroups.Add(_featureName);
	}

	// Token: 0x06007ECC RID: 32460 RVA: 0x00302145 File Offset: 0x00300345
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

	// Token: 0x06007ECD RID: 32461 RVA: 0x0030217E File Offset: 0x0030037E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllGroups(this.groupsResult, this.EditPrefab);
	}
}
