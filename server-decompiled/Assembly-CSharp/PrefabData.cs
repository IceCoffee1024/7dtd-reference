using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x0200092E RID: 2350
public class PrefabData
{
	// Token: 0x060043B4 RID: 17332 RVA: 0x001A8D68 File Offset: 0x001A6F68
	public PrefabData(PathAbstractions.AbstractedLocation _location)
	{
		this.POIMarkers = new PrefabMarkerVolumeList(null);
		this.location = _location;
		this.Name = _location.Name.ToLower();
	}

	// Token: 0x060043B5 RID: 17333 RVA: 0x001A8DB8 File Offset: 0x001A6FB8
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

	// Token: 0x060043B6 RID: 17334 RVA: 0x001A8F48 File Offset: 0x001A7148
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

	// Token: 0x060043B7 RID: 17335 RVA: 0x001A8FE0 File Offset: 0x001A71E0
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

	// Token: 0x040036E0 RID: 14048
	public const int ThemeRepeatDistanceDefault = 300;

	// Token: 0x040036E1 RID: 14049
	public const int DuplicateRepeatDistanceDefault = 1000;

	// Token: 0x040036E2 RID: 14050
	public Vector3i size;

	// Token: 0x040036E3 RID: 14051
	public readonly string Name;

	// Token: 0x040036E4 RID: 14052
	public FastTags<TagGroup.Poi> Tags;

	// Token: 0x040036E5 RID: 14053
	public FastTags<TagGroup.Poi> ThemeTags;

	// Token: 0x040036E6 RID: 14054
	public int ThemeRepeatDistance = 300;

	// Token: 0x040036E7 RID: 14055
	public int DuplicateRepeatDistance = 1000;

	// Token: 0x040036E8 RID: 14056
	public byte rotationToFaceNorth;

	// Token: 0x040036E9 RID: 14057
	public float DensityScore;

	// Token: 0x040036EA RID: 14058
	public int DifficultyTier;

	// Token: 0x040036EB RID: 14059
	public int yOffset;

	// Token: 0x040036EC RID: 14060
	public PathAbstractions.AbstractedLocation location;

	// Token: 0x040036ED RID: 14061
	public readonly PrefabMarkerVolumeList POIMarkers;
}
