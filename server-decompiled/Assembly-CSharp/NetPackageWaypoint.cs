using System;
using UnityEngine.Scripting;

// Token: 0x0200085E RID: 2142
[Preserve]
public class NetPackageWaypoint : NetPackage
{
	// Token: 0x06003E87 RID: 16007 RVA: 0x00189F78 File Offset: 0x00188178
	public NetPackageWaypoint Setup(Waypoint _waypoint, EnumWaypointInviteMode _inviteMode, int _inviterEntityId)
	{
		this.waypoint = _waypoint;
		this.waypoint.InviterEntityId = _inviterEntityId;
		this.inviteMode = _inviteMode;
		this.inviterEntityId = _inviterEntityId;
		return this;
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x00189F9C File Offset: 0x0018819C
	public override void read(PooledBinaryReader _br)
	{
		this.waypoint = new Waypoint();
		this.waypoint.Read(_br, 7);
		this.inviteMode = (EnumWaypointInviteMode)_br.ReadByte();
		this.inviterEntityId = _br.ReadInt32();
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x00189FCE File Offset: 0x001881CE
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.waypoint.Write(_bw);
		_bw.Write((byte)this.inviteMode);
		_bw.Write(this.inviterEntityId);
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x00189FFC File Offset: 0x001881FC
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

	// Token: 0x06003E8B RID: 16011 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04003249 RID: 12873
	[PublicizedFrom(EAccessModifier.Private)]
	public Waypoint waypoint;

	// Token: 0x0400324A RID: 12874
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumWaypointInviteMode inviteMode;

	// Token: 0x0400324B RID: 12875
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;
}
