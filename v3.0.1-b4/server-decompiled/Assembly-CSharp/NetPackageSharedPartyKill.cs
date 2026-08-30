using System;
using UnityEngine.Scripting;

// Token: 0x0200083E RID: 2110
[Preserve]
public class NetPackageSharedPartyKill : NetPackage
{
	// Token: 0x06003DC1 RID: 15809 RVA: 0x00187592 File Offset: 0x00185792
	public NetPackageSharedPartyKill Setup(int _entityID, int _killerID)
	{
		this.entityID = _entityID;
		this.killerID = _killerID;
		return this;
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x001875A3 File Offset: 0x001857A3
	public NetPackageSharedPartyKill Setup(int _entityTypeID, int _xp, int _killerID, int _killedEntityID)
	{
		this.entityTypeID = _entityTypeID;
		this.xp = _xp;
		this.killerID = _killerID;
		this.entityID = _killedEntityID;
		return this;
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x001875C3 File Offset: 0x001857C3
	public override void read(PooledBinaryReader _br)
	{
		this.entityTypeID = _br.ReadInt32();
		this.xp = _br.ReadInt32();
		this.entityID = _br.ReadInt32();
		this.killerID = _br.ReadInt32();
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x001875F5 File Offset: 0x001857F5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityTypeID);
		_bw.Write(this.xp);
		_bw.Write(this.entityID);
		_bw.Write(this.killerID);
	}

	// Token: 0x06003DC5 RID: 15813 RVA: 0x00187630 File Offset: 0x00185830
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.SharedKillServer(this.entityID, this.killerID, 1f);
			return;
		}
		GameManager.Instance.SharedKillClient(this.entityTypeID, this.xp, null, this.entityID, -1);
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x0004A832 File Offset: 0x00048A32
	public override int GetLength()
	{
		return 25;
	}

	// Token: 0x040031EB RID: 12779
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityTypeID;

	// Token: 0x040031EC RID: 12780
	[PublicizedFrom(EAccessModifier.Private)]
	public int xp;

	// Token: 0x040031ED RID: 12781
	[PublicizedFrom(EAccessModifier.Private)]
	public int killerID;

	// Token: 0x040031EE RID: 12782
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityID;
}
