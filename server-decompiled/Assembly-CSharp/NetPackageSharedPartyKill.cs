using System;
using UnityEngine.Scripting;

// Token: 0x0200084B RID: 2123
[Preserve]
public class NetPackageSharedPartyKill : NetPackage
{
	// Token: 0x06003DCE RID: 15822 RVA: 0x001890CA File Offset: 0x001872CA
	public NetPackageSharedPartyKill Setup(int _entityID, int _killerID)
	{
		this.entityID = _entityID;
		this.killerID = _killerID;
		return this;
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x001890DB File Offset: 0x001872DB
	public NetPackageSharedPartyKill Setup(int _entityTypeID, int _xp, int _killerID, int _killedEntityID)
	{
		this.entityTypeID = _entityTypeID;
		this.xp = _xp;
		this.killerID = _killerID;
		this.entityID = _killedEntityID;
		return this;
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x001890FB File Offset: 0x001872FB
	public override void read(PooledBinaryReader _br)
	{
		this.entityTypeID = _br.ReadInt32();
		this.xp = _br.ReadInt32();
		this.entityID = _br.ReadInt32();
		this.killerID = _br.ReadInt32();
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x0018912D File Offset: 0x0018732D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityTypeID);
		_bw.Write(this.xp);
		_bw.Write(this.entityID);
		_bw.Write(this.killerID);
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00189168 File Offset: 0x00187368
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.SharedKillServer(this.entityID, this.killerID, 1f);
			return;
		}
		GameManager.Instance.SharedKillClient(this.entityTypeID, this.xp, null, this.entityID, -1);
	}

	// Token: 0x04003251 RID: 12881
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityTypeID;

	// Token: 0x04003252 RID: 12882
	[PublicizedFrom(EAccessModifier.Private)]
	public int xp;

	// Token: 0x04003253 RID: 12883
	[PublicizedFrom(EAccessModifier.Private)]
	public int killerID;

	// Token: 0x04003254 RID: 12884
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID;
}
