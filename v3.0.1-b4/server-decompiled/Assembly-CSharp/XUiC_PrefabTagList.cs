using System;
using UnityEngine.Scripting;

// Token: 0x02000FBC RID: 4028
[Preserve]
public class XUiC_PrefabTagList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06007F0D RID: 32525 RVA: 0x0030375C File Offset: 0x0030195C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.Tags.Test_AllSet(FastTags<TagGroup.Poi>.GetTag(_featureName));
	}

	// Token: 0x06007F0E RID: 32526 RVA: 0x00303782 File Offset: 0x00301982
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.Tags |= FastTags<TagGroup.Poi>.GetTag(_featureName);
	}

	// Token: 0x06007F0F RID: 32527 RVA: 0x003037A0 File Offset: 0x003019A0
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

	// Token: 0x06007F10 RID: 32528 RVA: 0x00303809 File Offset: 0x00301A09
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllTags(this.groupsResult, this.EditPrefab);
	}
}
