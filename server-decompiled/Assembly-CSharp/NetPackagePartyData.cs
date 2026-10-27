using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000827 RID: 2087
[Preserve]
public class NetPackagePartyData : NetPackage
{
	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06003D03 RID: 15619 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D04 RID: 15620 RVA: 0x00185BD8 File Offset: 0x00183DD8
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

	// Token: 0x06003D05 RID: 15621 RVA: 0x00185C2C File Offset: 0x00183E2C
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

	// Token: 0x06003D06 RID: 15622 RVA: 0x00185CB0 File Offset: 0x00183EB0
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

	// Token: 0x06003D07 RID: 15623 RVA: 0x00185D48 File Offset: 0x00183F48
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

	// Token: 0x040031A5 RID: 12709
	[PublicizedFrom(EAccessModifier.Private)]
	public int PartyID;

	// Token: 0x040031A6 RID: 12710
	[PublicizedFrom(EAccessModifier.Private)]
	public int LeaderIndex;

	// Token: 0x040031A7 RID: 12711
	[PublicizedFrom(EAccessModifier.Private)]
	public string VoiceLobbyId;

	// Token: 0x040031A8 RID: 12712
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x040031A9 RID: 12713
	[PublicizedFrom(EAccessModifier.Private)]
	public int changedEntityID = -1;

	// Token: 0x040031AA RID: 12714
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyData.PartyActions partyAction;

	// Token: 0x040031AB RID: 12715
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disbandParty;

	// Token: 0x02000828 RID: 2088
	public enum PartyActions
	{
		// Token: 0x040031AD RID: 12717
		SendInvite,
		// Token: 0x040031AE RID: 12718
		AcceptInvite,
		// Token: 0x040031AF RID: 12719
		ChangeLead,
		// Token: 0x040031B0 RID: 12720
		LeaveParty,
		// Token: 0x040031B1 RID: 12721
		KickFromParty,
		// Token: 0x040031B2 RID: 12722
		Disconnected,
		// Token: 0x040031B3 RID: 12723
		AutoJoin,
		// Token: 0x040031B4 RID: 12724
		SetVoiceLobby
	}
}
