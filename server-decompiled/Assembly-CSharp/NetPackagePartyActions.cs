using System;
using UnityEngine.Scripting;

// Token: 0x0200081B RID: 2075
[Preserve]
public class NetPackagePartyActions : NetPackage
{
	// Token: 0x06003CDB RID: 15579 RVA: 0x001847FC File Offset: 0x001829FC
	public NetPackagePartyActions Setup(NetPackagePartyActions.PartyActions _operation, int _invitedByEntityID, int _invitedEntityID, int[] _partyMembers = null, string _voiceLobbyId = null)
	{
		this.currentOperation = _operation;
		this.invitedByEntityID = _invitedByEntityID;
		this.invitedEntityID = _invitedEntityID;
		this.partyMembers = _partyMembers;
		this.voiceLobbyId = _voiceLobbyId;
		return this;
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x00184824 File Offset: 0x00182A24
	public override void read(PooledBinaryReader _br)
	{
		this.currentOperation = (NetPackagePartyActions.PartyActions)_br.ReadByte();
		this.invitedByEntityID = _br.ReadInt32();
		this.invitedEntityID = _br.ReadInt32();
		this.voiceLobbyId = _br.ReadString();
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x00184858 File Offset: 0x00182A58
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.currentOperation);
		_bw.Write(this.invitedByEntityID);
		_bw.Write(this.invitedEntityID);
		_bw.Write(this.voiceLobbyId ?? "");
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x001848A8 File Offset: 0x00182AA8
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

	// Token: 0x04003177 RID: 12663
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedByEntityID;

	// Token: 0x04003178 RID: 12664
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedEntityID;

	// Token: 0x04003179 RID: 12665
	[PublicizedFrom(EAccessModifier.Private)]
	public string voiceLobbyId;

	// Token: 0x0400317A RID: 12666
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyActions.PartyActions currentOperation;

	// Token: 0x0400317B RID: 12667
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x0200081C RID: 2076
	public enum PartyActions
	{
		// Token: 0x0400317D RID: 12669
		SendInvite,
		// Token: 0x0400317E RID: 12670
		AcceptInvite,
		// Token: 0x0400317F RID: 12671
		ChangeLead,
		// Token: 0x04003180 RID: 12672
		LeaveParty,
		// Token: 0x04003181 RID: 12673
		KickFromParty,
		// Token: 0x04003182 RID: 12674
		Disconnected,
		// Token: 0x04003183 RID: 12675
		JoinAutoParty,
		// Token: 0x04003184 RID: 12676
		SetVoiceLobby
	}
}
