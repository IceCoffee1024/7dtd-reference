using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009BD RID: 2493
[Preserve]
public class ObjectiveReturnToNPC : ObjectiveRandomGoto
{
	// Token: 0x060048E7 RID: 18663 RVA: 0x001CA59F File Offset: 0x001C879F
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveReturnToTrader_keyword", false, null) + ((base.OwnerQuest.CurrentState == Quest.QuestState.NotStarted) ? "" : ":");
		this.completeWithinRange = false;
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x001CA5D8 File Offset: 0x001C87D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupIcon()
	{
		this.icon = "ui_game_symbol_quest";
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x060048E9 RID: 18665 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x001CA5E8 File Offset: 0x001C87E8
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

	// Token: 0x060048EB RID: 18667 RVA: 0x001CA688 File Offset: 0x001C8888
	public override void OnStart()
	{
		base.OnStart();
		if (base.OwnerQuest.QuestClass.AddsToTierComplete)
		{
			base.OwnerQuest.OwnerJournal.HandleQuestCompleteToday(base.OwnerQuest);
		}
	}

	// Token: 0x060048EC RID: 18668 RVA: 0x001CA6B8 File Offset: 0x001C88B8
	public override BaseObjective Clone()
	{
		ObjectiveReturnToNPC objectiveReturnToNPC = new ObjectiveReturnToNPC();
		this.CopyValues(objectiveReturnToNPC);
		return objectiveReturnToNPC;
	}
}
