using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C2 RID: 2242
public class PartyQuests
{
	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x060040EE RID: 16622 RVA: 0x00195E56 File Offset: 0x00194056
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

	// Token: 0x060040EF RID: 16623 RVA: 0x00195E6C File Offset: 0x0019406C
	public static void EnforeInstance()
	{
		PartyQuests partyQuests = PartyQuests.Instance;
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x060040F0 RID: 16624 RVA: 0x00195E74 File Offset: 0x00194074
	public static bool AutoShare
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoShare);
		}
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x060040F1 RID: 16625 RVA: 0x00195E80 File Offset: 0x00194080
	public static bool AutoAccept
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoAccept);
		}
	}

	// Token: 0x060040F2 RID: 16626 RVA: 0x00195E8C File Offset: 0x0019408C
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

	// Token: 0x060040F3 RID: 16627 RVA: 0x00195EE6 File Offset: 0x001940E6
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

	// Token: 0x060040F4 RID: 16628 RVA: 0x00195F00 File Offset: 0x00194100
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameStarted(EntityPlayerLocal _newLocalPlayer)
	{
		this.localPlayer = _newLocalPlayer;
		this.localPlayer.PartyJoined += this.playerJoinedParty;
		this.localPlayer.QuestAccepted += this.newQuestAccepted;
		this.localPlayer.SharedQuestAdded += this.sharedQuestReceived;
		Log.Out(string.Format("[PartyQuests] Player registered: {0}", _newLocalPlayer));
	}

	// Token: 0x060040F5 RID: 16629 RVA: 0x00195F6C File Offset: 0x0019416C
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

	// Token: 0x060040F6 RID: 16630 RVA: 0x00195FDD File Offset: 0x001941DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerJoinedParty(Party _affectedParty, EntityPlayer _player)
	{
		if (PartyQuests.AutoShare)
		{
			ThreadManager.StartCoroutine(this.shareQuestsLater());
		}
	}

	// Token: 0x060040F7 RID: 16631 RVA: 0x00195FF2 File Offset: 0x001941F2
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestsLater()
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareAllQuestsWithParty(this.localPlayer);
		yield break;
	}

	// Token: 0x060040F8 RID: 16632 RVA: 0x00196001 File Offset: 0x00194201
	[PublicizedFrom(EAccessModifier.Private)]
	public void newQuestAccepted(Quest _q)
	{
		if (PartyQuests.AutoShare && _q.IsShareable)
		{
			PartyQuests.logQuest("Auto-sharing new quest", _q);
			ThreadManager.StartCoroutine(this.shareQuestLater(_q));
		}
	}

	// Token: 0x060040F9 RID: 16633 RVA: 0x0019602A File Offset: 0x0019422A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestLater(Quest _q)
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareQuestWithParty(_q, this.localPlayer, false);
		yield break;
	}

	// Token: 0x060040FA RID: 16634 RVA: 0x00196040 File Offset: 0x00194240
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

	// Token: 0x060040FB RID: 16635 RVA: 0x001960A8 File Offset: 0x001942A8
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

	// Token: 0x060040FC RID: 16636 RVA: 0x00196104 File Offset: 0x00194304
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

	// Token: 0x060040FD RID: 16637 RVA: 0x0019617C File Offset: 0x0019437C
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
					GameManager.Instance.QuestShareServer(new NetPackageSharedQuest.SharedQuestData(_selectedQuest.QuestCode, _selectedQuest.ID, _selectedQuest.GetPOIName(), _selectedQuest.GetLocation(), _selectedQuest.GetLocationSize(), vector, _localPlayer.entityId, entityPlayer.entityId, _selectedQuest.QuestGiverID));
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

	// Token: 0x060040FE RID: 16638 RVA: 0x001962F4 File Offset: 0x001944F4
	public static void AcceptSharedQuest(SharedQuestEntry _sharedQuest, EntityPlayerLocal _localPlayer)
	{
		if (_sharedQuest == null)
		{
			return;
		}
		QuestJournal questJournal = _localPlayer.QuestJournal;
		Quest quest = _sharedQuest.Quest;
		quest.RemoveMapObject();
		questJournal.AddQuest(quest, true);
		questJournal.RemoveSharedQuestEntry(_sharedQuest);
		quest.AddSharedLocation(_sharedQuest.Position, _sharedQuest.Size);
		quest.SetPositionData(Quest.PositionDataTypes.QuestGiver, _sharedQuest.ReturnPos);
		quest.Position = _sharedQuest.Position;
		NetPackageSharedQuest netPackageSharedQuest = NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(quest.QuestCode, _sharedQuest.SharedByPlayerID, _localPlayer.entityId, true);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageSharedQuest, false, _sharedQuest.SharedByPlayerID, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageSharedQuest, false);
	}

	// Token: 0x04003475 RID: 13429
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyQuests instance;

	// Token: 0x04003476 RID: 13430
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04003477 RID: 13431
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSeconds sendQuestsDelay = new WaitForSeconds(0.5f);
}
