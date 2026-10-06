using System;
using UnityEngine.Scripting;

// Token: 0x0200085F RID: 2143
[Preserve]
public class NetPackageTurretSync : NetPackage
{
	// Token: 0x06003E34 RID: 15924 RVA: 0x0018AE81 File Offset: 0x00189081
	public NetPackageTurretSync Setup(int _entityId, int _targetEntityId, bool _isOn, ItemValue _originalItemValue)
	{
		this.entityId = _entityId;
		this.targetEntityId = _targetEntityId;
		this.isOn = _isOn;
		this.itemValue = _originalItemValue;
		return this;
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x0018AEA1 File Offset: 0x001890A1
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.targetEntityId = _reader.ReadInt32();
		this.isOn = _reader.ReadBoolean();
		this.itemValue = ItemValue.None;
		this.itemValue.Read(_reader);
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x0018AEDE File Offset: 0x001890DE
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.targetEntityId);
		_writer.Write(this.isOn);
		this.itemValue.Write(_writer);
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x0018AF18 File Offset: 0x00189118
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

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06003E38 RID: 15928 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400328D RID: 12941
	public int entityId;

	// Token: 0x0400328E RID: 12942
	public int targetEntityId;

	// Token: 0x0400328F RID: 12943
	public bool isOn;

	// Token: 0x04003290 RID: 12944
	public ItemValue itemValue;
}
