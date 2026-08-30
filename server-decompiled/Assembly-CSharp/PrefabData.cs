using System;
using System.Collections.Generic;
using PrefabVolumes;

// Token: 0x02000929 RID: 2345
public class PrefabData
{
	// Token: 0x06004422 RID: 17442 RVA: 0x001A8C64 File Offset: 0x001A6E64
	public PrefabData(PathAbstractions.AbstractedLocation _location)
	{
		this.POIMarkers = new PrefabMarkerVolumeList(null);
		this.location = _location;
		this.Name = _location.Name.ToLower();
	}

	// Token: 0x06004423 RID: 17443 RVA: 0x001A8CB4 File Offset: 0x001A6EB4
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

	// Token: 0x06004424 RID: 17444 RVA: 0x001A8E44 File Offset: 0x001A7044
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

	// Token: 0x06004425 RID: 17445 RVA: 0x001A8EDC File Offset: 0x001A70DC
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

	// Token: 0x040036EC RID: 14060
	public const int ThemeRepeatDistanceDefault = 300;

	// Token: 0x040036ED RID: 14061
	public const int DuplicateRepeatDistanceDefault = 1000;

	// Token: 0x040036EE RID: 14062
	public Vector3i size;

	// Token: 0x040036EF RID: 14063
	public readonly string Name;

	// Token: 0x040036F0 RID: 14064
	public FastTags<TagGroup.Poi> Tags;

	// Token: 0x040036F1 RID: 14065
	public FastTags<TagGroup.Poi> ThemeTags;

	// Token: 0x040036F2 RID: 14066
	public int ThemeRepeatDistance = 300;

	// Token: 0x040036F3 RID: 14067
	public int DuplicateRepeatDistance = 1000;

	// Token: 0x040036F4 RID: 14068
	public byte rotationToFaceNorth;

	// Token: 0x040036F5 RID: 14069
	public float DensityScore;

	// Token: 0x040036F6 RID: 14070
	public int DifficultyTier;

	// Token: 0x040036F7 RID: 14071
	public int yOffset;

	// Token: 0x040036F8 RID: 14072
	public PathAbstractions.AbstractedLocation location;

	// Token: 0x040036F9 RID: 14073
	public readonly PrefabMarkerVolumeList POIMarkers;
}
