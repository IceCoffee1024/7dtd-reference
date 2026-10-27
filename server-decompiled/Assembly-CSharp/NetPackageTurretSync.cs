using System;
using UnityEngine.Scripting;

// Token: 0x02000869 RID: 2153
[Preserve]
public class NetPackageTurretSync : NetPackage
{
	// Token: 0x06003E57 RID: 15959 RVA: 0x0018BF29 File Offset: 0x0018A129
	public NetPackageTurretSync Setup(int _entityId, int _targetEntityId, bool _isOn, ItemValue _originalItemValue)
	{
		this.entityId = _entityId;
		this.targetEntityId = _targetEntityId;
		this.isOn = _isOn;
		this.itemValue = _originalItemValue;
		return this;
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x0018BF49 File Offset: 0x0018A149
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.targetEntityId = _reader.ReadInt32();
		this.isOn = _reader.ReadBoolean();
		this.itemValue = ItemValue.None;
		this.itemValue.Read(_reader);
	}

	// Token: 0x06003E59 RID: 15961 RVA: 0x0018BF86 File Offset: 0x0018A186
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.targetEntityId);
		_writer.Write(this.isOn);
		this.itemValue.Write(_writer);
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x0018BFC0 File Offset: 0x0018A1C0
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

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06003E5B RID: 15963 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040032AD RID: 12973
	public int entityId;

	// Token: 0x040032AE RID: 12974
	public int targetEntityId;

	// Token: 0x040032AF RID: 12975
	public bool isOn;

	// Token: 0x040032B0 RID: 12976
	public ItemValue itemValue;
}
