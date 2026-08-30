using System;
using UnityEngine.Scripting;

// Token: 0x020007CD RID: 1997
[Preserve]
public class NetPackageEditorPrefabInstance : NetPackage
{
	// Token: 0x06003B37 RID: 15159 RVA: 0x0017E4C8 File Offset: 0x0017C6C8
	public NetPackageEditorPrefabInstance Setup(NetPackageEditorPrefabInstance.EChangeType _changeType, PrefabInstance _prefabInstance)
	{
		this.changeType = _changeType;
		this.id = _prefabInstance.id;
		this.boundingBoxPosition = _prefabInstance.boundingBoxPosition;
		this.boundingBoxSize = _prefabInstance.boundingBoxSize;
		this.name = _prefabInstance.name;
		this.size = _prefabInstance.prefab.size;
		this.filename = _prefabInstance.prefab.PrefabName;
		this.localRotation = _prefabInstance.prefab.GetLocalRotation();
		this.yOffset = _prefabInstance.prefab.yOffset;
		return this;
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06003B38 RID: 15160 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x0017E554 File Offset: 0x0017C754
	public override void read(PooledBinaryReader _br)
	{
		this.changeType = (NetPackageEditorPrefabInstance.EChangeType)_br.ReadByte();
		this.id = _br.ReadInt32();
		this.boundingBoxPosition = StreamUtils.ReadVector3i(_br);
		this.boundingBoxSize = StreamUtils.ReadVector3i(_br);
		this.name = _br.ReadString();
		this.size = StreamUtils.ReadVector3i(_br);
		this.filename = _br.ReadString();
		this.localRotation = _br.ReadInt32();
		this.yOffset = _br.ReadInt32();
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x0017E5D0 File Offset: 0x0017C7D0
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.changeType);
		_bw.Write(this.id);
		StreamUtils.Write(_bw, this.boundingBoxPosition);
		StreamUtils.Write(_bw, this.boundingBoxSize);
		_bw.Write(this.name);
		StreamUtils.Write(_bw, this.size);
		_bw.Write(this.filename);
		_bw.Write(this.localRotation);
		_bw.Write(this.yOffset);
	}

	// Token: 0x06003B3B RID: 15163 RVA: 0x0017E654 File Offset: 0x0017C854
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			return;
		}
		switch (this.changeType)
		{
		case NetPackageEditorPrefabInstance.EChangeType.Added:
			PrefabInstanceClientManager.Instance.PrefabLoadedClient(this.id, this.boundingBoxPosition, this.boundingBoxSize, this.name, this.size, this.filename, this.localRotation, this.yOffset);
			return;
		case NetPackageEditorPrefabInstance.EChangeType.Changed:
			PrefabInstanceClientManager.Instance.PrefabChangedClient(this.id, this.boundingBoxPosition, this.boundingBoxSize, this.name, this.size, this.filename, this.localRotation, this.yOffset);
			return;
		case NetPackageEditorPrefabInstance.EChangeType.Removed:
			PrefabInstanceClientManager.Instance.PrefabRemovedClient(this.id);
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06003B3C RID: 15164 RVA: 0x0017E71B File Offset: 0x0017C91B
	public override int GetLength()
	{
		return 33 + this.name.Length + 12 + this.filename.Length + 4;
	}

	// Token: 0x04003051 RID: 12369
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorPrefabInstance.EChangeType changeType;

	// Token: 0x04003052 RID: 12370
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x04003053 RID: 12371
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxPosition;

	// Token: 0x04003054 RID: 12372
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxSize;

	// Token: 0x04003055 RID: 12373
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003056 RID: 12374
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04003057 RID: 12375
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x04003058 RID: 12376
	[PublicizedFrom(EAccessModifier.Private)]
	public int localRotation;

	// Token: 0x04003059 RID: 12377
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffset;

	// Token: 0x020007CE RID: 1998
	public enum EChangeType
	{
		// Token: 0x0400305B RID: 12379
		Added,
		// Token: 0x0400305C RID: 12380
		Changed,
		// Token: 0x0400305D RID: 12381
		Removed
	}
}
