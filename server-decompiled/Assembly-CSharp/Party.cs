using System;
using System.Collections.Generic;
using Audio;
using Twitch;
using UnityEngine;

// Token: 0x020008CC RID: 2252
public class Party
{
	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06004124 RID: 16676 RVA: 0x00196B0C File Offset: 0x00194D0C
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

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06004125 RID: 16677 RVA: 0x00196B34 File Offset: 0x00194D34
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

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06004126 RID: 16678 RVA: 0x00196B7C File Offset: 0x00194D7C
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

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06004127 RID: 16679 RVA: 0x00196BBC File Offset: 0x00194DBC
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

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06004128 RID: 16680 RVA: 0x00196BF8 File Offset: 0x00194DF8
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

	// Token: 0x06004129 RID: 16681 RVA: 0x00196C44 File Offset: 0x00194E44
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
	// (add) Token: 0x0600412A RID: 16682 RVA: 0x00196C88 File Offset: 0x00194E88
	// (remove) Token: 0x0600412B RID: 16683 RVA: 0x00196CC0 File Offset: 0x00194EC0
	public event OnPartyMembersChanged PartyMemberAdded;

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x0600412C RID: 16684 RVA: 0x00196CF8 File Offset: 0x00194EF8
	// (remove) Token: 0x0600412D RID: 16685 RVA: 0x00196D30 File Offset: 0x00194F30
	public event OnPartyMembersChanged PartyMemberRemoved;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x0600412E RID: 16686 RVA: 0x00196D68 File Offset: 0x00194F68
	// (remove) Token: 0x0600412F RID: 16687 RVA: 0x00196DA0 File Offset: 0x00194FA0
	public event OnPartyChanged PartyLeaderChanged;

	// Token: 0x06004130 RID: 16688 RVA: 0x00196DD8 File Offset: 0x00194FD8
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

	// Token: 0x06004131 RID: 16689 RVA: 0x00196F1C File Offset: 0x0019511C
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

	// Token: 0x06004132 RID: 16690 RVA: 0x00196F9C File Offset: 0x0019519C
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

	// Token: 0x06004133 RID: 16691 RVA: 0x00197050 File Offset: 0x00195250
	public bool ContainsMember(EntityPlayer player)
	{
		return this.MemberList != null && this.MemberList.Contains(player);
	}

	// Token: 0x06004134 RID: 16692 RVA: 0x00197068 File Offset: 0x00195268
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

	// Token: 0x06004135 RID: 16693 RVA: 0x001970AC File Offset: 0x001952AC
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

	// Token: 0x06004136 RID: 16694 RVA: 0x00197114 File Offset: 0x00195314
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

	// Token: 0x06004137 RID: 16695 RVA: 0x00197178 File Offset: 0x00195378
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

	// Token: 0x06004138 RID: 16696 RVA: 0x001971E0 File Offset: 0x001953E0
	public int GetPartyXP(EntityPlayer player, int startingXP)
	{
		int num = this.MemberCountInRange(player);
		return (int)((float)startingXP * (1f - 0.1f * (float)num));
	}

	// Token: 0x06004139 RID: 16697 RVA: 0x00197208 File Offset: 0x00195408
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

	// Token: 0x0600413A RID: 16698 RVA: 0x00197244 File Offset: 0x00195444
	public int[] GetMemberIdArray()
	{
		int[] array = new int[this.MemberList.Count];
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			array[i] = this.MemberList[i].entityId;
		}
		return array;
	}

	// Token: 0x0600413B RID: 16699 RVA: 0x00197290 File Offset: 0x00195490
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

	// Token: 0x0600413C RID: 16700 RVA: 0x001972EC File Offset: 0x001954EC
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

	// Token: 0x0600413D RID: 16701 RVA: 0x001976F8 File Offset: 0x001958F8
	public void Disband()
	{
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			this.MemberList[i].LeaveParty();
		}
		PartyManager.Current.RemoveParty(this);
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x00197738 File Offset: 0x00195938
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

	// Token: 0x0600413F RID: 16703 RVA: 0x00197838 File Offset: 0x00195A38
	public static void ServerHandleChangeLead(EntityPlayer newHost)
	{
		newHost.Party.SetLeader(newHost);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(newHost.Party, newHost.entityId, NetPackagePartyData.PartyActions.ChangeLead, false), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x00197888 File Offset: 0x00195A88
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

	// Token: 0x06004141 RID: 16705 RVA: 0x001978E4 File Offset: 0x00195AE4
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

	// Token: 0x06004142 RID: 16706 RVA: 0x00197A00 File Offset: 0x00195C00
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

	// Token: 0x06004143 RID: 16707 RVA: 0x00197B1C File Offset: 0x00195D1C
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

	// Token: 0x06004144 RID: 16708 RVA: 0x00197C25 File Offset: 0x00195E25
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool IsFull()
	{
		return this.MemberList.Count == 8;
	}

	// Token: 0x06004145 RID: 16709 RVA: 0x00197C38 File Offset: 0x00195E38
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

	// Token: 0x06004146 RID: 16710 RVA: 0x00197C98 File Offset: 0x00195E98
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

	// Token: 0x06004147 RID: 16711 RVA: 0x00197D20 File Offset: 0x00195F20
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

	// Token: 0x06004148 RID: 16712 RVA: 0x00197D70 File Offset: 0x00195F70
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

	// Token: 0x040034EA RID: 13546
	public int LeaderIndex;

	// Token: 0x040034EB RID: 13547
	public int PartyID = -1;

	// Token: 0x040034EC RID: 13548
	public string VoiceLobbyId;

	// Token: 0x040034ED RID: 13549
	public List<EntityPlayer> MemberList = new List<EntityPlayer>();

	// Token: 0x040034F1 RID: 13553
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> changedPlayers = new List<EntityPlayer>();
}
