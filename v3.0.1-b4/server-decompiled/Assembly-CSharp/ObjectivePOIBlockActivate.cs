using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200098C RID: 2444
[Preserve]
public class ObjectivePOIBlockActivate : BaseObjective
{
	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x060047E2 RID: 18402 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x060047E3 RID: 18403 RVA: 0x001BE8E8 File Offset: 0x001BCAE8
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x001C23C7 File Offset: 0x001C05C7
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x001C23D9 File Offset: 0x001C05D9
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRestorePower_keyword", false, null);
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x001C2400 File Offset: 0x001C0600
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

	// Token: 0x060047E7 RID: 18407 RVA: 0x001C2454 File Offset: 0x001C0654
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockDestroy -= this.Current_BlockDestroy;
		QuestEventManager.Current.BlockDestroy += this.Current_BlockDestroy;
		QuestEventManager.Current.BlockActivate -= this.Current_BlockActivate;
		QuestEventManager.Current.BlockActivate += this.Current_BlockActivate;
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x001C24C4 File Offset: 0x001C06C4
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

	// Token: 0x060047E9 RID: 18409 RVA: 0x001C24EC File Offset: 0x001C06EC
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

	// Token: 0x060047EA RID: 18410 RVA: 0x001C25AC File Offset: 0x001C07AC
	public void AddActivatedBlock(Vector3i blockPos)
	{
		NavObjectManager.Instance.UnRegisterNavObjectByPosition(blockPos.ToVector3() + Vector3.one * 0.5f, this.NavObjectName);
		byte currentValue = base.CurrentValue;
		base.CurrentValue = currentValue + 1;
		this.activateList.Remove(blockPos);
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x001C2604 File Offset: 0x001C0804
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

	// Token: 0x060047EC RID: 18412 RVA: 0x001C2668 File Offset: 0x001C0868
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

	// Token: 0x060047ED RID: 18413 RVA: 0x001C26D8 File Offset: 0x001C08D8
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

	// Token: 0x060047EE RID: 18414 RVA: 0x001C27CC File Offset: 0x001C09CC
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

	// Token: 0x060047EF RID: 18415 RVA: 0x001C2934 File Offset: 0x001C0B34
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

	// Token: 0x060047F0 RID: 18416 RVA: 0x001C29FC File Offset: 0x001C0BFC
	public override BaseObjective Clone()
	{
		ObjectivePOIBlockActivate objectivePOIBlockActivate = new ObjectivePOIBlockActivate();
		this.CopyValues(objectivePOIBlockActivate);
		objectivePOIBlockActivate.completeEvent = this.completeEvent;
		objectivePOIBlockActivate.neededCount = this.neededCount;
		objectivePOIBlockActivate.activateList = this.activateList;
		return objectivePOIBlockActivate;
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x001C2A3C File Offset: 0x001C0C3C
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIBlockActivate.PropBlockName))
		{
			this.ID = properties.Values[ObjectivePOIBlockActivate.PropBlockName];
		}
		properties.ParseString(ObjectivePOIBlockActivate.PropEventComplete, ref this.completeEvent);
	}

	// Token: 0x0400392D RID: 14637
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x0400392E RID: 14638
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount = -1;

	// Token: 0x0400392F RID: 14639
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeEvent = "";

	// Token: 0x04003930 RID: 14640
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> activateList;

	// Token: 0x04003931 RID: 14641
	public static string PropBlockName = "block_index";

	// Token: 0x04003932 RID: 14642
	public static string PropEventComplete = "complete_event";

	// Token: 0x04003933 RID: 14643
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;
}
