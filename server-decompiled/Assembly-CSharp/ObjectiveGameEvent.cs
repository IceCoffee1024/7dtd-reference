using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009A8 RID: 2472
[Preserve]
public class ObjectiveGameEvent : BaseObjective
{
	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06004802 RID: 18434 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06004803 RID: 18435 RVA: 0x001C5137 File Offset: 0x001C3337
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x06004804 RID: 18436 RVA: 0x001C514B File Offset: 0x001C334B
	public override void SetupDisplay()
	{
		base.Description = "Test Game Event";
		this.StatusText = "";
	}

	// Token: 0x06004805 RID: 18437 RVA: 0x001C5163 File Offset: 0x001C3363
	public override void AddHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted += this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied += this.Current_GameEventDenied;
	}

	// Token: 0x06004806 RID: 18438 RVA: 0x001C518D File Offset: 0x001C338D
	public override void RemoveHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted -= this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied -= this.Current_GameEventDenied;
	}

	// Token: 0x06004807 RID: 18439 RVA: 0x001C51B8 File Offset: 0x001C33B8
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

	// Token: 0x06004808 RID: 18440 RVA: 0x001C5220 File Offset: 0x001C3420
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventCompleted(string _gameEventID, int _targetEntityID, string _extraData, string _tag)
	{
		if (this.gameEventID == _gameEventID && _tag == this.gameEventTag && _targetEntityID == base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004809 RID: 18441 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventDenied(string gameEventID, int targetEntityID, string extraData, string tag)
	{
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x001C527C File Offset: 0x001C347C
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

	// Token: 0x0600480B RID: 18443 RVA: 0x001C52B0 File Offset: 0x001C34B0
	public override BaseObjective Clone()
	{
		ObjectiveGameEvent objectiveGameEvent = new ObjectiveGameEvent();
		this.CopyValues(objectiveGameEvent);
		objectiveGameEvent.gameEventID = this.gameEventID;
		objectiveGameEvent.gameEventTag = this.gameEventTag;
		return objectiveGameEvent;
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x001C52E3 File Offset: 0x001C34E3
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveGameEvent.PropGameEventID, ref this.gameEventID);
		properties.ParseString(ObjectiveGameEvent.PropGameEventTag, ref this.gameEventTag);
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x001C530E File Offset: 0x001C350E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("event_id", this.gameEventID);
	}

	// Token: 0x040039BD RID: 14781
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventID = "";

	// Token: 0x040039BE RID: 14782
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventTag = "quest";

	// Token: 0x040039BF RID: 14783
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventID = "event";

	// Token: 0x040039C0 RID: 14784
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventTag = "event_tag";

	// Token: 0x040039C1 RID: 14785
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveGameEvent.GameEventStates GameEventState;

	// Token: 0x020009A9 RID: 2473
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum GameEventStates
	{
		// Token: 0x040039C3 RID: 14787
		Start,
		// Token: 0x040039C4 RID: 14788
		Waiting,
		// Token: 0x040039C5 RID: 14789
		Complete
	}
}
