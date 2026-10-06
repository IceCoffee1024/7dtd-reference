using System;
using UnityEngine.Scripting;

// Token: 0x0200081A RID: 2074
[Preserve]
public class NetPackageParticleEffect : NetPackage
{
	// Token: 0x06003CD6 RID: 15574 RVA: 0x00184706 File Offset: 0x00182906
	public NetPackageParticleEffect Setup(ParticleEffect _pe, int _entityThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false)
	{
		this.pe = _pe;
		this.entityThatCausedIt = _entityThatCausedIt;
		this.worldSpawn = _worldSpawn;
		this.forceCreation = _forceCreation;
		return this;
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x00184726 File Offset: 0x00182926
	public override void read(PooledBinaryReader _br)
	{
		this.pe = new ParticleEffect();
		this.pe.Read(_br);
		this.entityThatCausedIt = _br.ReadInt32();
		this.forceCreation = _br.ReadBoolean();
		this.worldSpawn = _br.ReadBoolean();
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x00184763 File Offset: 0x00182963
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.pe.Write(_bw);
		_bw.Write(this.entityThatCausedIt);
		_bw.Write(this.forceCreation);
		_bw.Write(this.worldSpawn);
	}

	// Token: 0x06003CD9 RID: 15577 RVA: 0x0018479C File Offset: 0x0018299C
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

	// Token: 0x04003173 RID: 12659
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleEffect pe;

	// Token: 0x04003174 RID: 12660
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityThatCausedIt;

	// Token: 0x04003175 RID: 12661
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceCreation;

	// Token: 0x04003176 RID: 12662
	[PublicizedFrom(EAccessModifier.Private)]
	public bool worldSpawn;
}
