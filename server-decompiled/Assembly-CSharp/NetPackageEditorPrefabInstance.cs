using System;
using UnityEngine.Scripting;

// Token: 0x020007D0 RID: 2000
[Preserve]
public class NetPackageEditorPrefabInstance : NetPackage
{
	// Token: 0x06003B56 RID: 15190 RVA: 0x0017EA7C File Offset: 0x0017CC7C
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

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x06003B57 RID: 15191 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B58 RID: 15192 RVA: 0x0017EB08 File Offset: 0x0017CD08
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

	// Token: 0x06003B59 RID: 15193 RVA: 0x0017EB84 File Offset: 0x0017CD84
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

	// Token: 0x06003B5A RID: 15194 RVA: 0x0017EC08 File Offset: 0x0017CE08
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

	// Token: 0x0400304B RID: 12363
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorPrefabInstance.EChangeType changeType;

	// Token: 0x0400304C RID: 12364
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x0400304D RID: 12365
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxPosition;

	// Token: 0x0400304E RID: 12366
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxSize;

	// Token: 0x0400304F RID: 12367
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003050 RID: 12368
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04003051 RID: 12369
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x04003052 RID: 12370
	[PublicizedFrom(EAccessModifier.Private)]
	public int localRotation;

	// Token: 0x04003053 RID: 12371
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffset;

	// Token: 0x020007D1 RID: 2001
	public enum EChangeType
	{
		// Token: 0x04003055 RID: 12373
		Added,
		// Token: 0x04003056 RID: 12374
		Changed,
		// Token: 0x04003057 RID: 12375
		Removed
	}
}
