using System;
using UnityEngine.Scripting;

// Token: 0x02000FD4 RID: 4052
[Preserve]
public class XUiC_PrefabQuestTags : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007FFA RID: 32762 RVA: 0x00307ED7 File Offset: 0x003060D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.GetQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x06007FFB RID: 32763 RVA: 0x00307EEA File Offset: 0x003060EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.Parse(_featureName));
	}

	// Token: 0x06007FFC RID: 32764 RVA: 0x00307EFD File Offset: 0x003060FD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		this.EditPrefab.ToggleQuestTag(FastTags<TagGroup.Global>.GetTag(_featureName));
		this.RebuildList(false);
	}

	// Token: 0x06007FFD RID: 32765 RVA: 0x00307F17 File Offset: 0x00306117
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllQuestTags(this.groupsResult, this.EditPrefab);
	}
}
