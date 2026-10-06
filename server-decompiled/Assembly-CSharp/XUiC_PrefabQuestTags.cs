using System;
using UnityEngine.Scripting;

// Token: 0x02000FE3 RID: 4067
[Preserve]
public class XUiC_PrefabQuestTags : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008009 RID: 32777 RVA: 0x0030AEAF File Offset: 0x003090AF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.GetQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x0600800A RID: 32778 RVA: 0x0030AEC2 File Offset: 0x003090C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x0600800B RID: 32779 RVA: 0x0030AED5 File Offset: 0x003090D5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.GetTag(_featureName));
		this.RebuildList(false);
	}

	// Token: 0x0600800C RID: 32780 RVA: 0x0030AEEF File Offset: 0x003090EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllQuestTags(this.groupsResult, this.EditPrefab);
	}
}
