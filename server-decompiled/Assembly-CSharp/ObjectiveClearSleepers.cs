using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200099C RID: 2460
[Preserve]
public class ObjectiveClearSleepers : BaseObjective
{
	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x0600476C RID: 18284 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x0600476D RID: 18285 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool RequiresZombies
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x001C27A8 File Offset: 0x001C09A8
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.clearTag);
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x001C27BA File Offset: 0x001C09BA
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveClearAreas_keyword", false, null);
		this.SetupIcon();
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06004770 RID: 18288 RVA: 0x001C27D4 File Offset: 0x001C09D4
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x001C27E2 File Offset: 0x001C09E2
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
		this.StatusText = "";
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x001C27FC File Offset: 0x001C09FC
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

	// Token: 0x06004773 RID: 18291 RVA: 0x001C28A4 File Offset: 0x001C0AA4
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

	// Token: 0x06004774 RID: 18292 RVA: 0x001C292C File Offset: 0x001C0B2C
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

	// Token: 0x06004775 RID: 18293 RVA: 0x001C29C4 File Offset: 0x001C0BC4
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

	// Token: 0x06004776 RID: 18294 RVA: 0x001C2A1B File Offset: 0x001C0C1B
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupZombieCompassBounds(Vector3 poiPos, Vector3 poiSize)
	{
		base.OwnerQuest.OwnerJournal.OwnerPlayer.ZombieCompassBounds = new Rect(poiPos.x, poiPos.z, poiSize.x, poiSize.z);
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x001C2A50 File Offset: 0x001C0C50
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

	// Token: 0x06004778 RID: 18296 RVA: 0x001C2AB4 File Offset: 0x001C0CB4
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

	// Token: 0x06004779 RID: 18297 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupIcon()
	{
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x001C2B45 File Offset: 0x001C0D45
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 GetPosition()
	{
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition))
		{
			base.OwnerQuest.Position = this.position;
		}
		return Vector3.zero;
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x001C2B74 File Offset: 0x001C0D74
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

	// Token: 0x0600477C RID: 18300 RVA: 0x001C2BFA File Offset: 0x001C0DFA
	public override void Refresh()
	{
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x001C2C14 File Offset: 0x001C0E14
	public override BaseObjective Clone()
	{
		ObjectiveClearSleepers objectiveClearSleepers = new ObjectiveClearSleepers();
		this.CopyValues(objectiveClearSleepers);
		return objectiveClearSleepers;
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x001C2C2F File Offset: 0x001C0E2F
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		this.FinalizePoint(this.distanceOffset, pos.x, pos.y, pos.z);
		return true;
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x001C2C50 File Offset: 0x001C0E50
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("position", this.position.ToString());
	}

	// Token: 0x04003987 RID: 14727
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04003988 RID: 14728
	[PublicizedFrom(EAccessModifier.Private)]
	public float distanceOffset;

	// Token: 0x04003989 RID: 14729
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon = "ui_game_symbol_quest";

	// Token: 0x0400398A RID: 14730
	[PublicizedFrom(EAccessModifier.Private)]
	public string locationVariable = "gotolocation";

	// Token: 0x0400398B RID: 14731
	public Dictionary<Vector3, MapObjectSleeperVolume> SleeperMapObjectList = new Dictionary<Vector3, MapObjectSleeperVolume>();

	// Token: 0x0400398C RID: 14732
	public Dictionary<Vector3, NavObject> SleeperNavObjectList = new Dictionary<Vector3, NavObject>();

	// Token: 0x0200099D RID: 2461
	[PublicizedFrom(EAccessModifier.Private)]
	public enum GotoStates
	{
		// Token: 0x0400398E RID: 14734
		NoPosition,
		// Token: 0x0400398F RID: 14735
		TryRefresh,
		// Token: 0x04003990 RID: 14736
		TryComplete,
		// Token: 0x04003991 RID: 14737
		Completed
	}
}
