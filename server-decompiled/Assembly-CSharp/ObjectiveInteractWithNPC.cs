using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009AB RID: 2475
[Preserve]
public class ObjectiveInteractWithNPC : BaseObjective
{
	// Token: 0x06004829 RID: 18473 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupObjective()
	{
	}

	// Token: 0x0600482A RID: 18474 RVA: 0x001C6173 File Offset: 0x001C4373
	public override void SetupDisplay()
	{
		base.Description = Localization.Get("ObjectiveTalkToTrader_keyword", false, null);
		this.StatusText = "";
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x0600482B RID: 18475 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600482C RID: 18476 RVA: 0x001C6194 File Offset: 0x001C4394
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

	// Token: 0x0600482D RID: 18477 RVA: 0x001C628C File Offset: 0x001C448C
	public override void RemoveHooks()
	{
		QuestEventManager.Current.NPCInteract -= this.Current_NPCInteract;
	}

	// Token: 0x0600482E RID: 18478 RVA: 0x001C62A4 File Offset: 0x001C44A4
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

	// Token: 0x0600482F RID: 18479 RVA: 0x001C6328 File Offset: 0x001C4528
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

	// Token: 0x06004830 RID: 18480 RVA: 0x001C636C File Offset: 0x001C456C
	public override BaseObjective Clone()
	{
		ObjectiveInteractWithNPC objectiveInteractWithNPC = new ObjectiveInteractWithNPC();
		this.CopyValues(objectiveInteractWithNPC);
		objectiveInteractWithNPC.useClosest = this.useClosest;
		return objectiveInteractWithNPC;
	}

	// Token: 0x06004831 RID: 18481 RVA: 0x001C6393 File Offset: 0x001C4593
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveInteractWithNPC.PropUseClosest))
		{
			this.useClosest = StringParsers.ParseBool(properties.Values[ObjectiveInteractWithNPC.PropUseClosest], 0, -1, true);
		}
	}

	// Token: 0x06004832 RID: 18482 RVA: 0x001C63CC File Offset: 0x001C45CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("interacted_npc", this.interactedEntity.npcID);
	}

	// Token: 0x040039D8 RID: 14808
	public static string PropUseClosest = "use_closest";

	// Token: 0x040039D9 RID: 14809
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useClosest;

	// Token: 0x040039DA RID: 14810
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityNPC interactedEntity;
}
