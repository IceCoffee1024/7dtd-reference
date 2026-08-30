using System;
using UnityEngine.Scripting;

// Token: 0x02000987 RID: 2439
[Preserve]
public class ObjectiveGameEvent : BaseObjective
{
	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x060047A9 RID: 18345 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x001C10A4 File Offset: 0x001BF2A4
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x001C10B8 File Offset: 0x001BF2B8
	public override void SetupDisplay()
	{
		base.Description = "Test Game Event";
		this.StatusText = "";
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x001C10D0 File Offset: 0x001BF2D0
	public override void AddHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted += this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied += this.Current_GameEventDenied;
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x001C10FA File Offset: 0x001BF2FA
	public override void RemoveHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted -= this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied -= this.Current_GameEventDenied;
	}

	// Token: 0x060047AE RID: 18350 RVA: 0x001C1124 File Offset: 0x001BF324
	public override void Update(float updateTime)
	{
		switch (this.GameEventState)
		{
		case ObjectiveGameEvent.GameEventStates.Start:
		{
			EntityPlayer ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
			GameEventManager.Current.HandleAction(this.gameEventID, ownerPlayer, ownerPlayer, false, "", this.gameEventTag, false, true, "", null);
			this.GameEventState = ObjectiveGameEvent.GameEventStates.Waiting;
			break;
		}
		case ObjectiveGameEvent.GameEventStates.Waiting:
		case ObjectiveGameEvent.GameEventStates.Complete:
			break;
		default:
			return;
		}
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x001C118C File Offset: 0x001BF38C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventCompleted(string _gameEventID, int _targetEntityID, string _extraData, string _tag)
	{
		if (this.gameEventID == _gameEventID && _tag == this.gameEventTag && _targetEntityID == base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventDenied(string gameEventID, int targetEntityID, string extraData, string tag)
	{
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x001C11E8 File Offset: 0x001BF3E8
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		base.Complete = base.CurrentValue == 1;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060047B2 RID: 18354 RVA: 0x001C121C File Offset: 0x001BF41C
	public override BaseObjective Clone()
	{
		ObjectiveGameEvent objectiveGameEvent = new ObjectiveGameEvent();
		this.CopyValues(objectiveGameEvent);
		objectiveGameEvent.gameEventID = this.gameEventID;
		objectiveGameEvent.gameEventTag = this.gameEventTag;
		return objectiveGameEvent;
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x001C124F File Offset: 0x001BF44F
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveGameEvent.PropGameEventID, ref this.gameEventID);
		properties.ParseString(ObjectiveGameEvent.PropGameEventTag, ref this.gameEventTag);
	}

	// Token: 0x04003910 RID: 14608
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventID = "";

	// Token: 0x04003911 RID: 14609
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventTag = "quest";

	// Token: 0x04003912 RID: 14610
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventID = "event";

	// Token: 0x04003913 RID: 14611
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventTag = "event_tag";

	// Token: 0x04003914 RID: 14612
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveGameEvent.GameEventStates GameEventState;

	// Token: 0x02000988 RID: 2440
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum GameEventStates
	{
		// Token: 0x04003916 RID: 14614
		Start,
		// Token: 0x04003917 RID: 14615
		Waiting,
		// Token: 0x04003918 RID: 14616
		Complete
	}
}
