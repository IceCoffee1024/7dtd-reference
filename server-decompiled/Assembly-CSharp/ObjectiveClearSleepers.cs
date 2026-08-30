using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200098C RID: 2444
[Preserve]
public class ObjectiveClearSleepers : BaseObjective
{
	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x0600479F RID: 18335 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x060047A0 RID: 18336 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool RequiresZombies
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x001C10B0 File Offset: 0x001BF2B0
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.clearTag);
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x001C10C2 File Offset: 0x001BF2C2
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveClearAreas_keyword", false, null);
		this.SetupIcon();
	}

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x060047A3 RID: 18339 RVA: 0x001C10DC File Offset: 0x001BF2DC
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x060047A4 RID: 18340 RVA: 0x001C10EA File Offset: 0x001BF2EA
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
		this.StatusText = "";
	}

	// Token: 0x060047A5 RID: 18341 RVA: 0x001C1104 File Offset: 0x001BF304
	public override void AddHooks()
	{
		this.GetPosition();
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
		base.OwnerQuest.GetPositionData(out zero2, Quest.PositionDataTypes.POISize);
		QuestEventManager.Current.SleepersCleared += this.Current_SleepersCleared;
		QuestEventManager.Current.SleeperVolumePositionAdd += this.Current_SleeperVolumePositionAdd;
		QuestEventManager.Current.SleeperVolumePositionRemove += this.Current_SleeperVolumePositionRemove;
		QuestEventManager.Current.SubscribeToUpdateEvent(base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, zero);
		this.SetupZombieCompassBounds(zero, zero2);
	}

	// Token: 0x060047A6 RID: 18342 RVA: 0x001C11AC File Offset: 0x001BF3AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_SleeperVolumePositionAdd(Vector3 position)
	{
		if (this.NavObjectName == "")
		{
			if (!this.SleeperMapObjectList.ContainsKey(position))
			{
				MapObjectSleeperVolume mapObjectSleeperVolume = new MapObjectSleeperVolume(position);
				GameManager.Instance.World.ObjectOnMapAdd(mapObjectSleeperVolume);
				this.SleeperMapObjectList.Add(position, mapObjectSleeperVolume);
				return;
			}
		}
		else if (!this.SleeperNavObjectList.ContainsKey(position))
		{
			NavObject navObject = NavObjectManager.Instance.RegisterNavObject(this.NavObjectName, position, "", false, -1, null);
			this.SleeperNavObjectList.Add(position, navObject);
		}
	}

	// Token: 0x060047A7 RID: 18343 RVA: 0x001C1234 File Offset: 0x001BF434
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_SleeperVolumePositionRemove(Vector3 position)
	{
		if (this.NavObjectName == "")
		{
			if (this.SleeperMapObjectList.ContainsKey(position))
			{
				MapObject mapObject = this.SleeperMapObjectList[position];
				GameManager.Instance.World.ObjectOnMapRemove(mapObject.type, (int)mapObject.key);
				this.SleeperMapObjectList.Remove(position);
				return;
			}
		}
		else if (this.SleeperNavObjectList.ContainsKey(position))
		{
			NavObject navObject = this.SleeperNavObjectList[position];
			NavObjectManager.Instance.UnRegisterNavObject(navObject);
			this.SleeperNavObjectList.Remove(position);
		}
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x001C12CC File Offset: 0x001BF4CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveSleeperVolumeMapObjects()
	{
		if (this.NavObjectName == "")
		{
			GameManager.Instance.World.ObjectOnMapRemove(EnumMapObjectType.SleeperVolume);
			this.SleeperMapObjectList.Clear();
			return;
		}
		NavObjectManager.Instance.UnRegisterNavObjectByClass(this.NavObjectName);
		this.SleeperNavObjectList.Clear();
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x001C1323 File Offset: 0x001BF523
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupZombieCompassBounds(Vector3 poiPos, Vector3 poiSize)
	{
		base.OwnerQuest.OwnerJournal.OwnerPlayer.ZombieCompassBounds = new Rect(poiPos.x, poiPos.z, poiSize.x, poiSize.z);
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x001C1358 File Offset: 0x001BF558
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_SleepersCleared(Vector3 prefabPos)
	{
		Vector3 zero = Vector3.zero;
		base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
		if (zero.x != prefabPos.x || zero.z != prefabPos.z)
		{
			return;
		}
		if (base.OwnerQuest.CheckRequirements())
		{
			base.Complete = true;
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x001C13BC File Offset: 0x001BF5BC
	public override void RemoveHooks()
	{
		QuestEventManager.Current.SleepersCleared -= this.Current_SleepersCleared;
		Vector3 zero = Vector3.zero;
		base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
		QuestEventManager.Current.UnSubscribeToUpdateEvent(base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, zero);
		if (base.OwnerQuest.OwnerJournal.ActiveQuest == base.OwnerQuest)
		{
			base.OwnerQuest.OwnerJournal.OwnerPlayer.ZombieCompassBounds = default(Rect);
		}
		this.RemoveSleeperVolumeMapObjects();
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupIcon()
	{
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x001C144D File Offset: 0x001BF64D
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 GetPosition()
	{
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition))
		{
			base.OwnerQuest.Position = this.position;
		}
		return Vector3.zero;
	}

	// Token: 0x060047AE RID: 18350 RVA: 0x001C147C File Offset: 0x001BF67C
	public void FinalizePoint(float offset, float x, float y, float z)
	{
		this.distanceOffset = offset;
		this.position = new Vector3(x, y, z);
		base.OwnerQuest.DataVariables.Add(this.locationVariable, string.Format("{0},{1},{2},{3}", new object[]
		{
			offset.ToCultureInvariantString(),
			x.ToCultureInvariantString(),
			y.ToCultureInvariantString(),
			z.ToCultureInvariantString()
		}));
		base.OwnerQuest.Position = this.position;
		base.CurrentValue = 1;
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x001C1502 File Offset: 0x001BF702
	public override void Refresh()
	{
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x001C151C File Offset: 0x001BF71C
	public override BaseObjective Clone()
	{
		ObjectiveClearSleepers objectiveClearSleepers = new ObjectiveClearSleepers();
		this.CopyValues(objectiveClearSleepers);
		return objectiveClearSleepers;
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x001C1537 File Offset: 0x001BF737
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		this.FinalizePoint(this.distanceOffset, pos.x, pos.y, pos.z);
		return true;
	}

	// Token: 0x0400396B RID: 14699
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x0400396C RID: 14700
	[PublicizedFrom(EAccessModifier.Private)]
	public float distanceOffset;

	// Token: 0x0400396D RID: 14701
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon = "ui_game_symbol_quest";

	// Token: 0x0400396E RID: 14702
	[PublicizedFrom(EAccessModifier.Private)]
	public string locationVariable = "gotolocation";

	// Token: 0x0400396F RID: 14703
	public Dictionary<Vector3, MapObjectSleeperVolume> SleeperMapObjectList = new Dictionary<Vector3, MapObjectSleeperVolume>();

	// Token: 0x04003970 RID: 14704
	public Dictionary<Vector3, NavObject> SleeperNavObjectList = new Dictionary<Vector3, NavObject>();

	// Token: 0x0200098D RID: 2445
	[PublicizedFrom(EAccessModifier.Private)]
	public enum GotoStates
	{
		// Token: 0x04003972 RID: 14706
		NoPosition,
		// Token: 0x04003973 RID: 14707
		TryRefresh,
		// Token: 0x04003974 RID: 14708
		TryComplete,
		// Token: 0x04003975 RID: 14709
		Completed
	}
}
