using System;
using UnityEngine.Scripting;

// Token: 0x02000855 RID: 2133
[Preserve]
public class NetPackageSharedPartyKill : NetPackage
{
	// Token: 0x06003DF1 RID: 15857 RVA: 0x0018A172 File Offset: 0x00188372
	public NetPackageSharedPartyKill Setup(int _entityID, int _killerID)
	{
		this.entityID = _entityID;
		this.killerID = _killerID;
		return this;
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x0018A183 File Offset: 0x00188383
	public NetPackageSharedPartyKill Setup(int _entityTypeID, int _xp, int _killerID, int _killedEntityID)
	{
		this.entityTypeID = _entityTypeID;
		this.xp = _xp;
		this.killerID = _killerID;
		this.entityID = _killedEntityID;
		return this;
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x0018A1A3 File Offset: 0x001883A3
	public override void read(PooledBinaryReader _br)
	{
		this.entityTypeID = _br.ReadInt32();
		this.xp = _br.ReadInt32();
		this.entityID = _br.ReadInt32();
		this.killerID = _br.ReadInt32();
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x0018A1D5 File Offset: 0x001883D5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityTypeID);
		_bw.Write(this.xp);
		_bw.Write(this.entityID);
		_bw.Write(this.killerID);
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x0018A210 File Offset: 0x00188410
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.SharedKillServer(this.entityID, this.killerID, 1f);
			return;
		}
		GameManager.Instance.SharedKillClient(this.entityTypeID, this.xp, null, this.entityID, -1);
	}

	// Token: 0x04003271 RID: 12913
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityTypeID;

	// Token: 0x04003272 RID: 12914
	[PublicizedFrom(EAccessModifier.Private)]
	public int xp;

	// Token: 0x04003273 RID: 12915
	[PublicizedFrom(EAccessModifier.Private)]
	public int killerID;

	// Token: 0x04003274 RID: 12916
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID;
}
