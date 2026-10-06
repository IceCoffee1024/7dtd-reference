using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007CE RID: 1998
[Preserve]
public class NetPackageEditorAddVolumeFromClient : NetPackage
{
	// Token: 0x06003B4F RID: 15183 RVA: 0x0017E928 File Offset: 0x0017CB28
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, Vector3i _startPos, Vector3i _size)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.New;
		this.volumeType = _volumeType;
		this.startPos = _startPos;
		this.size = _size;
		return this;
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x0017E947 File Offset: 0x0017CB47
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, int _prefabInstanceId, int _existingIndex, Vector3i _offset)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.Clone;
		this.volumeType = _volumeType;
		this.startPos = _offset;
		this.prefabInstanceId = _prefabInstanceId;
		this.existingIndex = _existingIndex;
		return this;
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06003B51 RID: 15185 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x0017E970 File Offset: 0x0017CB70
	public override void read(PooledBinaryReader _br)
	{
		this.addType = (NetPackageEditorAddVolumeFromClient.EAddType)_br.ReadByte();
		this.volumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.startPos = StreamUtils.ReadVector3i(_br);
		this.size = StreamUtils.ReadVector3i(_br);
		this.prefabInstanceId = (int)_br.ReadInt16();
		this.existingIndex = (int)_br.ReadInt16();
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x0017E9C8 File Offset: 0x0017CBC8
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

	// Token: 0x06003B54 RID: 15188 RVA: 0x0017EA28 File Offset: 0x0017CC28
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.addType == NetPackageEditorAddVolumeFromClient.EAddType.New)
		{
			PrefabVolumeManager.Instance.AddVolumeServer(this.volumeType, this.startPos, this.size);
			return;
		}
		PrefabVolumeManager.Instance.CloneVolumeServer(this.volumeType, this.prefabInstanceId, this.existingIndex, this.startPos);
	}

	// Token: 0x04003042 RID: 12354
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorAddVolumeFromClient.EAddType addType;

	// Token: 0x04003043 RID: 12355
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs.EVolumeType volumeType;

	// Token: 0x04003044 RID: 12356
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i startPos;

	// Token: 0x04003045 RID: 12357
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04003046 RID: 12358
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x04003047 RID: 12359
	[PublicizedFrom(EAccessModifier.Private)]
	public int existingIndex;

	// Token: 0x020007CF RID: 1999
	public enum EAddType
	{
		// Token: 0x04003049 RID: 12361
		New,
		// Token: 0x0400304A RID: 12362
		Clone
	}
}
