using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001777 RID: 6007
	public class PrefabManagerData
	{
		// Token: 0x0600BA83 RID: 47747 RVA: 0x0044D044 File Offset: 0x0044B244
		public void LoadPrefabs()
		{
			if (this.AllPrefabDatas.Count != 0)
			{
				return;
			}
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			List<PathAbstractions.AbstractedLocation> availablePathsList = PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList(null, true, null);
			FastTags<TagGroup.Poi> fastTags = FastTags<TagGroup.Poi>.Parse("navonly,devonly,testonly,biomeonly");
			for (int i = 0; i < availablePathsList.Count; i++)
			{
				PathAbstractions.AbstractedLocation abstractedLocation = availablePathsList[i];
				int num = abstractedLocation.Folder.LastIndexOf("/Prefabs/");
				if (num < 0 || !abstractedLocation.Folder.Substring(num + 8, 5).EqualsCaseInsensitive("/test"))
				{
					PrefabData prefabData = PrefabData.LoadPrefabData(abstractedLocation);
					try
					{
						if (prefabData != null && !prefabData.Tags.Test_AnySet(fastTags) && !prefabData.Tags.IsEmpty)
						{
							this.AllPrefabDatas[abstractedLocation.Name.ToLower()] = prefabData;
						}
					}
					catch (Exception)
					{
						Log.Error("Could not load prefab data for " + abstractedLocation.Name);
					}
				}
			}
			Log.Out("LoadPrefabs {0} of {1} in {2} s", new object[]
			{
				this.AllPrefabDatas.Count,
				availablePathsList.Count,
				(float)microStopwatch.ElapsedMilliseconds * 0.001f
			});
		}

		// Token: 0x0600BA84 RID: 47748 RVA: 0x0044D198 File Offset: 0x0044B398
		public void ShufflePrefabData(int _seed)
		{
			this.prefabDataList.Clear();
			this.AllPrefabDatas.CopyValuesTo<string, PrefabData>(this.prefabDataList);
			this.prefabDataList.Shuffle<PrefabData>(_seed);
		}

		// Token: 0x0600BA85 RID: 47749 RVA: 0x0044D1C2 File Offset: 0x0044B3C2
		public void Cleanup()
		{
			this.AllPrefabDatas.Clear();
			this.prefabDataList.Clear();
		}

		// Token: 0x0600BA86 RID: 47750 RVA: 0x0044D1DC File Offset: 0x0044B3DC
		public Prefab GetPreviewPrefabWithAnyTags(FastTags<TagGroup.Poi> _tags, int _townshipId, Vector2i size = default(Vector2i), bool useAnySizeSmaller = false)
		{
			Vector2i minSize = (useAnySizeSmaller ? Vector2i.zero : size);
			List<PrefabData> list = this.prefabDataList.FindAll((PrefabData _pd) => !_pd.Tags.Test_AnySet(this.PartsAndTilesTags) && PrefabManager.isSizeValid(_pd, minSize, size) && _pd.Tags.Test_AnySet(_tags));
			if (list.Count == 0)
			{
				return null;
			}
			list.Shuffle<PrefabData>(this.previewSeed);
			Prefab prefab = new Prefab();
			prefab.Load(list[0].location, true, true, false, false);
			this.previewSeed++;
			return prefab;
		}

		// Token: 0x04008B3B RID: 35643
		public readonly Dictionary<string, PrefabData> AllPrefabDatas = new Dictionary<string, PrefabData>();

		// Token: 0x04008B3C RID: 35644
		public readonly List<PrefabData> prefabDataList = new List<PrefabData>();

		// Token: 0x04008B3D RID: 35645
		public readonly FastTags<TagGroup.Poi> PartsAndTilesTags = FastTags<TagGroup.Poi>.Parse("streettile,part");

		// Token: 0x04008B3E RID: 35646
		public readonly FastTags<TagGroup.Poi> WildernessTags = FastTags<TagGroup.Poi>.Parse("wilderness");

		// Token: 0x04008B3F RID: 35647
		public readonly FastTags<TagGroup.Poi> TraderTags = FastTags<TagGroup.Poi>.Parse("trader");

		// Token: 0x04008B40 RID: 35648
		public readonly FastTags<TagGroup.Poi> HasTags = FastTags<TagGroup.Poi>.Parse("has");

		// Token: 0x04008B41 RID: 35649
		[PublicizedFrom(EAccessModifier.Private)]
		public int previewSeed;
	}
}
