using System;
using UnityEngine.Scripting;

// Token: 0x0200081D RID: 2077
[Preserve]
public class NetPackagePartyActions : NetPackage
{
	// Token: 0x06003D17 RID: 15639 RVA: 0x00184C30 File Offset: 0x00182E30
	public NetPackagePartyActions Setup(NetPackagePartyActions.PartyActions _operation, int _invitedByEntityID, int _invitedEntityID, int[] _partyMembers = null, string _voiceLobbyId = null)
	{
		this.currentOperation = _operation;
		this.invitedByEntityID = _invitedByEntityID;
		this.invitedEntityID = _invitedEntityID;
		this.partyMembers = _partyMembers;
		this.voiceLobbyId = _voiceLobbyId;
		return this;
	}

	// Token: 0x06003D18 RID: 15640 RVA: 0x00184C58 File Offset: 0x00182E58
	public override void read(PooledBinaryReader _br)
	{
		this.currentOperation = (NetPackagePartyActions.PartyActions)_br.ReadByte();
		this.invitedByEntityID = _br.ReadInt32();
		this.invitedEntityID = _br.ReadInt32();
		this.voiceLobbyId = _br.ReadString();
	}

	// Token: 0x06003D19 RID: 15641 RVA: 0x00184C8C File Offset: 0x00182E8C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.currentOperation);
		_bw.Write(this.invitedByEntityID);
		_bw.Write(this.invitedEntityID);
		_bw.Write(this.voiceLobbyId ?? "");
	}

	// Token: 0x06003D1A RID: 15642 RVA: 0x00184CDC File Offset: 0x00182EDC
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

	// Token: 0x06003D1B RID: 15643 RVA: 0x00131719 File Offset: 0x0012F919
	public override int GetLength()
	{
		return 9;
	}

	// Token: 0x04003193 RID: 12691
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedByEntityID;

	// Token: 0x04003194 RID: 12692
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedEntityID;

	// Token: 0x04003195 RID: 12693
	[PublicizedFrom(EAccessModifier.Private)]
	public string voiceLobbyId;

	// Token: 0x04003196 RID: 12694
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyActions.PartyActions currentOperation;

	// Token: 0x04003197 RID: 12695
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x0200081E RID: 2078
	public enum PartyActions
	{
		// Token: 0x04003199 RID: 12697
		SendInvite,
		// Token: 0x0400319A RID: 12698
		AcceptInvite,
		// Token: 0x0400319B RID: 12699
		ChangeLead,
		// Token: 0x0400319C RID: 12700
		LeaveParty,
		// Token: 0x0400319D RID: 12701
		KickFromParty,
		// Token: 0x0400319E RID: 12702
		Disconnected,
		// Token: 0x0400319F RID: 12703
		JoinAutoParty,
		// Token: 0x040031A0 RID: 12704
		SetVoiceLobby
	}
}
