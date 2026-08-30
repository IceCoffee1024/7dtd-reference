using System;
using UnityEngine.Scripting;

// Token: 0x02000998 RID: 2456
[Preserve]
public class ObjectiveGameEvent : BaseObjective
{
	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x0600482F RID: 18479 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06004830 RID: 18480 RVA: 0x001C3898 File Offset: 0x001C1A98
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x06004831 RID: 18481 RVA: 0x001C38AC File Offset: 0x001C1AAC
	public override void SetupDisplay()
	{
		base.Description = "Test Game Event";
		this.StatusText = "";
	}

	// Token: 0x06004832 RID: 18482 RVA: 0x001C38C4 File Offset: 0x001C1AC4
	public override void AddHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted += this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied += this.Current_GameEventDenied;
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x001C38EE File Offset: 0x001C1AEE
	public override void RemoveHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted -= this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied -= this.Current_GameEventDenied;
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x001C3918 File Offset: 0x001C1B18
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

	// Token: 0x06004835 RID: 18485 RVA: 0x001C3980 File Offset: 0x001C1B80
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventCompleted(string _gameEventID, int _targetEntityID, string _extraData, string _tag)
	{
		if (this.gameEventID == _gameEventID && _tag == this.gameEventTag && _targetEntityID == base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventDenied(string gameEventID, int targetEntityID, string extraData, string tag)
	{
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x001C39DC File Offset: 0x001C1BDC
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

	// Token: 0x06004838 RID: 18488 RVA: 0x001C3A10 File Offset: 0x001C1C10
	public override BaseObjective Clone()
	{
		ObjectiveGameEvent objectiveGameEvent = new ObjectiveGameEvent();
		this.CopyValues(objectiveGameEvent);
		objectiveGameEvent.gameEventID = this.gameEventID;
		objectiveGameEvent.gameEventTag = this.gameEventTag;
		return objectiveGameEvent;
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x001C3A43 File Offset: 0x001C1C43
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveGameEvent.PropGameEventID, ref this.gameEventID);
		properties.ParseString(ObjectiveGameEvent.PropGameEventTag, ref this.gameEventTag);
	}

	// Token: 0x040039A1 RID: 14753
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventID = "";

	// Token: 0x040039A2 RID: 14754
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventTag = "quest";

	// Token: 0x040039A3 RID: 14755
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventID = "event";

	// Token: 0x040039A4 RID: 14756
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventTag = "event_tag";

	// Token: 0x040039A5 RID: 14757
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveGameEvent.GameEventStates GameEventState;

	// Token: 0x02000999 RID: 2457
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum GameEventStates
	{
		// Token: 0x040039A7 RID: 14759
		Start,
		// Token: 0x040039A8 RID: 14760
		Waiting,
		// Token: 0x040039A9 RID: 14761
		Complete
	}
}
