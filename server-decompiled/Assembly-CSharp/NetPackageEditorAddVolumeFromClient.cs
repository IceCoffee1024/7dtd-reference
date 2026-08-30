using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007CB RID: 1995
[Preserve]
public class NetPackageEditorAddVolumeFromClient : NetPackage
{
	// Token: 0x06003B2F RID: 15151 RVA: 0x0017E36F File Offset: 0x0017C56F
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, Vector3i _startPos, Vector3i _size)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.New;
		this.volumeType = _volumeType;
		this.startPos = _startPos;
		this.size = _size;
		return this;
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x0017E38E File Offset: 0x0017C58E
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, int _prefabInstanceId, int _existingIndex, Vector3i _offset)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.Clone;
		this.volumeType = _volumeType;
		this.startPos = _offset;
		this.prefabInstanceId = _prefabInstanceId;
		this.existingIndex = _existingIndex;
		return this;
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06003B31 RID: 15153 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x0017E3B8 File Offset: 0x0017C5B8
	public override void read(PooledBinaryReader _br)
	{
		this.addType = (NetPackageEditorAddVolumeFromClient.EAddType)_br.ReadByte();
		this.volumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.startPos = StreamUtils.ReadVector3i(_br);
		this.size = StreamUtils.ReadVector3i(_br);
		this.prefabInstanceId = (int)_br.ReadInt16();
		this.existingIndex = (int)_br.ReadInt16();
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x0017E410 File Offset: 0x0017C610
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.addType);
		_bw.Write((byte)this.volumeType);
		StreamUtils.Write(_bw, this.startPos);
		StreamUtils.Write(_bw, this.size);
		_bw.Write((short)this.prefabInstanceId);
		_bw.Write((short)this.existingIndex);
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x0017E470 File Offset: 0x0017C670
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.addType == NetPackageEditorAddVolumeFromClient.EAddType.New)
		{
			PrefabVolumeManager.Instance.AddVolumeServer(this.volumeType, this.startPos, this.size);
			return;
		}
		PrefabVolumeManager.Instance.CloneVolumeServer(this.volumeType, this.prefabInstanceId, this.existingIndex, this.startPos);
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x0017E4C4 File Offset: 0x0017C6C4
	public override int GetLength()
	{
		return 29;
	}

	// Token: 0x04003048 RID: 12360
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorAddVolumeFromClient.EAddType addType;

	// Token: 0x04003049 RID: 12361
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs.EVolumeType volumeType;

	// Token: 0x0400304A RID: 12362
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i startPos;

	// Token: 0x0400304B RID: 12363
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x0400304C RID: 12364
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x0400304D RID: 12365
	[PublicizedFrom(EAccessModifier.Private)]
	public int existingIndex;

	// Token: 0x020007CC RID: 1996
	public enum EAddType
	{
		// Token: 0x0400304F RID: 12367
		New,
		// Token: 0x04003050 RID: 12368
		Clone
	}
}
