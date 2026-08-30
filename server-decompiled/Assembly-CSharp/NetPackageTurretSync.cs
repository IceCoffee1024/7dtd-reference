using System;
using UnityEngine.Scripting;

// Token: 0x0200085C RID: 2140
[Preserve]
public class NetPackageTurretSync : NetPackage
{
	// Token: 0x06003E91 RID: 16017 RVA: 0x0018AFE5 File Offset: 0x001891E5
	public NetPackageTurretSync Setup(int _entityId, int _targetEntityId, bool _isOn, ItemValue _originalItemValue)
	{
		this.entityId = _entityId;
		this.targetEntityId = _targetEntityId;
		this.isOn = _isOn;
		this.itemValue = _originalItemValue;
		return this;
	}

	// Token: 0x06003E92 RID: 16018 RVA: 0x0018B005 File Offset: 0x00189205
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.targetEntityId = _reader.ReadInt32();
		this.isOn = _reader.ReadBoolean();
		this.itemValue = ItemValue.None;
		this.itemValue.Read(_reader);
	}

	// Token: 0x06003E93 RID: 16019 RVA: 0x0018B042 File Offset: 0x00189242
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.targetEntityId);
		_writer.Write(this.isOn);
		this.itemValue.Write(_writer);
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0018B07C File Offset: 0x0018927C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityTurret entityTurret = GameManager.Instance.World.GetEntity(this.entityId) as EntityTurret;
		if (entityTurret != null)
		{
			entityTurret.TargetEntityId = this.targetEntityId;
			entityTurret.OriginalItemValue = this.itemValue;
			entityTurret.IsOn = this.isOn;
		}
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06003E96 RID: 16022 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040032A2 RID: 12962
	public int entityId;

	// Token: 0x040032A3 RID: 12963
	public int targetEntityId;

	// Token: 0x040032A4 RID: 12964
	public bool isOn;

	// Token: 0x040032A5 RID: 12965
	public ItemValue itemValue;
}
