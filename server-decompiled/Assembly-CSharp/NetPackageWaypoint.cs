using System;
using UnityEngine.Scripting;

// Token: 0x02000877 RID: 2167
[Preserve]
public class NetPackageWaypoint : NetPackage
{
	// Token: 0x06003EA7 RID: 16039 RVA: 0x0018CF68 File Offset: 0x0018B168
	public NetPackageWaypoint Setup(Waypoint _waypoint, EnumWaypointInviteMode _inviteMode, int _inviterEntityId)
	{
		this.waypoint = _waypoint;
		this.waypoint.InviterEntityId = _inviterEntityId;
		this.inviteMode = _inviteMode;
		this.inviterEntityId = _inviterEntityId;
		return this;
	}

	// Token: 0x06003EA8 RID: 16040 RVA: 0x0018CF8C File Offset: 0x0018B18C
	public override void read(PooledBinaryReader _br)
	{
		this.waypoint = new Waypoint();
		this.waypoint.Read(_br, 7);
		this.inviteMode = (EnumWaypointInviteMode)_br.ReadByte();
		this.inviterEntityId = _br.ReadInt32();
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x0018CFBE File Offset: 0x0018B1BE
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.waypoint.Write(_bw);
		_bw.Write((byte)this.inviteMode);
		_bw.Write(this.inviterEntityId);
	}

	// Token: 0x06003EAA RID: 16042 RVA: 0x0018CFEC File Offset: 0x0018B1EC
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

	// Token: 0x040032D5 RID: 13013
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint waypoint;

	// Token: 0x040032D6 RID: 13014
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumWaypointInviteMode inviteMode;

	// Token: 0x040032D7 RID: 13015
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;
}
