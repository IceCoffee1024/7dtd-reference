using System;
using UnityEngine.Scripting;

// Token: 0x02000FF4 RID: 4084
[Preserve]
public class XUiC_PrefabTagList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x0600805D RID: 32861 RVA: 0x0030CA24 File Offset: 0x0030AC24
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.Tags.Test_AllSet(FastTags<TagGroup.Poi>.GetTag(_featureName));
	}

	// Token: 0x0600805E RID: 32862 RVA: 0x0030CA4A File Offset: 0x0030AC4A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.Tags |= FastTags<TagGroup.Poi>.GetTag(_featureName);
	}

	// Token: 0x0600805F RID: 32863 RVA: 0x0030CA68 File Offset: 0x0030AC68
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

	// Token: 0x06008060 RID: 32864 RVA: 0x0030CAD1 File Offset: 0x0030ACD1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllTags(this.groupsResult, this.EditPrefab);
	}
}
