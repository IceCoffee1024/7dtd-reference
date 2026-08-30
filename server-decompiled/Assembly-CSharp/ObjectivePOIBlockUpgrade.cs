using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200099E RID: 2462
[Preserve]
public class ObjectivePOIBlockUpgrade : BaseObjective
{
	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x0600487A RID: 18554 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x0600487B RID: 18555 RVA: 0x001C10DC File Offset: 0x001BF2DC
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x0600487C RID: 18556 RVA: 0x001C4BBB File Offset: 0x001C2DBB
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x0600487D RID: 18557 RVA: 0x001C52B8 File Offset: 0x001C34B8
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveBlockUpgrade_keyword", false, null);
		this.localizedName = ((this.ID != "" && this.ID != null) ? Localization.Get(this.ID, false, null) : "Any Block");
		base.OwnerQuest.AddQuestTag(QuestEventManager.restorePowerTag);
	}

	// Token: 0x0600487E RID: 18558 RVA: 0x001C531B File Offset: 0x001C351B
	public override void SetupDisplay()
	{
		base.Description = "TEST";
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.neededCount);
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x001C534E File Offset: 0x001C354E
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
		QuestEventManager.Current.BlockUpgrade += this.Current_BlockUpgrade;
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x001C5387 File Offset: 0x001C3587
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		QuestEventManager.Current.BlockUpgrade -= this.Current_BlockUpgrade;
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x001C53AC File Offset: 0x001C35AC
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

	// Token: 0x06004882 RID: 18562 RVA: 0x001C5448 File Offset: 0x001C3648
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

	// Token: 0x06004883 RID: 18563 RVA: 0x001C54DC File Offset: 0x001C36DC
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

	// Token: 0x06004884 RID: 18564 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_WaitingForServer()
	{
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Update()
	{
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Completed()
	{
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x001C55EC File Offset: 0x001C37EC
	public override BaseObjective Clone()
	{
		ObjectivePOIBlockUpgrade objectivePOIBlockUpgrade = new ObjectivePOIBlockUpgrade();
		this.CopyValues(objectivePOIBlockUpgrade);
		return objectivePOIBlockUpgrade;
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x001C5607 File Offset: 0x001C3807
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIBlockUpgrade.PropBlockName))
		{
			this.ID = properties.Values[ObjectivePOIBlockUpgrade.PropBlockName];
		}
	}

	// Token: 0x040039C5 RID: 14789
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName = "";

	// Token: 0x040039C6 RID: 14790
	[PublicizedFrom(EAccessModifier.Private)]
	public int neededCount = -1;

	// Token: 0x040039C7 RID: 14791
	public static string PropBlockName = "block_index";

	// Token: 0x040039C8 RID: 14792
	[PublicizedFrom(EAccessModifier.Private)]
	public new float updateTime;
}
