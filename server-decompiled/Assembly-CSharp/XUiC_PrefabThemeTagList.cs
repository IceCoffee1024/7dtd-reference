using System;
using UnityEngine.Scripting;

// Token: 0x02000FD6 RID: 4054
[Preserve]
public class XUiC_PrefabThemeTagList : XUiC_PrefabFeatureEditorList
{
	// Token: 0x06008004 RID: 32772 RVA: 0x00307FF8 File Offset: 0x003061F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool FeatureEnabled(string _featureName)
	{
		return this.EditPrefab.ThemeTags.Test_AllSet(FastTags<TagGroup.Poi>.GetTag(_featureName));
	}

	// Token: 0x06008005 RID: 32773 RVA: 0x0030801E File Offset: 0x0030621E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AddNewFeature(string _featureName)
	{
		this.EditPrefab.ThemeTags |= FastTags<TagGroup.Poi>.GetTag(_featureName);
	}

	// Token: 0x06008006 RID: 32774 RVA: 0x0030803C File Offset: 0x0030623C
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

	// Token: 0x06008007 RID: 32775 RVA: 0x003080A5 File Offset: 0x003062A5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetSupportedFeatures()
	{
		PrefabEditModeManager.Instance.GetAllThemeTags(this.groupsResult, this.EditPrefab);
	}
}
