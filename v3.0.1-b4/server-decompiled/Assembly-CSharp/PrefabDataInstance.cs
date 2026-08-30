using System;
using UnityEngine;

// Token: 0x0200091A RID: 2330
public class PrefabDataInstance
{
	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x060043A5 RID: 17317 RVA: 0x001A695E File Offset: 0x001A4B5E
	public Vector3i boundingBoxSize
	{
		get
		{
			return this.prefab.size;
		}
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x060043A6 RID: 17318 RVA: 0x001A696C File Offset: 0x001A4B6C
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

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x060043A7 RID: 17319 RVA: 0x001A69C4 File Offset: 0x001A4BC4
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

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x060043A8 RID: 17320 RVA: 0x001A6A28 File Offset: 0x001A4C28
	public PathAbstractions.AbstractedLocation location
	{
		get
		{
			return this.prefab.location;
		}
	}

	// Token: 0x060043A9 RID: 17321 RVA: 0x001A6A35 File Offset: 0x001A4C35
	public PrefabDataInstance(int _id, Vector3i _position, byte _rotation, PrefabData _prefabData)
	{
		this.id = _id;
		this.prefab = _prefabData;
		this.boundingBoxPosition = _position;
		this.rotation = _rotation;
		this.previewColor = PrefabDataInstance.previewColorDefault;
	}

	// Token: 0x04003671 RID: 13937
	public int id;

	// Token: 0x04003672 RID: 13938
	public PrefabData prefab;

	// Token: 0x04003673 RID: 13939
	public Vector3i boundingBoxPosition;

	// Token: 0x04003674 RID: 13940
	public byte rotation;

	// Token: 0x04003675 RID: 13941
	public Color32 previewColor;

	// Token: 0x04003676 RID: 13942
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 previewColorDefault = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
}
