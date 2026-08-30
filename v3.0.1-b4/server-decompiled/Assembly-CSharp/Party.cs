using System;
using System.Collections.Generic;
using Audio;
using Twitch;
using UnityEngine;

// Token: 0x020008C0 RID: 2240
public class Party
{
	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x060040BF RID: 16575 RVA: 0x00194A34 File Offset: 0x00192C34
	public EntityPlayer Leader
	{
		get
		{
			if (this.LeaderIndex >= this.MemberList.Count)
			{
				return null;
			}
			return this.MemberList[this.LeaderIndex];
		}
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x060040C0 RID: 16576 RVA: 0x00194A5C File Offset: 0x00192C5C
	public int GameStage
	{
		get
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.MemberList.Count; i++)
			{
				EntityPlayer entityPlayer = this.MemberList[i];
				list.Add(entityPlayer.gameStage);
			}
			return GameStageDefinition.CalcPartyLevel(list);
		}
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x060040C1 RID: 16577 RVA: 0x00194AA4 File Offset: 0x00192CA4
	public int HighestGameStage
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.MemberList.Count; i++)
			{
				EntityPlayer entityPlayer = this.MemberList[i];
				num = Mathf.Max(num, entityPlayer.gameStage);
			}
			return num;
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x060040C2 RID: 16578 RVA: 0x00194AE4 File Offset: 0x00192CE4
	public bool HasTwitchMember
	{
		get
		{
			for (int i = 0; i < this.MemberList.Count; i++)
			{
				if (this.MemberList[i].TwitchEnabled)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x060040C3 RID: 16579 RVA: 0x00194B20 File Offset: 0x00192D20
	public TwitchVoteLockTypes HasTwitchVoteLock
	{
		get
		{
			for (int i = 0; i < this.MemberList.Count; i++)
			{
				if (this.MemberList[i].TwitchVoteLock != TwitchVoteLockTypes.None)
				{
					return this.MemberList[i].TwitchVoteLock;
				}
			}
			return TwitchVoteLockTypes.None;
		}
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x00194B6C File Offset: 0x00192D6C
	public int GetHighestLootStage(float containerMod, float containerBonus)
	{
		int num = 0;
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			EntityPlayer entityPlayer = this.MemberList[i];
			num = Mathf.Max(num, entityPlayer.GetLootStage(containerMod, containerBonus));
		}
		return num;
	}

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x060040C5 RID: 16581 RVA: 0x00194BB0 File Offset: 0x00192DB0
	// (remove) Token: 0x060040C6 RID: 16582 RVA: 0x00194BE8 File Offset: 0x00192DE8
	public event OnPartyMembersChanged PartyMemberAdded;

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x060040C7 RID: 16583 RVA: 0x00194C20 File Offset: 0x00192E20
	// (remove) Token: 0x060040C8 RID: 16584 RVA: 0x00194C58 File Offset: 0x00192E58
	public event OnPartyMembersChanged PartyMemberRemoved;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060040C9 RID: 16585 RVA: 0x00194C90 File Offset: 0x00192E90
	// (remove) Token: 0x060040CA RID: 16586 RVA: 0x00194CC8 File Offset: 0x00192EC8
	public event OnPartyChanged PartyLeaderChanged;

	// Token: 0x060040CB RID: 16587 RVA: 0x00194D00 File Offset: 0x00192F00
	public bool AddPlayer(EntityPlayer player)
	{
		if (this.MemberList.Contains(player))
		{
			return false;
		}
		if (this.MemberList.Count == 8)
		{
			return false;
		}
		this.MemberList.Add(player);
		player.Party = this;
		player.RemoveAllPartyInvites();
		bool flag = false;
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			if (this.MemberList[i] is EntityPlayerLocal)
			{
				flag = true;
				break;
			}
		}
		if (this.PartyMemberAdded != null)
		{
			this.PartyMemberAdded(player);
		}
		for (int j = 0; j < this.MemberList.Count; j++)
		{
			this.MemberList[j].IsInPartyOfLocalPlayer = flag;
			this.MemberList[j].HandleOnPartyJoined();
			if (this.MemberList[j].NavObject != null)
			{
				this.MemberList[j].NavObject.UseOverrideColor = true;
				this.MemberList[j].NavObject.OverrideColor = Constants.TrackedFriendColors[j % Constants.TrackedFriendColors.Length];
				this.MemberList[j].NavObject.name = this.MemberList[j].PlayerDisplayName;
			}
		}
		return true;
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x00194E44 File Offset: 0x00193044
	public bool KickPlayer(EntityPlayer player)
	{
		if (!this.MemberList.Contains(player))
		{
			return false;
		}
		if (player.NavObject != null)
		{
			player.NavObject.UseOverrideColor = false;
		}
		this.MemberList.Remove(player);
		if (this.PartyMemberRemoved != null)
		{
			this.PartyMemberRemoved(player);
		}
		player.LeaveParty();
		player.IsInPartyOfLocalPlayer = false;
		if (this.MemberList.Count == 1)
		{
			this.MemberList[0].LeaveParty();
		}
		return true;
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x00194EC4 File Offset: 0x001930C4
	public bool RemovePlayer(EntityPlayer player)
	{
		if (!this.MemberList.Contains(player))
		{
			return false;
		}
		if (player.NavObject != null)
		{
			player.NavObject.UseOverrideColor = false;
		}
		this.MemberList.Remove(player);
		player.IsInPartyOfLocalPlayer = false;
		if (this.PartyMemberRemoved != null)
		{
			this.PartyMemberRemoved(player);
		}
		if (this.MemberList.Count != 1)
		{
			return true;
		}
		if (GameStats.GetBool(EnumGameStats.AutoParty) && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.MemberList[0].entityId == GameManager.Instance.World.GetPrimaryPlayerId())
		{
			return true;
		}
		this.MemberList[0].LeaveParty();
		return true;
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x00194F78 File Offset: 0x00193178
	public bool ContainsMember(EntityPlayer player)
	{
		return this.MemberList != null && this.MemberList.Contains(player);
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x00194F90 File Offset: 0x00193190
	public bool ContainsMember(int entityID)
	{
		if (this.MemberList == null)
		{
			return false;
		}
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			if (this.MemberList[i].entityId == entityID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x00194FD4 File Offset: 0x001931D4
	[PublicizedFrom(EAccessModifier.Internal)]
	public int MemberCountInRange(EntityPlayer player)
	{
		int num = 0;
		for (int i = 0; i < player.Party.MemberList.Count; i++)
		{
			EntityPlayer entityPlayer = player.Party.MemberList[i];
			if (!(entityPlayer == player) && Vector3.Distance(player.position, entityPlayer.position) < (float)GameStats.GetInt(EnumGameStats.PartySharedKillRange))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x0019503C File Offset: 0x0019323C
	[PublicizedFrom(EAccessModifier.Internal)]
	public int MemberCountNotInRange(EntityPlayer player)
	{
		int num = 0;
		for (int i = 0; i < player.Party.MemberList.Count; i++)
		{
			EntityPlayer entityPlayer = player.Party.MemberList[i];
			if (!(entityPlayer == player) && Vector3.Distance(player.position, entityPlayer.position) >= 15f)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x001950A0 File Offset: 0x001932A0
	public int MemberCountNotWithin(EntityPlayer player, Rect poiRect)
	{
		int num = 0;
		for (int i = 0; i < player.Party.MemberList.Count; i++)
		{
			EntityPlayer entityPlayer = player.Party.MemberList[i];
			if (!(entityPlayer == player))
			{
				Vector3 position = entityPlayer.position;
				position.y = position.z;
				if (!poiRect.Contains(position))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x00195108 File Offset: 0x00193308
	public int GetPartyXP(EntityPlayer player, int startingXP)
	{
		int num = this.MemberCountInRange(player);
		return (int)((float)startingXP * (1f - 0.1f * (float)num));
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x00195130 File Offset: 0x00193330
	public bool IsLocalParty()
	{
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			if (this.MemberList[i] is EntityPlayerLocal)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x0019516C File Offset: 0x0019336C
	public int[] GetMemberIdArray()
	{
		int[] array = new int[this.MemberList.Count];
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			array[i] = this.MemberList[i].entityId;
		}
		return array;
	}

	// Token: 0x060040D6 RID: 16598 RVA: 0x001951B8 File Offset: 0x001933B8
	public List<int> GetMemberIdList(EntityPlayer exclude)
	{
		List<int> list = null;
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			if (!(this.MemberList[i] == exclude))
			{
				if (list == null)
				{
					list = new List<int>();
				}
				list.Add(this.MemberList[i].entityId);
			}
		}
		return list;
	}

	// Token: 0x060040D7 RID: 16599 RVA: 0x00195214 File Offset: 0x00193414
	[PublicizedFrom(EAccessModifier.Internal)]
	public void UpdateMemberList(World world, int[] partyMembers)
	{
		EntityPlayerLocal entityPlayerLocal = null;
		List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
		if (localPlayers != null && localPlayers.Count > 0)
		{
			entityPlayerLocal = localPlayers[0];
		}
		this.changedPlayers.Clear();
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < partyMembers.Length; j++)
			{
				if (this.MemberList[i].entityId == partyMembers[j])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.changedPlayers.Add(this.MemberList[i]);
			}
		}
		for (int k = 0; k < this.changedPlayers.Count; k++)
		{
			this.changedPlayers[k].Party = null;
			this.changedPlayers[k].IsInPartyOfLocalPlayer = false;
			this.changedPlayers[k].HandleOnPartyLeave(this);
			if (this.changedPlayers[k].NavObject != null && this.changedPlayers[k] != entityPlayerLocal)
			{
				this.changedPlayers[k].NavObject.UseOverrideColor = false;
			}
			if (entityPlayerLocal != null && entityPlayerLocal.Party == this)
			{
				entityPlayerLocal.QuestJournal.RemoveSharedQuestForOwner(this.changedPlayers[k].entityId);
				entityPlayerLocal.QuestJournal.RemoveSharedQuestEntryByOwner(this.changedPlayers[k].entityId);
			}
		}
		bool flag2 = false;
		this.changedPlayers.Clear();
		for (int l = 0; l < this.MemberList.Count; l++)
		{
			this.changedPlayers.Add(this.MemberList[l]);
		}
		this.MemberList.Clear();
		int num = 0;
		for (int m = 0; m < partyMembers.Length; m++)
		{
			bool flag = false;
			for (int n = 0; n < this.changedPlayers.Count; n++)
			{
				if (this.changedPlayers[n].entityId == partyMembers[m])
				{
					this.MemberList.Add(this.changedPlayers[n]);
					this.MemberList[num].Party = this;
					if (this.MemberList[num] is EntityPlayerLocal)
					{
						flag2 = true;
					}
					this.MemberList[num++].RemoveAllPartyInvites();
					this.changedPlayers.RemoveAt(n);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				EntityPlayer entityPlayer = world.GetEntity(partyMembers[m]) as EntityPlayer;
				if (entityPlayer != null)
				{
					this.MemberList.Add(entityPlayer);
					this.MemberList[num].Party = this;
					if (this.MemberList[num] is EntityPlayerLocal)
					{
						flag2 = true;
					}
					this.MemberList[num++].RemoveAllPartyInvites();
				}
			}
		}
		for (int num2 = 0; num2 < this.MemberList.Count; num2++)
		{
			if (entityPlayerLocal != null && num2 != this.LeaderIndex)
			{
				entityPlayerLocal.RemovePartyInvite(this.MemberList[num2].entityId);
			}
			if (this.MemberList[num2].NavObject != null && entityPlayerLocal.Party == this.MemberList[num2].Party)
			{
				this.MemberList[num2].NavObject.UseOverrideColor = true;
				this.MemberList[num2].NavObject.OverrideColor = Constants.TrackedFriendColors[num2 % Constants.TrackedFriendColors.Length];
				this.MemberList[num2].NavObject.name = this.MemberList[num2].PlayerDisplayName;
			}
			this.MemberList[num2].IsInPartyOfLocalPlayer = flag2;
			this.MemberList[num2].HandleOnPartyJoined();
		}
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x00195620 File Offset: 0x00193820
	public void Disband()
	{
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			this.MemberList[i].LeaveParty();
		}
		PartyManager.Current.RemoveParty(this);
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x00195660 File Offset: 0x00193860
	public static void ServerHandleAcceptInvite(EntityPlayer invitedBy, EntityPlayer invitedEntity)
	{
		if (invitedBy.Party == null)
		{
			PartyManager.Current.CreateParty().AddPlayer(invitedBy);
		}
		invitedBy.Party.AddPlayer(invitedEntity);
		invitedEntity.RemoveAllPartyInvites();
		List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
		if (localPlayers != null && localPlayers.Count > 0)
		{
			EntityPlayerLocal entityPlayerLocal = localPlayers[0];
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.RemovePartyInvite(invitedEntity.entityId);
				GameManager.Instance.RemovePartyInvitesFromAllPlayers(entityPlayerLocal);
				if (entityPlayerLocal != invitedEntity && entityPlayerLocal.Party != null && entityPlayerLocal.Party == invitedEntity.Party)
				{
					Manager.PlayInsidePlayerHead("party_member_join", -1, 0f, false, false);
				}
				else if (entityPlayerLocal == invitedEntity)
				{
					Manager.PlayInsidePlayerHead("party_join", -1, 0f, false, false);
				}
			}
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(invitedBy.Party, invitedEntity.entityId, NetPackagePartyData.PartyActions.AcceptInvite, false), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060040DA RID: 16602 RVA: 0x00195760 File Offset: 0x00193960
	public static void ServerHandleChangeLead(EntityPlayer newHost)
	{
		newHost.Party.SetLeader(newHost);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(newHost.Party, newHost.entityId, NetPackagePartyData.PartyActions.ChangeLead, false), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x001957B0 File Offset: 0x001939B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetLeader(EntityPlayer newHost)
	{
		this.LeaderIndex = this.MemberList.IndexOf(newHost);
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			this.MemberList[i].HandleOnPartyChanged();
		}
		if (this.PartyLeaderChanged != null)
		{
			this.PartyLeaderChanged(this, newHost);
		}
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x0019580C File Offset: 0x00193A0C
	public static void ServerHandleKickParty(int entityID)
	{
		EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(entityID) as EntityPlayer;
		if (entityPlayer.Party != null)
		{
			Party party = entityPlayer.Party;
			EntityPlayer leader = party.Leader;
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer != null)
			{
				if (primaryPlayer == entityPlayer)
				{
					Manager.PlayInsidePlayerHead("party_leave", -1, 0f, false, false);
				}
				else if (primaryPlayer.Party == party)
				{
					primaryPlayer.QuestJournal.RemoveSharedQuestForOwner(entityPlayer.entityId);
					primaryPlayer.QuestJournal.RemoveSharedQuestEntryByOwner(entityPlayer.entityId);
					primaryPlayer.QuestJournal.RemovePlayerFromSharedWiths(entityPlayer);
					Manager.PlayInsidePlayerHead("party_member_leave", -1, 0f, false, false);
				}
			}
			party.KickPlayer(entityPlayer);
			entityPlayer.LeaveParty();
			if (leader == entityPlayer)
			{
				party.LeaderIndex = 0;
			}
			else
			{
				party.SetLeader(leader);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(party, entityID, NetPackagePartyData.PartyActions.KickFromParty, party.MemberList.Count == 0), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x00195928 File Offset: 0x00193B28
	public static void ServerHandleLeaveParty(EntityPlayer player, int entityID)
	{
		if (player.Party != null)
		{
			Party party = player.Party;
			EntityPlayer leader = party.Leader;
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer != null && primaryPlayer == player)
			{
				party.ClearAllNavObjectColors();
			}
			if (primaryPlayer != null)
			{
				if (primaryPlayer == player)
				{
					Manager.PlayInsidePlayerHead("party_leave", -1, 0f, false, false);
				}
				else if (primaryPlayer.Party == party)
				{
					primaryPlayer.QuestJournal.RemoveSharedQuestForOwner(player.entityId);
					primaryPlayer.QuestJournal.RemoveSharedQuestEntryByOwner(player.entityId);
					primaryPlayer.QuestJournal.RemovePlayerFromSharedWiths(player);
					Manager.PlayInsidePlayerHead("party_member_leave", -1, 0f, false, false);
				}
			}
			party.RemovePlayer(player);
			player.LeaveParty();
			if (leader == player)
			{
				party.LeaderIndex = 0;
			}
			else
			{
				party.SetLeader(leader);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(party, entityID, NetPackagePartyData.PartyActions.LeaveParty, party.MemberList.Count == 0), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x060040DE RID: 16606 RVA: 0x00195A44 File Offset: 0x00193C44
	public static void ServerHandleDisconnectParty(EntityPlayer player)
	{
		if (player.Party != null)
		{
			Party party = player.Party;
			EntityPlayer leader = party.Leader;
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer != null)
			{
				if (primaryPlayer == player)
				{
					Manager.PlayInsidePlayerHead("party_leave", -1, 0f, false, false);
				}
				else if (primaryPlayer.Party == party)
				{
					primaryPlayer.QuestJournal.RemoveSharedQuestForOwner(player.entityId);
					primaryPlayer.QuestJournal.RemoveSharedQuestEntryByOwner(player.entityId);
					primaryPlayer.QuestJournal.RemovePlayerFromSharedWiths(player);
					Manager.PlayInsidePlayerHead("party_member_leave", -1, 0f, false, false);
				}
			}
			party.RemovePlayer(player);
			player.LeaveParty();
			if (leader == player)
			{
				party.LeaderIndex = 0;
			}
			else
			{
				party.SetLeader(leader);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(party, player.entityId, NetPackagePartyData.PartyActions.Disconnected, party.MemberList.Count == 0), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x00195B4D File Offset: 0x00193D4D
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool IsFull()
	{
		return this.MemberList.Count == 8;
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x00195B60 File Offset: 0x00193D60
	public static void ServerHandleAutoJoinParty(EntityPlayer joiningEntity)
	{
		Party party = PartyManager.Current.GetParty(1);
		if (party == null)
		{
			party = PartyManager.Current.CreateParty();
		}
		if (party.AddPlayer(joiningEntity))
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(party, joiningEntity.entityId, NetPackagePartyData.PartyActions.AutoJoin, false), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x00195BC0 File Offset: 0x00193DC0
	public static void ServerHandleSetVoiceLoby(EntityPlayer player, string voiceLobbyId)
	{
		if (player.Party != null)
		{
			Party party = player.Party;
			party.VoiceLobbyId = voiceLobbyId;
			for (int i = 0; i < party.MemberList.Count; i++)
			{
				party.MemberList[i].HandleOnPartyChanged();
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(party, player.entityId, NetPackagePartyData.PartyActions.SetVoiceLobby, party.MemberList.Count == 0), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x00195C48 File Offset: 0x00193E48
	public void ClearAllNavObjectColors()
	{
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			if (this.MemberList[i].NavObject != null)
			{
				this.MemberList[i].NavObject.UseOverrideColor = false;
			}
		}
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x00195C98 File Offset: 0x00193E98
	[PublicizedFrom(EAccessModifier.Internal)]
	public EntityPlayer GetMemberAtIndex(int index, EntityPlayer excludePlayer)
	{
		int num = 0;
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			if (this.MemberList[i] != excludePlayer)
			{
				num++;
			}
			if (num == index)
			{
				return this.MemberList[i];
			}
		}
		return null;
	}

	// Token: 0x04003469 RID: 13417
	public int LeaderIndex;

	// Token: 0x0400346A RID: 13418
	public int PartyID = -1;

	// Token: 0x0400346B RID: 13419
	public string VoiceLobbyId;

	// Token: 0x0400346C RID: 13420
	public List<EntityPlayer> MemberList = new List<EntityPlayer>();

	// Token: 0x04003470 RID: 13424
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> changedPlayers = new List<EntityPlayer>();
}
