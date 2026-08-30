using System;
using System.IO;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000BE7 RID: 3047
public class WallVolume
{
	// Token: 0x06005BEF RID: 23535 RVA: 0x0023B347 File Offset: 0x00239547
	public static WallVolume Create(PrefabWallVolume psv, Vector3i _boxMin, Vector3i _boxMax)
	{
		WallVolume wallVolume = new WallVolume();
		wallVolume.SetMinMax(_boxMin, _boxMax);
		wallVolume.AddToPrefabInstance();
		return wallVolume;
	}

	// Token: 0x06005BF0 RID: 23536 RVA: 0x0023B360 File Offset: 0x00239560
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
	{
		this.BoxMin = _boxMin;
		this.BoxMax = _boxMax;
		this.Center = (this.BoxMin + this.BoxMax).ToVector3() * 0.5f;
	}

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06005BF1 RID: 23537 RVA: 0x0023B3A4 File Offset: 0x002395A4
	public PrefabInstance PrefabInstance
	{
		get
		{
			return this.prefabInstance;
		}
	}

	// Token: 0x06005BF2 RID: 23538 RVA: 0x0023B3AC File Offset: 0x002395AC
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

	// Token: 0x06005BF3 RID: 23539 RVA: 0x0023B404 File Offset: 0x00239604
	public static WallVolume Read(BinaryReader _br)
	{
		WallVolume wallVolume = new WallVolume();
		_br.ReadByte();
		wallVolume.SetMinMax(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
		return wallVolume;
	}

	// Token: 0x06005BF4 RID: 23540 RVA: 0x0023B454 File Offset: 0x00239654
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

	// Token: 0x04004787 RID: 18311
	public const int BinarySize = 25;

	// Token: 0x04004788 RID: 18312
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte VERSION = 1;

	// Token: 0x04004789 RID: 18313
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance prefabInstance;

	// Token: 0x0400478A RID: 18314
	public Vector3i BoxMin;

	// Token: 0x0400478B RID: 18315
	public Vector3i BoxMax;

	// Token: 0x0400478C RID: 18316
	public Vector3 Center;
}
