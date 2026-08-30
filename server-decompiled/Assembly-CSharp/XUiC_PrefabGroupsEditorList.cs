using System;
using UnityEngine.Scripting;

// Token: 0x02000FCD RID: 4045
[Preserve]
public class XUiC_PrefabGroupsEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007FBC RID: 32700 RVA: 0x003068F3 File Offset: 0x00304AF3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.editorGroups.Contains(_featureName);
	}

	// Token: 0x06007FBD RID: 32701 RVA: 0x00306906 File Offset: 0x00304B06
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.editorGroups.Add(_featureName);
	}

	// Token: 0x06007FBE RID: 32702 RVA: 0x00306919 File Offset: 0x00304B19
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

	// Token: 0x06007FBF RID: 32703 RVA: 0x00306952 File Offset: 0x00304B52
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllGroups(this.groupsResult, this.EditPrefab);
	}
}
