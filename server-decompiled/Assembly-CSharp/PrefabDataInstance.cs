using System;
using UnityEngine;

// Token: 0x0200092F RID: 2351
public class PrefabDataInstance
{
	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x060043B8 RID: 17336 RVA: 0x001A9036 File Offset: 0x001A7236
	public Vector3i boundingBoxSize
	{
		get
		{
			return this.prefab.size;
		}
	}

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x060043B9 RID: 17337 RVA: 0x001A9044 File Offset: 0x001A7244
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

	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x060043BA RID: 17338 RVA: 0x001A909C File Offset: 0x001A729C
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

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x060043BB RID: 17339 RVA: 0x001A9100 File Offset: 0x001A7300
	public PathAbstractions.AbstractedLocation location
	{
		get
		{
			return this.prefab.location;
		}
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x001A910D File Offset: 0x001A730D
	public PrefabDataInstance(int _id, Vector3i _position, byte _rotation, PrefabData _prefabData)
	{
		this.id = _id;
		this.prefab = _prefabData;
		this.boundingBoxPosition = _position;
		this.rotation = _rotation;
		this.previewColor = PrefabDataInstance.previewColorDefault;
	}

	// Token: 0x040036EE RID: 14062
	public int id;

	// Token: 0x040036EF RID: 14063
	public PrefabData prefab;

	// Token: 0x040036F0 RID: 14064
	public Vector3i boundingBoxPosition;

	// Token: 0x040036F1 RID: 14065
	public byte rotation;

	// Token: 0x040036F2 RID: 14066
	public Color32 previewColor;

	// Token: 0x040036F3 RID: 14067
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color32 previewColorDefault = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
}
