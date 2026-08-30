using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200099D RID: 2461
[Preserve]
public class ObjectivePOIBlockActivate : BaseObjective
{
	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x06004868 RID: 18536 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06004869 RID: 18537 RVA: 0x001C10DC File Offset: 0x001BF2DC
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x001C4BBB File Offset: 0x001C2DBB
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x001C4BCD File Offset: 0x001C2DCD
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRestorePower_keyword", false, null);
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x0600486C RID: 18540 RVA: 0x001C4BF4 File Offset: 0x001C2DF4
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
		if (this.neededCount == -1)
		{
			this.StatusText = "";
			return;
		}
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x001C4C48 File Offset: 0x001C2E48
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockDestroy -= this.Current_BlockDestroy;
		QuestEventManager.Current.BlockDestroy += this.Current_BlockDestroy;
		QuestEventManager.Current.BlockActivate -= this.Current_BlockActivate;
		QuestEventManager.Current.BlockActivate += this.Current_BlockActivate;
	}

	// Token: 0x0600486E RID: 18542 RVA: 0x001C4CB8 File Offset: 0x001C2EB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockDestroy(Block block, Vector3i blockPos)
	{
		if (this.activateList == null)
		{
			return;
		}
		if (!this.activateList.Contains(blockPos))
		{
			return;
		}
		base.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
	}

	// Token: 0x0600486F RID: 18543 RVA: 0x001C4CE0 File Offset: 0x001C2EE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockActivate(string blockname, Vector3i blockPos)
	{
		if (this.activateList == null)
		{
			return;
		}
		if (base.Complete)
		{
			return;
		}
		NavObjectManager.Instance.UnRegisterNavObjectByPosition(blockPos.ToVector3() + Vector3.one * 0.5f, this.NavObjectName);
		if (!this.activateList.Contains(blockPos))
		{
			return;
		}
		Block blockByName = Block.GetBlockByName(blockname, false);
		if ((this.ID == null || this.ID == "" || this.ID.EqualsCaseInsensitive(blockByName.IndexName)) && base.OwnerQuest.CheckRequirements())
		{
			byte currentValue = base.CurrentValue;
			base.CurrentValue = currentValue + 1;
			this.activateList.Remove(blockPos);
			this.Refresh();
		}
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x001C4DA0 File Offset: 0x001C2FA0
	public void AddActivatedBlock(Vector3i blockPos)
	{
		NavObjectManager.Instance.UnRegisterNavObjectByPosition(blockPos.ToVector3() + Vector3.one * 0.5f, this.NavObjectName);
		byte currentValue = base.CurrentValue;
		base.CurrentValue = currentValue + 1;
		this.activateList.Remove(blockPos);
	}

	// Token: 0x06004871 RID: 18545 RVA: 0x001C4DF8 File Offset: 0x001C2FF8
	public override void RemoveHooks()
	{
		QuestEventManager questEventManager = QuestEventManager.Current;
		questEventManager.RemoveObjectiveToBeUpdated(this);
		questEventManager.BlockActivate -= this.Current_BlockActivate;
		questEventManager.BlockDestroy -= this.Current_BlockDestroy;
		this.ClearNavObjects();
		if (base.OwnerQuest != null && base.OwnerQuest.RallyMarkerActivated)
		{
			questEventManager.ActiveQuestBlocks.Clear();
		}
	}

	// Token: 0x06004872 RID: 18546 RVA: 0x001C4E5C File Offset: 0x001C305C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearNavObjects()
	{
		if (this.activateList == null)
		{
			return;
		}
		for (int i = 0; i < this.activateList.Count; i++)
		{
			NavObjectManager.Instance.UnRegisterNavObjectByPosition(this.activateList[i].ToVector3() + Vector3.one * 0.5f, this.NavObjectName);
		}
		this.activateList.Clear();
	}

	// Token: 0x06004873 RID: 18547 RVA: 0x001C4ECC File Offset: 0x001C30CC
	public override void Refresh()
	{
		if (this.neededCount == -1)
		{
			return;
		}
		if ((int)base.CurrentValue > this.neededCount)
		{
			base.CurrentValue = (byte)this.neededCount;
		}
		if (base.Complete)
		{
			return;
		}
		base.Complete = (int)base.CurrentValue >= this.neededCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
			base.HandleRemoveHooks();
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				QuestEventManager.Current.FinishManagedQuest(base.OwnerQuest.QuestCode, base.OwnerQuest.OwnerJournal.OwnerPlayer);
				return;
			}
			Vector3 zero = Vector3.zero;
			base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.FinishManagedQuest, base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, zero, base.OwnerQuest.QuestCode), false);
		}
	}

	// Token: 0x06004874 RID: 18548 RVA: 0x001C4FC0 File Offset: 0x001C31C0
	public override void Update(float deltaTime)
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			if (this.neededCount == -1)
			{
				Vector3 zero = Vector3.zero;
				base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					if (base.OwnerQuest.SharedOwnerID == -1)
					{
						this.activateList = new List<Vector3i>();
						QuestEventManager.Current.SetupActivateForMP(base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, base.OwnerQuest.QuestCode, this.completeEvent, this.activateList, GameManager.Instance.World, zero, this.ID, base.OwnerQuest.GetSharedWithIDList());
						this.SetupActivationList(zero, this.activateList);
						return;
					}
				}
				else
				{
					if (base.OwnerQuest.SharedOwnerID == -1)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupRestorePower, base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, base.OwnerQuest.QuestCode, this.completeEvent, zero, this.ID, base.OwnerQuest.GetSharedWithIDList()), false);
						return;
					}
					if (QuestEventManager.Current.ActiveQuestBlocks != null && QuestEventManager.Current.ActiveQuestBlocks.Count > 0)
					{
						this.SetupActivationList(zero, QuestEventManager.Current.ActiveQuestBlocks);
					}
				}
			}
		}
	}

	// Token: 0x06004875 RID: 18549 RVA: 0x001C5128 File Offset: 0x001C3328
	public override bool SetupActivationList(Vector3 prefabPos, List<Vector3i> newActivateList)
	{
		Vector3 zero = Vector3.zero;
		base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
		if (zero.x != prefabPos.x && zero.z != prefabPos.z)
		{
			return false;
		}
		base.CurrentValue = 0;
		this.neededCount = newActivateList.Count;
		byte b = 0;
		for (int i = 0; i < newActivateList.Count; i++)
		{
			NavObjectManager.Instance.RegisterNavObject(this.NavObjectName, newActivateList[i].ToVector3() + Vector3.one * 0.5f, "", false, -1, null);
		}
		base.CurrentValue = b;
		QuestEventManager.Current.ActiveQuestBlocks = newActivateList;
		this.activateList = newActivateList;
		this.Refresh();
		this.SetupDisplay();
		return true;
	}

	// Token: 0x06004876 RID: 18550 RVA: 0x001C51F0 File Offset: 0x001C33F0
	public override BaseObjective Clone()
	{
		ObjectivePOIBlockActivate objectivePOIBlockActivate = new ObjectivePOIBlockActivate();
		this.CopyValues(objectivePOIBlockActivate);
		objectivePOIBlockActivate.completeEvent = this.completeEvent;
		objectivePOIBlockActivate.neededCount = this.neededCount;
		objectivePOIBlockActivate.activateList = this.activateList;
		return objectivePOIBlockActivate;
	}

	// Token: 0x06004877 RID: 18551 RVA: 0x001C5230 File Offset: 0x001C3430
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIBlockActivate.PropBlockName))
		{
			this.ID = properties.Values[ObjectivePOIBlockActivate.PropBlockName];
		}
		properties.ParseString(ObjectivePOIBlockActivate.PropEventComplete, ref this.completeEvent);
	}

	// Token: 0x040039BE RID: 14782
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040039BF RID: 14783
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount = -1;

	// Token: 0x040039C0 RID: 14784
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeEvent = "";

	// Token: 0x040039C1 RID: 14785
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> activateList;

	// Token: 0x040039C2 RID: 14786
	public static string PropBlockName = "block_index";

	// Token: 0x040039C3 RID: 14787
	public static string PropEventComplete = "complete_event";

	// Token: 0x040039C4 RID: 14788
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;
}
