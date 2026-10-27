using System;
using UnityEngine.Scripting;

// Token: 0x02000825 RID: 2085
[Preserve]
public class NetPackagePartyActions : NetPackage
{
	// Token: 0x06003CFE RID: 15614 RVA: 0x00185900 File Offset: 0x00183B00
	public NetPackagePartyActions Setup(NetPackagePartyActions.PartyActions _operation, int _invitedByEntityID, int _invitedEntityID, int[] _partyMembers = null, string _voiceLobbyId = null)
	{
		this.currentOperation = _operation;
		this.invitedByEntityID = _invitedByEntityID;
		this.invitedEntityID = _invitedEntityID;
		this.partyMembers = _partyMembers;
		this.voiceLobbyId = _voiceLobbyId;
		return this;
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x00185928 File Offset: 0x00183B28
	public override void read(PooledBinaryReader _br)
	{
		this.currentOperation = (NetPackagePartyActions.PartyActions)_br.ReadByte();
		this.invitedByEntityID = _br.ReadInt32();
		this.invitedEntityID = _br.ReadInt32();
		this.voiceLobbyId = _br.ReadString();
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x0018595C File Offset: 0x00183B5C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.currentOperation);
		_bw.Write(this.invitedByEntityID);
		_bw.Write(this.invitedEntityID);
		_bw.Write(this.voiceLobbyId ?? "");
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x001859AC File Offset: 0x00183BAC
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

	// Token: 0x04003197 RID: 12695
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedByEntityID;

	// Token: 0x04003198 RID: 12696
	[PublicizedFrom(EAccessModifier.Private)]
	public int invitedEntityID;

	// Token: 0x04003199 RID: 12697
	[PublicizedFrom(EAccessModifier.Private)]
	public string voiceLobbyId;

	// Token: 0x0400319A RID: 12698
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyActions.PartyActions currentOperation;

	// Token: 0x0400319B RID: 12699
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x02000826 RID: 2086
	public enum PartyActions
	{
		// Token: 0x0400319D RID: 12701
		SendInvite,
		// Token: 0x0400319E RID: 12702
		AcceptInvite,
		// Token: 0x0400319F RID: 12703
		ChangeLead,
		// Token: 0x040031A0 RID: 12704
		LeaveParty,
		// Token: 0x040031A1 RID: 12705
		KickFromParty,
		// Token: 0x040031A2 RID: 12706
		Disconnected,
		// Token: 0x040031A3 RID: 12707
		JoinAutoParty,
		// Token: 0x040031A4 RID: 12708
		SetVoiceLobby
	}
}
