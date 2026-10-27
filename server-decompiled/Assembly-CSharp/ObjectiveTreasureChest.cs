using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009C4 RID: 2500
[Preserve]
public class ObjectiveTreasureChest : BaseObjective
{
	// Token: 0x06004935 RID: 18741 RVA: 0x001C2FFB File Offset: 0x001C11FB
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.craftingTag);
	}

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x06004936 RID: 18742 RVA: 0x001CB264 File Offset: 0x001C9464
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			if (base.ObjectiveState != BaseObjective.ObjectiveStates.Complete)
			{
				return BaseObjective.ObjectiveValueTypes.Distance;
			}
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x06004937 RID: 18743 RVA: 0x001C27D4 File Offset: 0x001C09D4
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x06004938 RID: 18744 RVA: 0x001CB272 File Offset: 0x001C9472
	public override bool NeedsNPCSetPosition
	{
		get
		{
			return !this.useNearby;
		}
	}

	// Token: 0x06004939 RID: 18745 RVA: 0x001CB280 File Offset: 0x001C9480
	public override void SetupObjective()
	{
		this.keyword = ((this.containerType == ObjectiveTreasureChest.ContainerTypes.TreasureChest) ? Localization.Get("ObjectiveTreasureChest_keyword", false, null) : Localization.Get("ObjectiveLocateSupplies_keyword", false, null));
		this.expectedBlockValue = Block.GetBlockValue(this.ID, false);
		if (this.expectedBlockValue.isair)
		{
			Log.Error("ObjectiveTreasureChest: Invalid treasure container name.");
		}
		if (this.altBlockName != "")
		{
			this.altExpectedBlockValue = Block.GetBlockValue(this.altBlockName, false);
			return;
		}
		this.altExpectedBlockValue = BlockValue.Air;
	}

	// Token: 0x0600493A RID: 18746 RVA: 0x001C27E2 File Offset: 0x001C09E2
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
		this.StatusText = "";
	}

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x0600493B RID: 18747 RVA: 0x001CB310 File Offset: 0x001C9510
	public override string StatusText
	{
		get
		{
			if (base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
			{
				if (base.CurrentValue == 1)
				{
					return "";
				}
				if (this.currentDistance > 10f)
				{
					return ValueDisplayFormatters.Distance(this.currentDistance);
				}
				return Localization.Get("ObjectiveNearby_keyword", false, null);
			}
			else
			{
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
	}

	// Token: 0x0600493C RID: 18748 RVA: 0x001CB398 File Offset: 0x001C9598
	public override void AddHooks()
	{
		QuestEventManager questEventManager = QuestEventManager.Current;
		questEventManager.AddObjectiveToBeUpdated(this);
		questEventManager.ContainerOpened += this.Current_ContainerOpened;
		questEventManager.BlockDestroy += this.Current_BlockDestroy;
		questEventManager.ExplosionDetected += this.Current_ExplosionDetected;
	}

	// Token: 0x0600493D RID: 18749 RVA: 0x001CB3E8 File Offset: 0x001C95E8
	public override void RemoveHooks()
	{
		QuestEventManager questEventManager = QuestEventManager.Current;
		questEventManager.RemoveObjectiveToBeUpdated(this);
		questEventManager.ContainerOpened -= this.Current_ContainerOpened;
		questEventManager.BlockDestroy -= this.Current_BlockDestroy;
		questEventManager.ExplosionDetected -= this.Current_ExplosionDetected;
		if (this.projector != null)
		{
			global::UnityEngine.Object.Destroy(this.projector.gameObject);
			this.projector = null;
		}
	}

	// Token: 0x0600493E RID: 18750 RVA: 0x001CB45B File Offset: 0x001C965B
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		this.FinalizePoint((int)pos.x, (int)pos.y, (int)pos.z);
		return true;
	}

	// Token: 0x0600493F RID: 18751 RVA: 0x001CB479 File Offset: 0x001C9679
	public override void SetPosition(Vector3 position, Vector3 size)
	{
		this.FinalizePoint((int)position.x, (int)position.y, (int)position.z);
	}

	// Token: 0x06004940 RID: 18752 RVA: 0x001CB496 File Offset: 0x001C9696
	public override bool SetupPosition(EntityNPC ownerNPC = null, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		return !this.useNearby && this.GetPosition(ownerNPC);
	}

	// Token: 0x06004941 RID: 18753 RVA: 0x001CB4AC File Offset: 0x001C96AC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool GetPosition(EntityNPC ownerNPC = null)
	{
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.TreasurePoint) && base.OwnerQuest.GetPositionData(out this.offset, Quest.PositionDataTypes.TreasureOffset))
		{
			float num = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, (float)this.DefaultTreasureRadius, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			num = Mathf.Clamp(num, 0f, (float)this.DefaultTreasureRadius);
			World world = GameManager.Instance.World;
			Vector3 vector = this.position + this.offset * num;
			this.CurrentBlocksPerReduction = (int)EffectManager.GetValue(PassiveEffects.TreasureBlocksPerReduction, null, (float)this.blocksPerReduction, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			base.OwnerQuest.Position = vector;
			this.positionSet = true;
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.TreasurePoint, this.NavObjectName, this.CurrentRadius);
			base.CurrentValue = 2;
			if (this.useNearby && base.OwnerQuest.RallyMarkerActivated)
			{
				base.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
			}
			return true;
		}
		EntityAlive entityAlive = ((ownerNPC == null) ? base.OwnerQuest.OwnerJournal.OwnerPlayer : ownerNPC);
		if (this.Value != null && this.Value != "" && !StringParsers.TryParseFloat(this.Value, out this.distance) && this.Value.Contains("-"))
		{
			string[] array = this.Value.Split('-', StringSplitOptions.None);
			float num2 = StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any);
			float num3 = StringParsers.ParseFloat(array[1], 0, -1, NumberStyles.Any);
			this.distance = GameManager.Instance.World.GetGameRandom().RandomRange(num2, num3);
		}
		if (this.useNearby)
		{
			float num4 = Mathf.Clamp(EffectManager.GetValue(PassiveEffects.TreasureRadius, null, (float)this.DefaultTreasureRadius, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false), 0f, (float)this.DefaultTreasureRadius) * 0.5f - 0.5f;
			this.distance = GameManager.Instance.World.GetGameRandom().RandomRange(num4 * -1f, num4);
		}
		Vector3i zero = Vector3i.zero;
		this.CurrentBlocksPerReduction = (int)EffectManager.GetValue(PassiveEffects.TreasureBlocksPerReduction, null, (float)this.blocksPerReduction, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (base.OwnerQuest.Position == Vector3.zero)
		{
			base.OwnerQuest.Position = entityAlive.position;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (QuestEventManager.Current.GetTreasureContainerPosition(base.OwnerQuest.QuestCode, this.distance, this.DefaultTreasureRadius + 2, (float)this.DefaultTreasureRadius, base.OwnerQuest.Position, entityAlive.entityId, this.useNearby, this.CurrentBlocksPerReduction, out this.CurrentBlocksPerReduction, out zero, out this.offset))
			{
				base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasureOffset, this.offset);
				this.FinalizePoint(zero.x, zero.y, zero.z);
				return true;
			}
		}
		else
		{
			QuestEventManager.Current.GetTreasureContainerPosition(base.OwnerQuest.QuestCode, this.distance, this.DefaultTreasureRadius + 2, (float)this.DefaultTreasureRadius, base.OwnerQuest.Position, entityAlive.entityId, this.useNearby, this.CurrentBlocksPerReduction, out this.CurrentBlocksPerReduction, out zero, out this.offset);
			base.CurrentValue = 1;
		}
		return false;
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x001CB870 File Offset: 0x001C9A70
	public static bool CalculateTreasurePoint(Vector3 startPosition, float distance, int offset, float treasureRadius, bool useNearby, out Vector3i position, out Vector3 treasureOffset)
	{
		World world = GameManager.Instance.World;
		treasureOffset = Vector3.zero;
		Vector3 treasureOffset2 = ObjectiveTreasureChest.GetTreasureOffset(world);
		treasureOffset2.Normalize();
		Vector3 vector = startPosition + treasureOffset2 * (distance - 1f);
		position = new Vector3i(vector.x, (float)((int)GameManager.Instance.World.GetHeightAt(vector.x, vector.z) - 3), vector.z);
		Vector3 vector2 = position.ToVector3();
		if (!world.IsPositionInBounds(vector2) || !world.IsEmptyPosition(position) || world.IsPositionWithinPOI(vector2, offset) || world.IsPositionRadiated(vector2))
		{
			return false;
		}
		if (!GameManager.Instance.World.CheckForLevelNearbyHeights(vector.x, vector.z, 5))
		{
			return false;
		}
		if (GameManager.Instance.World.GetWaterAt(vector.x, vector.z))
		{
			return false;
		}
		if (vector.y <= 10f)
		{
			return false;
		}
		if (useNearby)
		{
			treasureOffset = (startPosition - vector2) / treasureRadius;
			treasureOffset.Normalize();
		}
		else
		{
			treasureOffset = ObjectiveTreasureChest.GetTreasureOffset(world);
			treasureOffset *= 0.9f;
		}
		return true;
	}

	// Token: 0x06004943 RID: 18755 RVA: 0x001CB9C4 File Offset: 0x001C9BC4
	public static Vector3 GetTreasureOffset(World world)
	{
		Vector2 randomInsideUnitCircle = world.GetGameRandom().RandomInsideUnitCircle;
		return new Vector3(randomInsideUnitCircle.x, 0f, randomInsideUnitCircle.y);
	}

	// Token: 0x06004944 RID: 18756 RVA: 0x001CB9F4 File Offset: 0x001C9BF4
	public void FinalizePoint(int x, int y, int z)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && base.CurrentValue != 1)
		{
			if (!base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.TreasurePoint))
			{
				this.position = new Vector3((float)x, (float)y, (float)z);
				this.neededContainerLocation = new Vector3i(x, y, z);
				base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasurePoint, this.position);
				if (base.OwnerQuest.DataVariables.ContainsKey("treasurecontainer"))
				{
					base.OwnerQuest.DataVariables["treasurecontainer"] = string.Format("{0},{1},{2}", x, y, z);
				}
				else
				{
					base.OwnerQuest.DataVariables.Add("treasurecontainer", string.Format("{0},{1},{2}", x, y, z));
				}
				if (base.OwnerQuest.OwnerJournal != null && base.OwnerQuest.OwnerJournal.OwnerPlayer != null)
				{
					float num = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, (float)this.DefaultTreasureRadius, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
					num = Mathf.Clamp(num, 0f, (float)this.DefaultTreasureRadius);
					World world = GameManager.Instance.World;
					if (!base.OwnerQuest.GetPositionData(out this.offset, Quest.PositionDataTypes.TreasureOffset))
					{
						this.offset = ObjectiveTreasureChest.GetTreasureOffset(world);
						base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasureOffset, this.offset);
					}
					Vector3 vector = this.position + this.offset * num;
					base.OwnerQuest.Position = vector;
					this.positionSet = true;
					base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.TreasurePoint, this.NavObjectName, this.CurrentRadius);
					base.CurrentValue = 2;
					return;
				}
				base.OwnerQuest.Position = this.position;
			}
			return;
		}
		this.position = new Vector3((float)x, (float)y, (float)z);
		this.neededContainerLocation = new Vector3i(x, y, z);
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasurePoint, this.position);
		if (base.OwnerQuest.DataVariables.ContainsKey("treasurecontainer"))
		{
			base.OwnerQuest.DataVariables["treasurecontainer"] = string.Format("{0},{1},{2}", x, y, z);
		}
		else
		{
			base.OwnerQuest.DataVariables.Add("treasurecontainer", string.Format("{0},{1},{2}", x, y, z));
		}
		if (base.OwnerQuest.OwnerJournal != null && base.OwnerQuest.OwnerJournal.OwnerPlayer != null)
		{
			float num2 = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, (float)this.DefaultTreasureRadius, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			num2 = Mathf.Clamp(num2, 0f, (float)this.DefaultTreasureRadius);
			World world2 = GameManager.Instance.World;
			if (!base.OwnerQuest.GetPositionData(out this.offset, Quest.PositionDataTypes.TreasureOffset))
			{
				if (this.useNearby)
				{
					this.offset = (base.OwnerQuest.Position - this.position) / num2;
					this.offset.Normalize();
				}
				else
				{
					this.offset = ObjectiveTreasureChest.GetTreasureOffset(world2);
				}
				base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasureOffset, this.offset);
			}
			Vector3 vector2 = this.position + this.offset * (num2 - 1f);
			base.OwnerQuest.Position = vector2;
			this.positionSet = true;
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.TreasurePoint, this.NavObjectName, this.CurrentRadius);
			base.CurrentValue = 2;
			return;
		}
		base.OwnerQuest.Position = this.position;
	}

	// Token: 0x06004945 RID: 18757 RVA: 0x001CBDE8 File Offset: 0x001C9FE8
	public void FinalizePointFromServer(int _blocksPerReduction, Vector3i _chestPos, Vector3 _treasureOffset)
	{
		this.CurrentBlocksPerReduction = _blocksPerReduction;
		this.position = _chestPos.ToVector3();
		this.neededContainerLocation = _chestPos;
		base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasurePoint, this.position);
		if (base.OwnerQuest.DataVariables.ContainsKey("treasurecontainer"))
		{
			base.OwnerQuest.DataVariables["treasurecontainer"] = string.Format("{0},{1},{2}", _chestPos.x, _chestPos.y, _chestPos.z);
		}
		else
		{
			base.OwnerQuest.DataVariables.Add("treasurecontainer", string.Format("{0},{1},{2}", _chestPos.x, _chestPos.y, _chestPos.z));
		}
		if (base.OwnerQuest.OwnerJournal != null && base.OwnerQuest.OwnerJournal.OwnerPlayer != null)
		{
			float num = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, (float)this.DefaultTreasureRadius, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			num = Mathf.Clamp(num, 0f, (float)this.DefaultTreasureRadius);
			World world = GameManager.Instance.World;
			this.offset = _treasureOffset;
			base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasureOffset, this.offset);
			Vector3 vector = this.position + this.offset * num;
			base.OwnerQuest.Position = vector;
			this.positionSet = true;
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.TreasurePoint, this.NavObjectName, this.CurrentRadius);
			base.CurrentValue = 2;
			return;
		}
		base.OwnerQuest.Position = this.position;
		this.offset = _treasureOffset;
	}

	// Token: 0x06004946 RID: 18758 RVA: 0x001CBFB8 File Offset: 0x001CA1B8
	public override void Update(float updateTime)
	{
		if (this.projector == null)
		{
			this.projector = this.CreateProjector().GetComponent<BoundaryProjectorTreasure>();
		}
		if (base.OwnerQuest.Active && base.OwnerQuest.MapObject == null && base.OwnerQuest.NavObject == null)
		{
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.TreasurePoint, this.NavObjectName, this.CurrentRadius);
		}
		switch (base.CurrentValue)
		{
		case 0:
			this.GetPosition(null);
			return;
		case 1:
			break;
		case 2:
		{
			EntityPlayer ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
			this.position = new Vector3(this.position.x, ownerPlayer.position.y, this.position.z);
			this.lastDistance = this.currentDistance;
			this.currentDistance = Vector3.Distance(ownerPlayer.position, this.position + this.offset * (float)this.CurrentRadius);
			if (this.currentDistance <= 20f)
			{
				if (!this.positionAdjusted)
				{
					int num = (int)this.position.x;
					int num2 = (int)this.position.z;
					int num3 = (int)(GameManager.Instance.World.GetTerrainHeight(num, num2) - 3);
					Vector3i vector3i = new Vector3i(num, num3, num2);
					if (GameManager.Instance.World.GetChunkFromWorldPos(vector3i) != null)
					{
						this.positionAdjusted = true;
						this.position = new Vector3((float)num, (float)num3, (float)num2);
						this.neededContainerLocation = vector3i;
						base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasurePoint, this.position);
					}
				}
				if (base.OwnerQuest.SharedOwnerID == -1)
				{
					if (GameManager.Instance.World.GetChunkFromWorldPos(this.neededContainerLocation) is Chunk)
					{
						BlockValue block = GameManager.Instance.World.GetBlock(this.neededContainerLocation);
						if (block.type != this.expectedBlockValue.type && (this.altExpectedBlockValue.isair || this.altExpectedBlockValue.type != block.type))
						{
							GameManager.Instance.World.SetBlockRPC(this.neededContainerLocation, this.expectedBlockValue, sbyte.MaxValue);
						}
						if (this.useNearby)
						{
							if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
							{
								QuestEventManager.Current.SetTreasureContainerPosition(base.OwnerQuest.QuestCode, this.neededContainerLocation);
							}
							else
							{
								SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(base.OwnerQuest.QuestCode, this.neededContainerLocation), false);
							}
						}
						base.CurrentValue = 4;
						base.OwnerQuest.RallyMarkerActivated = true;
						return;
					}
				}
				else if (GameManager.Instance.World.GetBlock(this.neededContainerLocation).type == this.expectedBlockValue.type)
				{
					base.CurrentValue = 4;
					base.OwnerQuest.RallyMarkerActivated = true;
					return;
				}
			}
			break;
		}
		case 3:
			if (GameManager.Instance.World.GetBlock(this.neededContainerLocation).type != this.expectedBlockValue.type)
			{
				base.CurrentValue = 2;
				return;
			}
			base.CurrentValue = 4;
			return;
		case 4:
			if (this.positionSet)
			{
				EntityPlayer ownerPlayer2 = base.OwnerQuest.OwnerJournal.OwnerPlayer;
				this.lastDistance = this.currentDistance;
				this.currentDistance = Vector2.Distance(new Vector2(ownerPlayer2.position.x, ownerPlayer2.position.z), new Vector2(this.position.x + this.offset.x * (float)this.CurrentRadius, this.position.z + this.offset.z * (float)this.CurrentRadius));
				if (this.currentDistance < 30f)
				{
					this.HandleNavObjects(ownerPlayer2);
					if (!this.projector.IsInitialized || this.lastDistance >= 30f)
					{
						ownerPlayer2.FireEvent(MinEventTypes.onTreasureRadiusEntered, true);
					}
					if (!this.projector.IsInitialized)
					{
						this.RadiusBoundsChanged();
						this.projector.IsInitialized = true;
						this.projector.gameObject.SetActive(true);
					}
					else if (this.lastDistance >= 30f)
					{
						this.RadiusBoundsChanged();
						this.projector.gameObject.SetActive(true);
					}
					if (this.originPos != Origin.position)
					{
						this.ResetProjectorPosition((float)this.CurrentRadius);
					}
					this.projector.WithinRadius = this.currentDistance <= this.projector.CurrentRadius;
					this.projector.transform.position = new Vector3(this.projector.transform.position.x, ownerPlayer2.transform.position.y, this.projector.transform.position.z);
					Chunk chunk = GameManager.Instance.World.GetChunkFromWorldPos(this.neededContainerLocation) as Chunk;
					if (chunk != null && chunk.IsDisplayed)
					{
						BlockValue block2 = GameManager.Instance.World.GetBlock(this.neededContainerLocation);
						if (block2.type != this.expectedBlockValue.type && (this.altExpectedBlockValue.isair || this.altExpectedBlockValue.type != block2.type))
						{
							base.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
							return;
						}
					}
				}
				else if (this.currentDistance > 30f)
				{
					this.projector.gameObject.SetActive(false);
					if (this.lastDistance <= 30f)
					{
						ownerPlayer2.FireEvent(MinEventTypes.onTreasureRadiusExited, true);
						return;
					}
				}
			}
			break;
		case 5:
			QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
			if (this.directNavObject != null)
			{
				NavObjectManager.Instance.UnRegisterNavObject(this.directNavObject);
				this.directNavObject = null;
			}
			base.OwnerQuest.OwnerJournal.OwnerPlayer.FireEvent(MinEventTypes.onTreasureRadiusCompleted, true);
			break;
		default:
			return;
		}
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x001CC5AC File Offset: 0x001CA7AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleNavObjects(EntityPlayer player)
	{
		float num = (float)this.CurrentRadius;
		num = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, num, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		num = Mathf.Clamp(num, 0f, num);
		if (this.directNavObject == null)
		{
			this.directNavObject = NavObjectManager.Instance.RegisterNavObject(this.DirectNavObjectName, this.neededContainerLocation.ToVector3() + new Vector3(0.5f, 0f, 0.5f), "", false, -1, null);
			if (base.OwnerQuest != null)
			{
				QuestClass questClass = base.OwnerQuest.QuestClass;
				this.directNavObject.name = questClass.Name;
			}
		}
		else
		{
			if (this.directNavObject.DisplayName == "" && base.OwnerQuest != null)
			{
				QuestClass questClass2 = base.OwnerQuest.QuestClass;
				this.directNavObject.name = questClass2.Name;
			}
			this.directNavObject.ExtraData = num;
		}
		if (num == 0f)
		{
			this.directNavObject.ForceDisabled = false;
			base.OwnerQuest.NavObject.ForceDisabled = true;
			return;
		}
		this.directNavObject.ForceDisabled = true;
		base.OwnerQuest.NavObject.ForceDisabled = false;
	}

	// Token: 0x06004948 RID: 18760 RVA: 0x001CC6EC File Offset: 0x001CA8EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RadiusBoundsChanged()
	{
		if (this.projector == null)
		{
			return;
		}
		EntityPlayer ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
		float num = (float)this.CurrentRadius;
		num = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, num, ownerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		num = Mathf.Clamp(num, 0f, num);
		this.projector.SetRadius(0, num);
		this.ResetProjectorPosition(num);
	}

	// Token: 0x06004949 RID: 18761 RVA: 0x001CC764 File Offset: 0x001CA964
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetProjectorPosition(float radius)
	{
		this.projector.transform.position = this.position - Origin.position + this.offset * radius + Vector3.up * 4f;
		this.originPos = Origin.position;
	}

	// Token: 0x0600494A RID: 18762 RVA: 0x001CC7C4 File Offset: 0x001CA9C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, TEFeatureStorage lootTE, bool firstTimeTouched)
	{
		if (base.Complete)
		{
			return;
		}
		if (this.neededContainerLocation.x == containerLocation.x && this.neededContainerLocation.y == containerLocation.y && this.neededContainerLocation.z == containerLocation.z && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 5;
			if (this.projector != null)
			{
				global::UnityEngine.Object.Destroy(this.projector.gameObject);
				this.projector = null;
			}
			NavObjectManager.Instance.UnRegisterNavObject(this.directNavObject);
			this.Refresh();
		}
	}

	// Token: 0x0600494B RID: 18763 RVA: 0x001CC864 File Offset: 0x001CAA64
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockDestroy(Block block, Vector3i blockPos)
	{
		if (this.blocksPerReduction <= 0)
		{
			return;
		}
		if (base.Complete)
		{
			return;
		}
		if (!(block.shape is BlockShapeTerrain))
		{
			return;
		}
		Vector3 vector = this.position + this.offset * (float)this.CurrentRadius;
		if (Vector3.Distance(new Vector3((float)blockPos.x, 0f, (float)blockPos.z), new Vector3(vector.x, 0f, vector.z)) < (float)this.CurrentRadius + 0.5f && this.AddToDestroyCount())
		{
			this.HandleParty();
		}
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x001CC900 File Offset: 0x001CAB00
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ExplosionDetected(Vector3 pos, int entityID, float blockDamage)
	{
		if (this.explosionEvent == "" || base.OwnerQuest.SharedOwnerID != -1)
		{
			return;
		}
		if (this.lastExplosionTime > Time.time)
		{
			return;
		}
		if (Vector3.Distance(this.position, pos) <= (float)(this.DefaultTreasureRadius * 2) && blockDamage > 1f && GameManager.Instance.World.GetEntity(entityID) is EntityPlayer)
		{
			GameEventManager.Current.HandleAction(this.explosionEvent, null, base.OwnerQuest.OwnerJournal.OwnerPlayer, false, this.neededContainerLocation, "", "", false, true, "", null, null);
			this.lastExplosionTime = Time.time + this.explosionEventDelay;
		}
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x001CC9C8 File Offset: 0x001CABC8
	public bool AddToDestroyCount()
	{
		if (this.CurrentRadius != 0)
		{
			this.destroyCount++;
			int num = this.CurrentBlocksPerReduction;
			int num2 = (int)EffectManager.GetValue(PassiveEffects.TreasureBlocksPerReduction, null, (float)this.blocksPerReduction, base.OwnerQuest.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			if (num2 != num)
			{
				num = (this.CurrentBlocksPerReduction = num2);
				QuestEventManager.Current.UpdateTreasureBlocksPerReduction(base.OwnerQuest.QuestCode, num);
			}
			if (this.destroyCount >= num)
			{
				this.CurrentRadius--;
				this.destroyCount -= num;
			}
			if (this.CurrentRadius < 0)
			{
				this.CurrentRadius = 0;
			}
			if (this.CurrentRadius != this.lastRadius)
			{
				World world = GameManager.Instance.World;
				base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.TreasureOffset, this.offset);
				base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.TreasurePoint, this.NavObjectName, this.CurrentRadius);
				this.RadiusBoundsChanged();
				base.OwnerQuest.HandleQuestEvent(base.OwnerQuest, "TreasureRadiusReduction");
				if (this.radiusReductionMessage != "")
				{
					GameManager.ShowTooltip(base.OwnerQuest.OwnerJournal.OwnerPlayer, Localization.Get(this.radiusReductionMessage, false, null), false, false, 0f);
				}
				if (this.radiusReductionSound != "" && Vector3.Distance(base.OwnerQuest.OwnerJournal.OwnerPlayer.position, this.position + this.offset * (float)this.CurrentRadius) <= (float)(this.DefaultTreasureRadius * 2))
				{
					Manager.PlayInsidePlayerHead(this.radiusReductionSound, -1, 0f, false, false);
				}
				this.lastRadius = this.CurrentRadius;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x001CCB98 File Offset: 0x001CAD98
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
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureRadiusBreak, ownerPlayer.entityId, base.OwnerQuest.QuestCode), true, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureRadiusBreak, ownerPlayer.entityId, base.OwnerQuest.QuestCode), false);
	}

	// Token: 0x0600494F RID: 18767 RVA: 0x001CCC2C File Offset: 0x001CAE2C
	public override void Refresh()
	{
		bool flag = base.CurrentValue == 5;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x001CCC64 File Offset: 0x001CAE64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveTreasureChest objectiveTreasureChest = (ObjectiveTreasureChest)objective;
		objectiveTreasureChest.containerType = this.containerType;
		objectiveTreasureChest.altBlockName = this.altBlockName;
		objectiveTreasureChest.altExpectedBlockValue = this.altExpectedBlockValue;
		objectiveTreasureChest.distance = this.distance;
		objectiveTreasureChest.DefaultTreasureRadius = this.DefaultTreasureRadius;
		objectiveTreasureChest.CurrentRadius = this.CurrentRadius;
		objectiveTreasureChest.DirectNavObjectName = this.DirectNavObjectName;
		objectiveTreasureChest.destroyCount = this.destroyCount;
		objectiveTreasureChest.lastRadius = this.lastRadius;
		objectiveTreasureChest.blocksPerReduction = this.blocksPerReduction;
		objectiveTreasureChest.radiusReductionSound = this.radiusReductionSound;
		objectiveTreasureChest.radiusReductionMessage = this.radiusReductionMessage;
		objectiveTreasureChest.originPos = this.originPos;
		objectiveTreasureChest.useNearby = this.useNearby;
		objectiveTreasureChest.explosionEventDelay = this.explosionEventDelay;
		objectiveTreasureChest.explosionEvent = this.explosionEvent;
	}

	// Token: 0x06004951 RID: 18769 RVA: 0x001CCD40 File Offset: 0x001CAF40
	public override BaseObjective Clone()
	{
		ObjectiveTreasureChest objectiveTreasureChest = new ObjectiveTreasureChest();
		this.CopyValues(objectiveTreasureChest);
		return objectiveTreasureChest;
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x001CCD5C File Offset: 0x001CAF5C
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropBlock))
		{
			this.ID = properties.Values[ObjectiveTreasureChest.PropBlock];
		}
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropAltBlock))
		{
			this.altBlockName = properties.Values[ObjectiveTreasureChest.PropAltBlock];
		}
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropDistance))
		{
			this.Value = properties.Values[ObjectiveTreasureChest.PropDistance];
		}
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropContainerType))
		{
			this.containerType = EnumUtils.Parse<ObjectiveTreasureChest.ContainerTypes>(properties.Values[ObjectiveTreasureChest.PropContainerType], false);
		}
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropDefaultRadius))
		{
			this.DefaultTreasureRadius = StringParsers.ParseSInt32(properties.Values[ObjectiveTreasureChest.PropDefaultRadius], 0, -1, NumberStyles.Integer);
		}
		this.CurrentRadius = this.DefaultTreasureRadius;
		this.lastRadius = this.CurrentRadius;
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropDirectNavObject))
		{
			this.DirectNavObjectName = properties.Values[ObjectiveTreasureChest.PropDirectNavObject];
		}
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropBlocksPerReduction))
		{
			this.blocksPerReduction = StringParsers.ParseSInt32(properties.Values[ObjectiveTreasureChest.PropBlocksPerReduction], 0, -1, NumberStyles.Integer);
		}
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropRadiusReductionSound))
		{
			this.radiusReductionSound = properties.Values[ObjectiveTreasureChest.PropRadiusReductionSound];
		}
		properties.ParseString(ObjectiveTreasureChest.PropRadiusReductionMessage, ref this.radiusReductionMessage);
		if (properties.Values.ContainsKey(ObjectiveTreasureChest.PropUseNearby))
		{
			this.useNearby = StringParsers.ParseBool(properties.Values[ObjectiveTreasureChest.PropUseNearby], 0, -1, true);
		}
		properties.ParseFloat(ObjectiveTreasureChest.PropExplosionEventDelay, ref this.explosionEventDelay);
		properties.ParseString(ObjectiveTreasureChest.PropExplosionEvent, ref this.explosionEvent);
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x001CCF44 File Offset: 0x001CB144
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("container_type", this.containerType.ToString());
		parameters.Add("position", this.position.ToString());
		parameters.Add("destroyed_block_count", this.destroyCount);
	}

	// Token: 0x06004954 RID: 18772 RVA: 0x001CCFA4 File Offset: 0x001CB1A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameObject CreateProjector()
	{
		if (ObjectiveTreasureChest.prefabProjector == null)
		{
			ObjectiveTreasureChest.prefabProjector = Resources.Load<GameObject>("Prefabs/prefabBoundaryProjectorTreasure");
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(ObjectiveTreasureChest.prefabProjector);
		gameObject.name = "Projector";
		gameObject.transform.position = new Vector3(-999f, -999f, -999f);
		return gameObject;
	}

	// Token: 0x06004955 RID: 18773 RVA: 0x001CD004 File Offset: 0x001CB204
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		if (this.projector != null)
		{
			global::UnityEngine.Object.Destroy(this.projector.gameObject);
			this.projector = null;
		}
		if (this.directNavObject != null)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this.directNavObject);
			this.directNavObject = null;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			QuestEventManager.Current.FinishTreasureQuest(base.OwnerQuest.QuestCode, base.OwnerQuest.OwnerJournal.OwnerPlayer);
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureComplete, base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, base.OwnerQuest.QuestCode), false);
		}
		base.OwnerQuest.OwnerJournal.OwnerPlayer.FireEvent(MinEventTypes.onTreasureRadiusCompleted, true);
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x001CD0DC File Offset: 0x001CB2DC
	public override void HandleFailed()
	{
		base.HandleFailed();
		if (this.projector != null)
		{
			global::UnityEngine.Object.Destroy(this.projector.gameObject);
			this.projector = null;
		}
		if (this.directNavObject != null)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this.directNavObject);
			this.directNavObject = null;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			QuestEventManager.Current.FinishTreasureQuest(base.OwnerQuest.QuestCode, base.OwnerQuest.OwnerJournal.OwnerPlayer);
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureComplete, base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, base.OwnerQuest.QuestCode), false);
		}
		base.OwnerQuest.OwnerJournal.OwnerPlayer.FireEvent(MinEventTypes.onTreasureRadiusCompleted, true);
	}

	// Token: 0x06004957 RID: 18775 RVA: 0x001CD1B4 File Offset: 0x001CB3B4
	public override void Read(PooledBinaryReader _br)
	{
		this.destroyCount = _br.ReadInt32();
		this.CurrentRadius = _br.ReadInt32();
		this.lastRadius = this.CurrentRadius;
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x001CD1DA File Offset: 0x001CB3DA
	public override void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.destroyCount);
		_bw.Write(this.CurrentRadius);
	}

	// Token: 0x06004959 RID: 18777 RVA: 0x001CD1F4 File Offset: 0x001CB3F4
	public override string ParseBinding(string bindingName)
	{
		string id = this.ID;
		string value = this.Value;
		if (!(bindingName == "distance"))
		{
			if (bindingName == "direction")
			{
				if (base.OwnerQuest.QuestGiverID != -1)
				{
					EntityNPC entityNPC = GameManager.Instance.World.GetEntity(base.OwnerQuest.QuestGiverID) as EntityNPC;
					if (entityNPC != null)
					{
						if (this.useNearby)
						{
							Vector3 vector = base.OwnerQuest.Position;
							vector.y = 0f;
							Vector3 vector2 = entityNPC.position;
							vector2.y = 0f;
							return ValueDisplayFormatters.Direction(GameUtils.GetDirByNormal(new Vector2(vector.x - vector2.x, vector.z - vector2.z)), false);
						}
						this.position.y = 0f;
						Vector3 vector3 = entityNPC.position;
						vector3.y = 0f;
						return ValueDisplayFormatters.Direction(GameUtils.GetDirByNormal(new Vector2(this.position.x - vector3.x, this.position.z - vector3.z)), false);
					}
				}
			}
		}
		else if (base.OwnerQuest.QuestGiverID != -1)
		{
			EntityNPC entityNPC2 = GameManager.Instance.World.GetEntity(base.OwnerQuest.QuestGiverID) as EntityNPC;
			if (entityNPC2 != null)
			{
				if (this.useNearby)
				{
					Vector3 vector4 = base.OwnerQuest.Position;
					vector4.y = 0f;
					vector4.y = 0f;
					Vector3 vector5 = entityNPC2.position;
					vector5.y = 0f;
					this.currentDistance = Vector3.Distance(vector5, vector4 + this.offset);
					return ValueDisplayFormatters.Distance(this.currentDistance);
				}
				this.position.y = 0f;
				Vector3 vector6 = entityNPC2.position;
				vector6.y = 0f;
				this.currentDistance = Vector3.Distance(vector6, this.position + this.offset);
				return ValueDisplayFormatters.Distance(this.currentDistance);
			}
		}
		return "";
	}

	// Token: 0x04003A58 RID: 14936
	[PublicizedFrom(EAccessModifier.Private)]
	public ObjectiveTreasureChest.ContainerTypes containerType;

	// Token: 0x04003A59 RID: 14937
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSet;

	// Token: 0x04003A5A RID: 14938
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue expectedBlockValue;

	// Token: 0x04003A5B RID: 14939
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue altExpectedBlockValue;

	// Token: 0x04003A5C RID: 14940
	public static int TreasureRadiusInitial = 9;

	// Token: 0x04003A5D RID: 14941
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastDistance = -1f;

	// Token: 0x04003A5E RID: 14942
	[PublicizedFrom(EAccessModifier.Private)]
	public float distance = 50f;

	// Token: 0x04003A5F RID: 14943
	[PublicizedFrom(EAccessModifier.Protected)]
	public float currentDistance;

	// Token: 0x04003A60 RID: 14944
	public int DefaultTreasureRadius = 9;

	// Token: 0x04003A61 RID: 14945
	public int CurrentRadius = 9;

	// Token: 0x04003A62 RID: 14946
	[PublicizedFrom(EAccessModifier.Private)]
	public string DirectNavObjectName = "";

	// Token: 0x04003A63 RID: 14947
	[PublicizedFrom(EAccessModifier.Private)]
	public string altBlockName = "";

	// Token: 0x04003A64 RID: 14948
	[PublicizedFrom(EAccessModifier.Private)]
	public string radiusReductionSound = "";

	// Token: 0x04003A65 RID: 14949
	[PublicizedFrom(EAccessModifier.Private)]
	public string radiusReductionMessage = "ttTreasureRadiusReduced";

	// Token: 0x04003A66 RID: 14950
	[PublicizedFrom(EAccessModifier.Private)]
	public int blocksPerReduction = 1;

	// Token: 0x04003A67 RID: 14951
	public int CurrentBlocksPerReduction = 1;

	// Token: 0x04003A68 RID: 14952
	[PublicizedFrom(EAccessModifier.Private)]
	public int destroyCount;

	// Token: 0x04003A69 RID: 14953
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastRadius;

	// Token: 0x04003A6A RID: 14954
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastExplosionTime = -1f;

	// Token: 0x04003A6B RID: 14955
	[PublicizedFrom(EAccessModifier.Private)]
	public float explosionEventDelay = 2f;

	// Token: 0x04003A6C RID: 14956
	[PublicizedFrom(EAccessModifier.Private)]
	public string explosionEvent = "";

	// Token: 0x04003A6D RID: 14957
	public static string PropBlock = "block";

	// Token: 0x04003A6E RID: 14958
	public static string PropAltBlock = "alt_block";

	// Token: 0x04003A6F RID: 14959
	public static string PropDistance = "distance";

	// Token: 0x04003A70 RID: 14960
	public static string PropContainerType = "container_type";

	// Token: 0x04003A71 RID: 14961
	public static string PropDefaultRadius = "default_radius";

	// Token: 0x04003A72 RID: 14962
	public static string PropDirectNavObject = "direct_nav_object";

	// Token: 0x04003A73 RID: 14963
	public static string PropBlocksPerReduction = "blocks_per_reduction";

	// Token: 0x04003A74 RID: 14964
	public static string PropRadiusReductionSound = "radius_reduction_sound";

	// Token: 0x04003A75 RID: 14965
	public static string PropUseNearby = "use_nearby";

	// Token: 0x04003A76 RID: 14966
	public static string PropExplosionEventDelay = "explosion_event_delay";

	// Token: 0x04003A77 RID: 14967
	public static string PropExplosionEvent = "explosion_event";

	// Token: 0x04003A78 RID: 14968
	public static string PropRadiusReductionMessage = "radius_reduction_message";

	// Token: 0x04003A79 RID: 14969
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useNearby;

	// Token: 0x04003A7A RID: 14970
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i neededContainerLocation = new Vector3i(-5000, -5000, -5000);

	// Token: 0x04003A7B RID: 14971
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04003A7C RID: 14972
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 originPos = Vector3.zero;

	// Token: 0x04003A7D RID: 14973
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionAdjusted;

	// Token: 0x04003A7E RID: 14974
	[PublicizedFrom(EAccessModifier.Private)]
	public NavObject directNavObject;

	// Token: 0x04003A7F RID: 14975
	[PublicizedFrom(EAccessModifier.Private)]
	public BoundaryProjectorTreasure projector;

	// Token: 0x04003A80 RID: 14976
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 offset;

	// Token: 0x04003A81 RID: 14977
	[PublicizedFrom(EAccessModifier.Protected)]
	public static GameObject prefabProjector;

	// Token: 0x020009C5 RID: 2501
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ContainerTypes
	{
		// Token: 0x04003A83 RID: 14979
		TreasureChest,
		// Token: 0x04003A84 RID: 14980
		Supplies
	}

	// Token: 0x020009C6 RID: 2502
	[PublicizedFrom(EAccessModifier.Private)]
	public enum TreasureChestStates
	{
		// Token: 0x04003A86 RID: 14982
		NoPosition,
		// Token: 0x04003A87 RID: 14983
		WaitingForPoint,
		// Token: 0x04003A88 RID: 14984
		TryCreate,
		// Token: 0x04003A89 RID: 14985
		ValidateCreation,
		// Token: 0x04003A8A RID: 14986
		Created,
		// Token: 0x04003A8B RID: 14987
		Completed
	}
}
