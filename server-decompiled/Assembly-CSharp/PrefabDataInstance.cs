using System;
using UnityEngine;

// Token: 0x0200092A RID: 2346
public class PrefabDataInstance
{
	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06004426 RID: 17446 RVA: 0x001A8F32 File Offset: 0x001A7132
	public Vector3i boundingBoxSize
	{
		get
		{
			return this.prefab.size;
		}
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x06004427 RID: 17447 RVA: 0x001A8F40 File Offset: 0x001A7140
	public Vector2i CenterXZ
	{
		get
		{
			Vector2i vector2i;
			vector2i.x = this.boundingBoxPosition.x + this.prefab.size.x / 2;
			vector2i.y = this.boundingBoxPosition.z + this.prefab.size.z / 2;
			return vector2i;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06004428 RID: 17448 RVA: 0x001A8F98 File Offset: 0x001A7198
	public Vector2 CenterXZV2
	{
		get
		{
			Vector2 vector;
			vector.x = (float)this.boundingBoxPosition.x + (float)this.prefab.size.x * 0.5f;
			vector.y = (float)this.boundingBoxPosition.z + (float)this.prefab.size.z * 0.5f;
			return vector;
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06004429 RID: 17449 RVA: 0x001A8FFC File Offset: 0x001A71FC
	public PathAbstractions.AbstractedLocation location
	{
		get
		{
			return this.prefab.location;
		}
	}

	// Token: 0x0600442A RID: 17450 RVA: 0x001A9009 File Offset: 0x001A7209
	public PrefabDataInstance(int _id, Vector3i _position, byte _rotation, PrefabData _prefabData)
	{
		this.id = _id;
		this.prefab = _prefabData;
		this.boundingBoxPosition = _position;
		this.rotation = _rotation;
		this.previewColor = PrefabDataInstance.previewColorDefault;
	}

	// Token: 0x040036FA RID: 14074
	public int id;

	// Token: 0x040036FB RID: 14075
	public PrefabData prefab;

	// Token: 0x040036FC RID: 14076
	public Vector3i boundingBoxPosition;

	// Token: 0x040036FD RID: 14077
	public byte rotation;

	// Token: 0x040036FE RID: 14078
	public Color32 previewColor;

	// Token: 0x040036FF RID: 14079
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 previewColorDefault = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
}
