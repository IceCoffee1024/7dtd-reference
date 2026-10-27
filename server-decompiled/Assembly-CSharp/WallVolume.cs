using System;
using System.IO;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000C04 RID: 3076
public class WallVolume
{
	// Token: 0x06005C2D RID: 23597 RVA: 0x0023FB8F File Offset: 0x0023DD8F
	public static WallVolume Create(PrefabWallVolume psv, Vector3i _boxMin, Vector3i _boxMax)
	{
		WallVolume wallVolume = new WallVolume();
		wallVolume.SetMinMax(_boxMin, _boxMax);
		wallVolume.AddToPrefabInstance();
		return wallVolume;
	}

	// Token: 0x06005C2E RID: 23598 RVA: 0x0023FBA8 File Offset: 0x0023DDA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
	{
		this.BoxMin = _boxMin;
		this.BoxMax = _boxMax;
		this.Center = (this.BoxMin + this.BoxMax).ToVector3() * 0.5f;
	}

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x06005C2F RID: 23599 RVA: 0x0023FBEC File Offset: 0x0023DDEC
	public PrefabInstance PrefabInstance
	{
		get
		{
			return this.prefabInstance;
		}
	}

	// Token: 0x06005C30 RID: 23600 RVA: 0x0023FBF4 File Offset: 0x0023DDF4
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

	// Token: 0x06005C31 RID: 23601 RVA: 0x0023FC4C File Offset: 0x0023DE4C
	public static WallVolume Read(BinaryReader _br)
	{
		WallVolume wallVolume = new WallVolume();
		_br.ReadByte();
		wallVolume.SetMinMax(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
		return wallVolume;
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x0023FC9C File Offset: 0x0023DE9C
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

	// Token: 0x040047EB RID: 18411
	public const int BinarySize = 25;

	// Token: 0x040047EC RID: 18412
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte VERSION = 1;

	// Token: 0x040047ED RID: 18413
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance prefabInstance;

	// Token: 0x040047EE RID: 18414
	public Vector3i BoxMin;

	// Token: 0x040047EF RID: 18415
	public Vector3i BoxMax;

	// Token: 0x040047F0 RID: 18416
	public Vector3 Center;
}
