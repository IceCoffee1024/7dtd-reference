using System;
using UnityEngine.Scripting;

// Token: 0x0200086D RID: 2157
[Preserve]
public class NetPackageWaypoint : NetPackage
{
	// Token: 0x06003E84 RID: 16004 RVA: 0x0018BEC0 File Offset: 0x0018A0C0
	public NetPackageWaypoint Setup(Waypoint _waypoint, EnumWaypointInviteMode _inviteMode, int _inviterEntityId)
	{
		this.waypoint = _waypoint;
		this.waypoint.InviterEntityId = _inviterEntityId;
		this.inviteMode = _inviteMode;
		this.inviterEntityId = _inviterEntityId;
		return this;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0018BEE4 File Offset: 0x0018A0E4
	public override void read(PooledBinaryReader _br)
	{
		this.waypoint = new Waypoint();
		this.waypoint.Read(_br, 7);
		this.inviteMode = (EnumWaypointInviteMode)_br.ReadByte();
		this.inviterEntityId = _br.ReadInt32();
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0018BF16 File Offset: 0x0018A116
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.waypoint.Write(_bw);
		_bw.Write((byte)this.inviteMode);
		_bw.Write(this.inviterEntityId);
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0018BF44 File Offset: 0x0018A144
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

	// Token: 0x040032B5 RID: 12981
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint waypoint;

	// Token: 0x040032B6 RID: 12982
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumWaypointInviteMode inviteMode;

	// Token: 0x040032B7 RID: 12983
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;
}
