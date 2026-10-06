using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000992 RID: 2450
[Preserve]
public class ObjectiveClearSleepers : BaseObjective
{
	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x06004749 RID: 18249 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x0600474A RID: 18250 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool RequiresZombies
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600474B RID: 18251 RVA: 0x001C1724 File Offset: 0x001BF924
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.clearTag);
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x001C1736 File Offset: 0x001BF936
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveClearAreas_keyword", false, null);
		this.SetupIcon();
	}

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x0600474D RID: 18253 RVA: 0x001C1750 File Offset: 0x001BF950
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x0600474E RID: 18254 RVA: 0x001C175E File Offset: 0x001BF95E
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
		this.StatusText = "";
	}

	// Token: 0x0600474F RID: 18255 RVA: 0x001C1778 File Offset: 0x001BF978
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

	// Token: 0x06004750 RID: 18256 RVA: 0x001C1820 File Offset: 0x001BFA20
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

	// Token: 0x06004751 RID: 18257 RVA: 0x001C18A8 File Offset: 0x001BFAA8
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

	// Token: 0x06004752 RID: 18258 RVA: 0x001C1940 File Offset: 0x001BFB40
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

	// Token: 0x06004753 RID: 18259 RVA: 0x001C1997 File Offset: 0x001BFB97
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupZombieCompassBounds(Vector3 poiPos, Vector3 poiSize)
	{
		base.OwnerQuest.OwnerJournal.OwnerPlayer.ZombieCompassBounds = new Rect(poiPos.x, poiPos.z, poiSize.x, poiSize.z);
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x001C19CC File Offset: 0x001BFBCC
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

	// Token: 0x06004755 RID: 18261 RVA: 0x001C1A30 File Offset: 0x001BFC30
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

	// Token: 0x06004756 RID: 18262 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupIcon()
	{
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x001C1AC1 File Offset: 0x001BFCC1
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 GetPosition()
	{
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition))
		{
			base.OwnerQuest.Position = this.position;
		}
		return Vector3.zero;
	}

	// Token: 0x06004758 RID: 18264 RVA: 0x001C1AF0 File Offset: 0x001BFCF0
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

	// Token: 0x06004759 RID: 18265 RVA: 0x001C1B76 File Offset: 0x001BFD76
	public override void Refresh()
	{
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x001C1B90 File Offset: 0x001BFD90
	public override BaseObjective Clone()
	{
		ObjectiveClearSleepers objectiveClearSleepers = new ObjectiveClearSleepers();
		this.CopyValues(objectiveClearSleepers);
		return objectiveClearSleepers;
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x001C1BAB File Offset: 0x001BFDAB
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		this.FinalizePoint(this.distanceOffset, pos.x, pos.y, pos.z);
		return true;
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x001C1BCC File Offset: 0x001BFDCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("position", this.position.ToString());
	}

	// Token: 0x04003967 RID: 14695
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04003968 RID: 14696
	[PublicizedFrom(EAccessModifier.Private)]
	public float distanceOffset;

	// Token: 0x04003969 RID: 14697
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon = "ui_game_symbol_quest";

	// Token: 0x0400396A RID: 14698
	[PublicizedFrom(EAccessModifier.Private)]
	public string locationVariable = "gotolocation";

	// Token: 0x0400396B RID: 14699
	public Dictionary<Vector3, MapObjectSleeperVolume> SleeperMapObjectList = new Dictionary<Vector3, MapObjectSleeperVolume>();

	// Token: 0x0400396C RID: 14700
	public Dictionary<Vector3, NavObject> SleeperNavObjectList = new Dictionary<Vector3, NavObject>();

	// Token: 0x02000993 RID: 2451
	[PublicizedFrom(EAccessModifier.Private)]
	public enum GotoStates
	{
		// Token: 0x0400396E RID: 14702
		NoPosition,
		// Token: 0x0400396F RID: 14703
		TryRefresh,
		// Token: 0x04003970 RID: 14704
		TryComplete,
		// Token: 0x04003971 RID: 14705
		Completed
	}
}
