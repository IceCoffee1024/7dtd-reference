using System;
using UnityEngine.Scripting;

// Token: 0x02000812 RID: 2066
[Preserve]
public class NetPackagePartyActions : NetPackage
{
	// Token: 0x06003CB8 RID: 15544 RVA: 0x00182858 File Offset: 0x00180A58
	public NetPackagePartyActions Setup(NetPackagePartyActions.PartyActions _operation, int _invitedByEntityID, int _invitedEntityID, int[] _partyMembers = null, string _voiceLobbyId = null)
	{
		this.currentOperation = _operation;
		this.invitedByEntityID = _invitedByEntityID;
		this.invitedEntityID = _invitedEntityID;
		this.partyMembers = _partyMembers;
		this.voiceLobbyId = _voiceLobbyId;
		return this;
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x00182880 File Offset: 0x00180A80
	public override void read(PooledBinaryReader _br)
	{
		this.currentOperation = (NetPackagePartyActions.PartyActions)_br.ReadByte();
		this.invitedByEntityID = _br.ReadInt32();
		this.invitedEntityID = _br.ReadInt32();
		this.voiceLobbyId = _br.ReadString();
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x001828B4 File Offset: 0x00180AB4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.currentOperation);
		_bw.Write(this.invitedByEntityID);
		_bw.Write(this.invitedEntityID);
		_bw.Write(this.voiceLobbyId ?? "");
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x00182904 File Offset: 0x00180B04
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.invitedEntityID) as EntityPlayer;
		EntityPlayer entityPlayer2 = _world.GetEntity(this.invitedByEntityID) as EntityPlayer;
		if (entityPlayer == null || entityPlayer2 == null)
		{
			return;
		}
		switch (this.currentOperation)
		{
		case NetPackagePartyActions.PartyActions.SendInvite:
			if (entityPlayer2.HasPendingPartyInvite(this.invitedEntityID))
			{
				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.AcceptInvite, this.invitedEntityID, this.invitedByEntityID, null, null), false);
					return;
				}
				if (!entityPlayer2.IsInParty())
				{
					Party.ServerHandleAcceptInvite(entityPlayer, entityPlayer2);
					return;
				}
				if (!entityPlayer.IsInParty())
				{
					Party.ServerHandleAcceptInvite(entityPlayer2, entityPlayer);
					return;
				}
			}
			else if (!entityPlayer.IsInParty())
			{
				entityPlayer.AddPartyInvite(this.invitedByEntityID);
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.SendInvite, this.invitedByEntityID, this.invitedEntityID, null, null), false, -1, -1, -1, null, 192, false);
				}
				EntityPlayerLocal entityPlayerLocal = entityPlayer as EntityPlayerLocal;
				if (entityPlayerLocal != null)
				{
					GameManager.ShowTooltip(entityPlayerLocal, string.Format(Localization.Get("ttPartyInviteReceived", false, null), entityPlayer2.PlayerDisplayName), null, "party_invite_receive", null, false, false, 0f);
					return;
				}
			}
			break;
		case NetPackagePartyActions.PartyActions.AcceptInvite:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && entityPlayer.Party == null)
			{
				Party.ServerHandleAcceptInvite(entityPlayer2, entityPlayer);
				return;
			}
			break;
		case NetPackagePartyActions.PartyActions.ChangeLead:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Party.ServerHandleChangeLead(entityPlayer);
				return;
			}
			break;
		case NetPackagePartyActions.PartyActions.LeaveParty:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && entityPlayer.Party != null)
			{
				Party.ServerHandleLeaveParty(entityPlayer, this.invitedEntityID);
				return;
			}
			break;
		case NetPackagePartyActions.PartyActions.KickFromParty:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && entityPlayer.Party != null)
			{
				Party.ServerHandleKickParty(this.invitedEntityID);
				return;
			}
			break;
		case NetPackagePartyActions.PartyActions.Disconnected:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && entityPlayer.Party != null)
			{
				Party.ServerHandleDisconnectParty(entityPlayer);
				return;
			}
			break;
		case NetPackagePartyActions.PartyActions.JoinAutoParty:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Party.ServerHandleAutoJoinParty(entityPlayer);
				return;
			}
			break;
		case NetPackagePartyActions.PartyActions.SetVoiceLobby:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Party.ServerHandleSetVoiceLoby(entityPlayer, this.voiceLobbyId);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public override int GetLength()
	{
		return 9;
	}

	// Token: 0x04003114 RID: 12564
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedByEntityID;

	// Token: 0x04003115 RID: 12565
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedEntityID;

	// Token: 0x04003116 RID: 12566
	[PublicizedFrom(EAccessModifier.Private)]
	public string voiceLobbyId;

	// Token: 0x04003117 RID: 12567
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyActions.PartyActions currentOperation;

	// Token: 0x04003118 RID: 12568
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x02000813 RID: 2067
	public enum PartyActions
	{
		// Token: 0x0400311A RID: 12570
		SendInvite,
		// Token: 0x0400311B RID: 12571
		AcceptInvite,
		// Token: 0x0400311C RID: 12572
		ChangeLead,
		// Token: 0x0400311D RID: 12573
		LeaveParty,
		// Token: 0x0400311E RID: 12574
		KickFromParty,
		// Token: 0x0400311F RID: 12575
		Disconnected,
		// Token: 0x04003120 RID: 12576
		JoinAutoParty,
		// Token: 0x04003121 RID: 12577
		SetVoiceLobby
	}
}
