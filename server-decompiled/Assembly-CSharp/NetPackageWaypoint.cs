using System;
using UnityEngine.Scripting;

// Token: 0x0200086A RID: 2154
[Preserve]
public class NetPackageWaypoint : NetPackage
{
	// Token: 0x06003EEC RID: 16108 RVA: 0x0018C048 File Offset: 0x0018A248
	public NetPackageWaypoint Setup(Waypoint _waypoint, EnumWaypointInviteMode _inviteMode, int _inviterEntityId)
	{
		this.waypoint = _waypoint;
		this.waypoint.InviterEntityId = _inviterEntityId;
		this.inviteMode = _inviteMode;
		this.inviterEntityId = _inviterEntityId;
		return this;
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0018C06C File Offset: 0x0018A26C
	public override void read(PooledBinaryReader _br)
	{
		this.waypoint = new Waypoint();
		this.waypoint.Read(_br, 7);
		this.inviteMode = (EnumWaypointInviteMode)_br.ReadByte();
		this.inviterEntityId = _br.ReadInt32();
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0018C09E File Offset: 0x0018A29E
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.waypoint.Write(_bw);
		_bw.Write((byte)this.inviteMode);
		_bw.Write(this.inviterEntityId);
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x0018C0CC File Offset: 0x0018A2CC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidEntityIdForSender(this.inviterEntityId, false))
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.WaypointInviteServer(this.waypoint, this.inviteMode, this.inviterEntityId);
			return;
		}
		GameManager.Instance.WaypointInviteClient(this.waypoint, this.inviteMode, this.inviterEntityId, null);
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x040032CA RID: 13002
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint waypoint;

	// Token: 0x040032CB RID: 13003
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumWaypointInviteMode inviteMode;

	// Token: 0x040032CC RID: 13004
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;
}
