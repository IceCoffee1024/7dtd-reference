using System;
using UnityEngine;

// Token: 0x02000939 RID: 2361
public class PrefabDataInstance
{
	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x060043DB RID: 17371 RVA: 0x001AA0BA File Offset: 0x001A82BA
	public Vector3i boundingBoxSize
	{
		get
		{
			return this.prefab.size;
		}
	}

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x060043DC RID: 17372 RVA: 0x001AA0C8 File Offset: 0x001A82C8
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

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x060043DD RID: 17373 RVA: 0x001AA120 File Offset: 0x001A8320
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

	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x060043DE RID: 17374 RVA: 0x001AA184 File Offset: 0x001A8384
	public PathAbstractions.AbstractedLocation location
	{
		get
		{
			return this.prefab.location;
		}
	}

	// Token: 0x060043DF RID: 17375 RVA: 0x001AA191 File Offset: 0x001A8391
	public PrefabDataInstance(int _id, Vector3i _position, byte _rotation, PrefabData _prefabData)
	{
		this.id = _id;
		this.prefab = _prefabData;
		this.boundingBoxPosition = _position;
		this.rotation = _rotation;
		this.previewColor = PrefabDataInstance.previewColorDefault;
	}

	// Token: 0x0400370E RID: 14094
	public int id;

	// Token: 0x0400370F RID: 14095
	public PrefabData prefab;

	// Token: 0x04003710 RID: 14096
	public Vector3i boundingBoxPosition;

	// Token: 0x04003711 RID: 14097
	public byte rotation;

	// Token: 0x04003712 RID: 14098
	public Color32 previewColor;

	// Token: 0x04003713 RID: 14099
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 previewColorDefault = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
}
