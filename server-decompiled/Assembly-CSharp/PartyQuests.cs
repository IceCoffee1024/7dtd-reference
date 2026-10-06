using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public class PartyQuests
{
	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x060040E9 RID: 16617 RVA: 0x00198032 File Offset: 0x00196232
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

	// Token: 0x060040EA RID: 16618 RVA: 0x00198048 File Offset: 0x00196248
	public static void EnforeInstance()
	{
		PartyQuests partyQuests = PartyQuests.Instance;
	}

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x060040EB RID: 16619 RVA: 0x00198050 File Offset: 0x00196250
	public static bool AutoShare
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoShare);
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x060040EC RID: 16620 RVA: 0x0019805C File Offset: 0x0019625C
	public static bool AutoAccept
	{
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsQuestsAutoAccept);
		}
	}

	// Token: 0x060040ED RID: 16621 RVA: 0x00198068 File Offset: 0x00196268
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

	// Token: 0x060040EE RID: 16622 RVA: 0x001980C2 File Offset: 0x001962C2
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

	// Token: 0x060040EF RID: 16623 RVA: 0x001980DC File Offset: 0x001962DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameStarted(EntityPlayerLocal _newLocalPlayer)
	{
		this.localPlayer = _newLocalPlayer;
		this.localPlayer.PartyJoined += this.playerJoinedParty;
		this.localPlayer.QuestAccepted += this.newQuestAccepted;
		this.localPlayer.SharedQuestAdded += this.sharedQuestReceived;
		Log.Out(string.Format("[PartyQuests] Player registered: {0}", _newLocalPlayer));
	}

	// Token: 0x060040F0 RID: 16624 RVA: 0x00198148 File Offset: 0x00196348
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

	// Token: 0x060040F1 RID: 16625 RVA: 0x001981B9 File Offset: 0x001963B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerJoinedParty(Party _affectedParty, EntityPlayer _player)
	{
		if (PartyQuests.AutoShare)
		{
			ThreadManager.StartCoroutine(this.shareQuestsLater());
		}
	}

	// Token: 0x060040F2 RID: 16626 RVA: 0x001981CE File Offset: 0x001963CE
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestsLater()
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareAllQuestsWithParty(this.localPlayer);
		yield break;
	}

	// Token: 0x060040F3 RID: 16627 RVA: 0x001981DD File Offset: 0x001963DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void newQuestAccepted(Quest _q)
	{
		if (PartyQuests.AutoShare && _q.IsShareable)
		{
			PartyQuests.logQuest("Auto-sharing new quest", _q);
			ThreadManager.StartCoroutine(this.shareQuestLater(_q));
		}
	}

	// Token: 0x060040F4 RID: 16628 RVA: 0x00198206 File Offset: 0x00196406
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator shareQuestLater(Quest _q)
	{
		yield return PartyQuests.sendQuestsDelay;
		PartyQuests.ShareQuestWithParty(_q, this.localPlayer, false);
		yield break;
	}

	// Token: 0x060040F5 RID: 16629 RVA: 0x0019821C File Offset: 0x0019641C
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

	// Token: 0x060040F6 RID: 16630 RVA: 0x00198284 File Offset: 0x00196484
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

	// Token: 0x060040F7 RID: 16631 RVA: 0x001982E0 File Offset: 0x001964E0
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

	// Token: 0x060040F8 RID: 16632 RVA: 0x00198358 File Offset: 0x00196558
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
					GameManager.Instance.QuestShareServer(new NetPackageSharedQuest.SharedQuestData(_selectedQuest.QuestUniqueId, _selectedQuest.QuestCode, _selectedQuest.ID, _selectedQuest.GetPOIName(), _selectedQuest.GetLocation(), _selectedQuest.GetLocationSize(), vector, _localPlayer.entityId, entityPlayer.entityId, _selectedQuest.QuestGiverID));
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

	// Token: 0x060040F9 RID: 16633 RVA: 0x001984D8 File Offset: 0x001966D8
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

	// Token: 0x040034EA RID: 13546
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyQuests instance;

	// Token: 0x040034EB RID: 13547
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x040034EC RID: 13548
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSeconds sendQuestsDelay = new WaitForSeconds(0.5f);
}
