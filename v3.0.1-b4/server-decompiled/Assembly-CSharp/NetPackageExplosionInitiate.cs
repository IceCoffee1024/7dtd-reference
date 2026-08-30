using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007EA RID: 2026
[Preserve]
public class NetPackageExplosionInitiate : NetPackage
{
	// Token: 0x06003BB9 RID: 15289 RVA: 0x0017ECDD File Offset: 0x0017CEDD
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

	// Token: 0x06003BBA RID: 15290 RVA: 0x0017ED20 File Offset: 0x0017CF20
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

	// Token: 0x06003BBB RID: 15291 RVA: 0x0017EDB0 File Offset: 0x0017CFB0
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

	// Token: 0x06003BBC RID: 15292 RVA: 0x0017EE50 File Offset: 0x0017D050
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ExplosionServer(this.worldPos, this.blockPos, this.rotation, this.explosionData, this.entityId, this.delay, this.bRemoveBlockAtExplPosition, this.itemValueExplosive);
	}

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06003BBD RID: 15293 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x001758DA File Offset: 0x00173ADA
	public override int GetLength()
	{
		return 70;
	}

	// Token: 0x0400305A RID: 12378
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPos;

	// Token: 0x0400305B RID: 12379
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400305C RID: 12380
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion rotation;

	// Token: 0x0400305D RID: 12381
	[PublicizedFrom(EAccessModifier.Private)]
	public ExplosionData explosionData;

	// Token: 0x0400305E RID: 12382
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400305F RID: 12383
	[PublicizedFrom(EAccessModifier.Private)]
	public float delay;

	// Token: 0x04003060 RID: 12384
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bRemoveBlockAtExplPosition;

	// Token: 0x04003061 RID: 12385
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValueExplosive;
}
