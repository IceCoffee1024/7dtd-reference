using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008DD RID: 2269
public class PartyQuests
{
	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x0600410C RID: 16652 RVA: 0x001990DA File Offset: 0x001972DA
	public static PartyQuests Instance
	{
		get
		{
			PartyQuests partyQuests;
			if ((partyQuests = PartyQuests.instance) == null)
			{
				partyQuests = (PartyQuests.instance = new PartyQuests());
			}
			return partyQuests;
		}
	}

	// Token: 0x0600410D RID: 16653 RVA: 0x001990F0 File Offset: 0x001972F0
	public static void EnforeInstance()
	{
		PartyQuests partyQuests = PartyQuests.Instance;
	}

	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x0600410E RID: 16654 RVA: 0x001990F8 File Offset: 0x001972F8
	public static bool AutoShare
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoShare);
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x0600410F RID: 16655 RVA: 0x00199104 File Offset: 0x00197304
	public static bool AutoAccept
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoAccept);
		}
	}

	// Token: 0x06004110 RID: 16656 RVA: 0x00199110 File Offset: 0x00197310
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyQuests()
	{
		GameManager.Instance.OnLocalPlayerChanged += this.localPlayerChangedEvent;
		World world = GameManager.Instance.World;
		EntityPlayerLocal entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		if (entityPlayerLocal != null)
		{
			this.gameStarted(entityPlayerLocal);
		}
		Log.Out("[PartyQuests] Initialized");
	}

	// Token: 0x06004111 RID: 16657 RVA: 0x0019916A File Offset: 0x0019736A
	[PublicizedFrom(EAccessModifier.Private)]
	public void localPlayerChangedEvent(EntityPlayerLocal _newLocalPlayer)
	{
		if (_newLocalPlayer == null)
		{
			this.gameEnded();
			return;
		}
		this.gameStarted(_newLocalPlayer);
	}

	// Token: 0x06004112 RID: 16658 RVA: 0x00199184 File Offset: 0x00197384
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameStarted(EntityPlayerLocal _newLocalPlayer)
	{
		this.localPlayer = _newLocalPlayer;
		this.localPlayer.PartyJoined += this.playerJoinedParty;
		this.localPlayer.QuestAccepted += this.newQuestAccepted;
		this.localPlayer.SharedQuestAdded += this.sharedQuestReceived;
		Log.Out(string.Format("[PartyQuests] Player registered: {0}", _newLocalPlayer));
	}

	// Token: 0x06004113 RID: 16659 RVA: 0x001991F0 File Offset: 0x001973F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameEnded()
	{
		if (this.localPlayer != null)
		{
			this.localPlayer.PartyJoined -= this.playerJoinedParty;
			this.localPlayer.QuestAccepted -= this.newQuestAccepted;
			this.localPlayer.SharedQuestAdded -= this.sharedQuestReceived;
		}
		this.localPlayer = null;
		Log.Out("[PartyQuests] Player unregistered");
	}

	// Token: 0x06004114 RID: 16660 RVA: 0x00199261 File Offset: 0x00197461
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerJoinedParty(Party _affectedParty, EntityPlayer _player)
	{
		if (PartyQuests.AutoShare)
		{
			ThreadManager.StartCoroutine(this.shareQuestsLater());
		}
	}

	// Token: 0x06004115 RID: 16661 RVA: 0x00199276 File Offset: 0x00197476
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestsLater()
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareAllQuestsWithParty(this.localPlayer);
		yield break;
	}

	// Token: 0x06004116 RID: 16662 RVA: 0x00199285 File Offset: 0x00197485
	[PublicizedFrom(EAccessModifier.Private)]
	public void newQuestAccepted(Quest _q)
	{
		if (PartyQuests.AutoShare && _q.IsShareable)
		{
			PartyQuests.logQuest("Auto-sharing new quest", _q);
			ThreadManager.StartCoroutine(this.shareQuestLater(_q));
		}
	}

	// Token: 0x06004117 RID: 16663 RVA: 0x001992AE File Offset: 0x001974AE
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestLater(Quest _q)
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareQuestWithParty(_q, this.localPlayer, false);
		yield break;
	}

	// Token: 0x06004118 RID: 16664 RVA: 0x001992C4 File Offset: 0x001974C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void sharedQuestReceived(SharedQuestEntry _entry)
	{
		if (PartyQuests.AutoAccept)
		{
			int sharedByPlayerID = _entry.SharedByPlayerID;
			string text = "-unknown-";
			EntityPlayer entityPlayer;
			if (GameManager.Instance.World.Players.dict.TryGetValue(sharedByPlayerID, out entityPlayer))
			{
				text = entityPlayer.EntityName;
			}
			PartyQuests.logQuest("Received shared quest from " + text, _entry.Quest);
			PartyQuests.AcceptSharedQuest(_entry, this.localPlayer);
		}
	}

	// Token: 0x06004119 RID: 16665 RVA: 0x0019932C File Offset: 0x0019752C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void logQuest(string _prefix, Quest _q)
	{
		Log.Out(string.Format("[PartyQuests] {0}: id={1}, code={2}, name={3}, POI {4}", new object[]
		{
			_prefix,
			_q.ID,
			_q.QuestCode,
			_q.QuestClass.Name,
			_q.GetParsedText("{poi.name}")
		}));
	}

	// Token: 0x0600411A RID: 16666 RVA: 0x00199388 File Offset: 0x00197588
	public static void ShareAllQuestsWithParty(EntityPlayerLocal _localPlayer)
	{
		if (_localPlayer == null)
		{
			return;
		}
		foreach (Quest quest in _localPlayer.QuestJournal.quests)
		{
			if (quest.IsShareable)
			{
				PartyQuests.logQuest("Auto-sharing quest with new party", quest);
				PartyQuests.ShareQuestWithParty(quest, _localPlayer, false);
			}
		}
	}

	// Token: 0x0600411B RID: 16667 RVA: 0x00199400 File Offset: 0x00197600
	public static void ShareQuestWithParty(Quest _selectedQuest, EntityPlayerLocal _localPlayer, bool _showTooltips)
	{
		if (_selectedQuest == null)
		{
			if (_showTooltips)
			{
				GameManager.ShowTooltip(_localPlayer, Localization.Get("ttQuestShareNoQuest", false, null), false, false, 0f);
			}
			return;
		}
		if (!_selectedQuest.IsShareable)
		{
			return;
		}
		if (!_localPlayer.IsInParty())
		{
			if (_showTooltips)
			{
				GameManager.ShowTooltip(_localPlayer, Localization.Get("ttQuestShareNoParty", false, null), false, false, 0f);
			}
			return;
		}
		_selectedQuest.SetupQuestCode();
		int num = 0;
		for (int i = 0; i < _localPlayer.Party.MemberList.Count; i++)
		{
			EntityPlayer entityPlayer = _localPlayer.Party.MemberList[i];
			if (!(entityPlayer == _localPlayer))
			{
				if (_selectedQuest.HasSharedWith(entityPlayer))
				{
					if (PartyQuests.AutoShare)
					{
						Log.Out("[PartyQuests] Not sharing with party member " + entityPlayer.EntityName + ", already shared");
					}
				}
				else
				{
					Vector3 vector;
					_selectedQuest.GetPositionData(out vector, Quest.PositionDataTypes.QuestGiver);
					GameManager.Instance.QuestShareServer(new NetPackageSharedQuest.SharedQuestData(_selectedQuest.QuestUniqueId, _selectedQuest.QuestCode, _selectedQuest.ID, _selectedQuest.GetPOINameKey(), _selectedQuest.GetLocation(), _selectedQuest.GetLocationSize(), vector, _localPlayer.entityId, entityPlayer.entityId, _selectedQuest.QuestGiverID));
					num++;
					if (PartyQuests.AutoShare)
					{
						Log.Out("[PartyQuests] Shared with party member " + entityPlayer.EntityName);
					}
				}
			}
		}
		if (_showTooltips)
		{
			GameManager.ShowTooltip(_localPlayer, (num == 0) ? Localization.Get("ttQuestShareNoPartyInRange", false, null) : string.Format(Localization.Get("ttQuestShareWithParty", false, null), _selectedQuest.QuestClass.Name), false, false, 0f);
		}
	}

	// Token: 0x0600411C RID: 16668 RVA: 0x00199580 File Offset: 0x00197780
	public static void AcceptSharedQuest(SharedQuestEntry _sharedQuest, EntityPlayerLocal _localPlayer)
	{
		if (_sharedQuest == null)
		{
			return;
		}
		QuestJournal questJournal = _localPlayer.QuestJournal;
		Quest quest = _sharedQuest.Quest;
		quest.RemoveMapObject();
		questJournal.AddQuest(quest, Quest.QuestSource.PartyShare, true);
		questJournal.RemoveSharedQuestEntry(_sharedQuest);
		quest.AddSharedLocation(_sharedQuest.Position, _sharedQuest.Size);
		quest.SetPositionData(Quest.PositionDataTypes.QuestGiver, _sharedQuest.ReturnPos);
		quest.Position = _sharedQuest.Position;
		NetPackageSharedQuest netPackageSharedQuest = NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(quest.QuestUniqueId, quest.QuestCode, _sharedQuest.SharedByPlayerID, _localPlayer.entityId, true);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageSharedQuest, false, _sharedQuest.SharedByPlayerID, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageSharedQuest, false);
	}

	// Token: 0x0400350A RID: 13578
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyQuests instance;

	// Token: 0x0400350B RID: 13579
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400350C RID: 13580
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSeconds sendQuestsDelay = new WaitForSeconds(0.5f);
}
