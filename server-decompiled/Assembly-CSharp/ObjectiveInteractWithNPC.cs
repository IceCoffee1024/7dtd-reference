using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009A1 RID: 2465
[Preserve]
public class ObjectiveInteractWithNPC : BaseObjective
{
	// Token: 0x06004806 RID: 18438 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupObjective()
	{
	}

	// Token: 0x06004807 RID: 18439 RVA: 0x001C50C7 File Offset: 0x001C32C7
	public override void SetupDisplay()
	{
		base.Description = Localization.Get("ObjectiveTalkToTrader_keyword", false, null);
		this.StatusText = "";
	}

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06004808 RID: 18440 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004809 RID: 18441 RVA: 0x001C50E8 File Offset: 0x001C32E8
	public override void AddHooks()
	{
		QuestEventManager.Current.NPCInteract += this.Current_NPCInteract;
		if (this.useClosest)
		{
			List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(null, new Bounds(base.OwnerQuest.Position, new Vector3(50f, 50f, 50f)));
			for (int i = 0; i < entitiesInBounds.Count; i++)
			{
				if (entitiesInBounds[i] is EntityNPC)
				{
					base.OwnerQuest.SetPositionData(Quest.PositionDataTypes.QuestGiver, entitiesInBounds[i].position);
					base.OwnerQuest.QuestGiverID = entitiesInBounds[i].entityId;
					base.OwnerQuest.QuestFaction = ((EntityNPC)entitiesInBounds[i]).NPCInfo.QuestFaction;
					base.OwnerQuest.RallyMarkerActivated = true;
					base.OwnerQuest.HandleMapObject(Quest.PositionDataTypes.QuestGiver, this.NavObjectName, -1);
					return;
				}
			}
		}
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x001C51E0 File Offset: 0x001C33E0
	public override void RemoveHooks()
	{
		QuestEventManager.Current.NPCInteract -= this.Current_NPCInteract;
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x001C51F8 File Offset: 0x001C33F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_NPCInteract(EntityNPC npc)
	{
		if ((!base.OwnerQuest.QuestClass.ReturnToQuestGiver || base.OwnerQuest.QuestGiverID == -1 || base.OwnerQuest.CheckIsQuestGiver(npc.entityId)) && base.OwnerQuest.CheckRequirements())
		{
			if (base.OwnerQuest.QuestFaction == 0)
			{
				base.OwnerQuest.QuestFaction = npc.NPCInfo.QuestFaction;
			}
			this.interactedEntity = npc;
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x001C527C File Offset: 0x001C347C
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, this.PlayObjectiveComplete, null);
		}
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x001C52C0 File Offset: 0x001C34C0
	public override BaseObjective Clone()
	{
		ObjectiveInteractWithNPC objectiveInteractWithNPC = new ObjectiveInteractWithNPC();
		this.CopyValues(objectiveInteractWithNPC);
		objectiveInteractWithNPC.useClosest = this.useClosest;
		return objectiveInteractWithNPC;
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x001C52E7 File Offset: 0x001C34E7
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveInteractWithNPC.PropUseClosest))
		{
			this.useClosest = StringParsers.ParseBool(properties.Values[ObjectiveInteractWithNPC.PropUseClosest], 0, -1, true);
		}
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x001C5320 File Offset: 0x001C3520
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("interacted_npc", this.interactedEntity.npcID);
	}

	// Token: 0x040039B8 RID: 14776
	public static string PropUseClosest = "use_closest";

	// Token: 0x040039B9 RID: 14777
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useClosest;

	// Token: 0x040039BA RID: 14778
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityNPC interactedEntity;
}
