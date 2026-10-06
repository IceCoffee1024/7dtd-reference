using System;
using System.IO;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000BF9 RID: 3065
public class WallVolume
{
	// Token: 0x06005C08 RID: 23560 RVA: 0x0023E643 File Offset: 0x0023C843
	public static WallVolume Create(PrefabWallVolume psv, Vector3i _boxMin, Vector3i _boxMax)
	{
		WallVolume wallVolume = new WallVolume();
		wallVolume.SetMinMax(_boxMin, _boxMax);
		wallVolume.AddToPrefabInstance();
		return wallVolume;
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x0023E65C File Offset: 0x0023C85C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
	{
		this.BoxMin = _boxMin;
		this.BoxMax = _boxMax;
		this.Center = (this.BoxMin + this.BoxMax).ToVector3() * 0.5f;
	}

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x06005C0A RID: 23562 RVA: 0x0023E6A0 File Offset: 0x0023C8A0
	public PrefabInstance PrefabInstance
	{
		get
		{
			return this.prefabInstance;
		}
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x0023E6A8 File Offset: 0x0023C8A8
	public bool AddToPrefabInstance()
	{
		this.prefabInstance = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().FindVolumeOwner(PrefabVolumeAbs.EVolumeType.Wall, this.BoxMin, this.BoxMax);
		if (this.prefabInstance != null)
		{
			this.prefabInstance.AddWallVolume(this);
			return true;
		}
		return false;
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x0023E700 File Offset: 0x0023C900
	public static WallVolume Read(BinaryReader _br)
	{
		WallVolume wallVolume = new WallVolume();
		_br.ReadByte();
		wallVolume.SetMinMax(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
		return wallVolume;
	}

	// Token: 0x06005C0D RID: 23565 RVA: 0x0023E750 File Offset: 0x0023C950
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(1);
		_bw.Write(this.BoxMin.x);
		_bw.Write(this.BoxMin.y);
		_bw.Write(this.BoxMin.z);
		_bw.Write(this.BoxMax.x);
		_bw.Write(this.BoxMax.y);
		_bw.Write(this.BoxMax.z);
	}

	// Token: 0x040047C6 RID: 18374
	public const int BinarySize = 25;

	// Token: 0x040047C7 RID: 18375
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte VERSION = 1;

	// Token: 0x040047C8 RID: 18376
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance prefabInstance;

	// Token: 0x040047C9 RID: 18377
	public Vector3i BoxMin;

	// Token: 0x040047CA RID: 18378
	public Vector3i BoxMax;

	// Token: 0x040047CB RID: 18379
	public Vector3 Center;
}
