using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x02000919 RID: 2329
public class PrefabData
{
	// Token: 0x060043A1 RID: 17313 RVA: 0x001A6690 File Offset: 0x001A4890
	public PrefabData(PathAbstractions.AbstractedLocation _location)
	{
		this.POIMarkers = new PrefabMarkerVolumeList(null);
		this.location = _location;
		this.Name = _location.Name.ToLower();
	}

	// Token: 0x060043A2 RID: 17314 RVA: 0x001A66E0 File Offset: 0x001A48E0
	public bool Init(DynamicProperties properties)
	{
		this.size = new Vector3i(-1, -1, -1);
		properties.ParseVec("PrefabSize", ref this.size);
		if (this.size.x < 0 || this.size.y < 0 || this.size.z < 0)
		{
			Log.Error("PrefabData {0} PrefabSize error", new object[] { this.Name });
			return false;
		}
		this.POIMarkers.ReadFromProperties(properties);
		this.rotationToFaceNorth = (byte)properties.GetInt("RotationToFaceNorth");
		if (properties.Values.ContainsKey("Tags"))
		{
			this.Tags = FastTags<TagGroup.Poi>.Parse(properties.Values["Tags"].Replace(" ", ""));
		}
		if (properties.Values.ContainsKey("ThemeTags"))
		{
			this.ThemeTags = FastTags<TagGroup.Poi>.Parse(properties.Values["ThemeTags"].Replace(" ", ""));
		}
		properties.ParseInt("ThemeRepeatDistance", ref this.ThemeRepeatDistance);
		properties.ParseInt("DuplicateRepeatDistance", ref this.DuplicateRepeatDistance);
		if (properties.Classes.ContainsKey("Stats"))
		{
			WorldStats worldStats = WorldStats.FromProperties(properties.Classes["Stats"]);
			if (worldStats != null)
			{
				this.DensityScore = (float)((worldStats.TotalVertices + 50000) / 100000);
			}
		}
		this.yOffset = properties.GetInt("YOffset");
		properties.ParseInt("DifficultyTier", ref this.DifficultyTier);
		return true;
	}

	// Token: 0x060043A3 RID: 17315 RVA: 0x001A6870 File Offset: 0x001A4A70
	public List<Marker> RotatePOIMarkers(bool _bLeft, int _rotCount)
	{
		Vector3i vector3i = this.size;
		List<Marker> list = new List<Marker>(this.POIMarkers.Count);
		for (int i = 0; i < this.POIMarkers.Count; i++)
		{
			list.Add(this.POIMarkers[i].CloneGeneric());
		}
		for (int j = 0; j < _rotCount; j++)
		{
			for (int k = 0; k < list.Count; k++)
			{
				list[k].RotateY(_bLeft, vector3i);
			}
			MathUtils.Swap(ref vector3i.x, ref vector3i.z);
		}
		return list;
	}

	// Token: 0x060043A4 RID: 17316 RVA: 0x001A6908 File Offset: 0x001A4B08
	public static PrefabData LoadPrefabData(PathAbstractions.AbstractedLocation _location)
	{
		if (!SdFile.Exists(_location.FullPathNoExtension + ".xml"))
		{
			return null;
		}
		DynamicProperties dynamicProperties = new DynamicProperties();
		if (!dynamicProperties.Load(_location.Folder, _location.Name))
		{
			return null;
		}
		PrefabData prefabData = new PrefabData(_location);
		if (!prefabData.Init(dynamicProperties))
		{
			return null;
		}
		return prefabData;
	}

	// Token: 0x04003663 RID: 13923
	public const int ThemeRepeatDistanceDefault = 300;

	// Token: 0x04003664 RID: 13924
	public const int DuplicateRepeatDistanceDefault = 1000;

	// Token: 0x04003665 RID: 13925
	public Vector3i size;

	// Token: 0x04003666 RID: 13926
	public readonly string Name;

	// Token: 0x04003667 RID: 13927
	public FastTags<TagGroup.Poi> Tags;

	// Token: 0x04003668 RID: 13928
	public FastTags<TagGroup.Poi> ThemeTags;

	// Token: 0x04003669 RID: 13929
	public int ThemeRepeatDistance = 300;

	// Token: 0x0400366A RID: 13930
	public int DuplicateRepeatDistance = 1000;

	// Token: 0x0400366B RID: 13931
	public byte rotationToFaceNorth;

	// Token: 0x0400366C RID: 13932
	public float DensityScore;

	// Token: 0x0400366D RID: 13933
	public int DifficultyTier;

	// Token: 0x0400366E RID: 13934
	public int yOffset;

	// Token: 0x0400366F RID: 13935
	public PathAbstractions.AbstractedLocation location;

	// Token: 0x04003670 RID: 13936
	public readonly PrefabMarkerVolumeList POIMarkers;
}
