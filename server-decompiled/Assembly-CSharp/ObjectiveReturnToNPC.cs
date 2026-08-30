using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009AD RID: 2477
[Preserve]
public class ObjectiveReturnToNPC : ObjectiveRandomGoto
{
	// Token: 0x0600490A RID: 18698 RVA: 0x001C8B06 File Offset: 0x001C6D06
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveReturnToTrader_keyword", false, null) + ((base.OwnerQuest.CurrentState == Quest.QuestState.NotStarted) ? "" : ":");
		this.completeWithinRange = false;
	}

	// Token: 0x0600490B RID: 18699 RVA: 0x001C8B3F File Offset: 0x001C6D3F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetupIcon()
	{
		this.icon = "ui_game_symbol_quest";
	}

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x0600490C RID: 18700 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600490D RID: 18701 RVA: 0x001C8B4C File Offset: 0x001C6D4C
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

	// Token: 0x0600490E RID: 18702 RVA: 0x001C8BEC File Offset: 0x001C6DEC
	public override void OnStart()
	{
		base.OnStart();
		if (base.OwnerQuest.QuestClass.AddsToTierComplete)
		{
			base.OwnerQuest.OwnerJournal.HandleQuestCompleteToday(base.OwnerQuest);
		}
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x001C8C1C File Offset: 0x001C6E1C
	public override BaseObjective Clone()
	{
		ObjectiveReturnToNPC objectiveReturnToNPC = new ObjectiveReturnToNPC();
		this.CopyValues(objectiveReturnToNPC);
		return objectiveReturnToNPC;
	}
}
