using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200098D RID: 2445
[Preserve]
public class ObjectivePOIBlockUpgrade : BaseObjective
{
	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x060047F4 RID: 18420 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x060047F5 RID: 18421 RVA: 0x001BE8E8 File Offset: 0x001BCAE8
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x001C23C7 File Offset: 0x001C05C7
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x001C2AC4 File Offset: 0x001C0CC4
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x060047F8 RID: 18424 RVA: 0x001C2B27 File Offset: 0x001C0D27
	public override void SetupDisplay()
	{
		base.Description = "TEST";
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x060047F9 RID: 18425 RVA: 0x001C2B5A File Offset: 0x001C0D5A
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x060047FA RID: 18426 RVA: 0x001C2B93 File Offset: 0x001C0D93
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x001C2BB8 File Offset: 0x001C0DB8
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

	// Token: 0x060047FC RID: 18428 RVA: 0x001C2C54 File Offset: 0x001C0E54
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

	// Token: 0x060047FD RID: 18429 RVA: 0x001C2CE8 File Offset: 0x001C0EE8
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

	// Token: 0x060047FE RID: 18430 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_WaitingForServer()
	{
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Update()
	{
	}

	// Token: 0x06004800 RID: 18432 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Completed()
	{
	}

	// Token: 0x06004801 RID: 18433 RVA: 0x001C2DF8 File Offset: 0x001C0FF8
	public override BaseObjective Clone()
	{
		ObjectivePOIBlockUpgrade objectivePOIBlockUpgrade = new ObjectivePOIBlockUpgrade();
		this.CopyValues(objectivePOIBlockUpgrade);
		return objectivePOIBlockUpgrade;
	}

	// Token: 0x06004802 RID: 18434 RVA: 0x001C2E13 File Offset: 0x001C1013
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIBlockUpgrade.PropBlockName))
		{
			this.ID = properties.Values[ObjectivePOIBlockUpgrade.PropBlockName];
		}
	}

	// Token: 0x04003934 RID: 14644
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x04003935 RID: 14645
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount = -1;

	// Token: 0x04003936 RID: 14646
	public static string PropBlockName = "block_index";

	// Token: 0x04003937 RID: 14647
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;
}
