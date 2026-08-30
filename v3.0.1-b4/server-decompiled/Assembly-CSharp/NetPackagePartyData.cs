using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000814 RID: 2068
[Preserve]
public class NetPackagePartyData : NetPackage
{
	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06003CBE RID: 15550 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x00182B30 File Offset: 0x00180D30
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

	// Token: 0x06003CC0 RID: 15552 RVA: 0x00182B84 File Offset: 0x00180D84
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

	// Token: 0x06003CC1 RID: 15553 RVA: 0x00182C08 File Offset: 0x00180E08
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

	// Token: 0x06003CC2 RID: 15554 RVA: 0x00182CA0 File Offset: 0x00180EA0
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

	// Token: 0x06003CC3 RID: 15555 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public override int GetLength()
	{
		return 9;
	}

	// Token: 0x04003122 RID: 12578
	[PublicizedFrom(EAccessModifier.Private)]
	public int PartyID;

	// Token: 0x04003123 RID: 12579
	[PublicizedFrom(EAccessModifier.Private)]
	public int LeaderIndex;

	// Token: 0x04003124 RID: 12580
	[PublicizedFrom(EAccessModifier.Private)]
	public string VoiceLobbyId;

	// Token: 0x04003125 RID: 12581
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x04003126 RID: 12582
	[PublicizedFrom(EAccessModifier.Private)]
	public int changedEntityID = -1;

	// Token: 0x04003127 RID: 12583
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyData.PartyActions partyAction;

	// Token: 0x04003128 RID: 12584
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disbandParty;

	// Token: 0x02000815 RID: 2069
	public enum PartyActions
	{
		// Token: 0x0400312A RID: 12586
		SendInvite,
		// Token: 0x0400312B RID: 12587
		AcceptInvite,
		// Token: 0x0400312C RID: 12588
		ChangeLead,
		// Token: 0x0400312D RID: 12589
		LeaveParty,
		// Token: 0x0400312E RID: 12590
		KickFromParty,
		// Token: 0x0400312F RID: 12591
		Disconnected,
		// Token: 0x04003130 RID: 12592
		AutoJoin,
		// Token: 0x04003131 RID: 12593
		SetVoiceLobby
	}
}
