using System;
using UnityEngine.Scripting;

// Token: 0x02000FF3 RID: 4083
[Preserve]
public class XUiC_PrefabQuestTags : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008058 RID: 32856 RVA: 0x0030C9CB File Offset: 0x0030ABCB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.GetQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x06008059 RID: 32857 RVA: 0x0030C9DE File Offset: 0x0030ABDE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x0600805A RID: 32858 RVA: 0x0030C9F1 File Offset: 0x0030ABF1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.GetTag(_featureName));
		this.RebuildList(false);
	}

	// Token: 0x0600805B RID: 32859 RVA: 0x0030CA0B File Offset: 0x0030AC0B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllQuestTags(this.groupsResult, this.EditPrefab);
	}
}
