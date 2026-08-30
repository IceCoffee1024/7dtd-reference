using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007C0 RID: 1984
[Preserve]
public class NetPackageEditorAddVolumeFromClient : NetPackage
{
	// Token: 0x06003AD0 RID: 15056 RVA: 0x0017BF97 File Offset: 0x0017A197
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, Vector3i _startPos, Vector3i _size)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.New;
		this.volumeType = _volumeType;
		this.startPos = _startPos;
		this.size = _size;
		return this;
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x0017BFB6 File Offset: 0x0017A1B6
	public NetPackageEditorAddVolumeFromClient Setup(PrefabVolumeAbs.EVolumeType _volumeType, int _prefabInstanceId, int _existingIndex, Vector3i _offset)
	{
		this.addType = NetPackageEditorAddVolumeFromClient.EAddType.Clone;
		this.volumeType = _volumeType;
		this.startPos = _offset;
		this.prefabInstanceId = _prefabInstanceId;
		this.existingIndex = _existingIndex;
		return this;
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x0017BFE0 File Offset: 0x0017A1E0
	public override void read(PooledBinaryReader _br)
	{
		this.addType = (NetPackageEditorAddVolumeFromClient.EAddType)_br.ReadByte();
		this.volumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.startPos = StreamUtils.ReadVector3i(_br);
		this.size = StreamUtils.ReadVector3i(_br);
		this.prefabInstanceId = (int)_br.ReadInt16();
		this.existingIndex = (int)_br.ReadInt16();
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x0017C038 File Offset: 0x0017A238
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

	// Token: 0x06003AD5 RID: 15061 RVA: 0x0017C098 File Offset: 0x0017A298
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.addType == NetPackageEditorAddVolumeFromClient.EAddType.New)
		{
			PrefabVolumeManager.Instance.AddVolumeServer(this.volumeType, this.startPos, this.size);
			return;
		}
		PrefabVolumeManager.Instance.CloneVolumeServer(this.volumeType, this.prefabInstanceId, this.existingIndex, this.startPos);
	}

	// Token: 0x06003AD6 RID: 15062 RVA: 0x0017C0EC File Offset: 0x0017A2EC
	public override int GetLength()
	{
		return 29;
	}

	// Token: 0x04002FC9 RID: 12233
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorAddVolumeFromClient.EAddType addType;

	// Token: 0x04002FCA RID: 12234
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs.EVolumeType volumeType;

	// Token: 0x04002FCB RID: 12235
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i startPos;

	// Token: 0x04002FCC RID: 12236
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04002FCD RID: 12237
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x04002FCE RID: 12238
	[PublicizedFrom(EAccessModifier.Private)]
	public int existingIndex;

	// Token: 0x020007C1 RID: 1985
	public enum EAddType
	{
		// Token: 0x04002FD0 RID: 12240
		New,
		// Token: 0x04002FD1 RID: 12241
		Clone
	}
}
