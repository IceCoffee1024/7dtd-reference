using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CE RID: 2254
public class PartyQuests
{
	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06004153 RID: 16723 RVA: 0x00197F2E File Offset: 0x0019612E
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

	// Token: 0x06004154 RID: 16724 RVA: 0x00197F44 File Offset: 0x00196144
	public static void EnforeInstance()
	{
		PartyQuests partyQuests = PartyQuests.Instance;
	}

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06004155 RID: 16725 RVA: 0x00197F4C File Offset: 0x0019614C
	public static bool AutoShare
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoShare);
		}
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06004156 RID: 16726 RVA: 0x00197F58 File Offset: 0x00196158
	public static bool AutoAccept
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoAccept);
		}
	}

	// Token: 0x06004157 RID: 16727 RVA: 0x00197F64 File Offset: 0x00196164
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

	// Token: 0x06004158 RID: 16728 RVA: 0x00197FBE File Offset: 0x001961BE
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

	// Token: 0x06004159 RID: 16729 RVA: 0x00197FD8 File Offset: 0x001961D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameStarted(EntityPlayerLocal _newLocalPlayer)
	{
		this.localPlayer = _newLocalPlayer;
		this.localPlayer.PartyJoined += this.playerJoinedParty;
		this.localPlayer.QuestAccepted += this.newQuestAccepted;
		this.localPlayer.SharedQuestAdded += this.sharedQuestReceived;
		Log.Out(string.Format("[PartyQuests] Player registered: {0}", _newLocalPlayer));
	}

	// Token: 0x0600415A RID: 16730 RVA: 0x00198044 File Offset: 0x00196244
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

	// Token: 0x0600415B RID: 16731 RVA: 0x001980B5 File Offset: 0x001962B5
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerJoinedParty(Party _affectedParty, EntityPlayer _player)
	{
		if (PartyQuests.AutoShare)
		{
			ThreadManager.StartCoroutine(this.shareQuestsLater());
		}
	}

	// Token: 0x0600415C RID: 16732 RVA: 0x001980CA File Offset: 0x001962CA
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestsLater()
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareAllQuestsWithParty(this.localPlayer);
		yield break;
	}

	// Token: 0x0600415D RID: 16733 RVA: 0x001980D9 File Offset: 0x001962D9
	[PublicizedFrom(EAccessModifier.Private)]
	public void newQuestAccepted(Quest _q)
	{
		if (PartyQuests.AutoShare && _q.IsShareable)
		{
			PartyQuests.logQuest("Auto-sharing new quest", _q);
			ThreadManager.StartCoroutine(this.shareQuestLater(_q));
		}
	}

	// Token: 0x0600415E RID: 16734 RVA: 0x00198102 File Offset: 0x00196302
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestLater(Quest _q)
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareQuestWithParty(_q, this.localPlayer, false);
		yield break;
	}

	// Token: 0x0600415F RID: 16735 RVA: 0x00198118 File Offset: 0x00196318
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

	// Token: 0x06004160 RID: 16736 RVA: 0x00198180 File Offset: 0x00196380
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

	// Token: 0x06004161 RID: 16737 RVA: 0x001981DC File Offset: 0x001963DC
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

	// Token: 0x06004162 RID: 16738 RVA: 0x00198254 File Offset: 0x00196454
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

	// Token: 0x06004163 RID: 16739 RVA: 0x001983CC File Offset: 0x001965CC
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

	// Token: 0x040034F6 RID: 13558
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyQuests instance;

	// Token: 0x040034F7 RID: 13559
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x040034F8 RID: 13560
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSeconds sendQuestsDelay = new WaitForSeconds(0.5f);
}
