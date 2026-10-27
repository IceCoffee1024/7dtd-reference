using System;
using UnityEngine.Scripting;

// Token: 0x02000824 RID: 2084
[Preserve]
public class NetPackageParticleEffect : NetPackage
{
	// Token: 0x06003CF9 RID: 15609 RVA: 0x0018580A File Offset: 0x00183A0A
	public NetPackageParticleEffect Setup(ParticleEffect _pe, int _entityThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false)
	{
		this.pe = _pe;
		this.entityThatCausedIt = _entityThatCausedIt;
		this.worldSpawn = _worldSpawn;
		this.forceCreation = _forceCreation;
		return this;
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x0018582A File Offset: 0x00183A2A
	public override void read(PooledBinaryReader _br)
	{
		this.pe = new ParticleEffect();
		this.pe.Read(_br);
		this.entityThatCausedIt = _br.ReadInt32();
		this.forceCreation = _br.ReadBoolean();
		this.worldSpawn = _br.ReadBoolean();
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x00185867 File Offset: 0x00183A67
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.pe.Write(_bw);
		_bw.Write(this.entityThatCausedIt);
		_bw.Write(this.forceCreation);
		_bw.Write(this.worldSpawn);
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x001858A0 File Offset: 0x00183AA0
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

	// Token: 0x04003193 RID: 12691
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleEffect pe;

	// Token: 0x04003194 RID: 12692
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityThatCausedIt;

	// Token: 0x04003195 RID: 12693
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceCreation;

	// Token: 0x04003196 RID: 12694
	[PublicizedFrom(EAccessModifier.Private)]
	public bool worldSpawn;
}
