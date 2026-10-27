using System;
using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009B0 RID: 2480
[Preserve]
public class ObjectiveRallyPoint : BaseObjective
{
	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x06004879 RID: 18553 RVA: 0x00047076 File Offset: 0x00045276
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Distance;
		}
	}

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x0600487A RID: 18554 RVA: 0x001C27D4 File Offset: 0x001C09D4
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x0600487B RID: 18555 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x0600487C RID: 18556 RVA: 0x001C7448 File Offset: 0x001C5648
	public override string StatusText
	{
		get
		{
			if (base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
			{
				return ValueDisplayFormatters.Distance(this.currentDistance);
			}
			if (base.OwnerQuest.CurrentState == Quest.QuestState.NotStarted)
			{
				return "";
			}
			if (base.ObjectiveState == BaseObjective.ObjectiveStates.Failed)
			{
				return Localization.Get("failed", false, null);
			}
			return Localization.Get("completed", false, null);
		}
	}

	// Token: 0x0600487D RID: 18557 RVA: 0x001C74A4 File Offset: 0x001C56A4
	public override void SetupObjective()
	{
		ObjectiveRallyPoint.textActivateRallyPoint = Localization.Get("ObjectiveRallyPointActivate", false, null);
		ObjectiveRallyPoint.textHeadToRallyPoint = Localization.Get("ObjectiveRallyPointHeadTo", false, null);
		ObjectiveRallyPoint.textWaitForActivate = Localization.Get("ObjectiveWaitForActivate_keyword", false, null);
		this.keyword = Localization.Get("ObjectiveBlockActivate_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		if (base.OwnerQuest.SharedOwnerID != -1)
		{
			base.Description = ObjectiveRallyPoint.textWaitForActivate;
			return;
		}
		base.Description = ObjectiveRallyPoint.textActivateRallyPoint;
	}

	// Token: 0x0600487E RID: 18558 RVA: 0x001C754F File Offset: 0x001C574F
	public override void AddHooks()
	{
		QuestEventManager.Current.BlockActivate += this.Current_BlockActivate;
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x001C7568 File Offset: 0x001C5768
	public static void SetupFlags(List<BaseObjective> objectives)
	{
		HashSet<ObjectiveRallyPointData> hashSet = new HashSet<ObjectiveRallyPointData>();
		foreach (BaseObjective baseObjective in objectives)
		{
			ObjectiveRallyPoint objectiveRallyPoint = baseObjective as ObjectiveRallyPoint;
			if (objectiveRallyPoint != null && objectiveRallyPoint.isRallyVisible)
			{
				Transform blockTransform = ObjectiveRallyPoint.getBlockTransform(objectiveRallyPoint.rallyPos);
				if (blockTransform != null)
				{
					ObjectiveRallyPointData component = blockTransform.gameObject.GetComponent<ObjectiveRallyPointData>();
					if (component != null)
					{
						Quest ownerQuest = objectiveRallyPoint.OwnerQuest;
						if (ownerQuest.CurrentPhase == objectiveRallyPoint.Phase)
						{
							if (!hashSet.Contains(component))
							{
								hashSet.Add(component);
								component.ClearAllFlags();
							}
							component.AddFlag(objectiveRallyPoint.rallyMarkerType, ownerQuest.SharedOwnerID == -1);
						}
					}
				}
			}
		}
		foreach (ObjectiveRallyPointData objectiveRallyPointData in hashSet)
		{
			objectiveRallyPointData.UpdateAllFlags();
		}
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x001C767C File Offset: 0x001C587C
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform getBlockTransform(Vector3i rallyPos)
	{
		Transform transform = null;
		Chunk chunk = GameManager.Instance.World.GetChunkFromWorldPos(rallyPos) as Chunk;
		if (chunk != null)
		{
			BlockEntityData blockEntity = chunk.GetBlockEntity(rallyPos);
			if (blockEntity != null)
			{
				transform = blockEntity.transform;
			}
		}
		return transform;
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x001C76B7 File Offset: 0x001C58B7
	[PublicizedFrom(EAccessModifier.Private)]
	public void setVisibility(Transform t, string childName, bool isVisible)
	{
		this.isRallyVisible = isVisible;
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x001C76C0 File Offset: 0x001C58C0
	public override void RemoveObjectives()
	{
		base.RemoveObjectives();
		World world = GameManager.Instance.World;
		if (this.rallyPos != Vector3i.zero)
		{
			if (this.RallyStartType == ObjectiveRallyPoint.RallyStartTypes.Find)
			{
				Chunk chunk = world.GetChunkFromWorldPos(this.rallyPos) as Chunk;
				if (chunk != null)
				{
					BlockEntityData blockEntity = chunk.GetBlockEntity(this.rallyPos);
					if (blockEntity != null && blockEntity.transform != null)
					{
						this.setVisibility(blockEntity.transform, this.rallyMarkerType, false);
						return;
					}
				}
			}
			else
			{
				if (base.OwnerQuest.SharedOwnerID == -1)
				{
					GameManager.Instance.World.SetBlockRPC(this.rallyPos, BlockValue.Air, sbyte.MaxValue);
					return;
				}
				GameManager.Instance.StartCoroutine(this.setRallyPointVisibility(false));
			}
		}
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x001C7784 File Offset: 0x001C5984
	public override void RemoveHooks()
	{
		World world = GameManager.Instance.World;
		QuestEventManager.Current.BlockActivate -= this.Current_BlockActivate;
		if (this.rallyPos != Vector3i.zero)
		{
			if (this.RallyStartType == ObjectiveRallyPoint.RallyStartTypes.Find)
			{
				bool flag = base.OwnerQuest.OwnerJournal.HasQuestAtRallyPosition(this.rallyPos.ToVector3(), true) != null && base.OwnerQuest.OwnerJournal.ActiveQuest == null;
				GameManager.Instance.StartCoroutine(this.setRallyPointVisibility(flag));
				return;
			}
			if (base.OwnerQuest.SharedOwnerID == -1)
			{
				world.SetBlockRPC(this.rallyPos, BlockValue.Air, sbyte.MaxValue);
				return;
			}
			GameManager.Instance.StartCoroutine(this.setRallyPointVisibility(false));
		}
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x001C7850 File Offset: 0x001C5A50
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator setRallyPointVisibility(bool visible)
	{
		QuestEventManager.Current.BlockActivate -= this.Current_BlockActivate;
		if (visible)
		{
			QuestEventManager.Current.BlockActivate += this.Current_BlockActivate;
		}
		BlockValue bvRallyType = Block.GetBlockValue("questRallyMarker", false);
		World world = GameManager.Instance.World;
		Chunk chunk = world.GetChunkFromWorldPos(this.rallyPos) as Chunk;
		if (chunk != null)
		{
			BlockEntityData bd = chunk.GetBlockEntity(this.rallyPos);
			if (bd != null)
			{
				while (bd.transform == null)
				{
					yield return null;
				}
				if (!visible)
				{
					ObjectiveRallyPointData component = bd.transform.gameObject.GetComponent<ObjectiveRallyPointData>();
					if (component != null)
					{
						component.RemoveFlag(this.rallyMarkerType);
						component.UpdateAllFlags();
					}
				}
				if (bd.blockValue.EqualsExceptRotation(bvRallyType))
				{
					this.setVisibility(bd.transform, this.rallyMarkerType, visible);
				}
				else
				{
					world.SetBlockRPC(this.rallyPos, bvRallyType);
				}
			}
			else
			{
				world.SetBlockRPC(this.rallyPos, bvRallyType);
			}
			bd = null;
		}
		yield break;
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x001C7868 File Offset: 0x001C5A68
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockActivate(string blockName, Vector3i blockPos)
	{
		if (base.OwnerQuest.SharedOwnerID != -1)
		{
			return;
		}
		if (base.Complete)
		{
			return;
		}
		if (base.OwnerQuest.OwnerJournal.ActiveQuest != null)
		{
			return;
		}
		if (this.rallyPos != blockPos)
		{
			return;
		}
		if (TwitchManager.HasInstance && TwitchManager.Current.IsVoting)
		{
			GameManager.ShowTooltip(base.OwnerQuest.OwnerJournal.OwnerPlayer, Localization.Get("ttWaitForVoteQuest", false, null), false, false, 0f);
			return;
		}
		Vector3 zero = Vector3.zero;
		int num = GameUtils.WorldTimeToHours(GameManager.Instance.World.worldTime);
		if (this.startTime != -1 && this.endTime != -1)
		{
			if (this.startTime < this.endTime)
			{
				if (num < this.startTime || num >= this.endTime)
				{
					GameManager.ShowTooltip(base.OwnerQuest.OwnerJournal.OwnerPlayer, string.Format(Localization.Get("ObjectiveRallyPointInvalidStartTime", false, null), this.startTime, this.endTime), false, false, 0f);
					return;
				}
			}
			else if (num < this.startTime && num >= this.endTime)
			{
				GameManager.ShowTooltip(base.OwnerQuest.OwnerJournal.OwnerPlayer, string.Format(Localization.Get("ObjectiveRallyPointInvalidStartTime", false, null), this.startTime, this.endTime), false, false, 0f);
				return;
			}
		}
		base.OwnerQuest.RemoveSharedNotInRange();
		if (!base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition))
		{
			if (base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.Location))
			{
				this.RallyPointActivate(zero, true, QuestEventManager.POILockoutReasonTypes.None, 0UL);
			}
			return;
		}
		EntityPlayer ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Vector2 vector = new Vector2(zero.x, zero.z);
			ulong num2;
			QuestEventManager.POILockoutReasonTypes poilockoutReasonTypes = QuestEventManager.Current.CheckForPOILockouts(ownerPlayer.entityId, vector, out num2);
			this.RallyPointActivate(zero, poilockoutReasonTypes == QuestEventManager.POILockoutReasonTypes.None, poilockoutReasonTypes, num2);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.TryRallyMarker, ownerPlayer.entityId, zero, base.OwnerQuest.QuestCode), false);
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x001C7A90 File Offset: 0x001C5C90
	public void RallyPointActivate(Vector3 prefabPos, bool activate, QuestEventManager.POILockoutReasonTypes lockoutReason, ulong extraData)
	{
		ObjectiveRallyPoint.<>c__DisplayClass39_0 CS$<>8__locals1 = new ObjectiveRallyPoint.<>c__DisplayClass39_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.prefabPos = prefabPos;
		bool flag = base.OwnerQuest.PositionData.ContainsKey(Quest.PositionDataTypes.POIPosition);
		if (activate)
		{
			if (base.OwnerQuest.CheckRequirements())
			{
				if (!base.OwnerQuest.QuestClass.CanActivate())
				{
					GameManager.ShowTooltip(base.OwnerQuest.OwnerJournal.OwnerPlayer, Localization.Get("questunavailable", false, null), false, false, 0f);
					return;
				}
				this.HandleParty();
				base.OwnerQuest.RemoveMapObject();
				base.OwnerQuest.RallyMarkerActivated = true;
				CS$<>8__locals1.player = base.OwnerQuest.OwnerJournal.OwnerPlayer;
				if (flag)
				{
					base.OwnerQuest.OwnerJournal.ActiveQuest = base.OwnerQuest;
					base.OwnerQuest.Tracked = true;
					base.OwnerQuest.OwnerJournal.TrackedQuest = base.OwnerQuest;
					base.OwnerQuest.OwnerJournal.RefreshTracked();
					if (base.OwnerQuest.PositionData.ContainsKey(Quest.PositionDataTypes.TraderPosition))
					{
						base.OwnerQuest.OwnerJournal.AddPOIToTraderData((int)base.OwnerQuest.QuestClass.DifficultyTier, base.OwnerQuest.PositionData[Quest.PositionDataTypes.TraderPosition], base.OwnerQuest.PositionData[Quest.PositionDataTypes.POIPosition]);
					}
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						ThreadManager.StartCoroutine(CS$<>8__locals1.<RallyPointActivate>g__RallyPointActivatedCoroutine|0());
					}
					else
					{
						ObjectiveRallyPoint.OutstandingRallyPoint = this;
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.LockPOI, CS$<>8__locals1.player.entityId, base.OwnerQuest.ID, base.OwnerQuest.QuestTags, CS$<>8__locals1.prefabPos, base.OwnerQuest.GetSharedWithIDList()), false);
					}
				}
				else
				{
					this.rallyPointActivated();
				}
				if (this.activateEvent != "")
				{
					GameEventManager.Current.HandleAction(this.activateEvent, null, CS$<>8__locals1.player, false, new Vector3i(CS$<>8__locals1.prefabPos), "", "", false, true, "", null, null);
					return;
				}
			}
		}
		else
		{
			EntityPlayer ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
			switch (lockoutReason)
			{
			case QuestEventManager.POILockoutReasonTypes.PlayerInside:
				GameManager.ShowTooltip(ownerPlayer as EntityPlayerLocal, Localization.Get("poiLockoutPlayerInside", false, null), false, false, 0f);
				return;
			case QuestEventManager.POILockoutReasonTypes.Bedroll:
				GameManager.ShowTooltip(ownerPlayer as EntityPlayerLocal, Localization.Get("poiLockoutBedroll", false, null), false, false, 0f);
				return;
			case QuestEventManager.POILockoutReasonTypes.LandClaim:
				GameManager.ShowTooltip(ownerPlayer as EntityPlayerLocal, Localization.Get("poiLockoutLandClaim", false, null), false, false, 0f);
				return;
			case QuestEventManager.POILockoutReasonTypes.QuestLock:
			{
				if (extraData == 0UL)
				{
					GameManager.ShowTooltip(ownerPlayer as EntityPlayerLocal, Localization.Get("poiLockoutQuestActiveQuesters", false, null), false, false, 0f);
					return;
				}
				ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(extraData);
				int item = valueTuple.Item2;
				int item2 = valueTuple.Item3;
				GameManager.ShowTooltip(ownerPlayer as EntityPlayerLocal, Localization.Get("ttQuestLockedUntil", false, null), string.Format("{0:00}:{1:00}", item, item2 + 1), null, null, false, false, 0f);
				break;
			}
			default:
				return;
			}
		}
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x001C7DA6 File Offset: 0x001C5FA6
	[PublicizedFrom(EAccessModifier.Private)]
	public void rallyPointActivated()
	{
		ObjectiveRallyPoint.OutstandingRallyPoint = null;
		base.CurrentValue = 1;
		this.Refresh();
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x001C7DBB File Offset: 0x001C5FBB
	public void RallyPointActivated()
	{
		this.rallyPointActivated();
	}

	// Token: 0x06004889 RID: 18569 RVA: 0x001C7DC3 File Offset: 0x001C5FC3
	public bool IsActivated()
	{
		return this.currentValue == 1;
	}

	// Token: 0x0600488A RID: 18570 RVA: 0x001C7DD0 File Offset: 0x001C5FD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleParty()
	{
		EntityPlayer ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
		if (ownerPlayer.Party == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyQuestChange>().Setup(ownerPlayer.entityId, base.OwnerQuest.GetObjectiveIndex(this), true, base.OwnerQuest.QuestCode), true, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyQuestChange>().Setup(ownerPlayer.entityId, base.OwnerQuest.GetObjectiveIndex(this), true, base.OwnerQuest.QuestCode), false);
	}

	// Token: 0x0600488B RID: 18571 RVA: 0x001C7E7C File Offset: 0x001C607C
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x0600488C RID: 18572 RVA: 0x001C7EB1 File Offset: 0x001C60B1
	public Vector3i RallyPos
	{
		get
		{
			return this.rallyPos;
		}
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x001C7EBC File Offset: 0x001C60BC
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 HandleRallyPoint()
	{
		BlockValue blockValue = Block.GetBlockValue("questRallyMarker", false);
		if (this.RallyStartType == ObjectiveRallyPoint.RallyStartTypes.Find)
		{
			if (!base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition))
			{
				return Vector3.zero;
			}
			Vector3 vector;
			if (!base.OwnerQuest.GetPositionData(out vector, Quest.PositionDataTypes.POISize))
			{
				return Vector3.zero;
			}
			Vector3i vector3i = new Vector3i(this.position);
			int num = 32;
			this.outerRect = new Rect(this.position.x - (float)num, this.position.z - (float)num, vector.x + (float)(num * 2), vector.z + (float)(num * 2));
			Vector3 vector2;
			if (base.OwnerQuest.GetPositionData(out vector2, Quest.PositionDataTypes.Activate))
			{
				this.rallyPos = new Vector3i(vector2);
				this.position = vector2;
				this.positionSet = ObjectiveRallyPoint.PositionSetTypes.RallyMarkerPosition;
			}
			else
			{
				World world = GameManager.Instance.World;
				if ((this.rallyPos = this.GetRallyPosition(world, vector3i, new Vector3i(vector))) != Vector3i.zero)
				{
					BlockValue block = world.GetBlock(this.rallyPos);
					if (!(block.Block is BlockRallyMarker))
					{
						return Vector3.zero;
					}
					base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.Activate, this.rallyPos.ToVector3());
					Chunk chunk = world.GetChunkFromWorldPos(this.rallyPos) as Chunk;
					if (chunk != null)
					{
						BlockEntityData blockEntity = chunk.GetBlockEntity(this.rallyPos);
						if (blockEntity == null || !(blockEntity.transform != null) || block.type != blockValue.type)
						{
							world.SetBlockRPC(this.rallyPos, blockValue);
							return Vector3.zero;
						}
						this.setVisibility(blockEntity.transform, this.rallyMarkerType, base.OwnerQuest.OwnerJournal.ActiveQuest == null);
						this.positionSet = ObjectiveRallyPoint.PositionSetTypes.RallyMarkerPosition;
					}
				}
				else
				{
					this.rallyPos = new Vector3i(this.position);
					this.positionSet = ObjectiveRallyPoint.PositionSetTypes.POIPosition;
				}
			}
		}
		else
		{
			base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.Location);
			Vector3 vector3;
			if (base.OwnerQuest.GetPositionData(out vector3, Quest.PositionDataTypes.Activate))
			{
				this.rallyPos = new Vector3i(vector3);
			}
			else
			{
				int num2 = (int)this.position.x;
				int num3 = (int)this.position.z;
				int height = (int)GameManager.Instance.World.GetHeight(num2, num3);
				this.rallyPos = new Vector3i(num2, height, num3);
			}
			World world2 = GameManager.Instance.World;
			Chunk chunk2 = world2.GetChunkFromWorldPos(this.rallyPos) as Chunk;
			if (chunk2 == null)
			{
				this.rallyPos = new Vector3i(this.position);
				this.positionSet = ObjectiveRallyPoint.PositionSetTypes.POIPosition;
			}
			else
			{
				BlockValue blockValue2 = chunk2.GetBlock(World.toBlock(this.rallyPos));
				if (blockValue2.ischild)
				{
					this.rallyPos = new Vector3i(this.rallyPos.x + blockValue2.parentx, this.rallyPos.y + blockValue2.parenty, this.rallyPos.z + blockValue2.parentz);
					blockValue2 = chunk2.GetBlock(World.toBlock(this.rallyPos));
					base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.Activate, this.rallyPos.ToVector3());
				}
				if (!(blockValue2.Block is BlockRallyMarker))
				{
					if (!blockValue2.isair && !blockValue2.Block.IsTerrainDecoration)
					{
						this.rallyPos += Vector3i.up;
					}
					GameManager.Instance.World.SetBlockRPC(this.rallyPos, blockValue, sbyte.MaxValue);
					base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.Activate, this.rallyPos.ToVector3());
					return Vector3.zero;
				}
				BlockEntityData blockEntity2 = chunk2.GetBlockEntity(this.rallyPos);
				if (blockEntity2 == null || !(blockEntity2.transform != null) || blockValue2.type != blockValue.type)
				{
					world2.SetBlockRPC(this.rallyPos, blockValue);
					return Vector3.zero;
				}
				this.setVisibility(blockEntity2.transform, this.rallyMarkerType, true);
				this.positionSet = ObjectiveRallyPoint.PositionSetTypes.RallyMarkerPosition;
			}
		}
		this.position = this.rallyPos.ToVector3();
		base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.Activate, this.NavObjectName, -1);
		return this.rallyPos.ToVector3();
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x001C8304 File Offset: 0x001C6504
	public Vector3i GetRallyPosition(World _world, Vector3i _prefabPosition, Vector3i _prefabSize)
	{
		int num = World.toChunkXZ(_prefabPosition.x - 1);
		int num2 = World.toChunkXZ(_prefabPosition.x + _prefabSize.x + 1);
		int num3 = World.toChunkXZ(_prefabPosition.z - 1);
		int num4 = World.toChunkXZ(_prefabPosition.z + _prefabSize.z + 1);
		new List<Vector3i>();
		Rect rect = new Rect((float)_prefabPosition.x, (float)_prefabPosition.z, (float)_prefabSize.x, (float)_prefabSize.z);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunk = _world.GetChunkSync(i, j) as Chunk;
				if (chunk != null)
				{
					List<Vector3i> list = chunk.IndexedBlocks["Rally"];
					if (list != null)
					{
						for (int k = 0; k < list.Count; k++)
						{
							Vector3 vector = chunk.ToWorldPos(list[k]).ToVector3();
							if (rect.Contains(new Vector2(vector.x, vector.z)))
							{
								base.CurrentValue = 2;
								return chunk.ToWorldPos(list[k]);
							}
						}
					}
				}
			}
		}
		return Vector3i.zero;
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x001C8440 File Offset: 0x001C6640
	public override BaseObjective Clone()
	{
		ObjectiveRallyPoint objectiveRallyPoint = new ObjectiveRallyPoint();
		this.CopyValues(objectiveRallyPoint);
		return objectiveRallyPoint;
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x001C845C File Offset: 0x001C665C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveRallyPoint objectiveRallyPoint = (ObjectiveRallyPoint)objective;
		objectiveRallyPoint.RallyStartType = this.RallyStartType;
		objectiveRallyPoint.startTime = this.startTime;
		objectiveRallyPoint.endTime = this.endTime;
		objectiveRallyPoint.activateEvent = this.activateEvent;
		objectiveRallyPoint.rallyMarkerType = this.rallyMarkerType;
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x001C84B1 File Offset: 0x001C66B1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.HandleRallyPoint();
		if (this.positionSet != ObjectiveRallyPoint.PositionSetTypes.None)
		{
			base.CurrentValue = 2;
		}
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x001C84CC File Offset: 0x001C66CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Update()
	{
		if (this.positionSet != ObjectiveRallyPoint.PositionSetTypes.RallyMarkerPosition)
		{
			this.HandleRallyPoint();
			if (this.positionSet == ObjectiveRallyPoint.PositionSetTypes.None)
			{
				return;
			}
		}
		if (this.positionSet == ObjectiveRallyPoint.PositionSetTypes.RallyMarkerPosition)
		{
			GameManager.Instance.StartCoroutine(this.setRallyPointVisibility(base.OwnerQuest.OwnerJournal.ActiveQuest == null));
		}
		Vector3 vector = base.OwnerQuest.OwnerJournal.OwnerPlayer.position;
		if (this.RallyStartType == ObjectiveRallyPoint.RallyStartTypes.Create)
		{
			this.currentDistance = Vector3.Distance(vector, this.position + new Vector3(0.5f, 0f, 0.5f));
			if (this.currentDistance > this.distanceNeeded)
			{
				if (base.Description == "" || this.lastDistance <= this.distanceNeeded)
				{
					base.Description = ((base.OwnerQuest.SharedOwnerID != -1) ? ObjectiveRallyPoint.textWaitForActivate : ObjectiveRallyPoint.textHeadToRallyPoint);
				}
			}
			else
			{
				if (this.lastDistance > this.distanceNeeded)
				{
					this.setRallyPointVisible = true;
				}
				if (base.Description == "" || this.lastDistance > this.distanceNeeded)
				{
					base.Description = ((base.OwnerQuest.SharedOwnerID != -1) ? ObjectiveRallyPoint.textWaitForActivate : ObjectiveRallyPoint.textActivateRallyPoint);
				}
			}
			this.lastDistance = this.currentDistance;
			return;
		}
		this.currentDistance = Vector3.Distance(vector, this.position + new Vector3(0.5f, 0f, 0.5f));
		vector.y = vector.z;
		if (this.outerRect.Contains(vector))
		{
			if (base.Description == "" || this.lastDistance > this.distanceNeeded)
			{
				base.Description = ((base.OwnerQuest.SharedOwnerID != -1) ? ObjectiveRallyPoint.textWaitForActivate : ObjectiveRallyPoint.textActivateRallyPoint);
			}
			if (this.lastDistance > this.distanceNeeded)
			{
				this.setRallyPointVisible = true;
			}
			this.lastDistance = -1f;
			return;
		}
		if (this.lastDistance == -1f)
		{
			this.setRallyPointVisible = true;
			this.lastDistance = 1f;
		}
		if (base.Description == "" || this.lastDistance <= this.distanceNeeded)
		{
			base.Description = ((base.OwnerQuest.SharedOwnerID != -1) ? ObjectiveRallyPoint.textWaitForActivate : ObjectiveRallyPoint.textHeadToRallyPoint);
		}
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x001C8724 File Offset: 0x001C6924
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveRallyPoint.PropRallyStartMode))
		{
			this.RallyStartType = EnumUtils.Parse<ObjectiveRallyPoint.RallyStartTypes>(properties.Values[ObjectiveRallyPoint.PropRallyStartMode], false);
		}
		properties.ParseInt(ObjectiveRallyPoint.PropAllowedStartTime, ref this.startTime);
		properties.ParseInt(ObjectiveRallyPoint.PropAllowedEndTime, ref this.endTime);
		properties.ParseString(ObjectiveRallyPoint.PropActivateEvent, ref this.activateEvent);
		properties.ParseString(ObjectiveRallyPoint.PropRallyMarkerType, ref this.rallyMarkerType);
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x001C87AC File Offset: 0x001C69AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("position", this.position.ToString());
		parameters.Add("block_id", string.IsNullOrEmpty(this.ID) ? "Any Block" : this.ID);
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x001C87FC File Offset: 0x001C69FC
	public void RallyPointRefresh()
	{
		GameManager.Instance.StartCoroutine(this.setRallyPointVisibility(true));
	}

	// Token: 0x040039EE RID: 14830
	public ObjectiveRallyPoint.RallyStartTypes RallyStartType;

	// Token: 0x040039EF RID: 14831
	public static string PropRallyStartMode = "start_mode";

	// Token: 0x040039F0 RID: 14832
	public static string PropAllowedStartTime = "allowed_start_hour";

	// Token: 0x040039F1 RID: 14833
	public static string PropAllowedEndTime = "allowed_end_hour";

	// Token: 0x040039F2 RID: 14834
	public static string PropActivateEvent = "activate_event";

	// Token: 0x040039F3 RID: 14835
	public static string PropRallyMarkerType = "rally_marker_type";

	// Token: 0x040039F4 RID: 14836
	public static ObjectiveRallyPoint OutstandingRallyPoint = null;

	// Token: 0x040039F5 RID: 14837
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x040039F6 RID: 14838
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040039F7 RID: 14839
	[PublicizedFrom(EAccessModifier.Protected)]
	public float currentDistance;

	// Token: 0x040039F8 RID: 14840
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect outerRect;

	// Token: 0x040039F9 RID: 14841
	[PublicizedFrom(EAccessModifier.Private)]
	public int startTime = -1;

	// Token: 0x040039FA RID: 14842
	[PublicizedFrom(EAccessModifier.Private)]
	public int endTime = -1;

	// Token: 0x040039FB RID: 14843
	[PublicizedFrom(EAccessModifier.Private)]
	public ObjectiveRallyPoint.PositionSetTypes positionSet;

	// Token: 0x040039FC RID: 14844
	[PublicizedFrom(EAccessModifier.Private)]
	public static string textActivateRallyPoint;

	// Token: 0x040039FD RID: 14845
	[PublicizedFrom(EAccessModifier.Private)]
	public static string textHeadToRallyPoint;

	// Token: 0x040039FE RID: 14846
	[PublicizedFrom(EAccessModifier.Private)]
	public static string textWaitForActivate;

	// Token: 0x040039FF RID: 14847
	[PublicizedFrom(EAccessModifier.Protected)]
	public string rallyMarkerType = "questRallyMarker";

	// Token: 0x04003A00 RID: 14848
	[PublicizedFrom(EAccessModifier.Protected)]
	public string activateEvent = "";

	// Token: 0x04003A01 RID: 14849
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRallyVisible;

	// Token: 0x04003A02 RID: 14850
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i rallyPos = Vector3i.zero;

	// Token: 0x04003A03 RID: 14851
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastDistance = -1f;

	// Token: 0x04003A04 RID: 14852
	[PublicizedFrom(EAccessModifier.Private)]
	public float distanceNeeded = 50f;

	// Token: 0x04003A05 RID: 14853
	[PublicizedFrom(EAccessModifier.Private)]
	public bool setRallyPointVisible;

	// Token: 0x020009B1 RID: 2481
	public enum RallyStartTypes
	{
		// Token: 0x04003A07 RID: 14855
		Find,
		// Token: 0x04003A08 RID: 14856
		Create
	}

	// Token: 0x020009B2 RID: 2482
	[PublicizedFrom(EAccessModifier.Private)]
	public enum PositionSetTypes
	{
		// Token: 0x04003A0A RID: 14858
		None,
		// Token: 0x04003A0B RID: 14859
		POIPosition,
		// Token: 0x04003A0C RID: 14860
		RallyMarkerPosition
	}
}
