using System;
using UnityEngine.Scripting;

// Token: 0x02000811 RID: 2065
[Preserve]
public class NetPackageParticleEffect : NetPackage
{
	// Token: 0x06003CB2 RID: 15538 RVA: 0x00182762 File Offset: 0x00180962
	public NetPackageParticleEffect Setup(ParticleEffect _pe, int _entityThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false)
	{
		this.pe = _pe;
		this.entityThatCausedIt = _entityThatCausedIt;
		this.worldSpawn = _worldSpawn;
		this.forceCreation = _forceCreation;
		return this;
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x00182782 File Offset: 0x00180982
	public override void read(PooledBinaryReader _br)
	{
		this.pe = new ParticleEffect();
		this.pe.Read(_br);
		this.entityThatCausedIt = _br.ReadInt32();
		this.forceCreation = _br.ReadBoolean();
		this.worldSpawn = _br.ReadBoolean();
	}

	// Token: 0x06003CB4 RID: 15540 RVA: 0x001827BF File Offset: 0x001809BF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.pe.Write(_bw);
		_bw.Write(this.entityThatCausedIt);
		_bw.Write(this.forceCreation);
		_bw.Write(this.worldSpawn);
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x001827F8 File Offset: 0x001809F8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			_world.GetGameManager().SpawnParticleEffectServer(this.pe, this.entityThatCausedIt, this.forceCreation, this.worldSpawn);
			return;
		}
		_world.GetGameManager().SpawnParticleEffectClient(this.pe, this.entityThatCausedIt, this.forceCreation, this.worldSpawn);
	}

	// Token: 0x06003CB6 RID: 15542 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04003110 RID: 12560
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleEffect pe;

	// Token: 0x04003111 RID: 12561
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityThatCausedIt;

	// Token: 0x04003112 RID: 12562
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceCreation;

	// Token: 0x04003113 RID: 12563
	[PublicizedFrom(EAccessModifier.Private)]
	public bool worldSpawn;
}
