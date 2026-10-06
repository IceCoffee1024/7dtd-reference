using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004E2 RID: 1250
[Preserve]
public class EntityTrader : EntityNPC, ITrader
{
	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x0600286F RID: 10351 RVA: 0x000FB0B1 File Offset: 0x000F92B1
	// (set) Token: 0x06002870 RID: 10352 RVA: 0x000FB0B9 File Offset: 0x000F92B9
	public TraderData TraderData { get; set; }

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06002871 RID: 10353 RVA: 0x000FB0C2 File Offset: 0x000F92C2
	public int PreferredDistanceIndex
	{
		get
		{
			return this.preferredDistanceIndex;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06002872 RID: 10354 RVA: 0x000FB0CA File Offset: 0x000F92CA
	public TraderInfo TraderInfo
	{
		get
		{
			if (this.TraderData != null)
			{
				return this.TraderData.TraderInfo;
			}
			return null;
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06002873 RID: 10355 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsValidAimAssistSnapTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x000FB0E1 File Offset: 0x000F92E1
	public override void InitLocation(Vector3 _pos, Vector3 _rot)
	{
		_pos.y = Mathf.Floor(_pos.y);
		base.InitLocation(_pos, _rot);
		this.PhysicsTransform.gameObject.SetActive(false);
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x000FB110 File Offset: 0x000F9310
	public override void PostInit()
	{
		base.PostInit();
		if (this.TraderData == null)
		{
			this.TraderData = new TraderData();
		}
		this.SetupStartingItems();
		if (base.NPCInfo != null && base.NPCInfo.TraderID > 0)
		{
			this.TraderData.TraderID = base.NPCInfo.TraderID;
			this.IsGodMode.Value = true;
		}
		this.inventory.SetHoldingItemIdx(0);
		this.emodel.avatarController.SetArchetypeStance(base.NPCInfo.CurrentStance);
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x000FB1A0 File Offset: 0x000F93A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetupStartingItems()
	{
		for (int i = 0; i < this.itemsOnEnterGame.Count; i++)
		{
			ItemStack itemStack = this.itemsOnEnterGame[i];
			ItemClass forId = ItemClass.GetForId(itemStack.itemValue.type);
			if (forId.HasQuality)
			{
				itemStack.itemValue = new ItemValue(itemStack.itemValue.type, 1, 6, false, null, 1f);
			}
			else
			{
				itemStack.count = forId.MaxCount;
			}
			this.inventory.SetItem(i, itemStack);
		}
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x000FB224 File Offset: 0x000F9424
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitLocalActivationCommands(Action<EntityActivationCommand> _addCallback)
	{
		_addCallback(new EntityActivationCommand("talk", "talk", null, null));
		_addCallback(new EntityActivationCommand("trade", "map_trader", null, null));
		_addCallback(new EntityActivationCommand("remove", "x", null, null));
	}

	// Token: 0x06002878 RID: 10360 RVA: 0x000FB276 File Offset: 0x000F9476
	public void SetupActiveQuestsForPlayer(EntityPlayer player, int overrideFactionPoints = -1)
	{
		this.activeQuests = this.PopulateActiveQuests(player, -1, overrideFactionPoints);
		QuestEventManager.Current.SetupQuestList(this, player.entityId, this.activeQuests);
	}

	// Token: 0x06002879 RID: 10361 RVA: 0x000FB2A0 File Offset: 0x000F94A0
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (this.IsDead() || base.NPCInfo == null)
		{
			return false;
		}
		if (base.CommandIs(_commandName, "talk") || base.CommandIs(_commandName, "trade"))
		{
			return true;
		}
		if (base.CommandIs(_commandName, "remove"))
		{
			return GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) && !GameUtils.IsPlaytesting();
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x000FB308 File Offset: 0x000F9508
	public override string GetActivationText()
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		string text = primaryPlayer.playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + primaryPlayer.playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		return string.Format(Localization.Get("npcTooltipTalk", false, null), text, this.LocalizedEntityName);
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x000FB370 File Offset: 0x000F9570
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityActivated(EntityActivationCommand _command, EntityPlayerLocal _playerFocusing)
	{
		if (!this.TraderData.TraderInfo.IsTraderActivitiesOpen)
		{
			GameManager.ShowTooltip(_playerFocusing, this.GetNextTimeMessage(), string.Empty, "ui_denied", null, true, false, 0f);
			return;
		}
		if (_playerFocusing != null && (!_playerFocusing.PlayerUI.windowManager.IsModalWindowOpen() || _playerFocusing.PlayerUI.windowManager.GetModalWindow().Id == "radial"))
		{
			this.transientLockCommand = _command.commandId.ToString();
			LockManager.Instance.LockRequestLocal(this, 0);
		}
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x000027FC File Offset: 0x000009FC
	public override void MoveEntityHeaded(Vector3 _direction, bool _isDirAbsolute)
	{
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x000FB407 File Offset: 0x000F9607
	public string GetNextTimeMessage()
	{
		return string.Format(Localization.Get("ttNoInteractTrader", false, null), GameUtils.WorldTimeToHourMinutesString(this.TraderInfo.GetOpenTime()));
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x000FB42C File Offset: 0x000F962C
	public override void OnUpdateLive()
	{
		base.OnUpdateLive();
		if (this.questDictionary.Count == 0)
		{
			this.PopulateQuestList();
		}
		if (this.nativeCollider)
		{
			this.nativeCollider.enabled = true;
			this.nativeCollider.includeLayers = 17825792;
		}
		if (!GameManager.IsDedicatedServer)
		{
			EntityPlayerLocal primaryPlayer = this.world.GetPrimaryPlayer();
			this.emodel.SetLookAt(primaryPlayer.getHeadPosition());
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (this.traderArea == null)
			{
				this.traderArea = this.world.GetTraderAreaAt(new Vector3i(this.position));
			}
			if (this.traderArea != null)
			{
				if (this.updateTime <= 0f)
				{
					this.updateTime = Time.time + 3f;
				}
				if (Time.time > this.updateTime)
				{
					this.updateTime = Time.time + 1f;
					List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(this, new Bounds(this.position, Vector3.one * 10f));
					if (entitiesInBounds.Count > 0)
					{
						for (int i = 0; i < entitiesInBounds.Count; i++)
						{
							if (entitiesInBounds[i] is EntityNPC && entitiesInBounds[i].EntityClass == base.EntityClass)
							{
								if (entitiesInBounds[i].entityId < this.entityId)
								{
									this.IsDespawned = true;
									this.MarkToUnload();
								}
							}
							else if (entitiesInBounds[i] is EntityPlayer)
							{
								EntityPlayer entityPlayer = entitiesInBounds[i] as EntityPlayer;
								if (base.CanSee(entityPlayer))
								{
									if (this.GreetingDictionary.ContainsKey(entityPlayer))
									{
										if (Time.time < this.GreetingDictionary[entityPlayer])
										{
											this.GreetingDictionary[entityPlayer] = Time.time + EntityTrader.traderTalkDelayTime;
											goto IL_02BD;
										}
										this.GreetingDictionary[entityPlayer] = Time.time + EntityTrader.traderTalkDelayTime;
									}
									else
									{
										this.GreetingDictionary.Add(entityPlayer, Time.time + EntityTrader.traderTalkDelayTime);
									}
									int worldHour = this.world.WorldHour;
									if (this.world.isEventBloodMoon)
									{
										this.PlayVoiceSetEntry("greetbloodmoon", entityPlayer, false, true);
									}
									else if (worldHour >= 4 && worldHour <= 11)
									{
										this.PlayVoiceSetEntry("greetmorn", entityPlayer, false, true);
									}
									else if (worldHour >= 12 && worldHour <= 16)
									{
										this.PlayVoiceSetEntry("greetaft", entityPlayer, false, true);
									}
									else if (worldHour >= 17 && worldHour <= 19)
									{
										this.PlayVoiceSetEntry("greeteve", entityPlayer, false, true);
									}
									else if (worldHour >= 20)
									{
										this.PlayVoiceSetEntry("greetnightfall", entityPlayer, false, true);
									}
									else
									{
										this.PlayVoiceSetEntry("greeting", entityPlayer, false, true);
									}
									this.SendAnimReaction(1);
								}
							}
							IL_02BD:;
						}
					}
					if (this.TraderInfo == null)
					{
						return;
					}
					if (!this.traderArea.IsClosed)
					{
						if (this.TraderInfo.IsWarningTime)
						{
							if (!this.warningPlayed)
							{
								this.warningPlayed = true;
								this.traderArea.HandleWarning(this.world, this);
							}
						}
						else
						{
							this.warningPlayed = false;
						}
					}
					bool flag = !this.TraderInfo.IsOpen;
					if (this.traderArea.IsClosed != flag || this.firstTime)
					{
						bool flag2;
						if (flag)
						{
							flag2 = this.TraderInfo.ShouldPlayCloseSound;
							if (LockManager.Instance.IsLockedServer(this, 0))
							{
								LockManager.Instance.ForceUnlockLockTarget(this);
							}
						}
						else
						{
							flag2 = this.TraderInfo.ShouldPlayOpenSound;
						}
						this.firstTime = !this.traderArea.SetClosed(this.world, flag, this, flag2);
					}
				}
			}
		}
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000FB7D4 File Offset: 0x000F99D4
	public void PopulateQuestList()
	{
		if (base.NPCInfo == null || base.NPCInfo.Quests == null)
		{
			return;
		}
		this.specialQuestList = new List<QuestEntry>();
		this.questDictionary.Clear();
		for (int i = 0; i < base.NPCInfo.Quests.Count; i++)
		{
			string questID = base.NPCInfo.Quests[i].QuestID;
			if (QuestClass.GetQuest(questID).CheckCriteriaQuestGiver(this))
			{
				QuestEntry questEntry = base.NPCInfo.Quests[i];
				questEntry.QuestID = questID;
				if (questEntry.QuestClass.UniqueKey == "")
				{
					if (!this.questDictionary.ContainsKey((int)questEntry.QuestClass.DifficultyTier))
					{
						this.questDictionary.Add((int)questEntry.QuestClass.DifficultyTier, new List<QuestEntry>());
					}
					this.questDictionary[(int)questEntry.QuestClass.DifficultyTier].Add(questEntry);
				}
				else
				{
					this.specialQuestList.Add(questEntry);
				}
			}
		}
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x000FB8E4 File Offset: 0x000F9AE4
	public bool UpdateLocations(int tier, List<Vector2> pois)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			List<PrefabInstance> prefabsForTrader = QuestEventManager.Current.GetPrefabsForTrader(this.traderArea, tier, i, null);
			num += ((prefabsForTrader != null) ? prefabsForTrader.Count : 0);
		}
		return pois.Count >= num;
	}

	// Token: 0x06002881 RID: 10369 RVA: 0x000FB930 File Offset: 0x000F9B30
	public void SetActiveQuests(EntityPlayer player, NetPackageNPCQuestList.QuestPacketEntry[] questList)
	{
		if (this.activeQuests == null)
		{
			this.activeQuests = new List<Quest>();
		}
		this.activeQuests.Clear();
		if (questList != null)
		{
			for (int i = 0; i < questList.Length; i++)
			{
				Quest quest = QuestClass.GetQuest(questList[i].QuestID).CreateQuest();
				quest.QuestGiverID = this.entityId;
				quest.QuestFaction = base.NPCInfo.QuestFaction;
				quest.SetPosition(this, questList[i].QuestLocation, questList[i].QuestSize);
				quest.SetPositionData(Quest.PositionDataTypes.QuestGiver, this.position);
				quest.SetPositionData(Quest.PositionDataTypes.TraderPosition, questList[i].TraderPos);
				quest.DataVariables.Add("POIName", Localization.Get(questList[i].POIName, false, null));
				this.activeQuests.Add(quest);
			}
		}
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x000FBA18 File Offset: 0x000F9C18
	public void ClearActiveQuests(int playerID)
	{
		try
		{
			this.activeQuests = null;
		}
		catch
		{
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			QuestEventManager.Current.ClearQuestListForPlayer(this.entityId, playerID);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageNPCQuestList>().Setup(this.entityId, playerID), false);
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x000FBA7C File Offset: 0x000F9C7C
	public List<Quest> PopulateActiveQuests(EntityPlayer player, int currentTier = -1, int questFactionPoints = -1)
	{
		if (this.questDictionary.Count == 0)
		{
			this.PopulateQuestList();
			if (this.questDictionary.Count == 0)
			{
				return null;
			}
		}
		bool @bool = GameStats.GetBool(EnumGameStats.EnemySpawnMode);
		List<Quest> list = new List<Quest>();
		this.uniqueKeysUsed.Clear();
		Vector2 vector;
		if (this.traderArea != null)
		{
			vector = new Vector2((float)this.traderArea.Position.x, (float)this.traderArea.Position.z);
		}
		else
		{
			vector = new Vector2(this.position.x, this.position.z);
		}
		if (currentTier == -1)
		{
			currentTier = player.QuestJournal.GetCurrentFactionTier(base.NPCInfo.QuestFaction, 0, false);
		}
		if (questFactionPoints == -1)
		{
			questFactionPoints = player.QuestJournal.GlobalFactionPoints;
		}
		QuestTraderData traderData = player.QuestJournal.GetTraderData(vector);
		if (traderData != null)
		{
			traderData.CheckReset(player);
		}
		this.usedPOILocations.Clear();
		List<QuestEntry> list2 = new List<QuestEntry>();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 1; i <= currentTier; i++)
		{
			dictionary.Clear();
			List<Vector2> usedPOIs = player.QuestJournal.GetUsedPOIs(vector, i);
			if (usedPOIs != null)
			{
				if (this.UpdateLocations(i, usedPOIs))
				{
					if (traderData != null)
					{
						traderData.ClearTier(i);
						if (!(player is EntityPlayerLocal))
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNPCQuestList>().SetupClear(player.entityId, vector, i), false, player.entityId, -1, -1, null, 192, false);
						}
					}
				}
				else
				{
					this.usedPOILocations.AddRange(usedPOIs);
				}
			}
			list2.Clear();
			List<QuestEntry> list3 = this.questDictionary[i];
			for (int j = 0; j < list3.Count; j++)
			{
				QuestEntry questEntry = list3[j];
				if ((questEntry.StartStage == -1 || questEntry.StartStage <= questFactionPoints) && (questEntry.EndStage == -1 || questEntry.EndStage >= questFactionPoints) && questEntry.CheckRequirement(player))
				{
					list2.Add(questEntry);
				}
			}
			int num = 0;
			int num2 = 0;
			while (num2 < 100 && list2.Count != 0)
			{
				this.preferredDistanceIndex = this.distanceIndices[num];
				int num3 = this.rand.RandomRange(list2.Count);
				QuestEntry questEntry2 = list2[num3];
				QuestClass questClass = questEntry2.QuestClass;
				bool flag = false;
				int num4;
				if (dictionary.TryGetValue(questClass.Name, out num4))
				{
					if (questClass.MaxQuestCount == 0 || num4 < questClass.MaxQuestCount)
					{
						dictionary[questClass.Name] = num4 + 1;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					dictionary[questClass.Name] = 1;
				}
				if (!flag && this.rand.RandomFloat < questEntry2.Prob)
				{
					Quest quest = questClass.CreateQuest();
					quest.QuestGiverID = this.entityId;
					quest.QuestFaction = base.NPCInfo.QuestFaction;
					quest.SetPositionData(Quest.PositionDataTypes.QuestGiver, this.position);
					quest.SetPositionData(Quest.PositionDataTypes.TraderPosition, (this.traderArea != null) ? this.traderArea.Position : this.position);
					quest.SetupTags();
					if (@bool || !quest.QuestTags.Test_AnySet(QuestEventManager.clearTag))
					{
						if (quest.SetupPosition(this, player, this.usedPOILocations, player.entityId))
						{
							this.preferredDistanceIndex = (this.preferredDistanceIndex + 1) % 3;
							if (questClass.SingleQuest)
							{
								list2.RemoveAt(num3);
							}
							list.Add(quest);
							num++;
						}
						if (quest.QuestTags.Test_AnySet(QuestEventManager.treasureTag) && GameSparksCollector.CollectGamePlayData)
						{
							GameSparksCollector.IncrementCounter(GameSparksCollector.GSDataKey.QuestOfferedDistance, ((int)Vector3.Distance(quest.Position, this.position) / 50 * 50).ToString(), 1, true, GameSparksCollector.GSDataCollection.SessionUpdates);
						}
						if (num >= this.distanceIndices.Length)
						{
							break;
						}
					}
				}
				num2++;
			}
		}
		for (int k = 0; k < this.specialQuestList.Count; k++)
		{
			QuestEntry questEntry3 = this.specialQuestList[k];
			if ((questEntry3.StartStage == -1 || questEntry3.StartStage <= questFactionPoints) && (questEntry3.EndStage == -1 || questEntry3.EndStage >= questFactionPoints) && questEntry3.CheckRequirement(player))
			{
				list2.Add(questEntry3);
				if (questEntry3.QuestClass.UniqueKey == "" || !this.uniqueKeysUsed.Contains(questEntry3.QuestClass.UniqueKey))
				{
					QuestClass questClass2 = questEntry3.QuestClass;
					if ((int)(questClass2.DifficultyTier - 1) <= currentTier && !player.QuestJournal.FindCompletedQuest(questClass2.ID, questClass2.Repeatable ? ((int)base.NPCInfo.QuestFaction) : (-1)))
					{
						int l = 0;
						while (l < 100)
						{
							Quest quest2 = questClass2.CreateQuest();
							quest2.QuestGiverID = this.entityId;
							quest2.QuestFaction = base.NPCInfo.QuestFaction;
							quest2.SetPositionData(Quest.PositionDataTypes.QuestGiver, this.position);
							quest2.SetPositionData(Quest.PositionDataTypes.TraderPosition, (this.traderArea != null) ? this.traderArea.Position : this.position);
							quest2.SetupTags();
							if (!quest2.NeedsNPCSetPosition || quest2.SetupPosition(this, player, this.usedPOILocations, player.entityId))
							{
								list.Add(quest2);
								if (questClass2.UniqueKey != "")
								{
									this.uniqueKeysUsed.Add(questClass2.UniqueKey);
								}
								if (GameSparksCollector.CollectGamePlayData)
								{
									GameSparksCollector.IncrementCounter(GameSparksCollector.GSDataKey.QuestTraderToTraderDistance, ((int)Vector3.Distance(quest2.Position, this.position) / 50 * 50).ToString(), 1, true, GameSparksCollector.GSDataCollection.SessionUpdates);
									break;
								}
								break;
							}
							else
							{
								l++;
							}
						}
					}
				}
			}
		}
		list.Sort(delegate(Quest q0, Quest q1)
		{
			float sqrMagnitude = (q0.Position - player.position).sqrMagnitude;
			float sqrMagnitude2 = (q1.Position - player.position).sqrMagnitude;
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		});
		return list;
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000FC0BA File Offset: 0x000FA2BA
	public int GetQuestFactionPoints(EntityPlayer player)
	{
		return player.QuestJournal.GlobalFactionPoints;
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x000FC0C7 File Offset: 0x000FA2C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityTargeted(EntityAlive target)
	{
		base.OnEntityTargeted(target);
		if (!this.isEntityRemote && base.GetSpawnerSource() != EnumSpawnerSource.Dynamic && target)
		{
			this.world.aiDirector.NotifyIntentToAttack(this, target);
		}
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000FC0FC File Offset: 0x000FA2FC
	public override void ProcessDamageResponseLocal(DamageResponse _dmResponse)
	{
		if (base.NPCInfo != null && base.NPCInfo.TraderID > 0)
		{
			return;
		}
		base.SetAttackTarget((EntityAlive)GameManager.Instance.World.GetEntity(_dmResponse.Source.getEntityId()), 600);
		base.ProcessDamageResponseLocal(_dmResponse);
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanDamageEntity(int _sourceEntityId)
	{
		return false;
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000FC151 File Offset: 0x000FA351
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float _impulseScale)
	{
		if (base.NPCInfo != null && base.NPCInfo.TraderID > 0)
		{
			return 0;
		}
		return base.DamageEntity(_damageSource, _strength, _criticalHit, _impulseScale);
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000FC176 File Offset: 0x000FA376
	public override void AwardKill(EntityAlive killer)
	{
		if (base.NPCInfo != null && base.NPCInfo.TraderID > 0)
		{
			return;
		}
		base.AwardKill(killer);
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x000FC196 File Offset: 0x000FA396
	public override Vector3 GetLookVector()
	{
		if (this.lookAtPosition.Equals(Vector3.zero))
		{
			return base.GetLookVector();
		}
		return Vector3.Normalize(this.lookAtPosition - this.getHeadPosition());
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x000FC1C7 File Offset: 0x000FA3C7
	public override Ray GetLookRay()
	{
		return new Ray(this.position + new Vector3(0f, this.GetEyeHeight() * this.eyeHeightHackMod, 0f), this.GetLookVector());
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateSpeedForwardAndStrafe(Vector3 _dist, float _partialTicks)
	{
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x000FC1FC File Offset: 0x000FA3FC
	public override void PlayVoiceSetEntry(string name, EntityPlayer player, bool ignoreTime = true, bool showReactionAnim = true)
	{
		if (!TraderInfo.TraderDialog)
		{
			return;
		}
		if (this.lastVoiceTime - Time.time < 0f || ignoreTime)
		{
			string voiceSet = base.NPCInfo.VoiceSet;
			string text = base.NPCInfo.CurrentStance.ToStringCached<NPCInfo.StanceTypes>();
			if (voiceSet == "" || text == "")
			{
				return;
			}
			string text2 = (voiceSet + "_" + name).ToLower();
			Manager.StopAllSequencesOnEntity((player == null) ? this : player);
			if (player == null)
			{
				this.PlayOneShot(text2, false, false, false, null, 1f);
			}
			else if (player.isEntityRemote)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageAudioPlayInHead>().Setup(text2, true), false, player.entityId, -1, -1, null, 192, false);
			}
			else
			{
				player.PlayOneShot(text2, true, false, true, null, 1f);
			}
			if (showReactionAnim)
			{
				this.PlayAnimReaction(EntityTrader.AnimReaction.Neutral);
			}
			if (!ignoreTime)
			{
				this.lastVoiceTime = Time.time + 5f;
			}
		}
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x000FC30C File Offset: 0x000FA50C
	public void PlayAnimReaction(EntityTrader.AnimReaction reaction)
	{
		AvatarController avatarController = this.emodel.avatarController;
		if (avatarController)
		{
			avatarController.TriggerReaction((int)reaction);
		}
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000FC334 File Offset: 0x000FA534
	public void SendAnimReaction(int reaction)
	{
		List<AnimParamData> list = new List<AnimParamData>();
		list.Add(new AnimParamData(AvatarController.reactionTypeHash, AnimParamData.ValueTypes.Int, reaction));
		list.Add(new AnimParamData(AvatarController.reactionTriggerHash, AnimParamData.ValueTypes.Trigger, true));
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityAnimationData>().Setup(this.entityId, list), false, -1, -1, this.entityId, null, 192, false);
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000FC39E File Offset: 0x000FA59E
	public override bool IsSharedLock(ushort _channel)
	{
		if (_channel == 0)
		{
			return true;
		}
		if (_channel != 1)
		{
			throw new ArgumentOutOfRangeException("_channel", string.Format("Unsupported lock channel: {0}", _channel));
		}
		return false;
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000FC3C7 File Offset: 0x000FA5C7
	public override bool OnLockRequestServer(int _lockingPlayerID, PooledBinaryReader _brLocalContext, ushort _channel)
	{
		return !this.IsDead() && (this.traderArea == null || !this.traderArea.IsClosed || GameUtils.IsPlaytesting());
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000FC3F0 File Offset: 0x000FA5F0
	public override void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
		if (_channel == 0)
		{
			this.activeQuests = QuestEventManager.Current.GetQuestList(GameManager.Instance.World, this.entityId, _lockingPlayerID);
			if (this.activeQuests == null)
			{
				this.SetupActiveQuestsForPlayer(this.world.GetEntity(_lockingPlayerID) as EntityPlayer, -1);
			}
			else if (_lockingPlayerID != this.world.GetPrimaryPlayerId())
			{
				NetPackageNPCQuestList.SendQuestPacketsToPlayer(this, _lockingPlayerID);
			}
		}
		if (_channel == 1)
		{
			GameManager.Instance.traderManager.TraderInventoryRequested(this.TraderData, _lockingPlayerID);
			this.TraderData.Write(_bwServerContext);
		}
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x000FC47F File Offset: 0x000FA67F
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		if (_channel == 1)
		{
			this.TraderData.SetModified(this);
		}
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x000FC494 File Offset: 0x000FA694
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		if (_success && this.IsDead())
		{
			return false;
		}
		if (_channel == 0)
		{
			if (!_success)
			{
				return false;
			}
			LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
			this.SetNextTraderWindow(EntityTrader.TraderWindowState.Close);
			QuestEventManager.Current.NPCInteracted(this);
			QuestEventManager.Current.NPCMet(this);
			Quest nextCompletedQuest = uiforPrimaryPlayer.entityPlayer.QuestJournal.GetNextCompletedQuest(null, this.entityId);
			uiforPrimaryPlayer.xui.Trader.Trader = ((nextCompletedQuest != null) ? null : this);
			if (nextCompletedQuest != null)
			{
				uiforPrimaryPlayer.xui.Dialog.QuestTurnIn = nextCompletedQuest;
				this.SetNextTraderWindow(EntityTrader.TraderWindowState.QuestComplete);
			}
			else if (base.CommandIs(this.transientLockCommand, "talk"))
			{
				uiforPrimaryPlayer.xui.Dialog.Respondent = this;
				this.SetNextTraderWindow(EntityTrader.TraderWindowState.Dialog);
				QuestEventManager.Current.NPCInteracted(this);
			}
			else if (base.CommandIs(this.transientLockCommand, "trade"))
			{
				this.SetNextTraderWindow(EntityTrader.TraderWindowState.Trade);
			}
			else
			{
				if (!base.CommandIs(this.transientLockCommand, "remove"))
				{
					Log.Warning("[EntityTrader] Unexpected lock command '" + this.transientLockCommand + "'.");
					return false;
				}
				EntityPlayerLocal entityPlayer = uiforPrimaryPlayer.entityPlayer;
				Waypoint waypoint = ((entityPlayer != null) ? entityPlayer.Waypoints.GetLastKnownPositionWaypoint(this.entityId) : null);
				if (waypoint != null)
				{
					uiforPrimaryPlayer.entityPlayer.Waypoints.Collection.Remove(waypoint);
					NavObjectManager.Instance.UnRegisterNavObjectByPosition(waypoint.pos, "waypoint");
				}
				GameEventManager.Current.HandleAction("game_remove_entity", uiforPrimaryPlayer.entityPlayer, this, false, "", "", false, true, "", null);
			}
			this.TransitionToNextWindow();
		}
		if (_channel == 1)
		{
			LocalPlayerUI uiforPrimaryPlayer2 = LocalPlayerUI.GetUIForPrimaryPlayer();
			if (!_success)
			{
				GameManager.ShowTooltip(uiforPrimaryPlayer2.entityPlayer, Localization.Get("ttNoInteractPerson", false, null), string.Empty, "ui_denied", null, false, false, 0f);
				this.SetNextTraderWindow(EntityTrader.TraderWindowState.Close);
				this.TransitionToNextWindow();
				return false;
			}
			this.TraderData.Read(_brServerContext);
			uiforPrimaryPlayer2.xui.Trader.Trader = this;
			uiforPrimaryPlayer2.windowManager.CloseAllOpenModalWindows(null, false);
			uiforPrimaryPlayer2.windowManager.Open("trader", true);
		}
		return true;
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x000FC6C8 File Offset: 0x000FA8C8
	public void SetNextTraderWindow(EntityTrader.TraderWindowState _nextState)
	{
		this.nextWindow = _nextState;
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x000FC6D4 File Offset: 0x000FA8D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void TransitionToNextWindow()
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		EntityPlayerLocal entityPlayer = uiforPrimaryPlayer.entityPlayer;
		uiforPrimaryPlayer.entityPlayer.OverrideFOV = 30f;
		uiforPrimaryPlayer.xui.Dialog.Respondent = this;
		uiforPrimaryPlayer.xui.Trader.Trader = this;
		uiforPrimaryPlayer.xui.Dialog.KeepZoomOnClose = true;
		EntityTrader.TraderWindowState traderWindowState = this.nextWindow;
		this.nextWindow = EntityTrader.TraderWindowState.Close;
		switch (traderWindowState)
		{
		case EntityTrader.TraderWindowState.Dialog:
			XUiC_DialogWindowGroup.Open(uiforPrimaryPlayer.xui, new Action(this.TransitionToNextWindow));
			return;
		case EntityTrader.TraderWindowState.Trade:
			LockManager.Instance.UnlockRequestLocal();
			this.transientLockCommand = "trade";
			LockManager.Instance.LockRequestLocal(this, 1);
			return;
		case EntityTrader.TraderWindowState.QuestComplete:
			this.PlayVoiceSetEntry("quest_complete", entityPlayer, true, true);
			XUiC_QuestTurnInWindowGroup.Open(uiforPrimaryPlayer.xui, new Action(this.TransitionToNextWindow));
			return;
		case EntityTrader.TraderWindowState.Close:
			uiforPrimaryPlayer.xui.Dialog.Respondent = null;
			uiforPrimaryPlayer.entityPlayer.OverrideFOV = -1f;
			uiforPrimaryPlayer.xui.Dialog.KeepZoomOnClose = false;
			uiforPrimaryPlayer.xui.Trader.Trader = null;
			LockManager.Instance.UnlockRequestLocal();
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x04001E5E RID: 7774
	public float eyeHeightHackMod = 1f;

	// Token: 0x04001E5F RID: 7775
	public bool ShowWornEquipment;

	// Token: 0x04001E61 RID: 7777
	public TraderArea traderArea;

	// Token: 0x04001E62 RID: 7778
	public Dictionary<EntityPlayer, float> GreetingDictionary = new Dictionary<EntityPlayer, float>();

	// Token: 0x04001E63 RID: 7779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool firstTime = true;

	// Token: 0x04001E64 RID: 7780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateTime;

	// Token: 0x04001E65 RID: 7781
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool warningPlayed;

	// Token: 0x04001E66 RID: 7782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float traderTalkDelayTime = 90f;

	// Token: 0x04001E67 RID: 7783
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string CommandTalk = "talk";

	// Token: 0x04001E68 RID: 7784
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string CommandTrade = "trade";

	// Token: 0x04001E69 RID: 7785
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string CommandRemove = "remove";

	// Token: 0x04001E6A RID: 7786
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int preferredDistanceIndex;

	// Token: 0x04001E6B RID: 7787
	public List<QuestEntry> specialQuestList;

	// Token: 0x04001E6C RID: 7788
	public Dictionary<int, List<QuestEntry>> questDictionary = new Dictionary<int, List<QuestEntry>>();

	// Token: 0x04001E6D RID: 7789
	public List<Quest> activeQuests;

	// Token: 0x04001E6E RID: 7790
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Vector2> usedPOILocations = new List<Vector2>();

	// Token: 0x04001E6F RID: 7791
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<string> uniqueKeysUsed = new List<string>();

	// Token: 0x04001E70 RID: 7792
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public readonly int[] distanceIndices = new int[] { 0, 0, 0, 1, 2, 2, 2 };

	// Token: 0x04001E71 RID: 7793
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastVoiceTime;

	// Token: 0x04001E72 RID: 7794
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string transientLockCommand;

	// Token: 0x04001E73 RID: 7795
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityTrader.TraderWindowState nextWindow = EntityTrader.TraderWindowState.Close;

	// Token: 0x020004E3 RID: 1251
	public enum AnimReaction
	{
		// Token: 0x04001E75 RID: 7797
		Happy,
		// Token: 0x04001E76 RID: 7798
		Neutral,
		// Token: 0x04001E77 RID: 7799
		Angry
	}

	// Token: 0x020004E4 RID: 1252
	[PublicizedFrom(EAccessModifier.Private)]
	public enum LockChannel
	{
		// Token: 0x04001E79 RID: 7801
		Interaction,
		// Token: 0x04001E7A RID: 7802
		Trade
	}

	// Token: 0x020004E5 RID: 1253
	public enum TraderWindowState
	{
		// Token: 0x04001E7C RID: 7804
		Dialog,
		// Token: 0x04001E7D RID: 7805
		Trade,
		// Token: 0x04001E7E RID: 7806
		QuestComplete,
		// Token: 0x04001E7F RID: 7807
		Close
	}
}
