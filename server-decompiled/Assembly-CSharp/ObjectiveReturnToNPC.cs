using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009B3 RID: 2483
[Preserve]
public class ObjectiveReturnToNPC : ObjectiveRandomGoto
{
	// Token: 0x060048C4 RID: 18628 RVA: 0x001C94CB File Offset: 0x001C76CB
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveReturnToTrader_keyword", false, null) + ((base.OwnerQuest.CurrentState == Quest.QuestState.NotStarted) ? "" : ":");
		this.completeWithinRange = false;
	}

	// Token: 0x060048C5 RID: 18629 RVA: 0x001C9504 File Offset: 0x001C7704
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupIcon()
	{
		this.icon = "ui_game_symbol_quest";
	}

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x060048C6 RID: 18630 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060048C7 RID: 18631 RVA: 0x001C9514 File Offset: 0x001C7714
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetPosition()
	{
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.QuestGiver))
		{
			base.OwnerQuest.Position = this.position;
			this.positionSet = true;
			base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.QuestGiver, this.NavObjectName, -1);
			base.CurrentValue = 2;
			return this.position;
		}
		base.CurrentValue = 3;
		base.Complete = true;
		base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		this.positionSet = true;
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		this.HiddenObjective = true;
		base.OwnerQuest.RemoveMapObject();
		return Vector3.zero;
	}

	// Token: 0x060048C8 RID: 18632 RVA: 0x001C95B4 File Offset: 0x001C77B4
	public override void OnStart()
	{
		base.OnStart();
		if (base.OwnerQuest.QuestClass.AddsToTierComplete)
		{
			base.OwnerQuest.OwnerJournal.HandleQuestCompleteToday(base.OwnerQuest);
		}
	}

	// Token: 0x060048C9 RID: 18633 RVA: 0x001C95E4 File Offset: 0x001C77E4
	public override BaseObjective Clone()
	{
		ObjectiveReturnToNPC objectiveReturnToNPC = new ObjectiveReturnToNPC();
		this.CopyValues(objectiveReturnToNPC);
		return objectiveReturnToNPC;
	}
}
