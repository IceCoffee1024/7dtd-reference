using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009A4 RID: 2468
[Preserve]
public class ObjectivePOIBlockUpgrade : BaseObjective
{
	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x0600482F RID: 18479 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06004830 RID: 18480 RVA: 0x001C1750 File Offset: 0x001BF950
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x06004831 RID: 18481 RVA: 0x001C544F File Offset: 0x001C364F
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x06004832 RID: 18482 RVA: 0x001C5B78 File Offset: 0x001C3D78
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x001C5BDB File Offset: 0x001C3DDB
	public override void SetupDisplay()
	{
		base.Description = "TEST";
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x001C5C0E File Offset: 0x001C3E0E
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x06004835 RID: 18485 RVA: 0x001C5C47 File Offset: 0x001C3E47
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x001C5C6C File Offset: 0x001C3E6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockUpgrade(string blockname, Vector3i blockPos)
	{
		if (base.Complete)
		{
			return;
		}
		NavObjectManager.Instance.UnRegisterNavObjectByPosition(blockPos.ToVector3() + Vector3.one * 0.5f, this.NavObjectName);
		Block blockByName = Block.GetBlockByName(blockname, false);
		if ((this.ID == null || this.ID == "" || this.ID.EqualsCaseInsensitive(blockByName.IndexName)) && base.OwnerQuest.CheckRequirements())
		{
			byte currentValue = base.CurrentValue;
			base.CurrentValue = currentValue + 1;
			this.Refresh();
		}
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x001C5D08 File Offset: 0x001C3F08
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
			QuestEventManager.Current.FinishManagedQuest(base.OwnerQuest.QuestCode, base.OwnerQuest.OwnerJournal.OwnerPlayer);
		}
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x001C5D9C File Offset: 0x001C3F9C
	public override void Update(float deltaTime)
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			if (this.neededCount == -1)
			{
				Vector3 zero = Vector3.zero;
				base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
				if (base.OwnerQuest.SharedOwnerID == -1)
				{
					base.CurrentValue = 0;
					List<Vector3i> list = new List<Vector3i>();
					List<bool> list2 = new List<bool>();
					QuestEventManager.Current.SetupRepairForMP(list, list2, GameManager.Instance.World, zero);
					this.neededCount = list.Count;
					byte b = 0;
					for (int i = 0; i < list.Count; i++)
					{
						if (list2[i])
						{
							NavObjectManager.Instance.RegisterNavObject(this.NavObjectName, list[i].ToVector3() + Vector3.one * 0.5f, "", false, -1, null);
						}
						else
						{
							b += 1;
						}
					}
					base.CurrentValue = b;
					this.Refresh();
					this.SetupDisplay();
				}
			}
		}
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_WaitingForServer()
	{
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Update()
	{
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Completed()
	{
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x001C5EAC File Offset: 0x001C40AC
	public override BaseObjective Clone()
	{
		ObjectivePOIBlockUpgrade objectivePOIBlockUpgrade = new ObjectivePOIBlockUpgrade();
		this.CopyValues(objectivePOIBlockUpgrade);
		return objectivePOIBlockUpgrade;
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x001C5EC7 File Offset: 0x001C40C7
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIBlockUpgrade.PropBlockName))
		{
			this.ID = properties.Values[ObjectivePOIBlockUpgrade.PropBlockName];
		}
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x001C5EF8 File Offset: 0x001C40F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x040039C2 RID: 14786
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040039C3 RID: 14787
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount = -1;

	// Token: 0x040039C4 RID: 14788
	public static string PropBlockName = "block_index";

	// Token: 0x040039C5 RID: 14789
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;
}
