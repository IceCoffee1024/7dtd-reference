using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x02000938 RID: 2360
public class PrefabData
{
	// Token: 0x060043D7 RID: 17367 RVA: 0x001A9DEC File Offset: 0x001A7FEC
	public PrefabData(PathAbstractions.AbstractedLocation _location)
	{
		this.POIMarkers = new PrefabMarkerVolumeList(null);
		this.location = _location;
		this.Name = _location.Name.ToLower();
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x001A9E3C File Offset: 0x001A803C
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

	// Token: 0x060043D9 RID: 17369 RVA: 0x001A9FCC File Offset: 0x001A81CC
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

	// Token: 0x060043DA RID: 17370 RVA: 0x001AA064 File Offset: 0x001A8264
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

	// Token: 0x04003700 RID: 14080
	public const int ThemeRepeatDistanceDefault = 300;

	// Token: 0x04003701 RID: 14081
	public const int DuplicateRepeatDistanceDefault = 1000;

	// Token: 0x04003702 RID: 14082
	public Vector3i size;

	// Token: 0x04003703 RID: 14083
	public readonly string Name;

	// Token: 0x04003704 RID: 14084
	public FastTags<TagGroup.Poi> Tags;

	// Token: 0x04003705 RID: 14085
	public FastTags<TagGroup.Poi> ThemeTags;

	// Token: 0x04003706 RID: 14086
	public int ThemeRepeatDistance = 300;

	// Token: 0x04003707 RID: 14087
	public int DuplicateRepeatDistance = 1000;

	// Token: 0x04003708 RID: 14088
	public byte rotationToFaceNorth;

	// Token: 0x04003709 RID: 14089
	public float DensityScore;

	// Token: 0x0400370A RID: 14090
	public int DifficultyTier;

	// Token: 0x0400370B RID: 14091
	public int yOffset;

	// Token: 0x0400370C RID: 14092
	public PathAbstractions.AbstractedLocation location;

	// Token: 0x0400370D RID: 14093
	public readonly PrefabMarkerVolumeList POIMarkers;
}
