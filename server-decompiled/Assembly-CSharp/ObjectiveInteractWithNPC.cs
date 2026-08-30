using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200099B RID: 2459
[Preserve]
public class ObjectiveInteractWithNPC : BaseObjective
{
	// Token: 0x06004854 RID: 18516 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupObjective()
	{
	}

	// Token: 0x06004855 RID: 18517 RVA: 0x001C4863 File Offset: 0x001C2A63
	public override void SetupDisplay()
	{
		base.Description = Localization.Get("ObjectiveTalkToTrader_keyword", false, null);
		this.StatusText = "";
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x06004856 RID: 18518 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004857 RID: 18519 RVA: 0x001C4884 File Offset: 0x001C2A84
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

	// Token: 0x06004858 RID: 18520 RVA: 0x001C497C File Offset: 0x001C2B7C
	public override void RemoveHooks()
	{
		QuestEventManager.Current.NPCInteract -= this.Current_NPCInteract;
	}

	// Token: 0x06004859 RID: 18521 RVA: 0x001C4994 File Offset: 0x001C2B94
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_NPCInteract(EntityNPC npc)
	{
		if ((!base.OwnerQuest.QuestClass.ReturnToQuestGiver || base.OwnerQuest.QuestGiverID == -1 || base.OwnerQuest.CheckIsQuestGiver(npc.entityId)) && base.OwnerQuest.CheckRequirements())
		{
			if (base.OwnerQuest.QuestFaction == 0)
			{
				base.OwnerQuest.QuestFaction = npc.NPCInfo.QuestFaction;
			}
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x001C4A14 File Offset: 0x001C2C14
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

	// Token: 0x0600485B RID: 18523 RVA: 0x001C4A58 File Offset: 0x001C2C58
	public override BaseObjective Clone()
	{
		ObjectiveInteractWithNPC objectiveInteractWithNPC = new ObjectiveInteractWithNPC();
		this.CopyValues(objectiveInteractWithNPC);
		objectiveInteractWithNPC.useClosest = this.useClosest;
		return objectiveInteractWithNPC;
	}

	// Token: 0x0600485C RID: 18524 RVA: 0x001C4A7F File Offset: 0x001C2C7F
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveInteractWithNPC.PropUseClosest))
		{
			this.useClosest = StringParsers.ParseBool(properties.Values[ObjectiveInteractWithNPC.PropUseClosest], 0, -1, true);
		}
	}

	// Token: 0x040039BC RID: 14780
	public static string PropUseClosest = "use_closest";

	// Token: 0x040039BD RID: 14781
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useClosest;
}
