using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009AE RID: 2478
[Preserve]
public class ObjectivePOIBlockUpgrade : BaseObjective
{
	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06004852 RID: 18514 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x06004853 RID: 18515 RVA: 0x001C27D4 File Offset: 0x001C09D4
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x06004854 RID: 18516 RVA: 0x001C64FB File Offset: 0x001C46FB
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x06004855 RID: 18517 RVA: 0x001C6C24 File Offset: 0x001C4E24
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x06004856 RID: 18518 RVA: 0x001C6C87 File Offset: 0x001C4E87
	public override void SetupDisplay()
	{
		base.Description = "TEST";
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x06004857 RID: 18519 RVA: 0x001C6CBA File Offset: 0x001C4EBA
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x06004858 RID: 18520 RVA: 0x001C6CF3 File Offset: 0x001C4EF3
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x06004859 RID: 18521 RVA: 0x001C6D18 File Offset: 0x001C4F18
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

	// Token: 0x0600485A RID: 18522 RVA: 0x001C6DB4 File Offset: 0x001C4FB4
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

	// Token: 0x0600485B RID: 18523 RVA: 0x001C6E48 File Offset: 0x001C5048
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

	// Token: 0x0600485C RID: 18524 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_WaitingForServer()
	{
	}

	// Token: 0x0600485D RID: 18525 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Update()
	{
	}

	// Token: 0x0600485E RID: 18526 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Completed()
	{
	}

	// Token: 0x0600485F RID: 18527 RVA: 0x001C6F58 File Offset: 0x001C5158
	public override BaseObjective Clone()
	{
		ObjectivePOIBlockUpgrade objectivePOIBlockUpgrade = new ObjectivePOIBlockUpgrade();
		this.CopyValues(objectivePOIBlockUpgrade);
		return objectivePOIBlockUpgrade;
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x001C6F73 File Offset: 0x001C5173
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIBlockUpgrade.PropBlockName))
		{
			this.ID = properties.Values[ObjectivePOIBlockUpgrade.PropBlockName];
		}
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x001C6FA4 File Offset: 0x001C51A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("block_id", this.ID);
		parameters.Add("count", this.neededCount);
	}

	// Token: 0x040039E2 RID: 14818
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040039E3 RID: 14819
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount = -1;

	// Token: 0x040039E4 RID: 14820
	public static string PropBlockName = "block_index";

	// Token: 0x040039E5 RID: 14821
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;
}
