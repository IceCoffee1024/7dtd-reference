using System;
using System.Collections.Generic;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001746 RID: 5958
	public class PrefabManagerData
	{
		// Token: 0x0600B96F RID: 47471 RVA: 0x00449698 File Offset: 0x00447898
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

		// Token: 0x0600B970 RID: 47472 RVA: 0x004497EC File Offset: 0x004479EC
		public void ShufflePrefabData(int _seed)
		{
			this.prefabDataList.Clear();
			this.AllPrefabDatas.CopyValuesTo<string, PrefabData>(this.prefabDataList);
			this.prefabDataList.Shuffle<PrefabData>(_seed);
		}

		// Token: 0x0600B971 RID: 47473 RVA: 0x00449816 File Offset: 0x00447A16
		public void Cleanup()
		{
			this.AllPrefabDatas.Clear();
			this.prefabDataList.Clear();
		}

		// Token: 0x0600B972 RID: 47474 RVA: 0x00449830 File Offset: 0x00447A30
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

		// Token: 0x04008A52 RID: 35410
		public readonly Dictionary<string, PrefabData> AllPrefabDatas = new Dictionary<string, PrefabData>();

		// Token: 0x04008A53 RID: 35411
		public readonly List<PrefabData> prefabDataList = new List<PrefabData>();

		// Token: 0x04008A54 RID: 35412
		public readonly FastTags<TagGroup.Poi> PartsAndTilesTags = FastTags<TagGroup.Poi>.Parse("streettile,part");

		// Token: 0x04008A55 RID: 35413
		public readonly FastTags<TagGroup.Poi> WildernessTags = FastTags<TagGroup.Poi>.Parse("wilderness");

		// Token: 0x04008A56 RID: 35414
		public readonly FastTags<TagGroup.Poi> TraderTags = FastTags<TagGroup.Poi>.Parse("trader");

		// Token: 0x04008A57 RID: 35415
		public readonly FastTags<TagGroup.Poi> HasTags = FastTags<TagGroup.Poi>.Parse("has");

		// Token: 0x04008A58 RID: 35416
		[PublicizedFrom(EAccessModifier.Private)]
		public int previewSeed;
	}
}
