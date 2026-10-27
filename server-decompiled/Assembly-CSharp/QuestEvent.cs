using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200098A RID: 2442
[Preserve]
public class QuestEvent
{
	// Token: 0x060046B5 RID: 18101 RVA: 0x001C078E File Offset: 0x001BE98E
	public QuestEvent(string type)
	{
		this.EventType = type;
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x001C07B4 File Offset: 0x001BE9B4
	public void HandleEvent(Quest quest)
	{
		if (this.IsServerOnly && !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (GameManager.Instance.World.GetGameRandom().RandomFloat < this.Chance)
		{
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].PerformAction(quest);
			}
		}
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x001C081A File Offset: 0x001BEA1A
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.Owner.HandleVariablesForProperties(properties);
		properties.ParseFloat(QuestEvent.PropChance, ref this.Chance);
		properties.ParseBool(QuestEvent.PropServerOnly, ref this.IsServerOnly);
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x001C0854 File Offset: 0x001BEA54
	public QuestEvent Clone()
	{
		QuestEvent questEvent = new QuestEvent(this.EventType);
		questEvent.Chance = this.Chance;
		questEvent.IsServerOnly = this.IsServerOnly;
		if (this.Actions != null)
		{
			for (int i = 0; i < this.Actions.Count; i++)
			{
				BaseQuestAction baseQuestAction = this.Actions[i].Clone();
				baseQuestAction.Properties = new DynamicProperties();
				baseQuestAction.Owner = this.Owner;
				if (this.Actions[i].Properties != null)
				{
					baseQuestAction.Properties.CopyFrom(this.Actions[i].Properties, null);
				}
				questEvent.Actions.Add(baseQuestAction);
			}
		}
		return questEvent;
	}

	// Token: 0x04003912 RID: 14610
	public string EventType;

	// Token: 0x04003913 RID: 14611
	public float Chance = 1f;

	// Token: 0x04003914 RID: 14612
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x04003915 RID: 14613
	public bool IsServerOnly;

	// Token: 0x04003916 RID: 14614
	public static string PropChance = "chance";

	// Token: 0x04003917 RID: 14615
	public static string PropServerOnly = "server_only";

	// Token: 0x04003918 RID: 14616
	public QuestClass Owner;

	// Token: 0x04003919 RID: 14617
	public DynamicProperties Properties;
}
