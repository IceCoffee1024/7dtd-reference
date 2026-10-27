using System;
using UnityEngine.Scripting;

// Token: 0x020007DA RID: 2010
[Preserve]
public class NetPackageEditorPrefabInstance : NetPackage
{
	// Token: 0x06003B79 RID: 15225 RVA: 0x0017FB80 File Offset: 0x0017DD80
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

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x06003B7A RID: 15226 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x0017FC0C File Offset: 0x0017DE0C
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

	// Token: 0x06003B7C RID: 15228 RVA: 0x0017FC88 File Offset: 0x0017DE88
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

	// Token: 0x06003B7D RID: 15229 RVA: 0x0017FD0C File Offset: 0x0017DF0C
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

	// Token: 0x0400306B RID: 12395
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorPrefabInstance.EChangeType changeType;

	// Token: 0x0400306C RID: 12396
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x0400306D RID: 12397
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxPosition;

	// Token: 0x0400306E RID: 12398
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxSize;

	// Token: 0x0400306F RID: 12399
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003070 RID: 12400
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04003071 RID: 12401
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x04003072 RID: 12402
	[PublicizedFrom(EAccessModifier.Private)]
	public int localRotation;

	// Token: 0x04003073 RID: 12403
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffset;

	// Token: 0x020007DB RID: 2011
	public enum EChangeType
	{
		// Token: 0x04003075 RID: 12405
		Added,
		// Token: 0x04003076 RID: 12406
		Changed,
		// Token: 0x04003077 RID: 12407
		Removed
	}
}
