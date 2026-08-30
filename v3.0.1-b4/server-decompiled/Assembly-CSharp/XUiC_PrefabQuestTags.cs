using System;
using UnityEngine.Scripting;

// Token: 0x02000FBB RID: 4027
[Preserve]
public class XUiC_PrefabQuestTags : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007F08 RID: 32520 RVA: 0x00303703 File Offset: 0x00301903
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.GetQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x06007F09 RID: 32521 RVA: 0x00303716 File Offset: 0x00301916
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x06007F0A RID: 32522 RVA: 0x00303729 File Offset: 0x00301929
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.GetTag(_featureName));
		this.RebuildList(false);
	}

	// Token: 0x06007F0B RID: 32523 RVA: 0x00303743 File Offset: 0x00301943
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllQuestTags(this.groupsResult, this.EditPrefab);
	}
}
