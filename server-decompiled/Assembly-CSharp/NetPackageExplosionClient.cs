using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F4 RID: 2036
[Preserve]
public class NetPackageExplosionClient : NetPackage
{
	// Token: 0x06003C11 RID: 15377 RVA: 0x00180EA0 File Offset: 0x0017F0A0
	public NetPackageExplosionClient Setup(Vector3 _center, Quaternion _rotation, int _expType, int _blastPower, float _blastRadius, float _blockDamage, int _entityId, List<BlockChangeInfo> _explosionChanges)
	{
		this.center = _center;
		this.rotation = _rotation;
		this.expType = _expType;
		this.blastPower = _blastPower;
		this.blastRadius = (int)_blastRadius;
		this.blockDamage = (int)_blockDamage;
		this.entityId = _entityId;
		this.explosionChanges.Clear();
		this.explosionChanges.AddRange(_explosionChanges);
		return this;
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x00180F00 File Offset: 0x0017F100
	public override void read(PooledBinaryReader _br)
	{
		this.center = StreamUtils.ReadVector3(_br);
		this.rotation = StreamUtils.ReadQuaterion(_br);
		this.expType = (int)_br.ReadInt16();
		this.blastPower = (int)_br.ReadInt16();
		this.blastRadius = (int)_br.ReadInt16();
		this.blockDamage = (int)_br.ReadInt16();
		this.entityId = _br.ReadInt32();
		int num = (int)_br.ReadUInt16();
		this.explosionChanges.Clear();
		for (int i = 0; i < num; i++)
		{
			BlockChangeInfo blockChangeInfo = new BlockChangeInfo();
			blockChangeInfo.Read(_br);
			this.explosionChanges.Add(blockChangeInfo);
		}
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x00180F98 File Offset: 0x0017F198
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.center);
		StreamUtils.Write(_bw, this.rotation);
		_bw.Write((short)this.expType);
		_bw.Write((ushort)this.blastPower);
		_bw.Write((ushort)this.blastRadius);
		_bw.Write((ushort)this.blockDamage);
		_bw.Write(this.entityId);
		_bw.Write((ushort)this.explosionChanges.Count);
		for (int i = 0; i < this.explosionChanges.Count; i++)
		{
			this.explosionChanges[i].Write(_bw);
		}
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x00181040 File Offset: 0x0017F240
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ExplosionClient(this.center, this.rotation, this.expType, this.blastPower, (float)this.blastRadius, (float)this.blockDamage, this.entityId, this.explosionChanges);
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06003C15 RID: 15381 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x0018108F File Offset: 0x0017F28F
	public override int GetLength()
	{
		return 24 + this.explosionChanges.Count * 30;
	}

	// Token: 0x040030D1 RID: 12497
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x040030D2 RID: 12498
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion rotation;

	// Token: 0x040030D3 RID: 12499
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> explosionChanges = new List<BlockChangeInfo>();

	// Token: 0x040030D4 RID: 12500
	[PublicizedFrom(EAccessModifier.Private)]
	public int expType;

	// Token: 0x040030D5 RID: 12501
	[PublicizedFrom(EAccessModifier.Private)]
	public int blastPower;

	// Token: 0x040030D6 RID: 12502
	[PublicizedFrom(EAccessModifier.Private)]
	public int blastRadius;

	// Token: 0x040030D7 RID: 12503
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockDamage;

	// Token: 0x040030D8 RID: 12504
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
