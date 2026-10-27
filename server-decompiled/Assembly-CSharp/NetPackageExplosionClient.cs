using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000801 RID: 2049
[Preserve]
public class NetPackageExplosionClient : NetPackage
{
	// Token: 0x06003C36 RID: 15414 RVA: 0x001824CC File Offset: 0x001806CC
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

	// Token: 0x06003C37 RID: 15415 RVA: 0x0018252C File Offset: 0x0018072C
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

	// Token: 0x06003C38 RID: 15416 RVA: 0x001825C4 File Offset: 0x001807C4
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

	// Token: 0x06003C39 RID: 15417 RVA: 0x0018266C File Offset: 0x0018086C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ExplosionClient(this.center, this.rotation, this.expType, this.blastPower, (float)this.blastRadius, (float)this.blockDamage, this.entityId, this.explosionChanges);
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06003C3A RID: 15418 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040030EA RID: 12522
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x040030EB RID: 12523
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion rotation;

	// Token: 0x040030EC RID: 12524
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> explosionChanges = new List<BlockChangeInfo>();

	// Token: 0x040030ED RID: 12525
	[PublicizedFrom(EAccessModifier.Private)]
	public int expType;

	// Token: 0x040030EE RID: 12526
	[PublicizedFrom(EAccessModifier.Private)]
	public int blastPower;

	// Token: 0x040030EF RID: 12527
	[PublicizedFrom(EAccessModifier.Private)]
	public int blastRadius;

	// Token: 0x040030F0 RID: 12528
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockDamage;

	// Token: 0x040030F1 RID: 12529
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
