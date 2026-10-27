using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007D8 RID: 2008
[Preserve]
public class NetPackageEditorAddVolumeFromClient : NetPackage
{
	// Token: 0x06003B72 RID: 15218 RVA: 0x0017FA2C File Offset: 0x0017DC2C
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, Vector3i _startPos, Vector3i _size)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.New;
		this.volumeType = _volumeType;
		this.startPos = _startPos;
		this.size = _size;
		return this;
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x0017FA4B File Offset: 0x0017DC4B
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, int _prefabInstanceId, int _existingIndex, Vector3i _offset)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.Clone;
		this.volumeType = _volumeType;
		this.startPos = _offset;
		this.prefabInstanceId = _prefabInstanceId;
		this.existingIndex = _existingIndex;
		return this;
	}

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06003B74 RID: 15220 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B75 RID: 15221 RVA: 0x0017FA74 File Offset: 0x0017DC74
	public override void read(PooledBinaryReader _br)
	{
		this.addType = (NetPackageEditorAddVolumeFromClient.EAddType)_br.ReadByte();
		this.volumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.startPos = StreamUtils.ReadVector3i(_br);
		this.size = StreamUtils.ReadVector3i(_br);
		this.prefabInstanceId = (int)_br.ReadInt16();
		this.existingIndex = (int)_br.ReadInt16();
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x0017FACC File Offset: 0x0017DCCC
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

	// Token: 0x06003B77 RID: 15223 RVA: 0x0017FB2C File Offset: 0x0017DD2C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.addType == NetPackageEditorAddVolumeFromClient.EAddType.New)
		{
			PrefabVolumeManager.Instance.AddVolumeServer(this.volumeType, this.startPos, this.size);
			return;
		}
		PrefabVolumeManager.Instance.CloneVolumeServer(this.volumeType, this.prefabInstanceId, this.existingIndex, this.startPos);
	}

	// Token: 0x04003062 RID: 12386
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorAddVolumeFromClient.EAddType addType;

	// Token: 0x04003063 RID: 12387
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs.EVolumeType volumeType;

	// Token: 0x04003064 RID: 12388
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i startPos;

	// Token: 0x04003065 RID: 12389
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04003066 RID: 12390
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x04003067 RID: 12391
	[PublicizedFrom(EAccessModifier.Private)]
	public int existingIndex;

	// Token: 0x020007D9 RID: 2009
	public enum EAddType
	{
		// Token: 0x04003069 RID: 12393
		New,
		// Token: 0x0400306A RID: 12394
		Clone
	}
}
