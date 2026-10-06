using System;
using UnityEngine.Scripting;

// Token: 0x02000FDC RID: 4060
[Preserve]
public class XUiC_PrefabGroupsEditorList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007FCB RID: 32715 RVA: 0x003098CB File Offset: 0x00307ACB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.editorGroups.Contains(_featureName);
	}

	// Token: 0x06007FCC RID: 32716 RVA: 0x003098DE File Offset: 0x00307ADE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.editorGroups.Add(_featureName);
	}

	// Token: 0x06007FCD RID: 32717 RVA: 0x003098F1 File Offset: 0x00307AF1
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

	// Token: 0x06007FCE RID: 32718 RVA: 0x0030992A File Offset: 0x00307B2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllGroups(this.groupsResult, this.EditPrefab);
	}
}
