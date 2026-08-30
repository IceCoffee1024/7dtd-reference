using System;
using UnityEngine.Scripting;

// Token: 0x02000FD5 RID: 4053
[Preserve]
public class XUiC_PrefabTagList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007FFF RID: 32767 RVA: 0x00307F30 File Offset: 0x00306130
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.Tags.Test_AllSet(FastTags<TagGroup.Poi>.GetTag(_featureName));
	}

	// Token: 0x06008000 RID: 32768 RVA: 0x00307F56 File Offset: 0x00306156
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.Tags |= FastTags<TagGroup.Poi>.GetTag(_featureName);
	}

	// Token: 0x06008001 RID: 32769 RVA: 0x00307F74 File Offset: 0x00306174
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		FastTags<TagGroup.Poi> tag = FastTags<TagGroup.Poi>.GetTag(_featureName);
		if (this.EditPrefab.Tags.Test_AnySet(tag))
		{
			this.EditPrefab.Tags = this.EditPrefab.Tags.Remove(tag);
		}
		else
		{
			this.EditPrefab.Tags |= tag;
		}
		this.RebuildList(false);
	}

	// Token: 0x06008002 RID: 32770 RVA: 0x00307FDD File Offset: 0x003061DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllTags(this.groupsResult, this.EditPrefab);
	}
}
