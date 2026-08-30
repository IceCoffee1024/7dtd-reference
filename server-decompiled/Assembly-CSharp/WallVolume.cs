using System;
using System.IO;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000BD5 RID: 3029
public class WallVolume
{
	// Token: 0x06005B66 RID: 23398 RVA: 0x0023862B File Offset: 0x0023682B
	public static WallVolume Create(PrefabWallVolume psv, Vector3i _boxMin, Vector3i _boxMax)
	{
		WallVolume wallVolume = new WallVolume();
		wallVolume.SetMinMax(_boxMin, _boxMax);
		wallVolume.AddToPrefabInstance();
		return wallVolume;
	}

	// Token: 0x06005B67 RID: 23399 RVA: 0x00238644 File Offset: 0x00236844
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
	{
		this.BoxMin = _boxMin;
		this.BoxMax = _boxMax;
		this.Center = (this.BoxMin + this.BoxMax).ToVector3() * 0.5f;
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06005B68 RID: 23400 RVA: 0x00238688 File Offset: 0x00236888
	public PrefabInstance PrefabInstance
	{
		get
		{
			return this.prefabInstance;
		}
	}

	// Token: 0x06005B69 RID: 23401 RVA: 0x00238690 File Offset: 0x00236890
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

	// Token: 0x06005B6A RID: 23402 RVA: 0x002386E8 File Offset: 0x002368E8
	public static WallVolume Read(BinaryReader _br)
	{
		WallVolume wallVolume = new WallVolume();
		_br.ReadByte();
		wallVolume.SetMinMax(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
		return wallVolume;
	}

	// Token: 0x06005B6B RID: 23403 RVA: 0x00238738 File Offset: 0x00236938
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

	// Token: 0x040046EB RID: 18155
	public const int BinarySize = 25;

	// Token: 0x040046EC RID: 18156
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte VERSION = 1;

	// Token: 0x040046ED RID: 18157
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance prefabInstance;

	// Token: 0x040046EE RID: 18158
	public Vector3i BoxMin;

	// Token: 0x040046EF RID: 18159
	public Vector3i BoxMax;

	// Token: 0x040046F0 RID: 18160
	public Vector3 Center;
}
