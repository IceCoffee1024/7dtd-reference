using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x0200081D RID: 2077
[Preserve]
public class NetPackagePartyData : NetPackage
{
	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x00184AD4 File Offset: 0x00182CD4
	public NetPackagePartyData Setup(Party _party, int _changedEntityID, NetPackagePartyData.PartyActions _partyAction, bool _disbandParty = false)
	{
		this.PartyID = _party.PartyID;
		this.LeaderIndex = _party.LeaderIndex;
		this.VoiceLobbyId = _party.VoiceLobbyId;
		this.partyMembers = _party.GetMemberIdArray();
		this.changedEntityID = _changedEntityID;
		this.partyAction = _partyAction;
		this.disbandParty = _disbandParty;
		return this;
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x00184B28 File Offset: 0x00182D28
	public override void read(PooledBinaryReader _br)
	{
		this.PartyID = _br.ReadInt32();
		this.LeaderIndex = (int)_br.ReadByte();
		this.VoiceLobbyId = _br.ReadString();
		int num = _br.ReadInt32();
		this.partyMembers = new int[num];
		for (int i = 0; i < num; i++)
		{
			this.partyMembers[i] = _br.ReadInt32();
		}
		this.changedEntityID = _br.ReadInt32();
		this.partyAction = (NetPackagePartyData.PartyActions)_br.ReadByte();
		this.disbandParty = _br.ReadBoolean();
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x00184BAC File Offset: 0x00182DAC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.PartyID);
		_bw.Write((byte)this.LeaderIndex);
		_bw.Write(this.VoiceLobbyId ?? "");
		_bw.Write(this.partyMembers.Length);
		for (int i = 0; i < this.partyMembers.Length; i++)
		{
			_bw.Write(this.partyMembers[i]);
		}
		_bw.Write(this.changedEntityID);
		_bw.Write((byte)this.partyAction);
		_bw.Write(this.disbandParty);
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x00184C44 File Offset: 0x00182E44
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		Party party = PartyManager.Current.GetParty(this.PartyID);
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		Party party2 = null;
		if (primaryPlayer != null)
		{
			party2 = primaryPlayer.Party;
		}
		if (party == null)
		{
			party = PartyManager.Current.CreateClientParty(_world, this.PartyID, this.LeaderIndex, this.partyMembers, this.VoiceLobbyId);
		}
		else
		{
			party.LeaderIndex = this.LeaderIndex;
			party.VoiceLobbyId = this.VoiceLobbyId;
			party.UpdateMemberList(_world, this.partyMembers);
		}
		if (primaryPlayer != null)
		{
			EntityPlayer entityPlayer = (EntityPlayer)_world.GetEntity(this.changedEntityID);
			if (primaryPlayer == entityPlayer)
			{
				NetPackagePartyData.PartyActions partyActions = this.partyAction;
				if (partyActions != NetPackagePartyData.PartyActions.LeaveParty)
				{
					if (partyActions == NetPackagePartyData.PartyActions.KickFromParty)
					{
						Manager.PlayInsidePlayerHead("party_leave", -1, 0f, false, false);
						GameManager.ShowTooltip(primaryPlayer, Localization.Get("ttPartyKickedFromParty", false, null), false, false, 0f);
					}
				}
				else
				{
					Manager.PlayInsidePlayerHead("party_leave", -1, 0f, false, false);
				}
			}
			else if (party2 == party && this.changedEntityID != -1)
			{
				switch (this.partyAction)
				{
				case NetPackagePartyData.PartyActions.AcceptInvite:
					entityPlayer.RemoveAllPartyInvites();
					if (entityPlayer == primaryPlayer)
					{
						GameManager.Instance.RemovePartyInvitesFromAllPlayers(entityPlayer);
						Manager.PlayInsidePlayerHead("party_join", -1, 0f, false, false);
					}
					else
					{
						Manager.PlayInsidePlayerHead("party_member_join", -1, 0f, false, false);
					}
					break;
				case NetPackagePartyData.PartyActions.LeaveParty:
					entityPlayer.Party = null;
					if (entityPlayer != primaryPlayer)
					{
						Manager.PlayInsidePlayerHead("party_member_leave", -1, 0f, false, false);
						GameManager.ShowTooltip(primaryPlayer, string.Format(Localization.Get("ttPartyOtherLeftParty", false, null), entityPlayer.PlayerDisplayName), false, false, 0f);
					}
					break;
				case NetPackagePartyData.PartyActions.KickFromParty:
					entityPlayer.Party = null;
					if (entityPlayer != primaryPlayer)
					{
						Manager.PlayInsidePlayerHead("party_member_leave", -1, 0f, false, false);
						GameManager.ShowTooltip(primaryPlayer, string.Format(Localization.Get("ttPartyOtherKickedFromParty", false, null), entityPlayer.PlayerDisplayName), false, false, 0f);
						primaryPlayer.QuestJournal.RemovePlayerFromSharedWiths(entityPlayer);
					}
					break;
				case NetPackagePartyData.PartyActions.Disconnected:
					if (entityPlayer != primaryPlayer)
					{
						Manager.PlayInsidePlayerHead("party_member_leave", -1, 0f, false, false);
						GameManager.ShowTooltip(primaryPlayer, string.Format(Localization.Get("ttPartyDisconnectedFromParty", false, null), entityPlayer.PlayerDisplayName), false, false, 0f);
						primaryPlayer.QuestJournal.RemovePlayerFromSharedWiths(entityPlayer);
					}
					break;
				case NetPackagePartyData.PartyActions.SetVoiceLobby:
				{
					for (int i = 0; i < party.MemberList.Count; i++)
					{
						party.MemberList[i].HandleOnPartyChanged();
					}
					break;
				}
				}
			}
		}
		if (this.disbandParty)
		{
			party.Disband();
		}
	}

	// Token: 0x04003185 RID: 12677
	[PublicizedFrom(EAccessModifier.Private)]
	public int PartyID;

	// Token: 0x04003186 RID: 12678
	[PublicizedFrom(EAccessModifier.Private)]
	public int LeaderIndex;

	// Token: 0x04003187 RID: 12679
	[PublicizedFrom(EAccessModifier.Private)]
	public string VoiceLobbyId;

	// Token: 0x04003188 RID: 12680
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x04003189 RID: 12681
	[PublicizedFrom(EAccessModifier.Private)]
	public int changedEntityID = -1;

	// Token: 0x0400318A RID: 12682
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyData.PartyActions partyAction;

	// Token: 0x0400318B RID: 12683
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disbandParty;

	// Token: 0x0200081E RID: 2078
	public enum PartyActions
	{
		// Token: 0x0400318D RID: 12685
		SendInvite,
		// Token: 0x0400318E RID: 12686
		AcceptInvite,
		// Token: 0x0400318F RID: 12687
		ChangeLead,
		// Token: 0x04003190 RID: 12688
		LeaveParty,
		// Token: 0x04003191 RID: 12689
		KickFromParty,
		// Token: 0x04003192 RID: 12690
		Disconnected,
		// Token: 0x04003193 RID: 12691
		AutoJoin,
		// Token: 0x04003194 RID: 12692
		SetVoiceLobby
	}
}
