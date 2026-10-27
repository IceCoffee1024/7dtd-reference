using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009AD RID: 2477
[Preserve]
public class ObjectivePOIBlockActivate : BaseObjective
{
	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x0600483F RID: 18495 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06004840 RID: 18496 RVA: 0x001C27D4 File Offset: 0x001C09D4
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x001C64FB File Offset: 0x001C46FB
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x001C650D File Offset: 0x001C470D
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRestorePower_keyword", false, null);
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x06004843 RID: 18499 RVA: 0x001C6534 File Offset: 0x001C4734
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

	// Token: 0x06004844 RID: 18500 RVA: 0x001C6588 File Offset: 0x001C4788
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockDestroy -= this.Current_BlockDestroy;
		QuestEventManager.Current.BlockDestroy += this.Current_BlockDestroy;
		QuestEventManager.Current.BlockActivate -= this.Current_BlockActivate;
		QuestEventManager.Current.BlockActivate += this.Current_BlockActivate;
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x001C65F8 File Offset: 0x001C47F8
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

	// Token: 0x06004846 RID: 18502 RVA: 0x001C6620 File Offset: 0x001C4820
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

	// Token: 0x06004847 RID: 18503 RVA: 0x001C66E0 File Offset: 0x001C48E0
	public void AddActivatedBlock(Vector3i blockPos)
	{
		NavObjectManager.Instance.UnRegisterNavObjectByPosition(blockPos.ToVector3() + Vector3.one * 0.5f, this.NavObjectName);
		byte currentValue = base.CurrentValue;
		base.CurrentValue = currentValue + 1;
		this.activateList.Remove(blockPos);
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x001C6738 File Offset: 0x001C4938
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

	// Token: 0x06004849 RID: 18505 RVA: 0x001C679C File Offset: 0x001C499C
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

	// Token: 0x0600484A RID: 18506 RVA: 0x001C680C File Offset: 0x001C4A0C
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

	// Token: 0x0600484B RID: 18507 RVA: 0x001C6900 File Offset: 0x001C4B00
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

	// Token: 0x0600484C RID: 18508 RVA: 0x001C6A68 File Offset: 0x001C4C68
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

	// Token: 0x0600484D RID: 18509 RVA: 0x001C6B30 File Offset: 0x001C4D30
	public override BaseObjective Clone()
	{
		ObjectivePOIBlockActivate objectivePOIBlockActivate = new ObjectivePOIBlockActivate();
		this.CopyValues(objectivePOIBlockActivate);
		objectivePOIBlockActivate.completeEvent = this.completeEvent;
		objectivePOIBlockActivate.neededCount = this.neededCount;
		objectivePOIBlockActivate.activateList = this.activateList;
		return objectivePOIBlockActivate;
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x001C6B70 File Offset: 0x001C4D70
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIBlockActivate.PropBlockName))
		{
			this.ID = properties.Values[ObjectivePOIBlockActivate.PropBlockName];
		}
		properties.ParseString(ObjectivePOIBlockActivate.PropEventComplete, ref this.completeEvent);
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x001C6BBD File Offset: 0x001C4DBD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x040039DB RID: 14811
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040039DC RID: 14812
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount = -1;

	// Token: 0x040039DD RID: 14813
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeEvent = "";

	// Token: 0x040039DE RID: 14814
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> activateList;

	// Token: 0x040039DF RID: 14815
	public static string PropBlockName = "block_index";

	// Token: 0x040039E0 RID: 14816
	public static string PropEventComplete = "complete_event";

	// Token: 0x040039E1 RID: 14817
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;
}
