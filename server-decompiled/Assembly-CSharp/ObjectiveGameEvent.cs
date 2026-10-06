using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200099E RID: 2462
[Preserve]
public class ObjectiveGameEvent : BaseObjective
{
	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x060047DF RID: 18399 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x001C40B3 File Offset: 0x001C22B3
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x001C40C7 File Offset: 0x001C22C7
	public override void SetupDisplay()
	{
		base.Description = "Test Game Event";
		this.StatusText = "";
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x001C40DF File Offset: 0x001C22DF
	public override void AddHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted += this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied += this.Current_GameEventDenied;
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x001C4109 File Offset: 0x001C2309
	public override void RemoveHooks()
	{
		GameEventManager gameEventManager = GameEventManager.Current;
		gameEventManager.GameEventCompleted -= this.Current_GameEventCompleted;
		gameEventManager.GameEventDenied -= this.Current_GameEventDenied;
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x001C4134 File Offset: 0x001C2334
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

	// Token: 0x060047E5 RID: 18405 RVA: 0x001C419C File Offset: 0x001C239C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventCompleted(string _gameEventID, int _targetEntityID, string _extraData, string _tag)
	{
		if (this.gameEventID == _gameEventID && _tag == this.gameEventTag && _targetEntityID == base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_GameEventDenied(string gameEventID, int targetEntityID, string extraData, string tag)
	{
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x001C41F8 File Offset: 0x001C23F8
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

	// Token: 0x060047E8 RID: 18408 RVA: 0x001C422C File Offset: 0x001C242C
	public override BaseObjective Clone()
	{
		ObjectiveGameEvent objectiveGameEvent = new ObjectiveGameEvent();
		this.CopyValues(objectiveGameEvent);
		objectiveGameEvent.gameEventID = this.gameEventID;
		objectiveGameEvent.gameEventTag = this.gameEventTag;
		return objectiveGameEvent;
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x001C425F File Offset: 0x001C245F
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveGameEvent.PropGameEventID, ref this.gameEventID);
		properties.ParseString(ObjectiveGameEvent.PropGameEventTag, ref this.gameEventTag);
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x001C428A File Offset: 0x001C248A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("event_id", this.gameEventID);
	}

	// Token: 0x0400399D RID: 14749
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventID = "";

	// Token: 0x0400399E RID: 14750
	[PublicizedFrom(EAccessModifier.Protected)]
	public string gameEventTag = "quest";

	// Token: 0x0400399F RID: 14751
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventID = "event";

	// Token: 0x040039A0 RID: 14752
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropGameEventTag = "event_tag";

	// Token: 0x040039A1 RID: 14753
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveGameEvent.GameEventStates GameEventState;

	// Token: 0x0200099F RID: 2463
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum GameEventStates
	{
		// Token: 0x040039A3 RID: 14755
		Start,
		// Token: 0x040039A4 RID: 14756
		Waiting,
		// Token: 0x040039A5 RID: 14757
		Complete
	}
}
