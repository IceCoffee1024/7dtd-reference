using System;
using UnityEngine.Scripting;

// Token: 0x02000FF5 RID: 4085
[Preserve]
public class XUiC_PrefabThemeTagList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008062 RID: 32866 RVA: 0x0030CAEC File Offset: 0x0030ACEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.ThemeTags.Test_AllSet(FastTags<TagGroup.Poi>.GetTag(_featureName));
	}

	// Token: 0x06008063 RID: 32867 RVA: 0x0030CB12 File Offset: 0x0030AD12
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.ThemeTags |= FastTags<TagGroup.Poi>.GetTag(_featureName);
	}

	// Token: 0x06008064 RID: 32868 RVA: 0x0030CB30 File Offset: 0x0030AD30
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

	// Token: 0x06008065 RID: 32869 RVA: 0x0030CB99 File Offset: 0x0030AD99
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllThemeTags(this.groupsResult, this.EditPrefab);
	}
}
