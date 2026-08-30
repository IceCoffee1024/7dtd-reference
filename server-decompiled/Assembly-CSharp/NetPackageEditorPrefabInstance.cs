using System;
using UnityEngine.Scripting;

// Token: 0x020007C2 RID: 1986
[Preserve]
public class NetPackageEditorPrefabInstance : NetPackage
{
	// Token: 0x06003AD8 RID: 15064 RVA: 0x0017C0F0 File Offset: 0x0017A2F0
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

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003ADA RID: 15066 RVA: 0x0017C17C File Offset: 0x0017A37C
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

	// Token: 0x06003ADB RID: 15067 RVA: 0x0017C1F8 File Offset: 0x0017A3F8
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

	// Token: 0x06003ADC RID: 15068 RVA: 0x0017C27C File Offset: 0x0017A47C
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

	// Token: 0x06003ADD RID: 15069 RVA: 0x0017C343 File Offset: 0x0017A543
	public override int GetLength()
	{
		return 33 + this.name.Length + 12 + this.filename.Length + 4;
	}

	// Token: 0x04002FD2 RID: 12242
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorPrefabInstance.EChangeType changeType;

	// Token: 0x04002FD3 RID: 12243
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x04002FD4 RID: 12244
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxPosition;

	// Token: 0x04002FD5 RID: 12245
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i boundingBoxSize;

	// Token: 0x04002FD6 RID: 12246
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04002FD7 RID: 12247
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i size;

	// Token: 0x04002FD8 RID: 12248
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x04002FD9 RID: 12249
	[PublicizedFrom(EAccessModifier.Private)]
	public int localRotation;

	// Token: 0x04002FDA RID: 12250
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffset;

	// Token: 0x020007C3 RID: 1987
	public enum EChangeType
	{
		// Token: 0x04002FDC RID: 12252
		Added,
		// Token: 0x04002FDD RID: 12253
		Changed,
		// Token: 0x04002FDE RID: 12254
		Removed
	}
}
