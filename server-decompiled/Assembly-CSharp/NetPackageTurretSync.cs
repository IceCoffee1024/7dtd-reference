using System;
using UnityEngine.Scripting;

// Token: 0x02000850 RID: 2128
[Preserve]
public class NetPackageTurretSync : NetPackage
{
	// Token: 0x06003E2C RID: 15916 RVA: 0x00188F15 File Offset: 0x00187115
	public NetPackageTurretSync Setup(int _entityId, int _targetEntityId, bool _isOn, ItemValue _originalItemValue)
	{
		this.entityId = _entityId;
		this.targetEntityId = _targetEntityId;
		this.isOn = _isOn;
		this.itemValue = _originalItemValue;
		return this;
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x00188F35 File Offset: 0x00187135
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.targetEntityId = _reader.ReadInt32();
		this.isOn = _reader.ReadBoolean();
		this.itemValue = ItemValue.None;
		this.itemValue.Read(_reader);
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x00188F72 File Offset: 0x00187172
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.targetEntityId);
		_writer.Write(this.isOn);
		this.itemValue.Write(_writer);
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x00188FAC File Offset: 0x001871AC
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

	// Token: 0x06003E30 RID: 15920 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06003E31 RID: 15921 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003221 RID: 12833
	public int entityId;

	// Token: 0x04003222 RID: 12834
	public int targetEntityId;

	// Token: 0x04003223 RID: 12835
	public bool isOn;

	// Token: 0x04003224 RID: 12836
	public ItemValue itemValue;
}
