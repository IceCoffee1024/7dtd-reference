using System;
using System.Collections.Generic;
using Audio;
using Twitch;
using UnityEngine;

// Token: 0x020008DB RID: 2267
public class Party
{
	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x060040DD RID: 16605 RVA: 0x00197CB8 File Offset: 0x00195EB8
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

	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x060040DE RID: 16606 RVA: 0x00197CE0 File Offset: 0x00195EE0
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

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x060040DF RID: 16607 RVA: 0x00197D28 File Offset: 0x00195F28
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

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x060040E0 RID: 16608 RVA: 0x00197D68 File Offset: 0x00195F68
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

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x060040E1 RID: 16609 RVA: 0x00197DA4 File Offset: 0x00195FA4
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

	// Token: 0x060040E2 RID: 16610 RVA: 0x00197DF0 File Offset: 0x00195FF0
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

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x060040E3 RID: 16611 RVA: 0x00197E34 File Offset: 0x00196034
	// (remove) Token: 0x060040E4 RID: 16612 RVA: 0x00197E6C File Offset: 0x0019606C
	public event OnPartyMembersChanged PartyMemberAdded;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060040E5 RID: 16613 RVA: 0x00197EA4 File Offset: 0x001960A4
	// (remove) Token: 0x060040E6 RID: 16614 RVA: 0x00197EDC File Offset: 0x001960DC
	public event OnPartyMembersChanged PartyMemberRemoved;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060040E7 RID: 16615 RVA: 0x00197F14 File Offset: 0x00196114
	// (remove) Token: 0x060040E8 RID: 16616 RVA: 0x00197F4C File Offset: 0x0019614C
	public event OnPartyChanged PartyLeaderChanged;

	// Token: 0x060040E9 RID: 16617 RVA: 0x00197F84 File Offset: 0x00196184
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

	// Token: 0x060040EA RID: 16618 RVA: 0x001980C8 File Offset: 0x001962C8
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

	// Token: 0x060040EB RID: 16619 RVA: 0x00198148 File Offset: 0x00196348
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

	// Token: 0x060040EC RID: 16620 RVA: 0x001981FC File Offset: 0x001963FC
	public bool ContainsMember(EntityPlayer player)
	{
		return this.MemberList != null && this.MemberList.Contains(player);
	}

	// Token: 0x060040ED RID: 16621 RVA: 0x00198214 File Offset: 0x00196414
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

	// Token: 0x060040EE RID: 16622 RVA: 0x00198258 File Offset: 0x00196458
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

	// Token: 0x060040EF RID: 16623 RVA: 0x001982C0 File Offset: 0x001964C0
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

	// Token: 0x060040F0 RID: 16624 RVA: 0x00198324 File Offset: 0x00196524
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

	// Token: 0x060040F1 RID: 16625 RVA: 0x0019838C File Offset: 0x0019658C
	public int GetPartyXP(EntityPlayer player, int startingXP)
	{
		int num = this.MemberCountInRange(player);
		return (int)((float)startingXP * (1f - 0.1f * (float)num));
	}

	// Token: 0x060040F2 RID: 16626 RVA: 0x001983B4 File Offset: 0x001965B4
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

	// Token: 0x060040F3 RID: 16627 RVA: 0x001983F0 File Offset: 0x001965F0
	public int[] GetMemberIdArray()
	{
		int[] array = new int[this.MemberList.Count];
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			array[i] = this.MemberList[i].entityId;
		}
		return array;
	}

	// Token: 0x060040F4 RID: 16628 RVA: 0x0019843C File Offset: 0x0019663C
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

	// Token: 0x060040F5 RID: 16629 RVA: 0x00198498 File Offset: 0x00196698
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

	// Token: 0x060040F6 RID: 16630 RVA: 0x001988A4 File Offset: 0x00196AA4
	public void Disband()
	{
		for (int i = 0; i < this.MemberList.Count; i++)
		{
			this.MemberList[i].LeaveParty();
		}
		PartyManager.Current.RemoveParty(this);
	}

	// Token: 0x060040F7 RID: 16631 RVA: 0x001988E4 File Offset: 0x00196AE4
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

	// Token: 0x060040F8 RID: 16632 RVA: 0x001989E4 File Offset: 0x00196BE4
	public static void ServerHandleChangeLead(EntityPlayer newHost)
	{
		newHost.Party.SetLeader(newHost);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyData>().Setup(newHost.Party, newHost.entityId, NetPackagePartyData.PartyActions.ChangeLead, false), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x060040F9 RID: 16633 RVA: 0x00198A34 File Offset: 0x00196C34
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

	// Token: 0x060040FA RID: 16634 RVA: 0x00198A90 File Offset: 0x00196C90
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

	// Token: 0x060040FB RID: 16635 RVA: 0x00198BAC File Offset: 0x00196DAC
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

	// Token: 0x060040FC RID: 16636 RVA: 0x00198CC8 File Offset: 0x00196EC8
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

	// Token: 0x060040FD RID: 16637 RVA: 0x00198DD1 File Offset: 0x00196FD1
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool IsFull()
	{
		return this.MemberList.Count == 8;
	}

	// Token: 0x060040FE RID: 16638 RVA: 0x00198DE4 File Offset: 0x00196FE4
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

	// Token: 0x060040FF RID: 16639 RVA: 0x00198E44 File Offset: 0x00197044
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

	// Token: 0x06004100 RID: 16640 RVA: 0x00198ECC File Offset: 0x001970CC
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

	// Token: 0x06004101 RID: 16641 RVA: 0x00198F1C File Offset: 0x0019711C
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

	// Token: 0x040034FE RID: 13566
	public int LeaderIndex;

	// Token: 0x040034FF RID: 13567
	public int PartyID = -1;

	// Token: 0x04003500 RID: 13568
	public string VoiceLobbyId;

	// Token: 0x04003501 RID: 13569
	public List<EntityPlayer> MemberList = new List<EntityPlayer>();

	// Token: 0x04003505 RID: 13573
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> changedPlayers = new List<EntityPlayer>();
}
