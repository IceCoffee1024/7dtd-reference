using System;
using UnityEngine.Scripting;

// Token: 0x0200081C RID: 2076
[Preserve]
public class NetPackageParticleEffect : NetPackage
{
	// Token: 0x06003D11 RID: 15633 RVA: 0x00184B3A File Offset: 0x00182D3A
	public NetPackageParticleEffect Setup(ParticleEffect _pe, int _entityThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false)
	{
		this.pe = _pe;
		this.entityThatCausedIt = _entityThatCausedIt;
		this.worldSpawn = _worldSpawn;
		this.forceCreation = _forceCreation;
		return this;
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x00184B5A File Offset: 0x00182D5A
	public override void read(PooledBinaryReader _br)
	{
		this.pe = new ParticleEffect();
		this.pe.Read(_br);
		this.entityThatCausedIt = _br.ReadInt32();
		this.forceCreation = _br.ReadBoolean();
		this.worldSpawn = _br.ReadBoolean();
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x00184B97 File Offset: 0x00182D97
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.pe.Write(_bw);
		_bw.Write(this.entityThatCausedIt);
		_bw.Write(this.forceCreation);
		_bw.Write(this.worldSpawn);
	}

	// Token: 0x06003D14 RID: 15636 RVA: 0x00184BD0 File Offset: 0x00182DD0
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

	// Token: 0x06003D15 RID: 15637 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400318F RID: 12687
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleEffect pe;

	// Token: 0x04003190 RID: 12688
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityThatCausedIt;

	// Token: 0x04003191 RID: 12689
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceCreation;

	// Token: 0x04003192 RID: 12690
	[PublicizedFrom(EAccessModifier.Private)]
	public bool worldSpawn;
}
