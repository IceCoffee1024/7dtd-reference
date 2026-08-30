using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x0200081F RID: 2079
[Preserve]
public class NetPackagePartyData : NetPackage
{
	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06003D1D RID: 15645 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x00184F08 File Offset: 0x00183108
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

	// Token: 0x06003D1F RID: 15647 RVA: 0x00184F5C File Offset: 0x0018315C
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

	// Token: 0x06003D20 RID: 15648 RVA: 0x00184FE0 File Offset: 0x001831E0
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

	// Token: 0x06003D21 RID: 15649 RVA: 0x00185078 File Offset: 0x00183278
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

	// Token: 0x06003D22 RID: 15650 RVA: 0x00131719 File Offset: 0x0012F919
	public override int GetLength()
	{
		return 9;
	}

	// Token: 0x040031A1 RID: 12705
	[PublicizedFrom(EAccessModifier.Private)]
	public int PartyID;

	// Token: 0x040031A2 RID: 12706
	[PublicizedFrom(EAccessModifier.Private)]
	public int LeaderIndex;

	// Token: 0x040031A3 RID: 12707
	[PublicizedFrom(EAccessModifier.Private)]
	public string VoiceLobbyId;

	// Token: 0x040031A4 RID: 12708
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] partyMembers;

	// Token: 0x040031A5 RID: 12709
	[PublicizedFrom(EAccessModifier.Private)]
	public int changedEntityID = -1;

	// Token: 0x040031A6 RID: 12710
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackagePartyData.PartyActions partyAction;

	// Token: 0x040031A7 RID: 12711
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disbandParty;

	// Token: 0x02000820 RID: 2080
	public enum PartyActions
	{
		// Token: 0x040031A9 RID: 12713
		SendInvite,
		// Token: 0x040031AA RID: 12714
		AcceptInvite,
		// Token: 0x040031AB RID: 12715
		ChangeLead,
		// Token: 0x040031AC RID: 12716
		LeaveParty,
		// Token: 0x040031AD RID: 12717
		KickFromParty,
		// Token: 0x040031AE RID: 12718
		Disconnected,
		// Token: 0x040031AF RID: 12719
		AutoJoin,
		// Token: 0x040031B0 RID: 12720
		SetVoiceLobby
	}
}
