using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Audio;
using BhvrAnalyticsServices.Interfaces;
using Quests;
using Quests.Requirements;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine;

// Token: 0x020009CB RID: 2507
public class Quest
{
	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06004971 RID: 18801 RVA: 0x001CD88C File Offset: 0x001CBA8C
	// (set) Token: 0x06004972 RID: 18802 RVA: 0x001CD894 File Offset: 0x001CBA94
	public Quest.QuestState CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			if (this._currentState != value)
			{
				this._currentState = value;
				PrefabInstance.RefreshSwitchesInContainingPoi(this);
			}
		}
	}

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06004973 RID: 18803 RVA: 0x001CD8AC File Offset: 0x001CBAAC
	// (set) Token: 0x06004974 RID: 18804 RVA: 0x001CD8B4 File Offset: 0x001CBAB4
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06004975 RID: 18805 RVA: 0x001CD8BD File Offset: 0x001CBABD
	// (set) Token: 0x06004976 RID: 18806 RVA: 0x001CD8C5 File Offset: 0x001CBAC5
	public byte CurrentQuestVersion { get; set; }

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06004977 RID: 18807 RVA: 0x001CD8CE File Offset: 0x001CBACE
	// (set) Token: 0x06004978 RID: 18808 RVA: 0x001CD8D6 File Offset: 0x001CBAD6
	public byte CurrentFileVersion { get; set; }

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x06004979 RID: 18809 RVA: 0x001CD8DF File Offset: 0x001CBADF
	// (set) Token: 0x0600497A RID: 18810 RVA: 0x001CD8E7 File Offset: 0x001CBAE7
	public string PreviousQuest { get; set; }

	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x0600497B RID: 18811 RVA: 0x001CD8F0 File Offset: 0x001CBAF0
	// (set) Token: 0x0600497C RID: 18812 RVA: 0x001CD8F8 File Offset: 0x001CBAF8
	public bool OptionalComplete
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x0600497D RID: 18813 RVA: 0x001CD901 File Offset: 0x001CBB01
	// (set) Token: 0x0600497E RID: 18814 RVA: 0x001CD909 File Offset: 0x001CBB09
	public ulong FinishTime { get; set; }

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x0600497F RID: 18815 RVA: 0x001CD912 File Offset: 0x001CBB12
	// (set) Token: 0x06004980 RID: 18816 RVA: 0x001CD91A File Offset: 0x001CBB1A
	public byte CurrentPhase { get; set; }

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x06004981 RID: 18817 RVA: 0x001CD923 File Offset: 0x001CBB23
	// (set) Token: 0x06004982 RID: 18818 RVA: 0x001CD92B File Offset: 0x001CBB2B
	public int SharedOwnerID
	{
		get
		{
			return this.sharedOwnerID;
		}
		set
		{
			if (this.sharedOwnerID != value)
			{
				this.sharedOwnerID = value;
			}
		}
	}

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x06004983 RID: 18819 RVA: 0x001CD93D File Offset: 0x001CBB3D
	public static int QuestsPerDay
	{
		get
		{
			return GameStats.GetInt(EnumGameStats.QuestProgressionDailyLimit);
		}
	}

	// Token: 0x06004984 RID: 18820 RVA: 0x001CD946 File Offset: 0x001CBB46
	public void AddQuestTag(FastTags<TagGroup.Global> tag)
	{
		this.QuestTags |= tag;
	}

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x06004985 RID: 18821 RVA: 0x001CD95A File Offset: 0x001CBB5A
	public bool Active
	{
		get
		{
			return this.CurrentState == Quest.QuestState.InProgress || this.CurrentState == Quest.QuestState.ReadyForTurnIn;
		}
	}

	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x06004986 RID: 18822 RVA: 0x001CD970 File Offset: 0x001CBB70
	// (set) Token: 0x06004987 RID: 18823 RVA: 0x001CD978 File Offset: 0x001CBB78
	public Vector3 Position
	{
		get
		{
			return this.position;
		}
		set
		{
			this.position = value;
		}
	}

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x06004988 RID: 18824 RVA: 0x001CD981 File Offset: 0x001CBB81
	public bool HasPosition
	{
		get
		{
			return this.MapObject != null || this.NavObject != null;
		}
	}

	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x06004989 RID: 18825 RVA: 0x001CD998 File Offset: 0x001CBB98
	public string RequirementsString
	{
		get
		{
			if (this.Requirements.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.Requirements.Count; i++)
				{
					stringBuilder.Append(this.Requirements[i].CheckRequirement() ? "[DEFAULT_COLOR]" : "[MISSING_COLOR]");
					stringBuilder.Append(this.Requirements[i].Description);
					stringBuilder.Append("[-]");
					stringBuilder.Append((i < this.Requirements.Count - 1) ? ", " : "");
				}
				return stringBuilder.ToString();
			}
			return "";
		}
	}

	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x0600498A RID: 18826 RVA: 0x001CDA4B File Offset: 0x001CBC4B
	// (set) Token: 0x0600498B RID: 18827 RVA: 0x001CDA53 File Offset: 0x001CBC53
	public bool Tracked
	{
		get
		{
			return this.tracked;
		}
		set
		{
			if (this.tracked)
			{
				this.SetMapObjectSelected(false);
			}
			this.tracked = value;
			if (this.tracked)
			{
				this.SetMapObjectSelected(true);
			}
		}
	}

	// Token: 0x0600498C RID: 18828 RVA: 0x001CDA7C File Offset: 0x001CBC7C
	public byte GetActionIndex(BaseQuestAction action)
	{
		for (int i = 0; i < this.Actions.Count; i++)
		{
			if (action == this.Actions[i])
			{
				return (byte)i;
			}
		}
		return 0;
	}

	// Token: 0x0600498D RID: 18829 RVA: 0x001CDAB4 File Offset: 0x001CBCB4
	public byte GetObjectiveIndex(BaseObjective objective)
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (objective == this.Objectives[i])
			{
				return (byte)i;
			}
		}
		return 0;
	}

	// Token: 0x17000805 RID: 2053
	// (get) Token: 0x0600498E RID: 18830 RVA: 0x001CDAEC File Offset: 0x001CBCEC
	public int ActiveObjectives
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.Objectives.Count; i++)
			{
				if ((this.Objectives[i].Phase == 0 || this.Objectives[i].Phase == this.CurrentPhase) && !this.Objectives[i].HiddenObjective)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x17000806 RID: 2054
	// (get) Token: 0x0600498F RID: 18831 RVA: 0x001CDB55 File Offset: 0x001CBD55
	public QuestClass QuestClass
	{
		get
		{
			if (this.questClass == null)
			{
				this.questClass = QuestClass.GetQuest(this.ID);
			}
			return this.questClass;
		}
	}

	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x06004990 RID: 18832 RVA: 0x001CDB76 File Offset: 0x001CBD76
	public bool IsShareable
	{
		get
		{
			return this.SharedOwnerID == -1 && this.QuestClass.Shareable && !this.RallyMarkerActivated && this.CurrentState == Quest.QuestState.InProgress;
		}
	}

	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x06004991 RID: 18833 RVA: 0x001CDBA1 File Offset: 0x001CBDA1
	// (set) Token: 0x06004992 RID: 18834 RVA: 0x001CDBA9 File Offset: 0x001CBDA9
	public MapObject MapObject
	{
		get
		{
			return this.mapObject;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.mapObject != null)
			{
				GameManager.Instance.World.ObjectOnMapRemove(this.mapObject.type, (int)this.mapObject.key);
			}
			this.mapObject = value;
		}
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06004993 RID: 18835 RVA: 0x001CDBE0 File Offset: 0x001CBDE0
	public bool AddsProgression
	{
		get
		{
			return this.QuestProgressDay > 0;
		}
	}

	// Token: 0x06004994 RID: 18836 RVA: 0x001CDBEC File Offset: 0x001CBDEC
	public void HandleMapObject(Quest.PositionDataTypes dataType, string navObjectName, int defaultTreasureRadius = -1)
	{
		if (this.OwnerJournal == null)
		{
			return;
		}
		this.RemoveMapObject();
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		float num = -1f;
		bool flag = false;
		switch (dataType)
		{
		case Quest.PositionDataTypes.QuestGiver:
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.QuestGiver))
			{
				this.Position = zero;
				if (navObjectName == "")
				{
					this.MapObject = new MapObjectQuest(zero, "ui_game_symbol_quest");
					GameManager.Instance.World.ObjectOnMapAdd(this.MapObject);
				}
				flag = true;
			}
			break;
		case Quest.PositionDataTypes.Location:
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.Location))
			{
				this.Position = zero;
				if (navObjectName == "")
				{
					this.MapObject = new MapObjectQuest(zero, "ui_game_symbol_quest");
					GameManager.Instance.World.ObjectOnMapAdd(this.MapObject);
				}
				new Vector3(0.5f, 0f, 0.5f);
				flag = true;
			}
			break;
		case Quest.PositionDataTypes.POIPosition:
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition))
			{
				Vector3 zero3 = Vector3.zero;
				if (this.GetPositionData(out zero3, Quest.PositionDataTypes.POISize))
				{
					Vector3 vector = new Vector3(zero.x + zero3.x / 2f, zero.y, zero.z + zero3.z / 2f);
					vector.y = (float)((int)GameManager.Instance.World.GetHeightAt(vector.x, vector.y));
					this.Position = vector;
					if (navObjectName == "")
					{
						this.MapObject = new MapObjectQuest(vector, "ui_game_symbol_quest");
						GameManager.Instance.World.ObjectOnMapAdd(this.MapObject);
					}
				}
				new Vector3(0.5f, 0f, 0.5f);
				flag = true;
			}
			break;
		case Quest.PositionDataTypes.TreasurePoint:
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.TreasurePoint))
			{
				if (defaultTreasureRadius == -1)
				{
					defaultTreasureRadius = ObjectiveTreasureChest.TreasureRadiusInitial;
				}
				float num2 = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, (float)defaultTreasureRadius, this.OwnerJournal.OwnerPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				num2 = Mathf.Clamp(num2, 0f, (float)defaultTreasureRadius);
				World world = GameManager.Instance.World;
				Vector3 vector2;
				this.GetPositionData(out vector2, Quest.PositionDataTypes.TreasureOffset);
				this.Position = zero + vector2 * num2;
				if (navObjectName == "")
				{
					if (this.MapObject is MapObjectTreasureChest)
					{
						(this.MapObject as MapObjectTreasureChest).SetPosition(this.Position);
					}
					else
					{
						this.MapObject = new MapObjectTreasureChest(this.Position, this.QuestCode, defaultTreasureRadius);
						GameManager.Instance.World.ObjectOnMapAdd(this.MapObject);
					}
				}
				else
				{
					num = (float)defaultTreasureRadius;
				}
				flag = true;
			}
			break;
		case Quest.PositionDataTypes.FetchContainer:
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.FetchContainer))
			{
				this.Position = zero;
				if (navObjectName == "")
				{
					this.MapObject = new MapObjectFetchItem(zero + Vector3.one * 0.5f);
					GameManager.Instance.World.ObjectOnMapAdd(this.MapObject);
				}
				new Vector3(0.5f, 0f, 0.5f);
				flag = true;
			}
			break;
		case Quest.PositionDataTypes.HiddenCache:
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.HiddenCache))
			{
				this.Position = zero;
				if (navObjectName == "")
				{
					this.MapObject = new MapObjectHiddenCache(zero + Vector3.one * 0.5f);
					GameManager.Instance.World.ObjectOnMapAdd(this.MapObject);
				}
				new Vector3(0.5f, 0f, 0.5f);
				flag = true;
			}
			break;
		case Quest.PositionDataTypes.Activate:
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.Activate))
			{
				this.Position = zero;
				if (navObjectName == "")
				{
					this.MapObject = new MapObjectQuest(zero + Vector3.one * 0.5f, "ui_game_symbol_quest");
					GameManager.Instance.World.ObjectOnMapAdd(this.MapObject);
				}
				new Vector3(0.5f, 0f, 0.5f);
				flag = true;
			}
			break;
		}
		if (navObjectName != "" && flag)
		{
			World world2 = GameManager.Instance.World;
			EntityPlayer entityPlayer = world2.GetEntity(this.sharedOwnerID) as EntityPlayer;
			if (entityPlayer == null)
			{
				entityPlayer = world2.GetPrimaryPlayer();
			}
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(navObjectName, this.Position + new Vector3(0.5f, 0f, 0.5f), "", false, -1, entityPlayer);
			this.NavObject.IsActive = false;
			this.NavObject.ExtraData = num;
			QuestClass questClass = this.QuestClass;
			this.NavObject.name = string.Format("{0} ({1})", questClass.Name, entityPlayer.PlayerDisplayName);
		}
		this.SetMapObjectSelected(this.tracked);
	}

	// Token: 0x06004995 RID: 18837 RVA: 0x001CE0DC File Offset: 0x001CC2DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMapObjectSelected(bool isSelected)
	{
		if (this.NavObject != null)
		{
			this.NavObject.IsActive = isSelected;
		}
		if (this.MapObject != null)
		{
			if (this.MapObject is MapObjectQuest)
			{
				((MapObjectQuest)this.MapObject).IsSelected = isSelected;
				return;
			}
			if (this.MapObject is MapObjectTreasureChest)
			{
				((MapObjectTreasureChest)this.MapObject).IsSelected = isSelected;
				return;
			}
			if (this.MapObject is MapObjectFetchItem)
			{
				((MapObjectFetchItem)this.MapObject).IsSelected = isSelected;
				return;
			}
			if (this.MapObject is MapObjectHiddenCache)
			{
				((MapObjectHiddenCache)this.MapObject).IsSelected = isSelected;
				return;
			}
			if (this.MapObject is MapObjectRestorePower)
			{
				((MapObjectRestorePower)this.MapObject).IsSelected = isSelected;
			}
		}
	}

	// Token: 0x06004996 RID: 18838 RVA: 0x001CE1A4 File Offset: 0x001CC3A4
	public void SetupQuestCode()
	{
		if (this.QuestCode == 0)
		{
			this.QuestUniqueId = string.Concat(new string[]
			{
				DateTime.UtcNow.ToString("O"),
				"_",
				this.ID,
				"_",
				this.OwnerJournal.OwnerPlayer.entityId.ToString(),
				"_",
				this.QuestGiverID.ToString()
			});
			this.QuestCode = this.QuestUniqueId.GetHashCode();
		}
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x001CE238 File Offset: 0x001CC438
	public void RemoveMapObject()
	{
		if (this.MapObject != null)
		{
			this.MapObject = null;
		}
		if (this.NavObject != null)
		{
			NavObjectManager.Instance.UnRegisterNavObject(this.NavObject);
			this.NavObject = null;
		}
		Vector3 zero = Vector3.zero;
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition))
		{
			Vector3 zero2 = Vector3.zero;
			if (this.GetPositionData(out zero2, Quest.PositionDataTypes.POISize))
			{
				Vector3 vector = new Vector3(zero.x + zero2.x / 2f, this.OwnerJournal.OwnerPlayer.position.y, zero.z + zero2.z / 2f);
				GameManager.Instance.World.ObjectOnMapRemove(EnumMapObjectType.Quest, vector);
				return;
			}
		}
		else
		{
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.Location))
			{
				GameManager.Instance.World.ObjectOnMapRemove(EnumMapObjectType.Quest, zero);
				return;
			}
			if (this.GetPositionData(out zero, Quest.PositionDataTypes.TreasurePoint))
			{
				GameManager.Instance.World.ObjectOnMapRemove(EnumMapObjectType.TreasureChest, this.QuestCode);
			}
		}
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x001CE330 File Offset: 0x001CC530
	[PublicizedFrom(EAccessModifier.Internal)]
	public void UnhookQuest()
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].HandleRemoveHooks();
		}
		for (int j = 0; j < this.Objectives.Count; j++)
		{
			this.Objectives[j].RemoveObjectives();
		}
		this.RemoveMapObject();
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x001CE394 File Offset: 0x001CC594
	public Quest(string id)
	{
		this.ID = id;
		this.CurrentPhase = 1;
		this.CurrentState = Quest.QuestState.InProgress;
	}

	// Token: 0x0600499A RID: 18842 RVA: 0x001CE438 File Offset: 0x001CC638
	public void SetupTags()
	{
		this.NeedsNPCSetPosition = false;
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].OwnerQuest = this;
			this.Objectives[i].HandleVariables();
			this.Objectives[i].SetupQuestTag();
			if (this.Objectives[i].NeedsNPCSetPosition)
			{
				this.NeedsNPCSetPosition = true;
			}
		}
	}

	// Token: 0x0600499B RID: 18843 RVA: 0x001CE4B0 File Offset: 0x001CC6B0
	public bool SetupPosition(EntityNPC ownerNPC, EntityPlayer player = null, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1)
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (this.Objectives[i].SetupPosition(ownerNPC, player, usedPOILocations, entityIDforQuests))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600499C RID: 18844 RVA: 0x001CE4F0 File Offset: 0x001CC6F0
	public void SetPosition(EntityNPC ownerNPC, Vector3 position, Vector3 size)
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].OwnerQuest = this;
			this.Objectives[i].HandleVariables();
			this.Objectives[i].SetupQuestTag();
		}
		for (int j = 0; j < this.Objectives.Count; j++)
		{
			this.Objectives[j].SetPosition(position, size);
		}
	}

	// Token: 0x0600499D RID: 18845 RVA: 0x001CE570 File Offset: 0x001CC770
	public void SetObjectivePosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].OwnerQuest = this;
			this.Objectives[i].HandleVariables();
			this.Objectives[i].SetupQuestTag();
		}
		for (int j = 0; j < this.Objectives.Count; j++)
		{
			this.Objectives[j].SetPosition(dataType, position);
		}
	}

	// Token: 0x0600499E RID: 18846 RVA: 0x001CE5F0 File Offset: 0x001CC7F0
	public bool HandleRallyMarkerActivation(Vector3 prefabPos, bool rallyMarkerActivated, QuestEventManager.POILockoutReasonTypes lockoutReason, ulong extraData)
	{
		Vector3 zero = Vector3.zero;
		if (!this.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition))
		{
			return false;
		}
		if (zero != prefabPos)
		{
			return false;
		}
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].OwnerQuest = this;
			this.Objectives[i].HandleVariables();
			this.Objectives[i].SetupQuestTag();
		}
		for (int j = 0; j < this.Objectives.Count; j++)
		{
			ObjectiveRallyPoint objectiveRallyPoint = this.Objectives[j] as ObjectiveRallyPoint;
			if (objectiveRallyPoint != null)
			{
				objectiveRallyPoint.RallyPointActivate(prefabPos, rallyMarkerActivated, lockoutReason, extraData);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600499F RID: 18847 RVA: 0x001CE6A0 File Offset: 0x001CC8A0
	public void RefreshRallyMarker()
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (this.Objectives[i] is ObjectiveRallyPoint && this.Objectives[i].Phase == this.CurrentPhase)
			{
				(this.Objectives[i] as ObjectiveRallyPoint).RallyPointRefresh();
				return;
			}
		}
	}

	// Token: 0x060049A0 RID: 18848 RVA: 0x001CE708 File Offset: 0x001CC908
	public bool CheckIsQuestGiver(int entityID)
	{
		Entity entity = GameManager.Instance.World.GetEntity(entityID);
		return this.QuestGiverID == entityID || (entity != null && (entity.position - this.GetQuestGiverLocation()).magnitude < 3f);
	}

	// Token: 0x060049A1 RID: 18849 RVA: 0x001CE75C File Offset: 0x001CC95C
	public Vector3 GetQuestGiverLocation()
	{
		Vector3 zero = Vector3.zero;
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.QuestGiver))
		{
			return zero;
		}
		return Vector3.zero;
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x001CE784 File Offset: 0x001CC984
	public Vector3 GetLocation()
	{
		Vector3 zero = Vector3.zero;
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition))
		{
			return zero;
		}
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.TreasurePoint))
		{
			return zero;
		}
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.Location))
		{
			return zero;
		}
		return Vector3.zero;
	}

	// Token: 0x060049A3 RID: 18851 RVA: 0x001CE7C4 File Offset: 0x001CC9C4
	public Vector3 GetLocationSize()
	{
		Vector3 zero = Vector3.zero;
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.POISize))
		{
			return zero;
		}
		return Vector3.zero;
	}

	// Token: 0x060049A4 RID: 18852 RVA: 0x001CE7EC File Offset: 0x001CC9EC
	public Rect GetLocationRect()
	{
		int num = 5;
		Vector3 zero = Vector3.zero;
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition))
		{
			Vector3 zero2 = Vector3.zero;
			if (this.GetPositionData(out zero2, Quest.PositionDataTypes.POISize))
			{
				return new Rect(zero.x - (float)num, zero.z - (float)num, zero2.x + (float)(num * 2), zero2.z + (float)(num * 2));
			}
		}
		return Rect.zero;
	}

	// Token: 0x060049A5 RID: 18853 RVA: 0x001CE850 File Offset: 0x001CCA50
	public void StartQuest(Quest.QuestSource source, bool newQuest = true, bool notify = true)
	{
		if (newQuest)
		{
			this.CurrentState = Quest.QuestState.InProgress;
		}
		for (int i = 0; i < this.Actions.Count; i++)
		{
			this.Actions[i].OwnerQuest = this;
			this.Actions[i].HandleVariables();
			this.Actions[i].SetupAction();
			if (newQuest && this.Actions[i].Phase == (int)this.CurrentPhase && !this.Actions[i].OnComplete)
			{
				this.Actions[i].HandlePerformAction();
			}
		}
		for (int j = 0; j < this.Requirements.Count; j++)
		{
			this.Requirements[j].OwnerQuest = this;
			this.Requirements[j].HandleVariables();
			this.Requirements[j].SetupRequirement();
		}
		for (int k = 0; k < this.Objectives.Count; k++)
		{
			this.Objectives[k].OwnerQuest = this;
			this.Objectives[k].HandleVariables();
			this.Objectives[k].SetupQuestTag();
		}
		for (int l = 0; l < this.Objectives.Count; l++)
		{
			this.Objectives[l].SetupObjective();
			this.Objectives[l].SetupDisplay();
		}
		for (int m = 0; m < this.Objectives.Count; m++)
		{
			if (this.Objectives[m].Phase == this.CurrentPhase)
			{
				if (this.Objectives[m].ObjectiveState == BaseObjective.ObjectiveStates.NotStarted)
				{
					this.Objectives[m].ObjectiveState = BaseObjective.ObjectiveStates.InProgress;
				}
				if (this.CurrentState == Quest.QuestState.InProgress && this.Objectives[m].Phase == this.CurrentPhase)
				{
					this.Objectives[m].HandleRemoveHooks();
					this.Objectives[m].HandleAddHooks();
					this.Objectives[m].Refresh();
				}
			}
		}
		bool flag = false;
		for (int n = 0; n < this.Rewards.Count; n++)
		{
			this.Rewards[n].OwnerQuest = this;
			this.Rewards[n].HandleVariables();
			if (this.Rewards[n].ReceiveStage == BaseReward.ReceiveStages.QuestStart && newQuest)
			{
				this.Rewards[n].GiveReward();
			}
			if (this.Rewards[n].RewardIndex > 0 && !flag)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.SetupRewards();
		}
		if (newQuest && notify)
		{
			QuestClass quest2 = QuestClass.GetQuest(this.ID);
			string text = ((quest2.Name == quest2.SubTitle) ? quest2.Name : string.Format("{0} - {1}", quest2.Name, quest2.SubTitle));
			GameManager.ShowTooltip(this.OwnerJournal.OwnerPlayer, string.Format("{0} {1}: {2}", quest2.Category, Localization.Get("started", false, null), text), false, false, 0f);
			Manager.PlayInsidePlayerHead("quest_started", -1, 0f, false, false);
			GameManager.Instance.StartCoroutine(this.trackLater(this));
		}
		this.SetupQuestCode();
		if (newQuest && !Helper.IsEditingOrPlaytesting())
		{
			QuestEventManager.Current.ForceUpdate();
			PersistentPlayerData persistentPlayerData = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.SharedOwnerID) ?? this.OwnerJournal.OwnerPlayer.PersistentPlayerData;
			EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(persistentPlayerData.EntityId) as EntityPlayer;
			EntityTrader questGiverEntityNPC = GameManager.Instance.World.GetEntity(this.QuestGiverID) as EntityTrader;
			string poinameKey = this.GetPOINameKey();
			string text2 = ((poinameKey != null) ? Localization.Get(poinameKey, false, "english") : null);
			QuestAddedEventData questAddedEventData = new QuestAddedEventData();
			questAddedEventData.ServerId = Helper.GetServerId();
			questAddedEventData.SaveId = Helper.GetSaveId();
			questAddedEventData.QuestAddedTimestamp = DateTime.UtcNow.ToString("O");
			questAddedEventData.AddSource = source.ToString();
			questAddedEventData.QuestInstanceId = this.QuestUniqueId;
			questAddedEventData.QuestId = this.ID;
			questAddedEventData.QuestName = this.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropNameKey);
			questAddedEventData.QuestGroup = this.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropGroupNameKey);
			questAddedEventData.QuestCategory = this.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropCategoryKey);
			questAddedEventData.QuestTier = ((this.QuestClass.DifficultyTier != 0) ? new int?((int)this.QuestClass.DifficultyTier) : null);
			questAddedEventData.QuestTags = this.QuestTags.GetTagNames();
			questAddedEventData.IsShareable = this.QuestClass.Shareable;
			questAddedEventData.QuestOwnerProviderId = persistentPlayerData.NativeId.ReadablePlatformUserIdentifier;
			questAddedEventData.TraderQuestTier = ((entityPlayer != null && questGiverEntityNPC != null && source == Quest.QuestSource.Trader) ? new int?(entityPlayer.QuestJournal.GetCurrentFactionTier(questGiverEntityNPC.NPCInfo.QuestFaction, 0, false)) : null);
			questAddedEventData.OfferedQuests = ((questGiverEntityNPC != null && source == Quest.QuestSource.Trader) ? questGiverEntityNPC.activeQuests.Select<Quest, QuestAddedEventData.OfferedQuest>((Quest quest) => new QuestAddedEventData.OfferedQuest
			{
				quest_id = quest.ID,
				quest_tier = (int)quest.QuestClass.DifficultyTier,
				distance = Vector3.Distance(questGiverEntityNPC.position, quest.GetLocation() + quest.GetLocationSize() * 0.5f)
			}) : null);
			questAddedEventData.QuestDistance = ((source == Quest.QuestSource.ChallengeSystem) ? null : new float?(Vector3.Distance((questGiverEntityNPC != null && source == Quest.QuestSource.Trader) ? questGiverEntityNPC.position : this.OwnerJournal.OwnerPlayer.GetPosition(), this.GetLocation() + this.GetLocationSize() * 0.5f)));
			questAddedEventData.PoiAssigned = text2;
			questAddedEventData.ObjectiveCount = this.questClass.Objectives.Count;
			questAddedEventData.ActiveQuestCount = this.OwnerJournal.quests.Count<Quest>((Quest quest) => quest.CurrentState == Quest.QuestState.InProgress);
			questAddedEventData.TotalCompletedQuestCount = this.OwnerJournal.quests.Count<Quest>((Quest quest) => quest.CurrentState == Quest.QuestState.Completed);
			questAddedEventData.TotalQuestCount = this.OwnerJournal.quests.Count;
			questAddedEventData.CompletionType = this.QuestClass.CompletionType.ToString();
			questAddedEventData.QuestGiverEntityId = ((questGiverEntityNPC != null && source == Quest.QuestSource.Trader) ? questGiverEntityNPC.npcID : null);
			QuestAddedEventData questAddedEventData2 = questAddedEventData;
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(questAddedEventData2);
		}
		this.TrackingHelper.LocalPlayer = this.OwnerJournal.OwnerPlayer;
		this.TrackingHelper.QuestCode = this.QuestCode;
		this.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, false, null);
	}

	// Token: 0x060049A6 RID: 18854 RVA: 0x001CEFA8 File Offset: 0x001CD1A8
	public void SetupSharedQuest()
	{
		this.CurrentState = Quest.QuestState.NotStarted;
		for (int i = 0; i < this.Actions.Count; i++)
		{
			this.Actions[i].OwnerQuest = this;
			this.Actions[i].HandleVariables();
			this.Actions[i].SetupAction();
		}
		for (int j = 0; j < this.Requirements.Count; j++)
		{
			this.Requirements[j].OwnerQuest = this;
			this.Requirements[j].HandleVariables();
			this.Requirements[j].SetupRequirement();
		}
		for (int k = 0; k < this.Objectives.Count; k++)
		{
			this.Objectives[k].OwnerQuest = this;
			this.Objectives[k].HandleVariables();
			this.Objectives[k].SetupObjective();
			this.Objectives[k].SetupDisplay();
		}
		for (int l = 0; l < this.Rewards.Count; l++)
		{
			this.Rewards[l].OwnerQuest = this;
			this.Rewards[l].HandleVariables();
		}
	}

	// Token: 0x060049A7 RID: 18855 RVA: 0x001CF0E4 File Offset: 0x001CD2E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void AdvancePhase()
	{
		byte currentPhase = this.CurrentPhase;
		this.CurrentPhase = currentPhase + 1;
		for (int i = 0; i < this.Actions.Count; i++)
		{
			if (this.Actions[i].Phase == (int)this.CurrentPhase && !this.Actions[i].OnComplete)
			{
				this.Actions[i].HandlePerformAction();
			}
		}
		for (int j = 0; j < this.Objectives.Count; j++)
		{
			if (this.CurrentState == Quest.QuestState.InProgress)
			{
				if (this.Objectives[j].Phase == this.CurrentPhase - 1)
				{
					this.Objectives[j].HandlePhaseCompleted();
				}
				if (this.Objectives[j].Phase == this.CurrentPhase || this.Objectives[j].Phase == 0)
				{
					if (this.Objectives[j].ObjectiveState == BaseObjective.ObjectiveStates.NotStarted)
					{
						this.Objectives[j].ObjectiveState = BaseObjective.ObjectiveStates.InProgress;
					}
					this.Objectives[j].HandleRemoveHooks();
					this.Objectives[j].HandleAddHooks();
				}
				else
				{
					this.Objectives[j].HandleRemoveHooks();
				}
			}
			else
			{
				this.Objectives[j].HandleRemoveHooks();
			}
		}
	}

	// Token: 0x060049A8 RID: 18856 RVA: 0x001CF244 File Offset: 0x001CD444
	public void SetupRewards()
	{
		int num = 0;
		for (int i = 0; i < this.Rewards.Count; i++)
		{
			BaseReward baseReward = this.Rewards[i];
			baseReward.RewardIndex = (byte)i;
			if (!baseReward.isChainReward && baseReward.isChosenReward && !baseReward.isFixedLocation)
			{
				num++;
			}
		}
		if (num > 1)
		{
			World world = this.OwnerJournal.OwnerPlayer.world;
			for (int j = 0; j < 100; j++)
			{
				int num2 = world.GetGameRandom().RandomRange(this.Rewards.Count);
				int num3 = world.GetGameRandom().RandomRange(this.Rewards.Count);
				if (num2 != num3)
				{
					BaseReward baseReward2 = this.Rewards[num2];
					BaseReward baseReward3 = this.Rewards[num3];
					if (!baseReward2.isFixedLocation && baseReward2.isChosenReward && !baseReward3.isFixedLocation && baseReward3.isChosenReward)
					{
						byte rewardIndex = this.Rewards[num2].RewardIndex;
						this.Rewards[num2].RewardIndex = this.Rewards[num3].RewardIndex;
						this.Rewards[num3].RewardIndex = rewardIndex;
					}
				}
			}
		}
	}

	// Token: 0x060049A9 RID: 18857 RVA: 0x001CF390 File Offset: 0x001CD590
	public bool HandleActivateListReceived(Vector3 prefabPos, List<Vector3i> activateList)
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (this.Objectives[i].SetupActivationList(prefabPos, activateList))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060049AA RID: 18858 RVA: 0x001CF3CB File Offset: 0x001CD5CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator trackLater(Quest q)
	{
		yield return new WaitForSeconds(0.5f);
		if (XUi.IsGameRunning())
		{
			if (q.CurrentState == Quest.QuestState.InProgress)
			{
				if (this.OwnerJournal != null && null != this.OwnerJournal.OwnerPlayer)
				{
					LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.OwnerJournal.OwnerPlayer);
					if (null != uiforPlayer && null != uiforPlayer.xui && uiforPlayer.xui.QuestTracker != null)
					{
						XUiM_Quest questTracker = uiforPlayer.xui.QuestTracker;
						if (uiforPlayer.xui.Recipes.TrackedRecipe == null && questTracker.TrackedChallenge == null && questTracker.TrackedQuest == null)
						{
							q.Tracked = uiforPlayer.xui.QuestTracker.TrackedQuest == null || uiforPlayer.xui.QuestTracker.TrackedQuest == q;
						}
					}
					else
					{
						q.Tracked = false;
					}
					this.OwnerJournal.RefreshTracked();
				}
				else
				{
					q.Tracked = false;
				}
			}
			else
			{
				q.Tracked = false;
			}
		}
		yield break;
	}

	// Token: 0x060049AB RID: 18859 RVA: 0x001CF3E4 File Offset: 0x001CD5E4
	public bool CheckRequirements()
	{
		for (int i = 0; i < this.Requirements.Count; i++)
		{
			if ((this.Requirements[i].Phase == 0 || this.Requirements[i].Phase == (int)this.CurrentPhase) && !this.Requirements[i].CheckRequirement())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060049AC RID: 18860 RVA: 0x001CF44C File Offset: 0x001CD64C
	public Quest Clone()
	{
		Quest quest = new Quest(this.ID);
		quest.ID = this.ID;
		quest.OwnerJournal = this.OwnerJournal;
		quest.CurrentQuestVersion = this.CurrentQuestVersion;
		quest.CurrentState = this.CurrentState;
		quest.FinishTime = this.FinishTime;
		quest.SharedOwnerID = this.SharedOwnerID;
		quest.QuestGiverID = this.QuestGiverID;
		quest.CurrentPhase = this.CurrentPhase;
		quest.QuestUniqueId = this.QuestUniqueId;
		quest.QuestCode = this.QuestCode;
		quest.RallyMarkerActivated = this.RallyMarkerActivated;
		quest.Tracked = this.Tracked;
		quest.OptionalComplete = this.OptionalComplete;
		quest.QuestTags = this.QuestTags;
		quest.TrackingHelper = this.TrackingHelper;
		quest.QuestFaction = this.QuestFaction;
		quest.QuestProgressDay = this.QuestProgressDay;
		for (int i = 0; i < this.Actions.Count; i++)
		{
			BaseQuestAction baseQuestAction = this.Actions[i].Clone();
			baseQuestAction.OwnerQuest = quest;
			quest.Actions.Add(baseQuestAction);
		}
		for (int j = 0; j < this.Requirements.Count; j++)
		{
			BaseRequirement baseRequirement = this.Requirements[j].Clone();
			baseRequirement.OwnerQuest = quest;
			quest.Requirements.Add(baseRequirement);
		}
		for (int k = 0; k < this.Objectives.Count; k++)
		{
			BaseObjective baseObjective = this.Objectives[k].Clone();
			baseObjective.OwnerQuest = quest;
			quest.Objectives.Add(baseObjective);
		}
		for (int l = 0; l < this.Rewards.Count; l++)
		{
			BaseReward baseReward = this.Rewards[l].Clone();
			baseReward.OwnerQuest = quest;
			quest.Rewards.Add(baseReward);
		}
		foreach (KeyValuePair<string, string> keyValuePair in this.DataVariables)
		{
			quest.DataVariables.Add(keyValuePair.Key, keyValuePair.Value);
		}
		foreach (KeyValuePair<Quest.PositionDataTypes, Vector3> keyValuePair2 in this.PositionData)
		{
			quest.PositionData.Add(keyValuePair2.Key, keyValuePair2.Value);
		}
		return quest;
	}

	// Token: 0x060049AD RID: 18861 RVA: 0x001CF6E4 File Offset: 0x001CD8E4
	public void ResetObjectives()
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].ResetObjective();
		}
	}

	// Token: 0x060049AE RID: 18862 RVA: 0x001CF718 File Offset: 0x001CD918
	public void ResetQuest()
	{
		this.ResetObjectives();
		this.CurrentPhase = 0;
		this.CurrentState = Quest.QuestState.InProgress;
	}

	// Token: 0x060049AF RID: 18863 RVA: 0x001CF730 File Offset: 0x001CD930
	public void ResetToRallyPointObjective()
	{
		if (this.CurrentPhase == this.QuestClass.HighestPhase || !this.QuestClass.LoginRallyReset)
		{
			return;
		}
		this.RallyMarkerActivated = false;
		int num = -1;
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (this.Objectives[i] is ObjectiveRallyPoint)
			{
				num = (int)this.Objectives[i].Phase;
			}
			if (num != -1 && (int)this.Objectives[i].Phase >= num && this.Objectives[i].Phase <= this.CurrentPhase)
			{
				this.Objectives[i].ResetObjective();
			}
		}
		if (num != -1 && num < (int)this.CurrentPhase)
		{
			this.CurrentPhase = (byte)num;
		}
	}

	// Token: 0x060049B0 RID: 18864 RVA: 0x001CF7F9 File Offset: 0x001CD9F9
	public void RefreshQuestCompletion(QuestClass.CompletionTypes currentCompletionType = QuestClass.CompletionTypes.AutoComplete, List<BaseReward> rewardChoice = null, bool playObjectiveComplete = true, EntityNPC turnInNPC = null)
	{
		this.refreshQuestCompletion(currentCompletionType, rewardChoice, playObjectiveComplete, turnInNPC);
		PrefabInstance.RefreshSwitchesInContainingPoi(this);
	}

	// Token: 0x060049B1 RID: 18865 RVA: 0x001CF80C File Offset: 0x001CDA0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void refreshQuestCompletion(QuestClass.CompletionTypes currentCompletionType, List<BaseReward> rewardChoice, bool playObjectiveComplete, EntityNPC turnInNPC)
	{
		if (this.CurrentState != Quest.QuestState.InProgress && this.CurrentState != Quest.QuestState.ReadyForTurnIn)
		{
			return;
		}
		if (this.OwnerJournal == null)
		{
			return;
		}
		if (this.CurrentState == Quest.QuestState.InProgress)
		{
			this.OwnerJournal.RefreshQuest(this);
			this.OptionalComplete = true;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.Objectives.Count; i++)
			{
				if (this.Objectives[i].Phase == this.CurrentPhase || this.Objectives[i].Phase == 0)
				{
					if (this.Objectives[i].Optional)
					{
						if (!this.Objectives[i].Complete)
						{
							this.OptionalComplete = false;
						}
					}
					else if (!this.Objectives[i].Complete && !this.Objectives[i].AlwaysComplete)
					{
						flag = true;
					}
					else if (this.Objectives[i].ForcePhaseFinish)
					{
						flag2 = true;
					}
				}
			}
			if (flag)
			{
				if (flag2)
				{
					this.CloseQuest(Quest.QuestState.Failed, null);
					return;
				}
				return;
			}
			else if (this.CurrentPhase < this.QuestClass.HighestPhase)
			{
				if (!Helper.IsEditingOrPlaytesting())
				{
					foreach (BaseObjective baseObjective in this.Objectives.Where<BaseObjective>((BaseObjective objective) => objective.Phase == this.CurrentPhase && objective.ObjectiveState == BaseObjective.ObjectiveStates.Complete).ToList<BaseObjective>())
					{
						QuestProgressionEventData questProgressionEventData = new QuestProgressionEventData(baseObjective);
						ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(questProgressionEventData);
					}
				}
				this.AdvancePhase();
				if (playObjectiveComplete)
				{
					Manager.PlayInsidePlayerHead("quest_objective_complete", -1, 0f, false, false);
				}
				if (this.CurrentPhase == this.QuestClass.HighestPhase && this.OwnerJournal.ActiveQuest == this)
				{
					this.OwnerJournal.ActiveQuest = null;
					this.OwnerJournal.RefreshRallyMarkerPositions();
				}
				return;
			}
		}
		if (currentCompletionType != this.QuestClass.CompletionType)
		{
			this.CurrentState = Quest.QuestState.ReadyForTurnIn;
			return;
		}
		QuestEventManager.Current.QuestCompleted(this.QuestTags, this.questClass);
		this.CloseQuest(Quest.QuestState.Completed, rewardChoice);
		if (this.QuestClass.ResetTraderQuests)
		{
			EntityTrader entityTrader = turnInNPC as EntityTrader;
			if (entityTrader != null)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					EntityPlayerLocal ownerPlayer = this.OwnerJournal.OwnerPlayer;
					entityTrader.ClearActiveQuests(ownerPlayer.entityId);
					entityTrader.SetupActiveQuestsForPlayer(ownerPlayer, -1);
					return;
				}
				EntityPlayer ownerPlayer2 = this.OwnerJournal.OwnerPlayer;
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests, ownerPlayer2.entityId, this.QuestGiverID, entityTrader.GetQuestFactionPoints(ownerPlayer2)), false);
			}
		}
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x001CFAB8 File Offset: 0x001CDCB8
	public void CloseQuest(Quest.QuestState finalState, List<BaseReward> rewardChoice = null)
	{
		if (!Quest._endableStates.Contains(finalState))
		{
			Log.Warning(string.Format("Ending a quest in a state {0}. Should be: {1}.", finalState, string.Join<Quest.QuestState>(", ", Quest._endableStates)));
		}
		if (this.OwnerJournal == null)
		{
			return;
		}
		if (!Helper.IsEditingOrPlaytesting())
		{
			foreach (BaseObjective baseObjective in this.Objectives.Where<BaseObjective>((BaseObjective objective) => (objective.Phase == this.CurrentPhase && objective.ObjectiveState == BaseObjective.ObjectiveStates.Complete) || objective.ObjectiveState == BaseObjective.ObjectiveStates.Failed).ToList<BaseObjective>())
			{
				QuestProgressionEventData questProgressionEventData = new QuestProgressionEventData(baseObjective);
				ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(questProgressionEventData);
			}
		}
		this.OwnerJournal.RefreshQuest(this);
		this.CurrentState = finalState;
		bool flag = finalState == Quest.QuestState.Completed;
		this.HandleUnlockPOI(null);
		bool flag2 = !string.IsNullOrEmpty(this.PreviousQuest);
		bool flag3 = flag && flag2;
		if (flag3)
		{
			for (int i = 0; i < this.Rewards.Count; i++)
			{
				if (this.Rewards[i] is RewardQuest && (this.Rewards[i] as RewardQuest).IsChainQuest)
				{
					flag3 = false;
				}
			}
		}
		ToolTipEvent toolTipEvent = new ToolTipEvent();
		for (int j = 0; j < this.Rewards.Count; j++)
		{
			if (this.Rewards[j].ReceiveStage == BaseReward.ReceiveStages.AfterCompleteNotification)
			{
				toolTipEvent.EventHandler += this.QuestRewardsLater_Event;
				toolTipEvent.Parameter = this;
				break;
			}
		}
		EntityPlayerLocal ownerPlayer = this.OwnerJournal.OwnerPlayer;
		string text = ((this.questClass.Name == this.questClass.SubTitle) ? this.questClass.Name : string.Format("{0} - {1}", this.questClass.Name, this.questClass.SubTitle));
		string text2 = (flag ? Localization.Get("completed", false, null) : Localization.Get("failed", false, null));
		string text3 = (flag ? "quest_subtask_complete" : "quest_failed");
		ToolTipEvent toolTipEvent2 = ((flag && !flag3) ? toolTipEvent : null);
		GameManager.ShowTooltip(ownerPlayer, string.Format("{0} {1}: {2}", this.questClass.Category, text2, text), string.Empty, text3, toolTipEvent2, false, false, 0f);
		if (flag3)
		{
			GameManager.ShowTooltip(ownerPlayer, string.Format("{0} {1}: {2}", Localization.Get("questChain", false, null), Localization.Get("completed", false, null), this.questClass.GroupName), string.Empty, "quest_master_complete", toolTipEvent, false, false, 0f);
		}
		if (this.OwnerJournal.TrackedQuest == this)
		{
			this.OwnerJournal.TrackedQuest = null;
		}
		for (int k = 0; k < this.Objectives.Count; k++)
		{
			this.Objectives[k].HandleRemoveHooks();
		}
		for (int l = 0; l < this.Objectives.Count; l++)
		{
			this.Objectives[l].RemoveObjectives();
			if (flag)
			{
				this.Objectives[l].HandleCompleted();
			}
			else
			{
				this.Objectives[l].HandleFailed();
			}
		}
		this.RemoveMapObject();
		if (flag)
		{
			if (this.AddsProgression)
			{
				QuestEventManager.Current.HandleNewCompletedQuest(this.OwnerJournal.OwnerPlayer, this.QuestFaction, (int)this.QuestClass.DifficultyTier, this.QuestClass.AddsToTierComplete);
			}
			for (int m = 0; m < this.Rewards.Count; m++)
			{
				if (this.Rewards[m].ReceiveStage == BaseReward.ReceiveStages.QuestCompletion && (!this.Rewards[m].Optional || (this.Rewards[m].Optional && this.OptionalComplete)) && (!this.Rewards[m].isChosenReward || (this.Rewards[m].isChosenReward && rewardChoice != null && rewardChoice.Contains(this.Rewards[m]))))
				{
					this.Rewards[m].GiveReward();
				}
			}
			this.OwnerJournal.CompleteQuest(this);
			for (int n = 0; n < this.Actions.Count; n++)
			{
				BaseQuestAction baseQuestAction = this.Actions[n];
				if (baseQuestAction.OnComplete)
				{
					baseQuestAction.HandlePerformAction();
				}
			}
		}
		else
		{
			this.OptionalComplete = false;
			this.tracked = false;
			this.OwnerJournal.FailedQuest(this);
		}
		if (this.OwnerJournal.ActiveQuest == this)
		{
			this.OwnerJournal.ActiveQuest = null;
			this.OwnerJournal.RefreshRallyMarkerPositions();
		}
		if (this.QuestClass.ResetTraderQuests)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				QuestEventManager.Current.AddTraderResetQuestsForPlayer(this.OwnerJournal.OwnerPlayer.entityId, this.QuestGiverID);
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests, this.OwnerJournal.OwnerPlayer.entityId, this.QuestGiverID, -1), false);
			}
		}
		if (Helper.IsEditingOrPlaytesting())
		{
			return;
		}
		PersistentPlayerData persistentPlayerData = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.SharedOwnerID) ?? this.OwnerJournal.OwnerPlayer.PersistentPlayerData;
		QuestRemovedEventData questRemovedEventData = new QuestRemovedEventData
		{
			ServerId = Helper.GetServerId(),
			SaveId = Helper.GetSaveId(),
			QuestRemovedTimestamp = DateTime.UtcNow.ToString("O"),
			RemoveReason = finalState.ToString(),
			QuestInstanceId = this.QuestUniqueId,
			QuestId = this.ID,
			QuestName = this.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropNameKey),
			QuestGroup = this.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropGroupNameKey),
			QuestCategory = this.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropCategory),
			QuestTier = ((this.QuestClass.DifficultyTier != 0) ? new int?((int)this.QuestClass.DifficultyTier) : null),
			QuestOwnerProviderId = persistentPlayerData.NativeId.ReadablePlatformUserIdentifier
		};
		ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(questRemovedEventData);
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x001D0120 File Offset: 0x001CE320
	public void HandleUnlockPOI(EntityPlayer player = null)
	{
		Vector3 zero = Vector3.zero;
		if (this.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition))
		{
			if (player == null)
			{
				player = this.OwnerJournal.OwnerPlayer;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				QuestEventManager.Current.QuestUnlockPOI(player.entityId, zero);
				return;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.UnlockPOI, player.entityId, zero), false);
		}
	}

	// Token: 0x060049B4 RID: 18868 RVA: 0x001D0190 File Offset: 0x001CE390
	public void HandleQuestEvent(Quest ownerQuest, string eventType)
	{
		for (int i = 0; i < this.questClass.Events.Count; i++)
		{
			if (this.questClass.Events[i].EventType == eventType)
			{
				this.questClass.Events[i].HandleEvent(ownerQuest);
			}
		}
	}

	// Token: 0x060049B5 RID: 18869 RVA: 0x001D01F0 File Offset: 0x001CE3F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestRewardsLater_Event(object obj)
	{
		Quest quest = (Quest)obj;
		GameManager.Instance.StartCoroutine(this.GiveRewardsLater(quest));
	}

	// Token: 0x060049B6 RID: 18870 RVA: 0x001D0216 File Offset: 0x001CE416
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator GiveRewardsLater(Quest q)
	{
		yield return new WaitForSeconds(3f);
		if (XUi.IsGameRunning() && q.CurrentState == Quest.QuestState.Completed)
		{
			for (int i = 0; i < this.Rewards.Count; i++)
			{
				if (this.Rewards[i].ReceiveStage == BaseReward.ReceiveStages.AfterCompleteNotification && (!this.Rewards[i].Optional || (this.Rewards[i].Optional && q.OptionalComplete)))
				{
					this.Rewards[i].GiveReward();
				}
			}
		}
		yield break;
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x001D022C File Offset: 0x001CE42C
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestAction AddAction(BaseQuestAction action)
	{
		if (action != null)
		{
			this.Actions.Add(action);
		}
		return action;
	}

	// Token: 0x060049B8 RID: 18872 RVA: 0x001D023E File Offset: 0x001CE43E
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseRequirement AddRequirement(BaseRequirement requirement)
	{
		if (requirement != null)
		{
			this.Requirements.Add(requirement);
		}
		return requirement;
	}

	// Token: 0x060049B9 RID: 18873 RVA: 0x001D0250 File Offset: 0x001CE450
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseObjective AddObjective(BaseObjective objective)
	{
		if (objective != null)
		{
			this.Objectives.Add(objective);
		}
		return objective;
	}

	// Token: 0x060049BA RID: 18874 RVA: 0x001D0262 File Offset: 0x001CE462
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseReward AddReward(BaseReward reward)
	{
		if (reward != null)
		{
			this.Rewards.Add(reward);
		}
		return reward;
	}

	// Token: 0x060049BB RID: 18875 RVA: 0x001D0274 File Offset: 0x001CE474
	public string ParseVariable(string value)
	{
		if (value != null && value.Contains("{"))
		{
			int num = value.IndexOf("{") + 1;
			int num2 = value.IndexOf("}", num);
			if (num2 != -1)
			{
				string text = value.Substring(num, num2 - num);
				if (this.DataVariables.ContainsKey(text))
				{
					value = this.DataVariables[text];
				}
			}
		}
		return value;
	}

	// Token: 0x060049BC RID: 18876 RVA: 0x001D02D8 File Offset: 0x001CE4D8
	public void Read(PooledBinaryReader _br)
	{
		bool flag = true;
		this.CurrentFileVersion = _br.ReadByte();
		this.CurrentState = (Quest.QuestState)_br.ReadByte();
		this.SharedOwnerID = _br.ReadInt32();
		this.QuestGiverID = _br.ReadInt32();
		if (this.CurrentState == Quest.QuestState.InProgress)
		{
			this.tracked = _br.ReadBoolean();
			this.CurrentPhase = _br.ReadByte();
			this.QuestCode = _br.ReadInt32();
			this.QuestUniqueId = ((this.CurrentFileVersion >= 9) ? _br.ReadString() : this.QuestCode.ToString());
		}
		else if (this.CurrentState == Quest.QuestState.Completed)
		{
			this.CurrentPhase = this.QuestClass.HighestPhase;
		}
		PooledBinaryReader.StreamReadSizeMarker streamReadSizeMarker = default(PooledBinaryReader.StreamReadSizeMarker);
		if (this.CurrentFileVersion >= 7)
		{
			streamReadSizeMarker = _br.ReadSizeMarker(PooledBinaryWriter.EMarkerSize.UInt16);
		}
		try
		{
			for (int i = 0; i < this.Objectives.Count; i++)
			{
				this.Objectives[i].Read(_br);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
		}
		finally
		{
			uint num;
			if (this.CurrentFileVersion >= 7 && !_br.ValidateSizeMarker(ref streamReadSizeMarker, out num, true))
			{
				this.Objectives.Clear();
				Log.Error("Loading player quests: Quest with ID " + this.ID + ": Failed loading objectives");
				flag = false;
			}
		}
		int num2 = (int)_br.ReadByte();
		for (int j = 0; j < num2; j++)
		{
			string text = _br.ReadString();
			string text2 = _br.ReadString();
			if (!this.DataVariables.ContainsKey(text))
			{
				this.DataVariables.Add(text, text2);
			}
			else
			{
				this.DataVariables[text] = text2;
			}
		}
		if (this.CurrentState == Quest.QuestState.InProgress)
		{
			this.PositionData.Clear();
			int num3 = (int)_br.ReadByte();
			for (int k = 0; k < num3; k++)
			{
				Quest.PositionDataTypes positionDataTypes = (Quest.PositionDataTypes)_br.ReadByte();
				Vector3 vector = StreamUtils.ReadVector3(_br);
				this.SetPositionData(positionDataTypes, vector);
			}
			this.RallyMarkerActivated = _br.ReadBoolean();
		}
		else
		{
			this.FinishTime = _br.ReadUInt64();
		}
		if (this.CurrentState == Quest.QuestState.InProgress || this.CurrentState == Quest.QuestState.ReadyForTurnIn)
		{
			PooledBinaryReader.StreamReadSizeMarker streamReadSizeMarker2 = default(PooledBinaryReader.StreamReadSizeMarker);
			if (this.CurrentFileVersion >= 7)
			{
				streamReadSizeMarker2 = _br.ReadSizeMarker(PooledBinaryWriter.EMarkerSize.UInt16);
			}
			try
			{
				if (this.CurrentFileVersion <= 5)
				{
					for (int l = 0; l < this.Rewards.Count; l++)
					{
						this.Rewards[l].Read(_br);
					}
				}
				else
				{
					int num4 = _br.ReadInt32();
					for (int m = 0; m < num4; m++)
					{
						this.Rewards[m].Read(_br);
					}
				}
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			finally
			{
				uint num;
				if (this.CurrentFileVersion >= 7 && !_br.ValidateSizeMarker(ref streamReadSizeMarker2, out num, true))
				{
					this.Rewards.Clear();
					Log.Error("Loading player quests: Quest with ID " + this.ID + ": Failed loading rewards");
					flag = false;
				}
			}
		}
		if (this.CurrentFileVersion > 4)
		{
			this.QuestFaction = _br.ReadByte();
		}
		if (!flag && this.CurrentState != Quest.QuestState.Completed)
		{
			this.CurrentState = Quest.QuestState.Failed;
		}
		if (this.CurrentFileVersion >= 8)
		{
			this.QuestProgressDay = _br.ReadInt32();
		}
	}

	// Token: 0x060049BD RID: 18877 RVA: 0x001D061C File Offset: 0x001CE81C
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.ID);
		_bw.Write(this.CurrentQuestVersion);
		_bw.Write(Quest.FileVersion);
		_bw.Write((byte)this.CurrentState);
		_bw.Write(this.SharedOwnerID);
		_bw.Write(this.QuestGiverID);
		if (this.CurrentState == Quest.QuestState.InProgress)
		{
			_bw.Write(this.Tracked);
			_bw.Write(this.CurrentPhase);
			_bw.Write(this.QuestCode);
			_bw.Write(this.QuestUniqueId);
		}
		PooledBinaryWriter.StreamWriteSizeMarker streamWriteSizeMarker = _bw.ReserveSizeMarker(PooledBinaryWriter.EMarkerSize.UInt16);
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].Write(_bw);
		}
		_bw.FinalizeSizeMarker(ref streamWriteSizeMarker);
		_bw.Write((byte)this.DataVariables.Count);
		foreach (KeyValuePair<string, string> keyValuePair in this.DataVariables)
		{
			_bw.Write(keyValuePair.Key);
			_bw.Write(keyValuePair.Value);
		}
		if (this.CurrentState == Quest.QuestState.InProgress)
		{
			_bw.Write((byte)this.PositionData.Count);
			foreach (KeyValuePair<Quest.PositionDataTypes, Vector3> keyValuePair2 in this.PositionData)
			{
				_bw.Write((byte)keyValuePair2.Key);
				StreamUtils.Write(_bw, keyValuePair2.Value);
			}
			_bw.Write(this.RallyMarkerActivated);
		}
		else
		{
			_bw.Write(this.FinishTime);
		}
		if (this.CurrentState == Quest.QuestState.InProgress || this.CurrentState == Quest.QuestState.ReadyForTurnIn)
		{
			PooledBinaryWriter.StreamWriteSizeMarker streamWriteSizeMarker2 = _bw.ReserveSizeMarker(PooledBinaryWriter.EMarkerSize.UInt16);
			_bw.Write(this.Rewards.Count);
			for (int j = 0; j < this.Rewards.Count; j++)
			{
				this.Rewards[j].Write(_bw);
			}
			_bw.FinalizeSizeMarker(ref streamWriteSizeMarker2);
		}
		_bw.Write(this.QuestFaction);
		_bw.Write(this.QuestProgressDay);
	}

	// Token: 0x060049BE RID: 18878 RVA: 0x001D0854 File Offset: 0x001CEA54
	public void AddSharedLocation(Vector3 pos, Vector3 size)
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (this.Objectives[i].Phase == this.CurrentPhase && this.Objectives[i].SetLocation(pos, size))
			{
				return;
			}
		}
	}

	// Token: 0x060049BF RID: 18879 RVA: 0x001D08A8 File Offset: 0x001CEAA8
	public void AddSharedKill(string enemyType)
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (this.Objectives[i].Phase == this.CurrentPhase && this.Objectives[i].ID == enemyType)
			{
				BaseObjective baseObjective = this.Objectives[i];
				byte currentValue = baseObjective.CurrentValue;
				baseObjective.CurrentValue = currentValue + 1;
				this.Objectives[i].Refresh();
			}
		}
	}

	// Token: 0x060049C0 RID: 18880 RVA: 0x001D092A File Offset: 0x001CEB2A
	public int GetSharedWithCount()
	{
		if (this.sharedWithList == null)
		{
			return 0;
		}
		return this.sharedWithList.Count;
	}

	// Token: 0x060049C1 RID: 18881 RVA: 0x001D0944 File Offset: 0x001CEB44
	public int GetSharedWithCountNotInRange()
	{
		if (this.sharedWithList == null)
		{
			return 0;
		}
		EntityPlayer ownerPlayer = this.OwnerJournal.OwnerPlayer;
		int num = 0;
		Rect locationRect = this.GetLocationRect();
		for (int i = 0; i < this.sharedWithList.Count; i++)
		{
			EntityPlayer entityPlayer = this.sharedWithList[i];
			if (locationRect != Rect.zero)
			{
				Vector3 vector = entityPlayer.position;
				vector.y = vector.z;
				if (!locationRect.Contains(vector))
				{
					num++;
				}
			}
			else if (Vector3.Distance(ownerPlayer.position, entityPlayer.position) >= 15f)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060049C2 RID: 18882 RVA: 0x001D09E8 File Offset: 0x001CEBE8
	public List<EntityPlayer> GetSharedWithListNotInRange()
	{
		if (this.sharedWithList == null)
		{
			return null;
		}
		EntityPlayer ownerPlayer = this.OwnerJournal.OwnerPlayer;
		Rect locationRect = this.GetLocationRect();
		List<EntityPlayer> list = new List<EntityPlayer>();
		for (int i = 0; i < this.sharedWithList.Count; i++)
		{
			EntityPlayer entityPlayer = this.sharedWithList[i];
			if (locationRect != Rect.zero)
			{
				Vector3 vector = entityPlayer.position;
				vector.y = vector.z;
				if (!locationRect.Contains(vector))
				{
					list.Add(entityPlayer);
				}
			}
			else if (Vector3.Distance(ownerPlayer.position, entityPlayer.position) >= 15f)
			{
				list.Add(entityPlayer);
			}
		}
		return list;
	}

	// Token: 0x060049C3 RID: 18883 RVA: 0x001D0A98 File Offset: 0x001CEC98
	public void RemoveSharedNotInRange()
	{
		if (this.sharedWithList == null)
		{
			return;
		}
		EntityPlayer ownerPlayer = this.OwnerJournal.OwnerPlayer;
		Rect locationRect = this.GetLocationRect();
		for (int i = this.sharedWithList.Count - 1; i >= 0; i--)
		{
			EntityPlayer entityPlayer = this.sharedWithList[i];
			if (locationRect != Rect.zero)
			{
				Vector3 vector = entityPlayer.position;
				vector.y = vector.z;
				if (!locationRect.Contains(vector))
				{
					this.sharedWithList.RemoveAt(i);
				}
			}
			else if (Vector3.Distance(ownerPlayer.position, entityPlayer.position) >= 15f)
			{
				this.sharedWithList.RemoveAt(i);
			}
		}
	}

	// Token: 0x060049C4 RID: 18884 RVA: 0x001D0B48 File Offset: 0x001CED48
	public int[] GetSharedWithIDList()
	{
		if (this.sharedWithList == null)
		{
			return null;
		}
		int[] array = new int[this.sharedWithList.Count];
		for (int i = 0; i < this.sharedWithList.Count; i++)
		{
			array[i] = this.sharedWithList[i].entityId;
		}
		return array;
	}

	// Token: 0x060049C5 RID: 18885 RVA: 0x001D0B9C File Offset: 0x001CED9C
	public bool HasSharedWith(EntityPlayer player)
	{
		if (this.sharedWithList == null)
		{
			return false;
		}
		for (int i = 0; i < this.sharedWithList.Count; i++)
		{
			if (this.sharedWithList[i] == player)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060049C6 RID: 18886 RVA: 0x001D0BE0 File Offset: 0x001CEDE0
	public void AddSharedWith(EntityPlayer player)
	{
		if (this.sharedWithList == null)
		{
			this.sharedWithList = new List<EntityPlayer>();
		}
		if (!this.sharedWithList.Contains(player))
		{
			this.sharedWithList.Add(player);
		}
	}

	// Token: 0x060049C7 RID: 18887 RVA: 0x001D0C10 File Offset: 0x001CEE10
	public bool RemoveSharedWith(EntityPlayer player)
	{
		bool flag = false;
		if (this.sharedWithList == null)
		{
			return false;
		}
		for (int i = this.sharedWithList.Count - 1; i >= 0; i--)
		{
			if (this.sharedWithList[i].entityId == player.entityId)
			{
				flag = true;
				this.sharedWithList.RemoveAt(i);
			}
		}
		if (this.sharedWithList.Count == 0)
		{
			this.sharedWithList = null;
		}
		return flag;
	}

	// Token: 0x060049C8 RID: 18888 RVA: 0x001D0C80 File Offset: 0x001CEE80
	public void SetPositionData(Quest.PositionDataTypes dataType, Vector3 value)
	{
		if (!this.PositionData.ContainsKey(dataType))
		{
			this.PositionData.Add(dataType, value);
		}
		else
		{
			this.PositionData[dataType] = value;
		}
		if (this.OwnerJournal != null)
		{
			PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.OwnerJournal.OwnerPlayer.entityId);
			if (playerDataFromEntityID != null)
			{
				playerDataFromEntityID.AddQuestPosition(this.QuestCode, dataType, value);
			}
		}
	}

	// Token: 0x060049C9 RID: 18889 RVA: 0x001D0CF0 File Offset: 0x001CEEF0
	public void RemovePositionData(Quest.PositionDataTypes dataType)
	{
		if (this.PositionData.ContainsKey(dataType))
		{
			this.PositionData.Remove(dataType);
		}
	}

	// Token: 0x060049CA RID: 18890 RVA: 0x001D0D0D File Offset: 0x001CEF0D
	public bool GetPositionData(out Vector3 pos, Quest.PositionDataTypes dataType)
	{
		if (this.PositionData.ContainsKey(dataType))
		{
			pos = this.PositionData[dataType];
			return true;
		}
		pos = Vector3.zero;
		return false;
	}

	// Token: 0x060049CB RID: 18891 RVA: 0x001D0D3D File Offset: 0x001CEF3D
	public string GetParsedText(string text)
	{
		if (text.Contains("{"))
		{
			text = this.ParseBindingVariables(text);
		}
		return text;
	}

	// Token: 0x060049CC RID: 18892 RVA: 0x001D0D58 File Offset: 0x001CEF58
	[PublicizedFrom(EAccessModifier.Private)]
	public string ParseBindingVariables(string response)
	{
		if (string.IsNullOrEmpty(response))
		{
			return response;
		}
		int num = response.IndexOf('{');
		while (num != -1)
		{
			int num2 = response.IndexOf('}', num);
			if (num2 != -1)
			{
				string text = response.Substring(num, num2 - num + 1);
				string[] array = text.Substring(1, text.Length - 2).Split(new char[] { '_', '.' });
				if (array.Length == 2)
				{
					response = response.Replace(text, this.GetVariableText(array[0], -1, array[1]));
				}
				if (array.Length == 3)
				{
					response = response.Replace(text, this.GetVariableText(array[0], Convert.ToInt32(array[1]), array[2]));
				}
			}
			if (num + 1 < response.Length)
			{
				num = response.IndexOf('{', num + 1);
			}
			else
			{
				num = -1;
			}
		}
		return response;
	}

	// Token: 0x060049CD RID: 18893 RVA: 0x001D0E20 File Offset: 0x001CF020
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetVariableText(string field, int index, string variableName)
	{
		int num = 0;
		if (!(field == "fetch"))
		{
			if (!(field == "buff"))
			{
				if (!(field == "kill"))
				{
					if (!(field == "goto"))
					{
						if (!(field == "poi"))
						{
							if (field == "treasure")
							{
								for (int i = 0; i < this.Objectives.Count; i++)
								{
									if (this.Objectives[i] is ObjectiveTreasureChest && (++num == index || index == -1))
									{
										return this.Objectives[i].ParseBinding(variableName);
									}
								}
							}
						}
						else
						{
							for (int j = 0; j < this.Objectives.Count; j++)
							{
								if (this.Objectives[j] is ObjectiveRandomPOIGoto && (++num == index || index == -1))
								{
									return this.Objectives[j].ParseBinding(variableName);
								}
							}
						}
					}
					else
					{
						for (int k = 0; k < this.Objectives.Count; k++)
						{
							if (this.Objectives[k] is ObjectiveGoto && (++num == index || index == -1))
							{
								return this.Objectives[k].ParseBinding(variableName);
							}
						}
					}
				}
				else
				{
					for (int l = 0; l < this.Objectives.Count; l++)
					{
						if (this.Objectives[l] is ObjectiveEntityKill && (++num == index || index == -1))
						{
							return this.Objectives[l].ParseBinding(variableName);
						}
					}
				}
			}
			else
			{
				for (int m = 0; m < this.Objectives.Count; m++)
				{
					if (this.Objectives[m] is ObjectiveBuff && (++num == index || index == -1))
					{
						return this.Objectives[m].ParseBinding(variableName);
					}
				}
			}
		}
		else
		{
			for (int n = 0; n < this.Objectives.Count; n++)
			{
				if ((this.Objectives[n] is ObjectiveFetch || this.Objectives[n] is ObjectiveFetchKeep) && (++num == index || index == -1))
				{
					return this.Objectives[n].ParseBinding(variableName);
				}
			}
		}
		return field;
	}

	// Token: 0x060049CE RID: 18894 RVA: 0x001D107D File Offset: 0x001CF27D
	public string GetPOIName()
	{
		if (this.DataVariables.ContainsKey("POIName"))
		{
			return this.DataVariables["POIName"];
		}
		return "";
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x001D10A7 File Offset: 0x001CF2A7
	public string GetPOINameKey()
	{
		return this.DataVariables.GetValueOrDefault("POINameKey");
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x001D10BC File Offset: 0x001CF2BC
	public bool CanTurnInQuest(List<BaseReward> rewardChoice)
	{
		EntityPlayerLocal ownerPlayer = this.OwnerJournal.OwnerPlayer;
		ItemStack[] array = ownerPlayer.bag.ItemGrid.CloneItems();
		ItemStack[] array2 = ownerPlayer.inventory.ItemGrid.CloneItems();
		ItemStack[] array3 = new ItemStack[array.Length + array2.Length];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			array3[num++] = array[i];
		}
		for (int j = 0; j < array2.Length; j++)
		{
			array3[num++] = array2[j];
		}
		for (int k = 0; k < this.Rewards.Count; k++)
		{
			if (this.Rewards[k].ReceiveStage == BaseReward.ReceiveStages.QuestCompletion && (!this.Rewards[k].Optional || (this.Rewards[k].Optional && this.OptionalComplete)) && (!this.Rewards[k].isChosenReward || (this.Rewards[k].isChosenReward && rewardChoice != null && rewardChoice.Contains(this.Rewards[k]))))
			{
				ItemStack rewardItem = this.Rewards[k].GetRewardItem();
				if (!rewardItem.IsEmpty())
				{
					XUiM_PlayerInventory.TryStackItem(0, rewardItem, array3);
					if (rewardItem.count > 0 && ItemStack.AddToItemStackArray(array3, rewardItem, -1) == -1)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x04003A9B RID: 15003
	public const string POINameKey = "POINameKey";

	// Token: 0x04003A9C RID: 15004
	public QuestJournal OwnerJournal;

	// Token: 0x04003A9D RID: 15005
	public static byte FileVersion = 9;

	// Token: 0x04003A9E RID: 15006
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Quest.QuestState[] _endableStates = new Quest.QuestState[]
	{
		Quest.QuestState.Completed,
		Quest.QuestState.Failed,
		Quest.QuestState.Cancelled
	};

	// Token: 0x04003A9F RID: 15007
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest.QuestState _currentState;

	// Token: 0x04003AA7 RID: 15015
	[PublicizedFrom(EAccessModifier.Private)]
	public int sharedOwnerID = -1;

	// Token: 0x04003AA8 RID: 15016
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> sharedWithList;

	// Token: 0x04003AA9 RID: 15017
	public string QuestUniqueId;

	// Token: 0x04003AAA RID: 15018
	public int QuestCode;

	// Token: 0x04003AAB RID: 15019
	public int QuestGiverID = -1;

	// Token: 0x04003AAC RID: 15020
	public static int MaxQuestTier = 5;

	// Token: 0x04003AAD RID: 15021
	public static int QuestsPerTier = 10;

	// Token: 0x04003AAE RID: 15022
	public static string StarterQuest = "quest_whiteRiverCitizen1";

	// Token: 0x04003AAF RID: 15023
	public byte QuestFaction;

	// Token: 0x04003AB0 RID: 15024
	public bool RallyMarkerActivated;

	// Token: 0x04003AB1 RID: 15025
	public FastTags<TagGroup.Global> QuestTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003AB2 RID: 15026
	public bool NeedsNPCSetPosition;

	// Token: 0x04003AB3 RID: 15027
	public int QuestProgressDay = int.MinValue;

	// Token: 0x04003AB4 RID: 15028
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position = Vector3.zero;

	// Token: 0x04003AB5 RID: 15029
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tracked;

	// Token: 0x04003AB6 RID: 15030
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04003AB7 RID: 15031
	[PublicizedFrom(EAccessModifier.Private)]
	public MapObject mapObject;

	// Token: 0x04003AB8 RID: 15032
	public PrefabInstance QuestPrefab;

	// Token: 0x04003AB9 RID: 15033
	public NavObject NavObject;

	// Token: 0x04003ABA RID: 15034
	public Dictionary<Quest.PositionDataTypes, Vector3> PositionData = new EnumDictionary<Quest.PositionDataTypes, Vector3>();

	// Token: 0x04003ABB RID: 15035
	public Dictionary<string, string> DataVariables = new Dictionary<string, string>();

	// Token: 0x04003ABC RID: 15036
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x04003ABD RID: 15037
	public List<BaseRequirement> Requirements = new List<BaseRequirement>();

	// Token: 0x04003ABE RID: 15038
	public List<BaseObjective> Objectives = new List<BaseObjective>();

	// Token: 0x04003ABF RID: 15039
	public List<BaseReward> Rewards = new List<BaseReward>();

	// Token: 0x04003AC0 RID: 15040
	public TrackingHandler TrackingHelper = new TrackingHandler();

	// Token: 0x020009CC RID: 2508
	public enum QuestSource
	{
		// Token: 0x04003AC2 RID: 15042
		Debug,
		// Token: 0x04003AC3 RID: 15043
		ChallengeSystem,
		// Token: 0x04003AC4 RID: 15044
		QuestSystem,
		// Token: 0x04003AC5 RID: 15045
		TreasureMap,
		// Token: 0x04003AC6 RID: 15046
		Trader,
		// Token: 0x04003AC7 RID: 15047
		PartyShare,
		// Token: 0x04003AC8 RID: 15048
		Load
	}

	// Token: 0x020009CD RID: 2509
	public enum QuestState
	{
		// Token: 0x04003ACA RID: 15050
		NotStarted,
		// Token: 0x04003ACB RID: 15051
		InProgress,
		// Token: 0x04003ACC RID: 15052
		ReadyForTurnIn,
		// Token: 0x04003ACD RID: 15053
		Completed,
		// Token: 0x04003ACE RID: 15054
		Failed,
		// Token: 0x04003ACF RID: 15055
		Cancelled
	}

	// Token: 0x020009CE RID: 2510
	public enum PositionDataTypes
	{
		// Token: 0x04003AD1 RID: 15057
		QuestGiver,
		// Token: 0x04003AD2 RID: 15058
		Location,
		// Token: 0x04003AD3 RID: 15059
		POIPosition,
		// Token: 0x04003AD4 RID: 15060
		POISize,
		// Token: 0x04003AD5 RID: 15061
		TreasurePoint,
		// Token: 0x04003AD6 RID: 15062
		FetchContainer,
		// Token: 0x04003AD7 RID: 15063
		HiddenCache,
		// Token: 0x04003AD8 RID: 15064
		Activate,
		// Token: 0x04003AD9 RID: 15065
		TreasureOffset,
		// Token: 0x04003ADA RID: 15066
		TraderPosition
	}
}
