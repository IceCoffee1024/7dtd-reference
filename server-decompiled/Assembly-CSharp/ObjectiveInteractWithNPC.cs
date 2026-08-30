using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200098A RID: 2442
[Preserve]
public class ObjectiveInteractWithNPC : BaseObjective
{
	// Token: 0x060047CE RID: 18382 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupObjective()
	{
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x001C206F File Offset: 0x001C026F
	public override void SetupDisplay()
	{
		base.Description = Localization.Get("ObjectiveTalkToTrader_keyword", false, null);
		this.StatusText = "";
	}

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x060047D0 RID: 18384 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayObjectiveComplete
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x001C2090 File Offset: 0x001C0290
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

	// Token: 0x060047D2 RID: 18386 RVA: 0x001C2188 File Offset: 0x001C0388
	public override void RemoveHooks()
	{
		QuestEventManager.Current.NPCInteract -= this.Current_NPCInteract;
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x001C21A0 File Offset: 0x001C03A0
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

	// Token: 0x060047D4 RID: 18388 RVA: 0x001C2220 File Offset: 0x001C0420
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

	// Token: 0x060047D5 RID: 18389 RVA: 0x001C2264 File Offset: 0x001C0464
	public override BaseObjective Clone()
	{
		ObjectiveInteractWithNPC objectiveInteractWithNPC = new ObjectiveInteractWithNPC();
		this.CopyValues(objectiveInteractWithNPC);
		objectiveInteractWithNPC.useClosest = this.useClosest;
		return objectiveInteractWithNPC;
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x001C228B File Offset: 0x001C048B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveInteractWithNPC.PropUseClosest))
		{
			this.useClosest = StringParsers.ParseBool(properties.Values[ObjectiveInteractWithNPC.PropUseClosest], 0, -1, true);
		}
	}

	// Token: 0x0400392B RID: 14635
	public static string PropUseClosest = "use_closest";

	// Token: 0x0400392C RID: 14636
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useClosest;
}
