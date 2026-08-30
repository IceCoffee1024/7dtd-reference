using System;
using UnityEngine.Scripting;

// Token: 0x0200084A RID: 2122
[Preserve]
public class NetPackageSharedPartyKill : NetPackage
{
	// Token: 0x06003E26 RID: 15910 RVA: 0x001895B2 File Offset: 0x001877B2
	public NetPackageSharedPartyKill Setup(int _entityID, int _killerID)
	{
		this.entityID = _entityID;
		this.killerID = _killerID;
		return this;
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x001895C3 File Offset: 0x001877C3
	public NetPackageSharedPartyKill Setup(int _entityTypeID, int _xp, int _killerID, int _killedEntityID)
	{
		this.entityTypeID = _entityTypeID;
		this.xp = _xp;
		this.killerID = _killerID;
		this.entityID = _killedEntityID;
		return this;
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x001895E3 File Offset: 0x001877E3
	public override void read(PooledBinaryReader _br)
	{
		this.entityTypeID = _br.ReadInt32();
		this.xp = _br.ReadInt32();
		this.entityID = _br.ReadInt32();
		this.killerID = _br.ReadInt32();
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x00189615 File Offset: 0x00187815
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityTypeID);
		_bw.Write(this.xp);
		_bw.Write(this.entityID);
		_bw.Write(this.killerID);
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x00189650 File Offset: 0x00187850
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.SharedKillServer(this.entityID, this.killerID, 1f);
			return;
		}
		GameManager.Instance.SharedKillClient(this.entityTypeID, this.xp, null, this.entityID, -1);
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x0004AC4A File Offset: 0x00048E4A
	public override int GetLength()
	{
		return 25;
	}

	// Token: 0x0400326A RID: 12906
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityTypeID;

	// Token: 0x0400326B RID: 12907
	[PublicizedFrom(EAccessModifier.Private)]
	public int xp;

	// Token: 0x0400326C RID: 12908
	[PublicizedFrom(EAccessModifier.Private)]
	public int killerID;

	// Token: 0x0400326D RID: 12909
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID;
}
