using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E9 RID: 2025
[Preserve]
public class NetPackageExplosionClient : NetPackage
{
	// Token: 0x06003BB2 RID: 15282 RVA: 0x0017EAC8 File Offset: 0x0017CCC8
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

	// Token: 0x06003BB3 RID: 15283 RVA: 0x0017EB28 File Offset: 0x0017CD28
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

	// Token: 0x06003BB4 RID: 15284 RVA: 0x0017EBC0 File Offset: 0x0017CDC0
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

	// Token: 0x06003BB5 RID: 15285 RVA: 0x0017EC68 File Offset: 0x0017CE68
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ExplosionClient(this.center, this.rotation, this.expType, this.blastPower, (float)this.blastRadius, (float)this.blockDamage, this.entityId, this.explosionChanges);
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06003BB6 RID: 15286 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0017ECB7 File Offset: 0x0017CEB7
	public override int GetLength()
	{
		return 24 + this.explosionChanges.Count * 30;
	}

	// Token: 0x04003052 RID: 12370
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003053 RID: 12371
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion rotation;

	// Token: 0x04003054 RID: 12372
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> explosionChanges = new List<BlockChangeInfo>();

	// Token: 0x04003055 RID: 12373
	[PublicizedFrom(EAccessModifier.Private)]
	public int expType;

	// Token: 0x04003056 RID: 12374
	[PublicizedFrom(EAccessModifier.Private)]
	public int blastPower;

	// Token: 0x04003057 RID: 12375
	[PublicizedFrom(EAccessModifier.Private)]
	public int blastRadius;

	// Token: 0x04003058 RID: 12376
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockDamage;

	// Token: 0x04003059 RID: 12377
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
