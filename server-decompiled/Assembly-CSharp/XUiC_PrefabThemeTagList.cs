using System;
using UnityEngine.Scripting;

// Token: 0x02000FE5 RID: 4069
[Preserve]
public class XUiC_PrefabThemeTagList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008013 RID: 32787 RVA: 0x0030AFD0 File Offset: 0x003091D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.ThemeTags.Test_AllSet(FastTags<TagGroup.Poi>.GetTag(_featureName));
	}

	// Token: 0x06008014 RID: 32788 RVA: 0x0030AFF6 File Offset: 0x003091F6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.ThemeTags |= FastTags<TagGroup.Poi>.GetTag(_featureName);
	}

	// Token: 0x06008015 RID: 32789 RVA: 0x0030B014 File Offset: 0x00309214
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ToggleFeature(string _featureName)
	{
		FastTags<TagGroup.Poi> tag = FastTags<TagGroup.Poi>.GetTag(_featureName);
		if (this.EditPrefab.ThemeTags.Test_AnySet(tag))
		{
			this.EditPrefab.ThemeTags = this.EditPrefab.ThemeTags.Remove(tag);
		}
		else
		{
			this.EditPrefab.ThemeTags |= tag;
		}
		this.RebuildList(false);
	}

	// Token: 0x06008016 RID: 32790 RVA: 0x0030B07D File Offset: 0x0030927D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllThemeTags(this.groupsResult, this.EditPrefab);
	}
}
