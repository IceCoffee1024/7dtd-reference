using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000802 RID: 2050
[Preserve]
public class NetPackageExplosionInitiate : NetPackage
{
	// Token: 0x06003C3C RID: 15420 RVA: 0x001826CE File Offset: 0x001808CE
	public NetPackageExplosionInitiate Setup(Vector3 _worldPos, Vector3i _blockPos, Quaternion _rotation, ExplosionData _explosionData, int _entityId, float _delay, bool _bRemoveBlockAtExplPosition, ItemValue _itemValueExplosive)
	{
		this.worldPos = _worldPos;
		this.blockPos = _blockPos;
		this.rotation = _rotation;
		this.explosionData = _explosionData;
		this.entityId = _entityId;
		this.delay = _delay;
		this.bRemoveBlockAtExplPosition = _bRemoveBlockAtExplPosition;
		this.itemValueExplosive = _itemValueExplosive;
		return this;
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x00182710 File Offset: 0x00180910
	public override void read(PooledBinaryReader _br)
	{
		this.worldPos = StreamUtils.ReadVector3(_br);
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.rotation = StreamUtils.ReadQuaterion(_br);
		int num = (int)_br.ReadUInt16();
		this.explosionData = new ExplosionData(_br.ReadBytes(num));
		this.entityId = _br.ReadInt32();
		this.delay = _br.ReadSingle();
		this.bRemoveBlockAtExplPosition = _br.ReadBoolean();
		if (_br.ReadBoolean())
		{
			this.itemValueExplosive = new ItemValue();
			this.itemValueExplosive.Read(_br);
		}
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x001827A0 File Offset: 0x001809A0
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.worldPos);
		StreamUtils.Write(_bw, this.blockPos);
		StreamUtils.Write(_bw, this.rotation);
		byte[] array = this.explosionData.ToByteArray();
		_bw.Write((ushort)array.Length);
		_bw.Write(array);
		_bw.Write(this.entityId);
		_bw.Write(this.delay);
		_bw.Write(this.bRemoveBlockAtExplPosition);
		_bw.Write(this.itemValueExplosive != null);
		if (this.itemValueExplosive != null)
		{
			this.itemValueExplosive.Write(_bw);
		}
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x00182840 File Offset: 0x00180A40
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ExplosionServer(this.worldPos, this.blockPos, this.rotation, this.explosionData, this.entityId, this.delay, this.bRemoveBlockAtExplPosition, this.itemValueExplosive);
	}

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06003C40 RID: 15424 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x040030F2 RID: 12530
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPos;

	// Token: 0x040030F3 RID: 12531
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040030F4 RID: 12532
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion rotation;

	// Token: 0x040030F5 RID: 12533
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosionData;

	// Token: 0x040030F6 RID: 12534
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030F7 RID: 12535
	[PublicizedFrom(EAccessModifier.Private)]
	public float delay;

	// Token: 0x040030F8 RID: 12536
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bRemoveBlockAtExplPosition;

	// Token: 0x040030F9 RID: 12537
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValueExplosive;
}
