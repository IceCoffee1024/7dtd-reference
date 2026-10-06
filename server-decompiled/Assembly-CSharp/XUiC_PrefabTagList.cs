using System;
using UnityEngine.Scripting;

// Token: 0x02000FE4 RID: 4068
[Preserve]
public class XUiC_PrefabTagList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x0600800E RID: 32782 RVA: 0x0030AF08 File Offset: 0x00309108
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.Tags.Test_AllSet(FastTags<TagGroup.Poi>.GetTag(_featureName));
	}

	// Token: 0x0600800F RID: 32783 RVA: 0x0030AF2E File Offset: 0x0030912E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.Tags |= FastTags<TagGroup.Poi>.GetTag(_featureName);
	}

	// Token: 0x06008010 RID: 32784 RVA: 0x0030AF4C File Offset: 0x0030914C
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

	// Token: 0x06008011 RID: 32785 RVA: 0x0030AFB5 File Offset: 0x003091B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllTags(this.groupsResult, this.EditPrefab);
	}
}
